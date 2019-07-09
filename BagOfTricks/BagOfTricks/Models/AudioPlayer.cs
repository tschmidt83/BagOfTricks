using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BagOfTricks.Interfaces;
using BagOfTricks.Helpers;
using NAudio.Wave;

namespace BagOfTricks.Models
{
    /// <summary>
    /// Class for audio playback. Can play one music stream and several effects streams in parallel.
    /// </summary>
    public class AudioPlayer : INotifyPropertyChanged, IDisposable
    {
        /***** IsMusicPlaying *****/
        private bool m_IsMusicPlaying = false;

        /// <summary>
        /// Indicates whether the music player is running.
        /// </summary>
        public bool IsMusicPlaying
        {
            get { return m_IsMusicPlaying; }
            private set { m_IsMusicPlaying = value; RaisePropertyChanged("IsMusicPlaying"); }
        }

        /***** LoopBackgroundMusic *****/
        private bool m_LoopBackgroundMusic = true;

        /// <summary>
        ///  Specifies whether the current background music should be looped. True by default.
        /// </summary>
        public bool LoopBackgroundMusic
        {
            get { return m_LoopBackgroundMusic; }
            set { m_LoopBackgroundMusic = value; RaisePropertyChanged("LoopBackgroundMusic"); }
        }

        /***** MusicVolume *****/
        private float m_MusicVolume = 1.0f;

        /// <summary>
        /// Music playback volume
        /// </summary>
        public float MusicVolume
        {
            get { return m_MusicVolume; }
            set { m_MusicVolume = value; RaisePropertyChanged("MusicVolume"); }
        }

        /// <summary>
        /// NAudio base class for reading audio files
        /// </summary>
        private ISampleProvider MusicFileReader;

        // Fx stack, necessary to check effect looping
        private Dictionary<ISampleProvider, CachedEffect> FxStack = new Dictionary<ISampleProvider, CachedEffect>();

        /// <summary>
        /// Necessary for outputting multiple streams at once
        /// </summary>
        private NAudio.Wave.SampleProviders.MixingSampleProvider Mixer;

        /// <summary>
        /// NAudio base class for wave output
        /// </summary>
        private WaveOut OutputDevice = new WaveOut();

        // This flag needs to be set manually in MusicStop() to prevent the Mixer_MixerInputEnded from looping the current track.
        // Advancing in the playlist must be handled in the playlist manager where the MusicFinished event is processed.
        private bool RequestMusicStop = false;

        // This flag needs to be set manually in Dispose() to get the chance to close all streams.
        private bool Shutdown = false;

        /// <summary>
        /// Event which will be raised when the music track is finished and not looped. This allows the playlist class to advance to the next track.
        /// </summary>
        public event EventHandler MusicFinished;

        /// <summary>
        ///  Default constructor
        /// </summary>
        public AudioPlayer()
        {
            Mixer = new NAudio.Wave.SampleProviders.MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
            Mixer.ReadFully = true;

            // TODO: Init once (here) or init each time a play starts when nothing else is running?
            OutputDevice.Init(Mixer);
            OutputDevice.Play();

            Mixer.MixerInputEnded += Mixer_MixerInputEnded;
        }

        /// <summary>
        /// IDisposable member. Close the mixer.
        /// </summary>
        public void Dispose()
        {
            Shutdown = true;
            // TODO: iterate over all mixer inputs and dispose them?
            Mixer.RemoveAllMixerInputs();
            Mixer.ReadFully = false;
            FxStack.Clear();
            OutputDevice.Dispose();
        }

        /// <summary>
        /// Returns the position of the currently playing music stream.
        /// </summary>
        /// <returns>Position in bytes</returns>
        public long MusicGetStreamPositionBytes()
        {
            long pos = 0;

            try
            {
                AudioFileReader reader = MusicFileReader as AudioFileReader;

                if (reader != null)
                {
                    pos = reader.Position;
                }
            }
            catch
            {
                pos = 0;
            }

            return pos;
        }

        /// <summary>
        /// This event gets called when a mixer input stops playing because its stream has ended.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains information about the sample provider that has ended.</param>
        private void Mixer_MixerInputEnded(object sender, NAudio.Wave.SampleProviders.SampleProviderEventArgs e)
        {
            if (Shutdown == false)
            {
                // Check which sample provider has ended
                if (e.SampleProvider == MusicFileReader)
                {
                    IsMusicPlaying = false;

                    if (RequestMusicStop)
                    {
                        RequestMusicStop = false;

                        // Raise event to update GUI
                        if (MusicFinished != null)
                            MusicFinished(this, null);
                    }
                    else
                    {
                        if (LoopBackgroundMusic)
                        {
                            // Loop music: reset stream position
                            MusicReplay();
                        }
                        else
                        {
                            // Raise event to update GUI
                            if (MusicFinished != null)
                                MusicFinished(this, null);
                        }
                    }
                }
                else
                {
                    // Check if it is an effect on the stack
                    if (FxStack.ContainsKey(e.SampleProvider))
                    {
                        // Get the corresponding effect and remove it from the stack
                        CachedEffect fx = FxStack[e.SampleProvider];
                        fx.IsPlaying = false;
                        FxStack.Remove(e.SampleProvider);
                        if (fx.LoopEffect)
                            EffectPlay(fx);
                    }
                }
            }
        }

        /// <summary>
        /// Plays background music.
        /// </summary>
        /// <param name="path">Path to audio file</param>
        /// <param name="loop">Loop audio (default: true)</param>
        /// <param name="volume">Playback volume (default: full)</param>
        /// <returns>True if playback started successfully.</returns>
        public bool MusicPlay(string path, bool loop = true, float volume = 1.0f)
        {
            bool success = false;

            try
            {
                MusicFileReader = new AudioFileReader(path);
                AudioFileReader reader = MusicFileReader as AudioFileReader;
                if (reader != null)
                {
                    reader.Volume = MusicVolume;
                }

                LoopBackgroundMusic = loop;

                Mixer.AddMixerInput(ConvertMonoStereo(MusicFileReader));

                IsMusicPlaying = true;
                success = true;
            }
            catch(Exception ex)
            {
                MusicFileReader = null;
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Re-plays the last music track
        /// </summary>
        /// <returns>True if playback started successfully.</returns>
        private bool MusicReplay()
        {
            bool success = false;

            try
            { 
                AudioFileReader reader = MusicFileReader as AudioFileReader;
                if (reader != null)
                {
                    string path = reader.FileName;
                    float volume = reader.Volume;
                    reader.Position = 0;
                    success = MusicPlay(path, true, volume);
                }
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        ///  Stops the background music.
        /// </summary>
        public void MusicStop()
        {
            if (IsMusicPlaying)
            {
                Mixer.RemoveMixerInput(MusicFileReader);
                AudioFileReader reader = MusicFileReader as AudioFileReader;
                if (reader != null)
                {
                    reader.Dispose();
                }

                IsMusicPlaying = false;
            }
        }

        /// <summary>
        ///  Sets the current stream position for the background music.
        /// </summary>
        /// <param name="percent">Position in percent</param>
        public void MusicSetStreamPosition(double percent)
        {
            AudioFileReader reader = MusicFileReader as AudioFileReader;

            if(reader != null)
            {
                long length = reader.Length;
                long newPos = (long)(length * percent / 100);
                reader.Position = newPos;
            }
        }

        /// <summary>
        /// Sets the current stream position for the background music.
        /// </summary>
        /// <param name="bytes">Position in bytes</param>
        public void MusicSetStreamPositionBytes(long bytes)
        {
            AudioFileReader reader = MusicFileReader as AudioFileReader;

            if (reader != null)
            {
                if(reader.Length > bytes)
                    reader.Position = bytes;
            }
        }

        /// <summary>
        /// Plays a single sound effect.
        /// </summary>
        /// <param name="effect">Cached sound effect</param>
        /// <returns>True if playback started successfully.</returns>
        public bool EffectPlay(CachedEffect effect)
        {
            bool success = false;

            if (effect.IsInitialized && !effect.IsPlaying)
            {
                try
                {
                    effect.Restart();
                    ISampleProvider sp = ConvertMonoStereo(effect);
                    FxStack.Add(sp, effect);
                    Mixer.AddMixerInput(sp);
                    effect.IsPlaying = true;
                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                }
            }

            return success;
        }

        // From https://markheath.net/post/fire-and-forget-audio-playback-with
        /// <summary>
        /// Converts a SampleProvider to mono or stereo, in respect to the playback mixer
        /// </summary>
        /// <param name="input">Input sample provider</param>
        /// <returns>Converted sample provider</returns>
        private ISampleProvider ConvertMonoStereo(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == Mixer.WaveFormat.Channels)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 1 && Mixer.WaveFormat.Channels == 2)
            {
                return new NAudio.Wave.SampleProviders.MonoToStereoSampleProvider(input);
            }
            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        #endregion
    }
}

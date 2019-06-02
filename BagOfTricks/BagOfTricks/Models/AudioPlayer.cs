using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;

namespace BagOfTricks.Models
{
    /// <summary>
    /// Class for audio playback. Can play one music stream and several effects streams in parallel.
    /// </summary>
    public class AudioPlayer : INotifyPropertyChanged
    {
        /***** IsMusicPlaying *****/
        private bool m_IsMusicPlaying;

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

        private AudioFileReader MusicFileReader;
        private WaveOut MusicPlayer = new WaveOut();

        // This flag needs to be set manually in MusicStop() to prevent the MusicPlayer_PlaybackStopped from looping the current track.
        // Advancing in the playlist must be handled in the playlist manager where the MusicFinished event is processed.
        private bool RequestMusicStop = false;

        /// <summary>
        /// Event which will be raised when the music track is finished and not looped. This allows the playlist class to advance to the next track.
        /// </summary>
        public event EventHandler MusicFinished;

        /// <summary>
        ///  Default constructor
        /// </summary>
        public AudioPlayer()
        {
            MusicPlayer.PlaybackStopped += MusicPlayer_PlaybackStopped;
        }

        /// <summary>
        /// This event gets called when the playback stops.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MusicPlayer_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (e.Exception == null)
            {
                if (RequestMusicStop == false)
                {
                    // End of stream
                    if (LoopBackgroundMusic)
                    {
                        // TODO: make looping more efficient. Break is too long. Maybe MusicSetStreamPosition() is not the right way? Maybe there is a property in one of the base classes?
                        MusicSetStreamPosition(0);
                        MusicPlayer.Play();
                        if (MusicPlayer.PlaybackState == PlaybackState.Playing)
                            IsMusicPlaying = true;
                        else
                            IsMusicPlaying = false;
                    }
                    else
                    {
                        IsMusicPlaying = false;

                        // Raise event to update GUI and maybe advance in playlist
                        if (MusicFinished != null)
                            MusicFinished(this, null);
                    }
                }
                else
                {
                    // Manual stop
                    RequestMusicStop = false;
                    IsMusicPlaying = false;

                    // Raise event to update GUI
                    if (MusicFinished != null)
                        MusicFinished(this, null);
                }
            }
        }

        /// <summary>
        /// Plays background music.
        /// </summary>
        /// <param name="path">Path to audio file</param>
        /// <param name="loop">Loop audio (default: true)</param>
        /// <returns>True if playback started successfully.</returns>
        public bool MusicPlay(string path, bool loop = true)
        {
            bool success = false;

            try
            {
                MusicFileReader = new AudioFileReader(path);
                MusicFileReader.Volume = MusicVolume;

                LoopBackgroundMusic = loop;

                MusicPlayer.Init(MusicFileReader);
                MusicPlayer.Play();
                if (MusicPlayer.PlaybackState == PlaybackState.Playing)
                {
                    IsMusicPlaying = true;
                    success = true;
                }
            }
            catch(Exception ex)
            {
                MusicFileReader = null;
                success = false;
            }
            return success;
        }

        /// <summary>
        ///  Stops the background music.
        /// </summary>
        public void MusicStop()
        {
            if (MusicPlayer != null && MusicPlayer.PlaybackState == PlaybackState.Playing)
                MusicPlayer.Stop();
            IsMusicPlaying = false;
        }

        /// <summary>
        ///  Sets the current stream position for the background music.
        /// </summary>
        /// <param name="percent">Position in percent</param>
        public void MusicSetStreamPosition(double percent)
        {
            if(MusicFileReader != null)
            {
                long length = MusicFileReader.Length;
                long newPos = (long)(length * percent / 100);
                MusicFileReader.Position = newPos;
            }
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

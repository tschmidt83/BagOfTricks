using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;

namespace BagOfTricks.Helpers
{
    // Based on https://markheath.net/post/fire-and-forget-audio-playback-with
    public class CachedEffect : ISampleProvider
    {
        /***** Volume *****/
        private float m_Volume;

        public float Volume
        {
            get { return m_Volume; }
            set { m_Volume = value; RaisePropertyChanged("Volume"); }
        }

        /***** LoopEffect *****/
        private bool m_LoopEffect = false;

        public bool LoopEffect
        {
            get { return m_LoopEffect; }
            set { m_LoopEffect = value; RaisePropertyChanged("LoopEffect"); }
        }

        /***** Path *****/
        private string m_Path;

        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; RaisePropertyChanged("Path"); }
        }

        public float[] AudioData { get; private set; }
        private long position;
        public WaveFormat WaveFormat { get; private set; }

        public CachedEffect(string audioFileName)
        {
            Path = audioFileName;
            using (var audioFileReader = new AudioFileReader(audioFileName))
            {
                // TODO: could add resampling in here if required
                WaveFormat = audioFileReader.WaveFormat;
                var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
                var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }
                AudioData = wholeFile.ToArray();
            }
        }

        public void Restart()
        {
            position = 0;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            // TODO: If volume control is needed, the samples must be adjusted here.
            var availableSamples = AudioData.Length - position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(AudioData, position, buffer, offset, samplesToCopy);
            position += samplesToCopy;
            return (int)samplesToCopy;
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

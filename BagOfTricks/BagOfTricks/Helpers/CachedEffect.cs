using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using NAudio.Wave;

namespace BagOfTricks.Helpers
{
    // Based on https://markheath.net/post/fire-and-forget-audio-playback-with
    [Serializable]
    public class CachedEffect : ISampleProvider
    {
        /***** IsPlaying *****/
        private bool m_IsPlaying = false;

        [XmlIgnore]
        public bool IsPlaying
        {
            get { return m_IsPlaying; }
            set { m_IsPlaying = value; RaisePropertyChanged("IsPlaying"); }
        }

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

        /***** Name *****/
        private string m_Name = "---";

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; RaisePropertyChanged("Name"); }
        }

        private bool m_IsInitialized = false;

        [XmlIgnore]
        public bool IsInitialized
        {
            get { return m_IsInitialized; }
            private set { m_IsInitialized = value; }
        }

        public float[] AudioData { get; private set; }
        private long position;
        public WaveFormat WaveFormat { get; private set; }

        public CachedEffect()
        {
        }

        public CachedEffect(string audioFileName)
        {
            Initialize(audioFileName);
        }

        // Initialize the cached effect
        public void Initialize(string audioFileName)
        {
            if (!IsPlaying)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(audioFileName);
                Volume = 1.0f;
                LoopEffect = false;
                Name = fi.Name.Replace(fi.Extension, "");
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

                IsInitialized = true;
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

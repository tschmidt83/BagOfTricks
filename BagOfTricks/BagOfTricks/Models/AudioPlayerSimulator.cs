using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BagOfTricks.Helpers;
using BagOfTricks.Interfaces;

namespace BagOfTricks.Models
{
    public class AudioPlayerSimulator : IAudioPlayer, INotifyPropertyChanged
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

        public event EventHandler MusicFinished;

        public bool EffectPlay(CachedEffect effect)
        {
            throw new NotImplementedException();
        }

        public bool MusicPlay(string path, bool loop = true, float volume = 1)
        {
            throw new NotImplementedException();
        }

        public void MusicSetStreamPosition(double percent)
        {
            throw new NotImplementedException();
        }

        public void MusicStop()
        {
            throw new NotImplementedException();
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

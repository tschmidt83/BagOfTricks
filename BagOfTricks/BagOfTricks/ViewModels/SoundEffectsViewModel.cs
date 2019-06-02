using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;

using BagOfTricks.Interfaces;
using BagOfTricks.Helpers;
using BagOfTricks.Models;

namespace BagOfTricks.ViewModels
{
    public class SoundEffectsViewModel : ViewModelBase
    {
        private const int NUMBER_OF_EFFECTS = 18;
        private readonly IAudioPlayer MyAudioPlayer;

        /***** EditMode *****/
        private bool m_EditMode = false;

        public bool EditMode
        {
            get { return m_EditMode; }
            set { m_EditMode = value; RaisePropertyChanged("EditMode"); }
        }

        /***** EffectsCollection *****/
        private List<CachedEffect> m_EffectsCollection = new List<CachedEffect>(NUMBER_OF_EFFECTS);

        public List<CachedEffect> EffectsCollection
        {
            get { return m_EffectsCollection; }
            set { m_EffectsCollection = value; RaisePropertyChanged("EffectsCollection"); }
        }


        private RelayCommand<CachedEffect> m_PlayEffectCommand;

        #region Command properties
        public RelayCommand<CachedEffect> PlayEffectCommand
        {
            get
            {
                if (m_PlayEffectCommand == null)
                    m_PlayEffectCommand = new RelayCommand<CachedEffect>((p) => PlayEffect(p), (p) => EditMode || p.IsInitialized);
                return m_PlayEffectCommand;
            }
        }
        #endregion

        /// <summary>
        ///  Default (empty) constructor
        /// </summary>
        public SoundEffectsViewModel()
        {
            MyAudioPlayer = new AudioPlayerSimulator();

            for (int i = 0; i < NUMBER_OF_EFFECTS; i++)
                EffectsCollection.Add(new CachedEffect());
        }

        /// <summary>
        /// Constructor with injection
        /// </summary>
        /// <param name="player">Audio player</param>
        [PreferredConstructor]
        public SoundEffectsViewModel(IAudioPlayer player)
        {
            MyAudioPlayer = player;

            for (int i = 0; i < NUMBER_OF_EFFECTS; i++)
                EffectsCollection.Add(new CachedEffect());
        }

        private void EditEffect(CachedEffect p)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Audio-Dateien (MP3, WAV)|*.mp3;*.wav";
            dlg.Title = "Soundeffekt laden";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                if(!p.IsPlaying)
                {
                    p.Initialize(dlg.FileName);
                }
            }
        }

        private void PlayEffect(CachedEffect p)
        {
            if(EditMode)
            {
                EditEffect(p);
            }
            else
            {
                MyAudioPlayer.EffectPlay(p);
            }
        }
    }
}

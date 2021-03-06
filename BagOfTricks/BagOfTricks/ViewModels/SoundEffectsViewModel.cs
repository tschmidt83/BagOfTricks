﻿using System;
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
        private readonly AudioPlayer MyAudioPlayer;

        /***** MyEffectsCollection *****/
        private EffectsCollection m_MyEffectsCollection = new EffectsCollection();

        public EffectsCollection MyEffectsCollection
        {
            get { return m_MyEffectsCollection; }
            set { m_MyEffectsCollection = value; }
        }

        private RelayCommand<CachedEffect> m_PlayEffectCommand;
        private RelayCommand<CachedEffect> m_EditEffectCommand;
        private RelayCommand<CachedEffect> m_ClearEffectCommand;
        private RelayCommand m_SaveEffectListCommand;
        private RelayCommand m_LoadEffectListCommand;

        #region Command properties
        public RelayCommand<CachedEffect> PlayEffectCommand
        {
            get
            {
                if (m_PlayEffectCommand == null)
                    m_PlayEffectCommand = new RelayCommand<CachedEffect>((p) => PlayEffect(p), (p) => p != null && p.IsInitialized);
                return m_PlayEffectCommand;
            }
        }

        public RelayCommand<CachedEffect> EditEffectCommand
        {
            get
            {
                if (m_EditEffectCommand == null)
                    m_EditEffectCommand = new RelayCommand<CachedEffect>((p) => EditEffect(p), (p) => p != null && !p.IsPlaying);
                return m_EditEffectCommand;
            }
        }

        public RelayCommand<CachedEffect> ClearEffectCommand
        {
            get
            {
                if (m_ClearEffectCommand == null)
                    m_ClearEffectCommand = new RelayCommand<CachedEffect>((p) => ClearEffect(p), (p) => p != null && p.IsInitialized && !p.IsPlaying);
                return m_ClearEffectCommand;
            }
        }

        public RelayCommand LoadEffectListCommand
        {
            get
            {
                if (m_LoadEffectListCommand == null)
                    m_LoadEffectListCommand = new RelayCommand(() => LoadEffectList(), () => true);
                return m_LoadEffectListCommand;
            }
        }

        public RelayCommand SaveEffectListCommand
        {
            get
            {
                if (m_SaveEffectListCommand == null)
                    m_SaveEffectListCommand = new RelayCommand(() => SaveEffectList(), () => true);
                return m_SaveEffectListCommand;
            }
        }
        #endregion

        /// <summary>
        ///  Default (empty) constructor
        /// </summary>
        public SoundEffectsViewModel()
        {
            MyAudioPlayer = null;
        }

        /// <summary>
        /// Constructor with injection
        /// </summary>
        /// <param name="player">Audio player</param>
        [PreferredConstructor]
        public SoundEffectsViewModel(AudioPlayer player)
        {
            MyAudioPlayer = player;
        }

        /// <summary>
        /// Clears an effect
        /// </summary>
        /// <param name="p">Effect</param>
        private void ClearEffect(CachedEffect p)
        {
            p.Clear();
        }

        /// <summary>
        /// Edits (loads) an effect
        /// </summary>
        /// <param name="p">Effect</param>
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

        /// <summary>
        /// Plays or stops an effect
        /// </summary>
        /// <param name="p">Effect</param>
        private void PlayEffect(CachedEffect p)
        {
            if (p.IsPlaying)
                MyAudioPlayer.EffectStop(p);
            else
                MyAudioPlayer.EffectPlay(p);
        }

        private void SaveEffectList()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Effekt-Liste (*.boe)|*.boe";
            dlg.Title = "Effektliste speichern";
            bool? result = dlg.ShowDialog();
            if(result == true)
            {
                bool success = false;
                try
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(dlg.FileName, false))
                    {
                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(EffectsCollection));
                        serializer.Serialize(writer, MyEffectsCollection);
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                }

                if (success)
                    System.Windows.MessageBox.Show("Export erfolgreich!");
                else
                    System.Windows.MessageBox.Show("Fehler beim Export!");
            }
        }

        private void LoadEffectList()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Effekt-Liste (*.boe)|*.boe";
            dlg.Title = "Effektliste laden";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                // TODO: stop all running effects with MyAudioPlayer.StopAllEffects()
                bool success = false;
                try
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(dlg.FileName))
                    {
                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(EffectsCollection));
                        MyEffectsCollection = (EffectsCollection)serializer.Deserialize(reader);
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                }

                if (success == false)
                {
                    System.Windows.MessageBox.Show("Fehler beim Laden!");
                }
                else
                {
                    // Initialize all effects
                    foreach (CachedEffect e in MyEffectsCollection.EffectsList)
                        e.Initialize(e.EffectPath);
                }
            }
        }
    }
}

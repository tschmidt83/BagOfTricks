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
using BagOfTricks.Models;

namespace BagOfTricks.ViewModels
{
    public class BackgroundMusicViewModel : ViewModelBase
    {
        private readonly IAudioPlayer MyAudioPlayer;

        private RelayCommand m_LoadPlaylistCommand;
        private RelayCommand m_SavePlaylistCommand;
        private RelayCommand m_AddToPlaylistCommand;
        private RelayCommand m_RemoveFromPlaylistCommand;
        private RelayCommand m_StartPlayCommand;
        private RelayCommand m_StopPlayCommand;

        #region Command Properties

        public RelayCommand LoadPlaylistCommand
        {
            get
            {
                if (m_LoadPlaylistCommand == null)
                    m_LoadPlaylistCommand = new RelayCommand(() => LoadPlaylist(), () => true);
                return m_LoadPlaylistCommand;
            }
        }

        public RelayCommand SavePlaylistCommand
        {
            get
            {
                if (m_SavePlaylistCommand == null)
                    m_SavePlaylistCommand = new RelayCommand(() => SavePlaylist(), () => true);
                return m_SavePlaylistCommand;
            }
        }

        public RelayCommand AddToPlaylistCommand
        {
            get
            {
                if (m_AddToPlaylistCommand == null)
                    m_AddToPlaylistCommand = new RelayCommand(() => AddToPLaylist(), () => true);
                return m_AddToPlaylistCommand;
            }
        }

        public RelayCommand RemoveFromPlaylistCommand
        {
            get
            {
                if (m_RemoveFromPlaylistCommand == null)
                    m_RemoveFromPlaylistCommand = new RelayCommand(() => RemoveFromPlaylist(), () => true);
                return m_RemoveFromPlaylistCommand;
            }
        }

        public RelayCommand StartPlayCommand
        {
            get
            {
                if (m_StartPlayCommand == null)
                    m_StartPlayCommand = new RelayCommand(() => StartPlay(), () => true);
                return m_StartPlayCommand;
            }
        }

        public RelayCommand StopPlayCommand
        {
            get
            {
                if (m_StopPlayCommand == null)
                    m_StopPlayCommand = new RelayCommand(() => StopPlay(), () => MyAudioPlayer.IsMusicPlaying);
                return m_StopPlayCommand;
            }
        }

        #endregion

        /***** CurrentPlaylist *****/
        private Playlist m_CurrentPlaylist = new Playlist();

        public Playlist CurrentPlaylist
        {
            get { return m_CurrentPlaylist; }
            set { m_CurrentPlaylist = value; RaisePropertyChanged("CurrentPlaylist"); }
        }

        /***** SelectedEntry *****/
        private PlaylistEntry m_SelectedEntry = null;

        public PlaylistEntry SelectedEntry
        {
            get { return m_SelectedEntry; }
            set { m_SelectedEntry = value; RaisePropertyChanged("SelectedEntry"); }
        }

        /***** CurrentPlayback *****/
        private PlaylistEntry m_CurrentPlayback = null;

        public PlaylistEntry CurrentPlayback
        {
            get { return m_CurrentPlayback; }
            set { m_CurrentPlayback = value; RaisePropertyChanged("CurrentPlayback"); }
        }

        /// <summary>
        /// Default (empty) constructor
        /// </summary>
        public BackgroundMusicViewModel()
        {
            MyAudioPlayer = new DesignModels.AudioPlayerSimulator();
        }

        /// <summary>
        /// Constructor with injection
        /// </summary>
        /// <param name="player">Audio player</param>
        [PreferredConstructor]
        public BackgroundMusicViewModel(IAudioPlayer player)
        {
            MyAudioPlayer = player;
            MyAudioPlayer.MusicFinished += MyAudioPlayer_MusicFinished;
        }

        /// <summary>
        /// Event handler for the end of a music track
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyAudioPlayer_MusicFinished(object sender, EventArgs e)
        {
            // TODO: Advance in playlist?
        }

        /// <summary>
        /// Loads a playlist from disk
        /// </summary>
        private void LoadPlaylist()
        {
            // TODO: add import possibility for standard playlists (m3u, ...)
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Bag Of Tricks Playlist (*.bop)|*.bot";
            dlg.Title = "Playlist laden";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                MyAudioPlayer.MusicStop();

                if (LoadPlaylistXml(dlg.FileName) == false)
                    System.Windows.MessageBox.Show("Fehler beim Laden!");
                SelectedEntry = null;
                if (CurrentPlaylist.Entries.Count > 0)
                    CurrentPlayback = CurrentPlaylist.Entries[0];
                else
                    CurrentPlayback = null;
            }
        }

        /// <summary>
        /// Saves the current playlist to disk
        /// </summary>
        private void SavePlaylist()
        {
            // TODO: add export possibility for standard playlists (m3u, ...)

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Bag Of Tricks Playlist (*.bop)|*.bot";
            dlg.Title = "Playlist speichern";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                if (SavePlaylistXml(dlg.FileName))
                    System.Windows.MessageBox.Show("Export erfolgreich.");
                else
                    System.Windows.MessageBox.Show("Fehler beim Export!");
            }
        }

        /// <summary>
        /// Add a new file to the playlist
        /// </summary>
        private void AddToPLaylist()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Audio-Dateien (MP3, WAV)|*.mp3;*.wav";
            dlg.Title = "Zur Playlist hinzufügen";
            bool? result = dlg.ShowDialog();
            if(result == true)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(dlg.FileName);
                PlaylistEntry entry = new PlaylistEntry();
                entry.Path = dlg.FileName;
                entry.DisplayName = fi.Name;
                CurrentPlaylist.Add(entry);
            }
        }

        /// <summary>
        /// Remove the currently selected file from the playlist
        /// </summary>
        private void RemoveFromPlaylist()
        {
            if(SelectedEntry != null)
            {
                CurrentPlaylist.Remove(SelectedEntry);

                if(CurrentPlayback == SelectedEntry)
                {
                    // If removing the current playback, advance to next track
                }

                SelectedEntry = null;
            }
        }

        /// <summary>
        /// Save playlist to XML
        /// </summary>
        /// <param name="path">Export path</param>
        /// <returns>True if successful</returns>
        private bool SavePlaylistXml(string path)
        {
            bool success = false;

            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Playlist));
                    serializer.Serialize(writer, CurrentPlaylist);
                    success = true;
                }
            }
            catch(Exception ex)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Load playlist from XML
        /// </summary>
        /// <param name="path">Import path</param>
        /// <returns>True if successful</returns>
        private bool LoadPlaylistXml(string path)
        {
            bool success = false;

            try
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(path))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Playlist));
                    CurrentPlaylist = (Playlist)serializer.Deserialize(reader);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        ///  Stop playback
        /// </summary>
        private void StopPlay()
        {
            if (MyAudioPlayer.IsMusicPlaying)
                MyAudioPlayer.MusicStop();
        }

        /// <summary>
        /// Start playback at the currently selected track
        /// </summary>
        private void StartPlay()
        {
            if (CurrentPlaylist.Entries.Count > 0)
            {
                if (MyAudioPlayer.IsMusicPlaying)
                    MyAudioPlayer.MusicStop();

                if (SelectedEntry != null)
                {
                    MyAudioPlayer.MusicPlay(SelectedEntry.Path);
                    CurrentPlayback = SelectedEntry;
                }
                else
                {
                    MyAudioPlayer.MusicPlay(CurrentPlaylist.Entries[0].Path);
                    CurrentPlayback = CurrentPlaylist.Entries[0];
                }
            }
        }
    }
}

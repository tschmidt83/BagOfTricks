using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;

using BagOfTricks.Models;

namespace BagOfTricks.ViewModels
{
    public class BackgroundMusicViewModel : ViewModelBase
    {
        private RelayCommand m_LoadPlaylistCommand;
        private RelayCommand m_SavePlaylistCommand;
        private RelayCommand m_AddToPlaylistCommand;
        private RelayCommand m_RemoveFromPlaylistCommand;

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

        #endregion

        /***** CurrentPlaylist *****/
        private ObservableCollection<PlaylistEntry> m_CurrentPlaylist = new ObservableCollection<PlaylistEntry>();

        public ObservableCollection<PlaylistEntry> CurrentPlaylist
        {
            get { return m_CurrentPlaylist; }
            set { m_CurrentPlaylist = value; RaisePropertyChanged("CurrentPlaylist"); }
        }

        public BackgroundMusicViewModel()
        {
        }

        private void LoadPlaylist()
        {
        }

        private void SavePlaylist()
        {
        }

        private void AddToPLaylist()
        {
        }

        private void RemoveFromPlaylist()
        {
        }
    }
}

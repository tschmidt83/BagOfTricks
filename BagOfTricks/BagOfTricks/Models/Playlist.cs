using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BagOfTricks.Models
{
    [Serializable]
    public class Playlist : INotifyPropertyChanged
    {
        /***** Name *****/
        private string m_Name = "My Adventure Playlist";

        /// <summary>
        /// Playlist name
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; RaisePropertyChanged("Name"); }
        }

        /***** Entries *****/
        private ObservableCollection<PlaylistEntry> m_Entries = new ObservableCollection<PlaylistEntry>();

        /// <summary>
        /// Playlist entries
        /// </summary>
        public ObservableCollection<PlaylistEntry> Entries
        {
            get { return m_Entries; }
            set { m_Entries = value; RaisePropertyChanged("Entries"); }
        }

        /// <summary>
        /// Default constroctor
        /// </summary>
        public Playlist()
        {
        }
        
        /// <summary>
        /// Adds a new entry to the playlist
        /// </summary>
        /// <param name="entry">Entry to add </param>
        public void Add(PlaylistEntry entry)
        {
            Entries.Add(entry);
        }

        /// <summary>
        /// Removes an entry from the playlist
        /// </summary>
        /// <param name="entry">Entry to remove</param>
        public void Remove(PlaylistEntry entry)
        {
            Entries.Remove(entry);
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

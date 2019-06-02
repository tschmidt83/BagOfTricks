using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BagOfTricks.Models
{
    [Serializable]
    public class PlaylistEntry : INotifyPropertyChanged
    {
        /***** Path *****/
        private string m_Path;

        /// <summary>
        /// Path to the file
        /// </summary>
        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; RaisePropertyChanged("Path"); }
        }

        /***** DisplayName *****/
        private string m_DisplayName;

        /// <summary>
        /// Name to display
        /// </summary>
        public string DisplayName
        {
            get { return m_DisplayName; }
            set { m_DisplayName = value; RaisePropertyChanged("DisplayName"); }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlaylistEntry()
        {
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

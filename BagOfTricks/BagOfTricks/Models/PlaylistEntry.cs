using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOfTricks.Models
{
    public class PlaylistEntry : INotifyPropertyChanged
    {
        /***** Path *****/
        private string m_Path;

        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; RaisePropertyChanged("Path"); }
        }

        /***** DisplayName *****/
        private string m_DisplayName;

        public string DisplayName
        {
            get { return m_DisplayName; }
            set { m_DisplayName = value; RaisePropertyChanged("DisplayName"); }
        }

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

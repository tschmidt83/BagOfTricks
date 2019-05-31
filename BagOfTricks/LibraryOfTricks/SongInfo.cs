using System;
using System.ComponentModel;

namespace LibraryOfTricks
{
    public class SongInfo : INotifyPropertyChanged
    {
        /***** FilePath *****/
        private string m_FilePath;

        public string FilePath
        {
            get { return m_FilePath; }
            set { m_FilePath = value; RaisePropertyChanged("FilePath"); }
        }

        /***** DisplayName *****/
        private string m_DisplayName;

        public string DisplayName
        {
            get { return m_DisplayName; }
            set { m_DisplayName = value; RaisePropertyChanged("DisplayName"); }
        }

        public SongInfo()
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

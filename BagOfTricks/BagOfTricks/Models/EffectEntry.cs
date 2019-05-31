using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOfTricks.Models
{
    public class EffectEntry : INotifyPropertyChanged
    {
        /***** EffectID *****/
        private int m_EffectID = -1;

        public int EffectID
        {
            get { return m_EffectID; }
            set { m_EffectID = value; RaisePropertyChanged("EffectID"); }
        }

        /***** Path *****/
        private string m_Path;

        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; RaisePropertyChanged("Path"); }
        }

        /***** DisplayName *****/
        private int m_DisplayName;

        public int DisplayName
        {
            get { return m_DisplayName; }
            set { m_DisplayName = value; RaisePropertyChanged("DisplayName"); }
        }

        /***** Cached *****/
        private bool m_Cached = false;

        public bool Cached
        {
            get { return m_Cached; }
            set { m_Cached = value; RaisePropertyChanged("Cached"); }
        }

        public EffectEntry()
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

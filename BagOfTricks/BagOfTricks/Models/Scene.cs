using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOfTricks.Models
{
    /// <summary>
    /// A scene is a collection of music, effects, maybe a map, ...
    /// </summary>
    public class Scene : INotifyPropertyChanged
    {
        /***** Name *****/
        private int m_Name;

        /// <summary>
        /// Scene name
        /// </summary>
        public int Name
        {
            get { return m_Name; }
            set { m_Name = value; RaisePropertyChanged("Name"); }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Scene()
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

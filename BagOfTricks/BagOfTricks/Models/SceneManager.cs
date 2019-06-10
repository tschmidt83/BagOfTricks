using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOfTricks.Models
{
    // TODO: Is this redundant with the ADVENTURE class? If yes, prefer adventure!
    /// <summary>
    /// Scene management: load a pre-defined set that can, but needs not, contain a playlist, an effects set, a map, ...
    /// </summary>
    public class SceneManager
    {
        /***** IsEnabled *****/
        private bool m_IsEnabled = false;

        /// <summary>
        /// Indicates if the scene manager is enabled and has control over certain functions which are independent otherwise.
        /// </summary>
        public bool IsEnabled
        {
            get { return m_IsEnabled; }
            set { m_IsEnabled = value; RaisePropertyChanged("IsEnabled"); }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SceneManager()
        {
        }

        /// <summary>
        /// Advances to the previous scene (if possible) in the scene list.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool PreviousScene()
        {
            bool result = false;

            return result;
        }

        /// <summary>
        /// Advances to the next scene (if possible) in the scene list.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool NextScene()
        {
            bool result = false;

            return result;
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOfTricks.Models
{
    /// <summary>
    /// An "adventure" is a collection of scenes in this context
    /// </summary>
    public class Adventure : INotifyPropertyChanged
    {
        /***** AdventureName *****/
        private string m_AdventureName;

        /// <summary>
        /// Name of the adventure
        /// </summary>
        public string AdventureName
        {
            get { return m_AdventureName; }
            set { m_AdventureName = value; RaisePropertyChanged("AdventureName"); }
        }

        /***** Scenes *****/
        private List<Scene> m_Scenes = new List<Scene>();

        /// <summary>
        /// Scenes belonging to the adventure
        /// </summary>
        public List<Scene> Scenes
        {
            get { return m_Scenes; }
            private set { m_Scenes = value; RaisePropertyChanged("Scenes"); }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Adventure()
        {

        }

        /// <summary>
        /// Adds a scene to the adventure. The scene name needs to be unique in the adventure.
        /// </summary>
        /// <param name="s">Scene to add</param>
        /// <returns>True if successful (name was unique)</returns>
        public bool AddScene(Scene s)
        {
            bool success = false;

            return success;
        }

        /// <summary>
        /// Removes a scene from the adventure
        /// </summary>
        /// <param name="s">Scene to delete</param>
        public void RemoveScene(Scene s)
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

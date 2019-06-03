using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using BagOfTricks.Helpers;

namespace BagOfTricks.Models
{
    [Serializable]
    public class EffectsCollection
    {
        private const int NUMBER_OF_EFFECTS = 18;

        [XmlIgnore]
        public int NumberOfEffects
        {
            get { return NUMBER_OF_EFFECTS; }
        }

        /***** Name *****/
        private string m_Name = "My Effect Collection";

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; RaisePropertyChanged("Name"); }
        }

        /***** EffectsList *****/
        private List<Helpers.CachedEffect> m_EffectsList = new List<CachedEffect>(NUMBER_OF_EFFECTS);

        public List<Helpers.CachedEffect> EffectsList
        {
            get { return m_EffectsList; }
            set { m_EffectsList = value; RaisePropertyChanged("EffectsList"); }
        }

        // Default constructor
        public EffectsCollection()
        {
            for (int i = 0; i < NUMBER_OF_EFFECTS; i++)
                EffectsList.Add(new CachedEffect());
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

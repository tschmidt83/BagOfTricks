using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;

namespace BagOfTricks
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register types
            SimpleIoc.Default.Register<BagOfTricks.Interfaces.IAudioPlayer, BagOfTricks.Models.AudioPlayer>(true);
            SimpleIoc.Default.Register<BagOfTricks.Models.SceneManager>(true);

            // Register viewmodels
            SimpleIoc.Default.Register<ViewModels.MainViewModel>();
            SimpleIoc.Default.Register<ViewModels.BackgroundMusicViewModel>();
            SimpleIoc.Default.Register<ViewModels.SoundEffectsViewModel>();
        }

        public ViewModels.MainViewModel Main
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ViewModels.MainViewModel>();
            }
        }

        public ViewModels.BackgroundMusicViewModel BackgroundMusic
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ViewModels.BackgroundMusicViewModel>();
            }
        }

        public ViewModels.SoundEffectsViewModel SoundEffects
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ViewModels.SoundEffectsViewModel>();
            }
        }
    }
}

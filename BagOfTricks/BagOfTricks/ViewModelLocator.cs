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
            //SimpleIoc.Default.Register<BagOfTricks.Interfaces.IAudioPlayer, BagOfTricks.Models.AudioPlayer>(true);
            SimpleIoc.Default.Register<BagOfTricks.Models.AudioPlayer>(true);

            // Register viewmodels
            SimpleIoc.Default.Register<ViewModels.MainViewModel>();
            //SimpleIoc.Default.Register<ViewModels.AdventureManagerViewModel>();
            SimpleIoc.Default.Register<ViewModels.BackgroundMusicViewModel>();
            SimpleIoc.Default.Register<ViewModels.SoundEffectsViewModel>();
            SimpleIoc.Default.Register<ViewModels.DmMapViewModel>();
            SimpleIoc.Default.Register<ViewModels.PlayerMapViewModel>();
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

        public ViewModels.PlayerMapViewModel PlayerMap
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ViewModels.PlayerMapViewModel>();
            }
        }

        public ViewModels.DmMapViewModel DmMap
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ViewModels.DmMapViewModel>();
            }
        }
    }
}

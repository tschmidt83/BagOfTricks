using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;

using BagOfTricks.Interfaces;
using BagOfTricks.Models;

namespace BagOfTricks.ViewModels
{
    /// <summary>
    /// Top view model
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Reference to the audio player class
        /// </summary>
        private readonly IAudioPlayer MyAudioPlayer;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {
            MyAudioPlayer = new DesignModels.AudioPlayerSimulator();
        }

        /// <summary>
        /// Constructor with injection
        /// </summary>
        /// <param name="audio">Audio player</param>
        [PreferredConstructor]
        public MainViewModel(IAudioPlayer audio)
        {
            MyAudioPlayer = audio;
        }
    }
}

using System;
using BagOfTricks.Helpers;

namespace BagOfTricks.Interfaces
{
    public interface IAudioPlayer
    {
        bool IsMusicPlaying { get; }
        bool LoopBackgroundMusic { get; set; }
        float MusicVolume { get; set; }

        event EventHandler MusicFinished;

        bool EffectPlay(CachedEffect effect);
        bool MusicPlay(string path, bool loop = true, float volume = 1);
        void MusicSetStreamPosition(double percent);
        void MusicStop();
    }
}
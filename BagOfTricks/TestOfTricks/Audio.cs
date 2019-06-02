using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BagOfTricks.Models;
using BagOfTricks.Helpers;

namespace TestOfTricks
{
    [TestClass]
    public class Audio
    {
        private AudioPlayer MyAudioPlayer = new AudioPlayer();

        private bool TrackEnded = false;

        private void MyAudioPlayer_MusicFinished(object sender, EventArgs e)
        {
            TrackEnded = true;
        }

        [TestMethod]
        public void BackgroundMusic()
        {
            bool success = false;

            if (MyAudioPlayer.MusicPlay(@"C:\03 Quayside Pub.mp3"))
            {
                System.Threading.Thread.Sleep(5000);
                if (MyAudioPlayer.IsMusicPlaying)
                {
                    success = true;
                    MyAudioPlayer.MusicStop();
                    success = !MyAudioPlayer.IsMusicPlaying;
                }
            }

            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void BackgroundMusicPlaylistAdvance()
        {
            bool success = false;

            string[] playlist = new string[] {
                @"C:\03 Quayside Pub.mp3",
                @"C:\04 The Map.mp3" };

            MyAudioPlayer.MusicFinished += MyAudioPlayer_MusicFinished;

            if (MyAudioPlayer.MusicPlay(playlist[0], false))
            {
                System.Threading.Thread.Sleep(5000);
                MyAudioPlayer.MusicSetStreamPosition(99.0);
                System.Threading.Thread.Sleep(5000);
                if (TrackEnded)
                {
                    TrackEnded = false;
                    if (MyAudioPlayer.MusicPlay(playlist[1], false))
                    {
                        System.Threading.Thread.Sleep(5000);
                        MyAudioPlayer.MusicStop();
                        success = !MyAudioPlayer.IsMusicPlaying;
                    }
                }
            }

            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void BackgroundMusicLoop()
        {
            bool success = false;

            if (MyAudioPlayer.MusicPlay(@"C:\03 Quayside Pub.mp3"))
            {
                System.Threading.Thread.Sleep(5000);
                if (MyAudioPlayer.IsMusicPlaying)
                {
                    MyAudioPlayer.MusicSetStreamPosition(99.0);
                    System.Threading.Thread.Sleep(5000);
                    if (MyAudioPlayer.IsMusicPlaying)
                    {
                        success = true;
                        MyAudioPlayer.MusicStop();
                        success = !MyAudioPlayer.IsMusicPlaying;
                    }
                }
            }

            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void SingleEffectPlay()
        {
            bool success = false;

            // Prepare cached effects
            CachedEffect fx = new CachedEffect(@"C:\Chirp.mp3");

            if (MyAudioPlayer.EffectPlay(fx))
            {
                System.Threading.Thread.Sleep(15000);
                success = true;
            }

            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void MultipleEffectsPlay()
        {
            bool success = false;

            // Prepare cached effects
            List<CachedEffect> fxList = new List<CachedEffect>();
            for(int i = 0; i < 3; i++)
                fxList.Add(new CachedEffect(@"C:\Chirp.mp3"));

            for(int i = 0; i < fxList.Count; i++)
            {
                success = MyAudioPlayer.EffectPlay(fxList[i]);
                if(success)
                    System.Threading.Thread.Sleep(1000);
                else
                    break;
            }

            if(success)
                System.Threading.Thread.Sleep(5000);

            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void MultipleEffectsWithMusic()
        {
            bool success = false;

            // Prepare cached effects
            List<CachedEffect> fxList = new List<CachedEffect>();
            for (int i = 0; i < 3; i++)
                fxList.Add(new CachedEffect(@"C:\Chirp.mp3"));

            // Start music first
            if (MyAudioPlayer.MusicPlay(@"C:\03 Quayside Pub.mp3"))
            {
                System.Threading.Thread.Sleep(5000);
                if (MyAudioPlayer.IsMusicPlaying)
                {
                    // Add effects
                    for (int i = 0; i < fxList.Count; i++)
                    {
                        success = MyAudioPlayer.EffectPlay(fxList[i]);
                        if (success)
                            System.Threading.Thread.Sleep(2000);
                        else
                            break;
                    }

                    MyAudioPlayer.MusicStop();
                    success = !MyAudioPlayer.IsMusicPlaying;
                }
            }

            if (success)
                System.Threading.Thread.Sleep(5000);

            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void SingleEffectLoop()
        {
            bool success = false;

            // Prepare cached effects
            CachedEffect fx = new CachedEffect(@"C:\Chirp.mp3");
            fx.LoopEffect = true;

            if (MyAudioPlayer.EffectPlay(fx))
            {
                System.Threading.Thread.Sleep(15000);
                success = true;
            }

            Assert.AreEqual(true, success);
        }
    }
}

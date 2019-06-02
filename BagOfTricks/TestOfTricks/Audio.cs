using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BagOfTricks.Models;

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
    }
}

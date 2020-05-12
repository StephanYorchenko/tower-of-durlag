using System;
using System.IO;
using System.Media;
using System.Windows.Forms;
using LabirintDemoGame.Controllers;
using LabirintDemoGame.Visualization;

namespace LabirintDemoGame
{
    internal static class Program
    {
        public static void Main()
        {
            SoundPlayer simpleSound = new SoundPlayer(@"Sounds\Sound1.wav");
            simpleSound.PlayLooping();
            var a = Game.CreateFromConfig("5%7;13%9;17%25;17%17;19%37;27%49;65%65;99%99");
            Application.Run(new LabyrinthWindow(a));
        }
    }
}
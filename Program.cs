﻿using System.Windows.Forms;
using LabirintDemoGame.Controllers;
using LabirintDemoGame.Visualization;

namespace LabirintDemoGame
{
    internal static class Program
    {
        public static void Main()
        {
            //var a = Game.CreateFromConfig("5%7;13%9;17%25;17%17;19%37;27%49;65%65;99%99");
            // var a = new Game(5, 7);
            Application.Run(new LabyrinthWindow());
        }
    }
}
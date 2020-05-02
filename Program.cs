using System;
using System.Windows.Forms;

namespace LabirintDemoGame
{
    internal static class Program
    {
        public static void Main()
        {
            var a = Game.CreateFromConfig("5%7");
            Console.WriteLine(a.Level.Map);
            Application.Run(new LabyrinthWindow(a.Level.Map));
        }
    }
}
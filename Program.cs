using System;
using System.Windows.Forms;

namespace LabirintDemoGame
{
    internal static class Program
    {
        public static void Main()
        {
            var a = new MapController(13, 9);
            Console.WriteLine(a);
            Application.Run(new LabyrinthWindow(a));
        }
    }
}
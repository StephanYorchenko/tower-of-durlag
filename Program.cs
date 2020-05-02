using System;
using System.Windows.Forms;

namespace LabirintDemoGame
{
    internal static class Program
    {
        public static void Main()
        {
            var a = new Game(13, 9);
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
                //TODO: console player
            } while (!a.Player.Equals(a.EndPoint) && key.Key != ConsoleKey.Escape);
        }
    }
}
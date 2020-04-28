using System;

namespace LabirintDemoGame
{
    internal static class Program
    {
        public static void Main()
        {
            var a = new Labyrinth(13, 9);
            a.GenerateLabyrinth();
            Console.WriteLine(a);
        }
    }
}
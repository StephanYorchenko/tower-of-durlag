using System;

namespace LabirintDemoGame
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var a = new Labyrinth(9, 9);
            a.GenerateLabyrinth();
            Console.WriteLine(a);
        }
    }
}
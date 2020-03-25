using System;

namespace LabirintDemoGame
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var a = new Labyrinth(7, 7);
            Console.WriteLine(a);
            a.GenerateLabyrinth();
            Console.WriteLine(a);
        }
    }
}
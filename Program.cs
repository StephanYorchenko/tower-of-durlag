using System;

namespace LabirintDemoGame
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var a = new Labyrinth(5, 19);
            a.GenerateLabyrinth();
            Console.WriteLine(a);
        }
    }
}
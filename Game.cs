using System;
using System.Collections.Generic;
using System.Linq;

namespace LabirintDemoGame
{
    public class Game
    {
        public Labyrinth Maze;
        public int MazeWidth => Maze.Width;
        public int MazeHeight => Maze.Height;
        
        public Cell InitialPoint => Maze.InitialPoint;
        public Cell EndPoint => Maze.EndPoint;
        
        public Cell Player;
        
        public Game(int width, int height)
        {
            Maze = new Labyrinth(height, width);
            Maze.GenerateLabyrinth();
            Player = Maze.InitialPoint;    
        }

        public override string ToString()
        {
            var maze = new List<string[]>();
            for (int i = 0; i < MazeHeight; i++)
            {
                maze.Add(new string[MazeWidth]);
                for (int j = 0; j < MazeWidth; j++)
                    maze[i][j] = "# ";
            }

            foreach (var cell in Maze.Maze)
                maze[cell.X][cell.Y] = cell.Equals(InitialPoint)
                    ? "S "  
                    : cell.Equals(EndPoint) ? "E " : "  ";
            return string.Join("\n", maze.Select(x => string.Join("", x)).ToArray());
        }
    }    
}
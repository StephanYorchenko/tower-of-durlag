using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabirintDemoGame
{
    public class Game
    {
        private readonly Labyrinth labyrinth;
        public int MazeWidth => labyrinth.Width;
        public int MazeHeight => labyrinth.Height;
        
        public Cell InitialPoint => labyrinth.InitialPoint;
        public Cell EndPoint => labyrinth.EndPoint;
        
        public Cell Player;

        public Cell[,] Maze;
        
        public Game(int width, int height)
        {
            labyrinth = new Labyrinth(height, width);
            labyrinth.GenerateLabyrinth();
            Player = new Cell(InitialPoint.X, InitialPoint.Y, CellTypes.Player);
            Maze = labyrinth.ToArray();
            Maze[Player.X, Player.Y] = Player;
        }

        public override string ToString()
        {
            var stringMaze = new StringBuilder();
            for (var i = 0; i < MazeHeight; i++)
                stringMaze.Append(string.Join("",
                    Enumerable.Range(0 ,MazeWidth)
                        .Select(x => Maze[i, x].ToString())) + "\n");
            return stringMaze.ToString();
        }
    }    
}
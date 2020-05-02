using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabirintDemoGame
{
    public class MapController
    {
        private readonly LabyrinthGenerator labyrinthGenerator;
        public int MazeWidth => labyrinthGenerator.Width;
        public int MazeHeight => labyrinthGenerator.Height;
        
        public Cell InitialPoint => labyrinthGenerator.InitialPoint;
        public Cell EndPoint => labyrinthGenerator.EndPoint;
        
        public Cell PlayerPosition;

        public Cell[,] Maze { get; }
        
        public MapController(int width, int height)
        {
            labyrinthGenerator = new LabyrinthGenerator(height, width);
            labyrinthGenerator.GenerateLabyrinth();
            PlayerPosition = new Cell(InitialPoint.X, InitialPoint.Y, CellTypes.Player);
            Maze = labyrinthGenerator.ToArray();
        }

        private bool IsMovingCorrect(Direction direction)
        {
            var x = direction.X + PlayerPosition.X;
            var y = direction.Y + PlayerPosition.Y;
            return x >= 0 && y >= 0 && x < MazeWidth && y < MazeHeight && Maze[x, y].Type != CellTypes.Wall;
        }

        public void MakePlayerMove(Direction direction)
        {
            if (IsMovingCorrect(direction))
                PlayerPosition = new Cell(direction.X + PlayerPosition.X,
                     direction.Y + PlayerPosition.Y, CellTypes.Player);
        }

        public override string ToString()
        {
            var stringMaze = new StringBuilder();
            for (var i = 0; i < MazeHeight; i++)
                stringMaze.Append(string.Join("",
                    Enumerable.Range(0, MazeWidth)
                        .Select(x => Maze[i, x].ToString())) + "\n");
            return stringMaze.ToString();
        }
    }    
}
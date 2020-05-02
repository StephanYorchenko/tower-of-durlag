using System;
using System.Collections.Generic;
using System.Linq;

namespace LabirintDemoGame
{
    public class Labyrinth
    {
        public int Height { get; }
        public int Width { get; }
        public Cell InitialPoint { get; set; }
        public Cell EndPoint { get; set; }
        public HashSet<Cell> Maze { get; }
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public HashSet<Cell> UnvisitedCells;
        private Stack<Cell> VisitingOrder { get; }

        private static readonly List<Tuple<int, int>> NeighboursCoordinated = new List<Tuple<int, int>>
        {
            new Tuple<int, int>(0 , -2),
            new Tuple<int, int>(2, 0),
            new Tuple<int, int>(-2, 0),
            new Tuple<int, int>(0, 2)
        };
        
        public Labyrinth(int height, int width)
        {
            Height = height;
            Width = width;
            Maze = new HashSet<Cell>();
            UnvisitedCells = new HashSet<Cell>();
            VisitingOrder = new Stack<Cell>((width - 1) * (height - 1));
            InitialPoint = new Cell();
            EndPoint = new Cell();
            StartGenerate();
        }

        private void StartGenerate()
        {
            for (var i = 0; i < Height; i++)
            for (var j = 0; j < Width; j++)
                if (i % 2 != 0 && j % 2 != 0 && i != Height - 1 && j != Width - 1)
                    UnvisitedCells.Add(new Cell(i, j, CellTypes.Empty));
        }

        public void GenerateLabyrinth()
        {
            var random = new Random();
            var currentCell = UnvisitedCells.ElementAt(random.Next(0, (Width / 2) * (Height / 2)));
            InitialPoint = currentCell;
            Maze.Add(currentCell);
            UnvisitedCells.Remove(currentCell);
            do
            {
                var currentNeighbours = GetUnvisitedNeighbours(currentCell);
                if (currentNeighbours.Any() && Maze.Contains(currentCell))
                {
                    var a = random.Next(0, currentNeighbours.Length);
                    var nextCell = currentNeighbours[a];
                    var middleCell = new Cell(
                        Math.Abs(currentCell.X + nextCell.X) / 2,
                        Math.Abs(currentCell.Y + nextCell.Y) / 2,
                        CellTypes.Empty);
                    Maze.Add(middleCell);
                    VisitingOrder.Push(currentCell);
                    currentCell = nextCell;
                    Maze.Add(currentCell);
                    UnvisitedCells.Remove(currentCell);
                }
                else
                {
                    if (EndPoint.Equals(new Cell()))
                        EndPoint = currentCell;
                    currentCell = VisitingOrder.Pop();
                }
            } while (UnvisitedCells.Count != 0 || VisitingOrder.Count != 0);
        }

        public Cell[] GetUnvisitedNeighbours(Cell currentCell)
        {
            return NeighboursCoordinated.Select(x =>
                                                 new Cell(currentCell.X + x.Item1,
                                                          currentCell.Y + x.Item2,
                                                          CellTypes.Empty))
                                        .Intersect(UnvisitedCells)
                                        .ToArray();
        }

        public override string ToString()
        {
            var maze = new List<string[]>();
            for (int i = 0; i < Height; i++)
            {
                maze.Add(new string[Width]);
                for (int j = 0; j < Width; j++)
                    maze[i][j] = "# ";
            }

            foreach (var cell in Maze)
                maze[cell.X][cell.Y] = !cell.Equals(InitialPoint) ? ". "  : "S ";
            return string.Join("\n", maze.Select(x => string.Join("", x)).ToArray());
        }
    }
}

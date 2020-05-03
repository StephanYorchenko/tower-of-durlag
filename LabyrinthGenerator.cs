using System;
using System.Collections.Generic;
using System.Linq;

namespace LabirintDemoGame
{
    public class LabyrinthGenerator
    {
        public int Width { get; }
        public int Height { get; }
        public Cell InitialPoint { get; private set; }
        public Cell EndPoint { get; private set; }
        private HashSet<Cell> Maze { get; }
        
        public readonly HashSet<Cell> UnvisitedCells;
        private Stack<Cell> VisitingOrder { get; }

        private static readonly List<Tuple<int, int>> NeighboursCoordinated = new List<Tuple<int, int>>
        {
            new Tuple<int, int>(0 , -2),
            new Tuple<int, int>(2, 0),
            new Tuple<int, int>(-2, 0),
            new Tuple<int, int>(0, 2)
        };
        
        public LabyrinthGenerator(int width, int height)
        {
            Width = width;
            Height = height;
            Maze = new HashSet<Cell>();
            UnvisitedCells = new HashSet<Cell>();
            VisitingOrder = new Stack<Cell>((height - 1) * (width - 1));
            InitialPoint = new Cell();
            EndPoint = new Cell();
            StartGenerate();
        }

        private void StartGenerate()
        {
            for (var i = 0; i < Width; i++)
            for (var j = 0; j < Height; j++)
                if (i % 2 != 0 && j % 2 != 0 && i != Width - 1 && j != Height - 1)
                    UnvisitedCells.Add(new Cell(i, j, CellTypes.Empty));
        }

        public void GenerateLabyrinth()
        {
            var random = new Random();
            var currentCell = UnvisitedCells.ElementAt(random.Next(0, (Height / 2) * (Width / 2)));
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
            for (var i = 0; i < Width; i++)
            {
                maze.Add(new string[Height]);
                for (var j = 0; j < Height; j++)
                    maze[i][j] = "# ";
            }

            foreach (var cell in Maze)
                maze[cell.X][cell.Y] = !cell.Equals(InitialPoint) ? ". "  : "S ";
            return string.Join("\n", maze.Select(x => string.Join("", x)).ToArray());
        }

        public Cell[,] ToArray()
        {
            var maze = new Cell[Width, Height];
            for (var i = 0; i < Width; i++)
            for (var j = 0; j < Height; j++)
                    maze[i, j] = new Cell(i, j, CellTypes.Wall);

            foreach (var cell in Maze)
                maze[cell.X, cell.Y] = cell.Equals(InitialPoint)
                    ? new Cell(cell.X, cell.Y, CellTypes.Start)  
                    : cell.Equals(EndPoint) 
                        ? new Cell(cell.X, cell.Y, CellTypes.End) 
                        : new Cell(cell.X, cell.Y, CellTypes.Empty);
            return maze;
        }
    }
}

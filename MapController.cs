using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabirintDemoGame
{
    public class MapController
    {
        private readonly LabyrinthGenerator labyrinthGenerator;
        public Cell PlayerPosition;
        public bool IsEndReached;
        public Cell[,] Maze { get; }
        public List<Cell> VisibleMaze { get; set; }
        public int MazeWidth => labyrinthGenerator.Width;
        public int MazeHeight => labyrinthGenerator.Height;
        
        public Cell InitialPoint => labyrinthGenerator.InitialPoint;
        public Cell EndPoint => labyrinthGenerator.EndPoint;

        public MapController(int width, int height)
        {
            labyrinthGenerator = new LabyrinthGenerator(width, height);
            labyrinthGenerator.GenerateLabyrinth();
            PlayerPosition = new Cell(InitialPoint.X, InitialPoint.Y, CellTypes.Player);
            Maze = labyrinthGenerator.ToArray();
            IsEndReached = false;
            ExploreCells();
            UpdateVisibleCells();
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
                PlayerPosition = new Cell(
                    direction.X + PlayerPosition.X,
                    direction.Y + PlayerPosition.Y,
                    CellTypes.Player);
            ExploreCells();
            UpdateVisibleCells();
            if (PlayerPosition.Equals(EndPoint))
                IsEndReached = true;
        }

        public override string ToString()
        {
            var stringMaze = new StringBuilder();
            for (var i = 0; i < MazeWidth; i++)
                stringMaze.Append(string.Join("",
                    Enumerable.Range(0, MazeHeight)
                        .Select(x => Maze[i, x].ToString())) + "\n");
            return stringMaze.ToString();
        }

        private void ExploreCells()
        {
            for (var i = -1; i <= 1; i++)
            for (var j = -1; j <= 1; j++)
                Maze[PlayerPosition.X + i, PlayerPosition.Y + j].IsExplored = true;
        }

        private void UpdateVisibleCells()
        {
            VisibleMaze = new List<Cell>();
            foreach (var cell in Maze)
                cell.SetVisiblity(cell.X >= PlayerPosition.X - 4 && cell.X <= PlayerPosition.X + 4 &&
                                  cell.Y >= PlayerPosition.Y - 4 && cell.Y <= PlayerPosition.X + 4);
            foreach (var cell in Maze)
                if (cell.IsVisible)
                    VisibleMaze.Add(cell);
        }
    }    
}
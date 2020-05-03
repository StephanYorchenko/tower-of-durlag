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
    }    
}
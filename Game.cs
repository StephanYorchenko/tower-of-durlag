using System.Text;

namespace LabirintDemoGame
{
    public class Game
    {
        public Player Player;
        public Level Level;

        public Game(int width, int height)
        {
            //TODO: add plot text and subjects parameters
            var map = new MapController(width, height);
            Level = new Level(map);
            Player = new Player();
        }

        public Game(Level level, Player player)
        {
            Level = level;
            Player = player;
        }

        public override string ToString()
        {
            var log = new StringBuilder();
            log.Append(Player + "\n");
            log.Append(Level.Map + "\n");
            return log.ToString();
        }

        public static Game CreateFromConfig(string text)
        {
            var lvl = Level.CreateFromConfig(text);
            return new Game(lvl, new Player());
        }

        public void MakePlayerMove(Directions direction)
        {
            Level.Map.MakePlayerMove(Player.Move(direction));
        }

        public Cell[,] Map => Level.Map.Maze;
        public int MazeWidth => Level.Map.MazeWidth;
        public int MazeHeight => Level.Map.MazeHeight;
        public Cell PlayerPosition => Level.Map.PlayerPosition;
        public Cell InitialPoint => Level.Map.InitialPoint;
        public Cell EndPoint => Level.Map.EndPoint;
    }
}
using System.Text;

namespace LabirintDemoGame
{
    public class Game
    {
        public MapController Map;
        public Player Player;
        public int Level;

        public Game()
        {
            Level = 1;
            Map = new MapController(3, 5);
            Player = new Player();
        }

        public override string ToString()
        {
            var log = new StringBuilder();
            log.Append(Player + "\n");
            log.Append(Map + "\n");
            log.Append(Level);
            return log.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using LabirintDemoGame.Architecture;

namespace LabirintDemoGame.Controllers
{
    public class Game
    {
        public Player Player;
        public PlotController Plot;
        public Level Level;
        private Queue<string> logLevels;

        public bool IsPlotStep { get; private set; }

        public Game(int width, int height)
        {
            //TODO: add plot text and subjects parameters
            var map = new MapController(width, height);
            var plot = new PlotController();
            Level = new Level(map, plot);
            Player = new Player();
            IsPlotStep = false;
        }

        public Game(Level level, Player player, Queue<string> queue = null)
        {
            Level = level;
            Player = player;
            logLevels = queue;
        }

        public override string ToString()
        {
            var log = new StringBuilder();
            log.Append(Player + "\n");
            log.Append(Level.Map);
            return log.ToString();
        }

        public static Game CreateFromConfig(string text)
        {
            var queue = new Queue<string>(text.Split(';'));
            var lvl = Level.CreateFromConfig(queue.Dequeue());
            return new Game(lvl, new Player(), queue);
        }

        public void UpdateToConfig(string text)
        {
            Level = Level.CreateFromConfig(text);
        }
        
        public void MakePlayerMove(Directions direction)
        {
            Level.Map.MakePlayerMove(Player.Move(direction));
            if (Level.Map.Gold) Player.ApplyChanges(new Option {Gold = 1});
            if (Level.Map.IsEndReached && logLevels.Count > 0)
                GetNextLevel();
            else if (Level.Map.IsEndReached)
                EndGame = true;
            else
                IsPlotStep = true;
        }

        public void GetNextLevel()
        {
            UpdateToConfig(logLevels.Dequeue());
        }

        public bool EndGame { get; private set; }

        public List<Cell> Map => Level.Map.VisibleMaze;
        public int MazeWidth => Level.Map.MazeWidth;
        public int MazeHeight => Level.Map.MazeHeight;
        public Cell PlayerPosition => Level.Map.PlayerPosition;
        public Cell InitialPoint => Level.Map.InitialPoint;
        public Cell EndPoint => Level.Map.EndPoint;
    }
}
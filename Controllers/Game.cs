using System;
using System.Collections.Generic;
using System.Text;
using LabirintDemoGame.Architecture;
using LabirintDemoGame.Generators;

namespace LabirintDemoGame.Controllers
{
    public class Game
    {
        private bool Config;
        public Player Player;
        public PlotController Plot;
        public Level Level;
        private Queue<string> logLevels;

        private MapSizeGenerator mapSizeGenerator;
        public Step StepType;

        public Game(int width, int height, bool config = false)
        {
            Config = config;
            mapSizeGenerator = new MapSizeGenerator(height, width);
            var map = mapSizeGenerator.NextController();
            var plot = new PlotController();
            Level = new Level(map, plot);
            Player = new Player();
            StepType = Step.Maze;
        }

        public Game(Level level, Player player, Queue<string> queue = null, bool config = false)
        {
            Config = config;
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
            return new Game(lvl, new Player(), queue, true);
        }

        public void UpdateToConfig(string text)
        {
            Level = Level.CreateFromConfig(text);
        }
        
        public void MakePlayerMove(Directions direction)
        {
            if (StepType != Step.Maze) return;
            Level.Map.MakePlayerMove(Player.Move(direction));
            if (Level.Map.Gold) Player.ApplyChanges(new Option {Gold = 1});
            if (Level.Map.IsEndReached && logLevels.Count > 0)
                GetNextLevel();
            else if (Level.Map.IsEndReached)
                StepType = Step.Plot;
        }

        public void StartPlotAct(Option startOption)
        {
            Player.ApplyChanges(startOption);
            if (Player.IsDead())
                EndGame = true;
        }
        public void MakePlotAction(int index)
        {
            Player.ApplyChanges(Plot.CurrentOptions[index]);
            if (Player.IsDead())
                EndGame = true;
        }

        public void GetNextLevel()
        {
            if (Config)
                UpdateToConfig(logLevels.Dequeue());
            else
            {
                Plot.SetNextAct();
                Level = new Level(mapSizeGenerator.NextController(), Plot);
            }
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
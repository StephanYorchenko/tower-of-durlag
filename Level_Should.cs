using System.Collections.Generic;
using NUnit.Framework;

namespace LabirintDemoGame
{
    [TestFixture]
    public class LevelShould
    {
        [Test]
        public void InitializationShould()
        {
            var map = new MapController(7, 9);
            var plot = new PlotAct("Hello world", new List<PlotSubject>());
            var level = new Level(map, plot);
            Assert.AreEqual(7, level.Map.MazeWidth);
            Assert.AreEqual(9, level.Map.MazeHeight);
        }

        [TestCase("5%9", 5, 9)]
        [TestCase("13%17", 13, 17)]
        public void CreateFromConfig(string config, int height, int width)
        {
            var level = Level.CreateFromConfig(config);
            Assert.AreEqual(height, level.Map.MazeHeight);
            Assert.AreEqual(width, level.Map.MazeWidth);
        }
        
    }
}
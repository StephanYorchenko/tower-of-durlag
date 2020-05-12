using LabirintDemoGame.Controllers;
using NUnit.Framework;

namespace LabirintDemoGame.Tests
{
    [TestFixture]
    public class LevelShould
    {
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
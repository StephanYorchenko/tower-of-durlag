using System.Collections.Generic;
using NUnit.Framework;

namespace LabirintDemoGame
{
    [TestFixture]
    public class Game_Should
    {
        [Test]
        public void IsSizeInitializationCorrect()
        {
            var gaming = new Game(7, 8);
            Assert.AreEqual(7, gaming.MazeWidth);
            Assert.AreEqual(8, gaming.MazeHeight);
        }
        
        [Test]
        public void IsLevelInitializationCorrect()
        {
            var gaming = new Game(Level.CreateFromConfig("5%7"), new Player());
            Assert.AreEqual(7, gaming.MazeWidth);
            Assert.AreEqual(5, gaming.MazeHeight);
        }
        
        [TestCase("3%5", 7)]
        [TestCase("5%3", 5)]
        public void IsToStringCorrect(string config, int expected)
        {
            var game = Game.CreateFromConfig(config);
            var actual = game.ToString().Split('\n').Length;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateFromConfigShould()
        {
            var gaming = Game.CreateFromConfig("5%5;7%9");
            Assert.AreEqual(5, gaming.MazeHeight);
            Assert.AreEqual(5, gaming.MazeWidth);
            gaming.GetNextLevel();
            Assert.AreEqual(9, gaming.MazeWidth);
            Assert.AreEqual(7, gaming.MazeHeight);
        }

        [TestCase("5%5", "7%9", 7, 9)]
        [TestCase("5%9", "19%13", 19, 13)]
        public void UpdateToConfigShould(string currentConf, string nextConf, int height, int width)
        {
            var game = Game.CreateFromConfig(currentConf);
            game.UpdateToConfig(nextConf);
            Assert.AreEqual(height, game.MazeHeight);
            Assert.AreEqual(width, game.MazeWidth);
        }
    }
}
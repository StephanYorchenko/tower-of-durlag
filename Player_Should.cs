using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LabirintDemoGame
{
    [TestFixture]
    public class Player_Should
    {

        [Test]
        public void IsInitializationCorrect()
        {
            var player = new Player();
            Assert.AreEqual(100, player.Health);
            Assert.AreEqual(0, player.Bag.Count);
            player = new Player(13, new []{new PlotSubject("some item")});
            Assert.AreEqual(13, player.Health);
        }

        [Test]
        public void PlayerChangeHpShould()
        {
            var player = new Player();
            Assert.AreEqual(100, player.Health);
            player.ChangeHp(10);
            Assert.AreEqual(90, player.Health);
        }

        [TestCase(Directions.Up, 0, -1)]
        [TestCase(Directions.Down, 0, 1)]
        [TestCase(Directions.Left, -1, 0)]
        [TestCase(Directions.Right, 1, 0)]
        public void MoveShould(Directions dir, int dirX, int dirY)
        {
            var actual = Player.Move(dir);
            Assert.AreEqual(dirX, actual.X);
            Assert.AreEqual(dirY, actual.Y);
        }

        [Test]
        public void ToStringShould()
        {
            var player = new Player(13, new List<PlotSubject>());
            Assert.AreEqual("13 -- <>", player.ToString());
            player.ChangeHp(-50);
            Assert.AreEqual("63 -- <>", player.ToString());
            player = new Player(72,
                new List<string>{"sword", "shield"}.Select(x => new PlotSubject(x)));
            Assert.AreEqual("72 -- <sword/shield>", player.ToString());
        }
    }
}
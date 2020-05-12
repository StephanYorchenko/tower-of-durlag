using System.Collections.Generic;
using System.Linq;
using LabirintDemoGame.Architecture;
using NUnit.Framework;

namespace LabirintDemoGame.Tests
{
    [TestFixture]
    public class PlayerShould
    {

        [Test]
        public void IsInitializationCorrect()
        {
            var player = new Player();
            Assert.AreEqual(100, player.Hp);
            //player = new Player(13, new []{new PlotSubject("some item")});
            Assert.AreEqual(13, player.Hp);
        }

        [Test]
        public void PlayerChangeHpShould()
        {
            var player = new Player();
            Assert.AreEqual(100, player.Hp);
            player.ApplyChanges(new Option{Hp=-10, Gold = 200});
            Assert.AreEqual(90, player.Hp);
            Assert.AreEqual(200, player.Gold);
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
            var player = new Player(13);
            Assert.AreEqual("13", player.ToString());
            player.ApplyChanges(new Option {Hp = 50, Requirements=new[]{1, 0, 0, 0, 0, 0}});
            Assert.AreEqual("63", player.ToString());
            player = new Player(72);
            Assert.AreEqual("72", player.ToString());
        }
    }
}
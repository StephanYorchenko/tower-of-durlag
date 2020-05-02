using NUnit.Framework;

namespace LabirintDemoGame
{
    [TestFixture]
    public class Player_Should
    {

        public void IsInitializationCorrect()
        {
            var player = new Player();
            Assert.AreEqual(100, player.Health);
            Assert.AreEqual(0, player.Bag.Count);
            player = new Player(13, new []{new PlotSubject("some item")});
            Assert.AreEqual(13, player.Health);
            
        }
    }
}
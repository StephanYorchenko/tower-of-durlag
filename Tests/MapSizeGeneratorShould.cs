using LabirintDemoGame.Generators;
using NUnit.Framework;

namespace LabirintDemoGame.Tests
{
    [TestFixture]
    public class MapSizeGeneratorShould
    {
        [Test]
        public void InitializeMapGeneratorShould()
        {
            var sizeGenerator = new MapSizeGenerator(5, 7);
            Assert.AreEqual(5, sizeGenerator.Height);
            Assert.AreEqual(7, sizeGenerator.Width);
        }
    }
}
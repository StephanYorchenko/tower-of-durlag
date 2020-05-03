using NUnit.Framework;

namespace LabirintDemoGame
{
    [TestFixture]
    public class MapController_Should
    {
        [TestCase(5, 9)]
        [TestCase(13, 17)]
        [TestCase(13, 45)]
        [TestCase(24, 56)]
        public void CorrectHeight(int height, int width)
        {
            var mapController = new MapController(width, height);
            Assert.AreEqual(height, mapController.MazeHeight);
        }
        
        [TestCase(5, 9)]
        [TestCase(13, 17)]
        [TestCase(13, 45)]
        [TestCase(24, 56)]
        public void CorrectWidth(int height, int width)
        {
            var mapController = new MapController(width, height);
            Assert.AreEqual(width, mapController.MazeWidth);
        }
        
        [TestCase(5, 9)]
        [TestCase(13, 17)]
        [TestCase(13, 45)]
        [TestCase(24, 56)]
        public void CorrectHeightAndWidth(int height, int width)
        {
            var mapController = new MapController(width, height);
            Assert.AreEqual(height, mapController.MazeHeight);
            Assert.AreEqual(width, mapController.MazeWidth);
        }
    }
}
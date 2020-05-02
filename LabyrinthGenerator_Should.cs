using NUnit.Framework;

namespace LabirintDemoGame
{
    [TestFixture]
    public class LabyrinthGeneratorShould
    {
        [TestCase(3, 3, 1)]
        [TestCase(5, 3, 2)]
        [TestCase(7, 7, 9)]
        public void StartGenerator_should(int width, int height, int expected)
        {
            var sampleLabyrinth = new LabyrinthGenerator(width, height);
            var example = new Cell(1, 1, CellTypes.Empty);
            Assert.AreEqual(expected, sampleLabyrinth.UnvisitedCells.Count);
            Assert.IsTrue(sampleLabyrinth.UnvisitedCells.Contains(example), "Sample start generator");
        }

        [Test]
        public void GetUnvisitedNeighbours_Should()
        {
            var labyrinth = new LabyrinthGenerator(7, 7);
            var neighboursMiddle = labyrinth.GetUnvisitedNeighbours(new Cell(3, 3, CellTypes.Empty));
            var neighboursCorner = labyrinth.GetUnvisitedNeighbours(new Cell(1, 1, CellTypes.Empty));
            Assert.AreEqual(4, neighboursMiddle.Length);
            Assert.AreEqual(2, neighboursCorner.Length);
        }
    }
}
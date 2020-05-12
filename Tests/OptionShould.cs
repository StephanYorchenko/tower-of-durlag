using LabirintDemoGame.Architecture;
using NUnit.Framework;

namespace LabirintDemoGame.Tests
{
    [TestFixture]
    public class OptionShould
    {
        private const string jsonTemplate = @"{""Name""     : ""Обыскать рюкзак"" }";

        [Test]
        public void FromJsonShould()
        {
            var actual = Option.FromJson(jsonTemplate);
            Assert.AreEqual("Обыскать рюкзак", actual.Name);
        }
    }
}
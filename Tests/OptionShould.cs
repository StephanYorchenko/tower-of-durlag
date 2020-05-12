using LabirintDemoGame.Architecture;
using NUnit.Framework;

namespace LabirintDemoGame.Tests
{
    [TestFixture]
    public class OptionShould
    {
        private const string JsonTemplate = @"{""Name""     : ""Обыскать рюкзак"" }";

        [Test]
        public void FromJsonShould()
        {
            var actual = Option.FromJson(JsonTemplate);
            Assert.AreEqual("Обыскать рюкзак", actual.Name);
        }
    }
}
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

        [Test]
        public void IsValidShould()
        {
            var player = new Player();
            var option = new Option {Requirements = "10,0,0,0,0,0"};
            Assert.IsFalse(option.IsValid(player));
            var option2 = new Option {Requirements = "1,0,0,0,0,0"};
            Assert.IsTrue(option2.IsValid(player));
        }
    }
}
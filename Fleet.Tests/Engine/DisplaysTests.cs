using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class DisplaysTests
    {
        [TestCase("Cockpit [FM:1]", "[FM]", 1)]
        [TestCase("Cockpit [FM:0] left", "[FM]", 0)]
        [TestCase("Cockpit [FM:12]", "[FM]", 12)]
        [TestCase("Cockpit [FM]", "[FM]", -1)]
        [TestCase("Cockpit [FM:x]", "[FM]", -1)]
        [TestCase("Cockpit [FM:]", "[FM]", -1)]
        [TestCase("Cockpit", "[FM]", -1)]
        [TestCase("Seat [MINER:2]", "[MINER]", 2)]
        public void SurfaceIndex(string name, string tag, int expected)
        {
            Assert.That(Displays.SurfaceIndex(name, tag), Is.EqualTo(expected));
        }

        [Test]
        public void EmptyDisplays_WriteDoesNothing()
        {
            var d = new Displays();
            Assert.That(d.Count, Is.EqualTo(0));
            Assert.DoesNotThrow(() => d.Write(new System.Text.StringBuilder("x")));
        }
    }
}

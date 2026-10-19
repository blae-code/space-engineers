using System.Collections.Generic;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class CargoIOTests
    {
        [Test]
        public void FillFromRaw()
        {
            Assert.That(CargoIO.FillFromRaw(500000, 1000000), Is.EqualTo(0.5).Within(1e-9));
            Assert.That(CargoIO.FillFromRaw(0, 0), Is.EqualTo(0));
            Assert.That(CargoIO.FillFromRaw(2000, 1000), Is.EqualTo(1));
            Assert.That(CargoIO.FillFromRaw(-5, 1000), Is.EqualTo(0));
        }

        [Test]
        public void ShouldEject()
        {
            var eject = new List<string> { "Stone", "Ice" };
            Assert.That(CargoIO.ShouldEject("MyObjectBuilder_Ore", "Stone", eject), Is.True);
            Assert.That(CargoIO.ShouldEject("MyObjectBuilder_Ore", "stone", eject), Is.True);
            Assert.That(CargoIO.ShouldEject("MyObjectBuilder_Ore", "Iron", eject), Is.False);
            Assert.That(CargoIO.ShouldEject("MyObjectBuilder_Ingot", "Stone", eject), Is.False);
            Assert.That(CargoIO.ShouldEject("MyObjectBuilder_Ore", "Stone", new List<string>()), Is.False);
        }
    }
}

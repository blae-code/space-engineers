using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class EnergyIOTests
    {
        [TestCase("SmallHydrogenTank", true)]
        [TestCase("SmallHydrogenTankSmall", true)]
        [TestCase("LargeHydrogenTankIndustrial", true)]
        [TestCase("OxygenTankSmall", false)]
        [TestCase("", false)]
        public void IsHydrogenTank(string subtype, bool expected)
        {
            Assert.That(EnergyIO.IsHydrogenTank(subtype), Is.EqualTo(expected));
        }

        [Test]
        public void NoBlocks_ReportsFullAndAbsent()
        {
            var e = new EnergyIO();
            Assert.That(e.HasBattery, Is.False);
            Assert.That(e.BatteryFill(), Is.EqualTo(1));
            Assert.That(e.HydrogenFill(), Is.EqualTo(1));
            Assert.That(e.UraniumKg(), Is.EqualTo(0));
            Assert.That(e.IsCharged(), Is.True);
            Assert.DoesNotThrow(() => e.SetCharging(true));
        }
    }
}

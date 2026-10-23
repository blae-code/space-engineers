using IngameScript;
using NUnit.Framework;
using System.Text;

namespace Fleet.Tests.Net
{
    [TestFixture]
    public class TmsDemandMathTests
    {
        [TestCase(500, 0.0, 500)]
        [TestCase(500, 250.0, 250)]
        [TestCase(500, 499.5, 1)]
        [TestCase(500, 500.0, 0)]
        [TestCase(500, 501.0, 0)]
        [TestCase(0, 0.0, 0)]
        public void TryCalculateDeficitKg_UsesCeilingAndNeverReturnsNegative(
            int targetKg, double stockKg, int expectedDeficitKg)
        {
            int deficitKg;

            bool valid = TmsDemandMath.TryCalculateDeficitKg(targetKg, stockKg, out deficitKg);

            Assert.That(valid, Is.True);
            Assert.That(deficitKg, Is.EqualTo(expectedDeficitKg));
        }

        [Test]
        public void TryCalculateDeficitKg_RejectsInvalidInputs()
        {
            int deficitKg;

            Assert.That(TmsDemandMath.TryCalculateDeficitKg(-1, 0.0, out deficitKg), Is.False);
            Assert.That(TmsDemandMath.TryCalculateDeficitKg(1, -1.0, out deficitKg), Is.False);
            Assert.That(TmsDemandMath.TryCalculateDeficitKg(1, double.NaN, out deficitKg), Is.False);
            Assert.That(TmsDemandMath.TryCalculateDeficitKg(1, double.PositiveInfinity, out deficitKg),
                Is.False);
            Assert.That(deficitKg, Is.EqualTo(0));
        }

        [TestCase("URANIUM", 5)]
        [TestCase("PLATINUM", 5)]
        [TestCase("GOLD", 5)]
        [TestCase("MAGNESIUM", 5)]
        [TestCase("IRON", 3)]
        public void TryGetPriority_ReturnsConfiguredPriority(string oreType, int expectedPriority)
        {
            int priority;

            bool valid = TmsDemandMath.TryGetPriority(oreType, out priority);

            Assert.That(valid, Is.True);
            Assert.That(priority, Is.EqualTo(expectedPriority));
        }

        [Test]
        public void TryGetPriority_RejectsUnknownOre()
        {
            int priority;

            Assert.That(TmsDemandMath.TryGetPriority("NICKEL", out priority), Is.False);
            Assert.That(priority, Is.EqualTo(0));
        }

        [Test]
        public void TryWriteBaseDemand_IntegerOverloadUsesCanonicalDigits()
        {
            var buffer = new StringBuilder();

            bool valid = TmsProtocol.TryWriteBaseDemand(buffer, "IRON", 3, 2500000, 5000000);

            Assert.That(valid, Is.True);
            Assert.That(buffer.ToString(), Is.EqualTo("DEMAND;IRON;3;2500000;5000000"));
        }
    }
}

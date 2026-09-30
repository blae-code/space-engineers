using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class BrakingCurveTests
    {
        [TestCase(50, 1, 100, 1, 10)]
        [TestCase(50, 1, 5, 1, 5)]
        [TestCase(50, 1, 100, 0.8, 8)]
        [TestCase(0, 1, 100, 1, 0)]
        [TestCase(-3, 1, 100, 1, 0)]
        [TestCase(50, 0, 100, 1, 0)]
        [TestCase(50, -2, 100, 1, 0)]
        [TestCase(50, 1, 0, 1, 0)]
        public void SafeSpeed(double dist, double a, double cap, double margin, double expected)
        {
            Assert.That(BrakingCurve.SafeSpeed(dist, a, cap, margin), Is.EqualTo(expected).Within(1e-9));
        }

        [Test]
        public void StopDistance()
        {
            Assert.That(BrakingCurve.StopDistance(10, 2), Is.EqualTo(25).Within(1e-9));
            Assert.That(BrakingCurve.StopDistance(-10, 2), Is.EqualTo(25).Within(1e-9));
            Assert.That(BrakingCurve.StopDistance(0, 2), Is.EqualTo(0).Within(1e-9));
            Assert.That(double.IsPositiveInfinity(BrakingCurve.StopDistance(5, 0)), Is.True);
        }

        [Test]
        public void SafeSpeed_IsConsistentWithStopDistance()
        {
            var v = BrakingCurve.SafeSpeed(80, 4, 1000, 1);
            Assert.That(BrakingCurve.StopDistance(v, 4), Is.EqualTo(80).Within(1e-6));
        }
    }
}
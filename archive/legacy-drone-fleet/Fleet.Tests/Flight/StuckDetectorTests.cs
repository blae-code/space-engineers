using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class StuckDetectorTests
    {
        [Test]
        public void StuckAfterTimeout()
        {
            var s = new StuckDetector(5, 0.2);
            Assert.That(s.Update(0, 0, true), Is.False);
            Assert.That(s.Update(4.9, 0.1, true), Is.False);
            Assert.That(s.Update(5.0, 0, true), Is.True);
        }

        [Test]
        public void ProgressRestartsTheClock()
        {
            var s = new StuckDetector(5, 0.2);
            s.Update(0, 0, true);
            s.Update(3, 1.0, true);
            Assert.That(s.Update(7, 0, true), Is.False);
            Assert.That(s.Update(11.9, 0, true), Is.False);
            Assert.That(s.Update(12, 0, true), Is.True);
        }

        [Test]
        public void NotThrusting_IsNeverStuck()
        {
            var s = new StuckDetector(5, 0.2);
            s.Update(0, 0, true);
            Assert.That(s.Update(10, 0, false), Is.False);
            Assert.That(s.Update(11, 0, true), Is.False);
        }

        [Test]
        public void Reset_Clears()
        {
            var s = new StuckDetector(5, 0.2);
            s.Update(0, 0, true);
            s.Reset();
            Assert.That(s.Update(6, 0, true), Is.False);
        }
    }
}
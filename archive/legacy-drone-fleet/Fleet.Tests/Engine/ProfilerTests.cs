using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class ProfilerTests
    {
        [Test]
        public void Empty_IsZero()
        {
            var p = new Profiler(3);
            Assert.That(p.Last, Is.EqualTo(0));
            Assert.That(p.Average, Is.EqualTo(0));
            Assert.That(p.AverageMs, Is.EqualTo(0));
            Assert.That(p.Peak, Is.EqualTo(0));
            Assert.That(p.Count, Is.EqualTo(0));
        }

        [Test]
        public void AveragesOverWindow_AndDropsOldest()
        {
            var p = new Profiler(3);
            p.Record(10, 1.0); p.Record(20, 2.0); p.Record(30, 3.0);
            Assert.That(p.Average, Is.EqualTo(20).Within(1e-9));
            Assert.That(p.AverageMs, Is.EqualTo(2.0).Within(1e-9));
            p.Record(40, 4.0);
            Assert.That(p.Count, Is.EqualTo(3));
            Assert.That(p.Average, Is.EqualTo(30).Within(1e-9));
            Assert.That(p.AverageMs, Is.EqualTo(3.0).Within(1e-9));
            Assert.That(p.Last, Is.EqualTo(40));
        }

        [Test]
        public void Peak_TracksMax_AndResets()
        {
            var p = new Profiler(2);
            p.Record(50, 0.5); p.Record(10, 0.1); p.Record(20, 0.2);
            Assert.That(p.Peak, Is.EqualTo(50));
            Assert.That(p.PeakMs, Is.EqualTo(0.5).Within(1e-9));
            p.ResetPeak();
            Assert.That(p.Peak, Is.EqualTo(0));
            Assert.That(p.PeakMs, Is.EqualTo(0));
            Assert.That(p.Average, Is.EqualTo(15).Within(1e-9));
            p.Record(5, 0.05);
            Assert.That(p.Peak, Is.EqualTo(5));
        }

        [Test]
        public void PartialWindow_AveragesOnlyHeldSamples()
        {
            var p = new Profiler(10);
            p.Record(4, 1); p.Record(6, 3);
            Assert.That(p.Count, Is.EqualTo(2));
            Assert.That(p.Average, Is.EqualTo(5).Within(1e-9));
            Assert.That(p.AverageMs, Is.EqualTo(2).Within(1e-9));
        }
    }
}
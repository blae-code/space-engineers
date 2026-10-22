using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class DrillLogicTests
    {
        [Test]
        public void ReachesDepth_ThenRetracts_ThenDone()
        {
            var d = new DrillLogic();
            d.Start(10, 1.5);
            Assert.That(d.Update(0, 5, 1.5), Is.EqualTo(DrillLogic.Phase.Advance));
            Assert.That(d.Update(1, 10, 1.5), Is.EqualTo(DrillLogic.Phase.Retract));
            Assert.That(d.Update(2, 3, -4), Is.EqualTo(DrillLogic.Phase.Retract));
            Assert.That(d.Update(3, 0.4, -4), Is.EqualTo(DrillLogic.Phase.Done));
            Assert.That(d.WasBlocked, Is.False);
        }

        [Test]
        public void Stall_HalvesFeed_ThenBlocks()
        {
            var d = new DrillLogic();
            d.Start(10, 1.5);
            d.Update(0, 1, 0);
            d.Update(2.9, 1, 0);
            Assert.That(d.Feed, Is.EqualTo(1.5));
            d.Update(3.0, 1, 0);
            Assert.That(d.Feed, Is.EqualTo(0.75));
            Assert.That(d.Update(7.9, 1, 0), Is.EqualTo(DrillLogic.Phase.Advance));
            Assert.That(d.Update(8.0, 1, 0), Is.EqualTo(DrillLogic.Phase.Retract));
            Assert.That(d.WasBlocked, Is.True);
        }

        [Test]
        public void Recovery_KeepsHalvedFeed_AndResetsTimer()
        {
            var d = new DrillLogic();
            d.Start(10, 1.5);
            d.Update(0, 1, 0);
            d.Update(3, 1, 0);               // halved
            d.Update(4, 1.2, 0.5);           // moving again (0.5 >= 0.075)
            Assert.That(d.Feed, Is.EqualTo(0.75));
            d.Update(20, 2, 0);              // stalls again: timer restarts at 20
            Assert.That(d.Update(24.9, 2, 0), Is.EqualTo(DrillLogic.Phase.Advance));
            Assert.That(d.Update(25, 2, 0), Is.EqualTo(DrillLogic.Phase.Retract));
        }

        [Test]
        public void BeginRetract_External()
        {
            var d = new DrillLogic();
            d.Start(10, 1.5);
            d.BeginRetract();
            Assert.That(d.Current, Is.EqualTo(DrillLogic.Phase.Retract));
            Assert.That(d.Update(1, 0.2, -3), Is.EqualTo(DrillLogic.Phase.Done));
            Assert.That(d.WasBlocked, Is.False);
        }

        [Test]
        public void Start_ResetsEverything()
        {
            var d = new DrillLogic();
            d.Start(10, 1.5);
            d.Update(0, 1, 0); d.Update(3, 1, 0); d.Update(8, 1, 0);
            d.Start(20, 2);
            Assert.That(d.Current, Is.EqualTo(DrillLogic.Phase.Advance));
            Assert.That(d.Feed, Is.EqualTo(2));
            Assert.That(d.WasBlocked, Is.False);
            Assert.That(d.Update(9, 1, 0), Is.EqualTo(DrillLogic.Phase.Advance));  // stall timer restarted
        }
    }
}

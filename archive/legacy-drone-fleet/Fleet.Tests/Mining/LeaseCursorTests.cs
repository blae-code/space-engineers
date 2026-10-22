using NUnit.Framework;
using VRage.Game.ModAPI.Ingame.Utilities;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class LeaseCursorTests
    {
        static Lease L(int job, int id, params int[] holes)
        {
            var l = new Lease { JobId = job, LeaseId = id, Count = holes.Length };
            for (int k = 0; k < holes.Length; k++) l[k] = holes[k];
            return l;
        }

        [Test] public void Fresh_IsInactive()
        {
            var c = new LeaseCursor();
            Assert.That(c.Active, Is.False);
            Assert.That(c.Complete, Is.False);
            Assert.That(c.Next(), Is.EqualTo(-1));
        }

        [Test] public void Next_WalksUnfinishedHolesInLeaseOrder()
        {
            var c = new LeaseCursor();
            var l = L(1, 5, 7, 2, 9);
            c.Set(ref l);
            Assert.That(c.Next(), Is.EqualTo(7));
            Assert.That(c.MarkDone(7), Is.True);
            Assert.That(c.Next(), Is.EqualTo(2));
            Assert.That(c.MarkDone(9), Is.True);
            Assert.That(c.Next(), Is.EqualTo(2));
            Assert.That(c.DoneMask, Is.EqualTo(5));
            Assert.That(c.Complete, Is.False);
            c.MarkDone(2);
            Assert.That(c.Complete, Is.True);
            Assert.That(c.Next(), Is.EqualTo(-1));
        }

        [Test] public void MarkDone_OutsideTheLease_IsFalse()
        {
            var c = new LeaseCursor();
            var l = L(1, 5, 7);
            c.Set(ref l);
            Assert.That(c.MarkDone(8), Is.False);
            Assert.That(c.DoneMask, Is.EqualTo(0));
        }

        [Test] public void SameLeaseAgain_KeepsTheMask_NewLeaseResetsIt()
        {
            var c = new LeaseCursor();
            var l = L(1, 5, 7, 8);
            c.Set(ref l);
            c.MarkDone(7);
            var again = L(1, 5, 7, 8);
            c.Set(ref again);
            Assert.That(c.DoneMask, Is.EqualTo(1));
            var next = L(1, 6, 3);
            c.Set(ref next);
            Assert.That(c.LeaseId, Is.EqualTo(6));
            Assert.That(c.DoneMask, Is.EqualTo(0));
        }

        [Test] public void EmptyLease_IsInactive()
        {
            var c = new LeaseCursor();
            var l = L(1, 5, 7);
            c.Set(ref l);
            var none = L(1, 0);
            c.Set(ref none);
            Assert.That(c.Active, Is.False);
            Assert.That(c.JobId, Is.EqualTo(1));
        }

        [Test] public void SaveLoad_RoundTrips()
        {
            var c = new LeaseCursor();
            var l = L(4, 12, 10, 11, 12, 13);
            c.Set(ref l);
            c.MarkDone(11); c.MarkDone(13);
            var ini = new MyIni();
            c.Save(ini, "Fleet");
            var d = new LeaseCursor();
            d.Load(ini, "Fleet");
            Assert.That(d.JobId, Is.EqualTo(4));
            Assert.That(d.LeaseId, Is.EqualTo(12));
            Assert.That(d.Count, Is.EqualTo(4));
            Assert.That(d.DoneMask, Is.EqualTo(10));
            Assert.That(d.Next(), Is.EqualTo(10));
        }

        [TestCase("")]
        [TestCase("[Fleet]\nlease=1,2,3\n")]
        [TestCase("[Fleet]\nlease=1,2,5,0,1,2,3,0\n")]      // count over 4
        [TestCase("[Fleet]\nlease=1,2,2,0,1,0,0,4\n")]      // mask bit beyond count
        [TestCase("[Fleet]\nlease=1,2,1,-3,0,0,0,0\n")]     // negative hole
        [TestCase("[Fleet]\nlease=1,2,x,0,0,0,0,0\n")]
        public void Load_Garbled_GivesNoLease(string text)
        {
            var ini = new MyIni();
            ini.TryParse(text);
            var c = new LeaseCursor();
            var l = L(9, 9, 1);
            c.Set(ref l);
            c.Load(ini, "Fleet");
            Assert.That(c.Active, Is.False);
            Assert.That(c.LeaseId, Is.EqualTo(0));
        }
    }
}

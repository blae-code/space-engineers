using NUnit.Framework;
using VRage.Game.ModAPI.Ingame.Utilities;
using static IngameScript.Program;

namespace Fleet.Tests.Dispatch
{
    [TestFixture]
    public class LeaseTableTests
    {
        const long A = 101, B = 202;

        static LeaseTable Table(int holes)
        {
            var t = new LeaseTable();
            t.Start(1, holes);
            return t;
        }

        static Lease Grant(LeaseTable t, long drone, double now, int size = 3)
        {
            var l = new Lease();
            t.Grant(drone, now, size, ref l);
            return l;
        }

        [Test] public void Start_AllFree()
        {
            var t = Table(9);
            Assert.That(t.JobId, Is.EqualTo(1));
            Assert.That(t.Holes, Is.EqualTo(9));
            Assert.That(t.FreeCount, Is.EqualTo(9));
            Assert.That(t.LeasedCount, Is.EqualTo(0));
            Assert.That(t.DoneCount, Is.EqualTo(0));
            Assert.That(t.Complete, Is.False);
            Assert.That(t.StateOf(0), Is.EqualTo(LeaseTable.Free));
            Assert.That(t.StateOf(9), Is.EqualTo(-1));
        }

        [Test] public void Grant_TakesHolesInSpiralOrder()
        {
            var t = Table(9);
            var a = Grant(t, A, 0);
            var b = Grant(t, B, 0);
            Assert.That(a.JobId, Is.EqualTo(1));
            Assert.That(a.Count, Is.EqualTo(3));
            Assert.That(new[] { a[0], a[1], a[2] }, Is.EqualTo(new[] { 0, 1, 2 }));
            Assert.That(new[] { b[0], b[1], b[2] }, Is.EqualTo(new[] { 3, 4, 5 }));
            Assert.That(b.LeaseId, Is.Not.EqualTo(a.LeaseId));
            Assert.That(t.LeasedCount, Is.EqualTo(6));
            Assert.That(t.FreeCount, Is.EqualTo(3));
            Assert.That(t.StateOf(4), Is.EqualTo(LeaseTable.Leased));
        }

        [Test] public void Grant_SizeIsClampedToOneToFour()
        {
            var t = Table(20);
            Assert.That(Grant(t, A, 0, 0).Count, Is.EqualTo(1));
            Assert.That(Grant(t, B, 0, 9).Count, Is.EqualTo(4));
        }

        [Test] public void Grant_Again_ReturnsTheSameLease()
        {
            var t = Table(9);
            var first = Grant(t, A, 0);
            var again = Grant(t, A, 5);
            Assert.That(again.LeaseId, Is.EqualTo(first.LeaseId));
            Assert.That(again[2], Is.EqualTo(first[2]));
            Assert.That(t.LeasedCount, Is.EqualTo(3));
        }

        [Test] public void Heard_MarksMaskedHolesDone()
        {
            var t = Table(9);
            var a = Grant(t, A, 0);
            t.Heard(A, a.LeaseId, 0x5, 1);   // holes 0 and 2
            Assert.That(t.StateOf(0), Is.EqualTo(LeaseTable.Done));
            Assert.That(t.StateOf(1), Is.EqualTo(LeaseTable.Leased));
            Assert.That(t.StateOf(2), Is.EqualTo(LeaseTable.Done));
            Assert.That(t.DoneCount, Is.EqualTo(2));
            Assert.That(t.LeasedCount, Is.EqualTo(1));
            t.Heard(A, a.LeaseId, 0x5, 2);   // repeated: idempotent
            Assert.That(t.DoneCount, Is.EqualTo(2));
        }

        [Test] public void Heard_FromAnotherDrone_IsIgnored()
        {
            var t = Table(9);
            var a = Grant(t, A, 0);
            t.Heard(B, a.LeaseId, 0x7, 1);
            Assert.That(t.DoneCount, Is.EqualTo(0));
        }

        [Test] public void FinishedLease_NextGrantIsNew()
        {
            var t = Table(9);
            var a = Grant(t, A, 0);
            t.Heard(A, a.LeaseId, 0x7, 1);
            var next = Grant(t, A, 2);
            Assert.That(next.LeaseId, Is.Not.EqualTo(a.LeaseId));
            Assert.That(next[0], Is.EqualTo(3));
        }

        [Test] public void NothingFree_GivesEmptyLease_AndCompleteWhenAllDone()
        {
            var t = Table(2);
            var a = Grant(t, A, 0, 4);
            Assert.That(a.Count, Is.EqualTo(2));
            var b = Grant(t, B, 0);
            Assert.That(b.Count, Is.EqualTo(0));
            Assert.That(b.JobId, Is.EqualTo(1));
            Assert.That(t.Complete, Is.False);
            t.Heard(A, a.LeaseId, 0x3, 1);
            Assert.That(t.Complete, Is.True);
        }

        [Test] public void Expire_ReturnsUnfinishedHolesToTheEnd()
        {
            var t = Table(6);
            var a = Grant(t, A, 0);           // 0 1 2
            t.Heard(A, a.LeaseId, 0x1, 10);   // hole 0 done
            t.Expire(309, 300);               // heard at 10: not yet silent
            Assert.That(t.LeasedCount, Is.EqualTo(2));
            t.Expire(310, 300);
            Assert.That(t.LeasedCount, Is.EqualTo(0));
            Assert.That(t.FreeCount, Is.EqualTo(5));
            Assert.That(t.StateOf(0), Is.EqualTo(LeaseTable.Done));
            var b = Grant(t, B, 311, 4);      // free order now 3 4 5 1 2
            Assert.That(new[] { b[0], b[1], b[2], b[3] }, Is.EqualTo(new[] { 3, 4, 5, 1 }));
        }

        [Test] public void ExpiredDrone_ComesBack_GetsANewLease()
        {
            var t = Table(9);
            var a = Grant(t, A, 0);
            t.Expire(400, 300);
            t.Heard(A, a.LeaseId, 0x7, 401);  // its old lease is gone: no effect
            Assert.That(t.DoneCount, Is.EqualTo(0));
            var again = Grant(t, A, 402);
            Assert.That(again.LeaseId, Is.Not.EqualTo(a.LeaseId));
        }

        [Test] public void Start_AgainClearsLeases_ButLeaseIdsKeepCounting()
        {
            var t = Table(9);
            var a = Grant(t, A, 0);
            t.Start(2, 4);
            Assert.That(t.LeasedCount, Is.EqualTo(0));
            var b = Grant(t, A, 1);
            Assert.That(b.JobId, Is.EqualTo(2));
            Assert.That(b.LeaseId, Is.GreaterThan(a.LeaseId));
            Assert.That(b[0], Is.EqualTo(0));
        }

        [Test] public void SaveLoad_RoundTrips_WithGracePeriod()
        {
            var t = Table(6);
            var a = Grant(t, A, 0);           // 0 1 2
            t.Heard(A, a.LeaseId, 0x2, 5);    // hole 1 done
            var ini = new MyIni();
            t.Save(ini, "Leases");
            var u = new LeaseTable();
            Assert.That(u.Load(ini, "Leases", 1000), Is.True);
            Assert.That(u.JobId, Is.EqualTo(1));
            Assert.That(u.DoneCount, Is.EqualTo(1));
            Assert.That(u.LeasedCount, Is.EqualTo(2));
            Assert.That(u.FreeCount, Is.EqualTo(3));
            u.Expire(1299, 300);              // heard "at 1000" after the reload
            Assert.That(u.LeasedCount, Is.EqualTo(2));
            var again = Grant(u, A, 1300);
            Assert.That(again.LeaseId, Is.EqualTo(a.LeaseId));
            var b = Grant(u, B, 1300);
            Assert.That(b.LeaseId, Is.GreaterThan(a.LeaseId));
            Assert.That(b[0], Is.EqualTo(3));
        }

        [TestCase("")]
        [TestCase("[Leases]\njobId=1\nholes=3\nstate=00\n")]
        [TestCase("[Leases]\njobId=1\nholes=3\nstate=0x0\n")]
        [TestCase("[Leases]\njobId=0\nholes=3\nstate=000\n")]
        public void Load_Garbled_GivesEmptyTable(string text)
        {
            var ini = new MyIni();
            ini.TryParse(text);
            var t = Table(9);
            Assert.That(t.Load(ini, "Leases", 0), Is.False);
            Assert.That(t.Holes, Is.EqualTo(0));
            Assert.That(t.JobId, Is.EqualTo(0));
        }

        [Test] public void Load_LeasedHoleWithoutALease_BecomesFree()
        {
            var ini = new MyIni();
            ini.TryParse("[Leases]\njobId=3\nholes=3\nnextId=5\nstate=120\nleases=0\n");
            var t = new LeaseTable();
            Assert.That(t.Load(ini, "Leases", 0), Is.True);
            Assert.That(t.StateOf(0), Is.EqualTo(LeaseTable.Free));
            Assert.That(t.StateOf(1), Is.EqualTo(LeaseTable.Done));
            Assert.That(t.FreeCount, Is.EqualTo(2));
            Assert.That(Grant(t, A, 0).LeaseId, Is.EqualTo(5));
        }
    }
}

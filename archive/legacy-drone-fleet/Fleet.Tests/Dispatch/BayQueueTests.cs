using System.Collections.Generic;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Dispatch
{
    [TestFixture]
    public class BayQueueTests
    {
        const long A = 101, B = 202, C = 303;
        const double Reserve = 90;

        static BayQueue Queue(params long[] bays)
        {
            var q = new BayQueue();
            q.SetBays(new List<long>(bays));
            return q;
        }

        [Test] public void FreeBay_IsReservedAndGranted()
        {
            var q = Queue(11, 12);
            long arg;
            Assert.That(q.Request(A, 0, Reserve, out arg), Is.EqualTo(FleetMsg.DockGo));
            Assert.That(arg, Is.EqualTo(11));
            Assert.That(q.Request(B, 0, Reserve, out arg), Is.EqualTo(FleetMsg.DockGo));
            Assert.That(arg, Is.EqualTo(12));
            Assert.That(q.ReservedBay(A, 1), Is.EqualTo(11));
        }

        [Test] public void Request_IsIdempotent()
        {
            var q = Queue(11);
            long arg;
            q.Request(A, 0, Reserve, out arg);
            Assert.That(q.Request(A, 5, Reserve, out arg), Is.EqualTo(FleetMsg.DockGo));
            Assert.That(arg, Is.EqualTo(11));
            q.Request(B, 5, Reserve, out arg);
            Assert.That(q.Request(B, 6, Reserve, out arg), Is.EqualTo(FleetMsg.DockWait));
            Assert.That(arg, Is.EqualTo(0));
            Assert.That(q.Waiting, Is.EqualTo(1));
        }

        [Test] public void NoFreeBay_QueuesInOrder()
        {
            var q = Queue(11);
            long arg;
            q.Request(A, 0, Reserve, out arg);
            Assert.That(q.Request(B, 0, Reserve, out arg), Is.EqualTo(FleetMsg.DockWait));
            Assert.That(arg, Is.EqualTo(0));
            Assert.That(q.Request(C, 0, Reserve, out arg), Is.EqualTo(FleetMsg.DockWait));
            Assert.That(arg, Is.EqualTo(1));
            Assert.That(q.SlotOf(C), Is.EqualTo(1));
        }

        [Test] public void OccupiedBay_IsNotFree()
        {
            var q = Queue(11, 12);
            q.SetOccupied(11, true);
            long arg;
            q.Request(A, 0, Reserve, out arg);
            Assert.That(arg, Is.EqualTo(12));
        }

        [Test] public void Docking_EndsTheReservation_UndockingFreesTheBay()
        {
            var q = Queue(11);
            long arg, drone, bay;
            q.Request(A, 0, Reserve, out arg);
            q.Request(B, 0, Reserve, out arg);          // waits
            q.SetOccupied(11, true);                    // A docked
            Assert.That(q.ReservedBay(A, 1), Is.EqualTo(0));
            Assert.That(q.Tick(2, Reserve, out drone, out bay), Is.False);
            q.SetOccupied(11, false);                   // A undocked
            Assert.That(q.Tick(3, Reserve, out drone, out bay), Is.True);
            Assert.That(drone, Is.EqualTo(B));
            Assert.That(bay, Is.EqualTo(11));
            Assert.That(q.Waiting, Is.EqualTo(0));
            Assert.That(q.ReservedBay(B, 4), Is.EqualTo(11));
        }

        [Test] public void LapsedReservation_PassesTheBayOn()
        {
            var q = Queue(11);
            long arg, drone, bay;
            q.Request(A, 0, Reserve, out arg);
            q.Request(B, 0, Reserve, out arg);
            Assert.That(q.Tick(89, Reserve, out drone, out bay), Is.False);
            Assert.That(q.Tick(90, Reserve, out drone, out bay), Is.True);
            Assert.That(drone, Is.EqualTo(B));
            Assert.That(q.ReservedBay(A, 91), Is.EqualTo(0));
        }

        [Test] public void Tick_PromotesOnlyOnePerCall()
        {
            var q = Queue(11, 12);
            long arg, drone, bay;
            q.SetOccupied(11, true); q.SetOccupied(12, true);
            q.Request(A, 0, Reserve, out arg);
            q.Request(B, 0, Reserve, out arg);
            q.SetOccupied(11, false); q.SetOccupied(12, false);
            Assert.That(q.Tick(1, Reserve, out drone, out bay), Is.True);
            Assert.That(drone, Is.EqualTo(A));
            Assert.That(q.SlotOf(B), Is.EqualTo(0), "the rest move up a slot");
            Assert.That(q.QueuedAt(0), Is.EqualTo(B));
            Assert.That(q.QueuedAt(1), Is.EqualTo(0));
            Assert.That(q.Tick(1, Reserve, out drone, out bay), Is.True);
            Assert.That(drone, Is.EqualTo(B));
            Assert.That(bay, Is.EqualTo(12));
        }

        [Test] public void Forget_LeavesQueueAndReservation()
        {
            var q = Queue(11);
            long arg;
            q.Request(A, 0, Reserve, out arg);
            q.Request(B, 0, Reserve, out arg);
            q.Request(C, 0, Reserve, out arg);
            q.Forget(B);
            Assert.That(q.SlotOf(C), Is.EqualTo(0));
            q.Forget(A);
            Assert.That(q.ReservedBay(A, 1), Is.EqualTo(0));
        }

        [Test] public void SetBays_KeepsStateForSurvivingBays()
        {
            var q = Queue(11, 12);
            long arg;
            q.Request(A, 0, Reserve, out arg);          // reserves 11
            q.SetOccupied(12, true);
            q.SetBays(new List<long> { 12, 11, 13 });
            Assert.That(q.BayCount, Is.EqualTo(3));
            Assert.That(q.ReservedBay(A, 1), Is.EqualTo(11));
            q.Request(B, 1, Reserve, out arg);
            Assert.That(arg, Is.EqualTo(13), "12 still occupied, 11 still reserved");
        }

        [Test] public void Clear_DropsReservationsAndQueue()
        {
            var q = Queue(11);
            long arg;
            q.Request(A, 0, Reserve, out arg);
            q.Request(B, 0, Reserve, out arg);
            q.Clear();
            Assert.That(q.Waiting, Is.EqualTo(0));
            Assert.That(q.Request(B, 1, Reserve, out arg), Is.EqualTo(FleetMsg.DockGo));
        }

        [Test] public void FullQueue_WaitsWithoutASlot()
        {
            var q = Queue();
            long arg;
            for (int i = 0; i < BayQueue.MaxQueue; i++) q.Request(1000 + i, 0, Reserve, out arg);
            Assert.That(q.Request(A, 0, Reserve, out arg), Is.EqualTo(FleetMsg.DockWait));
            Assert.That(arg, Is.EqualTo(-1));
            Assert.That(q.Waiting, Is.EqualTo(BayQueue.MaxQueue));
        }
    }
}

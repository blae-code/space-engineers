using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Dispatch
{
    // C43: the carrier survives a world reload mid-job (slice 3 spec §7).
    [TestFixture]
    public class DispatcherPersistenceTests
    {
        const long A = 101, B = 202;

        static string JobLine()
        {
            var j = new FleetJob { Origin = new Vector3D(10, 0, 0), Forward = new Vector3D(0, 0, -1), Up = new Vector3D(0, 1, 0), Width = 3, Height = 3, Spacing = 3 };
            return JobCodec.Encode(j);
        }

        static string Status(int job, int lease, int mask)
        {
            var s = new DroneStatus { Name = "A", State = (int)MinerState.Drill, Hydrogen = -1, Lift100 = -1, JobId = job, LeaseId = lease, DoneMask = mask, Note = "" };
            return FleetLink.Encode(new StringBuilder(), ref s).ToString();
        }

        static MyIni Saved(Dispatcher d)
        {
            var ini = new MyIni();
            d.Save(ini);
            var back = new MyIni();
            Assert.That(back.TryParse(ini.ToString()), Is.True);
            return back;
        }

        [Test] public void Reload_KeepsJobAndLeases_WithGrace()
        {
            var d = new Dispatcher();
            d.OnJob(A, JobLine(), 0);
            d.OnLeaseRequest(A, "N|2|1", 0);                  // lease 1: holes 0 1 2
            d.OnStatus(A, Status(1, 1, 0x1), 5);              // hole 0 done
            var e = new Dispatcher();
            Assert.That(e.Load(Saved(d), 1000), Is.True);
            Assert.That(e.HasJob, Is.True);
            Assert.That(e.Job.JobId, Is.EqualTo(1));
            Assert.That(e.Job.HoleCount, Is.EqualTo(9));
            Assert.That(e.Leases.DoneCount, Is.EqualTo(1));
            Assert.That(e.Leases.LeasedCount, Is.EqualTo(2));
            e.Tick(1299);
            Assert.That(e.Leases.LeasedCount, Is.EqualTo(2), "leases reload as heard at 1000");
            e.Outbox.Clear();
            e.OnLeaseRequest(A, "N|2|1", 1300);
            Assert.That(e.Outbox[0].Text, Is.EqualTo("L|2|1|1|3|0|1|2"), "same lease after the reload");
        }

        [Test] public void Reload_NextJobGetsAFreshId()
        {
            var d = new Dispatcher();
            d.OnJob(A, JobLine(), 0);
            d.OnJob(A, JobLine(), 1);                         // job 2
            var e = new Dispatcher();
            e.Load(Saved(d), 10);
            e.OnJob(B, JobLine(), 11);
            Assert.That(e.Job.JobId, Is.EqualTo(3));
        }

        [Test] public void Reload_WithoutAJob_IsFalse_ButKeepsTheIdCounter()
        {
            var d = new Dispatcher();
            var e = new Dispatcher();
            Assert.That(e.Load(Saved(d), 0), Is.False);
            Assert.That(e.HasJob, Is.False);
            e.OnJob(A, JobLine(), 1);
            Assert.That(e.Job.JobId, Is.EqualTo(1));
        }

        [Test] public void Reload_DropsReservations_DronesAskAgain()
        {
            var d = new Dispatcher();
            d.Bays.SetBays(new List<long> { 11 });
            d.OnDock(A, "R|2", 0);
            d.Load(Saved(d), 1);
            d.Outbox.Clear();
            d.OnDock(B, "R|2", 2);
            Assert.That(d.Outbox[0].Text, Is.EqualTo("G|2|11"));
        }

        [Test] public void Reload_GarbledLeases_StartTheJobAfresh()
        {
            var d = new Dispatcher();
            d.OnJob(A, JobLine(), 0);
            d.OnLeaseRequest(A, "N|2|1", 0);
            var ini = Saved(d);
            ini.Set("Dispatch.Leases", "state", "garbage");
            var e = new Dispatcher();
            Assert.That(e.Load(ini, 5), Is.True);
            Assert.That(e.Leases.JobId, Is.EqualTo(1));
            Assert.That(e.Leases.FreeCount, Is.EqualTo(9));
        }

        [Test] public void Load_EmptyStorage_IsFalse()
        {
            var e = new Dispatcher();
            Assert.That(e.Load(new MyIni(), 0), Is.False);
            Assert.That(e.Leases.Holes, Is.EqualTo(0));
        }
    }
}

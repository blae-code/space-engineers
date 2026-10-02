using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Dispatch
{
    [TestFixture]
    public class DispatcherTests
    {
        const long A = 101, B = 202, C = 303;

        static string JobLine(int w = 3, int h = 3)
        {
            var j = new FleetJob { Origin = new Vector3D(10, 0, 0), Forward = new Vector3D(0, 0, -1), Up = new Vector3D(0, 1, 0), Width = w, Height = h, Spacing = 3 };
            return JobCodec.Encode(j);
        }

        static string Status(string name, MinerState state, int flags, int job = 0, int lease = 0, int mask = 0)
        {
            var s = new DroneStatus { Name = name, State = (int)state, Battery = 90, Hydrogen = -1, Lift100 = -1, Flags = flags, JobId = job, LeaseId = lease, DoneMask = mask, Note = "" };
            return FleetLink.Encode(new StringBuilder(), ref s).ToString();
        }

        static string Out(Dispatcher d)
        {
            var lines = new List<string>();
            foreach (var o in d.Outbox) lines.Add(o.To + " " + o.Kind + " " + o.Text);
            d.Outbox.Clear();
            return string.Join("\n", lines);
        }

        static Dispatcher WithJob()
        {
            var d = new Dispatcher();
            d.OnJob(A, JobLine(), 0);
            d.Outbox.Clear();
            return d;
        }

        [Test] public void OnJob_MintsTheIdAndEchoesTheJob()
        {
            var d = new Dispatcher();
            d.OnJob(A, JobLine(), 0);
            Assert.That(d.HasJob, Is.True);
            Assert.That(d.Job.JobId, Is.EqualTo(1));
            Assert.That(d.Leases.JobId, Is.EqualTo(1));
            Assert.That(d.Leases.Holes, Is.EqualTo(9));
            Assert.That(Out(d), Is.EqualTo("101 job J|2|1|10000|0|0|0|0|-1000000|0|1000000|0|3|3|3000|0|0|0"));
            d.OnJob(B, JobLine(2, 2), 1);
            Assert.That(d.Job.JobId, Is.EqualTo(2), "a new job replaces the old one");
            Assert.That(d.Leases.Holes, Is.EqualTo(4));
        }

        [Test] public void OnJob_Garbled_KeepsTheOldJob()
        {
            var d = WithJob();
            d.OnJob(B, "J|2|nonsense", 1);
            Assert.That(d.Job.JobId, Is.EqualTo(1));
            Assert.That(d.Note, Does.Contain("bad job"));
            Assert.That(Out(d), Is.EqualTo(""));
        }

        [Test] public void LeaseRequest_WithoutAJob_GetsAnEmptyLease()
        {
            var d = new Dispatcher();
            d.OnLeaseRequest(A, "N|2|0", 0);
            Assert.That(Out(d), Is.EqualTo("101 lease L|2|0|0|0"));
        }

        [Test] public void LeaseRequest_DroneWithoutTheJob_GetsJobThenLease()
        {
            var d = WithJob();
            d.OnLeaseRequest(B, "N|2|0", 1);
            Assert.That(Out(d), Is.EqualTo(
                "202 job J|2|1|10000|0|0|0|0|-1000000|0|1000000|0|3|3|3000|0|0|0\n" +
                "202 lease L|2|1|1|3|0|1|2"));
        }

        [Test] public void LeaseRequest_DroneWithTheJob_GetsOnlyTheLease_SizeFromSetting()
        {
            var d = WithJob();
            d.LeaseSize = 2;
            d.OnLeaseRequest(A, "N|2|1", 1);
            Assert.That(Out(d), Is.EqualTo("101 lease L|2|1|1|2|0|1"));
            d.OnLeaseRequest(A, "N|2|1", 2);
            Assert.That(Out(d), Is.EqualTo("101 lease L|2|1|1|2|0|1"), "same lease while unfinished");
        }

        [Test] public void LeaseRequest_Garbled_IsIgnored()
        {
            var d = WithJob();
            d.OnLeaseRequest(A, "N|2|x", 1);
            Assert.That(Out(d), Is.EqualTo(""));
        }

        [Test] public void Status_ReportsDoneHoles_ForTheCurrentJobOnly()
        {
            var d = WithJob();
            d.OnLeaseRequest(A, "N|2|1", 1);
            d.OnStatus(A, Status("A", MinerState.Drill, FleetLink.FlagHasJob, 1, 1, 0x3), 2);
            Assert.That(d.Leases.DoneCount, Is.EqualTo(2));
            Assert.That(d.Drones.Count, Is.EqualTo(1));
            d.OnStatus(A, Status("A", MinerState.Drill, FleetLink.FlagHasJob, 7, 1, 0x7), 3);
            Assert.That(d.Leases.DoneCount, Is.EqualTo(2), "another job id: ignored");
        }

        [Test] public void Dock_GoThenWait_SlotDependsOnTheMarker()
        {
            var d = WithJob();
            d.Bays.SetBays(new List<long> { 11 });
            d.OnDock(A, "R|2", 0);
            d.OnDock(B, "R|2", 0);
            Assert.That(Out(d), Is.EqualTo("101 dock G|2|11\n202 dock W|2|-1"));
            d.HasMarker = true;
            d.OnDock(B, "R|2", 1);
            Assert.That(Out(d), Is.EqualTo("202 dock W|2|0"));
            d.OnDock(C, "G|2|5", 1);
            Assert.That(Out(d), Is.EqualTo(""), "only R is a request");
        }

        [Test] public void Tick_PushesGo_AndMovesTheQueueUp()
        {
            var d = WithJob();
            d.HasMarker = true;
            d.Bays.SetBays(new List<long> { 11 });
            d.OnDock(A, "R|2", 0);
            d.OnDock(B, "R|2", 0);
            d.OnDock(C, "R|2", 0);
            Out(d);
            d.Tick(89);
            Assert.That(Out(d), Is.EqualTo(""));
            d.Tick(90);   // A's reservation lapsed
            Assert.That(Out(d), Is.EqualTo("202 dock G|2|11\n303 dock W|2|0"));
        }

        [Test] public void Tick_ExpiresSilentLeases()
        {
            var d = WithJob();
            d.OnLeaseRequest(A, "N|2|1", 0);
            d.Tick(299);
            Assert.That(d.Leases.LeasedCount, Is.EqualTo(3));
            d.Tick(300);
            Assert.That(d.Leases.LeasedCount, Is.EqualTo(0));
        }

        [Test] public void JobRequest_PicksAnIdleDockedLead()
        {
            var d = new Dispatcher();
            d.OnStatus(A, Status("A", MinerState.Drill, FleetLink.FlagConnected), 0);
            d.OnStatus(B, Status("B", MinerState.Idle, 0), 0);
            d.OnStatus(C, Status("C", MinerState.Idle, FleetLink.FlagConnected), 0);
            Assert.That(d.OnJobRequest("Q|2|GPS:Rock:1:2:3:|4|5", 1), Is.True);
            Assert.That(Out(d), Is.EqualTo(
                "303 cmd SET Miner Width 4\n303 cmd SET Miner Height 5\n303 cmd GOTO GPS:Rock:1:2:3:\n303 cmd SHARE"));
            Assert.That(d.OnJobRequest("Q|2|GPS:Rock:1:2:3:|0|0", 2), Is.True);
            Assert.That(Out(d), Is.EqualTo("303 cmd GOTO GPS:Rock:1:2:3:\n303 cmd SHARE"));
        }

        [Test] public void JobRequest_NoIdleDockedDrone_SaysWhy()
        {
            var d = new Dispatcher();
            d.OnStatus(A, Status("A", MinerState.Idle, FleetLink.FlagConnected | FleetLink.FlagSafe), 0);
            Assert.That(d.OnJobRequest("Q|2|GPS:Rock:1:2:3:|0|0", 1), Is.False);
            Assert.That(d.Note, Does.Contain("no idle docked drone"));
            Assert.That(d.OnJobRequest("garbage", 1), Is.False);
            Assert.That(Out(d), Is.EqualTo(""));
        }

        [Test] public void FleetStart_OnlyDockedDronesHoldingTheJob_FleetStopEveryone()
        {
            var d = WithJob();
            d.OnStatus(A, Status("A", MinerState.Idle, FleetLink.FlagConnected, 1), 0);
            d.OnStatus(B, Status("B", MinerState.Idle, FleetLink.FlagConnected, 0), 0);
            d.OnStatus(C, Status("C", MinerState.Drill, 0, 1), 0);
            Assert.That(d.FleetStart(), Is.EqualTo(1));
            Assert.That(Out(d), Is.EqualTo("101 cmd START"));
            Assert.That(d.FleetStop(), Is.EqualTo(3));
            Assert.That(Out(d), Is.EqualTo("101 cmd STOP\n202 cmd STOP\n303 cmd STOP"));
        }

        [Test] public void FleetStart_WithoutAJob_SendsNothing()
        {
            var d = new Dispatcher();
            d.OnStatus(A, Status("A", MinerState.Idle, FleetLink.FlagConnected), 0);
            Assert.That(d.FleetStart(), Is.EqualTo(0));
            Assert.That(Out(d), Is.EqualTo(""));
        }
    }
}

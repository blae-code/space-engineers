using System.Text;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class FleetMsgTests
    {
        static string Lease(Lease l) { return FleetMsg.EncodeLease(new StringBuilder(), ref l).ToString(); }

        [Test] public void Lease_Format_AndRoundTrip()
        {
            var l = new Lease { JobId = 3, LeaseId = 12, Count = 3, I0 = 0, I1 = 5, I2 = 17 };
            Assert.That(Lease(l), Is.EqualTo("L|2|3|12|3|0|5|17"));
            var back = new Lease();
            Assert.That(FleetMsg.DecodeLease(Lease(l), ref back), Is.True);
            Assert.That(back.JobId, Is.EqualTo(3));
            Assert.That(back.LeaseId, Is.EqualTo(12));
            Assert.That(back.Count, Is.EqualTo(3));
            Assert.That(back[0], Is.EqualTo(0));
            Assert.That(back[1], Is.EqualTo(5));
            Assert.That(back[2], Is.EqualTo(17));
        }

        [Test] public void Lease_Empty_MeansNothingLeft()
        {
            var l = new Lease { JobId = 3, LeaseId = 0, Count = 0 };
            Assert.That(Lease(l), Is.EqualTo("L|2|3|0|0"));
            var back = new Lease { Count = 2 };
            Assert.That(FleetMsg.DecodeLease("L|2|3|0|0", ref back), Is.True);
            Assert.That(back.Count, Is.EqualTo(0));
        }

        [Test] public void Lease_Indexer_CoversFourSlots()
        {
            var l = new Lease();
            for (int k = 0; k < 4; k++) l[k] = 10 + k;
            Assert.That(l.I0, Is.EqualTo(10));
            Assert.That(l.I3, Is.EqualTo(13));
            Assert.That(l[2], Is.EqualTo(12));
        }

        [TestCase(null)]
        [TestCase("L|1|3|12|1|0")]            // wrong version
        [TestCase("L|2|3|12|5|0|1|2|3|4")]    // more than four
        [TestCase("L|2|3|12|2|0")]            // fewer indices than n
        [TestCase("L|2|3|12|1|0|9")]          // more indices than n
        [TestCase("L|2|3|12|1|-1")]           // negative index
        [TestCase("L|2|3|x|1|0")]
        public void Lease_RejectsBadLines_AndLeavesLeaseUntouched(string line)
        {
            var l = new Lease { JobId = 99, Count = 1 };
            Assert.That(FleetMsg.DecodeLease(line, ref l), Is.False);
            Assert.That(l.JobId, Is.EqualTo(99));
        }

        [Test] public void LeaseRequest_RoundTrip()
        {
            Assert.That(FleetMsg.EncodeLeaseRequest(new StringBuilder(), 4).ToString(), Is.EqualTo("N|2|4"));
            int job;
            Assert.That(FleetMsg.DecodeLeaseRequest("N|2|4", out job), Is.True);
            Assert.That(job, Is.EqualTo(4));
            Assert.That(FleetMsg.DecodeLeaseRequest("N|2|0", out job), Is.True);
            Assert.That(job, Is.EqualTo(0));
            Assert.That(FleetMsg.DecodeLeaseRequest("N|2|4|1", out job), Is.False);
            Assert.That(FleetMsg.DecodeLeaseRequest("N|2|", out job), Is.False);
        }

        [Test] public void Dock_AllThreeForms()
        {
            Assert.That(FleetMsg.EncodeDock(new StringBuilder(), FleetMsg.DockRequest, 0).ToString(), Is.EqualTo("R|2"));
            Assert.That(FleetMsg.EncodeDock(new StringBuilder(), FleetMsg.DockGo, 123456789012).ToString(), Is.EqualTo("G|2|123456789012"));
            Assert.That(FleetMsg.EncodeDock(new StringBuilder(), FleetMsg.DockWait, -1).ToString(), Is.EqualTo("W|2|-1"));
            int type; long arg;
            Assert.That(FleetMsg.DecodeDock("R|2", out type, out arg), Is.True);
            Assert.That(type, Is.EqualTo(FleetMsg.DockRequest));
            Assert.That(FleetMsg.DecodeDock("G|2|123456789012", out type, out arg), Is.True);
            Assert.That(type, Is.EqualTo(FleetMsg.DockGo));
            Assert.That(arg, Is.EqualTo(123456789012));
            Assert.That(FleetMsg.DecodeDock("W|2|2", out type, out arg), Is.True);
            Assert.That(type, Is.EqualTo(FleetMsg.DockWait));
            Assert.That(arg, Is.EqualTo(2));
        }

        [TestCase(null)]
        [TestCase("R|1")]
        [TestCase("G|2|")]
        [TestCase("W|2|1|2")]
        [TestCase("X|2|5")]
        public void Dock_RejectsBadLines(string line)
        {
            int type; long arg;
            Assert.That(FleetMsg.DecodeDock(line, out type, out arg), Is.False);
            Assert.That(type, Is.EqualTo(-1));
        }

        [Test] public void JobRequest_RoundTrip()
        {
            var line = FleetMsg.EncodeJobRequest("GPS:Rock:100:-2.5:3:", 4, 0);
            Assert.That(line, Is.EqualTo("Q|2|GPS:Rock:100:-2.5:3:|4|0"));
            string gps; int w, h;
            Assert.That(FleetMsg.DecodeJobRequest(line, out gps, out w, out h), Is.True);
            Assert.That(gps, Is.EqualTo("GPS:Rock:100:-2.5:3:"));
            Assert.That(w, Is.EqualTo(4));
            Assert.That(h, Is.EqualTo(0));
        }

        [Test] public void JobRequest_RefusesUnusableGps()
        {
            Assert.That(FleetMsg.EncodeJobRequest(null, 0, 0), Is.Null);
            Assert.That(FleetMsg.EncodeJobRequest("Rock:1:2:3", 0, 0), Is.Null);
            Assert.That(FleetMsg.EncodeJobRequest("GPS:a|b:1:2:3:", 0, 0), Is.Null);
        }

        [TestCase(null)]
        [TestCase("Q|2|GPS:R:1:2:3:|4")]          // missing H
        [TestCase("Q|2|R:1:2:3:|4|4")]            // not a GPS
        [TestCase("Q|2|GPS:R:1:2:3:|51|4")]       // over 50
        [TestCase("Q|2|GPS:R:1:2:3:|-1|4")]       // negative
        [TestCase("Q|1|GPS:R:1:2:3:|4|4")]        // wrong version
        public void JobRequest_RejectsBadLines(string line)
        {
            string gps; int w, h;
            Assert.That(FleetMsg.DecodeJobRequest(line, out gps, out w, out h), Is.False);
            Assert.That(gps, Is.Null);
        }
    }
}

using System.Text;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    // Slice 3: status line version 2 carries the fleet job, lease and done mask.
    [TestFixture]
    public class FleetLinkV2Tests
    {
        static DroneStatus Sample()
        {
            return new DroneStatus
            {
                Name = "Miner-02", State = (int)MinerState.Drill, Reason = (int)ReturnReason.None,
                Hole = 2, Holes = 9, Cargo = 40, Battery = 70, Hydrogen = -1, Lift100 = -1,
                Flags = FleetLink.FlagHasJob, JobId = 4, LeaseId = 11, DoneMask = 5, Note = "ok"
            };
        }

        [Test]
        public void Encode_V2_Format()
        {
            var s = Sample();
            Assert.That(FleetLink.Encode(new StringBuilder(), ref s).ToString(),
                Is.EqualTo("2|Miner-02|5|0|2|9|40|70|-1|-1|2|4|11|5|ok"));
        }

        [Test]
        public void RoundTrip_CarriesJobLeaseAndMask()
        {
            var s = Sample();
            var line = FleetLink.Encode(new StringBuilder(), ref s).ToString();
            var back = new DroneStatus();
            Assert.That(FleetLink.Decode(line, ref back), Is.True);
            Assert.That(back.JobId, Is.EqualTo(4));
            Assert.That(back.LeaseId, Is.EqualTo(11));
            Assert.That(back.DoneMask, Is.EqualTo(5));
            Assert.That(back.Note, Is.EqualTo("ok"));
            Assert.That(back.Cargo, Is.EqualTo(40));
        }

        [Test]
        public void Version1Line_StillDecodes_WithZeroFleetFields()
        {
            var back = new DroneStatus { JobId = 9, LeaseId = 9, DoneMask = 9 };
            Assert.That(FleetLink.Decode("1|Old|5|0|3|25|64|88|-1|182|2|note", ref back), Is.True);
            Assert.That(back.Name, Is.EqualTo("Old"));
            Assert.That(back.Flags, Is.EqualTo(2));
            Assert.That(back.Note, Is.EqualTo("note"));
            Assert.That(back.JobId, Is.EqualTo(0));
            Assert.That(back.LeaseId, Is.EqualTo(0));
            Assert.That(back.DoneMask, Is.EqualTo(0));
        }

        [TestCase("2|Miner-02|5|0|2|9|40|70|-1|-1|2|4|11|ok")]       // mask missing
        [TestCase("2|Miner-02|5|0|2|9|40|70|-1|-1|2|4|x|5|ok")]      // lease not a number
        [TestCase("2|Miner-02|5|0|2|9|40|70|-1|-1|2|4|11|5")]        // note field missing
        public void BadV2Lines_Rejected_AndStatusUntouched(string line)
        {
            var s = Sample();
            Assert.That(FleetLink.Decode(line, ref s), Is.False);
            Assert.That(s.LeaseId, Is.EqualTo(11));
            Assert.That(s.Cargo, Is.EqualTo(40));
        }
    }
}

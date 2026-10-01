using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class FleetLinkTests
    {
        static DroneStatus Sample()
        {
            return new DroneStatus
            {
                Name = "Miner-01", State = (int)MinerState.Drill, Reason = (int)ReturnReason.None,
                Hole = 3, Holes = 25, Cargo = 64, Battery = 88, Hydrogen = -1, Lift100 = 182,
                Flags = FleetLink.FlagHasJob, Note = ""
            };
        }

        static string Encode(DroneStatus s)
        {
            return FleetLink.Encode(new StringBuilder(), ref s).ToString();
        }

        [Test]
        public void Encode_Format()
        {
            Assert.That(Encode(Sample()), Is.EqualTo("1|Miner-01|5|0|3|25|64|88|-1|182|2|"));
        }

        [Test]
        public void RoundTrip()
        {
            var s = Sample();
            s.Note = "stuck";
            var back = new DroneStatus();
            Assert.That(FleetLink.Decode(Encode(s), ref back), Is.True);
            Assert.That(back.Name, Is.EqualTo("Miner-01"));
            Assert.That(back.State, Is.EqualTo((int)MinerState.Drill));
            Assert.That(back.Hole, Is.EqualTo(3));
            Assert.That(back.Holes, Is.EqualTo(25));
            Assert.That(back.Cargo, Is.EqualTo(64));
            Assert.That(back.Battery, Is.EqualTo(88));
            Assert.That(back.Hydrogen, Is.EqualTo(-1));
            Assert.That(back.Lift100, Is.EqualTo(182));
            Assert.That(back.Flags, Is.EqualTo(FleetLink.FlagHasJob));
            Assert.That(back.Note, Is.EqualTo("stuck"));
        }

        [Test]
        public void PipesAndNewlinesInText_CannotBreakFraming()
        {
            var s = Sample();
            s.Name = "a|b"; s.Note = "x|y\nz";
            var back = new DroneStatus();
            Assert.That(FleetLink.Decode(Encode(s), ref back), Is.True);
            Assert.That(back.Name, Is.EqualTo("a/b"));
            Assert.That(back.Note, Is.EqualTo("x/y z"));
        }

        [TestCase("2|Miner-01|5|0|3|25|64|88|-1|182|2|")]   // future version
        [TestCase("1|Miner-01|5|0|3|25|64|88|-1|182|2")]    // note field missing
        [TestCase("1|Miner-01|5|0|x|25|64|88|-1|182|2|")]   // not a number
        [TestCase("1|Miner-01|5|0|3")]
        [TestCase("")]
        [TestCase(null)]
        public void Decode_Rejects_AndLeavesStatusUntouched(string line)
        {
            var s = Sample();
            Assert.That(FleetLink.Decode(line, ref s), Is.False);
            Assert.That(s.Cargo, Is.EqualTo(64));
        }

        [Test]
        public void Decode_ReusesUnchangedNameString()
        {
            var s = new DroneStatus();
            var line = Encode(Sample());
            FleetLink.Decode(line, ref s);
            var name = s.Name;
            FleetLink.Decode(string.Copy(line), ref s);
            Assert.That(s.Name, Is.SameAs(name));
        }

        [TestCase("SET Miner Width 6", SettingsSchema.Width, 6)]
        [TestCase("set miner onDamage stop", SettingsSchema.OnDamage, 2)]
        [TestCase("Flight MaxSpeed 80", SettingsSchema.MaxSpeed, 80)]
        [TestCase("SET  Miner   Loop  false", SettingsSchema.Loop, 0)]
        public void TryParseSet_Valid(string text, int field, double value)
        {
            int f; double v; string err;
            Assert.That(FleetLink.TryParseSet(text, out f, out v, out err), Is.True, err);
            Assert.That(f, Is.EqualTo(field));
            Assert.That(v, Is.EqualTo(value));
        }

        [TestCase("SET Miner Width", "usage")]
        [TestCase("SET Miner Colour 3", "unknown setting")]
        [TestCase("SET Miner Width wide", "bad value")]
        [TestCase("SET Miner Width 99", "out of range")]
        [TestCase("SET Fleet Name Bob", "unknown setting")]
        public void TryParseSet_Invalid_SaysWhy(string text, string why)
        {
            int f; double v; string err;
            Assert.That(FleetLink.TryParseSet(text, out f, out v, out err), Is.False);
            Assert.That(err, Does.Contain(why));
        }

        [Test]
        public void SetCommand_RoundTripsThroughTryParseSet()
        {
            for (int i = 0; i < SettingsSchema.Fields.Length; i++)
            {
                var fld = SettingsSchema.Fields[i];
                var cmd = FleetLink.SetCommand(i, fld.Max);
                int f; double v; string err;
                Assert.That(FleetLink.TryParseSet(cmd, out f, out v, out err), Is.True, cmd + " " + err);
                Assert.That(f, Is.EqualTo(i));
                Assert.That(v, Is.EqualTo(fld.Max).Within(1e-9), cmd);
            }
        }
    }

    [TestFixture]
    public class RosterTests
    {
        static string Line(string name, int cargo)
        {
            var s = new DroneStatus { Name = name, State = (int)MinerState.Drill, Cargo = cargo, Battery = 50, Hydrogen = -1, Lift100 = -1, Note = "" };
            return FleetLink.Encode(new StringBuilder(), ref s).ToString();
        }

        [Test]
        public void StatusLines_AddAndUpdateByAddress()
        {
            var r = new Roster();
            r.Update(101, Line("A", 10), 0);
            r.Update(202, Line("B", 20), 0);
            r.Update(101, Line("A", 30), 1);
            Assert.That(r.Count, Is.EqualTo(2));
            Assert.That(r[0].Status.Cargo, Is.EqualTo(30));
            Assert.That(r.Find("b").Address, Is.EqualTo(202));
            Assert.That(r.Find("C"), Is.Null);
        }

        [Test]
        public void InvalidLine_DoesNotAddAnEntry()
        {
            var r = new Roster();
            Assert.That(r.Update(101, "garbage", 0), Is.Null);
            Assert.That(r.Count, Is.EqualTo(0));
        }

        [Test]
        public void Stale_ThenDropped_OrderKept()
        {
            var r = new Roster();
            r.Update(1, Line("A", 0), 0);
            r.Update(2, Line("B", 0), 50);
            r.Update(3, Line("C", 0), 0);
            Assert.That(r.IsStale(r[0], 11), Is.True);
            Assert.That(r.IsStale(r[1], 55), Is.False);
            r.Expire(61);
            Assert.That(r.Count, Is.EqualTo(1));
            Assert.That(r[0].Status.Name, Is.EqualTo("B"));
            r.Update(4, Line("D", 0), 62);   // a freed slot is reused
            Assert.That(r.Count, Is.EqualTo(2));
            Assert.That(r[1].Status.Name, Is.EqualTo("D"));
        }

        [Test]
        public void Full_RefusesNewDrones_KeepsUpdatingKnownOnes()
        {
            var r = new Roster();
            for (int i = 0; i < Roster.Capacity; i++) r.Update(i + 1, Line("D" + i, 0), 0);
            Assert.That(r.Update(999, Line("X", 0), 0), Is.Null);
            Assert.That(r.Update(1, Line("D0", 77), 1).Status.Cargo, Is.EqualTo(77));
        }

        [Test]
        public void AppendEntry_ShowsStateCargoBatteryAndStaleness()
        {
            var r = new Roster();
            r.Update(1, Line("Miner-01", 64), 0);
            var sb = new StringBuilder();
            r.Now = 5; r.AppendEntry(sb, 0);
            Assert.That(sb.ToString(), Is.EqualTo("Miner-01   Drill       64%  bat 50%"));
            sb.Clear(); r.Now = 20; r.AppendEntry(sb, 0);
            Assert.That(sb.ToString(), Does.EndWith("  stale"));
        }

        [Test]
        public void Entry_ConfigAndPendingEdits()
        {
            var e = new RosterEntry();
            double v;
            Assert.That(e.TryGet(SettingsSchema.Width, out v), Is.False, "unknown until the cfg reply");
            e.ApplyConfig("[Miner]\nWidth=7\n", new List<string>());
            Assert.That(e.TryGet(SettingsSchema.Width, out v), Is.True);
            Assert.That(v, Is.EqualTo(7));
            e.MarkSent(SettingsSchema.Width, 9);
            Assert.That(e.IsPending(SettingsSchema.Width), Is.True);
            e.TryGet(SettingsSchema.Width, out v);
            Assert.That(v, Is.EqualTo(9));
            e.ApplyConfig("[Miner]\nWidth=9\n", new List<string>());
            Assert.That(e.IsPending(SettingsSchema.Width), Is.False);
        }
    }
}

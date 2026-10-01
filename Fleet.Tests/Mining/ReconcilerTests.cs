using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class ReconcilerTests
    {
        static ReconcileInput In(MinerState saved, bool connected = false, double cargo = 0, double dist = 0, bool hasJob = true)
        {
            return new ReconcileInput { Saved = saved, HomeFound = true, Connected = connected, HasJob = hasJob, CargoFill = cargo, DistToHoleEntrance = dist, ShipSize = 5 };
        }

        static MinerState R(ReconcileInput i, ReloadPolicy p, out string note, out bool redo) { return Reconciler.Resolve(i, p, out note, out redo); }

        [Test]
        public void NoHome_Holds()
        {
            string note; bool redo;
            var i = In(MinerState.RouteOut); i.HomeFound = false;
            Assert.That(R(i, ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Hold));
            Assert.That(note, Is.EqualTo(Reconciler.NoHome));
        }

        [Test]
        public void Docked_WithCargo_Unloads_ElseCharges()
        {
            string note; bool redo;
            Assert.That(R(In(MinerState.Dock, connected: true, cargo: 0.5), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Unload));
            Assert.That(R(In(MinerState.RouteBack, connected: true, cargo: 0), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Charge));
            Assert.That(note, Is.Null);
            Assert.That(R(In(MinerState.Idle, connected: true), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Idle));
            Assert.That(R(In(MinerState.Dock, connected: true, hasJob: false), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Idle));
        }

        [Test]
        public void Docked_HoldPolicy_IsIdle()
        {
            string note; bool redo;
            Assert.That(R(In(MinerState.Charge, connected: true, cargo: 0.5), ReloadPolicy.Hold, out note, out redo), Is.EqualTo(MinerState.Idle));
            Assert.That(note, Is.EqualTo(Reconciler.PolicyHold));
        }

        [Test]
        public void SafeHoldRecordingIdle_BeforeReload()
        {
            string note; bool redo;
            Assert.That(R(In(MinerState.Safe), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Hold));
            Assert.That(note, Is.EqualTo(Reconciler.WasSafe));
            Assert.That(R(In(MinerState.Hold), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Hold));
            Assert.That(note, Is.EqualTo(Reconciler.WasHold));
            Assert.That(R(In(MinerState.Recording), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Hold));
            Assert.That(note, Is.EqualTo(Reconciler.WasRecording));
            Assert.That(R(In(MinerState.Idle), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Idle));
        }

        [Test]
        public void HoldPolicy_Undocked_Holds()
        {
            string note; bool redo;
            Assert.That(R(In(MinerState.RouteOut), ReloadPolicy.Hold, out note, out redo), Is.EqualTo(MinerState.Hold));
            Assert.That(note, Is.EqualTo(Reconciler.PolicyHold));
        }

        [TestCase(MinerState.Undock, MinerState.DockPathIn)]
        [TestCase(MinerState.DockPathOut, MinerState.DockPathIn)]
        [TestCase(MinerState.Dock, MinerState.DockPathIn)]
        [TestCase(MinerState.Charge, MinerState.DockPathIn)]
        [TestCase(MinerState.RouteOut, MinerState.RouteBack)]
        [TestCase(MinerState.Drill, MinerState.RouteBack)]
        public void ReturnHomePolicy(MinerState saved, MinerState expected)
        {
            string note; bool redo;
            Assert.That(R(In(saved), ReloadPolicy.ReturnHome, out note, out redo), Is.EqualTo(expected));
        }

        [TestCase(MinerState.Undock, MinerState.DockPathOut)]
        [TestCase(MinerState.DockPathOut, MinerState.DockPathOut)]
        [TestCase(MinerState.RouteOut, MinerState.RouteOut)]
        [TestCase(MinerState.RouteBack, MinerState.RouteBack)]
        [TestCase(MinerState.DockPathIn, MinerState.DockPathIn)]
        [TestCase(MinerState.Dock, MinerState.Dock)]
        [TestCase(MinerState.Unload, MinerState.Dock)]
        [TestCase(MinerState.Charge, MinerState.Dock)]
        [TestCase(MinerState.Position, MinerState.Position)]
        public void Resume_Undocked(MinerState saved, MinerState expected)
        {
            string note; bool redo;
            Assert.That(R(In(saved), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(expected));
            Assert.That(redo, Is.False);
        }

        [Test]
        public void Resume_Drill_FarFromHole_Repositions_AndRedoes()
        {
            string note; bool redo;
            Assert.That(R(In(MinerState.Drill, dist: 40), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Position));
            Assert.That(redo, Is.True);
        }

        [Test]
        public void Resume_Drill_InsideHole_RetractsFirst_AndRedoes()
        {
            string note; bool redo;
            Assert.That(R(In(MinerState.Retract, dist: 3), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Retract));
            Assert.That(redo, Is.True);
        }

        [Test]
        public void Resume_WithoutJob_Holds()
        {
            string note; bool redo;
            Assert.That(R(In(MinerState.Drill, hasJob: false), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Hold));
            Assert.That(note, Is.EqualTo(Reconciler.NoJob));
            Assert.That(R(In(MinerState.Position, hasJob: false), ReloadPolicy.Resume, out note, out redo), Is.EqualTo(MinerState.Hold));
        }
    }
}

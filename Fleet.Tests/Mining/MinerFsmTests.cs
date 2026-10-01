using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class MinerFsmTests
    {
        Settings s;
        [SetUp] public void Init() { s = new Settings(); }

        static MinerInput Base() { return new MinerInput { HasJob = true, HasDockPath = true, Connected = false }; }

        static void Go(MinerFsm f, Settings s, MinerInput i, MinerState expected)
        {
            f.Step(i, s);
            Assert.That(f.State, Is.EqualTo(expected));
        }

        [Test]
        public void FullMissionLoop()
        {
            var f = new MinerFsm(MinerState.Idle);
            var i = Base(); i.Command = Cmd.Start; i.Connected = true;
            Go(f, s, i, MinerState.Undock);
            i = Base(); i.UndockClear = true;             Go(f, s, i, MinerState.DockPathOut);
            i = Base(); i.PathDone = true;                Go(f, s, i, MinerState.RouteOut);
            i = Base(); i.RouteDone = true;               Go(f, s, i, MinerState.Position);
            i = Base(); i.AtHole = true;                  Go(f, s, i, MinerState.Drill);
            i = Base(); i.DrillFinished = true;           Go(f, s, i, MinerState.Retract);
            i = Base(); i.RetractDone = true;             Go(f, s, i, MinerState.Position);
            i = Base(); i.AtHole = true;                  Go(f, s, i, MinerState.Drill);
            i = Base(); i.Trigger = ReturnReason.CargoFull; Go(f, s, i, MinerState.Retract);
            Assert.That(f.PendingReason, Is.EqualTo(ReturnReason.CargoFull));
            i = Base(); i.RetractDone = true;             Go(f, s, i, MinerState.RouteBack);
            i = Base(); i.RouteDone = true;               Go(f, s, i, MinerState.DockPathIn);
            i = Base(); i.PathDone = true;                Go(f, s, i, MinerState.Dock);
            i = Base(); i.Connected = true;               Go(f, s, i, MinerState.Unload);
            i = Base(); i.Connected = true; i.Unloaded = true; Go(f, s, i, MinerState.Charge);
            i = Base(); i.Connected = true; i.Charged = true;  Go(f, s, i, MinerState.Undock);
            Assert.That(f.PendingReason, Is.EqualTo(ReturnReason.None));
        }

        [Test]
        public void NoDockPath_SkipsDockPathStates()
        {
            var f = new MinerFsm(MinerState.Undock);
            var i = Base(); i.HasDockPath = false; i.UndockClear = true;
            Go(f, s, i, MinerState.RouteOut);
            f.Force(MinerState.RouteBack, null);
            i = Base(); i.HasDockPath = false; i.RouteDone = true;
            Go(f, s, i, MinerState.Dock);
        }

        [Test]
        public void Start_RequiresJobAndDock()
        {
            var f = new MinerFsm(MinerState.Idle);
            var i = Base(); i.Command = Cmd.Start; i.HasJob = false; i.Connected = true;
            Go(f, s, i, MinerState.Hold);
            Assert.That(f.Note, Is.EqualTo(MinerFsm.NoteNoJob));

            f = new MinerFsm(MinerState.Idle);
            i = Base(); i.Command = Cmd.Start;
            Go(f, s, i, MinerState.Hold);
            Assert.That(f.Note, Is.EqualTo(MinerFsm.NoteNotDocked));
        }

        [Test]
        public void Stop_ThenCont_Resumes()
        {
            var f = new MinerFsm(MinerState.RouteOut);
            var i = Base(); i.Command = Cmd.Stop;
            Assert.That(f.Step(i, s), Is.True);
            Assert.That(f.State, Is.EqualTo(MinerState.Hold));
            Assert.That(f.ResumeState, Is.EqualTo(MinerState.RouteOut));
            Assert.That(f.Note, Is.EqualTo(MinerFsm.NoteStopped));
            i = Base(); i.Command = Cmd.Cont;
            Go(f, s, i, MinerState.RouteOut);
            Assert.That(f.Note, Is.Null);
        }

        [Test]
        public void Stop_WhileIdle_DoesNothing()
        {
            var f = new MinerFsm(MinerState.Idle);
            var i = Base(); i.Command = Cmd.Stop;
            Assert.That(f.Step(i, s), Is.False);
            Assert.That(f.State, Is.EqualTo(MinerState.Idle));
        }

        [Test]
        public void Home_FromDrill_RetractsThenGoesHome()
        {
            var f = new MinerFsm(MinerState.Drill);
            var i = Base(); i.Command = Cmd.Home;
            Go(f, s, i, MinerState.Retract);
            Assert.That(f.PendingReason, Is.EqualTo(ReturnReason.Manual));
            i = Base(); i.RetractDone = true;
            Go(f, s, i, MinerState.RouteBack);
        }

        [Test]
        public void Home_FromHold_UsesResumeState()
        {
            var f = new MinerFsm(MinerState.DockPathOut);
            var i = Base(); i.Command = Cmd.Stop; f.Step(i, s);
            i = Base(); i.Command = Cmd.Home;
            Go(f, s, i, MinerState.DockPathIn);

            f = new MinerFsm(MinerState.Position);
            i = Base(); i.Command = Cmd.Stop; f.Step(i, s);
            i = Base(); i.Command = Cmd.Home;
            Go(f, s, i, MinerState.RouteBack);
        }

        [Test]
        public void TriggerWhileTravellingOut_TurnsBack()
        {
            var f = new MinerFsm(MinerState.RouteOut);
            var i = Base(); i.Trigger = ReturnReason.LowBattery;
            Go(f, s, i, MinerState.RouteBack);
            Assert.That(f.PendingReason, Is.EqualTo(ReturnReason.LowBattery));
        }

        [Test]
        public void TriggersIgnoredWhileReturning()
        {
            var f = new MinerFsm(MinerState.RouteBack);
            var i = Base(); i.Trigger = ReturnReason.CargoFull;
            Assert.That(f.Step(i, s), Is.False);
            Assert.That(f.State, Is.EqualTo(MinerState.RouteBack));
        }

        [Test]
        public void Damage_WithStopPolicy_Holds()
        {
            s.OnDamage = DamagePolicy.Stop;
            var f = new MinerFsm(MinerState.Drill);
            var i = Base(); i.Trigger = ReturnReason.Damage;
            Go(f, s, i, MinerState.Hold);
            Assert.That(f.Note, Is.EqualTo(MinerFsm.NoteDamage));
        }

        [Test]
        public void Stuck_Holds_ButNotWhileDrilling()
        {
            var f = new MinerFsm(MinerState.RouteOut);
            var i = Base(); i.Stuck = true;
            Go(f, s, i, MinerState.Hold);
            Assert.That(f.Note, Is.EqualTo(MinerFsm.NoteStuck));

            f = new MinerFsm(MinerState.Drill);
            i = Base(); i.Stuck = true;
            Go(f, s, i, MinerState.Drill);
        }

        [Test]
        public void JobComplete_GoesHome_ThenIdlesAfterCharge()
        {
            var f = new MinerFsm(MinerState.Retract);
            var i = Base(); i.RetractDone = true; i.JobComplete = true;
            Go(f, s, i, MinerState.RouteBack);
            Assert.That(f.PendingReason, Is.EqualTo(ReturnReason.JobDone));
            f.Force(MinerState.Charge, null);
            i = Base(); i.Connected = true; i.Charged = true;
            Go(f, s, i, MinerState.Idle);
        }

        [Test]
        public void LoopFalse_IdlesAfterCharge()
        {
            s.Loop = false;
            var f = new MinerFsm(MinerState.Charge);
            var i = Base(); i.Connected = true; i.Charged = true;
            Go(f, s, i, MinerState.Idle);
        }

        [Test]
        public void DockFailed_And_UraniumLow_Hold()
        {
            var f = new MinerFsm(MinerState.Dock);
            var i = Base(); i.DockFailed = true;
            Go(f, s, i, MinerState.Hold);
            Assert.That(f.Note, Is.EqualTo(MinerFsm.NoteDockFailed));

            f = new MinerFsm(MinerState.Charge);
            i = Base(); i.Connected = true; i.UraniumLow = true; i.Charged = true;
            Go(f, s, i, MinerState.Hold);
            Assert.That(f.Note, Is.EqualTo(MinerFsm.NoteUranium));
        }

        [Test]
        public void Recording_And_Reset()
        {
            var f = new MinerFsm(MinerState.Idle);
            var i = Base(); i.Command = Cmd.RecordDock;
            Go(f, s, i, MinerState.Recording);
            i = Base(); i.Command = Cmd.StopRec;
            Go(f, s, i, MinerState.Idle);

            f = new MinerFsm(MinerState.RouteOut);
            i = Base(); i.Command = Cmd.Reset;
            Go(f, s, i, MinerState.Idle);
            Assert.That(f.PendingReason, Is.EqualTo(ReturnReason.None));
        }

        [Test]
        public void JobCompleteAtPosition_GoesHome()
        {
            var f = new MinerFsm(MinerState.Position);
            var i = Base(); i.JobComplete = true;
            Go(f, s, i, MinerState.RouteBack);
            Assert.That(f.PendingReason, Is.EqualTo(ReturnReason.JobDone));
        }
    }
}

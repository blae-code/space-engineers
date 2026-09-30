# C23 — MinerFsm (the mining drone's state machine)

Milestone C · Difficulty: hard · Executor: local+review

## Goal
Decide the drone's next state from its current state, the operator's command and a set of plain
observations (supplied by the integration layer each tick). This card makes **no** game calls and drives
**no** hardware; it only decides. It is the most important logic in the drone, so the tests walk the full
mission loop.

## Files
- Create: `Fleet.Drone.Miner/Mining/MinerFsm.cs`
- Create: `Fleet.Tests/Mining/MinerFsmTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Core/Enums.cs`, `Fleet.Engine/Config/Settings.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, types nested and `public`.
- C# 6 only. No LINQ. `Step` must not allocate (notes are the string constants below).

## Interface (implement exactly)
```csharp
namespace IngameScript
{
    public partial class Program
    {
        public struct MinerInput
        {
            public Cmd Command;          // this tick's operator command (None on most ticks)
            public bool HasJob, HasDockPath, Connected;
            public ReturnReason Trigger; // from ReturnTriggers.Evaluate; None = keep working
            public bool UndockClear;     // backed off far enough from the connector
            public bool PathDone;        // dock-path follower finished (DockPathOut / DockPathIn)
            public bool RouteDone;       // job-route leg finished (RouteOut / RouteBack)
            public bool AtHole;          // at the current hole entrance and aligned
            public bool DrillFinished;   // DrillLogic switched to Retract on its own (depth reached or blocked)
            public bool RetractDone;     // DrillLogic reached Done
            public bool JobComplete;     // no holes left
            public bool DockFailed, Stuck, Unloaded, Charged, UraniumLow;
        }

        public class MinerFsm
        {
            public const string NoteStopped = "stopped by operator";
            public const string NoteNoJob = "no job defined";
            public const string NoteNotDocked = "START requires the drone to be docked";
            public const string NoteStuck = "stuck";
            public const string NoteDockFailed = "docking failed";
            public const string NoteUranium = "uranium below minimum";
            public const string NoteDamage = "damage detected";

            public MinerFsm(MinerState initial);
            public MinerState State { get; }
            public MinerState ResumeState { get; }     // where CONT returns to from Hold
            public ReturnReason PendingReason { get; } // why the drone is heading home
            public string Note { get; }                // last note (null when none)
            public void Force(MinerState s, string note);   // set State and Note directly (reconcile, RESET)
            public bool Step(MinerInput i, Settings s);      // returns true if State changed
        }
    }
}
```

## Rules for `Step` (apply the FIRST section that changes the state, then return)

Sections run in order A → B → C. A section that changes nothing (e.g. `Stop` while Idle, or no command)
falls through to the next. The constructor sets `State = initial`, `ResumeState = MinerState.Idle`,
`PendingReason = None`, `Note = null`.

Helper **GoHold(note)**: `ResumeState = State; State = Hold; Note = note`.
"Outbound" states = Undock, DockPathOut, RouteOut, Position, Drill, Retract.
"Dock-area" states = Undock, DockPathOut, DockPathIn, Dock.

**A. Commands**
- `Stop`: if State is not Idle/Hold/Safe → GoHold(`NoteStopped`).
- `Home` (sets `PendingReason = Manual` when it changes state):
  Drill → Retract · Undock or DockPathOut → DockPathIn · RouteOut or Position → RouteBack ·
  Hold → (ResumeState is a dock-area state ? DockPathIn : RouteBack) · other states: no change.
- `Start`, or `Cont` while Idle: only from Idle — `!HasJob` → GoHold(`NoteNoJob`);
  `!Connected` → GoHold(`NoteNotDocked`); else `PendingReason = None`, → Undock.
- `Cont` while Hold: if ResumeState is not Idle/Hold/Safe/Recording → State = ResumeState, `Note = null`.
- `RecordDock` / `RecordJob`: from Idle or Hold → Recording. `StopRec`: from Recording → Idle.
- `Reset`: → Idle, `PendingReason = None`, `Note = null`.

**B. Faults** (only in states other than Idle, Hold, Safe, Recording)
- `Trigger == Damage && s.OnDamage == DamagePolicy.Stop` → GoHold(`NoteDamage`).
- `Stuck` and State is not Drill → GoHold(`NoteStuck`).

**C. Per state**
| State | Condition → next (and side effects) |
|---|---|
| Undock | `UndockClear` → HasDockPath ? DockPathOut : RouteOut |
| DockPathOut | `Trigger != None` → PendingReason = Trigger, DockPathIn · else `PathDone` → RouteOut |
| RouteOut | `Trigger != None` → PendingReason = Trigger, RouteBack · else `RouteDone` → Position |
| Position | `Trigger != None` → PendingReason = Trigger, RouteBack · else `JobComplete` → PendingReason = JobDone, RouteBack · else `AtHole` → Drill |
| Drill | `Trigger != None` → PendingReason = Trigger, Retract · else `DrillFinished` → Retract |
| Retract | `RetractDone` → PendingReason != None ? RouteBack : (JobComplete ? PendingReason = JobDone, RouteBack : Position) |
| RouteBack | `RouteDone` → HasDockPath ? DockPathIn : Dock |
| DockPathIn | `PathDone` → Dock |
| Dock | `Connected` → Unload · else `DockFailed` → GoHold(`NoteDockFailed`) |
| Unload | `Unloaded` → Charge |
| Charge | `UraniumLow` → GoHold(`NoteUranium`) · else `Charged` → (PendingReason is JobDone or Damage, or `!s.Loop`) ? Idle : (PendingReason = None, Undock) |
| Idle, Hold, Safe, Recording | no automatic transitions |

## Tests — create `Fleet.Tests/Mining/MinerFsmTests.cs` verbatim
```csharp
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
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~MinerFsmTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'MinerFsm' could not be found".
- [ ] 3. Create `Fleet.Drone.Miner/Mining/MinerFsm.cs`. Structure `Step` as three private methods
      `bool ApplyCommand(...)`, `bool ApplyFaults(...)`, `bool ApplyState(...)`, called in that order,
      stopping at the first that returns true; use a `switch (State)` in `ApplyState`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Drone.Miner/Mining/MinerFsm.cs Fleet.Tests/Mining/MinerFsmTests.cs; git commit -m "feat(miner): mission state machine"`

## Done when
All MinerFsmTests pass.

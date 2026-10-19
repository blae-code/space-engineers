# C32 — Wire MiningStats into the drone and its LCD

Pilot · Difficulty: easy · Executor: local · Kind: wire · Depends on: C31

## Goal
The drone keeps a `MiningStats` for its current job, updates it every Update10 and on every state
change, saves and loads it with the rest of its Storage, resets it when a new job is defined, and shows
it as one line on the LCD once there is anything to show.

## Files
- Modify: `Fleet.Drone.Miner/Subsystems/MinerSubsystem.cs`
- Modify: `Fleet.Drone.Miner/Subsystems/UiSubsystem.cs`
- Create: `Fleet.Tests/Mining/MiningStatsSmokeTests.cs`

## Attach in Continue
This card, `Fleet.Drone.Miner/Mining/MiningStats.cs` (from C31), and the two files to modify.

## Constraints (always)
- Script code: C# 6, no LINQ, no allocation in `Update10` or `Render`.
- **Insert exactly the seven lines below and change nothing else.** Each goes on a NEW line directly
  AFTER the anchor line, with the same indentation as the anchor. Every anchor appears exactly once in
  its file. If one does not, STOP and report.

## Edits

`Fleet.Drone.Miner/Subsystems/MinerSubsystem.cs`

| # | Anchor line (unique in the file) | Insert after it |
|---|---|---|
| 1 | `public readonly MinerFsm Fsm = new MinerFsm(MinerState.Idle);` | `public readonly MiningStats Stats = new MiningStats();` |
| 2 | `input.Trigger = ReturnTriggers.Evaluate(ti, S);` | `Stats.Tick(Bb.Time, Fsm.State);` |
| 3 | `_r.Events.Add(Bb.Time, from, to, Fsm.PendingReason, Fsm.Note);` | `Stats.OnTransition(from, to, Bb.Mass);` |
| 4 | `_jobRight = Vector3D.Cross(fwd, up);` | `Stats.Reset();` |
| 5 | `PathCodec.Save(ini, "Miner.JobPath", _jobPath);` | `Stats.Save(ini, "Miner.Stats");` |
| 6 | `HoleIndex = ini.Get(n, "holeIndex").ToInt32(0);` | `Stats.Load(ini, "Miner.Stats");` |

`Fleet.Drone.Miner/Subsystems/UiSubsystem.cs`, in `Render()`:

| # | Anchor | Insert after it |
|---|---|---|
| 7 | the `sb.Append('\n');` line DIRECTLY BELOW `if (bb.HasReactor) SbFormat.AppendFixed(sb.Append("  U "), bb.UraniumKg, 1).Append("kg");` | `if (_miner.Stats.Any) _miner.Stats.Append(sb).Append('\n');` |

Known limitation (do not fix here): `Bb.Mass` is the ship's *physical* mass, so on worlds with an
inventory multiplier the ore figure is divided by that multiplier.

## Hot paths
`MinerSubsystem.Update10`, `UiSubsystem.Render`

## Tests — create `Fleet.Tests/Mining/MiningStatsSmokeTests.cs` verbatim
```csharp
using System;
using System.Runtime.Serialization;
using System.Text;
using FakeItEasy;
using NUnit.Framework;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace Fleet.Tests.Mining
{
    // C32: mining stats wired into the real drone Program (same empty-grid harness as ProgramSmokeTests).
    [TestFixture]
    public class MiningStatsSmokeTests
    {
        string _lastScreen = "";

        IngameScript.Program Build(string storage)
        {
            var me = A.Fake<IMyProgrammableBlock>();
            A.CallTo(() => me.CubeGrid).Returns(A.Fake<IMyCubeGrid>());
            me.CustomData = "";
            var surface = A.Fake<IMyTextSurface>();
            A.CallTo(() => surface.WriteText(A<StringBuilder>._, A<bool>._))
                .Invokes((StringBuilder sb, bool append) => _lastScreen = sb.ToString());
            A.CallTo(() => me.GetSurface(0)).Returns(surface);
            var program = FormatterServices.GetUninitializedObject(typeof(IngameScript.Program));
            var backend = (Sandbox.ModAPI.IMyGridProgram)program;
            backend.Runtime = A.Fake<IMyGridProgramRuntimeInfo>();
            backend.Echo = s => { };
            backend.Me = me;
            backend.Storage = storage;
            backend.GridTerminalSystem = A.Fake<IMyGridTerminalSystem>();
            var igc = A.Fake<IMyIntergridCommunicationSystem>();
            backend.IGC_ContextGetter = () => igc;
            typeof(IngameScript.Program).GetConstructor(Type.EmptyTypes).Invoke(program, null);
            return (IngameScript.Program)program;
        }

        const string Saved = "[Miner]\nv=3\nstate=0\n[Miner.Stats]\ntrips=2\noreKg=1500\njobSeconds=1800\n";

        [Test]
        public void NoStats_NoStatsLine()
        {
            Build("").Main("", UpdateType.Update10 | UpdateType.Update100);
            Assert.That(_lastScreen, Does.Not.Contain("trips "));
        }

        [Test]
        public void SavedStats_AreShownOnTheLcd()
        {
            Build(Saved).Main("", UpdateType.Update10 | UpdateType.Update100);
            Assert.That(_lastScreen, Does.Contain("trips 2  ore 1500kg  3000kg/h  0.5h"));
        }

        [Test]
        public void SavedStats_SurviveSaveAndReload()
        {
            var p = Build(Saved);
            p.Main("", UpdateType.Update10 | UpdateType.Update100);
            p.Save();
            var storage = ((Sandbox.ModAPI.IMyGridProgram)p).Storage;
            Assert.That(storage, Does.Contain("[Miner.Stats]"));
            Assert.That(storage, Does.Contain("trips=2"));
            Build(storage).Main("", UpdateType.Update10 | UpdateType.Update100);
            Assert.That(_lastScreen, Does.Contain("trips 2  ore 1500kg"));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~MiningStatsSmokeTests" 2>&1 | tail -n 25`
      Expected: `Failed!` — Failed: 2 (`SavedStats_AreShownOnTheLcd`, `SavedStats_SurviveSaveAndReload`), Passed: 1.
- [ ] 3. Make edits 1–7.
- [ ] 4. Run the same command. Expected: `Passed!` — Failed: 0, Passed: 3.
- [ ] 5. Run `dotnet test Fleet.Tests 2>&1 | tail -n 3`. Expected: `Passed!`, Failed: 0.
- [ ] 6. Run `python3 tools/card-check.py C32`. Expected: `RESULT: PASS`.

## Done when
All MiningStatsSmokeTests pass and the full suite passes.

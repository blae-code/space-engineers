# C22 — Reconciler (what to do after a world reload)

Milestone C · Difficulty: medium · Executor: local

## Goal
After a reload the saved state may not match reality (the save can be older than the last transition).
Compare the saved state with what the drone observes now and pick a safe state to continue in — or
**Hold** with a reason. It never picks a state that moves the drone blindly.

## Files
- Create: `Fleet.Drone.Miner/Mining/Reconciler.cs`
- Create: `Fleet.Tests/Mining/ReconcilerTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Core/Enums.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, types nested and `public`.
- C# 6 only. No LINQ. Notes are the string constants below (no string building).

## Interface (implement exactly)
```csharp
namespace IngameScript
{
    public partial class Program
    {
        public struct ReconcileInput
        {
            public MinerState Saved;
            public bool HomeFound, Connected, HasJob;
            public double CargoFill;            // 0..1
            public double DistToHoleEntrance;   // m, from the drill face to the current hole's entrance
            public double ShipSize;             // m
        }

        public static class Reconciler
        {
            public const string NoHome = "home connector not found";
            public const string PolicyHold = "reload policy: Hold";
            public const string NoJob = "no job defined";
            public const string WasRecording = "recording interrupted by reload";
            public const string WasSafe = "was SAFE before reload";
            public const string WasHold = "was holding before reload";
            // note: null unless one of the constants applies. redoHole: true when the current hole must be re-drilled from its entrance.
            public static MinerState Resolve(ReconcileInput i, ReloadPolicy policy, out string note, out bool redoHole);
        }
    }
}
```

## Rules (first match wins; `note = null`, `redoHole = false` unless stated)
1. `!HomeFound` → **Hold**, note `NoHome`.
2. `Connected`:
   - policy `Hold` → **Idle**, note `PolicyHold`;
   - else `CargoFill > 0.01` → **Unload**;
   - else `HasJob` and `Saved` is not Idle/Hold/Safe/Recording → **Charge**;
   - else → **Idle**.
3. `Saved == Safe` → **Hold**, `WasSafe`. `Saved == Hold` → **Hold**, `WasHold`.
   `Saved == Recording` → **Hold**, `WasRecording`. `Saved == Idle` → **Idle**.
4. policy `Hold` → **Hold**, `PolicyHold`.
5. policy `ReturnHome` → `Saved` in {Undock, DockPathOut, DockPathIn, Dock, Unload, Charge} → **DockPathIn**;
   otherwise → **RouteBack**.
6. policy `Resume`:
   - Undock → **DockPathOut**; DockPathOut / RouteOut / RouteBack / DockPathIn → the **same** state;
   - Dock / Unload / Charge (not connected) → **Dock**;
   - Position → `HasJob` ? **Position** : **Hold** `NoJob`;
   - Drill / Retract → `!HasJob` → **Hold** `NoJob`; else `redoHole = true` and
     `DistToHoleEntrance > 2 * ShipSize` → **Position**, otherwise → **Retract**.

(Spec §4's example "saved RouteBack, cargo empty → Idle/Charge" is rule 2 when docked; undocked, the drone
simply keeps returning.)

## Tests — create `Fleet.Tests/Mining/ReconcilerTests.cs` verbatim
```csharp
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
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~ReconcilerTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'Reconciler' does not exist".
- [ ] 3. Create `Fleet.Drone.Miner/Mining/Reconciler.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Drone.Miner/Mining/Reconciler.cs Fleet.Tests/Mining/ReconcilerTests.cs; git commit -m "feat(miner): reload reconciler"`

## Done when
All ReconcilerTests pass.

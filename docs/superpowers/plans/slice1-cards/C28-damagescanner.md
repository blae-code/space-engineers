# C28 — DamageScanner

Milestone D · Difficulty: medium · Executor: local+review

## Goal
Detect damage by comparing the drone's functional block count and total integrity against a baseline taken
at `SETJOB`. The scan is **amortised** across ticks (a slice of blocks per call) so large drones never spike.

## Files
- Create: `Fleet.Drone.Miner/Io/DamageScanner.cs`
- Create: `Fleet.Tests/Mining/DamageScannerTests.cs` (pure helper only)

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. `Refresh` may allocate; `Step` must not.

## Space Engineers API you need
```csharp
using Sandbox.ModAPI.Ingame; using VRage.Game.ModAPI.Ingame;
IMyTerminalBlock b; b.Closed (bool: destroyed/removed); b.IsFunctional;
IMySlimBlock slim = b.CubeGrid.GetCubeBlock(b.Position);   // null if gone
slim.BuildIntegrity, slim.MaxIntegrity (float)
```
(Only terminal blocks can be enumerated by scripts; armour damage is therefore not seen. That is accepted
for slice 1.)

## Interface (implement exactly)
```csharp
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program
    {
        public class DamageScanner
        {
            public const int BlocksPerStep = 20;
            public const double IntegrityThreshold = 0.98;  // damaged below 98 % of baseline integrity
            public DamageScanner();
            public void Refresh(List<IMyTerminalBlock> blocks);  // keeps the list reference
            public bool HasBaseline { get; }
            public void TakeBaseline();          // full synchronous pass; called on SETJOB (rare)
            // Scans the next BlocksPerStep blocks; after a full pass, updates Damaged. Returns Damaged.
            public bool Step();
            public bool Damaged { get; }
            // pure helper (unit-tested)
            public static bool IsDamaged(int baseCount, double baseIntegrity, int count, double integrity);
        }
    }
}
```

## Behaviour
- A pass counts blocks that are not `Closed`, are `IsFunctional`, and whose slim block is non-null, and sums
  `BuildIntegrity`. `IsDamaged` = `count < baseCount || integrity < baseIntegrity * IntegrityThreshold`.
- No baseline → `Step` returns false and does nothing.
- `Refresh` restarts any scan pass in progress (index and partial sums reset); the baseline is kept.

## Tests — create `Fleet.Tests/Mining/DamageScannerTests.cs` verbatim
```csharp
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class DamageScannerTests
    {
        [Test]
        public void IsDamaged()
        {
            Assert.That(DamageScanner.IsDamaged(10, 1000, 10, 1000), Is.False);
            Assert.That(DamageScanner.IsDamaged(10, 1000, 9, 1000), Is.True);
            Assert.That(DamageScanner.IsDamaged(10, 1000, 10, 981), Is.False);
            Assert.That(DamageScanner.IsDamaged(10, 1000, 10, 979), Is.True);
        }

        [Test]
        public void NoBaseline_NeverDamaged()
        {
            var d = new DamageScanner();
            Assert.That(d.HasBaseline, Is.False);
            Assert.That(d.Step(), Is.False);
        }
    }
}
```

## Steps
- [ ] 1. Create the test file; run `dotnet test Fleet.Tests --filter "FullyQualifiedName~DamageScannerTests" 2>&1 | tail -n 25` → build error.
- [ ] 2. Create `Fleet.Drone.Miner/Io/DamageScanner.cs`.
- [ ] 3. Run the same command → `Passed!`.
- [ ] 4. `dotnet build Fleet.Drone.Miner -c Release 2>&1 | tail -n 15` → "successfully deployed".
- [ ] 5. Commit: `git add Fleet.Drone.Miner/Io/DamageScanner.cs Fleet.Tests/Mining/DamageScannerTests.cs; git commit -m "feat(miner): damage scanner"`

## Done when
Tests pass and the Release build is clean. In-game: checklist G9.

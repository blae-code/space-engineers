# C20 — DrillLogic (one hole: advance, stall handling, retract)

Milestone C · Difficulty: medium · Executor: local

## Goal
Drive one hole: advance at the work speed until the target depth; if progress stalls, halve the feed; if it
stays stalled, give up on the hole (**blocked**) instead of grinding forever; then retract to the entrance.

## Files
- Create: `Fleet.Drone.Miner/Mining/DrillLogic.cs`
- Create: `Fleet.Tests/Mining/DrillLogicTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. No allocation.

## Interface (implement exactly)
```csharp
namespace IngameScript
{
    public partial class Program
    {
        public class DrillLogic
        {
            public enum Phase { Advance, Retract, Done }
            public const double StallRatio = 0.1;          // stalled when forward speed < StallRatio * Feed
            public const double StallHalveSeconds = 3;     // first stall: halve the feed after this long
            public const double StallBlockSeconds = 5;     // after halving: blocked after this long stalled
            public const double RetractDoneDepth = 0.5;    // retract finished at or above this depth (m)
            public Phase Current { get; }
            public double Feed { get; }                    // current advance speed (m/s)
            public bool WasBlocked { get; }
            public void Start(double targetDepth, double workSpeed);  // Advance, Feed = workSpeed, clears flags
            public void BeginRetract();                               // switch to Retract now (e.g. cargo full)
            // depth: metres below the hole entrance along the mining direction. fwdSpeed: velocity along it.
            public Phase Update(double now, double depth, double fwdSpeed);
        }
    }
}
```

## Behaviour — `Update`
- **Advance:**
  1. `depth >= targetDepth` → Retract.
  2. Else stalled = `fwdSpeed < StallRatio * Feed`. If not stalled: stop the stall timer. If stalled and the
     timer is not running: start it at `now`. If stalled and running, `elapsed = now - since`:
     - not yet halved and `elapsed >= StallHalveSeconds` → `Feed /= 2`, mark halved, restart the timer at `now`;
     - already halved and `elapsed >= StallBlockSeconds` → `WasBlocked = true`, → Retract.
- **Retract:** `depth <= RetractDoneDepth` → Done.
- **Done:** stays Done.
- Returns `Current`.

## Tests — create `Fleet.Tests/Mining/DrillLogicTests.cs` verbatim
```csharp
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class DrillLogicTests
    {
        [Test]
        public void ReachesDepth_ThenRetracts_ThenDone()
        {
            var d = new DrillLogic();
            d.Start(10, 1.5);
            Assert.That(d.Update(0, 5, 1.5), Is.EqualTo(DrillLogic.Phase.Advance));
            Assert.That(d.Update(1, 10, 1.5), Is.EqualTo(DrillLogic.Phase.Retract));
            Assert.That(d.Update(2, 3, -4), Is.EqualTo(DrillLogic.Phase.Retract));
            Assert.That(d.Update(3, 0.4, -4), Is.EqualTo(DrillLogic.Phase.Done));
            Assert.That(d.WasBlocked, Is.False);
        }

        [Test]
        public void Stall_HalvesFeed_ThenBlocks()
        {
            var d = new DrillLogic();
            d.Start(10, 1.5);
            d.Update(0, 1, 0);
            d.Update(2.9, 1, 0);
            Assert.That(d.Feed, Is.EqualTo(1.5));
            d.Update(3.0, 1, 0);
            Assert.That(d.Feed, Is.EqualTo(0.75));
            Assert.That(d.Update(7.9, 1, 0), Is.EqualTo(DrillLogic.Phase.Advance));
            Assert.That(d.Update(8.0, 1, 0), Is.EqualTo(DrillLogic.Phase.Retract));
            Assert.That(d.WasBlocked, Is.True);
        }

        [Test]
        public void Recovery_KeepsHalvedFeed_AndResetsTimer()
        {
            var d = new DrillLogic();
            d.Start(10, 1.5);
            d.Update(0, 1, 0);
            d.Update(3, 1, 0);               // halved
            d.Update(4, 1.2, 0.5);           // moving again (0.5 >= 0.075)
            Assert.That(d.Feed, Is.EqualTo(0.75));
            d.Update(20, 2, 0);              // stalls again: timer restarts at 20
            Assert.That(d.Update(24.9, 2, 0), Is.EqualTo(DrillLogic.Phase.Advance));
            Assert.That(d.Update(25, 2, 0), Is.EqualTo(DrillLogic.Phase.Retract));
        }

        [Test]
        public void BeginRetract_External()
        {
            var d = new DrillLogic();
            d.Start(10, 1.5);
            d.BeginRetract();
            Assert.That(d.Current, Is.EqualTo(DrillLogic.Phase.Retract));
            Assert.That(d.Update(1, 0.2, -3), Is.EqualTo(DrillLogic.Phase.Done));
            Assert.That(d.WasBlocked, Is.False);
        }

        [Test]
        public void Start_ResetsEverything()
        {
            var d = new DrillLogic();
            d.Start(10, 1.5);
            d.Update(0, 1, 0); d.Update(3, 1, 0); d.Update(8, 1, 0);
            d.Start(20, 2);
            Assert.That(d.Current, Is.EqualTo(DrillLogic.Phase.Advance));
            Assert.That(d.Feed, Is.EqualTo(2));
            Assert.That(d.WasBlocked, Is.False);
            Assert.That(d.Update(9, 1, 0), Is.EqualTo(DrillLogic.Phase.Advance));  // stall timer restarted
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~DrillLogicTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'DrillLogic' could not be found".
- [ ] 3. Create `Fleet.Drone.Miner/Mining/DrillLogic.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Drone.Miner/Mining/DrillLogic.cs Fleet.Tests/Mining/DrillLogicTests.cs; git commit -m "feat(miner): drill logic with stall handling"`

## Done when
All DrillLogicTests pass.

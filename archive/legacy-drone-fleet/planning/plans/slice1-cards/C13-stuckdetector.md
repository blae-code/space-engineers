# C13 — StuckDetector

Milestone B · Difficulty: easy · Executor: local

## Goal
Detect a ship that is pushing but not getting anywhere: thrusting while its speed toward the target stays
below a threshold for `stuckSeconds` without a break.

## Files
- Create: `Fleet.Flight/Flight/StuckDetector.cs`
- Create: `Fleet.Tests/Flight/StuckDetectorTests.cs`

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
        public class StuckDetector
        {
            public StuckDetector(double stuckSeconds, double minSpeed);   // e.g. (5, 0.2)
            public double StuckSeconds, MinSpeed;
            // progressSpeed: velocity component toward the target (m/s). thrusting: the helm is commanding motion.
            // Returns true once (thrusting && progressSpeed < MinSpeed) has held continuously for >= StuckSeconds.
            public bool Update(double now, double progressSpeed, bool thrusting);
            public void Reset();
        }
    }
}
```

## Behaviour
- Keep `bool timing` and `double since`. When the stuck condition holds and not timing: start timing at
  `now`. When it holds and timing: stuck if `now - since >= StuckSeconds`. When it does not hold: stop timing.
- `Reset()` stops timing.

## Tests — create `Fleet.Tests/Flight/StuckDetectorTests.cs` verbatim
```csharp
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class StuckDetectorTests
    {
        [Test]
        public void StuckAfterTimeout()
        {
            var s = new StuckDetector(5, 0.2);
            Assert.That(s.Update(0, 0, true), Is.False);
            Assert.That(s.Update(4.9, 0.1, true), Is.False);
            Assert.That(s.Update(5.0, 0, true), Is.True);
        }

        [Test]
        public void ProgressRestartsTheClock()
        {
            var s = new StuckDetector(5, 0.2);
            s.Update(0, 0, true);
            s.Update(3, 1.0, true);
            Assert.That(s.Update(7, 0, true), Is.False);
            Assert.That(s.Update(11.9, 0, true), Is.False);
            Assert.That(s.Update(12, 0, true), Is.True);
        }

        [Test]
        public void NotThrusting_IsNeverStuck()
        {
            var s = new StuckDetector(5, 0.2);
            s.Update(0, 0, true);
            Assert.That(s.Update(10, 0, false), Is.False);
            Assert.That(s.Update(11, 0, true), Is.False);
        }

        [Test]
        public void Reset_Clears()
        {
            var s = new StuckDetector(5, 0.2);
            s.Update(0, 0, true);
            s.Reset();
            Assert.That(s.Update(6, 0, true), Is.False);
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~StuckDetectorTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'StuckDetector' could not be found".
- [ ] 3. Create `Fleet.Flight/Flight/StuckDetector.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Flight/Flight/StuckDetector.cs Fleet.Tests/Flight/StuckDetectorTests.cs; git commit -m "feat(flight): stuck detector"`

## Done when
All StuckDetectorTests pass.

# C10 — BrakingCurve (safe approach speed)

Milestone B · Difficulty: easy · Executor: local

## Goal
The helm never flies faster than it can stop. Given the distance left and the deceleration available,
return the highest safe speed, and the distance needed to stop from a given speed.

## Files
- Create: `Fleet.Flight/Flight/BrakingCurve.cs`
- Create: `Fleet.Tests/Flight/BrakingCurveTests.cs`

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
        public static class BrakingCurve
        {
            // min(cap, sqrt(2 * aBrake * dist) * margin). Returns 0 if dist <= 0, aBrake <= 0 or cap <= 0.
            public static double SafeSpeed(double dist, double aBrake, double cap, double margin);
            // speed^2 / (2 * aBrake). Returns double.PositiveInfinity if aBrake <= 0 (cannot stop). Speed sign ignored.
            public static double StopDistance(double speed, double aBrake);
        }
    }
}
```

## Tests — create `Fleet.Tests/Flight/BrakingCurveTests.cs` verbatim
```csharp
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class BrakingCurveTests
    {
        [TestCase(50, 1, 100, 1, 10)]
        [TestCase(50, 1, 5, 1, 5)]
        [TestCase(50, 1, 100, 0.8, 8)]
        [TestCase(0, 1, 100, 1, 0)]
        [TestCase(-3, 1, 100, 1, 0)]
        [TestCase(50, 0, 100, 1, 0)]
        [TestCase(50, -2, 100, 1, 0)]
        [TestCase(50, 1, 0, 1, 0)]
        public void SafeSpeed(double dist, double a, double cap, double margin, double expected)
        {
            Assert.That(BrakingCurve.SafeSpeed(dist, a, cap, margin), Is.EqualTo(expected).Within(1e-9));
        }

        [Test]
        public void StopDistance()
        {
            Assert.That(BrakingCurve.StopDistance(10, 2), Is.EqualTo(25).Within(1e-9));
            Assert.That(BrakingCurve.StopDistance(-10, 2), Is.EqualTo(25).Within(1e-9));
            Assert.That(BrakingCurve.StopDistance(0, 2), Is.EqualTo(0).Within(1e-9));
            Assert.That(double.IsPositiveInfinity(BrakingCurve.StopDistance(5, 0)), Is.True);
        }

        [Test]
        public void SafeSpeed_IsConsistentWithStopDistance()
        {
            var v = BrakingCurve.SafeSpeed(80, 4, 1000, 1);
            Assert.That(BrakingCurve.StopDistance(v, 4), Is.EqualTo(80).Within(1e-6));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~BrakingCurveTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'BrakingCurve' does not exist".
- [ ] 3. Create `Fleet.Flight/Flight/BrakingCurve.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Flight/Flight/BrakingCurve.cs Fleet.Tests/Flight/BrakingCurveTests.cs; git commit -m "feat(flight): braking curve"`

## Done when
All BrakingCurveTests pass.

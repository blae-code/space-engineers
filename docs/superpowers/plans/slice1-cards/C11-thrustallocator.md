# C11 — ThrustAllocator (force → thruster override per direction)

Milestone B · Difficulty: medium · Executor: local

## Goal
The helm computes the force it wants in **ship-local** coordinates. This card splits that force across the
six thruster groups (one per direction the ship can be pushed) as override fractions, and answers "how much
force can the ship produce along this direction right now?" (used for braking).

## Files
- Create: `Fleet.Flight/Flight/ThrustAllocator.cs`
- Create: `Fleet.Tests/Flight/ThrustAllocatorTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. **No allocation** (arrays are passed in).

## Space Engineers facts you need
- Ship-local axes: Right = +X, Up = +Y, **Forward = −Z**, Backward = +Z.
- Groups are indexed by `(int)Base6Directions.Direction`: **Forward=0, Backward=1, Left=2, Right=3, Up=4,
  Down=5**. Group *g* holds the thrusters that push the ship **toward** direction *g*.
  So a force with X > 0 needs group Right(3), X < 0 → Left(2), Y > 0 → Up(4), Y < 0 → Down(5),
  Z > 0 → Backward(1), Z < 0 → Forward(0).
- A thruster override of exactly 0 means "not overridden", and then the inertia dampeners fire it. To keep
  every thruster under script control while the helm flies, the smallest override written is `MinOverride`.

## Interface (implement exactly)
```csharp
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public static class ThrustAllocator
        {
            public const double MinOverride = 1e-5;
            // localForce in newtons (ship-local). maxThrust[6]: newtons each group can push now.
            // Writes ratios[6] in [MinOverride, 1]. Returns saturation = max over used axes of |F_axis| / maxThrust[group]
            // (0 for a zero force; double.PositiveInfinity if a needed group has maxThrust <= 0).
            public static double Allocate(Vector3D localForce, double[] maxThrust, double[] ratios);
            // Largest force magnitude achievable along the unit ship-local direction localDir:
            // min over components with |d_i| > 1e-9 of maxThrust[group(d_i)] / |d_i|. 0 if localDir is ~zero or any needed group is 0.
            public static double MaxForceAlong(Vector3D localDir, double[] maxThrust);
        }
    }
}
```

## Behaviour
- `Allocate`: set all six ratios to `MinOverride`; for each axis with a non-zero component pick the group
  per the table above; if its `maxThrust <= 0` the saturation becomes +∞ and that ratio stays `MinOverride`;
  otherwise `r = |F| / max`, saturation = max(saturation, r), `ratios[g] = Math.Max(MinOverride, Math.Min(1, r))`.

## Tests — create `Fleet.Tests/Flight/ThrustAllocatorTests.cs` verbatim
```csharp
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class ThrustAllocatorTests
    {
        const int F = 0, B = 1, L = 2, R = 3, U = 4, D = 5;
        static double[] All(double v) { return new[] { v, v, v, v, v, v }; }
        const double Min = ThrustAllocator.MinOverride;

        [Test]
        public void UpForce_UsesUpGroup_OthersMin()
        {
            var ratios = new double[6];
            var sat = ThrustAllocator.Allocate(new Vector3D(0, 500, 0), All(1000), ratios);
            Assert.That(ratios[U], Is.EqualTo(0.5).Within(1e-9));
            Assert.That(ratios[D], Is.EqualTo(Min));
            Assert.That(ratios[F], Is.EqualTo(Min));
            Assert.That(sat, Is.EqualTo(0.5).Within(1e-9));
        }

        [Test]
        public void MinusZ_IsForwardGroup()
        {
            var ratios = new double[6];
            ThrustAllocator.Allocate(new Vector3D(0, 0, -300), All(1000), ratios);
            Assert.That(ratios[F], Is.EqualTo(0.3).Within(1e-9));
            Assert.That(ratios[B], Is.EqualTo(Min));
        }

        [Test]
        public void Saturates_AndClampsToOne()
        {
            var ratios = new double[6];
            var sat = ThrustAllocator.Allocate(new Vector3D(-2000, 0, 0), All(1000), ratios);
            Assert.That(ratios[L], Is.EqualTo(1));
            Assert.That(ratios[R], Is.EqualTo(Min));
            Assert.That(sat, Is.EqualTo(2).Within(1e-9));
        }

        [Test]
        public void ZeroForce_AllMin_ZeroSaturation()
        {
            var ratios = new double[6];
            var sat = ThrustAllocator.Allocate(Vector3D.Zero, All(1000), ratios);
            foreach (var r in ratios) Assert.That(r, Is.EqualTo(Min));
            Assert.That(sat, Is.EqualTo(0));
        }

        [Test]
        public void ZeroCapacityGroup()
        {
            var max = All(1000); max[B] = 0;
            var ratios = new double[6];
            var sat = ThrustAllocator.Allocate(new Vector3D(0, 0, 50), max, ratios);
            Assert.That(double.IsPositiveInfinity(sat), Is.True);
            Assert.That(ratios[B], Is.EqualTo(Min));
        }

        [Test]
        public void MultiAxis()
        {
            var ratios = new double[6];
            var max = new[] { 1000.0, 1000, 1000, 2000, 4000, 1000 };
            var sat = ThrustAllocator.Allocate(new Vector3D(1000, 1000, 0), max, ratios);
            Assert.That(ratios[R], Is.EqualTo(0.5).Within(1e-9));
            Assert.That(ratios[U], Is.EqualTo(0.25).Within(1e-9));
            Assert.That(sat, Is.EqualTo(0.5).Within(1e-9));
        }

        [Test]
        public void MaxForceAlong()
        {
            var max = new[] { 1000.0, 1000, 1000, 1000, 500, 1000 };
            Assert.That(ThrustAllocator.MaxForceAlong(new Vector3D(0, 1, 0), max), Is.EqualTo(500).Within(1e-9));
            var diag = Vector3D.Normalize(new Vector3D(1, 1, 0));
            Assert.That(ThrustAllocator.MaxForceAlong(diag, max), Is.EqualTo(500 / diag.Y).Within(1e-6));
            Assert.That(ThrustAllocator.MaxForceAlong(Vector3D.Zero, max), Is.EqualTo(0));
            max[F] = 0;
            Assert.That(ThrustAllocator.MaxForceAlong(new Vector3D(0, 0, -1), max), Is.EqualTo(0));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~ThrustAllocatorTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'ThrustAllocator' does not exist".
- [ ] 3. Create `Fleet.Flight/Flight/ThrustAllocator.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Flight/Flight/ThrustAllocator.cs Fleet.Tests/Flight/ThrustAllocatorTests.cs; git commit -m "feat(flight): thrust allocator"`

## Done when
All ThrustAllocatorTests pass.

# C19 — HoleGrid (hole order and spacing)

Milestone C · Difficulty: medium · Executor: local

## Goal
A mining job is a grid of holes, Width × Height, around the job origin. Mine them in a **centre-out
spiral** (the point you aimed at comes first), space them from the drone's actual drill layout, and
compute each hole's entrance position.

## Files
- Create: `Fleet.Drone.Miner/Mining/HoleGrid.cs`
- Create: `Fleet.Tests/Mining/HoleGridTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. `Spiral` fills a caller-provided list (it may be called at job setup; no need to be allocation-free, but do not create lists inside).
- `Vector2I` (integer X, Y) and `Vector3D` are in `VRageMath`.

## Interface (implement exactly)
```csharp
using System.Collections.Generic;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public static class HoleGrid
        {
            public const double DrillRadius = 1.4;   // small-grid drill cutting radius, m
            public const double Overlap = 0.9;       // holes overlap slightly so no ridges are left
            // Odd n: -(n-1)/2 .. (n-1)/2.  Even n: -n/2 .. n/2-1 (centre biased low).
            public static void Bounds(int n, out int min, out int max);
            // Clears 'order' and fills it with width*height distinct cells in centre-out spiral order.
            public static void Spiral(int width, int height, List<Vector2I> order);
            // origin + right * cell.X * spacing + up * cell.Y * spacing
            public static Vector3D HoleEntrance(Vector3D origin, Vector3D right, Vector3D up, Vector2I cell, double spacing);
            // Drill positions in ship-local coordinates (drills face ship-forward, so X/Y span the cutting face):
            // max(spanX, spanY) + 2 * DrillRadius * Overlap, where span = max - min of that coordinate.
            // Empty list -> 2 * DrillRadius * Overlap.
            public static double SpacingFromDrills(List<Vector3D> drillPositionsShipLocal);
        }
    }
}
```

## Spiral algorithm (use exactly this, the tests depend on the order)
Start at (0,0) and emit it if inside the bounds. Then walk: direction sequence **+X, +Y, −X, −Y**
repeating; step lengths **1, 1, 2, 2, 3, 3, 4, 4, …** (each length used for two consecutive directions).
After every single step, emit the cell if it lies inside the bounds from `Bounds(width)` / `Bounds(height)`.
Stop as soon as `width * height` cells have been emitted.

Example 3×3: (0,0) (1,0) (1,1) (0,1) (−1,1) (−1,0) (−1,−1) (0,−1) (1,−1).

## Tests — create `Fleet.Tests/Mining/HoleGridTests.cs` verbatim
```csharp
using System.Collections.Generic;
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class HoleGridTests
    {
        static List<Vector2I> Run(int w, int h) { var l = new List<Vector2I>(); HoleGrid.Spiral(w, h, l); return l; }
        static Vector2I V(int x, int y) { return new Vector2I(x, y); }

        [Test]
        public void Bounds()
        {
            int min, max;
            HoleGrid.Bounds(3, out min, out max); Assert.That(min, Is.EqualTo(-1)); Assert.That(max, Is.EqualTo(1));
            HoleGrid.Bounds(4, out min, out max); Assert.That(min, Is.EqualTo(-2)); Assert.That(max, Is.EqualTo(1));
            HoleGrid.Bounds(1, out min, out max); Assert.That(min, Is.EqualTo(0)); Assert.That(max, Is.EqualTo(0));
        }

        [Test]
        public void Spiral3x3()
        {
            Assert.That(Run(3, 3), Is.EqualTo(new[] { V(0,0), V(1,0), V(1,1), V(0,1), V(-1,1), V(-1,0), V(-1,-1), V(0,-1), V(1,-1) }));
        }

        [Test]
        public void Spiral2x2()
        {
            Assert.That(Run(2, 2), Is.EqualTo(new[] { V(0,0), V(-1,0), V(-1,-1), V(0,-1) }));
        }

        [Test]
        public void Spiral1x5_Column()
        {
            Assert.That(Run(1, 5), Is.EqualTo(new[] { V(0,0), V(0,1), V(0,-1), V(0,2), V(0,-2) }));
        }

        [Test]
        public void Spiral4x1_Row()
        {
            Assert.That(Run(4, 1), Is.EqualTo(new[] { V(0,0), V(1,0), V(-1,0), V(-2,0) }));
        }

        [Test]
        public void Spiral_LargeGrid_CoversEveryCellOnce()
        {
            var l = Run(7, 5);
            Assert.That(l.Count, Is.EqualTo(35));
            Assert.That(l, Is.Unique);
            Assert.That(l[0], Is.EqualTo(V(0, 0)));
        }

        [Test]
        public void Spiral_ClearsListFirst()
        {
            var l = new List<Vector2I> { V(9, 9) };
            HoleGrid.Spiral(1, 1, l);
            Assert.That(l, Is.EqualTo(new[] { V(0, 0) }));
        }

        [Test]
        public void HoleEntrance()
        {
            var p = HoleGrid.HoleEntrance(new Vector3D(10, 0, 0), new Vector3D(1, 0, 0), new Vector3D(0, 1, 0), V(2, -1), 2.5);
            Assert.That(Vector3D.Distance(p, new Vector3D(15, -2.5, 0)), Is.LessThan(1e-9));
        }

        [Test]
        public void SpacingFromDrills()
        {
            Assert.That(HoleGrid.SpacingFromDrills(new List<Vector3D>()), Is.EqualTo(2.52).Within(1e-9));
            Assert.That(HoleGrid.SpacingFromDrills(new List<Vector3D> { new Vector3D(0, 0, -3) }), Is.EqualTo(2.52).Within(1e-9));
            var pair = new List<Vector3D> { new Vector3D(-1.25, 0, -3), new Vector3D(1.25, 0.5, -3) };
            Assert.That(HoleGrid.SpacingFromDrills(pair), Is.EqualTo(5.02).Within(1e-9));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~HoleGridTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'HoleGrid' does not exist".
- [ ] 3. Create `Fleet.Drone.Miner/Mining/HoleGrid.cs`. Guard the spiral loop with a maximum step count
      (e.g. `(Math.Max(width, height) + 2) * (Math.Max(width, height) + 2) * 4`) so bad input cannot hang.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Drone.Miner/Mining/HoleGrid.cs Fleet.Tests/Mining/HoleGridTests.cs; git commit -m "feat(miner): hole grid spiral and spacing"`

## Done when
All HoleGridTests pass.

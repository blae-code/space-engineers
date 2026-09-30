# C15 — PathFollower

Milestone B · Difficulty: medium · Executor: local

## Goal
Fly a recorded path forwards (outbound) or in reverse (home), waypoint by waypoint. Each update returns
the current waypoint, a speed cap that slows the ship before sharp corners, and the distance remaining
along the path (the helm brakes against this, not against the next waypoint, so it doesn't stop at every
point). Resuming after a reload starts from the **nearest** point.

## Files
- Create: `Fleet.Flight/Flight/PathFollower.cs`
- Create: `Fleet.Tests/Flight/PathFollowerTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. **No allocation** (keep a reference to the caller's list; never copy it).

## Interface (implement exactly)
```csharp
using System.Collections.Generic;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class PathFollower
        {
            public const double CornerFreeAngle = 0.349;  // radians (20 degrees): no slowdown below this turn
            public const double MinCornerSpeed = 3;       // m/s
            public void Start(List<Vector3D> path, bool reverse);                       // Index = 0
            public void StartNearest(List<Vector3D> path, bool reverse, Vector3D localPos); // Index = traversal index of the nearest point
            public bool Done { get; }
            public int Index { get; }   // traversal index of the current waypoint
            // Returns the current waypoint (local frame).
            public Vector3D Update(Vector3D localPos, double reachDist, double maxSpeed, out double speedCap, out double remaining);
        }
    }
}
```

## Behaviour
- **Traversal order:** traversal index *i* maps to path index `reverse ? Count - 1 - i : i`. Let `P(i)` be
  the point at traversal index *i*, and `last = Count - 1`.
- Empty path: `Done = true` after `Start`/`StartNearest`; `Update` returns `localPos`, `speedCap = 0`,
  `remaining = 0`.
- `Update`:
  1. While `Index < last` and `Distance(localPos, P(Index)) <= reachDist`: `Index++`.
  2. If `Index == last` and `Distance(localPos, P(last)) <= reachDist`: `Done = true`.
  3. `remaining = Distance(localPos, P(Index)) + Σ Distance(P(k), P(k+1))` for k = Index .. last-1.
  4. `speedCap`: if `Index < last`: incoming = `P(Index) - (Index > 0 ? P(Index-1) : localPos)`,
     outgoing = `P(Index+1) - P(Index)`; θ = angle between them (0 if either is shorter than 1e-6);
     `speedCap = θ < CornerFreeAngle ? maxSpeed : Math.Max(MinCornerSpeed, maxSpeed * (1 - θ / Math.PI))`.
     If `Index == last`: `speedCap = maxSpeed`.
  5. Return `P(Index)`.

## Tests — create `Fleet.Tests/Flight/PathFollowerTests.cs` verbatim
```csharp
using System.Collections.Generic;
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class PathFollowerTests
    {
        static List<Vector3D> Straight() { return new List<Vector3D> { new Vector3D(0, 0, 0), new Vector3D(10, 0, 0), new Vector3D(20, 0, 0) }; }
        static List<Vector3D> Corner() { return new List<Vector3D> { new Vector3D(0, 0, 0), new Vector3D(10, 0, 0), new Vector3D(10, 10, 0) }; }

        [Test]
        public void Straight_SkipsReachedPoint_FullSpeed_Remaining()
        {
            var f = new PathFollower();
            f.Start(Straight(), false);
            double cap, rem;
            var t = f.Update(Vector3D.Zero, 2, 20, out cap, out rem);
            Assert.That(t, Is.EqualTo(new Vector3D(10, 0, 0)));
            Assert.That(f.Index, Is.EqualTo(1));
            Assert.That(cap, Is.EqualTo(20).Within(1e-9));
            Assert.That(rem, Is.EqualTo(20).Within(1e-9));
            Assert.That(f.Done, Is.False);
        }

        [Test]
        public void Corner_SlowsDown()
        {
            var f = new PathFollower();
            f.Start(Corner(), false);
            double cap, rem;
            f.Update(new Vector3D(1, 0, 0), 2, 20, out cap, out rem);
            Assert.That(cap, Is.EqualTo(10).Within(1e-6));   // 90 degree turn: 20 * (1 - 0.5)
        }

        [Test]
        public void SharpCorner_HasFloor()
        {
            var path = new List<Vector3D> { new Vector3D(0, 0, 0), new Vector3D(10, 0, 0), new Vector3D(0, 0.1, 0) };
            var f = new PathFollower();
            f.Start(path, false);
            double cap, rem;
            f.Update(new Vector3D(1, 0, 0), 2, 4, out cap, out rem);
            Assert.That(cap, Is.EqualTo(PathFollower.MinCornerSpeed).Within(1e-9));
        }

        [Test]
        public void Reverse_TraversesBackwards()
        {
            var f = new PathFollower();
            f.Start(Straight(), true);
            double cap, rem;
            var t = f.Update(new Vector3D(20, 0, 0), 2, 20, out cap, out rem);
            Assert.That(t, Is.EqualTo(new Vector3D(10, 0, 0)));
            Assert.That(rem, Is.EqualTo(20).Within(1e-9));
        }

        [Test]
        public void StartNearest_ResumesMidPath()
        {
            var f = new PathFollower();
            f.StartNearest(Straight(), true, new Vector3D(11, 0, 0));
            double cap, rem;
            var t = f.Update(new Vector3D(11, 0, 0), 2, 20, out cap, out rem);
            Assert.That(t, Is.EqualTo(new Vector3D(0, 0, 0)));
            Assert.That(rem, Is.EqualTo(11).Within(1e-9));
        }

        [Test]
        public void Done_AtLastPoint()
        {
            var f = new PathFollower();
            f.StartNearest(Straight(), false, new Vector3D(19.5, 0, 0));
            double cap, rem;
            f.Update(new Vector3D(19.5, 0, 0), 2, 20, out cap, out rem);
            Assert.That(f.Done, Is.True);
        }

        [Test]
        public void EmptyPath_IsDone()
        {
            var f = new PathFollower();
            f.Start(new List<Vector3D>(), false);
            Assert.That(f.Done, Is.True);
            double cap, rem;
            var p = new Vector3D(1, 2, 3);
            Assert.That(f.Update(p, 2, 20, out cap, out rem), Is.EqualTo(p));
            Assert.That(rem, Is.EqualTo(0));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~PathFollowerTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'PathFollower' could not be found".
- [ ] 3. Create `Fleet.Flight/Flight/PathFollower.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Flight/Flight/PathFollower.cs Fleet.Tests/Flight/PathFollowerTests.cs; git commit -m "feat(flight): path follower"`

## Done when
All PathFollowerTests pass.

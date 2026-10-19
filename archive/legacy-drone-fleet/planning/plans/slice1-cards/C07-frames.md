# C07 — Frames (connector-relative coordinates)

Milestone A · Difficulty: easy · Executor: local

## Goal
Every stored position and direction is kept **relative to the home connector** so that paths and jobs stay
valid when the carrier moves or the world reloads. This card converts between world space and a frame
(any `MatrixD`, usually the home connector's `WorldMatrix`).

## Files
- Create: `Fleet.Engine/Nav/Frames.cs`
- Create: `Fleet.Tests/Engine/FramesTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. No allocation (all `Vector3D`/`MatrixD` are structs — fine).

## Space Engineers math you need (VRageMath)
- A `MatrixD` world matrix has `.Forward`, `.Up`, `.Right`, `.Backward`, `.Translation`. Local axes: Right = +X,
  Up = +Y, **Forward = −Z** (Backward = +Z).
- Point world → local: `Vector3D.TransformNormal(world - frame.Translation, MatrixD.Transpose(frame))`.
- Point local → world: `Vector3D.TransformNormal(local, frame) + frame.Translation` (equivalently `Vector3D.Transform(local, frame)`).
- Directions use the same formulas **without** the translation term.

## Interface (implement exactly)
```csharp
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public static class Frames
        {
            public static Vector3D ToLocalPoint(MatrixD frame, Vector3D worldPoint);
            public static Vector3D ToWorldPoint(MatrixD frame, Vector3D localPoint);
            public static Vector3D ToLocalDir(MatrixD frame, Vector3D worldDir);
            public static Vector3D ToWorldDir(MatrixD frame, Vector3D localDir);
        }
    }
}
```

## Tests — create `Fleet.Tests/Engine/FramesTests.cs` verbatim
```csharp
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class FramesTests
    {
        static void Near(Vector3D actual, Vector3D expected)
        {
            Assert.That(Vector3D.Distance(actual, expected), Is.LessThan(1e-9), "actual " + actual + " expected " + expected);
        }

        // Connector at (10,0,0) facing world +X (its Forward), Up = world +Y.
        static readonly MatrixD Conn = MatrixD.CreateWorld(new Vector3D(10, 0, 0), new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));

        [Test]
        public void PointInFrontOfConnector_IsLocalMinusZ()
        {
            Near(Frames.ToLocalPoint(Conn, new Vector3D(15, 0, 0)), new Vector3D(0, 0, -5));
        }

        [Test]
        public void PointAboveConnector_IsLocalPlusY()
        {
            Near(Frames.ToLocalPoint(Conn, new Vector3D(10, 3, 0)), new Vector3D(0, 3, 0));
        }

        [Test]
        public void LocalForward_IsConnectorForwardInWorld()
        {
            Near(Frames.ToWorldDir(Conn, new Vector3D(0, 0, -1)), new Vector3D(1, 0, 0));
            Near(Frames.ToLocalDir(Conn, new Vector3D(1, 0, 0)), new Vector3D(0, 0, -1));
        }

        [Test]
        public void Directions_IgnoreTranslation()
        {
            Near(Frames.ToLocalDir(Conn, new Vector3D(0, 1, 0)), new Vector3D(0, 1, 0));
        }

        [Test]
        public void RoundTrip_ArbitraryFrame()
        {
            var f = MatrixD.CreateWorld(new Vector3D(100, -50, 7), Vector3D.Normalize(new Vector3D(1, 2, 3)),
                Vector3D.Normalize(Vector3D.Cross(new Vector3D(1, 2, 3), new Vector3D(0, 1, 0))));
            var w = new Vector3D(12, 34, -56);
            Near(Frames.ToWorldPoint(f, Frames.ToLocalPoint(f, w)), w);
            var d = Vector3D.Normalize(new Vector3D(-3, 1, 2));
            Near(Frames.ToWorldDir(f, Frames.ToLocalDir(f, d)), d);
        }

        [Test]
        public void FollowsAMovedFrame()
        {
            // The same local point, re-evaluated after the carrier moved and turned, lands in the new place.
            var local = Frames.ToLocalPoint(Conn, new Vector3D(15, 0, 0));
            var moved = MatrixD.CreateWorld(new Vector3D(0, 0, 100), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0));
            Near(Frames.ToWorldPoint(moved, local), new Vector3D(0, 0, 95));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~FramesTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'Frames' does not exist".
- [ ] 3. Create `Fleet.Engine/Nav/Frames.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Engine/Nav/Frames.cs Fleet.Tests/Engine/FramesTests.cs; git commit -m "feat(engine): connector-relative frames"`

## Done when
All FramesTests pass.

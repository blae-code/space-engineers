# C12 — GyroMath (orientation control math)

Milestone B · Difficulty: hard · Executor: local+review

## Goal
Pure math for turning the ship: the rotation needed to bring one direction onto another (including the
**180° case**), a rate command that aligns forward and up, conversion of that command into one gyro's
override values, and the ship orientation that makes a **non-forward reference block** (e.g. a belly
connector) face the requested way.

## Files
- Create: `Fleet.Flight/Flight/GyroMath.cs`
- Create: `Fleet.Tests/Flight/GyroMathTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only (use `out` parameters, not tuples). No LINQ. No allocation.

## VRageMath facts you need
- Local axes: Right = +X, Up = +Y, **Forward = −Z**. `Vector3D.Forward == (0,0,-1)`, `Vector3D.Up == (0,1,0)`.
- `MatrixD.CreateWorld(pos, forward, up)`: `Right = Cross(forward, up)`; it re-orthogonalises `up`.
- `Vector3D.Cross(a, b)`, `Vector3D.Dot(a, b)`, `v.Length()`, `Vector3D.Normalize(v)`,
  `Vector3D.TransformNormal(v, m)`, `MatrixD.Transpose(m)`.
- A rotation of angle θ about unit axis **a** (right-hand rule) is represented by the vector **a**·θ.
  Rotating Forward (0,0,−1) toward Right (1,0,0) is a rotation about **−Y**.

## Interface (implement exactly)
```csharp
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public static class GyroMath
        {
            // Derived from the right-hand rule; VERIFIED IN-GAME by GYROTEST (checklist G1). Change only after G1.
            public const double PitchSign = 1, YawSign = -1, RollSign = -1;

            public static double AngleBetween(Vector3D a, Vector3D b);        // unit inputs; radians 0..PI (clamp the dot)
            // Rotation vector (unit axis * angle) taking unit 'from' onto unit 'to'.
            // Parallel -> Zero. Anti-parallel -> axis = fallbackAxis made perpendicular to 'from' (if that is
            // degenerate, use Cross(from, (1,0,0)) or Cross(from, (0,1,0)), whichever is longer), angle PI.
            public static Vector3D AxisAngle(Vector3D from, Vector3D to, Vector3D fallbackAxis);
            // World angular-velocity command (rad/s):
            //   e = AxisAngle(curFwd, tgtFwd, curUp) + AxisAngle(curUp, tgtUp, curFwd); omega = e * gain,
            //   scaled down to length maxRate if longer.
            public static Vector3D AlignRate(Vector3D curFwd, Vector3D curUp, Vector3D tgtFwd, Vector3D tgtUp, double gain, double maxRate);
            // World omega -> one gyro's (pitch, yaw, roll) override:
            //   l = TransformNormal(worldOmega, Transpose(gyroWorld)); return (PitchSign*l.X, YawSign*l.Y, RollSign*l.Z).
            public static Vector3D ToGyroOverride(Vector3D worldOmega, MatrixD gyroWorld);
            // refInShip = pose of the reference block in ship-local coordinates (world = refInShip * shipWorld).
            // Returns the ship forward/up (world) that make the reference face tgtRefFwd with up tgtRefUp:
            //   tgt = MatrixD.CreateWorld(Zero, tgtRefFwd, tgtRefUp); inv = Transpose(refInShip);
            //   shipFwd = TransformNormal(TransformNormal(Vector3D.Forward, inv), tgt);
            //   shipUp  = TransformNormal(TransformNormal(Vector3D.Up, inv), tgt).
            public static void ReferenceToShip(MatrixD refInShip, Vector3D tgtRefFwd, Vector3D tgtRefUp, out Vector3D shipFwd, out Vector3D shipUp);
            // Unit vector = up minus its component along fwd. If that is shorter than 1e-3, do the same with
            // fallbackUp; if still degenerate, return the longer of Cross(fwd,(1,0,0)) / Cross(fwd,(0,1,0)) normalised.
            public static Vector3D PerpendicularUp(Vector3D fwd, Vector3D up, Vector3D fallbackUp);
        }
    }
}
```

## Tolerances (use exactly these)
- `AxisAngle`: treat `from`/`to` as parallel or anti-parallel when `Cross(from, to).Length() < 1e-9`;
  parallel if the dot product is > 0 (return Zero), otherwise anti-parallel.
- A fallback axis is degenerate when its length after removing the `from` component is `< 1e-6`.
- `PerpendicularUp`: degenerate threshold `1e-3` (as stated in the interface).

## Tests — create `Fleet.Tests/Flight/GyroMathTests.cs` verbatim
```csharp
using System;
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class GyroMathTests
    {
        static readonly Vector3D Fwd = new Vector3D(0, 0, -1), Up = new Vector3D(0, 1, 0), Right = new Vector3D(1, 0, 0);
        static void Near(Vector3D a, Vector3D e, double tol = 1e-6) { Assert.That(Vector3D.Distance(a, e), Is.LessThan(tol), "actual " + a + " expected " + e); }

        [Test]
        public void Aligned_IsZero()
        {
            Near(GyroMath.AlignRate(Fwd, Up, Fwd, Up, 1, 10), Vector3D.Zero);
        }

        [Test]
        public void Yaw90Right_IsNegativeY()
        {
            Near(GyroMath.AlignRate(Fwd, Up, Right, Up, 1, 10), new Vector3D(0, -Math.PI / 2, 0));
        }

        [Test]
        public void RateIsClamped()
        {
            var o = GyroMath.AlignRate(Fwd, Up, Right, Up, 10, 1);
            Assert.That(o.Length(), Is.EqualTo(1).Within(1e-9));
            Near(Vector3D.Normalize(o), new Vector3D(0, -1, 0));
        }

        [Test]
        public void AntiParallel_StillTurns()
        {
            var o = GyroMath.AlignRate(Fwd, Up, new Vector3D(0, 0, 1), Up, 1, 10);
            Assert.That(Math.Abs(o.Y), Is.EqualTo(Math.PI).Within(1e-6));
            Assert.That(Math.Abs(o.X) + Math.Abs(o.Z), Is.LessThan(1e-6));
        }

        [Test]
        public void RollOnly_IsAboutZ()
        {
            Near(GyroMath.AlignRate(Fwd, Up, Fwd, Right, 1, 10), new Vector3D(0, 0, -Math.PI / 2));
        }

        [Test]
        public void AxisAngle_Parallel_IsZero_AndAngleBetween()
        {
            Near(GyroMath.AxisAngle(Up, Up, Fwd), Vector3D.Zero);
            Assert.That(GyroMath.AngleBetween(Fwd, Right), Is.EqualTo(Math.PI / 2).Within(1e-9));
            Assert.That(GyroMath.AngleBetween(Fwd, -Fwd), Is.EqualTo(Math.PI).Within(1e-9));
        }

        [Test]
        public void AxisAngle_AntiParallel_DegenerateFallback()
        {
            // fallback parallel to 'from' must still yield a perpendicular axis with angle PI
            var e = GyroMath.AxisAngle(Up, -Up, Up);
            Assert.That(e.Length(), Is.EqualTo(Math.PI).Within(1e-6));
            Assert.That(Math.Abs(Vector3D.Dot(Vector3D.Normalize(e), Up)), Is.LessThan(1e-6));
        }

        [Test]
        public void ToGyroOverride_IdentityGyro()
        {
            Near(GyroMath.ToGyroOverride(new Vector3D(0.3, 0, 0), MatrixD.Identity), new Vector3D(GyroMath.PitchSign * 0.3, 0, 0));
            Near(GyroMath.ToGyroOverride(new Vector3D(0, 0.3, 0), MatrixD.Identity), new Vector3D(0, GyroMath.YawSign * 0.3, 0));
            Near(GyroMath.ToGyroOverride(new Vector3D(0, 0, 0.3), MatrixD.Identity), new Vector3D(0, 0, GyroMath.RollSign * 0.3));
        }

        [Test]
        public void ToGyroOverride_RotatedGyro()
        {
            // Gyro facing world +X: its Right axis is world +Z, so world rotation about +Z is its pitch.
            var gyro = MatrixD.CreateWorld(Vector3D.Zero, Right, Up);
            Near(GyroMath.ToGyroOverride(new Vector3D(0, 0, 1), gyro), new Vector3D(GyroMath.PitchSign, 0, 0));
        }

        [Test]
        public void ReferenceToShip_Identity()
        {
            Vector3D f, u;
            GyroMath.ReferenceToShip(MatrixD.Identity, Right, Up, out f, out u);
            Near(f, Right); Near(u, Up);
        }

        [Test]
        public void ReferenceToShip_BellyConnector()
        {
            // Connector faces ship-down, its up is ship-forward. To make it face world-down with up = world -Z,
            // the ship must sit level: forward (0,0,-1), up (0,1,0).
            var belly = MatrixD.CreateWorld(new Vector3D(0, -2, 0), new Vector3D(0, -1, 0), Fwd);
            Vector3D f, u;
            GyroMath.ReferenceToShip(belly, new Vector3D(0, -1, 0), Fwd, out f, out u);
            Near(f, Fwd); Near(u, Up);
        }

        [Test]
        public void PerpendicularUp()
        {
            Near(GyroMath.PerpendicularUp(Fwd, Vector3D.Normalize(new Vector3D(0, 1, -1)), Right), Up);
            Near(GyroMath.PerpendicularUp(Up, Up, Right), Right);            // up parallel to fwd -> fallback
            var p = GyroMath.PerpendicularUp(Up, Up, Up);                     // both degenerate -> any perpendicular
            Assert.That(p.Length(), Is.EqualTo(1).Within(1e-9));
            Assert.That(Math.Abs(Vector3D.Dot(p, Up)), Is.LessThan(1e-9));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~GyroMathTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'GyroMath' does not exist".
- [ ] 3. Create `Fleet.Flight/Flight/GyroMath.cs`. Clamp dot products to [-1, 1] before `Math.Acos`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Flight/Flight/GyroMath.cs Fleet.Tests/Flight/GyroMathTests.cs; git commit -m "feat(flight): gyro alignment math"`

## Done when
All GyroMathTests pass. Do **not** change the sign constants to make a test pass — if a sign test fails,
the implementation is wrong.

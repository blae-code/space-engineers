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

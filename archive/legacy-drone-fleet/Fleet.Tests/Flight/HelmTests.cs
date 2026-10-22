using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    class FakeShipIO : IShipIO
    {
        public MatrixD World = MatrixD.Identity;
        public Vector3D Vel, AngVel, Grav;
        public double M = 1000;
        public double[] Max = { 20000, 20000, 20000, 20000, 20000, 20000 };
        public double[] LastRatios = new double[6];
        public Vector3D LastOmega;
        public int SetThrustCalls;
        public bool Released;
        public MatrixD WorldMatrix { get { return World; } }
        public Vector3D LinearVelocity { get { return Vel; } }
        public Vector3D AngularVelocity { get { return AngVel; } }
        public Vector3D Gravity { get { return Grav; } }
        public double Mass { get { return M; } }
        public void GetMaxThrust(double[] maxByDir) { System.Array.Copy(Max, maxByDir, 6); }
        public void SetThrust(double[] ratioByDir) { System.Array.Copy(ratioByDir, LastRatios, 6); SetThrustCalls++; }
        public void SetGyro(Vector3D worldOmega) { LastOmega = worldOmega; }
        public void ReleaseControls() { Released = true; }
    }

    [TestFixture]
    public class HelmTests
    {
        const int F = 0, B = 1, U = 4;
        const double Dt = 1.0 / 6;
        FakeShipIO io; Helm helm;

        [SetUp] public void Init() { io = new FakeShipIO(); helm = new Helm(io); }

        static PoseTarget At(Vector3D pos, double cap = 10)
        {
            return new PoseTarget { Position = pos, Forward = new Vector3D(0, 0, -1), Up = new Vector3D(0, 1, 0), SpeedCap = cap, BrakeDist = -1 };
        }

        [Test]
        public void Hover_CancelsGravity()
        {
            io.Grav = new Vector3D(0, -9.81, 0);
            helm.Engage(At(Vector3D.Zero));
            helm.Update(Dt);
            Assert.That(io.LastRatios[U], Is.EqualTo(9810.0 / 20000).Within(0.01));
        }

        [Test]
        public void FarTargetAhead_ThrustsForward()
        {
            helm.Engage(At(new Vector3D(0, 0, -100)));
            helm.Update(Dt);
            Assert.That(io.LastRatios[F], Is.GreaterThan(0.9));
            Assert.That(io.LastRatios[B], Is.EqualTo(ThrustAllocator.MinOverride));
            Assert.That(helm.Distance, Is.EqualTo(100).Within(1e-9));
        }

        [Test]
        public void TooFastNearTarget_Brakes()
        {
            io.Vel = new Vector3D(0, 0, -50);
            helm.Engage(At(new Vector3D(0, 0, -10), 100));
            helm.Update(Dt);
            Assert.That(io.LastRatios[B], Is.GreaterThan(0.9));
        }

        [Test]
        public void NoBrakingThrust_DoesNotAccelerate()
        {
            io.Max[B] = 0;   // nothing can push the ship backwards, so it could never stop
            helm.Engage(At(new Vector3D(0, 0, -100)));
            helm.Update(Dt);
            Assert.That(io.LastRatios[F], Is.EqualTo(ThrustAllocator.MinOverride));
        }

        [Test]
        public void TurnsTowardTargetForward()
        {
            var t = At(Vector3D.Zero);
            t.Forward = new Vector3D(1, 0, 0);
            helm.Engage(t);
            helm.Update(Dt);
            Assert.That(io.LastOmega.Y, Is.LessThan(-0.1));
            Assert.That(helm.AlignmentError, Is.EqualTo(System.Math.PI / 2).Within(1e-6));
        }

        [Test]
        public void BellyReference_AlreadyAligned_NoRotation()
        {
            helm.SetReference(MatrixD.CreateWorld(new Vector3D(0, -2, 0), new Vector3D(0, -1, 0), new Vector3D(0, 0, -1)));
            helm.Engage(new PoseTarget { Position = new Vector3D(0, -2, 0), Forward = new Vector3D(0, -1, 0), Up = new Vector3D(0, 0, -1), SpeedCap = 1, BrakeDist = -1 });
            helm.Update(Dt);
            Assert.That(helm.Distance, Is.LessThan(1e-9));
            Assert.That(io.LastOmega.Length(), Is.LessThan(1e-6));
            Assert.That(helm.AlignmentError, Is.LessThan(1e-6));
        }

        [Test]
        public void Release_StopsCommanding()
        {
            helm.Engage(At(new Vector3D(0, 0, -100)));
            helm.Release();
            Assert.That(io.Released, Is.True);
            Assert.That(helm.Active, Is.False);
            helm.Update(Dt);
            Assert.That(io.SetThrustCalls, Is.EqualTo(0));
        }

        [Test]
        public void BrakeDistOverride_IsUsed()
        {
            // Target only 1 m away, but 200 m of path remain: must not creep at the 1 m braking speed.
            var t = At(new Vector3D(0, 0, -1), 50);
            t.BrakeDist = 200;
            helm.Engage(t);
            helm.Update(Dt);
            Assert.That(io.LastRatios[F], Is.GreaterThan(0.9));
        }
    }
}

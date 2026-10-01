using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    // Slice 2: the helm must settle onto a target moving at constant velocity (a drifting carrier).
    [TestFixture]
    public class HelmMovingTargetTests
    {
        const double Dt = 1.0 / 6;

        // Point-mass integration of the thrust the helm commanded (identity orientation).
        static void Step(FakeShipIO io, double dt)
        {
            var f = Vector3D.Zero;
            for (int d = 0; d < 6; d++)
                f += (Vector3D)Base6Directions.GetVector((Base6Directions.Direction)d) * (io.LastRatios[d] * io.Max[d]);
            io.Vel += (f / io.M + io.Grav) * dt;
            var w = io.World; w.Translation += io.Vel * dt; io.World = w;
        }

        static double Simulate(Vector3D targetVel, int ticks, out FakeShipIO io, out Vector3D targetPos)
        {
            io = new FakeShipIO();
            var helm = new Helm(io);
            targetPos = new Vector3D(30, 0, -40);
            for (int i = 0; i < ticks; i++)
            {
                helm.Engage(new PoseTarget { Position = targetPos, Forward = new Vector3D(0, 0, -1), Up = new Vector3D(0, 1, 0),
                    SpeedCap = 20, BrakeDist = -1, Velocity = targetVel });
                helm.Update(Dt);
                Step(io, Dt);
                targetPos += targetVel * Dt;    // sample ship and target at the same instant
            }
            return Vector3D.Distance(io.World.Translation, targetPos);
        }

        [Test]
        public void MovingTarget_MatchesVelocity_AndConverges()
        {
            FakeShipIO io; Vector3D pos;
            var v = new Vector3D(5, 0, 0);
            double dist = Simulate(v, 6 * 60, out io, out pos);
            Assert.That(dist, Is.LessThan(0.5), "settles on the moving target");
            Assert.That((io.Vel - v).Length(), Is.LessThan(0.2), "flies with the carrier");
        }

        [Test]
        public void WithoutFeedForward_WouldLag_SoTheFeatureIsReal()
        {
            // Same chase with Velocity left at zero: the slice-1 helm trails a moving target.
            var io = new FakeShipIO();
            var helm = new Helm(io);
            var v = new Vector3D(5, 0, 0);
            var p = new Vector3D(30, 0, -40);
            for (int i = 0; i < 6 * 60; i++)
            {
                helm.Engage(new PoseTarget { Position = p, Forward = new Vector3D(0, 0, -1), Up = new Vector3D(0, 1, 0), SpeedCap = 20, BrakeDist = -1 });
                helm.Update(Dt);
                Step(io, Dt);
                p += v * Dt;
            }
            Assert.That(Vector3D.Distance(io.World.Translation, p), Is.GreaterThan(0.5));
        }

        [Test]
        public void StaticTarget_StillStops()
        {
            FakeShipIO io; Vector3D pos;
            double dist = Simulate(Vector3D.Zero, 6 * 60, out io, out pos);
            Assert.That(dist, Is.LessThan(0.5));
            Assert.That(io.Vel.Length(), Is.LessThan(0.2));
        }
    }
}

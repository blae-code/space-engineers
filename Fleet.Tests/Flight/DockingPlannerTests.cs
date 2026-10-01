using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class DockingPlannerTests
    {
        // Home connector at origin, face pointing world +X, up +Y.
        static readonly MatrixD Home = MatrixD.CreateWorld(Vector3D.Zero, new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
        static readonly Vector3D Facing = new Vector3D(-1, 0, 0);
        static void Near(Vector3D a, Vector3D e) { Assert.That(Vector3D.Distance(a, e), Is.LessThan(1e-6), "actual " + a + " expected " + e); }

        PoseTarget Step(DockingPlanner d, double now, Vector3D pos, Vector3D fwd, bool connectable = false, bool connected = false)
        {
            return d.Update(now, Home, pos, fwd, connectable, connected, 5, 5, 0.5);
        }

        [Test]
        public void Approach_TargetsStandoff()
        {
            var d = new DockingPlanner(); d.Start(0);
            var t = Step(d, 0, new Vector3D(20, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Approach));
            Near(t.Position, new Vector3D(5, 0, 0));
            Near(t.Forward, Facing);
            Near(t.Up, new Vector3D(0, 1, 0));
            Assert.That(t.SpeedCap, Is.EqualTo(5));
            Assert.That(t.Precision, Is.False);
            Assert.That(t.BrakeDist, Is.EqualTo(-1));
        }

        [Test]
        public void Align_ThenCreep_WithPrecisionInside5m()
        {
            var d = new DockingPlanner(); d.Start(0);
            Step(d, 0, new Vector3D(5.3, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Align));
            var t = Step(d, 1, new Vector3D(5.3, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Creep));
            Near(t.Position, Vector3D.Zero);
            Assert.That(t.SpeedCap, Is.EqualTo(0.5));
            Assert.That(t.Precision, Is.False);
            t = Step(d, 2, new Vector3D(4, 0, 0), Facing);
            Assert.That(t.Precision, Is.True);
        }

        [Test]
        public void Misaligned_StaysInAlign()
        {
            var d = new DockingPlanner(); d.Start(0);
            Step(d, 0, new Vector3D(5, 0, 0), Facing);
            Step(d, 1, new Vector3D(5, 0, 0), new Vector3D(0, 0, -1));
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Align));
        }

        [Test]
        public void Connectable_ThenConnected()
        {
            var d = new DockingPlanner(); d.Start(0);
            Step(d, 0, new Vector3D(5, 0, 0), Facing);
            Step(d, 1, new Vector3D(5, 0, 0), Facing);
            var t = Step(d, 2, new Vector3D(1.5, 0, 0), Facing, connectable: true);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Connect));
            Assert.That(t.SpeedCap, Is.EqualTo(0));
            Assert.That(t.Precision, Is.True);
            Step(d, 3, new Vector3D(1.5, 0, 0), Facing, connectable: false, connected: true);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Done));
        }

        [Test]
        public void Creep_TimesOut()
        {
            var d = new DockingPlanner(); d.Start(0);
            Step(d, 0, new Vector3D(5, 0, 0), Facing);
            Step(d, 1, new Vector3D(5, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Creep));
            Step(d, 30, new Vector3D(3, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Creep));
            Step(d, 31.5, new Vector3D(3, 0, 0), Facing);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Failed));
        }

        [Test]
        public void AlreadyConnected_IsDone()
        {
            var d = new DockingPlanner(); d.Start(0);
            Step(d, 0, new Vector3D(1.5, 0, 0), Facing, connected: true);
            Assert.That(d.Current, Is.EqualTo(DockingPlanner.Phase.Done));
        }

        [Test]
        public void UndockTarget()
        {
            Near(DockingPlanner.UndockTarget(Home, 10), new Vector3D(10, 0, 0));
        }
    }
}

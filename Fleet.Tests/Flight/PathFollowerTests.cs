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
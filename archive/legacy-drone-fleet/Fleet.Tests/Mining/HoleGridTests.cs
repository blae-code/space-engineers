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

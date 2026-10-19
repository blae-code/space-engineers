using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class ThrustAllocatorTests
    {
        const int F = 0, B = 1, L = 2, R = 3, U = 4, D = 5;
        static double[] All(double v) { return new[] { v, v, v, v, v, v }; }
        const double Min = ThrustAllocator.MinOverride;

        [Test]
        public void UpForce_UsesUpGroup_OthersMin()
        {
            var ratios = new double[6];
            var sat = ThrustAllocator.Allocate(new Vector3D(0, 500, 0), All(1000), ratios);
            Assert.That(ratios[U], Is.EqualTo(0.5).Within(1e-9));
            Assert.That(ratios[D], Is.EqualTo(Min));
            Assert.That(ratios[F], Is.EqualTo(Min));
            Assert.That(sat, Is.EqualTo(0.5).Within(1e-9));
        }

        [Test]
        public void MinusZ_IsForwardGroup()
        {
            var ratios = new double[6];
            ThrustAllocator.Allocate(new Vector3D(0, 0, -300), All(1000), ratios);
            Assert.That(ratios[F], Is.EqualTo(0.3).Within(1e-9));
            Assert.That(ratios[B], Is.EqualTo(Min));
        }

        [Test]
        public void Saturates_AndClampsToOne()
        {
            var ratios = new double[6];
            var sat = ThrustAllocator.Allocate(new Vector3D(-2000, 0, 0), All(1000), ratios);
            Assert.That(ratios[L], Is.EqualTo(1));
            Assert.That(ratios[R], Is.EqualTo(Min));
            Assert.That(sat, Is.EqualTo(2).Within(1e-9));
        }

        [Test]
        public void ZeroForce_AllMin_ZeroSaturation()
        {
            var ratios = new double[6];
            var sat = ThrustAllocator.Allocate(Vector3D.Zero, All(1000), ratios);
            foreach (var r in ratios) Assert.That(r, Is.EqualTo(Min));
            Assert.That(sat, Is.EqualTo(0));
        }

        [Test]
        public void ZeroCapacityGroup()
        {
            var max = All(1000); max[B] = 0;
            var ratios = new double[6];
            var sat = ThrustAllocator.Allocate(new Vector3D(0, 0, 50), max, ratios);
            Assert.That(double.IsPositiveInfinity(sat), Is.True);
            Assert.That(ratios[B], Is.EqualTo(Min));
        }

        [Test]
        public void MultiAxis()
        {
            var ratios = new double[6];
            var max = new[] { 1000.0, 1000, 1000, 2000, 4000, 1000 };
            var sat = ThrustAllocator.Allocate(new Vector3D(1000, 1000, 0), max, ratios);
            Assert.That(ratios[R], Is.EqualTo(0.5).Within(1e-9));
            Assert.That(ratios[U], Is.EqualTo(0.25).Within(1e-9));
            Assert.That(sat, Is.EqualTo(0.5).Within(1e-9));
        }

        [Test]
        public void MaxForceAlong()
        {
            var max = new[] { 1000.0, 1000, 1000, 1000, 500, 1000 };
            Assert.That(ThrustAllocator.MaxForceAlong(new Vector3D(0, 1, 0), max), Is.EqualTo(500).Within(1e-9));
            var diag = Vector3D.Normalize(new Vector3D(1, 1, 0));
            Assert.That(ThrustAllocator.MaxForceAlong(diag, max), Is.EqualTo(500 / diag.Y).Within(1e-6));
            Assert.That(ThrustAllocator.MaxForceAlong(Vector3D.Zero, max), Is.EqualTo(0));
            max[F] = 0;
            Assert.That(ThrustAllocator.MaxForceAlong(new Vector3D(0, 0, -1), max), Is.EqualTo(0));
        }
    }
}
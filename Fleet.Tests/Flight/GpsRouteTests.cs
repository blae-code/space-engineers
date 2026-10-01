using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class GpsRouteTests
    {
        static void Near(Vector3D a, Vector3D e) { Assert.That(Vector3D.Distance(a, e), Is.LessThan(1e-6), "actual " + a + " expected " + e); }

        [TestCase("100.5", 100.5)]
        [TestCase("-20", -20)]
        [TestCase("3e2", 300)]
        [TestCase("+1.5E-1", 0.15)]
        [TestCase(".5", 0.5)]
        [TestCase("5.", 5)]
        [TestCase("0", 0)]
        public void ParseNumber_Valid(string s, double expected)
        {
            double v;
            Assert.That(Gps.TryParseNumber(s, out v), Is.True);
            Assert.That(v, Is.EqualTo(expected).Within(1e-9));
        }

        [TestCase("")]
        [TestCase("-")]
        [TestCase("1.2.3")]
        [TestCase("abc")]
        [TestCase("1e")]
        [TestCase(" 1")]
        [TestCase("e5")]
        public void ParseNumber_Invalid(string s)
        {
            double v;
            Assert.That(Gps.TryParseNumber(s, out v), Is.False);
        }

        [Test]
        public void ParseGps()
        {
            Vector3D p;
            Assert.That(Gps.TryParse("GPS:Ore:100.5:-20:3e2:", out p), Is.True);
            Near(p, new Vector3D(100.5, -20, 300));
            Assert.That(Gps.TryParse("GPS:Big Rock:1:2:3:#FF75C9F1:", out p), Is.True);
            Near(p, new Vector3D(1, 2, 3));
            Assert.That(Gps.TryParse("GPS:bad:x:2:3:", out p), Is.False);
            Assert.That(Gps.TryParse("hello", out p), Is.False);
            Assert.That(Gps.TryParse("GPS:Ore:1:2:", out p), Is.False);
        }

        [Test]
        public void Space_FliesStraight_ThenDone()
        {
            var r = new GpsRoute();
            r.Start(new Vector3D(100, 0, 0), false);
            var t = r.Update(Vector3D.Zero, Vector3D.Zero, 0, 150, 2);
            Near(t, new Vector3D(100, 0, 0));
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Space));
            Assert.That(r.Remaining, Is.EqualTo(100).Within(1e-9));
            r.Update(new Vector3D(99, 0, 0), Vector3D.Zero, 0, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Done));
        }

        [Test]
        public void Planet_Climb_Cruise_Descend_Done()
        {
            var g = new Vector3D(0, -9.81, 0);
            var r = new GpsRoute();
            r.Start(new Vector3D(1000, 0, 0), true);

            var t = r.Update(Vector3D.Zero, g, 50, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Climb));
            Near(t, new Vector3D(0, 105, 0));

            t = r.Update(new Vector3D(0, 100, 0), g, 145, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Cruise));
            Near(t, new Vector3D(100, 105, 0));
            Assert.That(r.Remaining, Is.EqualTo(1000 + 100).Within(1e-6));

            t = r.Update(new Vector3D(990, 150, 0), g, 150, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Descend));
            Near(t, new Vector3D(1000, 0, 0));

            r.Update(new Vector3D(1000, 1, 0), g, 1, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Done));
        }

        [Test]
        public void LeavingGravity_SwitchesToSpace()
        {
            var r = new GpsRoute();
            r.Start(new Vector3D(0, 5000, 0), true);
            r.Update(Vector3D.Zero, Vector3D.Zero, 0, 150, 2);
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Space));
        }
    }
}

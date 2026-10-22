using System.Collections.Generic;
using NUnit.Framework;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class PathRecorderTests
    {
        [Test]
        public void AddsAtMinSpacing_WhenSlow()
        {
            var r = new PathRecorder(100);
            r.Begin(Vector3D.Zero);
            Assert.That(r.Update(new Vector3D(3, 0, 0), 1), Is.True);
            Assert.That(r.Points.Count, Is.EqualTo(1));
            r.Update(new Vector3D(5, 0, 0), 1);
            Assert.That(r.Points.Count, Is.EqualTo(2));
        }

        [Test]
        public void SpacingGrowsWithSpeed()
        {
            var r = new PathRecorder(100);
            r.Begin(Vector3D.Zero);
            r.Update(new Vector3D(7, 0, 0), 10);       // needs 10 m
            Assert.That(r.Points.Count, Is.EqualTo(1));
            r.Update(new Vector3D(10.5, 0, 0), 10);
            Assert.That(r.Points.Count, Is.EqualTo(2));
        }

        [Test]
        public void StopsWhenFull()
        {
            var r = new PathRecorder(3);
            r.Begin(Vector3D.Zero);
            r.Update(new Vector3D(5, 0, 0), 0);
            r.Update(new Vector3D(10, 0, 0), 0);
            Assert.That(r.IsFull, Is.True);
            Assert.That(r.Update(new Vector3D(15, 0, 0), 0), Is.False);
            Assert.That(r.Points.Count, Is.EqualTo(3));
        }

        [Test]
        public void End_AddsFinalPoint_UnlessTooClose()
        {
            var r = new PathRecorder(10);
            r.Begin(Vector3D.Zero);
            r.End(new Vector3D(0.3, 0, 0));
            Assert.That(r.Points.Count, Is.EqualTo(1));
            r.End(new Vector3D(2, 0, 0));
            Assert.That(r.Points.Count, Is.EqualTo(2));
        }

        [Test]
        public void Begin_Clears()
        {
            var r = new PathRecorder(10);
            r.Begin(Vector3D.Zero); r.Update(new Vector3D(6, 0, 0), 0);
            r.Begin(new Vector3D(1, 1, 1));
            Assert.That(r.Points.Count, Is.EqualTo(1));
            Assert.That(r.Points[0], Is.EqualTo(new Vector3D(1, 1, 1)));
        }

        [Test]
        public void Codec_RoundTrip()
        {
            var pts = new List<Vector3D> { new Vector3D(1.234, -5, 0), new Vector3D(100.5, 2.25, -3.333), new Vector3D(0, 0, 0) };
            var ini = new MyIni();
            PathCodec.Save(ini, "Path.Dock", pts);
            var ini2 = new MyIni();
            Assert.That(ini2.TryParse(ini.ToString()), Is.True);
            var back = new List<Vector3D>();
            Assert.That(PathCodec.Load(ini2, "Path.Dock", back), Is.True);
            Assert.That(back.Count, Is.EqualTo(3));
            for (int i = 0; i < 3; i++)
                Assert.That(Vector3D.Distance(back[i], pts[i]), Is.LessThan(0.01));
        }

        [Test]
        public void Codec_MissingSection_IsFalseAndEmpty()
        {
            var back = new List<Vector3D> { Vector3D.One };
            Assert.That(PathCodec.Load(new MyIni(), "Path.Job", back), Is.False);
            Assert.That(back, Is.Empty);
        }

        [Test]
        public void Codec_MissingCoordinate_IsFalseAndEmpty()
        {
            var ini = new MyIni();
            ini.TryParse("[P]\nn=2\nx0=1\ny0=2\nz0=3\nx1=4\ny1=5\n");
            var back = new List<Vector3D>();
            Assert.That(PathCodec.Load(ini, "P", back), Is.False);
            Assert.That(back, Is.Empty);
        }

        [Test]
        public void Codec_ZeroPoints_IsTrue()
        {
            var ini = new MyIni();
            PathCodec.Save(ini, "P", new List<Vector3D>());
            var back = new List<Vector3D> { Vector3D.One };
            Assert.That(PathCodec.Load(ini, "P", back), Is.True);
            Assert.That(back, Is.Empty);
        }
    }
}

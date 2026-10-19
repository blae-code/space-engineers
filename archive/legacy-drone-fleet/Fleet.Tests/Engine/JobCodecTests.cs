using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class JobCodecTests
    {
        static FleetJob Sample()
        {
            var j = new FleetJob
            {
                JobId = 7, Origin = new Vector3D(1000.25, -2.5, 30), Forward = new Vector3D(0, 0, -1),
                Up = new Vector3D(0, 1, 0), Width = 3, Height = 2, Spacing = 2.75, Gps = true
            };
            j.Route.Add(new Vector3D(1, 2, 3));
            j.Route.Add(new Vector3D(-4, 5.5, 6));
            j.DockPath.Add(new Vector3D(0, 0, -10));
            return j;
        }

        [Test] public void Encode_Format()
        {
            Assert.That(JobCodec.Encode(Sample()), Is.EqualTo(
                "J|2|7|1000250|-2500|30000|0|0|-1000000|0|1000000|0|3|2|2750|1|2|1000|2000|3000|-4000|5500|6000|1|0|0|-10000"));
        }

        [Test] public void RoundTrip()
        {
            var back = new FleetJob();
            Assert.That(JobCodec.Decode(JobCodec.Encode(Sample()), back), Is.True);
            Assert.That(back.JobId, Is.EqualTo(7));
            Assert.That(back.Origin.X, Is.EqualTo(1000.25).Within(1e-9));
            Assert.That(back.Origin.Y, Is.EqualTo(-2.5).Within(1e-9));
            Assert.That(back.Forward.Z, Is.EqualTo(-1).Within(1e-9));
            Assert.That(back.Up.Y, Is.EqualTo(1).Within(1e-9));
            Assert.That(back.Width, Is.EqualTo(3));
            Assert.That(back.Height, Is.EqualTo(2));
            Assert.That(back.HoleCount, Is.EqualTo(6));
            Assert.That(back.Spacing, Is.EqualTo(2.75).Within(1e-9));
            Assert.That(back.Gps, Is.True);
            Assert.That(back.Route.Count, Is.EqualTo(2));
            Assert.That(back.Route[1].Y, Is.EqualTo(5.5).Within(1e-9));
            Assert.That(back.DockPath.Count, Is.EqualTo(1));
            Assert.That(back.DockPath[0].Z, Is.EqualTo(-10).Within(1e-9));
        }

        [Test] public void EmptyPaths_RoundTrip()
        {
            var j = Sample(); j.Route.Clear(); j.DockPath.Clear();
            var back = new FleetJob();
            Assert.That(JobCodec.Decode(JobCodec.Encode(j), back), Is.True);
            Assert.That(back.Route.Count, Is.EqualTo(0));
            Assert.That(back.DockPath.Count, Is.EqualTo(0));
        }

        [Test] public void Decode_NormalisesDirections()
        {
            var back = new FleetJob();
            Assert.That(JobCodec.Decode("J|2|1|0|0|0|0|0|-999000|0|1001000|0|1|1|3000|0|0|0", back), Is.True);
            Assert.That(back.Forward.Length(), Is.EqualTo(1).Within(1e-9));
            Assert.That(back.Up.Length(), Is.EqualTo(1).Within(1e-9));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("J|1|1|0|0|0|0|0|-1000000|0|1000000|0|1|1|3000|0|0|0")]        // wrong version
        [TestCase("X|2|1|0|0|0|0|0|-1000000|0|1000000|0|1|1|3000|0|0|0")]        // wrong kind
        [TestCase("J|2|1|0|0|0|0|0|-1000000|0|1000000|0|0|1|3000|0|0|0")]        // width 0
        [TestCase("J|2|1|0|0|0|0|0|-1000000|0|1000000|0|51|1|3000|0|0|0")]       // width over 50
        [TestCase("J|2|1|0|0|0|0|0|-1000000|0|1000000|0|1|1|0|0|0|0")]           // spacing 0
        [TestCase("J|2|1|0|0|0|0|0|-1000000|0|1000000|0|1|1|3000|2|0|0")]        // gps not 0/1
        [TestCase("J|2|1|0|0|0|0|0|0|0|1000000|0|1|1|3000|0|0|0")]               // zero forward
        [TestCase("J|2|1|0|0|0|0|0|-1000000|0|1000000|0|1|1|3000|0|1|0")]        // route count too big
        [TestCase("J|2|1|0|0|0|0|0|-1000000|0|1000000|0|1|1|3000|0|0|1")]        // dock count too big
        [TestCase("J|2|1|0|0|0|0|0|-1000000|0|1000000|0|1|1|3000|0|0|0|5")]      // trailing field
        [TestCase("J|2|1|0|0|0|0|0|-1000000|0|1000000|0|1|1|3000|0|0|x")]        // not a number
        public void Decode_RejectsBadLines_AndLeavesJobUntouched(string line)
        {
            var j = Sample();
            Assert.That(JobCodec.Decode(line, j), Is.False);
            Assert.That(j.JobId, Is.EqualTo(7));
            Assert.That(j.Route.Count, Is.EqualTo(2));
        }

        [Test] public void CopyFrom_CopiesEverything()
        {
            var a = Sample(); var b = new FleetJob();
            b.CopyFrom(a);
            Assert.That(JobCodec.Encode(b), Is.EqualTo(JobCodec.Encode(a)));
            a.Route.Clear();
            Assert.That(b.Route.Count, Is.EqualTo(2), "lists are copied, not shared");
        }
    }
}

using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class BeaconTests
    {
        static BayPose Bay(long id = 42)
        {
            return new BayPose
            {
                BayId = id, Position = new Vector3D(100, 0, 0), Forward = new Vector3D(0, 0, -1), Up = new Vector3D(0, 1, 0),
                Velocity = new Vector3D(5, 0, 0), AngularVelocity = Vector3D.Zero
            };
        }

        [Test]
        public void PackUnpack_RoundTrips()
        {
            var b = Bay();
            b.AngularVelocity = new Vector3D(0, 0.1, 0);
            object boxed = Beacon.Pack(ref b);   // what IGC hands the drone
            BayPose back;
            Assert.That(Beacon.TryUnpack(boxed, out back), Is.True);
            Assert.That(back.BayId, Is.EqualTo(42));
            Assert.That(back.Position, Is.EqualTo(b.Position));
            Assert.That(back.Velocity, Is.EqualTo(b.Velocity));
            Assert.That(back.AngularVelocity, Is.EqualTo(b.AngularVelocity));
        }

        [Test]
        public void Unpack_RejectsOtherPayloadsAndDegenerateAxes()
        {
            BayPose p;
            Assert.That(Beacon.TryUnpack("1|Miner|...", out p), Is.False);
            Assert.That(Beacon.TryUnpack(null, out p), Is.False);
            var b = Bay(); b.Forward = Vector3D.Zero;
            Assert.That(Beacon.TryUnpack(Beacon.Pack(ref b), out p), Is.False);
        }

        [Test]
        public void PointVelocity_AddsRotation()
        {
            // Spinning about +Y at 0.1 rad/s, a point 10 m along +X moves toward -Z at 1 m/s.
            var v = Beacon.PointVelocity(Vector3D.Zero, new Vector3D(0, 0.1, 0), Vector3D.Zero, new Vector3D(10, 0, 0));
            Assert.That((v - new Vector3D(0, 0, -1)).Length(), Is.LessThan(1e-9));
        }

        [Test]
        public void Tracker_IgnoresOtherBays_AndUnsetHome()
        {
            var t = new HomeTracker();
            var b = Bay();
            t.Offer(ref b, 0);
            Assert.That(t.Heard, Is.False, "no home yet");
            t.SetHome(7);
            t.Offer(ref b, 0);
            Assert.That(t.Heard, Is.False, "not our bay");
            t.SetHome(42);
            t.Offer(ref b, 0);
            Assert.That(t.Heard, Is.True);
        }

        [Test]
        public void Tracker_ExtrapolatesTranslation()
        {
            var t = new HomeTracker(); t.SetHome(42);
            var b = Bay(); t.Offer(ref b, 10);
            MatrixD pose; Vector3D vel;
            t.Predict(11.5, out pose, out vel);
            Assert.That((pose.Translation - new Vector3D(107.5, 0, 0)).Length(), Is.LessThan(1e-9));
            Assert.That(vel, Is.EqualTo(new Vector3D(5, 0, 0)));
        }

        [Test]
        public void Tracker_ExtrapolatesRotation_RightHanded()
        {
            // Yawing about +Y: d(fwd)/dt = w x fwd. After a quarter turn -Z must point to -X.
            var t = new HomeTracker(); t.SetHome(42);
            var b = Bay(); b.Velocity = Vector3D.Zero; b.AngularVelocity = new Vector3D(0, MathHelper.PiOver2, 0);
            t.Offer(ref b, 0);
            MatrixD pose; Vector3D vel;
            t.Predict(1, out pose, out vel);
            Assert.That((pose.Forward - new Vector3D(-1, 0, 0)).Length(), Is.LessThan(1e-6));
            Assert.That((pose.Up - new Vector3D(0, 1, 0)).Length(), Is.LessThan(1e-6));
        }

        [Test]
        public void Tracker_GoesStale_ThenRecovers()
        {
            var t = new HomeTracker(); t.SetHome(42);
            var b = Bay();
            Assert.That(t.Lost(0), Is.False, "never heard is not lost (static slice-1 carrier)");
            t.Offer(ref b, 0);
            Assert.That(t.Fresh(1.9), Is.True);
            Assert.That(t.Lost(2.1), Is.True);
            t.Offer(ref b, 3);
            Assert.That(t.Fresh(3.5), Is.True);
            t.SetHome(43);
            Assert.That(t.Heard, Is.False, "new home forgets the old bay");
        }
    }
}

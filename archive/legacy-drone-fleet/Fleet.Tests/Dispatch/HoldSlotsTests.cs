using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Dispatch
{
    [TestFixture]
    public class HoldSlotsTests
    {
        static readonly MatrixD Marker = MatrixD.CreateWorld(new Vector3D(100, 0, 0), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0));

        [Test] public void Slot0_IsOneSpacingAboveTheMarker()
        {
            var p = HoldSlots.Position(Marker, 0, 15);
            Assert.That(p.X, Is.EqualTo(100).Within(1e-9));
            Assert.That(p.Y, Is.EqualTo(15).Within(1e-9));
            Assert.That(p.Z, Is.EqualTo(0).Within(1e-9));
        }

        [Test] public void SlotsStackAlongTheMarkersUp()
        {
            var tilted = MatrixD.CreateWorld(Vector3D.Zero, new Vector3D(0, 0, -1), new Vector3D(1, 0, 0));
            var p = HoldSlots.Position(tilted, 2, 10);
            Assert.That(p.X, Is.EqualTo(30).Within(1e-9));
            Assert.That(p.Y, Is.EqualTo(0).Within(1e-9));
        }

        [Test] public void TryPosition_HoldsInPlaceWithoutMarkerOrSlot()
        {
            Vector3D p;
            Assert.That(HoldSlots.TryPosition(false, Marker, 0, 15, out p), Is.False);
            Assert.That(HoldSlots.TryPosition(true, Marker, -1, 15, out p), Is.False);
            Assert.That(HoldSlots.TryPosition(true, Marker, 1, 15, out p), Is.True);
            Assert.That(p.Y, Is.EqualTo(30).Within(1e-9));
        }

        [Test] public void MarkerId_IsMinusOne_AndTracksLikeABay()
        {
            Assert.That(HoldSlots.MarkerId, Is.EqualTo(-1));
            var t = new HomeTracker();
            t.SetHome(HoldSlots.MarkerId);
            var pose = new BayPose { BayId = -1, Position = new Vector3D(0, 0, 0), Forward = new Vector3D(0, 0, -1), Up = new Vector3D(0, 1, 0), Velocity = new Vector3D(2, 0, 0) };
            t.Offer(ref pose, 10);
            Assert.That(t.Fresh(10.5), Is.True);
            MatrixD m; Vector3D v;
            t.Predict(11, out m, out v);
            Assert.That(HoldSlots.Position(m, 0, 15).X, Is.EqualTo(2).Within(1e-9));
        }
    }
}

using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class FramesTests
    {
        static void Near(Vector3D actual, Vector3D expected)
        {
            Assert.That(Vector3D.Distance(actual, expected), Is.LessThan(1e-9), "actual " + actual + " expected " + expected);
        }

        // Connector at (10,0,0) facing world +X (its Forward), Up = world +Y.
        static readonly MatrixD Conn = MatrixD.CreateWorld(new Vector3D(10, 0, 0), new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));

        [Test]
        public void PointInFrontOfConnector_IsLocalMinusZ()
        {
            Near(Frames.ToLocalPoint(Conn, new Vector3D(15, 0, 0)), new Vector3D(0, 0, -5));
        }

        [Test]
        public void PointAboveConnector_IsLocalPlusY()
        {
            Near(Frames.ToLocalPoint(Conn, new Vector3D(10, 3, 0)), new Vector3D(0, 3, 0));
        }

        [Test]
        public void LocalForward_IsConnectorForwardInWorld()
        {
            Near(Frames.ToWorldDir(Conn, new Vector3D(0, 0, -1)), new Vector3D(1, 0, 0));
            Near(Frames.ToLocalDir(Conn, new Vector3D(1, 0, 0)), new Vector3D(0, 0, -1));
        }

        [Test]
        public void Directions_IgnoreTranslation()
        {
            Near(Frames.ToLocalDir(Conn, new Vector3D(0, 1, 0)), new Vector3D(0, 1, 0));
        }

        [Test]
        public void RoundTrip_ArbitraryFrame()
        {
            var f = MatrixD.CreateWorld(new Vector3D(100, -50, 7), Vector3D.Normalize(new Vector3D(1, 2, 3)),
                Vector3D.Normalize(Vector3D.Cross(new Vector3D(1, 2, 3), new Vector3D(0, 1, 0))));
            var w = new Vector3D(12, 34, -56);
            Near(Frames.ToWorldPoint(f, Frames.ToLocalPoint(f, w)), w);
            var d = Vector3D.Normalize(new Vector3D(-3, 1, 2));
            Near(Frames.ToWorldDir(f, Frames.ToLocalDir(f, d)), d);
        }

        [Test]
        public void FollowsAMovedFrame()
        {
            // The same local point, re-evaluated after the carrier moved and turned, lands in the new place.
            var local = Frames.ToLocalPoint(Conn, new Vector3D(15, 0, 0));
            var moved = MatrixD.CreateWorld(new Vector3D(0, 0, 100), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0));
            Near(Frames.ToWorldPoint(moved, local), new Vector3D(0, 0, 95));
        }
    }
}
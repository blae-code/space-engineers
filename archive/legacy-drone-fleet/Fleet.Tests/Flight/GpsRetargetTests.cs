using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class GpsRetargetTests
    {
        [Test]
        public void Retarget_KeepsPhase_MovesTarget()
        {
            var r = new GpsRoute();
            r.Start(new Vector3D(0, 0, -1000), false);
            r.Update(Vector3D.Zero, Vector3D.Zero, 0, 150, 3);
            r.Retarget(new Vector3D(50, 0, -1000));
            Assert.That(r.Current, Is.EqualTo(GpsRoute.Phase.Space));
            Assert.That(r.Target, Is.EqualTo(new Vector3D(50, 0, -1000)));
            var carrot = r.Update(Vector3D.Zero, Vector3D.Zero, 0, 150, 3);
            Assert.That(carrot.X, Is.GreaterThan(0), "now steering toward the moved target");
        }
    }
}

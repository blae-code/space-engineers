using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    [TestFixture]
    public class ShipIOTests
    {
        const int F = 0, B = 1, L = 2, R = 3, U = 4, D = 5;

        [Test]
        public void GroupOf_IdentityShip()
        {
            Assert.That(ShipIO.GroupOf(MatrixD.Identity, new Vector3D(0, 0, -1)), Is.EqualTo(F));
            Assert.That(ShipIO.GroupOf(MatrixD.Identity, new Vector3D(0, 1, 0)), Is.EqualTo(U));
            Assert.That(ShipIO.GroupOf(MatrixD.Identity, new Vector3D(-1, 0, 0)), Is.EqualTo(L));
        }

        [Test]
        public void GroupOf_RotatedShip()
        {
            // Ship facing world +X: a thruster pushing world +X pushes the ship Forward.
            var ship = MatrixD.CreateWorld(Vector3D.Zero, new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
            Assert.That(ShipIO.GroupOf(ship, new Vector3D(1, 0, 0)), Is.EqualTo(F));
            Assert.That(ShipIO.GroupOf(ship, new Vector3D(0, 0, 1)), Is.EqualTo(R));
        }

        [Test]
        public void ReleaseWithoutRefresh_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new ShipIO().ReleaseControls());
        }
    }
}

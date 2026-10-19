using System.Text;
using NUnit.Framework;
using VRageMath;
using static IngameScript.Program;

namespace Fleet.Tests.Flight
{
    // Tests for the C25 extensions (three-axis GYROTEST, mass guard, GyroVerdict). The card's own
    // verbatim tests live in ShipIOTests.cs and are never edited.
    [TestFixture]
    public class ShipIOExtraTests
    {
        [Test]
        public void GyroVerdict_Thresholds()
        {
            Assert.That(ShipIO.GyroVerdict(1.0), Is.EqualTo("OK"));
            Assert.That(ShipIO.GyroVerdict(0.7), Is.EqualTo("OK"));
            Assert.That(ShipIO.GyroVerdict(-1.0), Is.EqualTo("SIGN FLIPPED"));
            Assert.That(ShipIO.GyroVerdict(9.5), Is.EqualTo("UNITS: RPM?"));
            Assert.That(ShipIO.GyroVerdict(0.2), Is.EqualTo("UNEXPECTED"));
        }

        [Test]
        public void FreshShipIO_MassIsZero()
        {
            Assert.That(new ShipIO().Mass, Is.EqualTo(0));
        }

        [Test]
        public void FreshShipIO_ControlsDoNotThrow()
        {
            var io = new ShipIO();
            var max = new double[] { 1, 2, 3, 4, 5, 6 };
            Assert.DoesNotThrow(() => io.ReleaseControls());
            Assert.DoesNotThrow(() => io.SetGyro(new Vector3D(0, 1, 0)));
            Assert.DoesNotThrow(() => io.SetThrust(new double[6]));
            Assert.DoesNotThrow(() => io.GetMaxThrust(max));
            Assert.That(max, Is.EqualTo(new double[6]));
        }

        [Test]
        public void GyroTest_RunsThreePhasesThenReports()
        {
            var io = new ShipIO();
            var sb = new StringBuilder("stale");
            io.BeginGyroTest(0);
            Assert.That(io.GyroTestRunning, Is.True);

            io.UpdateGyroTest(1.0, sb);
            Assert.That(sb.ToString(), Is.EqualTo("GYROTEST running: pitch"));
            io.UpdateGyroTest(2.0, sb);
            Assert.That(sb.ToString(), Is.EqualTo("GYROTEST running: yaw"));
            io.UpdateGyroTest(4.0, sb);
            Assert.That(sb.ToString(), Is.EqualTo("GYROTEST running: roll"));
            Assert.That(io.GyroTestRunning, Is.True);

            io.UpdateGyroTest(6.0, sb);
            Assert.That(io.GyroTestRunning, Is.False);
            // No controller bound: angular velocity reads zero, so every axis is UNEXPECTED.
            Assert.That(sb.ToString(), Is.EqualTo(
                "GYROTEST pitch: measured/commanded = 0.00 (UNEXPECTED)\n" +
                "GYROTEST yaw: measured/commanded = 0.00 (UNEXPECTED)\n" +
                "GYROTEST roll: measured/commanded = 0.00 (UNEXPECTED)\n"));
        }
    }
}

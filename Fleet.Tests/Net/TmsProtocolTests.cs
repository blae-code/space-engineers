using System.Text;
using IngameScript;
using NUnit.Framework;

namespace Fleet.Tests.Net
{
    [TestFixture]
    public class TmsProtocolTests
    {
        [Test]
        public void TryReadHandover_AcceptsCanonicalPayload()
        {
            string target;
            string timestamp;

            bool valid = TmsProtocol.TryReadHandover("HANDOVER;GND-01;123.5", out target, out timestamp);

            Assert.That(valid, Is.True);
            Assert.That(target, Is.EqualTo("GND-01"));
            Assert.That(timestamp, Is.EqualTo("123.5"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("ACK;GND-01;123")]
        [TestCase("HANDOVER;;123")]
        [TestCase("HANDOVER;GND-01;")]
        [TestCase("HANDOVER;GND-01;123;EXTRA")]
        public void TryReadHandover_RejectsMalformedPayload(string payload)
        {
            string target;
            string timestamp;

            bool valid = TmsProtocol.TryReadHandover(payload, out target, out timestamp);

            Assert.That(valid, Is.False);
            Assert.That(target, Is.Empty);
            Assert.That(timestamp, Is.Empty);
        }

        [Test]
        public void TryWriteHandover_WritesCanonicalPayload()
        {
            var buffer = new StringBuilder();

            bool valid = TmsProtocol.TryWriteHandover(buffer, "GND-01", "123.5");

            Assert.That(valid, Is.True);
            Assert.That(buffer.ToString(), Is.EqualTo("HANDOVER;GND-01;123.5"));
        }

        [Test]
        public void TryWriteHandover_RejectsEmptyOrDelimitedFieldsWithoutChangingBuffer()
        {
            var buffer = new StringBuilder("unchanged");

            bool emptyTarget = TmsProtocol.TryWriteHandover(buffer, "", "123");
            bool delimitedTarget = TmsProtocol.TryWriteHandover(buffer, "GND;01", "123");

            Assert.That(emptyTarget, Is.False);
            Assert.That(delimitedTarget, Is.False);
            Assert.That(buffer.ToString(), Is.EqualTo("unchanged"));
        }
    }
}

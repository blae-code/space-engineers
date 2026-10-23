using System.Collections.Generic;
using System.Text;
using IngameScript;
using NUnit.Framework;

namespace Fleet.Tests.Net
{
    [TestFixture]
    public class TmsDispatchProtocolTests
    {
        [Test]
        public void TryReadJitRequest_ReadsCanonicalPayload()
        {
            var fields = new List<string>();
            string requester;
            string tier;
            string eta;
            string ore;
            string mass;

            bool valid = TmsProtocol.TryReadJitRequest(
                "JIT;MAS-01;AIR;45;RARE_ORE;45000", fields,
                out requester, out tier, out eta, out ore, out mass);

            Assert.That(valid, Is.True);
            Assert.That(requester, Is.EqualTo("MAS-01"));
            Assert.That(tier, Is.EqualTo("AIR"));
            Assert.That(eta, Is.EqualTo("45"));
            Assert.That(ore, Is.EqualTo("RARE_ORE"));
            Assert.That(mass, Is.EqualTo("45000"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("CALL;MAS-01;AIR;45;ORE;100")]
        [TestCase("JIT;MAS-01;AIR;45;ORE")]
        [TestCase("JIT;MAS-01;AIR;45;ORE;100;EXTRA")]
        [TestCase("JIT;MAS-01;;45;ORE;100")]
        public void TryReadJitRequest_RejectsMalformedPayload(string payload)
        {
            var fields = new List<string>();
            string requester;
            string tier;
            string eta;
            string ore;
            string mass;

            bool valid = TmsProtocol.TryReadJitRequest(
                payload, fields, out requester, out tier, out eta, out ore, out mass);

            Assert.That(valid, Is.False);
            Assert.That(requester, Is.Empty);
            Assert.That(tier, Is.Empty);
            Assert.That(eta, Is.Empty);
            Assert.That(ore, Is.Empty);
            Assert.That(mass, Is.Empty);
        }

        [Test]
        public void MuleDispatch_ReadWriteRoundTrip()
        {
            var buffer = new StringBuilder();
            var fields = new List<string>();
            string mule;
            string pad;
            string gps;
            string payloadType;
            string requestId;

            bool written = TmsProtocol.TryWriteMuleDispatch(
                buffer, "MUL-01", "MAS-PAD-1", "GPS:GND-01:1:2:3:", "RARE_ORE", "REQ-7");
            bool read = TmsProtocol.TryReadMuleDispatch(
                buffer.ToString(), fields, out mule, out pad, out gps, out payloadType, out requestId);

            Assert.That(written, Is.True);
            Assert.That(read, Is.True);
            Assert.That(mule, Is.EqualTo("MUL-01"));
            Assert.That(pad, Is.EqualTo("MAS-PAD-1"));
            Assert.That(gps, Is.EqualTo("GPS:GND-01:1:2:3:"));
            Assert.That(payloadType, Is.EqualTo("RARE_ORE"));
            Assert.That(requestId, Is.EqualTo("REQ-7"));
        }

        [Test]
        public void CatDispatch_ReadWriteRoundTrip()
        {
            var buffer = new StringBuilder();
            var fields = new List<string>();
            string cat;
            string destination;
            string ore;
            string requestId;

            bool written = TmsProtocol.TryWriteCatDispatch(buffer, "CAT-01", "REF-01", "IRON", "REQ-8");
            bool read = TmsProtocol.TryReadCatDispatch(
                buffer.ToString(), fields, out cat, out destination, out ore, out requestId);

            Assert.That(written, Is.True);
            Assert.That(read, Is.True);
            Assert.That(cat, Is.EqualTo("CAT-01"));
            Assert.That(destination, Is.EqualTo("REF-01"));
            Assert.That(ore, Is.EqualTo("IRON"));
            Assert.That(requestId, Is.EqualTo("REQ-8"));
        }

        [Test]
        public void BaseDemand_ReadWriteRoundTrip()
        {
            var buffer = new StringBuilder();
            var fields = new List<string>();
            string ore;
            string priority;
            string deficit;
            string target;

            bool written = TmsProtocol.TryWriteBaseDemand(buffer, "IRON", "3", "2500000", "5000000");
            bool read = TmsProtocol.TryReadBaseDemand(
                buffer.ToString(), fields, out ore, out priority, out deficit, out target);

            Assert.That(written, Is.True);
            Assert.That(read, Is.True);
            Assert.That(ore, Is.EqualTo("IRON"));
            Assert.That(priority, Is.EqualTo("3"));
            Assert.That(deficit, Is.EqualTo("2500000"));
            Assert.That(target, Is.EqualTo("5000000"));
        }

        [Test]
        public void Writers_RejectInvalidFieldsWithoutChangingBuffer()
        {
            var buffer = new StringBuilder("unchanged");

            bool muleValid = TmsProtocol.TryWriteMuleDispatch(
                buffer, "MUL-01", "PAD;1", "GPS:GND-01:1:2:3:", "ORE", "REQ-1");
            bool demandValid = TmsProtocol.TryWriteBaseDemand(buffer, "IRON", "2", "10", "");

            Assert.That(muleValid, Is.False);
            Assert.That(demandValid, Is.False);
            Assert.That(buffer.ToString(), Is.EqualTo("unchanged"));
        }
    }
}

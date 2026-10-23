# C35 — TMS routed dispatch contract and receivers

Pilot · Difficulty: medium · Executor: claude · Kind: wire

## Goal
Carry route endpoints in terrestrial JIT/dispatch messages, have the active Ground Base C2 route
validated requests on the correct channel, and make Mule/Caterpillar accept only strict assignments
addressed to their own IDs.

This card stages route assignments in the vehicles. It does not start movement; autonomous flight and
ground navigation require their own tested control slice.

## Files
- Modify: `scripts/Shared/Net/TmsProtocol.cs`
- Modify: `scripts/Rig.OS.Lite/Program.cs`
- Modify: `scripts/Base.C2/Program.cs`
- Modify: `scripts/Drone.Mule/Program.cs`
- Modify: `scripts/Train.Caterpillar/Program.cs`
- Modify: `scripts/Rig.OS.Lite/Instructions.readme`
- Create: `Fleet.Tests/Net/TmsRouteProtocolTests.cs`
- Modify: `docs/protocol/igc.md`
- Modify: `docs/CHANGELOG.md`
- Modify: `docs/superpowers/plans/terrestrial-mining-cards/terrestrial-mining.md`
- Create: `docs/superpowers/plans/terrestrial-mining-cards/C35-tms-routed-dispatch.md`

## Interface
- Routed JIT: `JIT;<requester_id>;<tier>;<eta_target_sec>;<ore_type>;<est_mass_kg>;<source_gps>`
- Mule assignment: `DISPATCH;<mule_id>;<pickup_gps>;<delivery_gps>;<payload_type>;<request_id>`
- Caterpillar assignment: `DISPATCH;<cat_id>;<destination_id>;<destination_gps>;<ore_type>;<request_id>`
- C2 accepts a JIT request only when it is master, requester is `MAS-01`, tier is `AIR` or `GND`,
  and the required endpoint is available.
- `AIR` JIT source GPS comes from grid-scoped `MAS MP Conn`; `GND` JIT GPS is the Mastodon PB/grid
  position, explicitly a coarse rendezvous waypoint because `MAS Stern Dump Conn` is on a piston subgrid.
- `AIR` emits only `TMS.MULE.DISPATCH`; `GND` emits only `TMS.CAT.DISPATCH`.
- Mule and Caterpillar ignore malformed assignments, unrelated tags, and assignments for another ID.
- Receivers record assignment fields but do not launch, drive, dock, or actuate hardware.

## Hot paths
No per-tick parser/writer call is added to the flight or traction loops. IGC payload parsing is only
performed when the Update100 listener has a pending message; JIT serialization is only performed on
the cargo threshold edge.

## Tests — create `Fleet.Tests/Net/TmsRouteProtocolTests.cs` verbatim
```csharp
using System.Collections.Generic;
using System.Text;
using IngameScript;
using NUnit.Framework;

namespace Fleet.Tests.Net
{
    [TestFixture]
    public class TmsRouteProtocolTests
    {
        [Test]
        public void JitRoute_ReadWriteRoundTrip()
        {
            var buffer = new StringBuilder();
            var fields = new List<string>();
            string requester;
            string tier;
            string eta;
            string ore;
            string mass;
            string sourceGps;

            bool written = TmsProtocol.TryWriteJitRequest(
                buffer, "MAS-01", "AIR", "45", "RARE_ORE", "45000",
                "GPS:MAS-01:1:2:3:");
            bool read = TmsProtocol.TryReadJitRequest(
                buffer.ToString(), fields, out requester, out tier, out eta, out ore, out mass,
                out sourceGps);

            Assert.That(written, Is.True);
            Assert.That(read, Is.True);
            Assert.That(sourceGps, Is.EqualTo("GPS:MAS-01:1:2:3:"));
        }

        [Test]
        public void JitRoute_IntegerWriterUsesCanonicalDigits()
        {
            var buffer = new StringBuilder();

            bool written = TmsProtocol.TryWriteJitRequest(
                buffer, "MAS-01", "AIR", 45, "RARE_ORE", 45000, "GPS:MAS-01:1:2:3:");

            Assert.That(written, Is.True);
            Assert.That(buffer.ToString(),
                Is.EqualTo("JIT;MAS-01;AIR;45;RARE_ORE;45000;GPS:MAS-01:1:2:3:"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("JIT;MAS-01;AIR;45;ORE;100")]
        [TestCase("JIT;MAS-01;AIR;45;ORE;100;GPS:MAS-01:1:2:3:;EXTRA")]
        [TestCase("JIT;MAS-01;AIR;45;ORE;100;")]
        [TestCase("JIT;MAS-01;AIR;45;ORE;100;not GPS")]
        public void JitRoute_RejectsMalformedPayload(string payload)
        {
            var fields = new List<string>();
            string requester;
            string tier;
            string eta;
            string ore;
            string mass;
            string sourceGps;

            bool valid = TmsProtocol.TryReadJitRequest(
                payload, fields, out requester, out tier, out eta, out ore, out mass,
                out sourceGps);

            Assert.That(valid, Is.False);
            Assert.That(sourceGps, Is.Empty);
        }

        [Test]
        public void MuleRoute_ReadWriteRoundTrip()
        {
            var buffer = new StringBuilder();
            var fields = new List<string>();
            string mule;
            string pickupGps;
            string deliveryGps;
            string payloadType;
            string requestId;

            bool written = TmsProtocol.TryWriteMuleRouteDispatch(
                buffer, "MUL-01", "GPS:MAS-01:1:2:3:", "GPS:GND-01:4:5:6:",
                "RARE_ORE", "REQ-7");
            bool read = TmsProtocol.TryReadMuleRouteDispatch(
                buffer.ToString(), fields, out mule, out pickupGps, out deliveryGps,
                out payloadType, out requestId);

            Assert.That(written, Is.True);
            Assert.That(read, Is.True);
            Assert.That(pickupGps, Is.EqualTo("GPS:MAS-01:1:2:3:"));
            Assert.That(deliveryGps, Is.EqualTo("GPS:GND-01:4:5:6:"));
        }

        [Test]
        public void CatRoute_ReadWriteRoundTrip()
        {
            var buffer = new StringBuilder();
            var fields = new List<string>();
            string cat;
            string destinationId;
            string destinationGps;
            string ore;
            string requestId;

            bool written = TmsProtocol.TryWriteCatDispatch(
                buffer, "CAT-01", "MAS-01", "GPS:MAS-01:1:2:3:", "IRON", "REQ-8");
            bool read = TmsProtocol.TryReadCatRouteDispatch(
                buffer.ToString(), fields, out cat, out destinationId, out destinationGps,
                out ore, out requestId);

            Assert.That(written, Is.True);
            Assert.That(read, Is.True);
            Assert.That(destinationId, Is.EqualTo("MAS-01"));
            Assert.That(destinationGps, Is.EqualTo("GPS:MAS-01:1:2:3:"));
        }

        [Test]
        public void RoutedWriters_RejectInvalidFieldsWithoutChangingBuffer()
        {
            var buffer = new StringBuilder("unchanged");

            bool jitValid = TmsProtocol.TryWriteJitRequest(
                buffer, "MAS-01", "AIR", "45", "ORE", "100", "");
            bool jitDelimiterValid = TmsProtocol.TryWriteJitRequest(
                buffer, "MAS-01", "AIR", "45", "ORE", "100", "GPS:MAS;01:1:2:3:");
            bool muleValid = TmsProtocol.TryWriteMuleRouteDispatch(
                buffer, "MUL-01", "MAS-PAD-1", "GPS:GND-01:4:5:6:", "ORE", "REQ-1");
            bool catValid = TmsProtocol.TryWriteCatDispatch(
                buffer, "CAT-01", "MAS-01", "GPS;BAD", "IRON", "REQ-1");

            Assert.That(jitValid, Is.False);
            Assert.That(jitDelimiterValid, Is.False);
            Assert.That(muleValid, Is.False);
            Assert.That(catValid, Is.False);
            Assert.That(buffer.ToString(), Is.EqualTo("unchanged"));
        }
    }
}
```

## Done when
- `dotnet test Fleet.Tests` passes, including the verbatim route tests.
- The eight terrestrial PB projects build in Release.
- `python3 tools/card-check.py C35` reports `RESULT: PASS`.
- Mule/Caterpillar do not actuate from a received assignment.

# C33 — TMS root handover and build baseline

Pilot · Difficulty: small · Executor: local · Kind: logic

## Goal
Provide a pure C# 6 codec for the canonical `TMS.C2.HANDOVER` payload, wire the existing RIG//OS-Lite
and Ground Base handlers to it, and restore Linux MDK build settings for the affected projects.

## Files
- Create: `scripts/Shared/Net/TmsProtocol.cs`
- Create: `Fleet.Tests/Fleet.Tests.csproj`
- Create: `Fleet.Tests/Net/TmsProtocolTests.cs`
- Modify: `scripts/Base.C2/mdk.ini`
- Modify: `scripts/Drone.Mule/mdk.ini`
- Modify: `scripts/Excavator.Mastodon/mdk.ini`
- Modify: `scripts/Train.Caterpillar/mdk.ini`
- Modify: `scripts/Drone.Kestrel/mdk.ini`
- Modify: `scripts/Drone.Albatross/mdk.ini`
- Modify: `scripts/Rig.OS.Lite/Program.cs`
- Modify: `scripts/Rig.OS.Lite/mdk.ini`
- Modify: `scripts/Base.C2/Program.cs`
- Modify: `scripts/Excavator.Mastodon/Program.cs`
- Modify: `scripts/Fob.Utility/Program.cs`
- Modify: `scripts/Fob.Utility/mdk.ini`
- Modify: `docs/protocol/igc.md`
- Modify: `docs/CHANGELOG.md`
- Create: `scripts/.gitignore`
- Create: `scripts/Directory.Build.props`
- Create: `scripts/Directory.Build.targets`
- Create: `docs/superpowers/plans/terrestrial-mining-cards/C33-tms-handover-contract.md`
- Create: `docs/superpowers/plans/terrestrial-mining-cards/terrestrial-mining.md`

## Interface
```csharp
public static class TmsProtocol
{
    public const string HandoverTag = "TMS.C2.HANDOVER";
    public static bool TryReadHandover(string payload, out string targetNode, out string timestamp);
    public static bool TryWriteHandover(StringBuilder buffer, string targetNode, string timestamp);
}
```

## Behaviour
- The payload is exactly `HANDOVER;<target_node>;<timestamp>`.
- Reject null, empty, wrong-verb, missing, empty, or extra fields. The timestamp remains opaque text.
- On a failed read, both `out` strings are empty.
- On a failed write, leave the supplied `StringBuilder` unchanged.
- Reject `;` in either field. Do not use LINQ, reflection, culture APIs, or runtime enum names.
- This codec is for the rare root-transfer event, not a per-tick status parser.

## Hot paths
None. Handover parsing and writing are event-driven.

## Tests — create `Fleet.Tests/Net/TmsProtocolTests.cs` verbatim
```csharp
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
```

## Steps
- Create the pure codec to the exact interface and behaviour above.
- Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~TmsProtocolTests"`.
- Run `python3 tools/card-check.py C33`.

## Done when
All four test methods pass, including malformed input and unchanged-buffer behavior.

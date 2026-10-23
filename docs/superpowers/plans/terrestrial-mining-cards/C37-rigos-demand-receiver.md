# C37 — RIG//OS-Lite demand receiver

Pilot · Difficulty: medium · Executor: claude · Kind: wire

## Goal
Consume the measured Base C2 demand stream in RIG//OS-Lite, retain the latest valid demand for all
five minerals, expire stale entries, and show the data on the Mastodon status displays. This card
does not select mining targets or control the excavator.

## Files
- Create: `scripts/Shared/Net/TmsDemandLedger.cs`
- Modify: `scripts/Rig.OS.Lite/Program.cs`
- Modify: `scripts/Rig.OS.Lite/Instructions.readme`
- Modify: `docs/protocol/igc.md`
- Modify: `docs/CHANGELOG.md`
- Modify: `docs/superpowers/plans/terrestrial-mining-cards/terrestrial-mining.md`
- Modify: `Fleet.Tests/Fleet.Tests.csproj`
- Create: `Fleet.Tests/Net/TmsDemandLedgerTests.cs`
- Create: `docs/superpowers/plans/terrestrial-mining-cards/C37-rigos-demand-receiver.md`

## Contract
- Parse only `TMS.BASE.DEMAND` payloads using the canonical `TmsProtocol.TryReadBaseDemand`.
- Accept only the five current demand types: Uranium, Platinum, Gold, Magnesium, and Iron.
- Require integer priority in 1..5 and nonnegative integer deficit and target values; reject a deficit
  larger than its target. Invalid packets leave prior ledger entries unchanged.
- Keep fixed-size records for all five minerals. Each valid packet refreshes only its ore record.
- Expire each record after 30 Update10 calls without a valid refresh, matching the existing 0.166 s
  watchdog cadence; zero-deficit records remain fresh but do not count as active deficits.
- Render freshness, priority, and deficit on the existing Mastodon boom display and terminal Echo
  using preallocated buffers. No mining, movement, docking, or hardware actuation is added.

## Hot paths
`Program.RenderBoomScreen` renders the fixed-size ledger using the existing reusable display buffer.
Packet parsing happens only when a broadcast listener has a pending message.

## Tests — create `Fleet.Tests/Net/TmsDemandLedgerTests.cs` verbatim
```csharp
using System.Collections.Generic;
using IngameScript;
using NUnit.Framework;

namespace Fleet.Tests.Net
{
    [TestFixture]
    public class TmsDemandLedgerTests
    {
        [Test]
        public void NewLedger_HasNoFreshDemand()
        {
            var ledger = new TmsDemandLedger();

            Assert.That(ledger.FreshCount, Is.EqualTo(0));
            Assert.That(ledger.DeficitCount, Is.EqualTo(0));
        }

        [Test]
        public void TryUpdatePayload_StoresCanonicalDemand()
        {
            var ledger = new TmsDemandLedger();
            var fields = new List<string>(8);
            TmsDemandLedger.DemandRecord demand;

            bool valid = ledger.TryUpdatePayload("DEMAND;URANIUM;5;15000;20000", fields);

            Assert.That(valid, Is.True);
            Assert.That(ledger.TryGet("URANIUM", out demand), Is.True);
            Assert.That(demand.Priority, Is.EqualTo(5));
            Assert.That(demand.DeficitKg, Is.EqualTo(15000));
            Assert.That(demand.TargetStockKg, Is.EqualTo(20000));
            Assert.That(demand.Fresh, Is.True);
            Assert.That(ledger.FreshCount, Is.EqualTo(1));
            Assert.That(ledger.DeficitCount, Is.EqualTo(1));
        }

        [Test]
        public void TryUpdatePayload_ZeroDeficitIsFreshButNotActive()
        {
            var ledger = new TmsDemandLedger();
            var fields = new List<string>(8);

            bool valid = ledger.TryUpdatePayload("DEMAND;IRON;3;0;2500000", fields);

            Assert.That(valid, Is.True);
            Assert.That(ledger.FreshCount, Is.EqualTo(1));
            Assert.That(ledger.DeficitCount, Is.EqualTo(0));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("DEMAND;NICKEL;3;10;100")]
        [TestCase("DEMAND;IRON;0;0;100")]
        [TestCase("DEMAND;IRON;6;0;100")]
        [TestCase("DEMAND;IRON;3;-1;100")]
        [TestCase("DEMAND;IRON;3;101;100")]
        [TestCase("DEMAND;IRON;3;1;not-a-number")]
        [TestCase("DEMAND;IRON;3;1;100;EXTRA")]
        public void TryUpdatePayload_RejectsInvalidPayloadWithoutChangingLedger(string payload)
        {
            var ledger = new TmsDemandLedger();
            var fields = new List<string>(8);
            TmsDemandLedger.DemandRecord demand;
            ledger.TryUpdatePayload("DEMAND;IRON;3;10;100", fields);

            bool valid = ledger.TryUpdatePayload(payload, fields);

            Assert.That(valid, Is.False);
            Assert.That(ledger.TryGet("IRON", out demand), Is.True);
            Assert.That(demand.DeficitKg, Is.EqualTo(10));
            Assert.That(demand.TargetStockKg, Is.EqualTo(100));
        }

        [Test]
        public void Tick_ExpiresDemandAfterThirtyUpdate10CallsWithoutRefresh()
        {
            var ledger = new TmsDemandLedger();
            var fields = new List<string>(8);
            TmsDemandLedger.DemandRecord demand;
            ledger.TryUpdatePayload("DEMAND;PLATINUM;5;100;200", fields);

            for (int i = 0; i < TmsDemandLedger.StaleAfterUpdate10Ticks - 1; i++)
                ledger.Tick();

            Assert.That(ledger.TryGet("PLATINUM", out demand), Is.True);
            ledger.Tick();

            Assert.That(ledger.TryGet("PLATINUM", out demand), Is.False);
            Assert.That(ledger.FreshCount, Is.EqualTo(0));
            Assert.That(ledger.DeficitCount, Is.EqualTo(0));
        }

        [Test]
        public void TryUpdatePayload_RefreshesOnlyTheMatchingMineral()
        {
            var ledger = new TmsDemandLedger();
            var fields = new List<string>(8);
            TmsDemandLedger.DemandRecord uranium;
            TmsDemandLedger.DemandRecord gold;
            ledger.TryUpdatePayload("DEMAND;URANIUM;5;100;200", fields);
            ledger.TryUpdatePayload("DEMAND;GOLD;5;50;75", fields);

            ledger.Tick();
            ledger.TryUpdatePayload("DEMAND;URANIUM;5;0;200", fields);

            Assert.That(ledger.TryGet("URANIUM", out uranium), Is.True);
            Assert.That(ledger.TryGet("GOLD", out gold), Is.True);
            Assert.That(uranium.DeficitKg, Is.EqualTo(0));
            Assert.That(gold.DeficitKg, Is.EqualTo(50));
            Assert.That(ledger.FreshCount, Is.EqualTo(2));
            Assert.That(ledger.DeficitCount, Is.EqualTo(1));
        }

        [Test]
        public void TryGetAt_ReturnsFixedMineralOrderAndFreshness()
        {
            var ledger = new TmsDemandLedger();
            var fields = new List<string>(8);
            TmsDemandLedger.DemandRecord demand;
            ledger.TryUpdatePayload("DEMAND;IRON;3;0;2500000", fields);

            Assert.That(ledger.TryGetAt(0, out demand), Is.True);
            Assert.That(demand.OreType, Is.EqualTo("URANIUM"));
            Assert.That(demand.Fresh, Is.False);
            Assert.That(ledger.TryGetAt(TmsDemandLedger.OreCount - 1, out demand), Is.True);
            Assert.That(demand.OreType, Is.EqualTo("IRON"));
            Assert.That(demand.Fresh, Is.True);
            Assert.That(ledger.TryGetAt(TmsDemandLedger.OreCount, out demand), Is.False);
        }
    }
}
```

## Done when
- `dotnet test Fleet.Tests` passes, including the verbatim ledger tests.
- `python3 tools/card-check.py C37` reports `RESULT: PASS`.
- `dotnet build scripts/Rig.OS.Lite -c Release` succeeds.
- Fresh deficits are visible; malformed or stale demand never appears as current.

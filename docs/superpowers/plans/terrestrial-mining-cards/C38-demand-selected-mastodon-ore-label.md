# C38 — Demand-selected Mastodon ore label

Pilot · Difficulty: medium · Executor: claude · Kind: wire

## Goal
Select the best fresh positive Base refinery deficit in RIG//OS-Lite and send only its ore label to
Mastodon. Apply changes immediately when not excavating; during excavation, keep only the latest
change pending until the next operator-issued `MACRO` command or until the operator leaves
excavation. The operator positions Mastodon over the deposit.

## Files
- Modify: `scripts/Shared/Net/TmsDemandLedger.cs`
- Modify: `scripts/Shared/Net/TmsProtocol.cs`
- Create: `scripts/Shared/Net/TmsOreTargetQueue.cs`
- Modify: `scripts/Rig.OS.Lite/Program.cs`
- Modify: `scripts/Rig.OS.Lite/Instructions.readme`
- Modify: `scripts/Excavator.Mastodon/Program.cs`
- Modify: `scripts/Excavator.Mastodon/Instructions.readme`
- Modify: `docs/protocol/igc.md`
- Modify: `docs/CHANGELOG.md`
- Modify: `docs/superpowers/plans/terrestrial-mining-cards/terrestrial-mining.md`
- Modify: `Fleet.Tests/Fleet.Tests.csproj`
- Create: `Fleet.Tests/Net/TmsOreTargetTests.cs`
- Create: `docs/superpowers/plans/terrestrial-mining-cards/C38-demand-selected-mastodon-ore-label.md`

## Contract
- Choose only a fresh demand with a positive deficit. Order candidates by priority descending,
  deficit descending, then the ledger's fixed order: Uranium, Platinum, Gold, Magnesium, Iron.
- Add `TMS.MAS.ORE_TARGET` with payload `TARGET;<ore_type>;<priority>` and clear sentinel
  `TARGET;NONE;0`. Reject unknown ores, invalid priorities, wrong field counts, empty fields, and
  malformed priority values.
- RIG//OS-Lite broadcasts only when ore type or priority changes, including the first clear sentinel
  and a clear when all demand is zero or stale. Show the last sent ore label and send status.
- Mastodon validates the payload and stores a latest-wins pending label while
  `STATE_EXCAVATING`. Apply it at the next operator-issued `MACRO` command or after a command leaves
  excavation. Apply labels immediately while not excavating.
- Display active and pending labels on Mastodon. Labels are data only: do not move the vehicle,
  locate deposits, alter the boom, or switch tools. The operator positions the Mastodon over ore.
- Do not change native safety behavior, block configuration, or the existing excavation macros.

## Hot paths
`UpdateFleetBusTick` selects and broadcasts only when the target changes.
`UpdateSubsystemsLoop` polls the pre-registered Mastodon label listener and updates the
fixed-size queue without hardware actuation.

## Tests — create `Fleet.Tests/Net/TmsOreTargetTests.cs` verbatim
```csharp
using System.Collections.Generic;
using System.Text;
using IngameScript;
using NUnit.Framework;

namespace Fleet.Tests.Net
{
    [TestFixture]
    public class TmsOreTargetTests
    {
        [Test]
        public void TryGetHighestPriorityDemand_PrefersPriorityOverLargerDeficit()
        {
            var ledger = new TmsDemandLedger();
            var fields = new List<string>(8);
            TmsDemandLedger.DemandRecord demand;
            ledger.TryUpdatePayload("DEMAND;IRON;3;100000;100000", fields);
            ledger.TryUpdatePayload("DEMAND;URANIUM;5;100;100000", fields);

            bool found = ledger.TryGetHighestPriorityDemand(out demand);

            Assert.That(found, Is.True);
            Assert.That(demand.OreType, Is.EqualTo("URANIUM"));
        }

        [Test]
        public void TryGetHighestPriorityDemand_BreaksSamePriorityByLargerDeficit()
        {
            var ledger = new TmsDemandLedger();
            var fields = new List<string>(8);
            TmsDemandLedger.DemandRecord demand;
            ledger.TryUpdatePayload("DEMAND;URANIUM;5;100;100000", fields);
            ledger.TryUpdatePayload("DEMAND;GOLD;5;200;100000", fields);

            bool found = ledger.TryGetHighestPriorityDemand(out demand);

            Assert.That(found, Is.True);
            Assert.That(demand.OreType, Is.EqualTo("GOLD"));
        }

        [Test]
        public void TryGetHighestPriorityDemand_UsesFixedMineralOrderForExactTie()
        {
            var ledger = new TmsDemandLedger();
            var fields = new List<string>(8);
            TmsDemandLedger.DemandRecord demand;
            ledger.TryUpdatePayload("DEMAND;GOLD;5;100;100000", fields);
            ledger.TryUpdatePayload("DEMAND;PLATINUM;5;100;100000", fields);
            ledger.TryUpdatePayload("DEMAND;URANIUM;5;100;100000", fields);

            bool found = ledger.TryGetHighestPriorityDemand(out demand);

            Assert.That(found, Is.True);
            Assert.That(demand.OreType, Is.EqualTo("URANIUM"));
        }

        [Test]
        public void TryGetHighestPriorityDemand_IgnoresZeroAndStaleDemand()
        {
            var ledger = new TmsDemandLedger();
            var fields = new List<string>(8);
            TmsDemandLedger.DemandRecord demand;
            ledger.TryUpdatePayload("DEMAND;URANIUM;5;0;100000", fields);
            ledger.TryUpdatePayload("DEMAND;IRON;3;100;100000", fields);
            for (int i = 0; i < TmsDemandLedger.StaleAfterUpdate10Ticks; i++)
                ledger.Tick();

            Assert.That(ledger.TryGetHighestPriorityDemand(out demand), Is.False);
        }

        [TestCase("URANIUM", 5, "TARGET;URANIUM;5")]
        [TestCase("IRON", 3, "TARGET;IRON;3")]
        [TestCase("NONE", 0, "TARGET;NONE;0")]
        public void OreTargetProtocol_RoundTripsCanonicalPayload(string oreType, int priority,
            string expectedPayload)
        {
            var buffer = new StringBuilder();
            var fields = new List<string>(4);
            string parsedOreType;
            int parsedPriority;

            Assert.That(TmsProtocol.TryWriteOreTarget(buffer, oreType, priority), Is.True);
            Assert.That(buffer.ToString(), Is.EqualTo(expectedPayload));
            Assert.That(TmsProtocol.TryReadOreTarget(expectedPayload, fields, out parsedOreType,
                out parsedPriority), Is.True);
            Assert.That(parsedOreType, Is.EqualTo(oreType));
            Assert.That(parsedPriority, Is.EqualTo(priority));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("TARGET;NICKEL;3")]
        [TestCase("TARGET;IRON;0")]
        [TestCase("TARGET;IRON;6")]
        [TestCase("TARGET;IRON;not-a-number")]
        [TestCase("TARGET;IRON")]
        [TestCase("TARGET;NONE;1")]
        [TestCase("TARGET;IRON;3;EXTRA")]
        public void OreTargetProtocol_RejectsMalformedPayload(string payload)
        {
            var fields = new List<string>(4);
            string oreType;
            int priority;

            Assert.That(TmsProtocol.TryReadOreTarget(payload, fields, out oreType, out priority),
                Is.False);
            Assert.That(oreType, Is.Empty);
            Assert.That(priority, Is.EqualTo(0));
        }

        [Test]
        public void OreTargetProtocol_RejectsInvalidWriteWithoutChangingBuffer()
        {
            var buffer = new StringBuilder("unchanged");

            bool unknownOre = TmsProtocol.TryWriteOreTarget(buffer, "NICKEL", 3);
            bool invalidClear = TmsProtocol.TryWriteOreTarget(buffer, "NONE", 1);

            Assert.That(unknownOre, Is.False);
            Assert.That(invalidClear, Is.False);
            Assert.That(buffer.ToString(), Is.EqualTo("unchanged"));
        }

        [Test]
        public void OreTargetQueue_AppliesImmediatelyWhenNotExcavating()
        {
            var queue = new TmsOreTargetQueue();

            bool valid = queue.TryQueue("IRON", 3, false);

            Assert.That(valid, Is.True);
            Assert.That(queue.Active.OreType, Is.EqualTo("IRON"));
            Assert.That(queue.Active.Priority, Is.EqualTo(3));
            Assert.That(queue.HasPending, Is.False);
        }

        [Test]
        public void OreTargetQueue_DefersUntilSafeBoundaryAndKeepsLatestPendingTarget()
        {
            var queue = new TmsOreTargetQueue();
            queue.TryQueue("IRON", 3, false);

            Assert.That(queue.TryQueue("URANIUM", 5, true), Is.True);
            Assert.That(queue.Active.OreType, Is.EqualTo("IRON"));
            Assert.That(queue.HasPending, Is.True);
            Assert.That(queue.TryQueue("GOLD", 5, true), Is.True);
            Assert.That(queue.Pending.OreType, Is.EqualTo("GOLD"));

            Assert.That(queue.ApplyPending(), Is.True);
            Assert.That(queue.Active.OreType, Is.EqualTo("GOLD"));
            Assert.That(queue.HasPending, Is.False);
            Assert.That(queue.ApplyPending(), Is.False);
        }

        [Test]
        public void OreTargetQueue_DefersClearSentinelAndRejectsInvalidTarget()
        {
            var queue = new TmsOreTargetQueue();
            queue.TryQueue("IRON", 3, false);

            Assert.That(queue.TryQueue("NONE", 0, true), Is.True);
            Assert.That(queue.Active.OreType, Is.EqualTo("IRON"));
            Assert.That(queue.Pending.OreType, Is.EqualTo("NONE"));
            Assert.That(queue.TryQueue("NICKEL", 3, true), Is.False);
            Assert.That(queue.Pending.OreType, Is.EqualTo("NONE"));

            queue.ApplyPending();
            Assert.That(queue.Active.OreType, Is.EqualTo("NONE"));
        }
    }
}
```

## Done when
- `dotnet test Fleet.Tests` passes, including the verbatim ore-target tests.
- `python3 tools/card-check.py C38` reports `RESULT: PASS`.
- Release builds of `scripts/Rig.OS.Lite` and `scripts/Excavator.Mastodon` succeed.
- `fish tools/check-size.fish` confirms both deployed scripts remain under 100,000 characters.

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

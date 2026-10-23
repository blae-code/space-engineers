# C36 — Base refinery ingot demand

Pilot · Difficulty: medium · Executor: claude · Kind: wire

## Goal
Broadcast measured Ground Base ingot deficits using the canonical demand payload. Preserve the
existing reserve targets, count ingots held anywhere on the Base PB's own grid, and do not make
RIG//OS or mining scripts move or actuate anything in this card.

## Files
- Create: `scripts/Shared/Net/TmsDemandMath.cs`
- Modify: `scripts/Shared/Net/TmsProtocol.cs`
- Modify: `scripts/Base.C2/Program.cs`
- Modify: `scripts/Base.C2/Instructions.readme`
- Modify: `docs/protocol/igc.md`
- Modify: `docs/CHANGELOG.md`
- Modify: `docs/superpowers/plans/terrestrial-mining-cards/terrestrial-mining.md`
- Modify: `Fleet.Tests/Fleet.Tests.csproj`
- Create: `Fleet.Tests/Net/TmsDemandMathTests.cs`
- Create: `docs/superpowers/plans/terrestrial-mining-cards/C36-base-ingot-demand.md`

## Contract
- Stock is the total ingot amount matching the ore's ingot subtype across every inventory-bearing
  terminal block on `Me.CubeGrid`; never inspect another grid.
- Cache the grid-scoped terminal block list and reuse a preallocated inventory-item list.
- Keep the existing `[Demand]` targets for Uranium, Platinum, Gold, and Magnesium in kg. Interpret
  `IronTons` as tonnes and convert to kg with checked range validation (`2500` → `2500000` kg).
- Use priority 5 for Uranium, Platinum, Gold, and Magnesium; priority 3 for Iron.
- Compute `deficit_kg = max(0, ceil(target_stock_kg - current_stock_kg))`. Invalid negative or
  non-finite stock inputs are rejected by the pure helper.
- Use `TmsProtocol.TryWriteBaseDemand` and broadcast one positional
  `DEMAND;<ore_type>;<priority>;<deficit_kg>;<target_stock_kg>` message per configured mineral,
  including zero-deficit messages so consumers can clear satisfied demand.
- Both periodic broadcast and `DEMAND_SYNC` use the same measured calculation. Only the active C2
  master broadcasts.
- Surface invalid or overflowing demand settings in Base C2 status; do not silently claim a
  successful demand refresh.
- Do not add movement, mining, docking, or hardware actuation.

## Hot paths
`Program.MeasureDemandStock` scans cached grid blocks with a reusable item buffer. Demand work runs
on the Update100 cycle or an operator-triggered refresh; no logic is added to flight or traction
control loops.

## Tests — create `Fleet.Tests/Net/TmsDemandMathTests.cs` verbatim
```csharp
using IngameScript;
using NUnit.Framework;
using System.Text;

namespace Fleet.Tests.Net
{
    [TestFixture]
    public class TmsDemandMathTests
    {
        [TestCase(500, 0.0, 500)]
        [TestCase(500, 250.0, 250)]
        [TestCase(500, 499.5, 1)]
        [TestCase(500, 500.0, 0)]
        [TestCase(500, 501.0, 0)]
        [TestCase(0, 0.0, 0)]
        public void TryCalculateDeficitKg_UsesCeilingAndNeverReturnsNegative(
            int targetKg, double stockKg, int expectedDeficitKg)
        {
            int deficitKg;

            bool valid = TmsDemandMath.TryCalculateDeficitKg(targetKg, stockKg, out deficitKg);

            Assert.That(valid, Is.True);
            Assert.That(deficitKg, Is.EqualTo(expectedDeficitKg));
        }

        [Test]
        public void TryCalculateDeficitKg_RejectsInvalidInputs()
        {
            int deficitKg;

            Assert.That(TmsDemandMath.TryCalculateDeficitKg(-1, 0.0, out deficitKg), Is.False);
            Assert.That(TmsDemandMath.TryCalculateDeficitKg(1, -1.0, out deficitKg), Is.False);
            Assert.That(TmsDemandMath.TryCalculateDeficitKg(1, double.NaN, out deficitKg), Is.False);
            Assert.That(TmsDemandMath.TryCalculateDeficitKg(1, double.PositiveInfinity, out deficitKg),
                Is.False);
            Assert.That(deficitKg, Is.EqualTo(0));
        }

        [TestCase("URANIUM", 5)]
        [TestCase("PLATINUM", 5)]
        [TestCase("GOLD", 5)]
        [TestCase("MAGNESIUM", 5)]
        [TestCase("IRON", 3)]
        public void TryGetPriority_ReturnsConfiguredPriority(string oreType, int expectedPriority)
        {
            int priority;

            bool valid = TmsDemandMath.TryGetPriority(oreType, out priority);

            Assert.That(valid, Is.True);
            Assert.That(priority, Is.EqualTo(expectedPriority));
        }

        [Test]
        public void TryGetPriority_RejectsUnknownOre()
        {
            int priority;

            Assert.That(TmsDemandMath.TryGetPriority("NICKEL", out priority), Is.False);
            Assert.That(priority, Is.EqualTo(0));
        }

        [Test]
        public void TryWriteBaseDemand_IntegerOverloadUsesCanonicalDigits()
        {
            var buffer = new StringBuilder();

            bool valid = TmsProtocol.TryWriteBaseDemand(buffer, "IRON", 3, 2500000, 5000000);

            Assert.That(valid, Is.True);
            Assert.That(buffer.ToString(), Is.EqualTo("DEMAND;IRON;3;2500000;5000000"));
        }
    }
}
```

## Done when
- `dotnet test Fleet.Tests` passes, including the verbatim math tests.
- `python3 tools/card-check.py C36` reports `RESULT: PASS`.
- Base C2 emits measured positional deficits for all five configured minerals and never scans another
  grid.
- `dotnet build Base.C2 -c Release` succeeds.

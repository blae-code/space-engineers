# C27 — EnergyIO (batteries, hydrogen, uranium, charging)

Milestone D · Difficulty: medium · Executor: local+review

## Goal
Read battery charge, hydrogen fill and reactor uranium; while docked, put batteries on **Recharge** and
hydrogen tanks on **Stockpile** until full; restore normal operation before leaving.

## Files
- Create: `Fleet.Drone.Miner/Io/EnergyIO.cs`
- Create: `Fleet.Tests/Mining/EnergyIOTests.cs` (pure helpers only)

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. `Refresh` may allocate; other methods must not.

## Space Engineers API you need
```csharp
using Sandbox.ModAPI.Ingame; using VRage.Game.ModAPI.Ingame;
IMyBatteryBlock b; b.CurrentStoredPower, b.MaxStoredPower (float, MWh); b.ChargeMode = ChargeMode.Recharge / ChargeMode.Auto;
IMyGasTank t; t.FilledRatio (double 0..1); t.Stockpile = true/false; t.BlockDefinition.SubtypeId (string)
IMyReactor r; r.GetInventory(0).GetItems(items); item.Type.SubtypeId == "Uranium"; item.Amount.RawValue (x1,000,000 = kg)
```

## Interface (implement exactly)
```csharp
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program
    {
        public class EnergyIO
        {
            public const double ChargedRatio = 0.95;
            public EnergyIO();
            public void Refresh(List<IMyBatteryBlock> batteries, List<IMyGasTank> tanks, List<IMyReactor> reactors); // keeps only hydrogen tanks
            public bool HasBattery { get; }
            public bool HasHydrogen { get; }
            public bool HasReactor { get; }
            public double BatteryFill();    // stored / max over all batteries, 0..1 (1 if none)
            public double HydrogenFill();   // mean FilledRatio of hydrogen tanks (1 if none)
            public double UraniumKg();      // total uranium in all reactors (0 if none)
            public void SetCharging(bool on);   // batteries Recharge/Auto, hydrogen tanks Stockpile on/off
            public bool IsCharged();            // battery (if any) and hydrogen (if any) >= ChargedRatio
            // pure helper (unit-tested)
            public static bool IsHydrogenTank(string subtypeId);   // contains "Hydrogen" (case-insensitive)
        }
    }
}
```

## Tests — create `Fleet.Tests/Mining/EnergyIOTests.cs` verbatim
```csharp
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class EnergyIOTests
    {
        [TestCase("SmallHydrogenTank", true)]
        [TestCase("SmallHydrogenTankSmall", true)]
        [TestCase("LargeHydrogenTankIndustrial", true)]
        [TestCase("OxygenTankSmall", false)]
        [TestCase("", false)]
        public void IsHydrogenTank(string subtype, bool expected)
        {
            Assert.That(EnergyIO.IsHydrogenTank(subtype), Is.EqualTo(expected));
        }

        [Test]
        public void NoBlocks_ReportsFullAndAbsent()
        {
            var e = new EnergyIO();
            Assert.That(e.HasBattery, Is.False);
            Assert.That(e.BatteryFill(), Is.EqualTo(1));
            Assert.That(e.HydrogenFill(), Is.EqualTo(1));
            Assert.That(e.UraniumKg(), Is.EqualTo(0));
            Assert.That(e.IsCharged(), Is.True);
            Assert.DoesNotThrow(() => e.SetCharging(true));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file; run `dotnet test Fleet.Tests --filter "FullyQualifiedName~EnergyIOTests" 2>&1 | tail -n 25` → build error.
- [ ] 2. Create `Fleet.Drone.Miner/Io/EnergyIO.cs`. `IsHydrogenTank`: `subtypeId.IndexOf("Hydrogen", StringComparison.OrdinalIgnoreCase) >= 0`.
- [ ] 3. Run the same command → `Passed!`.
- [ ] 4. `dotnet build Fleet.Drone.Miner -c Release 2>&1 | tail -n 15` → "successfully deployed".
- [ ] 5. Commit: `git add Fleet.Drone.Miner/Io/EnergyIO.cs Fleet.Tests/Mining/EnergyIOTests.cs; git commit -m "feat(miner): energy adapter"`

## Done when
Tests pass and the Release build is clean. In-game: checklist G10.

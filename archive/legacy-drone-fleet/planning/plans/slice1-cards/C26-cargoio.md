# C26 — CargoIO (fill level, ejection, unloading)

Milestone D · Difficulty: medium · Executor: local+review

## Goal
Measure how full the drone is, eject unwanted ores (default Stone) through ejector connectors while in the
job area, and — when docked — move all ore into the carrier's containers itself. Work is **amortised**: at
most a few item transfers per call, so no single tick spikes the instruction count.

## Files
- Create: `Fleet.Drone.Miner/Io/CargoIO.cs`
- Create: `Fleet.Tests/Mining/CargoIOTests.cs` (pure helpers only)

## Attach in Continue
This card, `Fleet.Engine/Config/Settings.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. Lists are fields created in the constructor; `Refresh` may allocate, per-tick
  methods must not (`string.Contains`/`==` do not allocate; `item.Type.SubtypeId` is an existing string).

## Space Engineers API you need
```csharp
using Sandbox.ModAPI.Ingame; using VRage.Game.ModAPI.Ingame; using VRage;
IMyTerminalBlock b; b.HasInventory; b.InventoryCount; IMyInventory inv = b.GetInventory(0);
inv.CurrentVolume.RawValue, inv.MaxVolume.RawValue      // long, fixed-point (x1,000,000)
List<MyInventoryItem> items; inv.GetItems(items);        // fills the list (clear it first)
MyInventoryItem it; it.Type.TypeId ("MyObjectBuilder_Ore"); it.Type.SubtypeId ("Stone"); it.Amount
bool moved = src.TransferItemTo(dst, it, null);          // null amount = as much as fits
bool can = src.CanTransferItemTo(dst, it.Type);          // conveyor path exists
IMyShipConnector c; c.ThrowOut = true;
IMyCargoContainer; b.CustomName.Contains(tag)
```

## Interface (implement exactly)
```csharp
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program
    {
        public class CargoIO
        {
            public const int TransfersPerCall = 4;
            public CargoIO();
            // own: the drone's inventory blocks (drills, containers, connectors). ejectors: drone connectors tagged
            // with the fleet tag and "Eject" in the name. Sets ThrowOut = true on ejectors.
            public void Refresh(List<IMyTerminalBlock> own, List<IMyShipConnector> ejectors);
            public double Fill();                                  // 0..1 over all own inventories
            public bool HasOre();                                  // any "MyObjectBuilder_Ore" item in own inventories
            // Moves up to TransfersPerCall stacks whose ore subtype is in s.Eject into ejectors. Returns transfers made.
            public int EjectStep(Settings s);
            // Moves up to TransfersPerCall ore stacks into 'targets' (containers on the carrier; tagged ones first).
            public int UnloadStep(List<IMyInventory> targets);
            // pure helpers (unit-tested)
            public static double FillFromRaw(long currentRaw, long maxRaw);   // 0 if maxRaw <= 0; clamp 0..1
            public static bool ShouldEject(string typeId, string subtypeId, List<string> eject); // ore only, case-insensitive
        }
    }
}
```

## Behaviour notes
- `Fill`: sum `CurrentVolume.RawValue` and `MaxVolume.RawValue` over every inventory of every own block,
  then `FillFromRaw`.
- Keep a round-robin index into the own-inventory list across calls so successive calls continue where the
  last stopped (amortisation).
- `UnloadStep` only moves items with `TypeId == "MyObjectBuilder_Ore"`, to the first target where
  `CanTransferItemTo` is true. The **caller** orders `targets` (tagged containers first); this class uses
  them in the given order.
- Ejector connectors are also in `own`: **never use an ejector's inventory as a source** in `EjectStep`
  or `UnloadStep` (keep a separate source list that excludes ejectors).

## Tests — create `Fleet.Tests/Mining/CargoIOTests.cs` verbatim
```csharp
using System.Collections.Generic;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class CargoIOTests
    {
        [Test]
        public void FillFromRaw()
        {
            Assert.That(CargoIO.FillFromRaw(500000, 1000000), Is.EqualTo(0.5).Within(1e-9));
            Assert.That(CargoIO.FillFromRaw(0, 0), Is.EqualTo(0));
            Assert.That(CargoIO.FillFromRaw(2000, 1000), Is.EqualTo(1));
            Assert.That(CargoIO.FillFromRaw(-5, 1000), Is.EqualTo(0));
        }

        [Test]
        public void ShouldEject()
        {
            var eject = new List<string> { "Stone", "Ice" };
            Assert.That(CargoIO.ShouldEject("MyObjectBuilder_Ore", "Stone", eject), Is.True);
            Assert.That(CargoIO.ShouldEject("MyObjectBuilder_Ore", "stone", eject), Is.True);
            Assert.That(CargoIO.ShouldEject("MyObjectBuilder_Ore", "Iron", eject), Is.False);
            Assert.That(CargoIO.ShouldEject("MyObjectBuilder_Ingot", "Stone", eject), Is.False);
            Assert.That(CargoIO.ShouldEject("MyObjectBuilder_Ore", "Stone", new List<string>()), Is.False);
        }
    }
}
```

## Steps
- [ ] 1. Create the test file; run `dotnet test Fleet.Tests --filter "FullyQualifiedName~CargoIOTests" 2>&1 | tail -n 25` → build error.
- [ ] 2. Create `Fleet.Drone.Miner/Io/CargoIO.cs`.
- [ ] 3. Run the same command → `Passed!`.
- [ ] 4. `dotnet build Fleet.Drone.Miner -c Release 2>&1 | tail -n 15` → "successfully deployed".
- [ ] 5. Commit: `git add Fleet.Drone.Miner/Io/CargoIO.cs Fleet.Tests/Mining/CargoIOTests.cs; git commit -m "feat(miner): cargo adapter"`

## Done when
Tests pass and the Release build is clean. In-game behaviour is verified at checklist items G4 and G6.

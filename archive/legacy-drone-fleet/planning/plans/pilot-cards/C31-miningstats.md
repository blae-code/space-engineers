# C31 — MiningStats (trips, ore delivered, working time)

Pilot · Difficulty: easy · Executor: local · Kind: logic

## Goal
A pure, zero-allocation record of a mining job: how many unload trips, how much ore was delivered
(the drop in ship mass across the Unload state), how long the drone spent working, and the rate in
kg/h, with save/load and a one-line LCD text. C32 wires it into the drone.

## Files
- Create: `Fleet.Drone.Miner/Mining/MiningStats.cs`
- Create: `Fleet.Tests/Mining/MiningStatsTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Core/Enums.cs`, `Fleet.Engine/Util/SbFormat.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, types nested and `public`.
- C# 6 only. No LINQ. No allocation in any method below. Use only `SbFormat` to put numbers into text.
- Needs `using System;`, `using System.Text;`, `using VRage.Game.ModAPI.Ingame.Utilities;` (for `MyIni`).

## Interface (implement exactly)
```csharp
public class MiningStats
{
    public int Trips { get; private set; }
    public double OreKg { get; private set; }
    public double JobSeconds { get; private set; }
    public double KgPerHour { get; }   // 0 while JobSeconds < 60, else OreKg * 3600 / JobSeconds
    public bool Any { get; }           // Trips > 0 || JobSeconds > 0

    public static bool IsWorking(MinerState s);   // false for Idle, Hold, Safe, Recording; true otherwise
    public void Reset();                          // all to 0, and forget the unload start and the last tick time
    public void Tick(double now, MinerState state);
    public void OnTransition(MinerState from, MinerState to, double shipMassKg);
    public void Save(MyIni ini, string section);  // keys "trips", "oreKg", "jobSeconds"
    public void Load(MyIni ini, string section);
    public StringBuilder Append(StringBuilder sb);
}
```

## Behaviour
- **Tick:** keep the previous `now` in a private field (start at -1). Add `now - previous` to
  `JobSeconds` only when there was a previous tick, `now >= previous`, and `IsWorking(state)`. Always
  store `now` as the new previous. (Time restarts at 0 after a reload, so a backwards step adds nothing.)
- **OnTransition:** entering `Unload` (from any other state) remembers `shipMassKg` as the unload start.
  Leaving `Unload` (to any other state): if a start is remembered, `Trips++` and
  `OreKg += Math.Max(0, start - shipMassKg)`. Either way forget the start (store -1).
- **Load:** call `Reset()`, then read the three keys with default 0; any negative value becomes 0
  (`Math.Max(0, …)`). `ToInt32(0)` / `ToDouble(0)` already turn junk into 0.
- **Append:** exactly `trips <Trips>  ore <round(OreKg)>kg  <rate>kg/h  <hours>h`, two spaces between
  fields. `<rate>` is `--` while `JobSeconds < 60`, else `round(KgPerHour)`. `<hours>` is
  `JobSeconds / 3600` with 1 decimal. Use `SbFormat.AppendInt(sb, (long)Math.Round(x))` for rounded
  integers and `SbFormat.AppendFixed(sb, x, 1)` for the hours. Return `sb`.

## Hot paths
`MiningStats.Tick`, `MiningStats.OnTransition`, `MiningStats.Append`

## Tests — create `Fleet.Tests/Mining/MiningStatsTests.cs` verbatim
```csharp
using System.Text;
using NUnit.Framework;
using VRage.Game.ModAPI.Ingame.Utilities;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class MiningStatsTests
    {
        static string Text(MiningStats m) { return m.Append(new StringBuilder()).ToString(); }

        [Test] public void Fresh_IsEmpty()
        {
            var m = new MiningStats();
            Assert.That(m.Any, Is.False);
            Assert.That(Text(m), Is.EqualTo("trips 0  ore 0kg  --kg/h  0.0h"));
        }

        [Test] public void IsWorking_ExcludesIdleHoldSafeRecording()
        {
            Assert.That(MiningStats.IsWorking(MinerState.Idle), Is.False);
            Assert.That(MiningStats.IsWorking(MinerState.Hold), Is.False);
            Assert.That(MiningStats.IsWorking(MinerState.Safe), Is.False);
            Assert.That(MiningStats.IsWorking(MinerState.Recording), Is.False);
            Assert.That(MiningStats.IsWorking(MinerState.Drill), Is.True);
            Assert.That(MiningStats.IsWorking(MinerState.Unload), Is.True);
            Assert.That(MiningStats.IsWorking(MinerState.Charge), Is.True);
        }

        [Test] public void Tick_CountsOnlyWorkingTime()
        {
            var m = new MiningStats();
            m.Tick(10, MinerState.Drill);          // first tick only records the time
            m.Tick(20, MinerState.Drill);          // +10
            m.Tick(50, MinerState.Hold);           // not working: +0
            m.Tick(55, MinerState.RouteBack);      // +5
            Assert.That(m.JobSeconds, Is.EqualTo(15).Within(1e-9));
            Assert.That(m.Any, Is.True);
        }

        [Test] public void Tick_TimeGoingBackwards_AddsNothing()
        {
            var m = new MiningStats();
            m.Tick(100, MinerState.Drill);
            m.Tick(3, MinerState.Drill);           // script restarted: Time counts from 0 again
            m.Tick(8, MinerState.Drill);           // +5
            Assert.That(m.JobSeconds, Is.EqualTo(5).Within(1e-9));
        }

        [Test] public void Unload_CountsTripAndMassDrop()
        {
            var m = new MiningStats();
            m.OnTransition(MinerState.Dock, MinerState.Unload, 5000);
            m.OnTransition(MinerState.Unload, MinerState.Charge, 3800);
            Assert.That(m.Trips, Is.EqualTo(1));
            Assert.That(m.OreKg, Is.EqualTo(1200).Within(1e-9));
            m.OnTransition(MinerState.Dock, MinerState.Unload, 4000);
            m.OnTransition(MinerState.Unload, MinerState.Undock, 4100);   // mass rose (refuelled): no negative ore
            Assert.That(m.Trips, Is.EqualTo(2));
            Assert.That(m.OreKg, Is.EqualTo(1200).Within(1e-9));
        }

        [Test] public void LeavingUnload_WithoutEntering_IsNotATrip()
        {
            var m = new MiningStats();
            m.OnTransition(MinerState.Unload, MinerState.Charge, 3000);   // e.g. a reload mid-unload
            Assert.That(m.Trips, Is.EqualTo(0));
            Assert.That(m.OreKg, Is.EqualTo(0));
        }

        [Test] public void Append_ShowsRateAfterOneMinute()
        {
            var m = new MiningStats();
            m.OnTransition(MinerState.Dock, MinerState.Unload, 2500);
            m.OnTransition(MinerState.Unload, MinerState.Charge, 1000);
            m.Tick(0, MinerState.Drill);
            m.Tick(1800, MinerState.Drill);
            Assert.That(m.KgPerHour, Is.EqualTo(3000).Within(1e-9));
            Assert.That(Text(m), Is.EqualTo("trips 1  ore 1500kg  3000kg/h  0.5h"));
        }

        [Test] public void Reset_ClearsEverything()
        {
            var m = new MiningStats();
            m.OnTransition(MinerState.Dock, MinerState.Unload, 2000);
            m.Tick(0, MinerState.Unload);
            m.Tick(100, MinerState.Unload);
            m.Reset();
            m.OnTransition(MinerState.Unload, MinerState.Charge, 1000);   // the unload start was forgotten too
            Assert.That(m.Any, Is.False);
            Assert.That(m.OreKg, Is.EqualTo(0));
        }

        [Test] public void SaveLoad_RoundTrips_AndBadValuesFallBackToZero()
        {
            var m = new MiningStats();
            m.OnTransition(MinerState.Dock, MinerState.Unload, 900);
            m.OnTransition(MinerState.Unload, MinerState.Charge, 400);
            m.Tick(0, MinerState.Drill);
            m.Tick(120, MinerState.Drill);
            var ini = new MyIni();
            m.Save(ini, "Miner.Stats");
            var n = new MiningStats();
            n.Load(ini, "Miner.Stats");
            Assert.That(n.Trips, Is.EqualTo(1));
            Assert.That(n.OreKg, Is.EqualTo(500).Within(1e-9));
            Assert.That(n.JobSeconds, Is.EqualTo(120).Within(1e-9));

            var bad = new MyIni();
            bad.TryParse("[Miner.Stats]\ntrips=-4\noreKg=junk\n");
            n.Load(bad, "Miner.Stats");
            Assert.That(n.Trips, Is.EqualTo(0));
            Assert.That(n.OreKg, Is.EqualTo(0));
            Assert.That(n.JobSeconds, Is.EqualTo(0));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~MiningStatsTests" 2>&1 | tail -n 25`
      Expected: build error "The type or namespace name 'MiningStats' could not be found".
- [ ] 3. Create `Fleet.Drone.Miner/Mining/MiningStats.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` — Failed: 0, Passed: 9.
- [ ] 5. Run `python3 tools/card-check.py C31`. Expected: `RESULT: PASS`.

## Done when
All MiningStatsTests pass. `Tick` contains no `new`. `Append` contains no `new`.

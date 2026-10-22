# C21 — ReturnTriggers (should the drone go home, and why?)

Milestone C · Difficulty: easy · Executor: local

## Goal
Evaluate every go-home condition in a fixed priority order and return the single most important reason.

## Files
- Create: `Fleet.Drone.Miner/Mining/ReturnTriggers.cs`
- Create: `Fleet.Tests/Mining/ReturnTriggersTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Config/Settings.cs`, `Fleet.Engine/Core/Enums.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, types nested and `public`.
- C# 6 only. No LINQ. No allocation.

## Interface (implement exactly)
```csharp
namespace IngameScript
{
    public partial class Program
    {
        public struct TriggerInput
        {
            public double CargoFill;     // 0..1
            public double LiftMargin;    // upward thrust / weight; +infinity in space
            public double BatteryFill, HydrogenFill;  // 0..1
            public double UraniumKg;
            public bool HasBattery, HasHydrogen, HasReactor;
            public bool Damaged, JobDone, ManualFull;
        }

        public static class ReturnTriggers
        {
            public static ReturnReason Evaluate(TriggerInput i, Settings s);
        }
    }
}
```

## Priority (first match wins)
1. `Damaged && s.OnDamage != DamagePolicy.Job` → `Damage`
2. `LiftMargin < s.MinLiftMargin` → `LowLift`
3. `HasBattery && BatteryFill * 100 < s.MinBattery` → `LowBattery`
4. `HasHydrogen && HydrogenFill * 100 < s.MinHydrogen` → `LowHydrogen`
5. `HasReactor && UraniumKg < s.MinUranium` → `LowUranium`
6. `CargoFill * 100 >= s.MaxLoad` → `CargoFull`
7. `ManualFull` → `Manual`
8. `JobDone` → `JobDone`
9. otherwise `None`

## Tests — create `Fleet.Tests/Mining/ReturnTriggersTests.cs` verbatim
```csharp
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class ReturnTriggersTests
    {
        Settings s;
        [SetUp] public void Init() { s = new Settings(); }

        static TriggerInput Healthy()
        {
            return new TriggerInput { CargoFill = 0.2, LiftMargin = double.PositiveInfinity, BatteryFill = 0.9, HydrogenFill = 0.9, UraniumKg = 50, HasBattery = true, HasHydrogen = true, HasReactor = true };
        }

        [Test] public void Healthy_IsNone() { Assert.That(ReturnTriggers.Evaluate(Healthy(), s), Is.EqualTo(ReturnReason.None)); }

        [Test] public void CargoFull_AtThreshold()
        {
            var i = Healthy(); i.CargoFill = 0.9;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.CargoFull));
        }

        [Test] public void LowLift()
        {
            var i = Healthy(); i.LiftMargin = 1.2;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.LowLift));
        }

        [Test] public void LowBattery_OnlyIfPresent()
        {
            var i = Healthy(); i.BatteryFill = 0.1;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.LowBattery));
            i.HasBattery = false;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.None));
        }

        [Test] public void LowHydrogen()
        {
            var i = Healthy(); i.HydrogenFill = 0.25;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.LowHydrogen));
        }

        [Test] public void LowUranium()
        {
            var i = Healthy(); i.UraniumKg = 4;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.LowUranium));
        }

        [Test] public void Damage_RespectsPolicy()
        {
            var i = Healthy(); i.Damaged = true;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.Damage));
            s.OnDamage = DamagePolicy.Stop;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.Damage));
            s.OnDamage = DamagePolicy.Job;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.None));
        }

        [Test] public void Manual_And_JobDone()
        {
            var i = Healthy(); i.ManualFull = true;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.Manual));
            i.ManualFull = false; i.JobDone = true;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.JobDone));
        }

        [Test] public void Priority_DamageBeatsEverything_LiftBeatsCargo()
        {
            var i = Healthy(); i.CargoFill = 1; i.LiftMargin = 1.0; i.BatteryFill = 0; i.JobDone = true;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.LowLift));
            i.Damaged = true;
            Assert.That(ReturnTriggers.Evaluate(i, s), Is.EqualTo(ReturnReason.Damage));
        }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~ReturnTriggersTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'ReturnTriggers' does not exist".
- [ ] 3. Create `Fleet.Drone.Miner/Mining/ReturnTriggers.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Drone.Miner/Mining/ReturnTriggers.cs Fleet.Tests/Mining/ReturnTriggersTests.cs; git commit -m "feat(miner): return triggers"`

## Done when
All ReturnTriggersTests pass.

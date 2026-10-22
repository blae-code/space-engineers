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

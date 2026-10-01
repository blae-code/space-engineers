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
            public static ReturnReason Evaluate(TriggerInput i, Settings s)
            {
                if (i.Damaged && s.OnDamage != DamagePolicy.Job) return ReturnReason.Damage;
                if (i.LiftMargin < s.MinLiftMargin) return ReturnReason.LowLift;
                if (i.HasBattery && i.BatteryFill * 100 < s.MinBattery) return ReturnReason.LowBattery;
                if (i.HasHydrogen && i.HydrogenFill * 100 < s.MinHydrogen) return ReturnReason.LowHydrogen;
                if (i.HasReactor && i.UraniumKg < s.MinUranium) return ReturnReason.LowUranium;
                if (i.CargoFill * 100 >= s.MaxLoad) return ReturnReason.CargoFull;
                if (i.ManualFull) return ReturnReason.Manual;
                if (i.JobDone) return ReturnReason.JobDone;
                return ReturnReason.None;
            }
        }
    }
}

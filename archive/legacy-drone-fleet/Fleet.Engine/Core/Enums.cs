namespace IngameScript
{
    public partial class Program
    {
        public enum MinerState : byte { Idle, Undock, DockPathOut, RouteOut, Position, Drill, Retract, RouteBack, DockPathIn, Dock, Unload, Charge, Recording, Hold, Safe }
        public enum ReturnReason : byte { None, CargoFull, LowLift, LowBattery, LowHydrogen, LowUranium, Damage, JobDone, Manual }
        public enum DamagePolicy : byte { Home, Job, Stop }
        public enum ReloadPolicy : byte { Resume, ReturnHome, Hold }
        public enum Cmd : byte { None, Unknown, Start, Stop, Home, Cont, Next, Prev, Full, RecordDock, RecordJob, StopRec, SetJob, Goto, Reboot, Reset, GyroTest, Up, Down, Apply, Back }

        public static class Names
        {
            public static readonly string[] State = { "Idle", "Undock", "DockPathOut", "RouteOut", "Position", "Drill", "Retract", "RouteBack", "DockPathIn", "Dock", "Unload", "Charge", "Recording", "Hold", "Safe" };
            public static readonly string[] Reason = { "", "CargoFull", "LowLift", "LowBattery", "LowHydrogen", "LowUranium", "Damage", "JobDone", "Manual" };
        }
    }
}

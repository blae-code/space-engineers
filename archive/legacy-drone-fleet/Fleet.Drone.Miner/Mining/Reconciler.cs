namespace IngameScript
{
    public partial class Program
    {
        public struct ReconcileInput
        {
            public MinerState Saved;
            public bool HomeFound, Connected, HasJob;
            public double CargoFill;            // 0..1
            public double DistToHoleEntrance;   // m, from the drill face to the current hole's entrance
            public double ShipSize;             // m
        }

        public static class Reconciler
        {
            public const string NoHome = "home connector not found";
            public const string PolicyHold = "reload policy: Hold";
            public const string NoJob = "no job defined";
            public const string WasRecording = "recording interrupted by reload";
            public const string WasSafe = "was SAFE before reload";
            public const string WasHold = "was holding before reload";

            // note: null unless one of the constants applies. redoHole: true when the current hole must be re-drilled from its entrance.
            public static MinerState Resolve(ReconcileInput i, ReloadPolicy policy, out string note, out bool redoHole)
            {
                note = null;
                redoHole = false;
                MinerState s = i.Saved;

                if (!i.HomeFound) { note = NoHome; return MinerState.Hold; }

                if (i.Connected)
                {
                    if (policy == ReloadPolicy.Hold) { note = PolicyHold; return MinerState.Idle; }
                    if (i.CargoFill > 0.01) return MinerState.Unload;
                    if (i.HasJob && s != MinerState.Idle && s != MinerState.Hold && s != MinerState.Safe && s != MinerState.Recording)
                        return MinerState.Charge;
                    return MinerState.Idle;
                }

                if (s == MinerState.Safe) { note = WasSafe; return MinerState.Hold; }
                if (s == MinerState.Hold) { note = WasHold; return MinerState.Hold; }
                if (s == MinerState.Recording) { note = WasRecording; return MinerState.Hold; }
                if (s == MinerState.Idle) return MinerState.Idle;

                if (policy == ReloadPolicy.Hold) { note = PolicyHold; return MinerState.Hold; }

                if (policy == ReloadPolicy.ReturnHome)
                {
                    switch (s)
                    {
                        case MinerState.Undock:
                        case MinerState.DockPathOut:
                        case MinerState.DockPathIn:
                        case MinerState.Dock:
                        case MinerState.Unload:
                        case MinerState.Charge:
                            return MinerState.DockPathIn;
                        default:
                            return MinerState.RouteBack;
                    }
                }

                // ReloadPolicy.Resume
                switch (s)
                {
                    case MinerState.Undock:
                        return MinerState.DockPathOut;
                    case MinerState.DockPathOut:
                    case MinerState.RouteOut:
                    case MinerState.RouteBack:
                    case MinerState.DockPathIn:
                        return s;
                    case MinerState.Dock:
                    case MinerState.Unload:
                    case MinerState.Charge:
                        return MinerState.Dock;
                    case MinerState.Position:
                        if (i.HasJob) return MinerState.Position;
                        note = NoJob;
                        return MinerState.Hold;
                    case MinerState.Drill:
                    case MinerState.Retract:
                        if (!i.HasJob) { note = NoJob; return MinerState.Hold; }
                        redoHole = true;
                        return i.DistToHoleEntrance > 2 * i.ShipSize ? MinerState.Position : MinerState.Retract;
                    default:
                        return MinerState.Hold;
                }
            }
        }
    }
}

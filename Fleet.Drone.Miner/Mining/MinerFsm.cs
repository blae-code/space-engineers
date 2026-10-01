namespace IngameScript
{
    public partial class Program
    {
        public struct MinerInput
        {
            public Cmd Command;          // this tick's operator command (None on most ticks)
            public bool HasJob, HasDockPath, Connected;
            public ReturnReason Trigger; // from ReturnTriggers.Evaluate; None = keep working
            public bool UndockClear;     // backed off far enough from the connector
            public bool PathDone;        // dock-path follower finished (DockPathOut / DockPathIn)
            public bool RouteDone;       // job-route leg finished (RouteOut / RouteBack)
            public bool AtHole;          // at the current hole entrance and aligned
            public bool DrillFinished;   // DrillLogic switched to Retract on its own (depth reached or blocked)
            public bool RetractDone;     // DrillLogic reached Done
            public bool JobComplete;     // no holes left
            public bool DockFailed, Stuck, Unloaded, Charged, UraniumLow;
        }

        public class MinerFsm
        {
            public const string NoteStopped = "stopped by operator";
            public const string NoteNoJob = "no job defined";
            public const string NoteNotDocked = "START requires the drone to be docked";
            public const string NoteStuck = "stuck";
            public const string NoteDockFailed = "docking failed";
            public const string NoteUranium = "uranium below minimum";
            public const string NoteDamage = "damage detected";

            public MinerFsm(MinerState initial)
            {
                State = initial;
                ResumeState = MinerState.Idle;
                PendingReason = ReturnReason.None;
                Note = null;
            }

            public MinerState State { get; private set; }
            public MinerState ResumeState { get; private set; }     // where CONT returns to from Hold
            public ReturnReason PendingReason { get; private set; } // why the drone is heading home
            public string Note { get; private set; }                // last note (null when none)

            public void Force(MinerState s, string note)
            {
                State = s;
                Note = note;
            }

            public bool Step(MinerInput i, Settings s)
            {
                if (ApplyCommand(i)) return true;
                if (ApplyFaults(i, s)) return true;
                return ApplyState(i, s);
            }

            void GoHold(string note)
            {
                ResumeState = State;
                State = MinerState.Hold;
                Note = note;
            }

            static bool IsPassive(MinerState st)
            {
                return st == MinerState.Idle || st == MinerState.Hold || st == MinerState.Safe;
            }

            static bool IsDockArea(MinerState st)
            {
                return st == MinerState.Undock || st == MinerState.DockPathOut
                    || st == MinerState.DockPathIn || st == MinerState.Dock;
            }

            bool GoHome(MinerState next)
            {
                PendingReason = ReturnReason.Manual;
                State = next;
                return true;
            }

            bool Go(MinerState next)
            {
                State = next;
                return true;
            }

            bool HoldWith(string note)
            {
                GoHold(note);
                return true;
            }

            bool ApplyCommand(MinerInput i)
            {
                switch (i.Command)
                {
                    case Cmd.Stop:
                        if (!IsPassive(State)) return HoldWith(NoteStopped);
                        return false;

                    case Cmd.Home:
                        switch (State)
                        {
                            case MinerState.Drill: return GoHome(MinerState.Retract);
                            case MinerState.Undock:
                            case MinerState.DockPathOut: return GoHome(MinerState.DockPathIn);
                            case MinerState.RouteOut:
                            case MinerState.Position: return GoHome(MinerState.RouteBack);
                            case MinerState.Hold:
                                return GoHome(IsDockArea(ResumeState) ? MinerState.DockPathIn : MinerState.RouteBack);
                        }
                        return false;

                    case Cmd.Start:
                    case Cmd.Cont:
                        if (State == MinerState.Idle)
                        {
                            if (!i.HasJob) return HoldWith(NoteNoJob);
                            if (!i.Connected) return HoldWith(NoteNotDocked);
                            PendingReason = ReturnReason.None;
                            return Go(MinerState.Undock);
                        }
                        if (i.Command == Cmd.Cont && State == MinerState.Hold
                            && !IsPassive(ResumeState) && ResumeState != MinerState.Recording)
                        {
                            Note = null;
                            return Go(ResumeState);
                        }
                        return false;

                    case Cmd.RecordDock:
                    case Cmd.RecordJob:
                        if (State == MinerState.Idle || State == MinerState.Hold) return Go(MinerState.Recording);
                        return false;

                    case Cmd.StopRec:
                        if (State == MinerState.Recording) return Go(MinerState.Idle);
                        return false;

                    case Cmd.Reset:
                        PendingReason = ReturnReason.None;
                        Note = null;
                        if (State != MinerState.Idle) return Go(MinerState.Idle);
                        return false;
                }
                return false;
            }

            bool ApplyFaults(MinerInput i, Settings s)
            {
                if (IsPassive(State) || State == MinerState.Recording) return false;

                if (i.Trigger == ReturnReason.Damage && s.OnDamage == DamagePolicy.Stop)
                    return HoldWith(NoteDamage);
                if (i.Stuck && State != MinerState.Drill)
                    return HoldWith(NoteStuck);
                return false;
            }

            bool ApplyState(MinerInput i, Settings s)
            {
                switch (State)
                {
                    case MinerState.Undock:
                        if (i.UndockClear) return Go(i.HasDockPath ? MinerState.DockPathOut : MinerState.RouteOut);
                        return false;

                    case MinerState.DockPathOut:
                        if (i.Trigger != ReturnReason.None)
                        {
                            PendingReason = i.Trigger;
                            return Go(MinerState.DockPathIn);
                        }
                        if (i.PathDone) return Go(MinerState.RouteOut);
                        return false;

                    case MinerState.RouteOut:
                        if (i.Trigger != ReturnReason.None)
                        {
                            PendingReason = i.Trigger;
                            return Go(MinerState.RouteBack);
                        }
                        if (i.RouteDone) return Go(MinerState.Position);
                        return false;

                    case MinerState.Position:
                        if (i.Trigger != ReturnReason.None)
                        {
                            PendingReason = i.Trigger;
                            return Go(MinerState.RouteBack);
                        }
                        if (i.JobComplete)
                        {
                            PendingReason = ReturnReason.JobDone;
                            return Go(MinerState.RouteBack);
                        }
                        if (i.AtHole) return Go(MinerState.Drill);
                        return false;

                    case MinerState.Drill:
                        if (i.Trigger != ReturnReason.None)
                        {
                            PendingReason = i.Trigger;
                            return Go(MinerState.Retract);
                        }
                        if (i.DrillFinished) return Go(MinerState.Retract);
                        return false;

                    case MinerState.Retract:
                        if (!i.RetractDone) return false;
                        if (PendingReason != ReturnReason.None) return Go(MinerState.RouteBack);
                        if (i.JobComplete)
                        {
                            PendingReason = ReturnReason.JobDone;
                            return Go(MinerState.RouteBack);
                        }
                        return Go(MinerState.Position);

                    case MinerState.RouteBack:
                        if (i.RouteDone) return Go(i.HasDockPath ? MinerState.DockPathIn : MinerState.Dock);
                        return false;

                    case MinerState.DockPathIn:
                        if (i.PathDone) return Go(MinerState.Dock);
                        return false;

                    case MinerState.Dock:
                        if (i.Connected) return Go(MinerState.Unload);
                        if (i.DockFailed) return HoldWith(NoteDockFailed);
                        return false;

                    case MinerState.Unload:
                        if (i.Unloaded) return Go(MinerState.Charge);
                        return false;

                    case MinerState.Charge:
                        if (i.UraniumLow) return HoldWith(NoteUranium);
                        if (!i.Charged) return false;
                        if (PendingReason == ReturnReason.JobDone
                            || PendingReason == ReturnReason.Damage
                            || !s.Loop)
                            return Go(MinerState.Idle);
                        PendingReason = ReturnReason.None;
                        return Go(MinerState.Undock);
                }
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program
    {
        // One message for the carrier adapter to send (unicast to 'To').
        public struct Outgoing
        {
            public long To;
            public string Kind;   // FleetMsg.Job / LeaseKind / Dock, or FleetLink.Command
            public string Text;
        }

        // The carrier's fleet brain (slice 3 spec §2-§5): inbound messages in, replies in Outbox. Pure: the
        // carrier Program owns IGC, blocks and Storage. Message handlers are rare paths and may allocate;
        // Tick runs every Update10 and allocates only when it actually sends something.
        public class Dispatcher
        {
            public readonly Roster Drones = new Roster();
            public readonly LeaseTable Leases = new LeaseTable();
            public readonly BayQueue Bays = new BayQueue();
            public readonly FleetJob Job = new FleetJob();
            public readonly List<Outgoing> Outbox = new List<Outgoing>();
            public int LeaseSize = 3;
            public double SilentSeconds = 300, ReserveSeconds = 90;
            public bool HasMarker;
            public string Note = "";
            public bool HasJob { get { return Job.JobId > 0; } }

            readonly FleetJob _scratch = new FleetJob();
            readonly StringBuilder _sb = new StringBuilder();
            int _nextJobId = 1;

            // A lead drone shares its job. The carrier mints the job id and echoes the job back, so the lead
            // holds exactly the job every other drone will receive.
            public void OnJob(long from, string line, double now)
            {
                if (!JobCodec.Decode(line, _scratch)) { Note = "bad job from drone"; return; }
                _scratch.JobId = _nextJobId++;
                Job.CopyFrom(_scratch);
                Leases.Start(Job.JobId, Job.HoleCount);
                Note = "job " + Job.JobId + ": " + Job.HoleCount + " holes";
                Send(from, FleetMsg.Job, JobCodec.Encode(Job));
            }

            // "N|2|heldJobId": a drone wants work. A drone not holding the current job gets the job first.
            public void OnLeaseRequest(long from, string line, double now)
            {
                int held;
                if (!FleetMsg.DecodeLeaseRequest(line, out held)) return;
                var l = new Lease();
                if (!HasJob) { SendLease(from, ref l); return; }
                if (held != Job.JobId) Send(from, FleetMsg.Job, JobCodec.Encode(Job));
                Leases.Grant(from, now, LeaseSize, ref l);
                SendLease(from, ref l);
            }

            // "R|2": a drone wants a bay. Without a hold marker a WAIT carries slot -1 (hold in place).
            public void OnDock(long from, string line, double now)
            {
                int type; long arg;
                if (!FleetMsg.DecodeDock(line, out type, out arg) || type != FleetMsg.DockRequest) return;
                type = Bays.Request(from, now, ReserveSeconds, out arg);
                SendDock(from, type, type == FleetMsg.DockWait && !HasMarker ? -1 : arg);
            }

            // Every status line: roster, and the lease it names is alive / has holes done.
            public void OnStatus(long from, string line, double now)
            {
                var e = Drones.Update(from, line, now);
                if (e == null || !HasJob || e.Status.JobId != Job.JobId) return;
                Leases.Heard(from, e.Status.LeaseId, e.Status.DoneMask, now);
            }

            // "Q|2|GPS|W|H" from the console or a probe: an idle docked drone becomes the lead. False (Note says
            // why) when the line is bad or no drone is free.
            public bool OnJobRequest(string line, double now)
            {
                string gps; int w, h;
                if (!FleetMsg.DecodeJobRequest(line, out gps, out w, out h)) { Note = "bad job request"; return false; }
                RosterEntry lead = null;
                for (int i = 0; i < Drones.Count && lead == null; i++)
                {
                    var s = Drones[i].Status;
                    if (s.State == (int)MinerState.Idle && (s.Flags & FleetLink.FlagConnected) != 0
                        && (s.Flags & FleetLink.FlagSafe) == 0) lead = Drones[i];
                }
                if (lead == null) { Note = "job request: no idle docked drone"; return false; }
                if (w > 0) Send(lead.Address, FleetLink.Command, "SET Miner Width " + w);
                if (h > 0) Send(lead.Address, FleetLink.Command, "SET Miner Height " + h);
                Send(lead.Address, FleetLink.Command, "GOTO " + gps);
                Send(lead.Address, FleetLink.Command, "SHARE");
                Note = "job request: lead " + lead.Status.Name;
                return true;
            }

            // FLEET START: START to every docked drone that holds the current job. Returns how many.
            public int FleetStart()
            {
                int n = 0;
                if (!HasJob) return 0;
                for (int i = 0; i < Drones.Count; i++)
                {
                    var s = Drones[i].Status;
                    if (s.JobId != Job.JobId || (s.Flags & FleetLink.FlagConnected) == 0) continue;
                    Send(Drones[i].Address, FleetLink.Command, "START");
                    n++;
                }
                return n;
            }

            // FLEET STOP: STOP to every drone on the roster. Returns how many.
            public int FleetStop()
            {
                for (int i = 0; i < Drones.Count; i++) Send(Drones[i].Address, FleetLink.Command, "STOP");
                return Drones.Count;
            }

            // Every Update10: silent leases expire; a freed bay goes to the head of the queue, and the drones
            // behind it are told their new slot.
            public void Tick(double now)
            {
                Drones.Expire(now);
                Leases.Expire(now, SilentSeconds);
                long drone, bay;
                if (!Bays.Tick(now, ReserveSeconds, out drone, out bay)) return;
                SendDock(drone, FleetMsg.DockGo, bay);
                for (int slot = 0; slot < Bays.Waiting; slot++) SendDock(Bays.QueuedAt(slot), FleetMsg.DockWait, HasMarker ? slot : -1);
            }

            // Rare path (save): the job, the next job id and the lease table. Bays and the queue are not saved:
            // after a reload drones simply ask again.
            public void Save(MyIni ini)
            {
                ini.Set("Dispatch", "job", HasJob ? JobCodec.Encode(Job) : "");
                ini.Set("Dispatch", "nextJobId", _nextJobId);
                if (HasJob) Leases.Save(ini, "Dispatch.Leases");
            }

            // Rare path (load). Leases come back as heard 'now' (grace period). A lease table that does not match
            // the job starts the job afresh. False = no job.
            public bool Load(MyIni ini, double now)
            {
                _nextJobId = Math.Max(1, ini.Get("Dispatch", "nextJobId").ToInt32(1));
                Bays.Clear();
                Job.JobId = 0;
                if (!JobCodec.Decode(ini.Get("Dispatch", "job").ToString(""), _scratch) || _scratch.JobId <= 0)
                {
                    Leases.Start(0, 0);
                    return false;
                }
                Job.CopyFrom(_scratch);
                if (!Leases.Load(ini, "Dispatch.Leases", now) || Leases.JobId != Job.JobId || Leases.Holes != Job.HoleCount)
                    Leases.Start(Job.JobId, Job.HoleCount);
                if (_nextJobId <= Job.JobId) _nextJobId = Job.JobId + 1;
                return true;
            }

            void SendLease(long to, ref Lease l)
            {
                _sb.Clear();
                Send(to, FleetMsg.LeaseKind, FleetMsg.EncodeLease(_sb, ref l).ToString());
            }

            void SendDock(long to, int type, long arg)
            {
                _sb.Clear();
                Send(to, FleetMsg.Dock, FleetMsg.EncodeDock(_sb, type, arg).ToString());
            }

            void Send(long to, string kind, string text)
            {
                Outbox.Add(new Outgoing { To = to, Kind = kind, Text = text });
            }
        }
    }
}

using System;
using System.Text;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program
    {
        // Carrier side: which drone may mine which hole of the shared job (slice 3 spec §4). Holes are spiral
        // indices. A lease is renewed by every status line that names it and expires only by silence.
        public class LeaseTable
        {
            public const int MaxHoles = 2500, MaxLeases = 32;
            public const byte Free = 0, Leased = 1, Done = 2;

            readonly byte[] _state = new byte[MaxHoles];
            readonly int[] _queue = new int[MaxHoles];   // the Free holes, in grant order (circular)
            int _head, _queued;
            readonly Lease[] _lease = new Lease[MaxLeases];   // Count 0 = slot unused
            readonly long[] _drone = new long[MaxLeases];
            readonly double[] _heard = new double[MaxLeases];
            int _nextId = 1;

            public int JobId { get; private set; }
            public int Holes { get; private set; }
            public int DoneCount { get; private set; }
            public int LeasedCount { get; private set; }
            public int FreeCount { get { return _queued; } }
            public bool Complete { get { return Holes > 0 && DoneCount == Holes; } }
            public int NextLeaseId { get { return _nextId; } }

            public int StateOf(int index) { return index >= 0 && index < Holes ? _state[index] : -1; }

            // A new job: every hole Free in spiral order, no leases. Lease ids keep counting across jobs.
            public void Start(int jobId, int holes)
            {
                JobId = jobId;
                Holes = Math.Max(0, Math.Min(holes, MaxHoles));
                for (int i = 0; i < Holes; i++) { _state[i] = Free; _queue[i] = i; }
                _head = 0; _queued = Holes;
                DoneCount = 0; LeasedCount = 0;
                for (int s = 0; s < MaxLeases; s++) _lease[s].Count = 0;
            }

            // The drone's current lease while it still has unfinished holes (same id: a lost reply is harmless),
            // else a new lease of up to 'size' Free holes. Count 0 = nothing left to lease.
            public void Grant(long drone, double now, int size, ref Lease lease)
            {
                int s = SlotOf(drone);
                if (s >= 0)
                {
                    if (Unfinished(s)) { _heard[s] = now; lease = _lease[s]; return; }
                    _lease[s].Count = 0;
                }
                lease.JobId = JobId; lease.LeaseId = 0; lease.Count = 0;
                if (_queued == 0) return;
                s = -1;
                for (int i = 0; i < MaxLeases; i++) if (_lease[i].Count == 0) { s = i; break; }
                if (s < 0) return;
                int n = Math.Min(Math.Max(size, 1), Math.Min(FleetMsg.MaxLease, _queued));
                var l = new Lease { JobId = JobId, LeaseId = _nextId++, Count = n };
                for (int k = 0; k < n; k++)
                {
                    int hole = _queue[_head];
                    _head = (_head + 1) % MaxHoles; _queued--;
                    _state[hole] = Leased;
                    l[k] = hole;
                }
                LeasedCount += n;
                _lease[s] = l; _drone[s] = drone; _heard[s] = now;
                lease = l;
            }

            // A status line from 'drone' naming 'leaseId': it is alive, and mask bit k = its k-th hole is done.
            public void Heard(long drone, int leaseId, int doneMask, double now)
            {
                if (leaseId == 0) return;
                for (int s = 0; s < MaxLeases; s++)
                {
                    if (_lease[s].Count == 0 || _lease[s].LeaseId != leaseId || _drone[s] != drone) continue;
                    _heard[s] = now;
                    for (int k = 0; k < _lease[s].Count; k++)
                    {
                        int hole = _lease[s][k];
                        if ((doneMask & (1 << k)) != 0 && _state[hole] == Leased)
                        {
                            _state[hole] = Done; LeasedCount--; DoneCount++;
                        }
                    }
                    return;
                }
            }

            // Leases silent for 'silentSeconds' give their unfinished holes back, at the END of the free order.
            public void Expire(double now, double silentSeconds)
            {
                for (int s = 0; s < MaxLeases; s++)
                {
                    if (_lease[s].Count == 0 || now - _heard[s] < silentSeconds) continue;
                    for (int k = 0; k < _lease[s].Count; k++)
                    {
                        int hole = _lease[s][k];
                        if (_state[hole] != Leased) continue;
                        _state[hole] = Free; LeasedCount--;
                        _queue[(_head + _queued) % MaxHoles] = hole; _queued++;
                    }
                    _lease[s].Count = 0;
                }
            }

            int SlotOf(long drone)
            {
                for (int s = 0; s < MaxLeases; s++) if (_lease[s].Count > 0 && _drone[s] == drone) return s;
                return -1;
            }

            bool Unfinished(int s)
            {
                for (int k = 0; k < _lease[s].Count; k++) if (_state[_lease[s][k]] == Leased) return true;
                return false;
            }

            // Rare path (save): holes as one '0'/'1'/'2' character each, one key per live lease.
            public void Save(MyIni ini, string section)
            {
                ini.Set(section, "jobId", JobId);
                ini.Set(section, "holes", Holes);
                ini.Set(section, "nextId", _nextId);
                var sb = new StringBuilder(Holes);
                for (int i = 0; i < Holes; i++) sb.Append((char)('0' + _state[i]));
                ini.Set(section, "state", sb.ToString());
                int n = 0;
                for (int s = 0; s < MaxLeases; s++)
                {
                    if (_lease[s].Count == 0) continue;
                    var l = _lease[s];
                    ini.Set(section, "lease" + n, _drone[s] + "," + l.LeaseId + "," + l.Count + "," + l.I0 + "," + l.I1 + "," + l.I2 + "," + l.I3);
                    n++;
                }
                ini.Set(section, "leases", n);
            }

            // Rare path (load). Leases come back as last heard 'now' (a grace period after a reload). The free
            // order is rebuilt in spiral order. False (empty table, no job) on missing or garbled data.
            public bool Load(MyIni ini, string section, double now)
            {
                Start(0, 0);
                int jobId = ini.Get(section, "jobId").ToInt32(0), holes = ini.Get(section, "holes").ToInt32(-1);
                string state = ini.Get(section, "state").ToString("");
                if (jobId <= 0 || holes < 1 || holes > MaxHoles || state.Length != holes) return false;
                for (int i = 0; i < holes; i++) if (state[i] < '0' || state[i] > '2') return false;
                Start(jobId, holes);
                _nextId = Math.Max(1, ini.Get(section, "nextId").ToInt32(1));
                int leases = Math.Min(ini.Get(section, "leases").ToInt32(0), MaxLeases), slot = 0;
                for (int n = 0; n < leases; n++)
                {
                    var f = ini.Get(section, "lease" + n).ToString("").Split(',');
                    long drone; int id, count;
                    if (f.Length != 7 || !long.TryParse(f[0], out drone) || !int.TryParse(f[1], out id)
                        || !int.TryParse(f[2], out count) || count < 1 || count > FleetMsg.MaxLease) continue;
                    var l = new Lease { JobId = jobId, LeaseId = id, Count = count };
                    bool ok = true;
                    for (int k = 0; k < count; k++)
                    {
                        int hole;
                        if (!int.TryParse(f[3 + k], out hole) || hole < 0 || hole >= holes) { ok = false; break; }
                        l[k] = hole;
                    }
                    if (!ok) continue;
                    _lease[slot] = l; _drone[slot] = drone; _heard[slot] = now; slot++;
                }
                _queued = 0; _head = 0;
                for (int i = 0; i < holes; i++)
                {
                    if (state[i] == '2') { _state[i] = Done; DoneCount++; }
                    else if (state[i] == '1' && LeasedBySlot(i)) { _state[i] = Leased; LeasedCount++; }
                    else { _state[i] = Free; _queue[_queued++] = i; }
                }
                return true;
            }

            bool LeasedBySlot(int hole)
            {
                for (int s = 0; s < MaxLeases; s++)
                    for (int k = 0; k < _lease[s].Count; k++) if (_lease[s][k] == hole) return true;
                return false;
            }
        }
    }
}

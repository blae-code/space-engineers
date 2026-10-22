using System.Collections.Generic;

namespace IngameScript
{
    public partial class Program
    {
        // Carrier side: which drone may dock on which bay, and who waits (slice 3 spec §5). Any free bay is
        // reserved for a requesting drone; when none is free the drone joins a FIFO queue of holding slots.
        public class BayQueue
        {
            public const int MaxBays = 16, MaxQueue = 32;

            readonly long[] _bay = new long[MaxBays];
            readonly bool[] _occupied = new bool[MaxBays];
            readonly long[] _resDrone = new long[MaxBays];      // 0 = not reserved
            readonly double[] _resUntil = new double[MaxBays];
            readonly long[] _queue = new long[MaxQueue];
            readonly long[] _oldBay = new long[MaxBays];
            readonly bool[] _oldOcc = new bool[MaxBays];
            readonly long[] _oldDrone = new long[MaxBays];
            readonly double[] _oldUntil = new double[MaxBays];
            int _bays, _waiting;

            public int BayCount { get { return _bays; } }
            public int Waiting { get { return _waiting; } }

            // The carrier's current bays (rare: setup / block rescan). State is kept for ids still present.
            public void SetBays(List<long> ids)
            {
                int old = _bays;
                for (int i = 0; i < old; i++)
                {
                    _oldBay[i] = _bay[i]; _oldOcc[i] = _occupied[i]; _oldDrone[i] = _resDrone[i]; _oldUntil[i] = _resUntil[i];
                }
                _bays = 0;
                for (int n = 0; n < ids.Count && _bays < MaxBays; n++)
                {
                    int b = _bays++;
                    _bay[b] = ids[n]; _occupied[b] = false; _resDrone[b] = 0; _resUntil[b] = 0;
                    for (int i = 0; i < old; i++)
                        if (_oldBay[i] == ids[n]) { _occupied[b] = _oldOcc[i]; _resDrone[b] = _oldDrone[i]; _resUntil[b] = _oldUntil[i]; }
                }
            }

            // From the connector: something is docked on the bay. Docking ends the reservation.
            public void SetOccupied(long bayId, bool occupied)
            {
                int b = IndexOf(bayId);
                if (b < 0) return;
                _occupied[b] = occupied;
                if (occupied) _resDrone[b] = 0;
            }

            // A drone asks for a bay. Returns FleetMsg.DockGo (arg = bay id) or FleetMsg.DockWait (arg = slot,
            // -1 = queue full). Idempotent: a drone that holds a reservation or a slot gets the same answer.
            public int Request(long drone, double now, double reserveSeconds, out long arg)
            {
                for (int b = 0; b < _bays; b++)
                    if (_resDrone[b] == drone && now < _resUntil[b]) { arg = _bay[b]; return FleetMsg.DockGo; }
                int slot = SlotOf(drone);
                if (slot >= 0) { arg = slot; return FleetMsg.DockWait; }
                int free = FreeBay(now);
                if (free >= 0)
                {
                    Reserve(free, drone, now, reserveSeconds);
                    arg = _bay[free];
                    return FleetMsg.DockGo;
                }
                if (_waiting >= MaxQueue) { arg = -1; return FleetMsg.DockWait; }
                _queue[_waiting] = drone;
                arg = _waiting++;
                return FleetMsg.DockWait;
            }

            // Drops lapsed reservations, then gives the first free bay to the head of the queue. True (with the
            // drone and bay to send GO to) when it did; at most one promotion per call.
            public bool Tick(double now, double reserveSeconds, out long drone, out long bayId)
            {
                drone = 0; bayId = 0;
                for (int b = 0; b < _bays; b++) if (_resDrone[b] != 0 && now >= _resUntil[b]) _resDrone[b] = 0;
                if (_waiting == 0) return false;
                int free = FreeBay(now);
                if (free < 0) return false;
                drone = _queue[0];
                RemoveAt(0);
                Reserve(free, drone, now, reserveSeconds);
                bayId = _bay[free];
                return true;
            }

            public int SlotOf(long drone)
            {
                for (int i = 0; i < _waiting; i++) if (_queue[i] == drone) return i;
                return -1;
            }

            // The drone waiting in 'slot' (0 = next in line), or 0.
            public long QueuedAt(int slot) { return slot >= 0 && slot < _waiting ? _queue[slot] : 0; }

            public long ReservedBay(long drone, double now)
            {
                for (int b = 0; b < _bays; b++) if (_resDrone[b] == drone && now < _resUntil[b]) return _bay[b];
                return 0;
            }

            // A drone that is gone (silent) or no longer needs a bay: out of the queue, reservation released.
            public void Forget(long drone)
            {
                int slot = SlotOf(drone);
                if (slot >= 0) RemoveAt(slot);
                for (int b = 0; b < _bays; b++) if (_resDrone[b] == drone) _resDrone[b] = 0;
            }

            // Every reservation and the whole queue (after a reload: drones re-request).
            public void Clear()
            {
                for (int b = 0; b < _bays; b++) _resDrone[b] = 0;
                _waiting = 0;
            }

            int IndexOf(long bayId)
            {
                for (int b = 0; b < _bays; b++) if (_bay[b] == bayId) return b;
                return -1;
            }

            int FreeBay(double now)
            {
                for (int b = 0; b < _bays; b++)
                    if (!_occupied[b] && (_resDrone[b] == 0 || now >= _resUntil[b])) return b;
                return -1;
            }

            void Reserve(int b, long drone, double now, double reserveSeconds)
            {
                _resDrone[b] = drone;
                _resUntil[b] = now + reserveSeconds;
            }

            void RemoveAt(int slot)
            {
                for (int i = slot + 1; i < _waiting; i++) _queue[i - 1] = _queue[i];
                _waiting--;
            }
        }
    }
}

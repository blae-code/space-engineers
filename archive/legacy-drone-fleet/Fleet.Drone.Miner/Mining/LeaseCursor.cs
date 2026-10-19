using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program
    {
        // Drone side of a fleet job (slice 3 spec §6): the leased holes and which of them are done. Replaces the
        // sequential HoleIndex walk while a fleet job is active.
        public class LeaseCursor
        {
            Lease _l;
            int _mask;

            public int JobId { get { return _l.JobId; } }
            public int LeaseId { get { return _l.LeaseId; } }
            public int Count { get { return _l.Count; } }
            public int DoneMask { get { return _mask; } }
            public bool Active { get { return _l.Count > 0; } }
            public bool Complete { get { return Active && _mask == (1 << _l.Count) - 1; } }

            // A lease from the carrier. The same lease again (a re-sent reply) keeps the done mask.
            public void Set(ref Lease l)
            {
                if (Active && l.JobId == _l.JobId && l.LeaseId == _l.LeaseId) return;
                _l = l;
                _mask = 0;
            }

            // The first leased hole not yet done, or -1.
            public int Next()
            {
                for (int k = 0; k < _l.Count; k++) if ((_mask & (1 << k)) == 0) return _l[k];
                return -1;
            }

            // True when 'hole' belongs to this lease.
            public bool MarkDone(int hole)
            {
                for (int k = 0; k < _l.Count; k++)
                    if (_l[k] == hole) { _mask |= 1 << k; return true; }
                return false;
            }

            public void Clear() { _l = new Lease(); _mask = 0; }

            // Rare path: one "job,lease,count,i0,i1,i2,i3,mask" value.
            public void Save(MyIni ini, string section)
            {
                ini.Set(section, "lease", _l.JobId + "," + _l.LeaseId + "," + _l.Count + "," + _l.I0 + "," + _l.I1 + "," + _l.I2 + "," + _l.I3 + "," + _mask);
            }

            // Rare path. Missing or garbled -> no lease.
            public void Load(MyIni ini, string section)
            {
                Clear();
                var f = ini.Get(section, "lease").ToString("").Split(',');
                if (f.Length != 8) return;
                var n = new int[8];
                for (int i = 0; i < 8; i++) if (!int.TryParse(f[i], out n[i])) return;
                if (n[2] < 0 || n[2] > FleetMsg.MaxLease || n[7] < 0 || (n[2] > 0 && n[7] >= (1 << n[2]))) return;
                for (int k = 0; k < n[2]; k++) if (n[3 + k] < 0) return;
                _l = new Lease { JobId = n[0], LeaseId = n[1], Count = n[2], I0 = n[3], I1 = n[4], I2 = n[5], I3 = n[6] };
                _mask = n[2] > 0 ? n[7] : 0;
            }
        }
    }
}

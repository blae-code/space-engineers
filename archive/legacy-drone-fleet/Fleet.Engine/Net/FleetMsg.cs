using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        // A batch of up to four spiral hole indices leased to one drone (slice 3 spec §4).
        public struct Lease
        {
            public int JobId, LeaseId, Count;
            public int I0, I1, I2, I3;

            public int this[int k]
            {
                get { return k == 0 ? I0 : k == 1 ? I1 : k == 2 ? I2 : I3; }
                set { if (k == 0) I0 = value; else if (k == 1) I1 = value; else if (k == 2) I2 = value; else I3 = value; }
            }
        }

        // Slice 3 message kinds and the small fixed-format lines (spec §3). Version 2 framing like status.
        public static class FleetMsg
        {
            public const string Job = "job", JobReq = "jobreq", LeaseKind = "lease", Dock = "dock";
            public const int MaxLease = 4;
            public const int DockRequest = 0, DockGo = 1, DockWait = 2;

            // "L|2|jobId|leaseId|n|i…" (carrier -> drone). n = 0: nothing left to lease.
            public static StringBuilder EncodeLease(StringBuilder sb, ref Lease l)
            {
                sb.Append("L|2|");
                SbFormat.AppendInt(sb, l.JobId).Append('|');
                SbFormat.AppendInt(sb, l.LeaseId).Append('|');
                SbFormat.AppendInt(sb, l.Count);
                for (int k = 0; k < l.Count; k++) SbFormat.AppendInt(sb.Append('|'), l[k]);
                return sb;
            }

            public static bool DecodeLease(string line, ref Lease l)
            {
                int pos;
                if (!Head(line, 'L', out pos)) return false;
                long job, id, n;
                if (!ReadLong(line, ref pos, out job) || !ReadLong(line, ref pos, out id) || !ReadLong(line, ref pos, out n)) return false;
                if (n < 0 || n > MaxLease) return false;
                var t = new Lease { JobId = (int)job, LeaseId = (int)id, Count = (int)n };
                for (int k = 0; k < n; k++)
                {
                    long i;
                    if (!ReadLong(line, ref pos, out i) || i < 0) return false;
                    t[k] = (int)i;
                }
                if (pos <= line.Length) return false;   // trailing fields
                l = t;
                return true;
            }

            // "N|2|jobId" (drone -> carrier): I need a lease; jobId is the job I hold (0 = none).
            public static StringBuilder EncodeLeaseRequest(StringBuilder sb, int jobId)
            {
                return SbFormat.AppendInt(sb.Append("N|2|"), jobId);
            }

            public static bool DecodeLeaseRequest(string line, out int jobId)
            {
                jobId = 0;
                int pos; long j;
                if (!Head(line, 'N', out pos) || !ReadLong(line, ref pos, out j) || pos <= line.Length) return false;
                jobId = (int)j;
                return true;
            }

            // "R|2" (drone: request a bay) · "G|2|bayId" (carrier: go) · "W|2|slot" (carrier: wait; -1 = hold in place)
            public static StringBuilder EncodeDock(StringBuilder sb, int type, long arg)
            {
                if (type == DockRequest) return sb.Append("R|2");
                sb.Append(type == DockGo ? "G|2|" : "W|2|");
                return SbFormat.AppendInt(sb, arg);
            }

            public static bool DecodeDock(string line, out int type, out long arg)
            {
                type = -1; arg = 0;
                if (line == "R|2") { type = DockRequest; return true; }
                int pos;
                if (Head(line, 'G', out pos)) type = DockGo;
                else if (Head(line, 'W', out pos)) type = DockWait;
                else return false;
                if (!ReadLong(line, ref pos, out arg) || pos <= line.Length) { type = -1; arg = 0; return false; }
                return true;
            }

            // "Q|2|GPS:name:x:y:z:|W|H" (console or probe -> carrier). Null when the GPS text is unusable. Rare path.
            public static string EncodeJobRequest(string gps, int width, int height)
            {
                if (gps == null || !gps.StartsWith("GPS:") || gps.IndexOf('|') >= 0 || gps.IndexOf('\n') >= 0) return null;
                return "Q|2|" + gps + "|" + width + "|" + height;
            }

            public static bool DecodeJobRequest(string line, out string gps, out int width, out int height)
            {
                gps = null; width = 0; height = 0;
                if (line == null) return false;
                var f = line.Split('|');
                if (f.Length != 5 || f[0] != "Q" || f[1] != "2" || !f[2].StartsWith("GPS:")) return false;
                int w, h;
                if (!int.TryParse(f[3], out w) || !int.TryParse(f[4], out h)) return false;
                if (w < 0 || w > JobCodec.MaxSide || h < 0 || h > JobCodec.MaxSide) return false;
                gps = f[2]; width = w; height = h;
                return true;
            }

            // "<kind>|2|" prefix; pos is set just past it.
            static bool Head(string line, char kind, out int pos)
            {
                pos = 4;
                return line != null && line.Length >= 4 && line[0] == kind && line[1] == '|' && line[2] == '2' && line[3] == '|';
            }

            // Optionally negative integer at pos, then '|' (consumed) or end of line (pos = Length + 1).
            static bool ReadLong(string line, ref int pos, out long value)
            {
                value = 0;
                if (pos >= line.Length) return false;
                bool neg = line[pos] == '-';
                int i = neg ? pos + 1 : pos, digits = 0;
                while (i < line.Length && line[i] >= '0' && line[i] <= '9' && digits < 18)
                {
                    value = value * 10 + (line[i] - '0');
                    i++; digits++;
                }
                if (digits == 0) return false;
                if (i < line.Length && line[i] != '|') return false;
                if (neg) value = -value;
                pos = i + 1;
                return true;
            }
        }
    }
}

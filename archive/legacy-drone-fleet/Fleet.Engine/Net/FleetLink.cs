using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        // One drone's live status as carried by the status broadcast (spec §10.2).
        public struct DroneStatus
        {
            public string Name;
            public int State, Reason;       // MinerState / ReturnReason as ints
            public int Hole, Holes;         // 1-based hole being worked, total holes (0 = no job)
            public int Cargo;               // percent
            public int Battery, Hydrogen;   // percent, -1 = none fitted
            public int Lift100;             // lift margin x 100, -1 = not in gravity
            public int Flags;
            public int JobId, LeaseId, DoneMask; // slice 3 fleet job: 0 = none; DoneMask bit k = the lease's k-th hole
            public string Note;             // Hold note / SAFE reason / diagnostic, may be empty
        }

        // Remote-console protocol: IGC tags, the versioned status line, the SET command.
        public static class FleetLink
        {
            public const int Version = 2;
            public const int FlagConnected = 1, FlagHasJob = 2, FlagSafe = 4, FlagNotReady = 8, FlagRecording = 16;

            // Tags are built once at setup (allocation is fine there).
            public static string Tag(string channel, string kind) { return "FLEET/" + channel + "/" + kind; }
            public const string Status = "status", Command = "cmd", Cfg = "cfg", Ack = "ack";

            // Appends "2|name|state|reason|hole|holes|cargo|bat|h2|lift|flags|job|lease|mask|note". '|' and newlines in
            // the name or note are written as '/' and ' ' so they can never break the framing.
            public static StringBuilder Encode(StringBuilder sb, ref DroneStatus s)
            {
                SbFormat.AppendInt(sb, Version).Append('|');
                AppendClean(sb, s.Name).Append('|');
                SbFormat.AppendInt(sb, s.State).Append('|');
                SbFormat.AppendInt(sb, s.Reason).Append('|');
                SbFormat.AppendInt(sb, s.Hole).Append('|');
                SbFormat.AppendInt(sb, s.Holes).Append('|');
                SbFormat.AppendInt(sb, s.Cargo).Append('|');
                SbFormat.AppendInt(sb, s.Battery).Append('|');
                SbFormat.AppendInt(sb, s.Hydrogen).Append('|');
                SbFormat.AppendInt(sb, s.Lift100).Append('|');
                SbFormat.AppendInt(sb, s.Flags).Append('|');
                SbFormat.AppendInt(sb, s.JobId).Append('|');
                SbFormat.AppendInt(sb, s.LeaseId).Append('|');
                SbFormat.AppendInt(sb, s.DoneMask).Append('|');
                return AppendClean(sb, s.Note);
            }

            static StringBuilder AppendClean(StringBuilder sb, string text)
            {
                if (text == null) return sb;
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    sb.Append(c == '|' ? '/' : c == '\n' || c == '\r' ? ' ' : c);
                }
                return sb;
            }

            // Parses a status line into s. False (s untouched) on a wrong version or malformed line.
            // Version 1 lines (no job/lease/mask fields) are still accepted; those fields then read 0.
            // Allocates only when the name or note text actually changed since the previous line.
            public static bool Decode(string line, ref DroneStatus s)
            {
                if (line == null) return false;
                int pos = 0, v;
                if (!ReadInt(line, ref pos, out v) || (v != 1 && v != Version)) return false;
                int nameStart = pos, nameEnd = line.IndexOf('|', pos);
                if (nameEnd < 0) return false;
                pos = nameEnd + 1;
                int state, reason, hole, holes, cargo, bat, h2, lift, flags;
                if (!ReadInt(line, ref pos, out state) || !ReadInt(line, ref pos, out reason)
                    || !ReadInt(line, ref pos, out hole) || !ReadInt(line, ref pos, out holes)
                    || !ReadInt(line, ref pos, out cargo) || !ReadInt(line, ref pos, out bat)
                    || !ReadInt(line, ref pos, out h2) || !ReadInt(line, ref pos, out lift)
                    || !ReadInt(line, ref pos, out flags))
                    return false;
                int job = 0, lease = 0, mask = 0;
                if (v == 2 && (!ReadInt(line, ref pos, out job) || !ReadInt(line, ref pos, out lease)
                    || !ReadInt(line, ref pos, out mask)))
                    return false;
                if (pos > line.Length) return false;   // the note field must be present (may be empty)
                s.Name = Reuse(s.Name, line, nameStart, nameEnd - nameStart);
                s.Note = Reuse(s.Note, line, pos, line.Length - pos);
                s.State = state; s.Reason = reason; s.Hole = hole; s.Holes = holes; s.Cargo = cargo;
                s.Battery = bat; s.Hydrogen = h2; s.Lift100 = lift; s.Flags = flags;
                s.JobId = job; s.LeaseId = lease; s.DoneMask = mask;
                return true;
            }

            static string Reuse(string old, string line, int start, int len)
            {
                if (old != null && old.Length == len && string.CompareOrdinal(old, 0, line, start, len) == 0) return old;
                return len == 0 ? "" : line.Substring(start, len);
            }

            // Reads an optionally negative integer at pos, then requires '|' (consumed) or end of line.
            // On success pos is past the separator, or line.Length + 1 at end of line.
            static bool ReadInt(string line, ref int pos, out int value)
            {
                value = 0;
                if (pos >= line.Length) return false;
                bool neg = line[pos] == '-';
                int i = neg ? pos + 1 : pos, digits = 0;
                while (i < line.Length && line[i] >= '0' && line[i] <= '9' && digits < 9)
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

            // "SET <section> <key> <value>" (the "SET" word already stripped by the caller is also accepted).
            // Validates against SettingsSchema; on failure 'error' says why. Rare path: may allocate.
            public static bool TryParseSet(string text, out int field, out double value, out string error)
            {
                field = -1; value = 0; error = null;
                var parts = (text ?? "").Trim().Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                int k = parts.Length > 0 && string.Equals(parts[0], "SET", System.StringComparison.OrdinalIgnoreCase) ? 1 : 0;
                if (parts.Length - k != 3) { error = "usage: SET <section> <key> <value>"; return false; }
                field = SettingsSchema.Find(parts[k], parts[k + 1]);
                if (field < 0) { error = "unknown setting " + parts[k] + "." + parts[k + 1]; return false; }
                var f = SettingsSchema.Fields[field];
                if (!SettingsSchema.TryParse(field, parts[k + 2], out value)) { error = "bad value for " + f.Key; return false; }
                if (value < f.Min - 1e-9 || value > f.Max + 1e-9) { error = f.Key + " out of range"; return false; }
                return true;
            }

            // The SET command text the console sends for an edit, e.g. "SET Miner Width 6". Rare path.
            public static string SetCommand(int field, double value)
            {
                var f = SettingsSchema.Fields[field];
                return "SET " + f.Section + " " + f.Key + " " + SettingsSchema.ToIniText(field, value);
            }
        }
    }
}

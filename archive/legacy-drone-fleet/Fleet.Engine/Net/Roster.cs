using System.Collections.Generic;
using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        // One drone as the console knows it: last status, its config (from the cfg reply) and the
        // edits sent but not yet echoed back. It is the console menu's value source for that drone.
        public class RosterEntry : IMenuValues
        {
            public long Address;
            public DroneStatus Status;
            public double LastSeen;
            public readonly Settings Config = new Settings();
            public bool HasConfig;
            public readonly bool[] Pending = new bool[SettingsSchema.Fields.Length];

            public bool TryGet(int field, out double value)
            {
                value = HasConfig ? SettingsSchema.Get(Config, field) : 0;
                return HasConfig;
            }

            public bool IsPending(int field) { return field >= 0 && field < Pending.Length && Pending[field]; }

            // The drone's Custom Data arrived (GETCFG reply or SET echo): it is now the truth.
            public void ApplyConfig(string customData, List<string> scratchWarnings)
            {
                scratchWarnings.Clear();
                if (!ConfigLoader.Load(customData, Config, scratchWarnings)) return;
                HasConfig = true;
                for (int i = 0; i < Pending.Length; i++) Pending[i] = false;
            }

            // Optimistic local view of a sent edit, shown with the pending marker until the echo.
            public void MarkSent(int field, double value)
            {
                if (SettingsSchema.Set(Config, field, value)) Pending[field] = true;
            }

            public void Reset(long address)
            {
                Address = address; Status = new DroneStatus(); LastSeen = 0; HasConfig = false;
                for (int i = 0; i < Pending.Length; i++) Pending[i] = false;
            }
        }

        // The console's fleet list, filled from status broadcasts. Fixed capacity, no per-tick allocation.
        public class Roster : IMenuList
        {
            public const int Capacity = 16;
            public const double StaleAfter = 10, DropAfter = 60;
            readonly RosterEntry[] _all = new RosterEntry[Capacity];
            readonly RosterEntry[] _live = new RosterEntry[Capacity];
            int _count;
            public double Now;              // set by the console every tick; used for staleness display

            public Roster()
            {
                for (int i = 0; i < Capacity; i++) _all[i] = new RosterEntry();
            }

            public int Count { get { return _count; } }
            public RosterEntry this[int i] { get { return _live[i]; } }

            public int IndexOf(long address)
            {
                for (int i = 0; i < _count; i++) if (_live[i].Address == address) return i;
                return -1;
            }

            // Name lookup for "SEND <drone> ..." (case-insensitive).
            public RosterEntry Find(string name)
            {
                for (int i = 0; i < _count; i++)
                    if (string.Equals(_live[i].Status.Name, name, System.StringComparison.OrdinalIgnoreCase)) return _live[i];
                return null;
            }

            // A status line from 'address'. Returns the entry, or null when the line is invalid or the
            // roster is full of other drones.
            public RosterEntry Update(long address, string line, double now)
            {
                int i = IndexOf(address);
                RosterEntry e;
                if (i >= 0) e = _live[i];
                else
                {
                    if (_count >= Capacity) return null;
                    e = _all[FreeSlot()];
                    var probe = new DroneStatus();
                    if (!FleetLink.Decode(line, ref probe)) return null;
                    e.Reset(address);
                    _live[_count++] = e;
                }
                if (!FleetLink.Decode(line, ref e.Status)) return null;
                e.LastSeen = now;
                return e;
            }

            int FreeSlot()
            {
                for (int s = 0; s < Capacity; s++)
                {
                    bool used = false;
                    for (int i = 0; i < _count; i++) if (_live[i] == _all[s]) { used = true; break; }
                    if (!used) return s;
                }
                return 0;
            }

            // Drops drones not heard from for DropAfter seconds, keeping the order of the rest.
            public void Expire(double now)
            {
                int w = 0;
                for (int r = 0; r < _count; r++)
                    if (now - _live[r].LastSeen <= DropAfter) _live[w++] = _live[r];
                for (int r = w; r < _count; r++) _live[r] = null;
                _count = w;
            }

            public bool IsStale(RosterEntry e, double now) { return now - e.LastSeen > StaleAfter; }

            // "Miner-01   Drill       64%  bat 88%" (+ " stale"), allocation-free.
            public void AppendEntry(StringBuilder sb, int index)
            {
                var e = _live[index];
                var s = e.Status;
                Pad(sb, s.Name, 11);
                Pad(sb, s.State >= 0 && s.State < Names.State.Length ? Names.State[s.State] : "?", 12);
                SbFormat.AppendInt(sb, s.Cargo).Append('%');
                if (s.Battery >= 0) SbFormat.AppendInt(sb.Append("  bat "), s.Battery).Append('%');
                if ((s.Flags & FleetLink.FlagSafe) != 0) sb.Append("  SAFE");
                if (IsStale(e, Now)) sb.Append("  stale");
            }

            static void Pad(StringBuilder sb, string text, int width)
            {
                sb.Append(text);
                for (int k = text == null ? 0 : text.Length; k < width; k++) sb.Append(' ');
            }
        }
    }
}

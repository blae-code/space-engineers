using System;
using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        public enum FieldKind : byte { Int, Number, Toggle, Choice }

        // One editable Custom Data key. Ranges mirror ConfigLoader, so a value the menu can produce is
        // always one the loader accepts.
        public class SettingField
        {
            public readonly string Group, Section, Key, Label;
            public readonly FieldKind Kind;
            public readonly double Min, Max, Step;
            public readonly int Decimals;
            public readonly string[] Choices;

            public SettingField(string group, string section, string key, string label, FieldKind kind,
                double min, double max, double step, int decimals, string[] choices)
            {
                Group = group; Section = section; Key = key; Label = label; Kind = kind;
                Min = min; Max = max; Step = step; Decimals = decimals; Choices = choices;
            }
        }

        // Typed, index-addressed access to every editable setting: the menu (both PBs), the drone's
        // remote SET command and the console's view of a drone's config all go through this table.
        public static class SettingsSchema
        {
            public const string Job = "Job", Behaviour = "Behaviour", Flight = "Flight", Energy = "Energy";
            public static readonly string[] Groups = { Job, Behaviour, Flight, Energy };
            static readonly string[] OnOff = { "Off", "On" };

            public const int Width = 0, Height = 1, Depth = 2, StartDepth = 3, WorkSpeed = 4, RetractSpeed = 5,
                MaxLoad = 6, MinLiftMargin = 7, Loop = 8, OnDamage = 9, OnReload = 10, MaxSpeed = 11,
                ApproachSpeed = 12, DockSpeed = 13, SafeAltitude = 14, StuckSeconds = 15, MinBattery = 16,
                MinHydrogen = 17, MinUranium = 18;

            public static readonly SettingField[] Fields =
            {
                F(Job, "Miner", "Width", "Width", FieldKind.Int, 1, 50, 1, 0),
                F(Job, "Miner", "Height", "Height", FieldKind.Int, 1, 50, 1, 0),
                F(Job, "Miner", "Depth", "Depth m", FieldKind.Number, 1, 500, 5, 0),
                F(Job, "Miner", "StartDepth", "Start depth m", FieldKind.Number, 0, 500, 1, 0),
                F(Job, "Miner", "WorkSpeed", "Drill speed", FieldKind.Number, 0.1, 10, 0.1, 1),
                F(Job, "Miner", "RetractSpeed", "Retract speed", FieldKind.Number, 0.1, 20, 0.5, 1),
                F(Job, "Miner", "MaxLoad", "Max load %", FieldKind.Number, 10, 100, 5, 0),
                F(Job, "Miner", "MinLiftMargin", "Min lift", FieldKind.Number, 1.05, 5, 0.05, 2),
                new SettingField(Behaviour, "Miner", "Loop", "Loop job", FieldKind.Toggle, 0, 1, 1, 0, OnOff),
                new SettingField(Behaviour, "Miner", "OnDamage", "On damage", FieldKind.Choice, 0, 2, 1, 0,
                    new[] { "Home", "Job", "Stop" }),
                new SettingField(Behaviour, "Reload", "OnReload", "On reload", FieldKind.Choice, 0, 2, 1, 0,
                    new[] { "Resume", "ReturnHome", "Hold" }),
                F(Flight, "Flight", "MaxSpeed", "Max speed", FieldKind.Number, 1, 500, 5, 0),
                F(Flight, "Flight", "ApproachSpeed", "Approach spd", FieldKind.Number, 0.5, 50, 0.5, 1),
                F(Flight, "Flight", "DockSpeed", "Dock speed", FieldKind.Number, 0.1, 2, 0.1, 1),
                F(Flight, "Flight", "SafeAltitude", "Safe alt m", FieldKind.Number, 20, 5000, 10, 0),
                F(Flight, "Flight", "StuckSeconds", "Stuck after s", FieldKind.Number, 1, 60, 1, 0),
                F(Energy, "Energy", "MinBattery", "Min battery %", FieldKind.Number, 0, 90, 5, 0),
                F(Energy, "Energy", "MinHydrogen", "Min H2 %", FieldKind.Number, 0, 90, 5, 0),
                F(Energy, "Energy", "MinUranium", "Min uranium kg", FieldKind.Number, 0, 1000, 1, 0),
            };

            static SettingField F(string g, string sec, string key, string label, FieldKind k,
                double min, double max, double step, int dec)
            {
                return new SettingField(g, sec, key, label, k, min, max, step, dec, null);
            }

            public static double Get(Settings s, int i)
            {
                switch (i)
                {
                    case Width: return s.Width;
                    case Height: return s.Height;
                    case Depth: return s.Depth;
                    case StartDepth: return s.StartDepth;
                    case WorkSpeed: return s.WorkSpeed;
                    case RetractSpeed: return s.RetractSpeed;
                    case MaxLoad: return s.MaxLoad;
                    case MinLiftMargin: return s.MinLiftMargin;
                    case Loop: return s.Loop ? 1 : 0;
                    case OnDamage: return (int)s.OnDamage;
                    case OnReload: return (int)s.OnReload;
                    case MaxSpeed: return s.MaxSpeed;
                    case ApproachSpeed: return s.ApproachSpeed;
                    case DockSpeed: return s.DockSpeed;
                    case SafeAltitude: return s.SafeAltitude;
                    case StuckSeconds: return s.StuckSeconds;
                    case MinBattery: return s.MinBattery;
                    case MinHydrogen: return s.MinHydrogen;
                    case MinUranium: return s.MinUranium;
                }
                return 0;
            }

            // Writes v into s. False (s untouched) when i is unknown or v is outside the field's range.
            public static bool Set(Settings s, int i, double v)
            {
                if (i < 0 || i >= Fields.Length) return false;
                var f = Fields[i];
                if (double.IsNaN(v) || v < f.Min - 1e-9 || v > f.Max + 1e-9) return false;
                if (f.Kind != FieldKind.Number) v = Math.Round(v);
                switch (i)
                {
                    case Width: s.Width = (int)v; break;
                    case Height: s.Height = (int)v; break;
                    case Depth: s.Depth = v; break;
                    case StartDepth: s.StartDepth = v; break;
                    case WorkSpeed: s.WorkSpeed = v; break;
                    case RetractSpeed: s.RetractSpeed = v; break;
                    case MaxLoad: s.MaxLoad = v; break;
                    case MinLiftMargin: s.MinLiftMargin = v; break;
                    case Loop: s.Loop = v > 0.5; break;
                    case OnDamage: s.OnDamage = (DamagePolicy)(int)v; break;
                    case OnReload: s.OnReload = (ReloadPolicy)(int)v; break;
                    case MaxSpeed: s.MaxSpeed = v; break;
                    case ApproachSpeed: s.ApproachSpeed = v; break;
                    case DockSpeed: s.DockSpeed = v; break;
                    case SafeAltitude: s.SafeAltitude = v; break;
                    case StuckSeconds: s.StuckSeconds = v; break;
                    case MinBattery: s.MinBattery = v; break;
                    case MinHydrogen: s.MinHydrogen = v; break;
                    case MinUranium: s.MinUranium = v; break;
                }
                return true;
            }

            // Case-insensitive lookup by Custom Data section + key; -1 if not editable.
            public static int Find(string section, string key)
            {
                for (int i = 0; i < Fields.Length; i++)
                    if (string.Equals(Fields[i].Section, section, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(Fields[i].Key, key, StringComparison.OrdinalIgnoreCase))
                        return i;
                return -1;
            }

            // Parses the text form used in Custom Data and in remote SET commands: a number, true/false/
            // on/off for toggles, or a choice name (case-insensitive) or its index for choices.
            public static bool TryParse(int i, string text, out double v)
            {
                v = 0;
                if (i < 0 || i >= Fields.Length || text == null) return false;
                var f = Fields[i];
                text = text.Trim();
                if (f.Kind == FieldKind.Toggle)
                {
                    if (Eq(text, "true") || Eq(text, "on") || text == "1") { v = 1; return true; }
                    if (Eq(text, "false") || Eq(text, "off") || text == "0") { v = 0; return true; }
                    return false;
                }
                if (f.Kind == FieldKind.Choice)
                    for (int c = 0; c < f.Choices.Length; c++)
                        if (Eq(text, f.Choices[c])) { v = c; return true; }
                return Gps.TryParseNumber(text, out v);
            }

            // The Custom Data text for a value: choice name, true/false, or a plain number.
            public static string ToIniText(int i, double v)
            {
                var f = Fields[i];
                if (f.Kind == FieldKind.Toggle) return v > 0.5 ? "true" : "false";
                if (f.Kind == FieldKind.Choice) return f.Choices[(int)v];
                var sb = new StringBuilder();
                return SbFormat.AppendFixed(sb, v, f.Kind == FieldKind.Int ? 0 : Math.Max(f.Decimals, 2)).ToString();
            }

            // Display form (allocation-free): On/Off, choice name, or fixed decimals.
            public static StringBuilder AppendValue(StringBuilder sb, int i, double v)
            {
                var f = Fields[i];
                if (f.Choices != null)
                {
                    int c = (int)Math.Round(v);
                    return sb.Append(c >= 0 && c < f.Choices.Length ? f.Choices[c] : "?");
                }
                return SbFormat.AppendFixed(sb, v, f.Decimals);
            }

            static bool Eq(string a, string b) { return string.Equals(a, b, StringComparison.OrdinalIgnoreCase); }
        }
    }
}

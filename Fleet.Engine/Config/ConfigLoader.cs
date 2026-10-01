using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program
    {
        public static class ConfigLoader
        {
            // Returns false if text is not valid INI (s untouched, one warning "Custom Data: not valid INI").
            // Empty or whitespace text is valid (all defaults, no warnings).
            public static bool Load(string text, Settings s, List<string> warnings)
            {
                var ini = new MyIni();
                MyIniParseResult r;
                if (!string.IsNullOrWhiteSpace(text) && !ini.TryParse(text, out r))
                {
                    warnings.Add("Custom Data: not valid INI");
                    return false;
                }

                // Load Fleet section
                var nameValue = ini.Get("Fleet", "Name").ToString("");
                if (string.IsNullOrEmpty(nameValue))
                {
                    // If Name is explicitly set to empty, warn but keep default
                    if (ini.ContainsKey("Fleet", "Name"))
                    {
                        warnings.Add("Fleet.Name: must not be empty");
                    }
                }
                else
                {
                    s.Name = nameValue;
                }
                
                var tagValue = ini.Get("Fleet", "Tag").ToString("");
                if (string.IsNullOrEmpty(tagValue))
                {
                    // If Tag is explicitly set to empty, warn but keep default
                    if (ini.ContainsKey("Fleet", "Tag"))
                    {
                        warnings.Add("Fleet.Tag: must not be empty");
                    }
                }
                else
                {
                    s.Tag = tagValue;
                }

                // Load Miner section
                ReadInt(ini, "Miner", "Width", ref s.Width, 1, 50, warnings);
                ReadInt(ini, "Miner", "Height", ref s.Height, 1, 50, warnings);
                ReadDouble(ini, "Miner", "Depth", ref s.Depth, 1, 500, warnings);
                ReadDouble(ini, "Miner", "StartDepth", ref s.StartDepth, 0, 500, warnings);
                ReadDouble(ini, "Miner", "WorkSpeed", ref s.WorkSpeed, 0.1, 10, warnings);
                ReadDouble(ini, "Miner", "RetractSpeed", ref s.RetractSpeed, 0.1, 20, warnings);
                ReadDouble(ini, "Miner", "MaxLoad", ref s.MaxLoad, 10, 100, warnings);
                ReadDouble(ini, "Miner", "MinLiftMargin", ref s.MinLiftMargin, 1.05, 5, warnings);
                
                var ejectValue = ini.Get("Miner", "Eject").ToString("");
                if (string.IsNullOrEmpty(ejectValue))
                {
                    // If Eject is empty, clear the list but don't set it to null
                    s.Eject.Clear();
                }
                else
                {
                    s.Eject.Clear();
                    foreach (var item in ejectValue.Split(','))
                    {
                        var trimmed = item.Trim();
                        if (!string.IsNullOrEmpty(trimmed))
                        {
                            s.Eject.Add(trimmed);
                        }
                    }
                }
                
                ReadBool(ini, "Miner", "Loop", ref s.Loop, warnings);
                int choice = (int)s.OnDamage;
                ReadChoice(ini, "Miner", "OnDamage", SettingsSchema.OnDamage, ref choice, warnings);
                s.OnDamage = (DamagePolicy)choice;

                // Load Flight section
                ReadDouble(ini, "Flight", "MaxSpeed", ref s.MaxSpeed, 1, 500, warnings);
                ReadDouble(ini, "Flight", "ApproachSpeed", ref s.ApproachSpeed, 0.5, 50, warnings);
                ReadDouble(ini, "Flight", "DockSpeed", ref s.DockSpeed, 0.1, 2, warnings);
                ReadDouble(ini, "Flight", "SafeAltitude", ref s.SafeAltitude, 20, 5000, warnings);
                ReadDouble(ini, "Flight", "StuckSeconds", ref s.StuckSeconds, 1, 60, warnings);

                // Load Energy section
                ReadDouble(ini, "Energy", "MinBattery", ref s.MinBattery, 0, 90, warnings);
                ReadDouble(ini, "Energy", "MinHydrogen", ref s.MinHydrogen, 0, 90, warnings);
                ReadDouble(ini, "Energy", "MinUranium", ref s.MinUranium, 0, 1000, warnings);

                // Load Reload section
                choice = (int)s.OnReload;
                ReadChoice(ini, "Reload", "OnReload", SettingsSchema.OnReload, ref choice, warnings);
                s.OnReload = (ReloadPolicy)choice;

                return true;
            }

            // Returns INI text containing every key, values taken from s; unknown sections/keys in text are kept.
            public static string Write(string text, Settings s)
            {
                var ini = new MyIni();
                MyIniParseResult r;
                if (!string.IsNullOrWhiteSpace(text) && !ini.TryParse(text, out r))
                {
                    // If parsing fails, start with empty ini
                    ini = new MyIni();
                }

                // Fleet section
                ini.Set("Fleet", "Name", s.Name);
                ini.Set("Fleet", "Tag", s.Tag);

                // Miner section
                ini.Set("Miner", "Width", s.Width);
                ini.Set("Miner", "Height", s.Height);
                ini.Set("Miner", "Depth", s.Depth);
                ini.Set("Miner", "StartDepth", s.StartDepth);
                ini.Set("Miner", "WorkSpeed", s.WorkSpeed);
                ini.Set("Miner", "RetractSpeed", s.RetractSpeed);
                ini.Set("Miner", "MaxLoad", s.MaxLoad);
                ini.Set("Miner", "MinLiftMargin", s.MinLiftMargin);
                ini.Set("Miner", "Eject", string.Join(",", s.Eject));
                ini.Set("Miner", "Loop", s.Loop);
                ini.Set("Miner", "OnDamage", SettingsSchema.Fields[SettingsSchema.OnDamage].Choices[(int)s.OnDamage]);

                // Flight section
                ini.Set("Flight", "MaxSpeed", s.MaxSpeed);
                ini.Set("Flight", "ApproachSpeed", s.ApproachSpeed);
                ini.Set("Flight", "DockSpeed", s.DockSpeed);
                ini.Set("Flight", "SafeAltitude", s.SafeAltitude);
                ini.Set("Flight", "StuckSeconds", s.StuckSeconds);

                // Energy section
                ini.Set("Energy", "MinBattery", s.MinBattery);
                ini.Set("Energy", "MinHydrogen", s.MinHydrogen);
                ini.Set("Energy", "MinUranium", s.MinUranium);

                // Reload section
                ini.Set("Reload", "OnReload", SettingsSchema.Fields[SettingsSchema.OnReload].Choices[(int)s.OnReload]);

                return ini.ToString();
            }

            private static void ReadInt(MyIni ini, string section, string key, ref int field, int min, int max, List<string> warnings)
            {
                if (ini.ContainsKey(section, key))
                {
                    var value = ini.Get(section, key);
                    int parsed;
                    if (value.TryGetInt32(out parsed))
                    {
                        if (parsed >= min && parsed <= max)
                        {
                            field = parsed;
                        }
                        else
                        {
                            warnings.Add($"{section}.{key}: out of range {min}..{max}");
                        }
                    }
                    else
                    {
                        warnings.Add($"{section}.{key}: not an integer");
                    }
                }
            }

            private static void ReadDouble(MyIni ini, string section, string key, ref double field, double min, double max, List<string> warnings)
            {
                if (ini.ContainsKey(section, key))
                {
                    var value = ini.Get(section, key);
                    double parsed;
                    if (value.TryGetDouble(out parsed))
                    {
                        if (parsed >= min && parsed <= max)
                        {
                            field = parsed;
                        }
                        else
                        {
                            warnings.Add($"{section}.{key}: out of range {min}..{max}");
                        }
                    }
                    else
                    {
                        warnings.Add($"{section}.{key}: not a number");
                    }
                }
            }

            private static void ReadBool(MyIni ini, string section, string key, ref bool field, List<string> warnings)
            {
                if (ini.ContainsKey(section, key))
                {
                    var value = ini.Get(section, key);
                    bool parsed;
                    if (value.TryGetBoolean(out parsed))
                    {
                        field = parsed;
                    }
                    else
                    {
                        warnings.Add($"{section}.{key}: not true/false");
                    }
                }
            }

            private static void ReadString(MyIni ini, string section, string key, ref string field, List<string> warnings)
            {
                if (ini.ContainsKey(section, key))
                {
                    var value = ini.Get(section, key);
                    field = value.ToString("");
                }
            }

            // Enum settings are matched against SettingsSchema's literal choice names, never Enum.ToString():
            // minify=full renames enum members, so their identifiers do not survive deployment.
            private static void ReadChoice(MyIni ini, string section, string key, int field, ref int value, List<string> warnings)
            {
                if (!ini.ContainsKey(section, key)) return;
                var raw = ini.Get(section, key).ToString("");
                var choices = SettingsSchema.Fields[field].Choices;
                for (int i = 0; i < choices.Length; i++)
                    if (string.Equals(choices[i], raw.Trim(), StringComparison.OrdinalIgnoreCase)) { value = i; return; }
                warnings.Add($"{section}.{key}: unknown value '{raw}'");
            }
        }
    }
}
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program
    {
        public static class StorageStore
        {
            public const string VersionKey = "v";

            // Clears scratch, then for each subsystem: Set(sub.Name, "v", sub.StorageVersion) and sub.Save(scratch).
            public static string Save(List<ISubsystem> subs, MyIni scratch)
            {
                scratch.Clear();
                for (int i = 0; i < subs.Count; i++)
                {
                    ISubsystem sub = subs[i];
                    scratch.Set(sub.Name, VersionKey, sub.StorageVersion);
                    sub.Save(scratch);
                }
                return scratch.ToString();
            }

            // Parses storage into scratch. Unparseable -> dropped.Add("*"), no subsystem is loaded.
            // For each subsystem whose section exists: v = Get(Name,"v").ToInt32(0); if !sub.Load(scratch, v) -> dropped.Add(sub.Name).
            // A subsystem with no section is left untouched (fresh start) and is NOT reported.
            public static void Load(string storage, List<ISubsystem> subs, MyIni scratch, List<string> dropped)
            {
                scratch.Clear();
                if (!scratch.TryParse(storage ?? ""))
                {
                    dropped.Add("*");
                    return;
                }
                for (int i = 0; i < subs.Count; i++)
                {
                    ISubsystem sub = subs[i];
                    if (!scratch.ContainsSection(sub.Name))
                        continue;
                    int v = scratch.Get(sub.Name, VersionKey).ToInt32(0);
                    if (!sub.Load(scratch, v))
                        dropped.Add(sub.Name);
                }
            }
        }
    }
}

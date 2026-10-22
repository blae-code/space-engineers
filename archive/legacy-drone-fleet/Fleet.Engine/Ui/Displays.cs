using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;

namespace IngameScript
{
    public partial class Program
    {
        public class Displays
        {
            readonly List<IMyTextSurface> _surfaces = new List<IMyTextSurface>();

            public Displays() { }

            // blocks: terminal blocks on the drone's grid. Collects panels whose name contains tag and provider
            // surfaces named with "[<tagCore>:n]" (tag "[FM]" -> tagCore "FM"). Sets text mode, Monospace, 0.8.
            public void Refresh(List<IMyTerminalBlock> blocks, string tag)
            {
                _surfaces.Clear();
                if (string.IsNullOrEmpty(tag)) return;
                for (int i = 0; i < blocks.Count; i++)
                {
                    IMyTerminalBlock b = blocks[i];
                    string name = b.CustomName;

                    // a text panel is also an IMyTextSurfaceProvider, so test the panel first
                    IMyTextPanel panel = b as IMyTextPanel;
                    if (panel != null)
                    {
                        if (name.Contains(tag)) Add(panel);
                        continue;
                    }

                    IMyTextSurfaceProvider provider = b as IMyTextSurfaceProvider;
                    if (provider == null) continue;
                    int index = SurfaceIndex(name, tag);
                    if (index < 0 || index >= provider.SurfaceCount) continue;
                    Add(provider.GetSurface(index));
                }
            }

            void Add(IMyTextSurface s)
            {
                if (s == null) return;
                s.ContentType = ContentType.TEXT_AND_IMAGE;
                s.Font = "Monospace";
                s.FontSize = 0.8f;
                _surfaces.Add(s);
            }

            public int Count { get { return _surfaces.Count; } }

            // same text to every surface
            public void Write(StringBuilder text)
            {
                for (int i = 0; i < _surfaces.Count; i++) _surfaces[i].WriteText(text);
            }

            // index n from a name containing "[<tagCore>:n]"; -1 if absent/invalid.
            public static int SurfaceIndex(string name, string tag)
            {
                if (name == null || tag == null || tag.Length < 3) return -1;
                string needle = "[" + tag.Substring(1, tag.Length - 2) + ":";
                int at = name.IndexOf(needle, StringComparison.Ordinal);
                if (at < 0) return -1;
                int p = at + needle.Length;
                int value = 0, digits = 0;
                while (p < name.Length && name[p] >= '0' && name[p] <= '9' && digits < 6)
                {
                    value = value * 10 + (name[p] - '0');
                    digits++;
                    p++;
                }
                if (digits == 0 || p >= name.Length || name[p] != ']') return -1;
                return value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public static class PathCodec
        {
            // Writes "n" = count and "x<i>", "y<i>", "z<i>" = coordinates rounded to 2 decimals. Save/load only.
            public static void Save(MyIni ini, string section, List<Vector3D> points)
            {
                ini.Set(section, "n", points.Count);
                for (int i = 0; i < points.Count; i++)
                {
                    ini.Set(section, "x" + i, Math.Round(points[i].X, 2));
                    ini.Set(section, "y" + i, Math.Round(points[i].Y, 2));
                    ini.Set(section, "z" + i, Math.Round(points[i].Z, 2));
                }
            }

            public static bool Load(MyIni ini, string section, List<Vector3D> points)
            {
                points.Clear();
                if (!ini.ContainsSection(section) || !ini.ContainsKey(section, "n")) return false;
                int n;
                if (!ini.Get(section, "n").TryGetInt32(out n) || n < 0) return false;
                for (int i = 0; i < n; i++)
                {
                    double x, y, z;
                    if (!ini.Get(section, "x" + i).TryGetDouble(out x)
                        || !ini.Get(section, "y" + i).TryGetDouble(out y)
                        || !ini.Get(section, "z" + i).TryGetDouble(out z))
                    {
                        points.Clear();
                        return false;
                    }
                    points.Add(new Vector3D(x, y, z));
                }
                return true;
            }
        }
    }
}

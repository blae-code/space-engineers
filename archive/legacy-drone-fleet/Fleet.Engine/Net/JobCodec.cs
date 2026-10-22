using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        // A mining job shared by the fleet (slice 3 spec §3): the hole grid and job route in world space, and
        // the lead drone's dock path local to its connector.
        public class FleetJob
        {
            public int JobId;
            public Vector3D Origin, Forward, Up;
            public int Width, Height;
            public double Spacing;
            public bool Gps;
            public readonly List<Vector3D> Route = new List<Vector3D>();
            public readonly List<Vector3D> DockPath = new List<Vector3D>();
            public int HoleCount { get { return Width * Height; } }

            public void CopyFrom(FleetJob o)
            {
                JobId = o.JobId; Origin = o.Origin; Forward = o.Forward; Up = o.Up;
                Width = o.Width; Height = o.Height; Spacing = o.Spacing; Gps = o.Gps;
                Route.Clear(); Route.AddRange(o.Route);
                DockPath.Clear(); DockPath.AddRange(o.DockPath);
            }
        }

        // "J|2|jobId|ox|oy|oz|fx|fy|fz|ux|uy|uz|W|H|spacing|gps|nRoute|x|y|z…|nDock|x|y|z…"
        // Positions and the spacing are integer millimetres, directions integer millionths. Rare path.
        public static class JobCodec
        {
            public const int MaxPoints = 500, MaxSide = 50;

            public static string Encode(FleetJob j)
            {
                var sb = new StringBuilder("J|2|");
                Int(sb, j.JobId);
                Vec(sb, j.Origin, 1000); Vec(sb, j.Forward, 1e6); Vec(sb, j.Up, 1e6);
                Int(sb, j.Width); Int(sb, j.Height);
                Int(sb, (long)Math.Round(j.Spacing * 1000));
                Int(sb, j.Gps ? 1 : 0);
                Points(sb, j.Route);
                Points(sb, j.DockPath);
                sb.Length--;   // trailing '|'
                return sb.ToString();
            }

            static void Int(StringBuilder sb, long v) { SbFormat.AppendInt(sb, v).Append('|'); }

            static void Vec(StringBuilder sb, Vector3D v, double scale)
            {
                Int(sb, (long)Math.Round(v.X * scale)); Int(sb, (long)Math.Round(v.Y * scale)); Int(sb, (long)Math.Round(v.Z * scale));
            }

            static void Points(StringBuilder sb, List<Vector3D> pts)
            {
                Int(sb, pts.Count);
                for (int i = 0; i < pts.Count; i++) Vec(sb, pts[i], 1000);
            }

            // Fills j and returns true only for a complete, valid line; otherwise j is untouched.
            public static bool Decode(string line, FleetJob j)
            {
                if (line == null) return false;
                var f = line.Split('|');
                if (f.Length < 18 || f[0] != "J" || f[1] != "2") return false;
                var n = new long[f.Length - 2];
                for (int i = 2; i < f.Length; i++) if (!long.TryParse(f[i], out n[i - 2])) return false;
                // n: 0 jobId, 1-3 origin, 4-6 fwd, 7-9 up, 10 W, 11 H, 12 spacing, 13 gps, 14 nRoute
                if (n[10] < 1 || n[10] > MaxSide || n[11] < 1 || n[11] > MaxSide || n[12] <= 0) return false;
                if (n[13] != 0 && n[13] != 1) return false;
                var fwd = Vec(n, 4, 1e-6); var up = Vec(n, 7, 1e-6);
                if (fwd.LengthSquared() < 0.25 || up.LengthSquared() < 0.25) return false;
                long nRoute = n[14];
                if (nRoute < 0 || nRoute > MaxPoints) return false;
                int dockAt = 15 + (int)nRoute * 3;
                if (dockAt >= n.Length) return false;
                long nDock = n[dockAt];
                if (nDock < 0 || nDock > MaxPoints || n.Length != dockAt + 1 + nDock * 3) return false;

                j.JobId = (int)n[0];
                j.Origin = Vec(n, 1, 1e-3);
                j.Forward = Vector3D.Normalize(fwd);
                j.Up = Vector3D.Normalize(up);
                j.Width = (int)n[10]; j.Height = (int)n[11];
                j.Spacing = n[12] / 1000.0;
                j.Gps = n[13] == 1;
                j.Route.Clear();
                for (int i = 0; i < nRoute; i++) j.Route.Add(Vec(n, 15 + i * 3, 1e-3));
                j.DockPath.Clear();
                for (int i = 0; i < nDock; i++) j.DockPath.Add(Vec(n, dockAt + 1 + i * 3, 1e-3));
                return true;
            }

            static Vector3D Vec(long[] n, int at, double scale)
            {
                return new Vector3D(n[at] * scale, n[at + 1] * scale, n[at + 2] * scale);
            }
        }
    }
}

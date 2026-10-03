using System;
using System.Text;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// A mining site: its frame, shaft spacing, and what is known about every shaft.
        /// Drones keep one for the site they work; the carrier keeps a library of them and
        /// merges every ShaftReport, so several drones can share a site without overlap.
        /// Shafts are numbered along a square spiral out from the site origin.
        /// </summary>
        public class SiteMap
        {
            public const int Cap = 121, Radius = 5, Width = 2 * Radius + 1; // 11 x 11 shafts at most

            // Shaft status codes (also sent over IGC as ShaftReport.A.Y).
            public const int Untouched = 0, Claimed = 1, Partial = 2, Done = 3, Empty = 4, Barren = 5, Blocked = 6;
            public static readonly string[] StatusNames = { "untouched", "in progress", "partial", "done", "no rock", "barren", "blocked" };

            // Spiral coordinates per shaft, and the reverse lookup grid. Built once.
            public static readonly int[] X = new int[Cap], Y = new int[Cap];
            static readonly int[] _grid = new int[Width * Width];
            static bool _built;

            public bool Defined;
            public Vector3D Pos, Fwd, Up;
            public double SpacingX = 5, SpacingY = 5;
            public int Limit = 25;

            public readonly int[] Status = new int[Cap];
            public readonly double[] Value = new double[Cap];
            public readonly long[] Claimant = new long[Cap];
            public readonly double[] ClaimAt = new double[Cap];
            /// <summary>Distance from the shaft entry to the rock face along the site forward, -1 = unknown (camera survey).</summary>
            public readonly double[] Face = new double[Cap];

            public SiteMap()
            {
                if (_built) return;
                _built = true;
                for (int i = 0; i < _grid.Length; i++) _grid[i] = -1;
                int x = 0, y = 0, dx = 1, dy = 0, segLen = 1, segDone = 0, turns = 0;
                for (int n = 0; n < Cap; n++)
                {
                    X[n] = x;
                    Y[n] = y;
                    _grid[(x + Radius) * Width + y + Radius] = n;
                    x += dx;
                    y += dy;
                    if (++segDone < segLen) continue;
                    segDone = 0;
                    int t = dx;
                    dx = -dy;
                    dy = t;
                    if (++turns % 2 == 0) segLen++;
                }
            }

            /// <summary>Shaft number at spiral coordinates, or -1 outside the map.</summary>
            public static int At(int x, int y)
            {
                if (x < -Radius || x > Radius || y < -Radius || y > Radius) return -1;
                return _grid[(x + Radius) * Width + y + Radius];
            }

            public void Define(Vector3D pos, Vector3D fwd, Vector3D up, double sx, double sy, int limit)
            {
                Defined = true;
                Pos = pos;
                Fwd = fwd;
                Up = up;
                SpacingX = sx;
                SpacingY = sy;
                Limit = limit <= 0 || limit > Cap ? Cap : limit;
                for (int i = 0; i < Cap; i++)
                {
                    Status[i] = Untouched;
                    Value[i] = 0;
                    Claimant[i] = 0;
                    Face[i] = -1;
                }
            }

            public Vector3D Entry(int shaft)
            {
                Vector3D right = Vector3D.Cross(Fwd, Up);
                return Pos + right * (X[shaft] * SpacingX) + Up * (Y[shaft] * SpacingY);
            }

            /// <summary>
            /// Merges a report. Finished shafts are never downgraded by a late claim, and an
            /// Untouched report only carries survey data (the face distance).
            /// </summary>
            public void Apply(int shaft, int status, double value, double face, long source, double now)
            {
                if (shaft < 0 || shaft >= Cap) return;
                if (face >= 0) Face[shaft] = face;
                if (status == Untouched) return;
                if (status == Claimed)
                {
                    if (Status[shaft] >= Done) return;
                    Status[shaft] = Claimed;
                    Claimant[shaft] = source;
                    ClaimAt[shaft] = now;
                    return;
                }
                Status[shaft] = status;
                Value[shaft] = value;
                Claimant[shaft] = 0;
            }

            /// <summary>Mean value of shafts that produced any ore (0 when none have).</summary>
            public double MeanValue()
            {
                double sum = 0;
                int n = 0;
                for (int i = 0; i < Limit; i++)
                    if (Value[i] > 0 && Status[i] != Claimed) { sum += Value[i]; n++; }
                return n > 0 ? sum / n : 0;
            }

            public bool IsRich(int shaft, double mean)
            {
                return mean > 0 && Value[shaft] >= 2 * mean;
            }

            public int Count(int status)
            {
                int n = 0;
                for (int i = 0; i < Limit; i++)
                    if (Status[i] == status) n++;
                return n;
            }

            /// <summary>
            /// Picks the next shaft for <paramref name="me"/>: resumable work first, then the
            /// untouched shaft whose neighbours yielded the most ore, steering away from barren
            /// rock and the asteroid's edge. Ties fall back to spiral order. -1 = site exhausted.
            /// </summary>
            public int Pick(long me, double now, double claimTimeout)
            {
                double mean = MeanValue();
                int best = -1;
                double bestScore = double.MinValue;
                for (int i = 0; i < Limit; i++)
                {
                    int s = Status[i];
                    bool mine = s == Claimed && Claimant[i] == me;
                    bool stale = s == Claimed && now - ClaimAt[i] > claimTimeout;
                    if (s != Untouched && s != Partial && !mine && !stale) continue;

                    double score = -i * 0.01;
                    if (s == Partial || mine) score += 100;
                    for (int dx = -1; dx <= 1; dx++)
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            int n = At(X[i] + dx, Y[i] + dy);
                            if (n < 0 || n >= Limit || n == i) continue;
                            switch (Status[n])
                            {
                                case Partial:
                                case Done: if (mean > 0) score += Math.Min(Value[n] / mean, 3); break;
                                case Barren: score -= 0.5; break;
                                case Empty: score -= 1; break;
                                case Blocked: score -= 0.25; break;
                            }
                        }
                    if (score > bestScore)
                    {
                        bestScore = score;
                        best = i;
                    }
                }
                return best;
            }

            // ---------------- Persistence (Save / constructor only) ----------------

            public void Write(StringBuilder sb)
            {
                var ic = System.Globalization.CultureInfo.InvariantCulture;
                sb.Append(Defined ? '1' : '0');
                Vec(sb, Pos);
                Vec(sb, Fwd);
                Vec(sb, Up);
                sb.Append('|').Append(SpacingX.ToString("R", ic))
                  .Append('|').Append(SpacingY.ToString("R", ic))
                  .Append('|').Append(Limit).Append('|');
                for (int i = 0; i < Cap; i++)
                    sb.Append((char)('0' + (Status[i] == Claimed ? Partial : Status[i])));
                sb.Append('|');
                for (int i = 0; i < Cap; i++)
                {
                    if (i > 0) sb.Append(',');
                    if (Value[i] > 0) sb.Append(Math.Round(Value[i]).ToString(ic));
                }
                sb.Append('|');
                for (int i = 0; i < Cap; i++)
                {
                    if (i > 0) sb.Append(',');
                    if (Face[i] >= 0) sb.Append(Math.Round(Face[i], 1).ToString(ic));
                }
            }

            public void Read(string s)
            {
                if (string.IsNullOrEmpty(s)) return;
                string[] f = s.Split('|');
                if (f.Length < 15) return;
                Defined = f[0] == "1";
                Pos = new Vector3D(Dbl(f[1]), Dbl(f[2]), Dbl(f[3]));
                Fwd = new Vector3D(Dbl(f[4]), Dbl(f[5]), Dbl(f[6]));
                Up = new Vector3D(Dbl(f[7]), Dbl(f[8]), Dbl(f[9]));
                SpacingX = Dbl(f[10]);
                SpacingY = Dbl(f[11]);
                Limit = Math.Max(1, Math.Min(Cap, (int)Dbl(f[12])));
                string st = f[13];
                string[] v = f[14].Split(',');
                string[] face = f.Length > 15 ? f[15].Split(',') : v;
                for (int i = 0; i < Cap; i++)
                {
                    Status[i] = i < st.Length ? Math.Max(0, Math.Min(Blocked, st[i] - '0')) : Untouched;
                    Value[i] = i < v.Length ? Dbl(v[i]) : 0;
                    Claimant[i] = 0;
                    Face[i] = f.Length > 15 && i < face.Length && face[i].Length > 0 ? Dbl(face[i]) : -1;
                }
            }

            static void Vec(StringBuilder sb, Vector3D v)
            {
                var ic = System.Globalization.CultureInfo.InvariantCulture;
                sb.Append('|').Append(v.X.ToString("R", ic))
                  .Append('|').Append(v.Y.ToString("R", ic))
                  .Append('|').Append(v.Z.ToString("R", ic));
            }

            public static double Dbl(string s)
            {
                double v;
                return double.TryParse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out v) ? v : 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public static class HoleGrid
        {
            public const double DrillRadius = 1.4;   // small-grid drill cutting radius, m
            public const double Overlap = 0.9;       // holes overlap slightly so no ridges are left

            // Odd n: -(n-1)/2 .. (n-1)/2.  Even n: -n/2 .. n/2-1 (centre biased low).
            public static void Bounds(int n, out int min, out int max)
            {
                min = -(n / 2);
                max = n % 2 == 1 ? (n - 1) / 2 : n / 2 - 1;
            }

            // Clears 'order' and fills it with width*height distinct cells in centre-out spiral order.
            public static void Spiral(int width, int height, List<Vector2I> order)
            {
                order.Clear();
                int total = width * height;
                if (total <= 0) return;

                int minX, maxX, minY, maxY;
                Bounds(width, out minX, out maxX);
                Bounds(height, out minY, out maxY);

                int x = 0, y = 0;
                if (x >= minX && x <= maxX && y >= minY && y <= maxY) order.Add(new Vector2I(x, y));

                int side = Math.Max(width, height) + 2;
                int guard = side * side * 4;
                int steps = 0;
                int dir = 0;
                int len = 1;
                int lenUses = 0;
                while (order.Count < total && steps < guard)
                {
                    for (int i = 0; i < len && order.Count < total && steps < guard; i++)
                    {
                        switch (dir)
                        {
                            case 0: x++; break;
                            case 1: y++; break;
                            case 2: x--; break;
                            default: y--; break;
                        }
                        steps++;
                        if (x >= minX && x <= maxX && y >= minY && y <= maxY) order.Add(new Vector2I(x, y));
                    }
                    dir = (dir + 1) % 4;
                    lenUses++;
                    if (lenUses == 2) { lenUses = 0; len++; }
                }
            }

            // origin + right * cell.X * spacing + up * cell.Y * spacing
            public static Vector3D HoleEntrance(Vector3D origin, Vector3D right, Vector3D up, Vector2I cell, double spacing)
            {
                return origin + right * (cell.X * spacing) + up * (cell.Y * spacing);
            }

            // Drill positions in ship-local coordinates (drills face ship-forward, so X/Y span the cutting face):
            // max(spanX, spanY) + 2 * DrillRadius * Overlap, where span = max - min of that coordinate.
            // Empty list -> 2 * DrillRadius * Overlap.
            public static double SpacingFromDrills(List<Vector3D> drillPositionsShipLocal)
            {
                double extra = 2 * DrillRadius * Overlap;
                if (drillPositionsShipLocal == null || drillPositionsShipLocal.Count == 0) return extra;
                double minX = double.MaxValue, maxX = double.MinValue, minY = double.MaxValue, maxY = double.MinValue;
                for (int i = 0; i < drillPositionsShipLocal.Count; i++)
                {
                    Vector3D p = drillPositionsShipLocal[i];
                    if (p.X < minX) minX = p.X;
                    if (p.X > maxX) maxX = p.X;
                    if (p.Y < minY) minY = p.Y;
                    if (p.Y > maxY) maxY = p.Y;
                }
                return Math.Max(maxX - minX, maxY - minY) + extra;
            }
        }
    }
}

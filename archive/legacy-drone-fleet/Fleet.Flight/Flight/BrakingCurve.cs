using System;

namespace IngameScript
{
    public partial class Program
    {
        public static class BrakingCurve
        {
            // min(cap, sqrt(2 * aBrake * dist) * margin). Returns 0 if dist <= 0, aBrake <= 0 or cap <= 0.
            public static double SafeSpeed(double dist, double aBrake, double cap, double margin)
            {
                if (dist <= 0 || aBrake <= 0 || cap <= 0)
                {
                    return 0;
                }

                // Calculate the distance-limited speed: sqrt(2 * aBrake * dist) * margin
                double limitedSpeed = Math.Sqrt(2 * aBrake * dist) * margin;

                // Return min(cap, limitedSpeed)
                return Math.Min(cap, limitedSpeed);
            }

            // speed^2 / (2 * aBrake). Returns double.PositiveInfinity if aBrake <= 0 (cannot stop). Speed sign ignored.
            public static double StopDistance(double speed, double aBrake)
            {
                if (aBrake <= 0)
                {
                    return double.PositiveInfinity;
                }

                // Use Math.Abs(speed) to ignore the sign of speed
                double absSpeed = Math.Abs(speed);
                return (absSpeed * absSpeed) / (2 * aBrake);
            }
        }
    }
}
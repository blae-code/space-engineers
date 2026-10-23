using System;

namespace IngameScript
{
    public static class TmsDemandMath
    {
        public static bool TryCalculateDeficitKg(int targetStockKg, double currentStockKg,
            out int deficitKg)
        {
            deficitKg = 0;
            if (targetStockKg < 0 || currentStockKg < 0.0 ||
                double.IsNaN(currentStockKg) || double.IsInfinity(currentStockKg))
                return false;

            double deficit = targetStockKg - currentStockKg;
            if (deficit <= 0.0) return true;

            deficitKg = (int)Math.Ceiling(deficit);
            return true;
        }

        public static bool TryGetPriority(string oreType, out int priority)
        {
            priority = 0;
            if (string.Equals(oreType, "URANIUM", StringComparison.Ordinal) ||
                string.Equals(oreType, "PLATINUM", StringComparison.Ordinal) ||
                string.Equals(oreType, "GOLD", StringComparison.Ordinal) ||
                string.Equals(oreType, "MAGNESIUM", StringComparison.Ordinal))
            {
                priority = 5;
                return true;
            }

            if (string.Equals(oreType, "IRON", StringComparison.Ordinal))
            {
                priority = 3;
                return true;
            }

            return false;
        }
    }
}

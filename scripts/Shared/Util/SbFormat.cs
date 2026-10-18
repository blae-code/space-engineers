using System;
using System.Text;

namespace IngameScript
{
    public static class SbFormat
    {
        public static StringBuilder AppendInt(StringBuilder sb, long value)
        {
            if (value < 0)
            {
                sb.Append('-');
                value = -value;
            }
            if (value >= 10)
                AppendInt(sb, value / 10);
            sb.Append((char)('0' + (value % 10)));
            return sb;
        }

        public static StringBuilder AppendFix2(StringBuilder sb, double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                sb.Append("--");
                return sb;
            }
            if (value < 0)
            {
                sb.Append('-');
                value = -value;
            }
            long whole = (long)value;
            long frac = (long)((value - whole) * 100.0 + 0.5);
            if (frac >= 100)
            {
                whole++;
                frac -= 100;
            }
            AppendInt(sb, whole);
            sb.Append('.');
            if (frac < 10) sb.Append('0');
            AppendInt(sb, frac);
            return sb;
        }

        public static StringBuilder AppendPct(StringBuilder sb, double ratio)
        {
            return AppendFix2(sb, ratio * 100.0).Append('%');
        }
    }
}

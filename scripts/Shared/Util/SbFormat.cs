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

        public static StringBuilder AppendDouble(StringBuilder sb, double value, int decimals = 1)
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
            AppendInt(sb, whole);
            if (decimals <= 0) return sb;

            sb.Append('.');
            double remainder = value - whole;
            for (int i = 0; i < decimals; i++)
            {
                remainder *= 10.0;
                int digit = (int)remainder;
                sb.Append((char)('0' + (digit % 10)));
                remainder -= digit;
            }
            return sb;
        }

        public static StringBuilder AppendAngleDeg(StringBuilder sb, float rad)
        {
            double deg = rad * (180.0 / Math.PI);
            return AppendDouble(sb, deg, 1).Append('°');
        }

        public static StringBuilder AppendPadded(StringBuilder sb, string text, int totalWidth, bool leftAlign = true)
        {
            if (text == null) text = "";
            int pad = totalWidth - text.Length;
            if (leftAlign)
            {
                sb.Append(text);
                for (int i = 0; i < pad; i++) sb.Append(' ');
            }
            else
            {
                for (int i = 0; i < pad; i++) sb.Append(' ');
                sb.Append(text);
            }
            return sb;
        }
    }
}

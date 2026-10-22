using System;
using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        public static class SbFormat
        {
            private static readonly char[] Buf = new char[20];

            public static StringBuilder AppendInt(StringBuilder sb, long value)
            {
                if (value == 0)
                {
                    sb.Append('0');
                    return sb;
                }

                bool negative = value < 0;
                if (negative)
                    value = -value;

                int pos = 20;
                while (value > 0)
                {
                    Buf[--pos] = (char)('0' + (value % 10));
                    value /= 10;
                }

                if (negative)
                    Buf[--pos] = '-';

                sb.Append(Buf, pos, 20 - pos);
                return sb;
            }

            public static StringBuilder AppendFixed(StringBuilder sb, double value, int decimals)
            {
                // Handle special case of zero
                if (value == 0.0)
                {
                    sb.Append('0');
                    if (decimals > 0)
                    {
                        sb.Append('.');
                        for (int i = 0; i < decimals; i++)
                            sb.Append('0');
                    }
                    return sb;
                }

                // Round the value
                double scaled = value * Math.Pow(10, decimals);
                long rounded = (long)Math.Round(scaled, MidpointRounding.AwayFromZero);

                bool negative = rounded < 0;
                if (negative)
                    rounded = -rounded;

                // Handle sign
                if (negative)
                    sb.Append('-');

                // Convert integer part
                AppendInt(sb, rounded / (long)Math.Pow(10, decimals));

                // Add decimal point and fractional part
                if (decimals > 0)
                {
                    sb.Append('.');
                    long fractional = rounded % (long)Math.Pow(10, decimals);
                    int pos = 20;
                    for (int i = 0; i < decimals; i++)
                    {
                        Buf[--pos] = (char)('0' + (fractional % 10));
                        fractional /= 10;
                    }
                    sb.Append(Buf, pos, 20 - pos);
                }

                return sb;
            }

            public static StringBuilder AppendPad2(StringBuilder sb, int value)
            {
                if (value < 10)
                    sb.Append('0');
                AppendInt(sb, value);
                return sb;
            }

            public static StringBuilder AppendPercent(StringBuilder sb, double ratio)
            {
                long percent = (long)Math.Round(ratio * 100, MidpointRounding.AwayFromZero);
                AppendInt(sb, percent);
                sb.Append('%');
                return sb;
            }

            public static StringBuilder AppendTime(StringBuilder sb, double seconds)
            {
                long total = (long)Math.Floor(seconds);
                long minutes = total / 60;
                long secondsPart = total % 60;

                AppendPad2(sb, (int)minutes);
                sb.Append(':');
                AppendPad2(sb, (int)secondsPart);
                return sb;
            }
        }
    }
}
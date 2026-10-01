using System;

namespace IngameScript
{
    public partial class Program
    {
        public static class CommandParser
        {
            public static Cmd Parse(string argument, out string rest)
            {
                rest = "";
                if (string.IsNullOrEmpty(argument))
                    return Cmd.None;

                string trimmed = argument.Trim();
                if (trimmed.Length == 0)
                    return Cmd.None;

                string tail;
                string first = SplitWord(trimmed, out tail);

                if (string.Equals(first, "RECORD", StringComparison.OrdinalIgnoreCase))
                {
                    string afterSecond;
                    string second = SplitWord(tail, out afterSecond);
                    if (string.Equals(second, "DOCK", StringComparison.OrdinalIgnoreCase))
                    {
                        rest = afterSecond;
                        return Cmd.RecordDock;
                    }
                    if (string.Equals(second, "JOB", StringComparison.OrdinalIgnoreCase))
                    {
                        rest = afterSecond;
                        return Cmd.RecordJob;
                    }
                    rest = trimmed;
                    return Cmd.Unknown;
                }

                Cmd cmd;
                switch (first.ToUpperInvariant())
                {
                    case "START": cmd = Cmd.Start; break;
                    case "STOP": cmd = Cmd.Stop; break;
                    case "HOME": cmd = Cmd.Home; break;
                    case "CONT": cmd = Cmd.Cont; break;
                    case "NEXT": cmd = Cmd.Next; break;
                    case "PREV": cmd = Cmd.Prev; break;
                    case "FULL": cmd = Cmd.Full; break;
                    case "STOPREC": cmd = Cmd.StopRec; break;
                    case "SETJOB": cmd = Cmd.SetJob; break;
                    case "GOTO": cmd = Cmd.Goto; break;
                    case "REBOOT": cmd = Cmd.Reboot; break;
                    case "RESET": cmd = Cmd.Reset; break;
                    case "GYROTEST": cmd = Cmd.GyroTest; break;
                    case "UP": cmd = Cmd.Up; break;
                    case "DOWN": cmd = Cmd.Down; break;
                    case "APPLY": cmd = Cmd.Apply; break;
                    case "BACK": cmd = Cmd.Back; break;
                    default:
                        rest = trimmed;
                        return Cmd.Unknown;
                }

                rest = tail;
                return cmd;
            }

            // Splits off the first whitespace-delimited word; tail is the remainder, trimmed ("" if none).
            private static string SplitWord(string text, out string tail)
            {
                int end = 0;
                while (end < text.Length && !char.IsWhiteSpace(text[end]))
                    end++;
                tail = end < text.Length ? text.Substring(end).Trim() : "";
                return text.Substring(0, end);
            }
        }
    }
}

using System;
using System.Globalization;

namespace IngameScript
{
    public partial class Program
    {
        public static class CommandParser
        {
            public static Cmd Parse(string argument, out string rest)
            {
                // Handle null, empty or whitespace input
                if (string.IsNullOrWhiteSpace(argument))
                {
                    rest = "";
                    return Cmd.None;
                }

                // Trim the argument
                string trimmed = argument.Trim();
                
                // Special case for GOTO - we want to extract everything after "goto"
                if (trimmed.StartsWith("goto", StringComparison.OrdinalIgnoreCase))
                {
                    rest = trimmed.Substring(4).Trim();
                    return Cmd.Goto;
                }
                
                // Split into words using any whitespace as delimiter
                string[] words = trimmed.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (words.Length == 0)
                {
                    rest = "";
                    return Cmd.None;
                }
                
                // Handle single word commands
                if (words.Length == 1)
                {
                    string firstWord = words[0].ToLowerInvariant();
                    
                    switch (firstWord)
                    {
                        case "start": rest = ""; return Cmd.Start;
                        case "stop": rest = ""; return Cmd.Stop;
                        case "home": rest = ""; return Cmd.Home;
                        case "cont": rest = ""; return Cmd.Cont;
                        case "next": rest = ""; return Cmd.Next;
                        case "prev": rest = ""; return Cmd.Prev;
                        case "full": rest = ""; return Cmd.Full;
                        case "stoprec": rest = ""; return Cmd.StopRec;
                        case "setjob": rest = ""; return Cmd.SetJob;
                        case "reboot": rest = ""; return Cmd.Reboot;
                        case "reset": rest = ""; return Cmd.Reset;
                        case "gyrotest": rest = ""; return Cmd.GyroTest;
                        case "up": rest = ""; return Cmd.Up;
                        case "down": rest = ""; return Cmd.Down;
                        case "apply": rest = ""; return Cmd.Apply;
                        case "back": rest = ""; return Cmd.Back;
                        default: 
                            rest = trimmed;
                            return Cmd.Unknown;
                    }
                }
                // Handle two-word commands
                else if (words.Length >= 2)
                {
                    string firstWord = words[0].ToLowerInvariant();
                    string secondWord = words[1].ToLowerInvariant();
                    
                    if (firstWord == "record")
                    {
                        if (secondWord == "dock")
                        {
                            // For "record dock", rest is everything after the two words
                            // Find exact position of the command in the original string
                            int pos = trimmed.IndexOf("record", StringComparison.OrdinalIgnoreCase);
                            if (pos >= 0)
                            {
                                // Move to end of "record dock" 
                                int endPos = pos + 11; // "record dock".Length = 11
                                if (endPos < trimmed.Length)
                                {
                                    rest = trimmed.Substring(endPos).Trim();
                                }
                                else
                                {
                                    rest = "";
                                }
                            }
                            else
                            {
                                rest = "";
                            }
                            return Cmd.RecordDock;
                        }
                        else if (secondWord == "job")
                        {
                            // For "record job", rest is everything after the two words
                            // Find exact position of the command in the original string
                            int pos = trimmed.IndexOf("record", StringComparison.OrdinalIgnoreCase);
                            if (pos >= 0)
                            {
                                // Move to end of "record job" 
                                int endPos = pos + 10; // "record job".Length = 10
                                if (endPos < trimmed.Length)
                                {
                                    rest = trimmed.Substring(endPos).Trim();
                                }
                                else
                                {
                                    rest = "";
                                }
                            }
                            else
                            {
                                rest = "";
                            }
                            return Cmd.RecordJob;
                        }
                        else
                        {
                            // RECORD without DOCK or JOB is unknown
                            rest = trimmed;
                            return Cmd.Unknown;
                        }
                    }
                    else
                    {
                        // Any other two-word command is unknown
                        rest = trimmed;
                        return Cmd.Unknown;
                    }
                }
                
                // Default case
                rest = trimmed;
                return Cmd.Unknown;
            }
        }
    }
}
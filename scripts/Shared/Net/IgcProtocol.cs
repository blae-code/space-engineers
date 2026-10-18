using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public static class IgcProtocol
    {
        public const char Sep = ';';

        public static void Split(string payload, List<string> output)
        {
            output.Clear();
            if (payload == null) return;
            int start = 0;
            for (int i = 0; i < payload.Length; i++)
            {
                if (payload[i] == Sep)
                {
                    output.Add(payload.Substring(start, i - start));
                    start = i + 1;
                }
            }
            if (start <= payload.Length)
                output.Add(payload.Substring(start));
        }

        public static StringBuilder Build(StringBuilder sb, string tag, string arg1)
        {
            return sb.Clear().Append(tag).Append(Sep).Append(arg1);
        }

        public static StringBuilder Build(StringBuilder sb, string tag, string arg1, string arg2)
        {
            return sb.Clear().Append(tag).Append(Sep).Append(arg1).Append(Sep).Append(arg2);
        }

        public static StringBuilder Build(StringBuilder sb, string tag, string arg1, string arg2, string arg3)
        {
            return sb.Clear().Append(tag).Append(Sep).Append(arg1).Append(Sep).Append(arg2).Append(Sep).Append(arg3);
        }
    }
}

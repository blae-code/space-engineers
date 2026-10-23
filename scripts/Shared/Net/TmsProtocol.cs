using System;
using System.Collections.Generic;
using System.Text;

namespace IngameScript
{
    public static class TmsProtocol
    {
        public const string HandoverTag = "TMS.C2.HANDOVER";
        public const string LogisticsJitTag = "TMS.LOGISTICS.JIT";
        public const string MuleDispatchTag = "TMS.MULE.DISPATCH";
        public const string CatDispatchTag = "TMS.CAT.DISPATCH";
        public const string BaseDemandTag = "TMS.BASE.DEMAND";

        public static bool TryReadHandover(string payload, out string targetNode, out string timestamp)
        {
            targetNode = "";
            timestamp = "";
            if (string.IsNullOrEmpty(payload)) return false;

            int first = payload.IndexOf(';');
            if (first <= 0) return false;
            int second = payload.IndexOf(';', first + 1);
            if (second <= first + 1 || second == payload.Length - 1) return false;
            if (payload.IndexOf(';', second + 1) >= 0) return false;
            if (!string.Equals(payload.Substring(0, first), "HANDOVER", StringComparison.Ordinal)) return false;

            targetNode = payload.Substring(first + 1, second - first - 1);
            timestamp = payload.Substring(second + 1);
            return true;
        }

        public static bool TryWriteHandover(StringBuilder buffer, string targetNode, string timestamp)
        {
            if (buffer == null || string.IsNullOrEmpty(targetNode) || string.IsNullOrEmpty(timestamp)) return false;
            if (targetNode.IndexOf(';') >= 0 || timestamp.IndexOf(';') >= 0) return false;

            buffer.Clear().Append("HANDOVER;").Append(targetNode).Append(';').Append(timestamp);
            return true;
        }

        public static bool TryReadJitRequest(string payload, List<string> fields, out string requesterId,
            out string tier, out string etaSeconds, out string oreType, out string estimatedMassKg)
        {
            requesterId = "";
            tier = "";
            etaSeconds = "";
            oreType = "";
            estimatedMassKg = "";
            if (!TryReadFields(payload, fields, 6, "JIT")) return false;

            requesterId = fields[1];
            tier = fields[2];
            etaSeconds = fields[3];
            oreType = fields[4];
            estimatedMassKg = fields[5];
            return true;
        }

        public static bool TryReadMuleDispatch(string payload, List<string> fields, out string muleId,
            out string pickupPad, out string destinationGps, out string payloadType, out string requestId)
        {
            muleId = "";
            pickupPad = "";
            destinationGps = "";
            payloadType = "";
            requestId = "";
            if (!TryReadFields(payload, fields, 6, "DISPATCH")) return false;

            muleId = fields[1];
            pickupPad = fields[2];
            destinationGps = fields[3];
            payloadType = fields[4];
            requestId = fields[5];
            return true;
        }

        public static bool TryWriteMuleDispatch(StringBuilder buffer, string muleId, string pickupPad,
            string destinationGps, string payloadType, string requestId)
        {
            if (buffer == null || !IsValidField(muleId) || !IsValidField(pickupPad) ||
                !IsValidField(destinationGps) || !IsValidField(payloadType) || !IsValidField(requestId))
                return false;

            buffer.Clear().Append("DISPATCH;").Append(muleId).Append(';').Append(pickupPad).Append(';')
                .Append(destinationGps).Append(';').Append(payloadType).Append(';').Append(requestId);
            return true;
        }

        public static bool TryReadCatDispatch(string payload, List<string> fields, out string catId,
            out string destinationId, out string oreType, out string requestId)
        {
            catId = "";
            destinationId = "";
            oreType = "";
            requestId = "";
            if (!TryReadFields(payload, fields, 5, "DISPATCH")) return false;

            catId = fields[1];
            destinationId = fields[2];
            oreType = fields[3];
            requestId = fields[4];
            return true;
        }

        public static bool TryWriteCatDispatch(StringBuilder buffer, string catId, string destinationId,
            string oreType, string requestId)
        {
            if (buffer == null || !IsValidField(catId) || !IsValidField(destinationId) ||
                !IsValidField(oreType) || !IsValidField(requestId))
                return false;

            buffer.Clear().Append("DISPATCH;").Append(catId).Append(';').Append(destinationId).Append(';')
                .Append(oreType).Append(';').Append(requestId);
            return true;
        }

        public static bool TryReadBaseDemand(string payload, List<string> fields, out string oreType,
            out string priority, out string deficitKg, out string targetStockKg)
        {
            oreType = "";
            priority = "";
            deficitKg = "";
            targetStockKg = "";
            if (!TryReadFields(payload, fields, 5, "DEMAND")) return false;

            oreType = fields[1];
            priority = fields[2];
            deficitKg = fields[3];
            targetStockKg = fields[4];
            return true;
        }

        public static bool TryWriteBaseDemand(StringBuilder buffer, string oreType, string priority,
            string deficitKg, string targetStockKg)
        {
            if (buffer == null || !IsValidField(oreType) || !IsValidField(priority) ||
                !IsValidField(deficitKg) || !IsValidField(targetStockKg))
                return false;

            buffer.Clear().Append("DEMAND;").Append(oreType).Append(';').Append(priority).Append(';')
                .Append(deficitKg).Append(';').Append(targetStockKg);
            return true;
        }

        static bool TryReadFields(string payload, List<string> fields, int count, string verb)
        {
            if (fields == null) return false;
            fields.Clear();
            if (string.IsNullOrEmpty(payload)) return false;

            int start = 0;
            for (int i = 0; i < payload.Length; i++)
            {
                if (payload[i] == ';')
                {
                    fields.Add(payload.Substring(start, i - start));
                    start = i + 1;
                }
            }
            fields.Add(payload.Substring(start));

            if (fields.Count != count || fields[0] != verb)
            {
                fields.Clear();
                return false;
            }
            for (int i = 1; i < fields.Count; i++)
            {
                if (string.IsNullOrEmpty(fields[i]))
                {
                    fields.Clear();
                    return false;
                }
            }
            return true;
        }

        static bool IsValidField(string value)
        {
            return !string.IsNullOrEmpty(value) && value.IndexOf(';') < 0;
        }
    }
}

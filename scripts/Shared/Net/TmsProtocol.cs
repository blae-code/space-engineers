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

        public static bool TryReadJitRequest(string payload, List<string> fields, out string requesterId,
            out string tier, out string etaSeconds, out string oreType, out string estimatedMassKg,
            out string sourceGps)
        {
            requesterId = "";
            tier = "";
            etaSeconds = "";
            oreType = "";
            estimatedMassKg = "";
            sourceGps = "";
            if (!TryReadFields(payload, fields, 7, "JIT")) return false;
            if (!IsValidGps(fields[6]))
            {
                fields.Clear();
                return false;
            }

            requesterId = fields[1];
            tier = fields[2];
            etaSeconds = fields[3];
            oreType = fields[4];
            estimatedMassKg = fields[5];
            sourceGps = fields[6];
            return true;
        }

        public static bool TryWriteJitRequest(StringBuilder buffer, string requesterId, string tier,
            string etaSeconds, string oreType, string estimatedMassKg, string sourceGps)
        {
            if (buffer == null || !IsValidField(requesterId) || !IsValidField(tier) ||
                !IsValidField(etaSeconds) || !IsValidField(oreType) || !IsValidField(estimatedMassKg) ||
                !IsValidGps(sourceGps))
                return false;

            buffer.Clear().Append("JIT;").Append(requesterId).Append(';').Append(tier).Append(';')
                .Append(etaSeconds).Append(';').Append(oreType).Append(';').Append(estimatedMassKg)
                .Append(';').Append(sourceGps);
            return true;
        }

        public static bool TryWriteJitRequest(StringBuilder buffer, string requesterId, string tier,
            int etaSeconds, string oreType, int estimatedMassKg, string sourceGps)
        {
            if (buffer == null || !IsValidField(requesterId) || !IsValidField(tier) ||
                etaSeconds < 0 || !IsValidField(oreType) || estimatedMassKg < 0 ||
                !IsValidGps(sourceGps))
                return false;

            buffer.Clear().Append("JIT;").Append(requesterId).Append(';').Append(tier).Append(';');
            AppendNonNegativeInt(buffer, etaSeconds);
            buffer.Append(';').Append(oreType).Append(';');
            AppendNonNegativeInt(buffer, estimatedMassKg);
            buffer.Append(';').Append(sourceGps);
            return true;
        }

        public static bool TryReadMuleRouteDispatch(string payload, List<string> fields, out string muleId,
            out string pickupGps, out string deliveryGps, out string payloadType, out string requestId)
        {
            if (!TryReadMuleDispatch(payload, fields, out muleId, out pickupGps, out deliveryGps,
                out payloadType, out requestId))
                return false;
            if (IsValidGps(pickupGps) && IsValidGps(deliveryGps)) return true;

            fields.Clear();
            muleId = "";
            pickupGps = "";
            deliveryGps = "";
            payloadType = "";
            requestId = "";
            return false;
        }

        public static bool TryWriteMuleRouteDispatch(StringBuilder buffer, string muleId,
            string pickupGps, string deliveryGps, string payloadType, string requestId)
        {
            if (!IsValidGps(pickupGps) || !IsValidGps(deliveryGps)) return false;
            return TryWriteMuleDispatch(buffer, muleId, pickupGps, deliveryGps, payloadType, requestId);
        }

        public static bool TryReadMuleDispatch(string payload, List<string> fields, out string muleId,
            out string pickupGps, out string destinationGps, out string payloadType, out string requestId)
        {
            muleId = "";
            pickupGps = "";
            destinationGps = "";
            payloadType = "";
            requestId = "";
            if (!TryReadFields(payload, fields, 6, "DISPATCH")) return false;

            muleId = fields[1];
            pickupGps = fields[2];
            destinationGps = fields[3];
            payloadType = fields[4];
            requestId = fields[5];
            return true;
        }

        public static bool TryWriteMuleDispatch(StringBuilder buffer, string muleId, string pickupGps,
            string destinationGps, string payloadType, string requestId)
        {
            if (buffer == null || !IsValidField(muleId) || !IsValidField(pickupGps) ||
                !IsValidField(destinationGps) || !IsValidField(payloadType) || !IsValidField(requestId))
                return false;

            buffer.Clear().Append("DISPATCH;").Append(muleId).Append(';').Append(pickupGps).Append(';')
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

        public static bool TryReadCatDispatch(string payload, List<string> fields, out string catId,
            out string destinationId, out string destinationGps, out string oreType, out string requestId)
        {
            catId = "";
            destinationId = "";
            destinationGps = "";
            oreType = "";
            requestId = "";
            if (!TryReadFields(payload, fields, 6, "DISPATCH")) return false;

            catId = fields[1];
            destinationId = fields[2];
            destinationGps = fields[3];
            oreType = fields[4];
            requestId = fields[5];
            return true;
        }

        public static bool TryReadCatRouteDispatch(string payload, List<string> fields, out string catId,
            out string destinationId, out string destinationGps, out string oreType, out string requestId)
        {
            if (!TryReadCatDispatch(payload, fields, out catId, out destinationId, out destinationGps,
                out oreType, out requestId))
                return false;
            if (IsValidGps(destinationGps)) return true;

            fields.Clear();
            catId = "";
            destinationId = "";
            destinationGps = "";
            oreType = "";
            requestId = "";
            return false;
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

        public static bool TryWriteCatDispatch(StringBuilder buffer, string catId, string destinationId,
            string destinationGps, string oreType, string requestId)
        {
            if (buffer == null || !IsValidField(catId) || !IsValidField(destinationId) ||
                !IsValidGps(destinationGps) || !IsValidField(oreType) || !IsValidField(requestId))
                return false;

            buffer.Clear().Append("DISPATCH;").Append(catId).Append(';').Append(destinationId).Append(';')
                .Append(destinationGps).Append(';').Append(oreType).Append(';').Append(requestId);
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

        static bool IsValidGps(string value)
        {
            if (string.IsNullOrEmpty(value) || !value.StartsWith("GPS:", StringComparison.Ordinal) ||
                value.IndexOf(';') >= 0 || value[value.Length - 1] != ':')
                return false;

            int nameEnd = value.IndexOf(':', 4);
            if (nameEnd <= 4) return false;
            int xEnd = value.IndexOf(':', nameEnd + 1);
            int yEnd = xEnd < 0 ? -1 : value.IndexOf(':', xEnd + 1);
            int zEnd = yEnd < 0 ? -1 : value.IndexOf(':', yEnd + 1);
            if (xEnd <= nameEnd + 1 || yEnd <= xEnd + 1 || zEnd != value.Length - 1 ||
                zEnd <= yEnd + 1)
                return false;

            return IsGpsCoordinate(value, nameEnd + 1, xEnd - nameEnd - 1) &&
                IsGpsCoordinate(value, xEnd + 1, yEnd - xEnd - 1) &&
                IsGpsCoordinate(value, yEnd + 1, zEnd - yEnd - 1);
        }

        static bool IsGpsCoordinate(string value, int start, int length)
        {
            int end = start + length;
            int digits = 0;
            bool decimalSeen = false;
            for (int i = start; i < end; i++)
            {
                char c = value[i];
                if (c >= '0' && c <= '9')
                {
                    digits++;
                    continue;
                }
                if (c == '-' && i == start) continue;
                if (c == '.' && !decimalSeen)
                {
                    decimalSeen = true;
                    continue;
                }
                return false;
            }
            return digits > 0;
        }

        static void AppendNonNegativeInt(StringBuilder buffer, int value)
        {
            if (value >= 10)
                AppendNonNegativeInt(buffer, value / 10);
            buffer.Append((char)('0' + value % 10));
        }
    }
}

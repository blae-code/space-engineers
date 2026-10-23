using System;
using System.Collections.Generic;

namespace IngameScript
{
    public sealed class TmsDemandLedger
    {
        public const int OreCount = 5;
        public const int StaleAfterUpdate10Ticks = 30;

        public struct DemandRecord
        {
            public string OreType;
            public int Priority;
            public int DeficitKg;
            public int TargetStockKg;
            public bool Fresh;
        }

        readonly DemandRecord[] _records = new DemandRecord[OreCount]
        {
            new DemandRecord { OreType = "URANIUM" },
            new DemandRecord { OreType = "PLATINUM" },
            new DemandRecord { OreType = "GOLD" },
            new DemandRecord { OreType = "MAGNESIUM" },
            new DemandRecord { OreType = "IRON" }
        };
        readonly int[] _ageUpdate10Ticks = new int[OreCount];

        public int FreshCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < OreCount; i++)
                    if (_records[i].Fresh) count++;
                return count;
            }
        }

        public int DeficitCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < OreCount; i++)
                    if (_records[i].Fresh && _records[i].DeficitKg > 0) count++;
                return count;
            }
        }

        public bool TryUpdatePayload(string payload, List<string> fields)
        {
            string oreType;
            string priorityText;
            string deficitText;
            string targetText;
            if (!TmsProtocol.TryReadBaseDemand(payload, fields, out oreType, out priorityText,
                out deficitText, out targetText))
                return false;

            int priority;
            int deficitKg;
            int targetStockKg;
            if (!int.TryParse(priorityText, out priority) ||
                !int.TryParse(deficitText, out deficitKg) ||
                !int.TryParse(targetText, out targetStockKg))
                return false;

            return TryUpdate(oreType, priority, deficitKg, targetStockKg);
        }

        public bool TryGet(string oreType, out DemandRecord demand)
        {
            int index = FindOre(oreType);
            if (index >= 0 && _records[index].Fresh)
            {
                demand = _records[index];
                return true;
            }

            demand = new DemandRecord();
            return false;
        }

        public bool TryGetAt(int index, out DemandRecord demand)
        {
            if (index < 0 || index >= OreCount)
            {
                demand = new DemandRecord();
                return false;
            }

            demand = _records[index];
            return true;
        }

        public void Tick()
        {
            for (int i = 0; i < OreCount; i++)
            {
                if (!_records[i].Fresh) continue;

                _ageUpdate10Ticks[i]++;
                if (_ageUpdate10Ticks[i] >= StaleAfterUpdate10Ticks)
                {
                    _records[i].Fresh = false;
                    _ageUpdate10Ticks[i] = StaleAfterUpdate10Ticks;
                }
            }
        }

        bool TryUpdate(string oreType, int priority, int deficitKg, int targetStockKg)
        {
            int index = FindOre(oreType);
            if (index < 0 || priority < 1 || priority > 5 ||
                deficitKg < 0 || targetStockKg < 0 || deficitKg > targetStockKg)
                return false;

            _records[index].Priority = priority;
            _records[index].DeficitKg = deficitKg;
            _records[index].TargetStockKg = targetStockKg;
            _records[index].Fresh = true;
            _ageUpdate10Ticks[index] = 0;
            return true;
        }

        int FindOre(string oreType)
        {
            for (int i = 0; i < OreCount; i++)
                if (string.Equals(_records[i].OreType, oreType, StringComparison.Ordinal))
                    return i;
            return -1;
        }
    }
}

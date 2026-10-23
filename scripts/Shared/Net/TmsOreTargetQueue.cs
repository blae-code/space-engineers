using System;

namespace IngameScript
{
    public sealed class TmsOreTargetQueue
    {
        public struct TargetRecord
        {
            public string OreType;
            public int Priority;
        }

        TargetRecord _active = new TargetRecord { OreType = "NONE", Priority = 0 };
        TargetRecord _pending = new TargetRecord { OreType = "NONE", Priority = 0 };
        bool _hasPending;

        public TargetRecord Active
        {
            get { return _active; }
        }

        public TargetRecord Pending
        {
            get { return _pending; }
        }

        public bool HasPending
        {
            get { return _hasPending; }
        }

        public bool TryQueue(string oreType, int priority, bool excavationActive)
        {
            if (!IsValidTarget(oreType, priority)) return false;

            if (IsSameTarget(_active, oreType, priority))
            {
                _hasPending = false;
                return true;
            }

            if (!excavationActive)
            {
                SetRecord(ref _active, oreType, priority);
                _hasPending = false;
                return true;
            }

            SetRecord(ref _pending, oreType, priority);
            _hasPending = true;
            return true;
        }

        public bool ApplyPending()
        {
            if (!_hasPending) return false;

            _active = _pending;
            _hasPending = false;
            return true;
        }

        static bool IsValidTarget(string oreType, int priority)
        {
            if (string.Equals(oreType, "NONE", StringComparison.Ordinal))
                return priority == 0;

            int configuredPriority;
            return TmsDemandMath.TryGetPriority(oreType, out configuredPriority) &&
                priority >= 1 && priority <= 5;
        }

        static bool IsSameTarget(TargetRecord target, string oreType, int priority)
        {
            return target.Priority == priority &&
                string.Equals(target.OreType, oreType, StringComparison.Ordinal);
        }

        static void SetRecord(ref TargetRecord target, string oreType, int priority)
        {
            target.OreType = oreType;
            target.Priority = priority;
        }
    }
}

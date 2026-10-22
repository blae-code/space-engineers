using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program
    {
        public class DamageScanner
        {
            public const int BlocksPerStep = 20;
            public const double IntegrityThreshold = 0.98;  // damaged below 98 % of baseline integrity

            List<IMyTerminalBlock> _blocks = new List<IMyTerminalBlock>();
            int _index;
            int _count;
            double _integrity;
            int _baseCount;
            double _baseIntegrity;
            bool _hasBaseline;
            bool _damaged;

            public DamageScanner() { }

            // keeps the list reference; restarts any pass in progress, the baseline is kept
            public void Refresh(List<IMyTerminalBlock> blocks)
            {
                _blocks = blocks;
                _index = 0;
                _count = 0;
                _integrity = 0;
            }

            public bool HasBaseline { get { return _hasBaseline; } }
            public bool Damaged { get { return _damaged; } }

            // full synchronous pass; called on SETJOB (rare)
            public void TakeBaseline()
            {
                _index = 0;
                _count = 0;
                _integrity = 0;
                for (int i = 0; i < _blocks.Count; i++) Measure(_blocks[i]);
                _baseCount = _count;
                _baseIntegrity = _integrity;
                _hasBaseline = true;
                _damaged = false;
                _index = 0;
                _count = 0;
                _integrity = 0;
            }

            // Scans the next BlocksPerStep blocks; after a full pass, updates Damaged. Returns Damaged.
            public bool Step()
            {
                if (!_hasBaseline) return false;
                int end = _index + BlocksPerStep;
                if (end > _blocks.Count) end = _blocks.Count;
                for (; _index < end; _index++) Measure(_blocks[_index]);
                if (_index >= _blocks.Count)
                {
                    _damaged = IsDamaged(_baseCount, _baseIntegrity, _count, _integrity);
                    _index = 0;
                    _count = 0;
                    _integrity = 0;
                }
                return _damaged;
            }

            void Measure(IMyTerminalBlock b)
            {
                if (b == null || b.Closed || !b.IsFunctional) return;
                IMySlimBlock slim = b.CubeGrid.GetCubeBlock(b.Position);
                if (slim == null) return;
                _count++;
                _integrity += slim.BuildIntegrity;
            }

            public static bool IsDamaged(int baseCount, double baseIntegrity, int count, double integrity)
            {
                return count < baseCount || integrity < baseIntegrity * IntegrityThreshold;
            }
        }
    }
}

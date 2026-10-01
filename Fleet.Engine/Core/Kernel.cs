using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program
    {
        public class Kernel
        {
            private readonly List<ISubsystem> _subs;
            private readonly Profiler _profiler;
            private readonly Func<int> _instructionCount;
            private bool _isSafe;
            private string _safeReason;
            private Action _onSafe;
            private ISubsystem _currentSub;

            public Kernel(List<ISubsystem> subs, Profiler profiler, Func<int> instructionCount)
            {
                _subs = subs;
                _profiler = profiler;
                _instructionCount = instructionCount;
                _isSafe = false;
                _safeReason = "";
            }

            public bool IsSafe { get { return _isSafe; } }
            public string SafeReason { get { return _safeReason; } }
            public Action OnSafe { get { return _onSafe; } set { _onSafe = value; } }

            public void Tick(UpdateType src, double lastRunMs)
            {
                if (IsSafe)
                    return;

                try
                {
                    // Process Update1 if requested
                    if ((src & UpdateType.Update1) != 0)
                    {
                        for (int i = 0; i < _subs.Count; i++)
                        {
                            _currentSub = _subs[i];
                            _currentSub.Update1();
                        }
                    }

                    // Process Update10 if requested
                    if ((src & UpdateType.Update10) != 0)
                    {
                        for (int i = 0; i < _subs.Count; i++)
                        {
                            _currentSub = _subs[i];
                            _currentSub.Update10();
                        }
                    }

                    // Process Update100 if requested
                    if ((src & UpdateType.Update100) != 0)
                    {
                        for (int i = 0; i < _subs.Count; i++)
                        {
                            _currentSub = _subs[i];
                            _currentSub.Update100();
                        }
                    }

                    // Record profiling data
                    _profiler.Record(_instructionCount(), lastRunMs);
                }
                catch (Exception e)
                {
                    EnterSafe(string.Format("{0}: {1}", _currentSub.Name, e.Message));
                }
            }

            public void EnterSafe(string reason)
            {
                if (_isSafe)
                    return;

                _isSafe = true;
                _safeReason = reason;
                
                if (_onSafe != null)
                    _onSafe();
            }

            public void ResetSafe()
            {
                _isSafe = false;
                _safeReason = "";
            }
        }
    }
}
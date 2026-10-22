using System;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class HeartbeatWatchdog
    {
        readonly double _timeoutSec;
        double _elapsedSec;
        bool _timedOut;

        public HeartbeatWatchdog(double timeoutSec = 5.0)
        {
            _timeoutSec = timeoutSec;
            _elapsedSec = 0.0;
            _timedOut = false;
        }

        public void Ping()
        {
            _elapsedSec = 0.0;
            _timedOut = false;
        }

        public void Tick(double deltaSec)
        {
            _elapsedSec += deltaSec;
            if (_elapsedSec >= _timeoutSec)
                _timedOut = true;
        }

        public bool IsTimedOut => _timedOut;
        public double ElapsedSec => _elapsedSec;
    }
}

using System;
using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        public class StuckDetector
        {
            private double _stuckSeconds;
            private double _minSpeed;
            private bool _timing;
            private double _since;

            public StuckDetector(double stuckSeconds, double minSpeed)
            {
                _stuckSeconds = stuckSeconds;
                _minSpeed = minSpeed;
                _timing = false;
                _since = 0;
            }

            public double StuckSeconds => _stuckSeconds;
            public double MinSpeed => _minSpeed;

            // progressSpeed: velocity component toward the target (m/s). thrusting: the helm is commanding motion.
            // Returns true once (thrusting && progressSpeed < MinSpeed) has held continuously for >= StuckSeconds.
            public bool Update(double now, double progressSpeed, bool thrusting)
            {
                if (thrusting && progressSpeed < _minSpeed)
                {
                    // Stuck condition holds
                    if (!_timing)
                    {
                        // Start timing
                        _timing = true;
                        _since = now;
                    }
                    else
                    {
                        // Check if stuck duration has passed
                        if (now - _since >= _stuckSeconds)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    // Stuck condition does not hold, stop timing
                    _timing = false;
                }

                return false;
            }

            public void Reset()
            {
                _timing = false;
            }
        }
    }
}
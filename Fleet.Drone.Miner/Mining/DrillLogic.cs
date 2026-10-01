namespace IngameScript
{
    public partial class Program
    {
        public class DrillLogic
        {
            public enum Phase { Advance, Retract, Done }
            public const double StallRatio = 0.1;          // stalled when forward speed < StallRatio * Feed
            public const double StallHalveSeconds = 3;     // first stall: halve the feed after this long
            public const double StallBlockSeconds = 5;     // after halving: blocked after this long stalled
            public const double RetractDoneDepth = 0.5;    // retract finished at or above this depth (m)

            double _targetDepth;
            bool _halved;
            bool _stallRunning;
            double _stallSince;

            public Phase Current { get; private set; }
            public double Feed { get; private set; }       // current advance speed (m/s)
            public bool WasBlocked { get; private set; }

            public void Start(double targetDepth, double workSpeed)
            {
                _targetDepth = targetDepth;
                Feed = workSpeed;
                Current = Phase.Advance;
                WasBlocked = false;
                _halved = false;
                _stallRunning = false;
                _stallSince = 0;
            }

            public void BeginRetract()
            {
                Current = Phase.Retract;
            }

            // depth: metres below the hole entrance along the mining direction. fwdSpeed: velocity along it.
            public Phase Update(double now, double depth, double fwdSpeed)
            {
                switch (Current)
                {
                    case Phase.Advance:
                        if (depth >= _targetDepth)
                        {
                            Current = Phase.Retract;
                            break;
                        }
                        bool stalled = fwdSpeed < StallRatio * Feed;
                        if (!stalled)
                        {
                            _stallRunning = false;
                        }
                        else if (!_stallRunning)
                        {
                            _stallRunning = true;
                            _stallSince = now;
                        }
                        else
                        {
                            double elapsed = now - _stallSince;
                            if (!_halved && elapsed >= StallHalveSeconds)
                            {
                                Feed /= 2;
                                _halved = true;
                                _stallSince = now;
                            }
                            else if (_halved && elapsed >= StallBlockSeconds)
                            {
                                WasBlocked = true;
                                Current = Phase.Retract;
                            }
                        }
                        break;
                    case Phase.Retract:
                        if (depth <= RetractDoneDepth) Current = Phase.Done;
                        break;
                }
                return Current;
            }
        }
    }
}

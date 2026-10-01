using System;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class DockingPlanner
        {
            public enum Phase { Approach, Align, Creep, Connect, Done, Failed }
            public const double StandoffTolerance = 0.5;   // m
            public const double AlignTolerance = 0.0349;   // rad (2 degrees)
            public const double PrecisionRange = 5;        // m: request Update1 inside this distance while creeping
            public const double CreepTimeout = 30;         // s

            private Phase _phase = Phase.Done;
            private double _creepStart;

            public Phase Current { get { return _phase; } }

            public void Start(double now)
            {
                _phase = Phase.Approach;
                _creepStart = now;
            }

            public PoseTarget Update(double now, MatrixD homeConn, Vector3D droneConnPos, Vector3D droneConnFwd,
                bool connectable, bool connected, double dockDist, double approachSpeed, double dockSpeed)
            {
                Vector3D homePos = homeConn.Translation;
                Vector3D homeFwd = homeConn.Forward;
                Vector3D homeUp = homeConn.Up;
                Vector3D standoff = homePos + homeFwd * dockDist;
                Vector3D facing = -homeFwd;

                // At most one transition per call.
                if (_phase != Phase.Done && connected)
                {
                    _phase = Phase.Done;
                }
                else if (_phase == Phase.Approach)
                {
                    if (Vector3D.Distance(droneConnPos, standoff) <= StandoffTolerance) _phase = Phase.Align;
                }
                else if (_phase == Phase.Align)
                {
                    double dot = Vector3D.Dot(droneConnFwd, facing);
                    if (dot > 1) dot = 1;
                    else if (dot < -1) dot = -1;
                    if (Math.Acos(dot) <= AlignTolerance)
                    {
                        _phase = Phase.Creep;
                        _creepStart = now;
                    }
                }
                else if (_phase == Phase.Creep)
                {
                    if (connectable) _phase = Phase.Connect;
                    else if (now - _creepStart > CreepTimeout) _phase = Phase.Failed;
                }

                PoseTarget t = new PoseTarget();
                t.Forward = facing;
                t.Up = homeUp;
                t.BrakeDist = -1;
                switch (_phase)
                {
                    case Phase.Approach:
                    case Phase.Align:
                        t.Position = standoff;
                        t.SpeedCap = approachSpeed;
                        t.Precision = false;
                        break;
                    case Phase.Creep:
                        t.Position = homePos;
                        t.SpeedCap = dockSpeed;
                        t.Precision = Vector3D.Distance(droneConnPos, homePos) < PrecisionRange;
                        break;
                    case Phase.Connect:
                        t.Position = droneConnPos;
                        t.SpeedCap = 0;
                        t.Precision = true;
                        break;
                    default:
                        t.Position = droneConnPos;
                        t.SpeedCap = 0;
                        t.Precision = false;
                        break;
                }
                return t;
            }

            public static Vector3D UndockTarget(MatrixD homeConn, double clearance)
            {
                return homeConn.Translation + homeConn.Forward * clearance;
            }
        }
    }
}

using System;
using VRage;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        // One carrier bay (a tagged connector) as broadcast by Fleet.Carrier (slice 2 spec §3).
        public struct BayPose
        {
            public long BayId;
            public Vector3D Position, Forward, Up;
            public Vector3D Velocity;          // world m/s of the bay point itself (v + w x r)
            public Vector3D AngularVelocity;   // carrier, world rad/s
        }

        // The beacon payload is a struct tuple, so the drone unboxes it without allocating.
        public static class Beacon
        {
            public const string Kind = "bay";

            public static MyTuple<long, Vector3D, Vector3D, Vector3D, Vector3D, Vector3D> Pack(ref BayPose p)
            {
                return new MyTuple<long, Vector3D, Vector3D, Vector3D, Vector3D, Vector3D>(
                    p.BayId, p.Position, p.Forward, p.Up, p.Velocity, p.AngularVelocity);
            }

            public static bool TryUnpack(object data, out BayPose p)
            {
                p = new BayPose();
                if (!(data is MyTuple<long, Vector3D, Vector3D, Vector3D, Vector3D, Vector3D>)) return false;
                var t = (MyTuple<long, Vector3D, Vector3D, Vector3D, Vector3D, Vector3D>)data;
                p.BayId = t.Item1; p.Position = t.Item2; p.Forward = t.Item3; p.Up = t.Item4;
                p.Velocity = t.Item5; p.AngularVelocity = t.Item6;
                return p.Forward.LengthSquared() > 0.5 && p.Up.LengthSquared() > 0.5;
            }

            // Velocity of a point on a rigid body: v_com + w x (point - com).
            public static Vector3D PointVelocity(Vector3D comVelocity, Vector3D angularVelocity, Vector3D com, Vector3D point)
            {
                return comVelocity + Vector3D.Cross(angularVelocity, point - com);
            }
        }

        // Drone side: the latest beacon for the home bay, extrapolated to "now" between beacons.
        public class HomeTracker
        {
            public const double StaleAfter = 2.0;   // s without a beacon -> the pose is no longer trusted
            public const double DeadReckon = 6.0;   // s after going stale the pose is still extrapolated (evade, not dock)
            public long HomeId { get; private set; }
            public bool Heard { get; private set; }
            BayPose _last;
            double _rxTime;

            // A new home connector (docked somewhere else): forget the old bay.
            public void SetHome(long homeId)
            {
                if (homeId == HomeId) return;
                HomeId = homeId;
                Heard = false;
            }

            public void Offer(ref BayPose p, double now)
            {
                if (HomeId == 0 || p.BayId != HomeId) return;
                _last = p;
                _rxTime = now;
                Heard = true;
            }

            public bool Fresh(double now) { return Heard && now - _rxTime < StaleAfter; }

            // Stale but recent: good enough to steer AWAY from the carrier, never to dock onto it.
            public bool CanDeadReckon(double now) { return Heard && now - _rxTime < StaleAfter + DeadReckon; }

            public Vector3D AngularVelocity { get { return _last.AngularVelocity; } }

            // After a reload: this home WAS tracked (a carrier script exists), so until a fresh beacon arrives
            // it is lost — not a static slice-1 home at its saved pose.
            public void AssumeLost(long homeId)
            {
                HomeId = homeId;
                Heard = homeId != 0;
                _rxTime = double.NegativeInfinity;
            }

            // RESET: the carrier script is gone for good; fall back to slice-1 static-home behaviour.
            public void Forget() { Heard = false; }

            // The carrier script was heard but has gone quiet: never fly at a guessed connector.
            public bool Lost(double now) { return Heard && now - _rxTime >= StaleAfter; }

            public double Age(double now) { return now - _rxTime; }

            public void Predict(double now, out MatrixD pose, out Vector3D velocity)
            {
                double age = Math.Max(0, now - _rxTime);
                var fwd = _last.Forward;
                var up = _last.Up;
                double w = _last.AngularVelocity.Length();
                if (w * age > 1e-9)
                {
                    var rot = MatrixD.CreateFromAxisAngle(_last.AngularVelocity / w, w * age);
                    fwd = Vector3D.TransformNormal(fwd, rot);
                    up = Vector3D.TransformNormal(up, rot);
                }
                pose = MatrixD.CreateWorld(_last.Position + _last.Velocity * age, Vector3D.Normalize(fwd), Vector3D.Normalize(up));
                velocity = _last.Velocity;
            }
        }
    }
}

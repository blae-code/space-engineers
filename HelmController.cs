using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// Flies the reference block (the ship controller) to a target pose that may
        /// itself be moving. The math runs in the reference block's local frame:
        /// world vectors come in through TransformNormal(v, Transpose(WorldMatrix)).
        /// </summary>
        public class HelmController : ISubsystem
        {
            const double PositionGain = 0.6;   // (m/s) of closing speed per metre of error
            const double VelocityGain = 1.5;   // (m/s^2) of acceleration per m/s of velocity error
            const double GyroGain = 2.5;       // (rad/s) per radian of attitude error
            const double MaxGyroRate = 2.0;    // rad/s

            // Thruster groups, indexed by the ref-local direction they PUSH the ship.
            const int Right = 0, Left = 1, Up = 2, Down = 3, Back = 4, Fwd = 5;

            readonly Program _p;
            readonly GridManager _grid;
            readonly List<IMyThrust>[] _groups = new List<IMyThrust>[6];
            int _groupedVersion = -1;

            bool _engaged;
            Vector3D _targetPos, _targetVel, _targetFwd, _targetUp;
            double _maxSpeed;

            public bool Engaged { get { return _engaged; } }
            public double DistanceToTarget { get; private set; } = double.MaxValue;
            public double RelativeSpeed { get; private set; }
            /// <summary>Largest of |yaw|, |pitch|, |roll| error, in radians.</summary>
            public double AlignmentError { get; private set; } = Math.PI;

            public HelmController(Program p, GridManager grid)
            {
                _p = p;
                _grid = grid;
                for (int i = 0; i < _groups.Length; i++)
                    _groups[i] = new List<IMyThrust>(16);
            }

            public void Initialize()
            {
                RegroupThrusters();
            }

            public void Update100() { }

            public void HandleMessage(FleetMessage message) { }

            // ---------------- Commands ----------------

            /// <summary>
            /// Fly the reference block to <paramref name="pos"/> while matching <paramref name="vel"/>
            /// (the target's own velocity) and facing <paramref name="fwd"/>/<paramref name="up"/>.
            /// Metrics are refreshed immediately, so callers can check them the same tick.
            /// </summary>
            public void SetTarget(Vector3D pos, Vector3D vel, Vector3D fwd, Vector3D up, double maxSpeed)
            {
                _targetPos = pos;
                _targetVel = vel;
                _targetFwd = fwd;
                _targetUp = up;
                _maxSpeed = maxSpeed;

                var ctrl = _grid.Controller;
                if (ctrl == null) return;
                if (!_engaged)
                {
                    _engaged = true;
                    ctrl.DampenersOverride = false; // we own every thruster now
                }

                MatrixD refT = MatrixD.Transpose(ctrl.WorldMatrix);
                double yaw, pitch, roll;
                Measure(ctrl, ref refT, out yaw, out pitch, out roll);
            }

            /// <summary>Hold the current pose, stationary in the world frame.</summary>
            public void HoldPosition()
            {
                var ctrl = _grid.Controller;
                if (ctrl == null) return;
                MatrixD m = ctrl.WorldMatrix;
                SetTarget(m.Translation, Vector3D.Zero, m.Forward, m.Up, 5);
            }

            /// <summary>Release every override and hand the ship back to inertial dampeners.</summary>
            public void Disengage()
            {
                _engaged = false;
                DistanceToTarget = double.MaxValue;
                AlignmentError = Math.PI;

                for (int i = 0; i < _grid.Gyros.Count; i++)
                {
                    var g = _grid.Gyros[i];
                    g.Pitch = 0;
                    g.Yaw = 0;
                    g.Roll = 0;
                    g.GyroOverride = false;
                }
                for (int i = 0; i < _grid.Thrusters.Count; i++)
                    _grid.Thrusters[i].ThrustOverridePercentage = 0;

                var ctrl = _grid.Controller;
                if (ctrl != null) ctrl.DampenersOverride = true;
            }

            // ---------------- Control loop ----------------

            public void Update10()
            {
                if (_grid.Version != _groupedVersion) RegroupThrusters();
                if (!_engaged) return;

                var ctrl = _grid.Controller;
                if (ctrl == null || ctrl.Closed)
                {
                    Disengage();
                    return;
                }

                MatrixD refM = ctrl.WorldMatrix;
                MatrixD refT = MatrixD.Transpose(refM);

                // Attitude. Roll is faded in as forward converges; it is ill-defined until then.
                double yaw, pitch, roll;
                Measure(ctrl, ref refT, out yaw, out pitch, out roll);
                double rollWeight = Math.Max(0, 1 - (Math.Abs(yaw) + Math.Abs(pitch)));
                ApplyGyros(ref refM,
                    ClampRate(pitch * GyroGain),
                    ClampRate(yaw * GyroGain),
                    ClampRate(roll * GyroGain * rollWeight));

                // Translation: braking-limited closing speed on top of the target's own velocity.
                Vector3D toTarget = _targetPos - ctrl.GetPosition();
                double dist = DistanceToTarget;
                double speed = Math.Min(_maxSpeed,
                    Math.Min(Math.Sqrt(2 * _p.Cfg.Decel * dist), dist * PositionGain));
                Vector3D desiredVel = _targetVel;
                if (dist > 1e-3) desiredVel += toTarget * (speed / dist);

                Vector3D vel = ctrl.GetShipVelocities().LinearVelocity;
                Vector3D accel = (desiredVel - vel) * VelocityGain - ctrl.GetNaturalGravity();
                Vector3D force = accel * ctrl.CalculateShipMass().PhysicalMass;

                Vector3D local = Vector3D.TransformNormal(force, refT);
                DriveAxis(Right, Left, local.X);
                DriveAxis(Up, Down, local.Y);
                DriveAxis(Back, Fwd, local.Z);
            }

            void Measure(IMyShipController ctrl, ref MatrixD refT, out double yaw, out double pitch, out double roll)
            {
                DistanceToTarget = Vector3D.Distance(_targetPos, ctrl.GetPosition());
                RelativeSpeed = (ctrl.GetShipVelocities().LinearVelocity - _targetVel).Length();

                // Desired forward in ref-local space (SE forward is -Z). Right and up are positive.
                Vector3D f = Vector3D.TransformNormal(_targetFwd, refT);
                yaw = Math.Atan2(f.X, -f.Z);
                pitch = Math.Atan2(f.Y, Math.Sqrt(f.X * f.X + f.Z * f.Z));

                roll = 0;
                if (!Vector3D.IsZero(_targetUp))
                {
                    Vector3D u = Vector3D.TransformNormal(_targetUp, refT);
                    roll = Math.Atan2(u.X, u.Y);
                }

                AlignmentError = Math.Max(Math.Abs(yaw), Math.Max(Math.Abs(pitch), Math.Abs(roll)));
            }

            void ApplyGyros(ref MatrixD refM, double pitch, double yaw, double roll)
            {
                // Gyro override axes are sign-flipped relative to a right-handed rotation,
                // hence the negated pitch (same convention as Whiplash141's ApplyGyroOverride).
                Vector3D world = Vector3D.TransformNormal(new Vector3D(-pitch, yaw, roll), refM);
                for (int i = 0; i < _grid.Gyros.Count; i++)
                {
                    var g = _grid.Gyros[i];
                    Vector3D l = Vector3D.TransformNormal(world, MatrixD.Transpose(g.WorldMatrix));
                    g.Pitch = (float)l.X;
                    g.Yaw = (float)l.Y;
                    g.Roll = (float)l.Z;
                    g.GyroOverride = true;
                }
            }

            static double ClampRate(double rate)
            {
                return Math.Max(-MaxGyroRate, Math.Min(MaxGyroRate, rate));
            }

            void DriveAxis(int positive, int negative, double force)
            {
                if (force >= 0)
                {
                    Drive(positive, force);
                    Drive(negative, 0);
                }
                else
                {
                    Drive(negative, -force);
                    Drive(positive, 0);
                }
            }

            void Drive(int group, double force)
            {
                var list = _groups[group];
                double capacity = 0;
                for (int i = 0; i < list.Count; i++)
                    if (list[i].IsWorking) capacity += list[i].MaxEffectiveThrust;

                float pct = capacity > 0 ? (float)Math.Min(force / capacity, 1.0) : 0f;
                for (int i = 0; i < list.Count; i++)
                    list[i].ThrustOverridePercentage = pct;
            }

            /// <summary>
            /// Bucket thrusters by the ref-local direction they push the ship
            /// (a thruster pushes along its own WorldMatrix.Backward).
            /// </summary>
            void RegroupThrusters()
            {
                _groupedVersion = _grid.Version;
                for (int i = 0; i < _groups.Length; i++)
                    _groups[i].Clear();

                var ctrl = _grid.Controller;
                if (ctrl == null) return;

                MatrixD refT = MatrixD.Transpose(ctrl.WorldMatrix);
                for (int i = 0; i < _grid.Thrusters.Count; i++)
                {
                    var t = _grid.Thrusters[i];
                    Vector3D p = Vector3D.TransformNormal(t.WorldMatrix.Backward, refT);
                    double ax = Math.Abs(p.X), ay = Math.Abs(p.Y), az = Math.Abs(p.Z);
                    int idx;
                    if (ax >= ay && ax >= az) idx = p.X > 0 ? Right : Left;
                    else if (ay >= az) idx = p.Y > 0 ? Up : Down;
                    else idx = p.Z > 0 ? Back : Fwd;
                    _groups[idx].Add(t);
                }
            }
        }
    }
}

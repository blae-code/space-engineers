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
            const double AutopilotReissue = 5; // s before re-arming an autopilot that switched itself off
            const string WaypointName = "FleetMiner";

            // Thruster groups, indexed by the ref-local direction they PUSH the ship.
            const int Right = 0, Left = 1, Up = 2, Down = 3, Back = 4, Fwd = 5;

            readonly Program _p;
            readonly GridManager _grid;
            readonly List<IMyThrust>[] _groups = new List<IMyThrust>[6];
            /// <summary>Working thrust (N) per push direction, refreshed every control tick.</summary>
            readonly double[] _cap = new double[6];
            const double BrakeMargin = 0.7; // plan stops on 70% of the deceleration we could produce
            int _groupedVersion = -1;

            bool _engaged;
            Vector3D _targetPos, _targetVel, _targetFwd, _targetUp;
            double _maxSpeed;

            bool _autopilot;
            Vector3D _autopilotTarget;
            double _autopilotIssuedAt = double.MinValue;

            public bool Engaged { get { return _engaged; } }
            /// <summary>True while the vanilla Remote Control autopilot is flying the ship.</summary>
            public bool OnAutopilot { get { return _autopilot; } }
            public double DistanceToTarget { get; private set; } = double.MaxValue;
            public double RelativeSpeed { get; private set; }
            /// <summary>Largest of |yaw|, |pitch|, |roll| error, in radians.</summary>
            public double AlignmentError { get; private set; } = Math.PI;
            /// <summary>
            /// Thrust available against gravity divided by weight, in the current attitude.
            /// Infinity in space. Below ~1.2 a loaded drone can no longer climb out safely.
            /// </summary>
            public double LiftRatio { get; private set; } = double.PositiveInfinity;
            /// <summary>Deceleration (m/s²) the last approach was planned with: thrust against the motion, mass and gravity.</summary>
            public double BrakeAccel { get; private set; }
            /// <summary>Planet surface clearance to keep (m); 0 = off. Set by the brain per state.</summary>
            public double AltitudeFloor;
            /// <summary>Height above the planet surface, or -1 in space.</summary>
            public double Elevation { get; private set; } = -1;

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

            public void Update100()
            {
                MeasureLift();
            }

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
                CancelAutopilot();
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

            /// <summary>
            /// Long, static legs only: hand the ship to the Remote Control autopilot, which brings
            /// vanilla collision avoidance. Never use it for a moving target (the carrier).
            /// Returns false when the reference block is not a working Remote Control.
            /// </summary>
            public bool AutopilotTo(Vector3D pos, double maxSpeed)
            {
                var rc = _grid.Controller as IMyRemoteControl;
                if (rc == null || !rc.IsWorking) return false;
                if (_engaged) Disengage();

                // Waypoints are only (re)written when the leg changes, or after the autopilot gave
                // up on its own, so the game-side waypoint allocation stays off the hot path.
                bool newLeg = !_autopilot || Vector3D.DistanceSquared(pos, _autopilotTarget) > 1;
                if (newLeg || (!rc.IsAutoPilotEnabled && _p.Clock - _autopilotIssuedAt > AutopilotReissue))
                {
                    rc.ClearWaypoints();
                    rc.AddWaypoint(pos, WaypointName);
                    rc.FlightMode = FlightMode.OneWay;
                    rc.Direction = Base6Directions.Direction.Forward;
                    rc.SpeedLimit = (float)maxSpeed;
                    rc.SetCollisionAvoidance(true);
                    rc.SetDockingMode(false);
                    rc.SetAutoPilotEnabled(true);
                    _autopilot = true;
                    _autopilotTarget = pos;
                    _autopilotIssuedAt = _p.Clock;
                }

                DistanceToTarget = Vector3D.Distance(pos, rc.GetPosition());
                RelativeSpeed = rc.GetShipSpeed();
                return true;
            }

            void CancelAutopilot()
            {
                if (!_autopilot) return;
                _autopilot = false;
                var rc = _grid.Controller as IMyRemoteControl;
                if (rc == null) return;
                rc.SetAutoPilotEnabled(false);
                rc.ClearWaypoints();
            }

            /// <summary>Release every override and hand the ship back to inertial dampeners.</summary>
            public void Disengage()
            {
                CancelAutopilot();
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

                // Translation: braking-limited closing speed on top of the target's own velocity. The
                // braking figure is what this ship can do right now: thrust pointing against the
                // motion, divided by current mass (cargo included), plus any help from gravity.
                Vector3D toTarget = _targetPos - ctrl.GetPosition();
                double dist = DistanceToTarget;
                double mass = ctrl.CalculateShipMass().PhysicalMass;
                Vector3D g = ctrl.GetNaturalGravity();
                for (int i = 0; i < 6; i++)
                {
                    double c = 0;
                    var list = _groups[i];
                    for (int k = 0; k < list.Count; k++)
                        if (list[k].IsWorking) c += list[k].MaxEffectiveThrust;
                    _cap[i] = c;
                }
                double brake = _p.Cfg.Decel > 0 ? _p.Cfg.Decel : 4;
                if (dist > 1e-3)
                {
                    Vector3D back = -toTarget / dist;
                    Vector3D u = Vector3D.TransformNormal(back, refT);
                    double f = Math.Min(AxisForce(u.X, Right, Left), Math.Min(AxisForce(u.Y, Up, Down), AxisForce(u.Z, Back, Fwd)));
                    double a = (f / mass + Vector3D.Dot(g, back)) * BrakeMargin;
                    brake = Math.Max(0.5, _p.Cfg.Decel > 0 ? Math.Min(a, _p.Cfg.Decel) : a);
                }
                BrakeAccel = brake;
                double speed = Math.Min(_maxSpeed,
                    Math.Min(Math.Sqrt(2 * brake * dist), dist * PositionGain));
                Vector3D desiredVel = _targetVel;
                if (dist > 1e-3) desiredVel += toTarget * (speed / dist);

                // Altitude floor: below it, add whatever climb rate is missing (planets only).
                double elevation;
                Elevation = ctrl.TryGetPlanetElevation(MyPlanetElevation.Surface, out elevation) ? elevation : -1;
                if (AltitudeFloor > 0 && Elevation >= 0 && Elevation < AltitudeFloor)
                {
                    Vector3D up = -Vector3D.Normalize(ctrl.GetNaturalGravity());
                    double climb = Math.Min(_maxSpeed, (AltitudeFloor - Elevation) * PositionGain + 2);
                    double vUp = Vector3D.Dot(desiredVel, up);
                    if (vUp < climb) desiredVel += up * (climb - vUp);
                }

                Vector3D vel = ctrl.GetShipVelocities().LinearVelocity;
                Vector3D accel = (desiredVel - vel) * VelocityGain - g;
                Vector3D force = accel * mass;

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
                var cfg = _p.Cfg;
                if (cfg.InvertPitch) pitch = -pitch;
                if (cfg.InvertYaw) yaw = -yaw;
                if (cfg.InvertRoll) roll = -roll;
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

            /// <summary>
            /// To hover, every ref axis must supply its share of m*g: F_i = m*g*u_i with
            /// |F_i| bounded by that axis' thrust, so lift = min_i(cap_i / |u_i|) / (m*g).
            /// </summary>
            void MeasureLift()
            {
                LiftRatio = double.PositiveInfinity;
                var ctrl = _grid.Controller;
                if (ctrl == null || ctrl.Closed) return;
                Vector3D g = ctrl.GetNaturalGravity();
                double gLen = g.Length();
                if (gLen < 0.05) return;

                MatrixD refT = MatrixD.Transpose(ctrl.WorldMatrix);
                Vector3D up = Vector3D.TransformNormal(-g / gLen, refT);
                double lift = double.PositiveInfinity;
                lift = Math.Min(lift, AxisLift(up.X, Right, Left));
                lift = Math.Min(lift, AxisLift(up.Y, Up, Down));
                lift = Math.Min(lift, AxisLift(up.Z, Back, Fwd));
                LiftRatio = lift / (ctrl.CalculateShipMass().PhysicalMass * gLen);
            }

            /// <summary>Self-test: every one of the six push directions has a working thruster.</summary>
            public bool ThrustOnAllAxes()
            {
                if (_grid.Version != _groupedVersion) RegroupThrusters();
                for (int g = 0; g < _groups.Length; g++)
                {
                    bool any = false;
                    for (int i = 0; i < _groups[g].Count && !any; i++)
                        any = _groups[g][i].IsFunctional;
                    if (!any) return false;
                }
                return true;
            }

            double AxisLift(double component, int positive, int negative)
            {
                double c = Math.Abs(component);
                if (c < 0.05) return double.PositiveInfinity;
                var list = _groups[component > 0 ? positive : negative];
                double capacity = 0;
                for (int i = 0; i < list.Count; i++)
                    if (list[i].IsFunctional) capacity += list[i].MaxEffectiveThrust; // counts sleeping (docked) thrusters too
                return capacity / c;
            }

            /// <summary>Largest force along a ref-local direction component the working thrusters allow.</summary>
            double AxisForce(double component, int positive, int negative)
            {
                double c = Math.Abs(component);
                return c < 0.05 ? double.PositiveInfinity : _cap[component > 0 ? positive : negative] / c;
            }

            void Drive(int group, double force)
            {
                var list = _groups[group];
                double capacity = _cap[group];

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

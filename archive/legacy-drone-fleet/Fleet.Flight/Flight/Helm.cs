using System;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        // The only code that commands thrusters and gyros. All game access goes through IShipIO.
        public class Helm
        {
            readonly IShipIO _io;
            readonly double[] _maxThrust;
            readonly double[] _ratios;
            MatrixD _refInShip = MatrixD.Identity;
            PoseTarget _target;

            public double RotationGain = 2.0;     // rad/s per rad of error
            public double MaxRotationRate = 1.5;  // rad/s
            public double BrakeMargin = 0.8;
            public double PositionGain = 1.5;     // 1/s: approach speed cap per metre of distance near the target
            public readonly PidVec VelocityPid = new PidVec(2.0, 0.2, 0.0, 2.0);

            public bool Active { get; private set; }
            public double Distance { get; private set; }        // reference point to target, set by Update
            public double AlignmentError { get; private set; }  // rad between reference forward and target forward

            public Helm(IShipIO io)
            {
                _io = io;
                _maxThrust = new double[6];
                _ratios = new double[6];
            }

            // Reference pose in ship-local coords; default MatrixD.Identity.
            public void SetReference(MatrixD refInShip)
            {
                _refInShip = refInShip;
            }

            // Sets the target and Active = true (does NOT reset the PID).
            public void Engage(PoseTarget target)
            {
                _target = target;
                Active = true;
            }

            public void Release()
            {
                _io.ReleaseControls();
                VelocityPid.Reset();
                Active = false;
            }

            public void Update(double dt)
            {
                if (!Active) return;

                MatrixD ship = _io.WorldMatrix;
                MatrixD refWorld = _refInShip * ship;
                Vector3D toTarget = _target.Position - refWorld.Translation;
                Distance = toTarget.Length();
                Vector3D dir = Distance > 1e-6 ? toTarget / Distance : Vector3D.Zero;
                double brakeDist = _target.BrakeDist >= 0 ? _target.BrakeDist : Distance;
                _io.GetMaxThrust(_maxThrust);
                double mass = _io.Mass;
                Vector3D g = _io.Gravity;
                MatrixD shipT = MatrixD.Transpose(ship);

                // Braking capability along -dir (thrust + the part of gravity that helps).
                Vector3D brakeLocal = Vector3D.TransformNormal(-dir, shipT);
                double aBrake = ThrustAllocator.MaxForceAlong(brakeLocal, _maxThrust) / mass + Vector3D.Dot(g, -dir);
                if (aBrake < 0) aBrake = 0;
                double speed = BrakingCurve.SafeSpeed(brakeDist, aBrake, _target.SpeedCap, BrakeMargin);
                // sqrt(2ad) has unbounded gain as d -> 0, which limit-cycles at 6 Hz; go linear close in.
                speed = Math.Min(speed, PositionGain * brakeDist);
                // Feed-forward: match the target frame's own velocity, approach relative to it (slice 2).
                Vector3D desiredVel = _target.Velocity + dir * speed;
                Vector3D accel = VelocityPid.Update(desiredVel - _io.LinearVelocity, dt);
                Vector3D force = (accel - g) * mass;  // cancel gravity
                ThrustAllocator.Allocate(Vector3D.TransformNormal(force, shipT), _maxThrust, _ratios);
                _io.SetThrust(_ratios);

                // Rotation: align the reference, not necessarily the ship nose.
                Vector3D up = GyroMath.PerpendicularUp(_target.Forward, _target.Up, refWorld.Up);
                Vector3D shipFwd, shipUp;
                GyroMath.ReferenceToShip(_refInShip, _target.Forward, up, out shipFwd, out shipUp);
                _io.SetGyro(GyroMath.AlignRate(ship.Forward, ship.Up, shipFwd, shipUp, RotationGain, MaxRotationRate));
                AlignmentError = GyroMath.AngleBetween(refWorld.Forward, _target.Forward);
            }
        }
    }
}

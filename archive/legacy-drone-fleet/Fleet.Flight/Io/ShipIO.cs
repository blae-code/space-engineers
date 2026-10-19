using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        // IShipIO over real blocks. Only Refresh allocates; every per-tick member is allocation-free.
        public class ShipIO : IShipIO
        {
            public double GyroScale = 1.0;      // multiply override values; set from GYROTEST if needed

            const double TestRate = 0.5;        // rad/s commanded in each GYROTEST phase
            const double TestPhaseSeconds = 2.0;
            const int TestPhases = 3;

            // GYROTEST phase order and names. Each phase rotates about one ship-local basis axis of VRage's
            // right-handed frame: +X = Right, +Y = Up, +Z = Backward. GyroMath.ToGyroOverride maps the local
            // X/Y/Z components of a world omega onto Pitch/Yaw/Roll (times PitchSign/YawSign/RollSign), so
            // commanding +Right, +Up and +Backward isolates exactly one of those three signs per phase.
            // A positive command is a right-hand-rule rotation about that axis; the measurement uses the same
            // axis, so ratio ~ +1 means the sign constant is right and ~ -1 means it is flipped.
            static readonly string[] AxisNames = { "pitch", "yaw", "roll" };

            readonly List<IMyThrust>[] _byDir;
            readonly double[] _testRatio;
            IMyShipController _ctrl;
            List<IMyGyro> _gyros;

            int _testPhase = -1;                // -1 = not running, else index into AxisNames
            double _phaseStart;

            public ShipIO()
            {
                _byDir = new List<IMyThrust>[6];
                for (int i = 0; i < 6; i++) _byDir[i] = new List<IMyThrust>();
                _testRatio = new double[TestPhases];
            }

            // Rebinds blocks (call after every block scan). Groups thrusters by push direction in ctrl's frame.
            public void Refresh(IMyShipController ctrl, List<IMyThrust> thrusters, List<IMyGyro> gyros)
            {
                _ctrl = ctrl;
                for (int i = 0; i < 6; i++) _byDir[i].Clear();
                if (ctrl != null && thrusters != null)
                {
                    MatrixD ship = ctrl.WorldMatrix;
                    for (int i = 0; i < thrusters.Count; i++)
                    {
                        IMyThrust t = thrusters[i];
                        if (t == null) continue;
                        _byDir[GroupOf(ship, t.WorldMatrix.Backward)].Add(t);
                    }
                }
                _gyros = gyros;
            }

            public bool TryGetElevation(out double elevation)
            {
                elevation = 0;
                if (_ctrl == null) return false;
                return _ctrl.TryGetPlanetElevation(MyPlanetElevation.Surface, out elevation);
            }

            // Pure helper: which ship-local group (Base6Directions.Direction) a thruster's push belongs to.
            public static int GroupOf(MatrixD shipWorld, Vector3D thrusterPushWorld)
            {
                Vector3D local = Vector3D.TransformNormal(thrusterPushWorld, MatrixD.Transpose(shipWorld));
                return (int)Base6Directions.GetClosestDirection((Vector3)local);
            }

            // Pure helper: GYROTEST verdict for a measured/commanded ratio. Constant strings, no allocation.
            public static string GyroVerdict(double ratio)
            {
                if (ratio >= 0.7 && ratio <= 1.3) return "OK";
                if (ratio >= -1.3 && ratio <= -0.7) return "SIGN FLIPPED";
                if (ratio > 5) return "UNITS: RPM?";
                return "UNEXPECTED";
            }

            public MatrixD WorldMatrix
            {
                get
                {
                    if (_ctrl == null) return MatrixD.Identity;
                    MatrixD m = _ctrl.WorldMatrix;
                    m.Translation = _ctrl.CenterOfMass;
                    return m;
                }
            }

            public Vector3D LinearVelocity
            {
                get { return _ctrl == null ? Vector3D.Zero : _ctrl.GetShipVelocities().LinearVelocity; }
            }

            public Vector3D AngularVelocity
            {
                get { return _ctrl == null ? Vector3D.Zero : _ctrl.GetShipVelocities().AngularVelocity; }
            }

            public Vector3D Gravity
            {
                get { return _ctrl == null ? Vector3D.Zero : _ctrl.GetNaturalGravity(); }
            }

            // 0 before Refresh. Once bound, never 0 or NaN (Helm divides by it): a non-positive or NaN mass reads 1.
            public double Mass
            {
                get
                {
                    if (_ctrl == null) return 0;
                    double m = _ctrl.CalculateShipMass().PhysicalMass;
                    return m > 0 ? m : 1;
                }
            }

            public void GetMaxThrust(double[] maxByDir)
            {
                for (int g = 0; g < 6; g++)
                {
                    double sum = 0;
                    List<IMyThrust> list = _byDir[g];
                    for (int i = 0; i < list.Count; i++)
                    {
                        IMyThrust t = list[i];
                        if (t.IsFunctional && t.Enabled) sum += t.MaxEffectiveThrust;
                    }
                    maxByDir[g] = sum;
                }
            }

            public void SetThrust(double[] ratioByDir)
            {
                for (int g = 0; g < 6; g++)
                {
                    float r = (float)ratioByDir[g];
                    List<IMyThrust> list = _byDir[g];
                    for (int i = 0; i < list.Count; i++) list[i].ThrustOverridePercentage = r;
                }
            }

            public void SetGyro(Vector3D worldOmega)
            {
                if (_gyros == null) return;
                for (int i = 0; i < _gyros.Count; i++)
                {
                    IMyGyro gy = _gyros[i];
                    Vector3D o = GyroMath.ToGyroOverride(worldOmega, gy.WorldMatrix) * GyroScale;
                    gy.GyroOverride = true;
                    gy.Pitch = (float)o.X;
                    gy.Yaw = (float)o.Y;
                    gy.Roll = (float)o.Z;
                }
            }

            // Safe without Refresh: every list is preallocated and the controller/gyros are null-checked.
            public void ReleaseControls()
            {
                for (int g = 0; g < 6; g++)
                {
                    List<IMyThrust> list = _byDir[g];
                    for (int i = 0; i < list.Count; i++) list[i].ThrustOverridePercentage = 0f;
                }
                if (_gyros != null)
                {
                    for (int i = 0; i < _gyros.Count; i++)
                    {
                        IMyGyro gy = _gyros[i];
                        gy.GyroOverride = false;
                        gy.Pitch = 0f;
                        gy.Yaw = 0f;
                        gy.Roll = 0f;
                    }
                }
                if (_ctrl != null) _ctrl.DampenersOverride = true;
            }

            // ---- GYROTEST ----------------------------------------------------------------------------

            // Three consecutive 2 s phases (pitch about Right, yaw about Up, roll about Backward),
            // each commanding 0.5 rad/s.
            public void BeginGyroTest(double now)
            {
                for (int i = 0; i < TestPhases; i++) _testRatio[i] = 0;
                _testPhase = 0;
                _phaseStart = now;
            }

            public bool GyroTestRunning
            {
                get { return _testPhase >= 0; }
            }

            // Call every Update10 while running. Replaces 'report' with "GYROTEST running: <axis>" while a
            // phase is in progress; when all three finish, writes one verdict line per axis, then releases.
            public void UpdateGyroTest(double now, System.Text.StringBuilder report)
            {
                if (_testPhase < 0) return;

                if (now - _phaseStart >= TestPhaseSeconds)
                {
                    // Last sample of the phase: the ship has had the whole phase to settle on the rate.
                    Vector3D axis = TestAxis(_testPhase);
                    _testRatio[_testPhase] = Vector3D.Dot(AngularVelocity, axis) / TestRate;
                    _testPhase++;
                    _phaseStart = now;

                    if (_testPhase >= TestPhases)
                    {
                        _testPhase = -1;
                        ReleaseControls();
                        report.Clear();
                        for (int i = 0; i < TestPhases; i++)
                        {
                            report.Append("GYROTEST ");
                            report.Append(AxisNames[i]);
                            report.Append(": measured/commanded = ");
                            SbFormat.AppendFixed(report, _testRatio[i], 2);
                            report.Append(" (");
                            report.Append(GyroVerdict(_testRatio[i]));
                            report.Append(")\n");
                        }
                        return;
                    }
                }

                SetGyro(TestAxis(_testPhase) * TestRate);
                report.Clear();
                report.Append("GYROTEST running: ");
                report.Append(AxisNames[_testPhase]);
            }

            // Current world direction of the phase's ship-local axis (re-read each call, so it tracks the ship).
            Vector3D TestAxis(int phase)
            {
                MatrixD m = WorldMatrix;
                if (phase == 0) return m.Right;
                if (phase == 1) return m.Up;
                return m.Backward;
            }
        }
    }
}

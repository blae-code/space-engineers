// Excavator.Mastodon · Space Engineers 1 MDK2 PB Script
// Target Block: [MAS] PB Excavator
// Grid: Large Grid Tractor Unit (MAS-01-T) · Update: Update1 / Update10
// Subsystem Groups: MAS STAB, MAS BOOM, MAS TOOL, MAS WHEEL, MAS HITCH, MAS AIRLOCKS, MAS VENTS CABIN, MAS VENTS AIRLOCK
// IGC Channels: TMS.MAS.STATUS, TMS.CONSIST.SYNC, TMS.CONSIST.TRACTION, TMS.CONSIST.SEVER, TMS.AIRLOCK.CYCLE, TMS.ENV.MODE
// Rule C4: Native blocks provide safety; PB provides decision & kinematic logic.

using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        #region State Constants & String Tables (Minifier-Safe Zero-GC)
        public const int STATE_TRANSIT = 0;
        public const int STATE_ANCHORING = 1;
        public const int STATE_EXCAVATING = 2;
        public const int STATE_RETRACTING = 3;

        public const int MACRO_NONE = 0;
        public const int MACRO_BENCH = 1;
        public const int MACRO_BORE = 2;
        public const int MACRO_TRENCH = 3;
        public const int MACRO_CRANE = 4;
        public const int MACRO_SALVAGE = 5;
        public const int MACRO_STOW = 6;
        public const int MACRO_DEPLOY = 7;

        public const int TOOL_MINING = 0;
        public const int TOOL_SALVAGE = 1;
        public const int TOOL_FABRICATION = 2;

        public const int POWER_MODE_SILENT = 0;
        public const int POWER_MODE_CRUISE = 1;
        public const int POWER_MODE_EXCAVATE = 2;
        public const int POWER_MODE_EMERGENCY = 3;
        public const int POWER_MODE_SHORE = 4;

        public const int ENV_EARTH_SCAVENGE = 0;
        public const int ENV_CLOSED_LOOP_VACUUM = 1;
        public const int ENV_CORROSIVE_ISOLATION = 2;

        public const int AIRLOCK_IDLE = 0;
        public const int AIRLOCK_EGRESS = 1;
        public const int AIRLOCK_INGRESS = 2;
        public const int AIRLOCK_DECON = 3;

        static readonly string[] StateNames = new string[]
        {
            "TRANSIT",
            "ANCHORING",
            "EXCAVATING",
            "RETRACTING"
        };

        static readonly string[] MacroNames = new string[]
        {
            "MANUAL_CTC",
            "BENCH_QUARRY",
            "VERTICAL_COLUMN_BORE",
            "TRENCH_CARVE",
            "CRANE_RECOVERY",
            "SALVAGE_DECONSTRUCT",
            "TRAVEL_STOW",
            "DEPLOY_READY"
        };

        static readonly string[] ToolModeNames = new string[]
        {
            "MINING_BEAM",
            "SALVAGE_BEAM",
            "FABRICATION_BEAM"
        };

        static readonly string[] PowerModeNames = new string[]
        {
            "SILENT_WATCHKEEPING",
            "TRACTIVE_CRUISE",
            "HEAVY_EXCAVATION",
            "EMERGENCY_FAILOVER",
            "SHORE_RECHARGE"
        };

        static readonly string[] EnvModeNames = new string[]
        {
            "EARTH_SCAVENGE",
            "CLOSED_LOOP_VACUUM",
            "CORROSIVE_ISOLATION"
        };

        static readonly string[] AirlockStateNames = new string[]
        {
            "IDLE",
            "EGRESS_EVACUATE",
            "INGRESS_PRESSURIZE",
            "DECON_WASH"
        };
        #endregion

        #region Hardware References
        IMyShipController _helm;
        IMyMotorAdvancedStator _turretAzimuth;
        IMyMotorAdvancedStator _boomShoulder;
        IMyMotorAdvancedStator _boomElbow;
        IMyMotorAdvancedStator _wristPitch;
        IMyMotorAdvancedStator _wristRoll;
        IMyShipWelder _toolGrapple; // Or magnetic plate
        IMyFunctionalBlock _clampBoomStow;

        readonly List<IMyPistonBase> _outriggersFwd = new List<IMyPistonBase>(2);
        readonly List<IMyPistonBase> _outriggersAft = new List<IMyPistonBase>(2);
        readonly List<IMyShipConnector> _outriggerPads = new List<IMyShipConnector>(4);

        IMyMotorAdvancedStator _hitchYaw;
        IMyMotorAdvancedStator _hitchPitch;
        IMyMotorAdvancedStator _hitchRoll;
        readonly List<IMyFunctionalBlock> _hitchClamps = new List<IMyFunctionalBlock>(2);

        readonly List<IMyFunctionalBlock> _miningLasers = new List<IMyFunctionalBlock>(2);
        readonly List<IMyAirVent> _cabinVents = new List<IMyAirVent>(4);
        readonly List<IMyAirVent> _atmoIntakeVents = new List<IMyAirVent>(2);
        readonly List<IMyDoor> _airlockDoorsInner = new List<IMyDoor>(4);
        readonly List<IMyDoor> _airlockDoorsOuter = new List<IMyDoor>(4);
        readonly List<IMyAirVent> _airlockVents = new List<IMyAirVent>(4);
        readonly List<IMyLightingBlock> _deconLights = new List<IMyLightingBlock>(4);
        #endregion

        #region Kinematics & Solvers
        // Boom link lengths (meters): Shoulder L1 = 15.0 m, Elbow L2 = 17.5 m, Tool offset L3 = 2.5 m
        const double L1 = 15.0;
        const double L2 = 17.5;
        const double L3 = 2.5;
        const double RAD2DEG = 180.0 / Math.PI;
        const double DEG2RAD = Math.PI / 180.0;

        double _targetX = 0.0;
        double _targetY = -5.0;
        double _targetZ = 18.0;

        double _macroTimer = 0.0;
        int _macroStep = 0;
        double _boreRadius = 6.0;
        double _boreAngle = 0.0;
        double _boreDepth = -5.0;
        #endregion

        #region Operational State
        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sbEcho = new StringBuilder(512);
        readonly StringBuilder _sbIgc = new StringBuilder(256);
        readonly List<string> _args = new List<string>(8);

        int _currentState = STATE_TRANSIT;
        int _activeMacro = MACRO_STOW;
        int _currentToolMode = TOOL_MINING;
        int _currentPowerMode = POWER_MODE_CRUISE;
        int _currentEnvMode = ENV_EARTH_SCAVENGE;
        int _airlockState = AIRLOCK_IDLE;
        double _airlockTimer = 0.0;

        int _tickCounter;
        bool _outriggersLocked;
        bool _hitchClampsEngaged;
        bool _dpuPushActive;
        bool _sternDockExtended;
        float _vehicleSpeedKmh;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1 | UpdateFrequency.Update10;
            LoadConfiguration();
            InitHardware();
            Echo("Excavator.Mastodon Initialized.");
        }

        public void Save()
        {
            // Persist macro state and stow status to Storage
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (!string.IsNullOrEmpty(argument))
            {
                HandleCommand(argument);
                return;
            }

            if ((updateSource & UpdateType.Update1) != 0)
            {
                UpdateKinematicsLoop();
            }

            if ((updateSource & UpdateType.Update10) != 0)
            {
                UpdateSubsystemsLoop();
            }
        }

        #region Kinematics & Macro Execution (60 Hz Hot Path)
        void UpdateKinematicsLoop()
        {
            _tickCounter++;

            // 1. Read vehicle motion
            if (_helm != null)
            {
                _vehicleSpeedKmh = (float)(_helm.GetShipSpeed() * 3.6);
            }

            // 2. High-speed highway sway stabilization
            if (_vehicleSpeedKmh > 30.0f && !_hitchClampsEngaged)
            {
                SetHitchClamps(true);
            }

            // 3. Auto-leveling outrigger routine
            if (_currentState == STATE_ANCHORING)
            {
                ExecuteAutoLeveling();
            }

            // 4. Boom Inverse Kinematics & Macro Execution
            if (_currentState == STATE_EXCAVATING)
            {
                ExecuteActiveMacro();
            }
            else if (_activeMacro == MACRO_STOW)
            {
                ExecuteTravelStow();
            }

            // 5. Zero-GC Status Output
            if ((_tickCounter & 63) == 0)
            {
                RenderKinematicsEcho();
            }
        }

        void ExecuteActiveMacro()
        {
            _macroTimer += 1.0 / 60.0;

            switch (_activeMacro)
            {
                case MACRO_BENCH:
                    // Planar horizontal raster sweep
                    _targetX = Math.Sin(_macroTimer * 0.25) * 12.0;
                    _targetZ = 20.0 + Math.Cos(_macroTimer * 0.05) * 4.0;
                    _targetY = -8.0;
                    SolveAndApplyIK(_targetX, _targetY, _targetZ, true);
                    break;

                case MACRO_BORE:
                    // Concentric cylindrical vertical column bore
                    _boreAngle += 0.05;
                    _targetX = Math.Cos(_boreAngle) * _boreRadius;
                    _targetZ = 16.0 + Math.Sin(_boreAngle) * _boreRadius;
                    _targetY = _boreDepth;
                    if (_wristRoll != null)
                    {
                        _wristRoll.TargetVelocityRPM = 5.0f; // Continuous shaft spin
                    }
                    SolveAndApplyIK(_targetX, _targetY, _targetZ, true); // true = lock aim straight down [0, -1, 0]
                    break;

                case MACRO_TRENCH:
                    // Linear trench carve along travel heading
                    _targetX = 0.0;
                    _targetZ = 14.0 + Math.Sin(_macroTimer * 0.2) * 8.0;
                    _targetY = -4.0;
                    SolveAndApplyIK(_targetX, _targetY, _targetZ, true);
                    break;

                case MACRO_CRANE:
                    // Manual offset hover with grapple ready
                    SolveAndApplyIK(_targetX, _targetY, _targetZ, false);
                    break;

                case MACRO_SALVAGE:
                    // Planar raster deconstruction sweep at 20 m standoff
                    _targetX = Math.Sin(_macroTimer * 0.3) * 10.0;
                    _targetZ = 22.0;
                    _targetY = 2.0 + Math.Cos(_macroTimer * 0.1) * 6.0;
                    SolveAndApplyIK(_targetX, _targetY, _targetZ, false);
                    break;
            }
        }

        void SolveAndApplyIK(double tx, double ty, double tz, bool lockVerticalAim)
        {
            if (_turretAzimuth == null || _boomShoulder == null || _boomElbow == null) return;

            // 1. Azimuth angle (Yaw)
            double azimuthRad = Math.Atan2(tx, tz);
            _turretAzimuth.TargetVelocityRPM = (float)((azimuthRad - _turretAzimuth.Angle) * 3.0);

            // 2. Planar Reach (R) and Height (Y)
            double r = Math.Sqrt(tx * tx + tz * tz);
            double dy = ty;

            // 3. Two-Link Inverse Kinematics (Law of Cosines)
            double distSq = r * r + dy * dy;
            double dist = Math.Sqrt(distSq);

            // Clamp reach inside physical envelope (min 6.0 m, max 32.0 m)
            if (dist > (L1 + L2 - 0.5)) dist = L1 + L2 - 0.5;
            if (dist < 6.0) dist = 6.0;

            double cosElbow = (L1 * L1 + L2 * L2 - dist * dist) / (2.0 * L1 * L2);
            cosElbow = MathHelper.Clamp(cosElbow, -1.0, 1.0);
            double elbowRad = Math.PI - Math.Acos(cosElbow);

            double alpha = Math.Atan2(dy, r);
            double cosShoulder = (L1 * L1 + dist * dist - L2 * L2) / (2.0 * L1 * dist);
            cosShoulder = MathHelper.Clamp(cosShoulder, -1.0, 1.0);
            double shoulderRad = alpha + Math.Acos(cosShoulder);

            // 4. Apply joint velocities toward target angles
            _boomShoulder.TargetVelocityRPM = (float)((shoulderRad - _boomShoulder.Angle) * 2.5);
            _boomElbow.TargetVelocityRPM = (float)((elbowRad - _boomElbow.Angle) * 2.5);

            // 5. Downward Vertical Aim Compensation [0, -1, 0]
            if (_wristPitch != null)
            {
                if (lockVerticalAim)
                {
                    double wristRad = -(shoulderRad - elbowRad) - (Math.PI * 0.5);
                    _wristPitch.TargetVelocityRPM = (float)((wristRad - _wristPitch.Angle) * 3.0);
                }
                else
                {
                    _wristPitch.TargetVelocityRPM = 0.0f;
                }
            }
        }

        void ExecuteTravelStow()
        {
            if (_turretAzimuth == null || _boomShoulder == null || _boomElbow == null) return;

            // Align Azimuth to 180 deg Aft (0 relative to spine Z trough)
            double targetAzimuth = Math.PI; // 180 deg
            _turretAzimuth.TargetVelocityRPM = (float)((targetAzimuth - _turretAzimuth.Angle) * 2.0);

            // Lower shoulder to 0 rad and fold elbow flat
            _boomShoulder.TargetVelocityRPM = (float)((0.0 - _boomShoulder.Angle) * 1.5);
            _boomElbow.TargetVelocityRPM = (float)((0.0 - _boomElbow.Angle) * 1.5);

            if (_wristPitch != null)
            {
                _wristPitch.TargetVelocityRPM = (float)((0.0 - _wristPitch.Angle) * 1.5);
            }
            if (_wristRoll != null)
            {
                _wristRoll.TargetVelocityRPM = (float)((0.0 - _wristRoll.Angle) * 1.5);
            }
        }

        void ExecuteAutoLeveling()
        {
            if (_helm == null) return;

            // Gravity vector in ship-local coordinates
            Vector3D localGravity = Vector3D.TransformNormal(_helm.GetTotalGravity(), MatrixD.Transpose(_helm.WorldMatrix));
            double pitchErr = localGravity.Z; // Forward / Aft inclination
            double rollErr = localGravity.X;  // Port / Starboard inclination

            bool leveled = Math.Abs(pitchErr) < 0.05 && Math.Abs(rollErr) < 0.05;

            for (int i = 0; i < _outriggersFwd.Count; i++)
            {
                float vel = (pitchErr < -0.05) ? 0.3f : -0.3f;
                _outriggersFwd[i].Velocity = vel;
            }

            for (int i = 0; i < _outriggersAft.Count; i++)
            {
                float vel = (pitchErr > 0.05) ? 0.3f : -0.3f;
                _outriggersAft[i].Velocity = vel;
            }

            if (leveled)
            {
                _outriggersLocked = true;
                _currentState = STATE_EXCAVATING;
                for (int i = 0; i < _outriggersFwd.Count; i++) _outriggersFwd[i].Velocity = 0.0f;
                for (int i = 0; i < _outriggersAft.Count; i++) _outriggersAft[i].Velocity = 0.0f;
            }
        }
        #endregion

        #region Subsystems & Environmental Loop (10 Hz Loop)
        void UpdateSubsystemsLoop()
        {
            // 1. DPU Throttle Synchronization across consist drawbar
            if (_helm != null)
            {
                Vector3 move = _helm.MoveIndicator;
                bool forwardThrottle = move.Z < -0.1f;
                bool braking = move.Z > 0.1f;

                if (forwardThrottle && !_dpuPushActive)
                {
                    _dpuPushActive = true;
                    BroadcastConsistTraction(100, 0);
                }
                else if (braking && _dpuPushActive)
                {
                    _dpuPushActive = false;
                    BroadcastConsistTraction(0, 100); // Dynamic regenerative counter-braking
                }
            }

            // 2. Active Gas-Reclamation Airlock Sequencing
            UpdateAirlockCycling();

            // 3. Environmental Mode Sequencing
            UpdateAtmosphericMode();

            // 4. Consist Status Broadcast
            BroadcastKinematicsStatus();
        }

        void UpdateAirlockCycling()
        {
            if (_airlockState == AIRLOCK_IDLE) return;

            _airlockTimer += 0.166; // ≈ 10 ticks at 60 Hz

            if (_airlockState == AIRLOCK_EGRESS)
            {
                // Step 1: Seal inner doors
                SetDoorList(_airlockDoorsInner, false);

                // Step 2: Depressurize chamber (gas reclamation)
                SetVentDepressurize(_airlockVents, true);

                // Step 3: Once evacuated (> 2 s), open outer door
                if (_airlockTimer >= 2.0)
                {
                    SetDoorList(_airlockDoorsOuter, true);
                    _airlockState = AIRLOCK_IDLE;
                    _airlockTimer = 0.0;
                }
            }
            else if (_airlockState == AIRLOCK_INGRESS)
            {
                // Step 1: Seal outer doors
                SetDoorList(_airlockDoorsOuter, false);

                // Step 2: Cyan UV Decontamination washdown
                SetLightState(_deconLights, true);

                // Step 3: Pressurize chamber
                SetVentDepressurize(_airlockVents, false);

                // Step 4: After 3 s decontamination, unseal inner door
                if (_airlockTimer >= 3.0)
                {
                    SetLightState(_deconLights, false);
                    SetDoorList(_airlockDoorsInner, true);
                    _airlockState = AIRLOCK_IDLE;
                    _airlockTimer = 0.0;
                }
            }
        }

        void UpdateAtmosphericMode()
        {
            if (_currentEnvMode == ENV_EARTH_SCAVENGE)
            {
                // Open exterior intake vents to scavenge free planetary O2
                for (int i = 0; i < _atmoIntakeVents.Count; i++)
                {
                    _atmoIntakeVents[i].Depressurize = true; // Sucking outside O2 into tanks
                    _atmoIntakeVents[i].Enabled = true;
                }
            }
            else
            {
                // Vacuum / Corrosive isolation: close intake vents, rely 100% on internal O2 tanks
                for (int i = 0; i < _atmoIntakeVents.Count; i++)
                {
                    _atmoIntakeVents[i].Enabled = false;
                }
            }
        }

        void BroadcastKinematicsStatus()
        {
            _sbIgc.Clear();
            _sbIgc.Append("KINEMATICS;");
            _sbIgc.Append(StateNames[_currentState]).Append(';');
            _sbIgc.Append(MacroNames[_activeMacro]).Append(';');
            _sbIgc.Append(_outriggersLocked ? "LOCKED" : "UNLOCKED").Append(';');
            _sbIgc.Append(_hitchClampsEngaged ? "CLAMPED" : "FREE").Append(';');
            _sbIgc.Append(_dpuPushActive ? "PUSH" : "IDLE").Append(';');
            SbFormat.AppendDouble(_sbIgc, _vehicleSpeedKmh, 1);

            IGC.SendBroadcastMessage("TMS.MAS.STATUS", _sbIgc.ToString());
        }

        void BroadcastConsistTraction(int pushPct, int brakePct)
        {
            _sbIgc.Clear();
            _sbIgc.Append("DPU;");
            SbFormat.AppendInt(_sbIgc, pushPct);
            _sbIgc.Append(';');
            SbFormat.AppendInt(_sbIgc, brakePct);

            IGC.SendBroadcastMessage("TMS.CONSIST.TRACTION", _sbIgc.ToString());
        }
        #endregion

        #region Command Handling
        void HandleCommand(string argument)
        {
            IgcProtocol.Split(argument, _args);
            if (_args.Count == 0) return;

            string cmd = _args[0];

            if (string.Compare(cmd, "STOW", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _activeMacro = MACRO_STOW;
                _currentState = STATE_TRANSIT;
            }
            else if (string.Compare(cmd, "DEPLOY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _activeMacro = MACRO_DEPLOY;
                _currentState = STATE_EXCAVATING;
                _targetX = 0.0;
                _targetY = -5.0;
                _targetZ = 18.0;
            }
            else if (string.Compare(cmd, "MACRO", StringComparison.OrdinalIgnoreCase) == 0 && _args.Count > 1)
            {
                string m = _args[1];
                if (string.Compare(m, "BENCH", StringComparison.OrdinalIgnoreCase) == 0) _activeMacro = MACRO_BENCH;
                else if (string.Compare(m, "BORE", StringComparison.OrdinalIgnoreCase) == 0) _activeMacro = MACRO_BORE;
                else if (string.Compare(m, "TRENCH", StringComparison.OrdinalIgnoreCase) == 0) _activeMacro = MACRO_TRENCH;
                else if (string.Compare(m, "CRANE", StringComparison.OrdinalIgnoreCase) == 0) _activeMacro = MACRO_CRANE;
                else if (string.Compare(m, "SALVAGE", StringComparison.OrdinalIgnoreCase) == 0) _activeMacro = MACRO_SALVAGE;
                else _activeMacro = MACRO_NONE;

                _currentState = STATE_EXCAVATING;
                _macroTimer = 0.0;
            }
            else if (string.Compare(cmd, "STABILIZE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(cmd, "LEVEL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _currentState = STATE_ANCHORING;
                _outriggersLocked = false;
            }
            else if (string.Compare(cmd, "UNSTABILIZE", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(cmd, "RETRACT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _currentState = STATE_RETRACTING;
                _outriggersLocked = false;
                for (int i = 0; i < _outriggersFwd.Count; i++) _outriggersFwd[i].Velocity = -0.5f;
                for (int i = 0; i < _outriggersAft.Count; i++) _outriggersAft[i].Velocity = -0.5f;
            }
            else if (string.Compare(cmd, "HITCH", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (_args.Count > 1 && string.Compare(_args[1], "CLAMP", StringComparison.OrdinalIgnoreCase) == 0)
                    SetHitchClamps(true);
                else if (_args.Count > 1 && string.Compare(_args[1], "FREE", StringComparison.OrdinalIgnoreCase) == 0)
                    SetHitchClamps(false);
                else
                    SetHitchClamps(!_hitchClampsEngaged);
            }
            else if (string.Compare(cmd, "TOOL_MODE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _currentToolMode = (_currentToolMode + 1) % 3;
            }
            else if (string.Compare(cmd, "AIRLOCK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string action = (_args.Count > 1) ? _args[1] : "CYCLE";
                if (string.Compare(action, "EGRESS", StringComparison.OrdinalIgnoreCase) == 0)
                    _airlockState = AIRLOCK_EGRESS;
                else if (string.Compare(action, "INGRESS", StringComparison.OrdinalIgnoreCase) == 0)
                    _airlockState = AIRLOCK_INGRESS;
                else
                    _airlockState = (_airlockState == AIRLOCK_IDLE) ? AIRLOCK_EGRESS : AIRLOCK_IDLE;

                _airlockTimer = 0.0;
            }
            else if (string.Compare(cmd, "ENV_MODE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _currentEnvMode = (_currentEnvMode + 1) % 3;
            }
            else if (string.Compare(cmd, "RELOAD", StringComparison.OrdinalIgnoreCase) == 0)
            {
                LoadConfiguration();
                InitHardware();
            }
        }

        void SetHitchClamps(bool clamp)
        {
            _hitchClampsEngaged = clamp;
            for (int i = 0; i < _hitchClamps.Count; i++)
            {
                _hitchClamps[i].Enabled = clamp;
            }
            _sbIgc.Clear();
            _sbIgc.Append("HITCH;CLAMP=").Append(clamp ? "TRUE" : "FALSE");
            IGC.SendBroadcastMessage("TMS.CONSIST.SYNC", _sbIgc.ToString());
        }

        void SetDoorList(List<IMyDoor> doors, bool open)
        {
            for (int i = 0; i < doors.Count; i++)
            {
                if (open) doors[i].OpenDoor();
                else doors[i].CloseDoor();
            }
        }

        void SetVentDepressurize(List<IMyAirVent> vents, bool depressurize)
        {
            for (int i = 0; i < vents.Count; i++)
            {
                vents[i].Depressurize = depressurize;
            }
        }

        void SetLightState(List<IMyLightingBlock> lights, bool on)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].Enabled = on;
            }
        }
        #endregion

        #region Hardware Discovery & Configuration
        void LoadConfiguration()
        {
            _ini.Clear();
            MyIniParseResult result;
            if (_ini.TryParse(Me.CustomData, out result))
            {
                _currentPowerMode = _ini.Get("Excavator", "PowerMode").ToInt32(_currentPowerMode);
                _currentEnvMode = _ini.Get("Excavator", "EnvMode").ToInt32(_currentEnvMode);
            }
        }

        void InitHardware()
        {
            _helm = GetBlockOnGrid<IMyShipController>("MAS Helm");
            _turretAzimuth = GetBlockOnGrid<IMyMotorAdvancedStator>("MAS Turret Azimuth");
            _boomShoulder = GetBlockOnGrid<IMyMotorAdvancedStator>("MAS Boom Shoulder");
            _boomElbow = GetBlockOnGrid<IMyMotorAdvancedStator>("MAS Boom Elbow");
            _wristPitch = GetBlockOnGrid<IMyMotorAdvancedStator>("MAS Tool Wrist Pitch");
            _wristRoll = GetBlockOnGrid<IMyMotorAdvancedStator>("MAS Wrist Roll");
            _clampBoomStow = GetBlockOnGrid<IMyFunctionalBlock>("MAS Clamp Boom Stow");

            // Outriggers
            _outriggersFwd.Clear();
            _outriggersAft.Clear();
            IMyPistonBase pFwdPort = GetBlockOnGrid<IMyPistonBase>("MAS Outrigger Fwd Port");
            IMyPistonBase pFwdStbd = GetBlockOnGrid<IMyPistonBase>("MAS Outrigger Fwd Stbd");
            IMyPistonBase pAftPort = GetBlockOnGrid<IMyPistonBase>("MAS Outrigger Aft Port");
            IMyPistonBase pAftStbd = GetBlockOnGrid<IMyPistonBase>("MAS Outrigger Aft Stbd");
            if (pFwdPort != null) _outriggersFwd.Add(pFwdPort);
            if (pFwdStbd != null) _outriggersFwd.Add(pFwdStbd);
            if (pAftPort != null) _outriggersAft.Add(pAftPort);
            if (pAftStbd != null) _outriggersAft.Add(pAftStbd);

            // Hitch & Stabilizers
            _hitchYaw = GetBlockOnGrid<IMyMotorAdvancedStator>("MAS Hitch Rotor Yaw");
            _hitchPitch = GetBlockOnGrid<IMyMotorAdvancedStator>("MAS Hitch Hinge Pitch");
            _hitchRoll = GetBlockOnGrid<IMyMotorAdvancedStator>("MAS Hitch Rotor Roll");
            _hitchClamps.Clear();
            IMyFunctionalBlock hcPort = GetBlockOnGrid<IMyFunctionalBlock>("MAS Hitch Clamp Port");
            IMyFunctionalBlock hcStbd = GetBlockOnGrid<IMyFunctionalBlock>("MAS Hitch Clamp Stbd");
            if (hcPort != null) _hitchClamps.Add(hcPort);
            if (hcStbd != null) _hitchClamps.Add(hcStbd);

            // Vents & Airlocks
            _cabinVents.Clear();
            GridTerminalSystem.GetBlocksOfType(_cabinVents, b => b.CubeGrid == Me.CubeGrid && b.CustomName.StartsWith("MAS Vent Bridge"));
            _atmoIntakeVents.Clear();
            GridTerminalSystem.GetBlocksOfType(_atmoIntakeVents, b => b.CubeGrid == Me.CubeGrid && b.CustomName.StartsWith("MAS Vent Atmo Intake"));

            _airlockDoorsInner.Clear();
            GridTerminalSystem.GetBlocksOfType(_airlockDoorsInner, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("Inner"));
            _airlockDoorsOuter.Clear();
            GridTerminalSystem.GetBlocksOfType(_airlockDoorsOuter, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("Outer"));
            _airlockVents.Clear();
            GridTerminalSystem.GetBlocksOfType(_airlockVents, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("Airlock"));
            _deconLights.Clear();
            GridTerminalSystem.GetBlocksOfType(_deconLights, b => b.CubeGrid == Me.CubeGrid && b.CustomName.Contains("Decon"));
        }

        T GetBlockOnGrid<T>(string name) where T : class
        {
            IMyTerminalBlock block = GridTerminalSystem.GetBlockWithName(name);
            if (block == null || block.CubeGrid != Me.CubeGrid) return null;
            return block as T;
        }

        void RenderKinematicsEcho()
        {
            _sbEcho.Clear();
            _sbEcho.Append("MASTODON-01 · EXCAVATOR KINEMATICS\n");
            _sbEcho.Append("State: ").Append(StateNames[_currentState]).Append('\n');
            _sbEcho.Append("Macro: ").Append(MacroNames[_activeMacro]).Append('\n');
            _sbEcho.Append("Tool Mode: ").Append(ToolModeNames[_currentToolMode]).Append('\n');
            _sbEcho.Append("Hitch: ").Append(_hitchClampsEngaged ? "LOCKED (HIGHWAY)" : "FLEX (3-AXIS)").Append('\n');
            _sbEcho.Append("DPU Push: ").Append(_dpuPushActive ? "ACTIVE (HILL CLIMB)" : "COAST").Append('\n');
            _sbEcho.Append("Speed: ");
            SbFormat.AppendDouble(_sbEcho, _vehicleSpeedKmh, 1);
            _sbEcho.Append(" km/h\nEnv: ").Append(EnvModeNames[_currentEnvMode]).Append('\n');
            _sbEcho.Append("Airlock: ").Append(AirlockStateNames[_airlockState]);
            Echo(_sbEcho.ToString());
        }
        #endregion
    }
}

// Train.Caterpillar · Space Engineers 1 MDK2 PB Script
// Target Block: [CAT] PB Train Master
// Grid: Large Grid (42 B Consist · 16 Wheels) · Update: Update1 + Update100
// Subsystem Groups: CAT Train Wheels, CAT Hitches, CAT Holds, CAT Loco Power, CAT Lights
// IGC Channels: TMS.CAT.STATUS (out), TMS.CAT.DISPATCH (in), TMS.CONVOY.SYNC (in)
// Rule C4: Native blocks provide safety; PB provides decision logic.

using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        #region Subsystem Configuration & Telemetry
        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sbEcho = new StringBuilder(512);
        readonly StringBuilder _sbIgc = new StringBuilder(256);
        readonly List<string> _args = new List<string>(8);
        readonly HeartbeatWatchdog _watchdog = new HeartbeatWatchdog(5.0);
        IMyBroadcastListener _dispatchListener;
        int _tickCounter;
        int _telemetryTicks;
        string _powerMode = "MODE 3: CRUISE";
        string _assignmentState = "NONE";
        string _destinationId = "";
        string _destinationGps = "";
        string _assignedOreType = "";
        string _requestId = "";
        bool _dumpActive;
        double _grossMassKg = 368500.0;
        double _payloadOreKg;
        float _speedKmh;
        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1 | UpdateFrequency.Update100;
            LoadConfiguration();
            InitHardware();
            InitNetwork();
            Echo("Train.Caterpillar Initialized.");
        }

        public void Save()
        {
            // State persisted to Storage
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (!string.IsNullOrEmpty(argument))
            {
                HandleCommand(argument);
                return;
            }

            if ((updateSource & (UpdateType.Update1)) != 0)
            {
                UpdateTickFast();
            }

            if ((updateSource & (UpdateType.Update100)) != 0)
            {
                UpdateTickSlow();
            }
        }

        void UpdateTickFast()
        {
            _tickCounter++;
            _watchdog.Tick(1.0 / 60.0);

            // Fast traction loop and speed monitoring
        }

        void UpdateTickSlow()
        {
            _telemetryTicks++;
            ProcessInboundIgc();
            BroadcastStatus();

            _sbEcho.Clear();
            _sbEcho.Append("Train.Caterpillar · ACTIVE\n");
            _sbEcho.Append("Consist: 42 B (16 Wheels)\n");
            _sbEcho.Append("Mode: ");
            _sbEcho.Append(_powerMode);
            _sbEcho.Append("\nGross Mass (t): ");
            SbFormat.AppendDouble(_sbEcho, _grossMassKg * 0.001, 1);
            _sbEcho.Append("\nPayload Ore (t): ");
            SbFormat.AppendDouble(_sbEcho, _payloadOreKg * 0.001, 1);
            _sbEcho.Append("\nSpeed (km/h): ");
            SbFormat.AppendDouble(_sbEcho, _speedKmh, 1);
            _sbEcho.Append("\nDump Active: ");
            _sbEcho.Append(_dumpActive ? "TRUE" : "FALSE");
            _sbEcho.Append("\nAssignment: ");
            _sbEcho.Append(_assignmentState);
            if (_assignmentState == "ASSIGNED")
            {
                _sbEcho.Append(" · ");
                _sbEcho.Append(_destinationId);
                _sbEcho.Append(" · ");
                _sbEcho.Append(_destinationGps);
                _sbEcho.Append(" · ");
                _sbEcho.Append(_assignedOreType);
                _sbEcho.Append(" · ");
                _sbEcho.Append(_requestId);
            }
            Echo(_sbEcho.ToString());
        }

        void HandleCommand(string argument)
        {
            IgcProtocol.Split(argument, _args);
            if (_args.Count == 0) return;

            string cmd = _args[0];
            if (string.Compare(cmd, "PING", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _watchdog.Ping();
            }
            else if (string.Compare(cmd, "MODE_CRUISE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _powerMode = "MODE 3: CRUISE";
            }
            else if (string.Compare(cmd, "MODE_HILL_CLIMB", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _powerMode = "MODE 4: HILL_CLIMB";
            }
            else if (string.Compare(cmd, "DUMP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _dumpActive = !_dumpActive;
            }
            else if (string.Compare(cmd, "RELOAD", StringComparison.OrdinalIgnoreCase) == 0)
            {
                LoadConfiguration();
                InitHardware();
            }
        }

        void BroadcastStatus()
        {
            _sbIgc.Clear();
            _sbIgc.Append("CAT-01;");
            SbFormat.AppendDouble(_sbIgc, _grossMassKg * 0.001, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _payloadOreKg * 0.001, 1);
            _sbIgc.Append(";");
            SbFormat.AppendDouble(_sbIgc, _speedKmh, 1);
            _sbIgc.Append(";");
            _sbIgc.Append(_dumpActive ? "DUMP_ACTIVE" : "EN_ROUTE");

            IGC.SendBroadcastMessage("TMS.CAT.STATUS", _sbIgc.ToString());
        }

        void LoadConfiguration()
        {
            _ini.Clear();
            MyIniParseResult result;
            if (_ini.TryParse(Me.CustomData, out result))
            {
                // Parse CustomData
            }
        }

        void InitNetwork()
        {
            _dispatchListener = IGC.RegisterBroadcastListener(TmsProtocol.CatDispatchTag);
        }

        void ProcessInboundIgc()
        {
            if (_dispatchListener == null) return;
            while (_dispatchListener.HasPendingMessage)
            {
                MyIGCMessage msg = _dispatchListener.AcceptMessage();
                HandleIgcPacket(msg.Tag, msg.Data as string);
            }
        }

        void HandleIgcPacket(string tag, string payload)
        {
            if (tag != TmsProtocol.CatDispatchTag || payload == null) return;

            string catId;
            string destinationId;
            string destinationGps;
            string oreType;
            string requestId;
            if (!TmsProtocol.TryReadCatRouteDispatch(payload, _args, out catId, out destinationId,
                out destinationGps, out oreType, out requestId))
                return;
            if (catId != "CAT-01") return;

            _destinationId = destinationId;
            _destinationGps = destinationGps;
            _assignedOreType = oreType;
            _requestId = requestId;
            _assignmentState = "ASSIGNED";
            _watchdog.Ping();
        }

        void InitHardware()
        {
            // Cache terminal blocks with strict grid scoping:
            // b => b.CubeGrid == Me.CubeGrid
        }
    }
}

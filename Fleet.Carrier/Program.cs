using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace IngameScript
{
    // Carrier bay beacon (slice 2 spec §3): every Update10, broadcasts each tagged connector's pose and
    // velocity so drones can undock from, return to and dock onto a MOVING carrier. Flying the carrier is
    // not this script's job (player or vanilla autopilot).
    public partial class Program : MyGridProgram
    {
        string _channel = "FM", _tag = "[FM]", _configText, _bayTag;
        readonly List<IMyShipConnector> _bays = new List<IMyShipConnector>();
        readonly List<IMyShipController> _ctrls = new List<IMyShipController>();
        readonly List<IMyTerminalBlock> _blocks = new List<IMyTerminalBlock>();
        readonly List<Vector3D> _lastPos = new List<Vector3D>();
        readonly Displays _displays = new Displays();
        readonly MyIni _ini = new MyIni();
        readonly StringBuilder _sb = new StringBuilder(1024);
        IMyShipController _ctrl;
        double _time, _lastBeaconTime = -1, _speed;
        int _update100s, _sent;

        public Program()
        {
            LoadConfig();
            Scan();
            Me.GetSurface(0).ContentType = ContentType.TEXT_AND_IMAGE;
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
        }

        void LoadConfig()
        {
            MyIniParseResult res;
            _ini.Clear();
            if (_ini.TryParse(Me.CustomData, out res))
            {
                var ch = _ini.Get("Carrier", "Channel").ToString(_channel).Trim();
                if (ch.Length > 0 && ch.IndexOf('/') < 0) _channel = ch;
                var tag = _ini.Get("Carrier", "Tag").ToString(_tag).Trim();
                if (tag.Length > 0) _tag = tag;
                if (!_ini.ContainsKey("Carrier", "Channel"))
                {
                    _ini.Set("Carrier", "Channel", _channel);
                    _ini.Set("Carrier", "Tag", _tag);
                    Me.CustomData = _ini.ToString();
                }
            }
            _configText = Me.CustomData;
            _bayTag = FleetLink.Tag(_channel, Beacon.Kind);
        }

        void Scan()
        {
            var grid = Me.CubeGrid;
            GridTerminalSystem.GetBlocksOfType(_bays, b => b.CubeGrid == grid && b.CustomName.Contains(_tag));
            GridTerminalSystem.GetBlocksOfType(_ctrls, b => b.CubeGrid == grid);
            GridTerminalSystem.GetBlocksOfType(_blocks, b => b.CubeGrid == grid);
            _ctrl = _ctrls.Count > 0 ? _ctrls[0] : null;
            _displays.Refresh(_blocks, _tag);
            _lastPos.Clear();
            for (int i = 0; i < _bays.Count; i++) _lastPos.Add(_bays[i].GetPosition());
            _lastBeaconTime = -1;
        }

        public void Main(string argument, UpdateType src)
        {
            _time += Runtime.TimeSinceLastRun.TotalSeconds;
            if ((src & (UpdateType.Terminal | UpdateType.Trigger | UpdateType.Script)) != 0
                && argument.Trim().Equals("RESCAN", StringComparison.OrdinalIgnoreCase))
                Scan();
            if ((src & UpdateType.Update10) != 0) Broadcast();
            if ((src & UpdateType.Update100) != 0)
            {
                if (Me.CustomData != _configText) { LoadConfig(); Scan(); }
                else if (++_update100s % 10 == 0) Scan();
                Render();
            }
        }

        void Broadcast()
        {
            double dt = _lastBeaconTime < 0 ? 0 : _time - _lastBeaconTime;
            _lastBeaconTime = _time;
            MyShipVelocities v = new MyShipVelocities();
            if (_ctrl != null) v = _ctrl.GetShipVelocities();
            _speed = 0;
            for (int i = 0; i < _bays.Count; i++)
            {
                var bay = _bays[i];
                if (bay.Closed) continue;
                var m = bay.WorldMatrix;
                var p = new BayPose { BayId = bay.EntityId, Position = m.Translation, Forward = m.Forward, Up = m.Up };
                if (_ctrl != null)
                {
                    p.Velocity = Beacon.PointVelocity(v.LinearVelocity, v.AngularVelocity, _ctrl.CenterOfMass, m.Translation);
                    p.AngularVelocity = v.AngularVelocity;
                }
                else if (dt > 1e-3)
                    p.Velocity = (m.Translation - _lastPos[i]) / dt;   // no controller: estimate, no rotation
                _lastPos[i] = m.Translation;
                _speed = Math.Max(_speed, p.Velocity.Length());
                IGC.SendBroadcastMessage(_bayTag, Beacon.Pack(ref p));
                _sent++;
            }
        }

        void Render()
        {
            var sb = _sb;
            sb.Clear();
            sb.Append("FLEET CARRIER  ch ").Append(_channel).Append("  bays ");
            SbFormat.AppendInt(sb, _bays.Count);
            SbFormat.AppendFixed(sb.Append("  "), _speed, 1).Append(" m/s\n");
            sb.Append(_ctrl != null ? "velocity: controller\n" : "velocity: estimated (add a cockpit for rotation)\n");
            if (_bays.Count == 0) sb.Append("!! no connectors named with ").Append(_tag).Append('\n');
            for (int i = 0; i < _bays.Count; i++)
            {
                var b = _bays[i];
                sb.Append(b.CustomName).Append(": ");
                if (b.Status == MyShipConnectorStatus.Connected && b.OtherConnector != null) sb.Append(b.OtherConnector.CubeGrid.CustomName);
                else if (b.Status == MyShipConnectorStatus.Connectable) sb.Append("docking...");
                else sb.Append("free");
                sb.Append('\n');
            }
            SbFormat.AppendInt(sb.Append("beacons sent "), _sent).Append('\n');
            _displays.Write(sb);
            Me.GetSurface(0).WriteText(sb);
        }
    }
}

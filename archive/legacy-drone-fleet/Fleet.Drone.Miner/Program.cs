using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        readonly Rig _r = new Rig();
        readonly List<ISubsystem> _subs = new List<ISubsystem>();
        readonly MyIni _ini = new MyIni();
        readonly List<string> _dropped = new List<string>();
        readonly Profiler _profiler = new Profiler(60);
        readonly Kernel _kernel;
        readonly MinerSubsystem _miner;
        readonly UiSubsystem _ui;
        readonly RemoteSubsystem _remote;
        int _update100s;

        public Program()
        {
            HoldControls();   // C30 §1.1: before anything else, nothing may keep flying after a reload

            _r.Gts = GridTerminalSystem;
            _r.Me = Me;
            _r.Runtime = Runtime;
            _r.Profiler = _profiler;
            _r.RunCommand = RunCommand;
            _r.Note = Note;

            ConfigLoader.Load(Me.CustomData, _r.Settings, _r.ConfigWarnings);
            if (Me.CustomData.IndexOf("Name=", StringComparison.Ordinal) < 0 && !string.IsNullOrWhiteSpace(Me.CubeGrid.CustomName))
                _r.Settings.Name = Me.CubeGrid.CustomName.Trim().Replace(' ', '-');   // unique, SEND-addressable
            var full = ConfigLoader.Write(Me.CustomData, _r.Settings);
            if (full != Me.CustomData && _r.ConfigWarnings.Count == 0) Me.CustomData = full;

            _r.Rescan();
            var sense = new SenseSubsystem(_r);
            _miner = new MinerSubsystem(_r);
            _ui = new UiSubsystem(_r, _miner);
            _remote = new RemoteSubsystem(_r, _miner, _ui, IGC);
            _ui.OnConfigReloaded = _remote.Retag;
            // Remote before Miner: beacons and remote commands are applied in the same tick they arrive.
            _subs.Add(sense); _subs.Add(_remote); _subs.Add(_miner); _subs.Add(_ui);

            StorageStore.Load(Storage, _subs, _ini, _dropped);
            for (int i = 0; i < _dropped.Count; i++) Note("storage dropped: " + _dropped[i]);

            _kernel = new Kernel(_subs, _profiler, () => Runtime.CurrentInstructionCount);
            _kernel.OnSafe = () => { _r.Ship.ReleaseControls(); _r.DrillsOn(false); };
            _r.Kernel = _kernel;
            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;
        }

        void HoldControls()
        {
            var grid = Me.CubeGrid;
            var thr = new List<IMyThrust>();
            GridTerminalSystem.GetBlocksOfType(thr, b => b.CubeGrid == grid);
            foreach (var t in thr) t.ThrustOverridePercentage = 0;
            var gyros = new List<IMyGyro>();
            GridTerminalSystem.GetBlocksOfType(gyros, b => b.CubeGrid == grid);
            foreach (var g in gyros) { g.GyroOverride = false; g.Pitch = 0; g.Yaw = 0; g.Roll = 0; }
            var drills = new List<IMyShipDrill>();
            GridTerminalSystem.GetBlocksOfType(drills, b => b.CubeGrid == grid);
            foreach (var d in drills) d.Enabled = false;
            var ctrls = new List<IMyShipController>();
            GridTerminalSystem.GetBlocksOfType(ctrls, b => b.CubeGrid == grid);
            foreach (var c in ctrls) c.DampenersOverride = true;
        }

        public void Save()
        {
            Storage = StorageStore.Save(_subs, _ini);
        }

        public void Main(string argument, UpdateType updateSource)
        {
            _r.Bb.Time += Runtime.TimeSinceLastRun.TotalSeconds;
            if ((updateSource & (UpdateType.Terminal | UpdateType.Trigger | UpdateType.Script | UpdateType.Mod)) != 0
                && !string.IsNullOrWhiteSpace(argument))
                RunCommand(argument);

            if ((updateSource & UpdateType.IGC) != 0) _remote.PollBays();   // stamp beacons on arrival

            bool slow = (updateSource & UpdateType.Update100) != 0;
            if (slow && ++_update100s % 10 == 0 && !_kernel.IsSafe) _r.Rescan();

            _kernel.Tick(updateSource, Runtime.LastRunTimeMs);

            // The kernel runs no subsystem while SAFE; keep the LCD, the status broadcast and the remote
            // inbox alive so the cockpit and the console both see why, and the console can RESET.
            if (_kernel.IsSafe)
            {
                try
                {
                    if ((updateSource & UpdateType.Update10) != 0) _remote.Poll();
                    if (slow) { _ui.Render(); _remote.Broadcast(); }
                }
                catch (Exception) { }
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100
                | (_miner.WantsUpdate1 && !_kernel.IsSafe ? UpdateFrequency.Update1 : UpdateFrequency.None);
        }

        // The single entry point for terminal arguments, menu commands and remote commands.
        void RunCommand(string text)
        {
            try
            {
                if (text.TrimStart().StartsWith("SET ", StringComparison.OrdinalIgnoreCase))
                {
                    int field; double value; string error;
                    if (FleetLink.TryParseSet(text, out field, out value, out error)) _ui.ApplySetting(field, value);
                    else Note(error);
                    return;
                }
                string rest;
                var cmd = CommandParser.Parse(text, out rest);
                if (_kernel.IsSafe && cmd != Cmd.Reset && cmd != Cmd.Up && cmd != Cmd.Down && cmd != Cmd.Apply && cmd != Cmd.Back)
                {
                    Note("SAFE: only RESET is accepted");
                    return;
                }
                switch (cmd)
                {
                    case Cmd.None: return;
                    case Cmd.Up:
                    case Cmd.Down:
                    case Cmd.Apply:
                    case Cmd.Back:
                        _ui.HandleMenu(cmd); return;
                    case Cmd.Unknown:
                        Note("unknown command: " + text.Trim()); return;
                    case Cmd.Reset:
                        _kernel.ResetSafe();
                        _miner.Command(Cmd.Reset, rest);
                        return;
                    default:
                        _miner.Command(cmd, rest); return;
                }
            }
            catch (Exception e)
            {
                _kernel.EnterSafe("command " + text.Trim() + ": " + e.Message);
            }
        }

        void Note(string text)
        {
            _r.LastNote = text ?? "";
            _r.LastNoteTime = _r.Bb.Time;
        }
    }
}

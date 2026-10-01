using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        // Block discovery + setup diagnostics (C30 §3). Grid-scoped; rescanned at construction, on
        // REBOOT and every 10th Update100. Allocates only on its first scan (lists are reused).
        public class BlockScan
        {
            public IMyShipController Controller;
            public IMyShipConnector Connector;
            public readonly List<IMyShipConnector> Ejectors = new List<IMyShipConnector>();
            public readonly List<IMyGyro> Gyros = new List<IMyGyro>();
            public readonly List<IMyThrust> Thrusters = new List<IMyThrust>();
            public readonly List<IMyShipDrill> Drills = new List<IMyShipDrill>();
            public readonly List<IMyBatteryBlock> Batteries = new List<IMyBatteryBlock>();
            public readonly List<IMyGasTank> Tanks = new List<IMyGasTank>();
            public readonly List<IMyReactor> Reactors = new List<IMyReactor>();
            public readonly List<IMyTerminalBlock> Inventories = new List<IMyTerminalBlock>();
            public readonly List<IMyTerminalBlock> All = new List<IMyTerminalBlock>();
            public readonly List<Vector3D> DrillLocal = new List<Vector3D>();
            public readonly List<string> Diagnostics = new List<string>();
            // Connector and drill-face poses in the CONTROLLER BLOCK's frame. The helm works in a centre-of-mass
            // frame, and the CoM moves as ore loads, so these are converted per tick (MinerSubsystem.CurrentRef).
            public MatrixD RefConnInCtrl = MatrixD.Identity, RefDrillInCtrl = MatrixD.Identity;
            public bool HasAntenna;
            public double ShipSize = 5;
            public bool Ready { get { return Diagnostics.Count == 0; } }

            readonly List<IMyShipController> _ctrls = new List<IMyShipController>();
            VRage.Game.ModAPI.Ingame.IMyCubeGrid _grid;
            readonly Func<IMyTerminalBlock, bool> _mine, _pilotable;   // cached: rescans run in Update100

            public BlockScan()
            {
                _mine = b => b.CubeGrid == _grid;
                _pilotable = b => b.CubeGrid == _grid && ((IMyShipController)b).CanControlShip;
            }
            readonly List<IMyShipConnector> _conns = new List<IMyShipConnector>();

            public const string NoController = "No cockpit or remote control", NoConnector = "No connector",
                NoGyros = "No gyros", NoThrusters = "No thrusters", NoDrills = "No drills", NoPower = "No power source";

            public void Scan(IMyGridTerminalSystem gts, IMyProgrammableBlock me, string tag)
            {
                var grid = me.CubeGrid;
                _grid = grid;
                gts.GetBlocksOfType(All, _mine);
                gts.GetBlocksOfType(_ctrls, _pilotable);
                gts.GetBlocksOfType(_conns, _mine);
                gts.GetBlocksOfType(Gyros, _mine);
                gts.GetBlocksOfType(Thrusters, _mine);
                gts.GetBlocksOfType(Drills, _mine);
                gts.GetBlocksOfType(Batteries, _mine);
                gts.GetBlocksOfType(Tanks, _mine);
                gts.GetBlocksOfType(Reactors, _mine);
                Inventories.Clear();
                HasAntenna = false;
                for (int i = 0; i < All.Count; i++)
                {
                    if (All[i].HasInventory) Inventories.Add(All[i]);
                    if (All[i] is IMyRadioAntenna || All[i] is IMyLaserAntenna) HasAntenna = true;
                }

                Controller = null;
                for (int i = 0; i < _ctrls.Count && Controller == null; i++)
                    if (_ctrls[i].CustomName.Contains(tag)) Controller = _ctrls[i];
                if (Controller == null && _ctrls.Count > 0) Controller = _ctrls[0];

                Connector = null;
                Ejectors.Clear();
                for (int i = 0; i < _conns.Count; i++)
                {
                    var c = _conns[i];
                    bool eject = c.CustomName.Contains("Eject");
                    if (eject) { if (c.CustomName.Contains(tag)) Ejectors.Add(c); continue; }
                    if (Connector == null || (c.CustomName.Contains(tag) && !Connector.CustomName.Contains(tag))) Connector = c;
                }

                Diagnostics.Clear();
                if (Controller == null) Diagnostics.Add(NoController);
                if (Connector == null) Diagnostics.Add(NoConnector);
                if (Gyros.Count == 0) Diagnostics.Add(NoGyros);
                if (Thrusters.Count == 0) Diagnostics.Add(NoThrusters);
                if (Drills.Count == 0) Diagnostics.Add(NoDrills);
                if (Batteries.Count == 0 && Tanks.Count == 0 && Reactors.Count == 0) Diagnostics.Add(NoPower);

                Derive(grid);
            }

            void Derive(VRage.Game.ModAPI.Ingame.IMyCubeGrid grid)
            {
                var ext = (Vector3D)(grid.Max - grid.Min + Vector3I.One) * grid.GridSize;
                ShipSize = Math.Max(1, ext.Length() / 2);
                if (Controller == null) return;
                var inv = MatrixD.Invert(Controller.WorldMatrix);
                if (Connector != null) RefConnInCtrl = Connector.WorldMatrix * inv;

                DrillLocal.Clear();
                if (Drills.Count == 0) return;
                var mean = Vector3D.Zero;
                for (int i = 0; i < Drills.Count; i++)
                {
                    var p = Vector3D.Transform(Drills[i].GetPosition(), inv);
                    DrillLocal.Add(p);
                    mean += p;
                }
                mean /= Drills.Count;
                // Drill face: mean drill position pushed 1.5 m along controller-forward, controller orientation.
                RefDrillInCtrl = MatrixD.Identity;
                RefDrillInCtrl.Translation = mean + Vector3D.Forward * 1.5;
            }
        }

        // The objects every drone subsystem shares, built once by Program.
        public class Rig
        {
            public readonly Blackboard Bb = new Blackboard();
            public readonly Settings Settings = new Settings();
            public readonly BlockScan Scan = new BlockScan();
            public readonly ShipIO Ship = new ShipIO();
            public readonly CargoIO Cargo = new CargoIO();
            public readonly EnergyIO Energy = new EnergyIO();
            public readonly DamageScanner Damage = new DamageScanner();
            public readonly Displays Displays = new Displays();
            public readonly EventLog Events = new EventLog(8);
            public readonly List<string> ConfigWarnings = new List<string>();
            public IMyGridTerminalSystem Gts;
            public IMyProgrammableBlock Me;
            public IMyGridProgramRuntimeInfo Runtime;
            public Kernel Kernel;
            public Profiler Profiler;
            public string LastNote = "";           // latest operator-facing note (not a state transition)
            public double LastNoteTime;
            public Action<string> RunCommand;      // the one entry point for terminal, menu and remote commands
            public Action<string> Note;            // a line for the event log / LCD

            public void Rescan()
            {
                Scan.Scan(Gts, Me, Settings.Tag);
                Ship.Refresh(Scan.Controller, Scan.Thrusters, Scan.Gyros);
                Cargo.Refresh(Scan.Inventories, Scan.Ejectors);
                Energy.Refresh(Scan.Batteries, Scan.Tanks, Scan.Reactors);
                Damage.Refresh(Scan.All);
                Displays.Refresh(Scan.All, Settings.Tag);
                Bb.ShipSize = Scan.ShipSize;
            }

            public void DrillsOn(bool on)
            {
                for (int i = 0; i < Scan.Drills.Count; i++) Scan.Drills[i].Enabled = on;
            }
        }
    }
}

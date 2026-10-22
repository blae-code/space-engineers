using System;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        // Fills the Blackboard from the ship every Update10 (C30 §5). No saved state, no allocation.
        public class SenseSubsystem : ISubsystem
        {
            readonly Rig _r;
            readonly double[] _max = new double[6];

            public SenseSubsystem(Rig r) { _r = r; }

            public string Name { get { return "Sense"; } }
            public int StorageVersion { get { return 1; } }

            public void Update1() { }
            public void Update100() { }
            public void HandleMessage(MyIGCMessage msg) { }
            public void Save(MyIni ini) { }
            public bool Load(MyIni ini, int savedVersion) { return true; }
            public void Status(StringBuilder sb) { }

            public void Update10()
            {
                var bb = _r.Bb;
                var ship = _r.Ship;
                if (_r.Scan.Controller == null) return;
                bb.ShipMatrix = ship.WorldMatrix;
                bb.Velocity = ship.LinearVelocity;
                bb.AngularVelocity = ship.AngularVelocity;
                bb.Gravity = ship.Gravity;
                double g = bb.Gravity.Length();
                bb.InGravity = g > 0.05;
                bb.Mass = ship.Mass;
                double elev;
                bb.HasElevation = ship.TryGetElevation(out elev);
                bb.Elevation = elev;

                bb.CargoFill = _r.Cargo.Fill();
                bb.HasBattery = _r.Energy.HasBattery;
                bb.HasHydrogen = _r.Energy.HasHydrogen;
                bb.HasReactor = _r.Energy.HasReactor;
                bb.BatteryFill = _r.Energy.BatteryFill();
                bb.HydrogenFill = _r.Energy.HydrogenFill();
                bb.UraniumKg = _r.Energy.UraniumKg();

                if (bb.InGravity && bb.Mass > 0)
                {
                    ship.GetMaxThrust(_max);
                    var upLocal = Vector3D.TransformNormal(-bb.Gravity / g, MatrixD.Transpose(bb.ShipMatrix));
                    bb.LiftMargin = ThrustAllocator.MaxForceAlong(upLocal, _max) / (bb.Mass * g);
                }
                else bb.LiftMargin = double.PositiveInfinity;

                _r.Damage.Step();
                bb.Damaged = _r.Damage.Damaged;

                var c = _r.Scan.Connector;
                bb.Connected = c != null && c.Status == MyShipConnectorStatus.Connected;
                bb.Connectable = c != null && c.Status == MyShipConnectorStatus.Connectable;
                bb.SensingTicks++;
            }
        }
    }
}

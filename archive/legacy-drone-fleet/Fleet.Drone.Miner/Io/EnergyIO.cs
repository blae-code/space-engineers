using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program
    {
        public class EnergyIO
        {
            public const double ChargedRatio = 0.95;
            const string UraniumSubtype = "Uranium";

            readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>();
            readonly List<IMyGasTank> _tanks = new List<IMyGasTank>();
            readonly List<IMyReactor> _reactors = new List<IMyReactor>();
            readonly List<MyInventoryItem> _items = new List<MyInventoryItem>();

            public EnergyIO() { }

            // keeps only hydrogen tanks
            public void Refresh(List<IMyBatteryBlock> batteries, List<IMyGasTank> tanks, List<IMyReactor> reactors)
            {
                _batteries.Clear();
                _tanks.Clear();
                _reactors.Clear();
                for (int i = 0; i < batteries.Count; i++) _batteries.Add(batteries[i]);
                for (int i = 0; i < tanks.Count; i++)
                {
                    if (IsHydrogenTank(tanks[i].BlockDefinition.SubtypeId)) _tanks.Add(tanks[i]);
                }
                for (int i = 0; i < reactors.Count; i++) _reactors.Add(reactors[i]);
            }

            public bool HasBattery { get { return _batteries.Count > 0; } }
            public bool HasHydrogen { get { return _tanks.Count > 0; } }
            public bool HasReactor { get { return _reactors.Count > 0; } }

            // stored / max over all batteries, 0..1 (1 if none)
            public double BatteryFill()
            {
                double stored = 0, max = 0;
                for (int i = 0; i < _batteries.Count; i++)
                {
                    stored += _batteries[i].CurrentStoredPower;
                    max += _batteries[i].MaxStoredPower;
                }
                if (max <= 0) return 1;
                double f = stored / max;
                if (f < 0) return 0;
                if (f > 1) return 1;
                return f;
            }

            // mean FilledRatio of hydrogen tanks (1 if none)
            public double HydrogenFill()
            {
                if (_tanks.Count == 0) return 1;
                double sum = 0;
                for (int i = 0; i < _tanks.Count; i++) sum += _tanks[i].FilledRatio;
                return sum / _tanks.Count;
            }

            // total uranium in all reactors (0 if none)
            public double UraniumKg()
            {
                long raw = 0;
                for (int r = 0; r < _reactors.Count; r++)
                {
                    IMyInventory inv = _reactors[r].GetInventory(0);
                    _items.Clear();
                    inv.GetItems(_items);
                    for (int j = 0; j < _items.Count; j++)
                    {
                        if (_items[j].Type.SubtypeId == UraniumSubtype) raw += _items[j].Amount.RawValue;
                    }
                }
                return raw / 1000000.0;
            }

            // batteries Recharge/Auto, hydrogen tanks Stockpile on/off
            public void SetCharging(bool on)
            {
                ChargeMode mode = on ? ChargeMode.Recharge : ChargeMode.Auto;
                for (int i = 0; i < _batteries.Count; i++) _batteries[i].ChargeMode = mode;
                for (int i = 0; i < _tanks.Count; i++) _tanks[i].Stockpile = on;
            }

            // battery (if any) and hydrogen (if any) >= ChargedRatio
            public bool IsCharged()
            {
                if (HasBattery && BatteryFill() < ChargedRatio) return false;
                if (HasHydrogen && HydrogenFill() < ChargedRatio) return false;
                return true;
            }

            public static bool IsHydrogenTank(string subtypeId)
            {
                return subtypeId != null && subtypeId.IndexOf("Hydrogen", StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }
    }
}

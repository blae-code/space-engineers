using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program
    {
        public class CargoIO
        {
            public const int TransfersPerCall = 4;
            const string OreType = "MyObjectBuilder_Ore";

            readonly List<IMyInventory> _all = new List<IMyInventory>();      // every own inventory (fill, HasOre)
            readonly List<IMyInventory> _sources = new List<IMyInventory>();  // own inventories minus ejectors
            readonly List<IMyInventory> _ejectors = new List<IMyInventory>(); // ejector connector inventories
            readonly List<MyInventoryItem> _items = new List<MyInventoryItem>();
            int _ejectCursor;
            int _unloadCursor;

            public CargoIO() { }

            // own: the drone's inventory blocks (drills, containers, connectors). ejectors: drone connectors tagged
            // with the fleet tag and "Eject" in the name. Sets ThrowOut = true on ejectors.
            public void Refresh(List<IMyTerminalBlock> own, List<IMyShipConnector> ejectors)
            {
                _all.Clear();
                _sources.Clear();
                _ejectors.Clear();
                _ejectCursor = 0;
                _unloadCursor = 0;

                for (int i = 0; i < ejectors.Count; i++)
                {
                    IMyShipConnector c = ejectors[i];
                    c.ThrowOut = true;
                    for (int k = 0; k < c.InventoryCount; k++)
                        _ejectors.Add(c.GetInventory(k));
                }

                for (int i = 0; i < own.Count; i++)
                {
                    IMyTerminalBlock b = own[i];
                    if (!b.HasInventory) continue;
                    bool isEjector = false;
                    for (int e = 0; e < ejectors.Count; e++)
                    {
                        if (ReferenceEquals(ejectors[e], b)) { isEjector = true; break; }
                    }
                    for (int k = 0; k < b.InventoryCount; k++)
                    {
                        IMyInventory inv = b.GetInventory(k);
                        _all.Add(inv);
                        if (!isEjector) _sources.Add(inv);
                    }
                }
            }

            public double Fill()
            {
                long cur = 0, max = 0;
                for (int i = 0; i < _all.Count; i++)
                {
                    cur += _all[i].CurrentVolume.RawValue;
                    max += _all[i].MaxVolume.RawValue;
                }
                return FillFromRaw(cur, max);
            }

            public bool HasOre()
            {
                for (int i = 0; i < _all.Count; i++)
                {
                    _items.Clear();
                    _all[i].GetItems(_items);
                    for (int j = 0; j < _items.Count; j++)
                    {
                        if (_items[j].Type.TypeId == OreType) return true;
                    }
                }
                return false;
            }

            // Moves up to TransfersPerCall stacks whose ore subtype is in s.Eject into ejectors. Returns transfers made.
            public int EjectStep(Settings s)
            {
                int n = _sources.Count;
                if (n == 0 || _ejectors.Count == 0) return 0;
                if (_ejectCursor >= n) _ejectCursor = 0;

                int moved = 0;
                for (int visited = 0; visited < n; visited++)
                {
                    IMyInventory src = _sources[_ejectCursor];
                    _items.Clear();
                    src.GetItems(_items);
                    for (int j = 0; j < _items.Count; j++)
                    {
                        MyInventoryItem it = _items[j];
                        if (!ShouldEject(it.Type.TypeId, it.Type.SubtypeId, s.Eject)) continue;
                        for (int t = 0; t < _ejectors.Count; t++)
                        {
                            IMyInventory dst = _ejectors[t];
                            if (!src.CanTransferItemTo(dst, it.Type)) continue;
                            if (src.TransferItemTo(dst, it, null))
                            {
                                moved++;
                                break;
                            }
                        }
                        // stay on this inventory: it may hold more stacks to move next call
                        if (moved >= TransfersPerCall) return moved;
                    }
                    _ejectCursor = (_ejectCursor + 1) % n;
                }
                return moved;
            }

            // Moves up to TransfersPerCall ore stacks into 'targets' (containers on the carrier; tagged ones first).
            public int UnloadStep(List<IMyInventory> targets)
            {
                int n = _sources.Count;
                if (n == 0 || targets == null || targets.Count == 0) return 0;
                if (_unloadCursor >= n) _unloadCursor = 0;

                int moved = 0;
                for (int visited = 0; visited < n; visited++)
                {
                    IMyInventory src = _sources[_unloadCursor];
                    _items.Clear();
                    src.GetItems(_items);
                    for (int j = 0; j < _items.Count; j++)
                    {
                        MyInventoryItem it = _items[j];
                        if (it.Type.TypeId != OreType) continue;
                        for (int t = 0; t < targets.Count; t++)
                        {
                            IMyInventory dst = targets[t];
                            if (!src.CanTransferItemTo(dst, it.Type)) continue;
                            if (src.TransferItemTo(dst, it, null))
                            {
                                moved++;
                                break;
                            }
                        }
                        if (moved >= TransfersPerCall) return moved;
                    }
                    _unloadCursor = (_unloadCursor + 1) % n;
                }
                return moved;
            }

            public static double FillFromRaw(long currentRaw, long maxRaw)
            {
                if (maxRaw <= 0) return 0;
                double f = (double)currentRaw / maxRaw;
                if (f < 0) return 0;
                if (f > 1) return 1;
                return f;
            }

            public static bool ShouldEject(string typeId, string subtypeId, List<string> eject)
            {
                if (typeId != OreType || eject == null) return false;
                for (int i = 0; i < eject.Count; i++)
                {
                    if (string.Equals(eject[i], subtypeId, StringComparison.OrdinalIgnoreCase)) return true;
                }
                return false;
            }
        }
    }
}

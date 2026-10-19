using System;
using System.Text;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program
    {
        public class MiningStats
        {
            public int Trips { get; private set; }
            public double OreKg { get; private set; }
            public double JobSeconds { get; private set; }
            public double KgPerHour { get { return JobSeconds < 60.0 ? 0.0 : OreKg * 3600.0 / JobSeconds; } }
            public bool Any { get { return Trips > 0 || JobSeconds > 0; } }

            double _previousTime;
            double _unloadStartMassKg;
            bool _hasUnloadStart;

            public MiningStats()
            {
                Reset();
            }

            public static bool IsWorking(MinerState s)
            {
                return s != MinerState.Idle && s != MinerState.Hold && s != MinerState.Safe && s != MinerState.Recording;
            }

            public void Reset()
            {
                Trips = 0;
                OreKg = 0.0;
                JobSeconds = 0.0;
                _previousTime = -1.0;
                _unloadStartMassKg = -1.0;
                _hasUnloadStart = false;
            }

            public void Tick(double now, MinerState state)
            {
                if (_previousTime >= 0 && now >= _previousTime && IsWorking(state))
                {
                    JobSeconds += now - _previousTime;
                }
                _previousTime = now;
            }

            public void OnTransition(MinerState from, MinerState to, double shipMassKg)
            {
                if (from != MinerState.Unload && to == MinerState.Unload)
                {
                    // Entering Unload
                    _unloadStartMassKg = shipMassKg;
                    _hasUnloadStart = true;
                }
                else if (from == MinerState.Unload && to != MinerState.Unload)
                {
                    // Leaving Unload
                    if (_hasUnloadStart)
                    {
                        Trips++;
                        double massDrop = Math.Max(0.0, _unloadStartMassKg - shipMassKg);
                        OreKg += massDrop;
                    }
                    _hasUnloadStart = false;
                }
            }

            public void Save(MyIni ini, string section)
            {
                ini.Set(section, "trips", Trips);
                ini.Set(section, "oreKg", OreKg);
                ini.Set(section, "jobSeconds", JobSeconds);
            }

            public void Load(MyIni ini, string section)
            {
                Reset();
                Trips = Math.Max(0, ini.Get(section, "trips").ToInt32(0));
                OreKg = Math.Max(0.0, ini.Get(section, "oreKg").ToDouble(0));
                JobSeconds = Math.Max(0.0, ini.Get(section, "jobSeconds").ToDouble(0));
            }

            public StringBuilder Append(StringBuilder sb)
            {
                sb.Append("trips ");
                SbFormat.AppendInt(sb, Trips);
                sb.Append("  ore ");
                SbFormat.AppendInt(sb, (long)Math.Round(OreKg));
                sb.Append("kg  ");
                if (JobSeconds < 60.0)
                {
                    sb.Append("--kg/h  ");
                }
                else
                {
                    SbFormat.AppendInt(sb, (long)Math.Round(KgPerHour));
                    sb.Append("kg/h  ");
                }
                SbFormat.AppendFixed(sb, JobSeconds / 3600.0, 1);
                sb.Append("h");
                return sb;
            }
        }
    }
}

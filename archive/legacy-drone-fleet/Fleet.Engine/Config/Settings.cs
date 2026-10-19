using System.Collections.Generic;

namespace IngameScript
{
    public partial class Program
    {
        public class Settings
        {
            // [Fleet]
            public string Name = "Miner-01";
            public string Tag = "[FM]";
            public string Channel = "FM";   // IGC channel shared with the remote console (spec §10.2)
            // [Miner]
            public int Width = 5;
            public int Height = 5;
            public double Depth = 30;
            public double StartDepth = 0;
            public double WorkSpeed = 1.5;
            public double RetractSpeed = 4;
            public double MaxLoad = 90;
            public double MinLiftMargin = 1.3;
            public readonly List<string> Eject = new List<string> { "Stone" };
            public bool Loop = true;
            public DamagePolicy OnDamage = DamagePolicy.Home;
            // [Flight]
            public double MaxSpeed = 60;
            public double ApproachSpeed = 5;
            public double DockSpeed = 0.5;
            public double SafeAltitude = 150;
            public double StuckSeconds = 5;
            // [Energy]
            public double MinBattery = 20;
            public double MinHydrogen = 30;
            public double MinUranium = 5;   // kg
            // [Reload]
            public ReloadPolicy OnReload = ReloadPolicy.Resume;
        }
    }
}

using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program
    {
        public interface ISubsystem
        {
            string Name { get; }            // also its Storage section name
            int StorageVersion { get; }
            void Update1();                 // only called while a subsystem requested Update1
            void Update10();
            void Update100();
            void HandleMessage(MyIGCMessage msg);   // unused in slice 1
            void Save(MyIni ini);                   // write own keys into section Name
            bool Load(MyIni ini, int savedVersion); // false = incompatible -> section dropped and reported
            void Status(StringBuilder sb);          // append LCD status lines
        }
    }
}

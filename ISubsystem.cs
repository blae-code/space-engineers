namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// Contract for every module. Only the kernel (Program) calls these methods.
        /// Implementations must not allocate inside Update10/Update100/HandleMessage.
        /// </summary>
        public interface ISubsystem
        {
            /// <summary>Called once from Program(). Allocation is allowed here.</summary>
            void Initialize();

            /// <summary>High-frequency math/physics (every 10 ticks).</summary>
            void Update10();

            /// <summary>Low-frequency network/UI work (every 100 ticks).</summary>
            void Update100();

            /// <summary>
            /// Receives every decoded fleet message. The instance is reused by CommsOfficer,
            /// so copy out any fields you need to keep.
            /// </summary>
            void HandleMessage(FleetMessage message);
        }
    }
}

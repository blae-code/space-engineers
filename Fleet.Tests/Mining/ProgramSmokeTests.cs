using System;
using System.Runtime.Serialization;
using System.Text;
using FakeItEasy;
using NUnit.Framework;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace Fleet.Tests.Mining
{
    // C30 §7: the real drone Program, constructed and ticked on an EMPTY grid (MDK gateway pattern).
    [TestFixture]
    public class ProgramSmokeTests
    {
        IMyProgrammableBlock _me;
        IMyIntergridCommunicationSystem _igc;
        string _lastScreen = "";
        string _broadcast;

        IMyGridProgramRuntimeInfo _runtime;

        IngameScript.Program Build(string storage = "", System.Action<IMyIntergridCommunicationSystem> igcSetup = null)
        {
            _me = A.Fake<IMyProgrammableBlock>();
            A.CallTo(() => _me.CubeGrid).Returns(A.Fake<IMyCubeGrid>());
            _me.CustomData = "";
            var surface = A.Fake<IMyTextSurface>();
            A.CallTo(() => surface.WriteText(A<StringBuilder>._, A<bool>._))
                .Invokes((StringBuilder sb, bool append) => _lastScreen = sb.ToString());
            A.CallTo(() => _me.GetSurface(0)).Returns(surface);

            _igc = A.Fake<IMyIntergridCommunicationSystem>();
            if (igcSetup != null) igcSetup(_igc);
            A.CallTo(_igc).Where(c => c.Method.Name == "SendBroadcastMessage")
                .Invokes(c => _broadcast = c.Arguments[1] as string);

            var program = FormatterServices.GetUninitializedObject(typeof(IngameScript.Program));
            var backend = (Sandbox.ModAPI.IMyGridProgram)program;
            _runtime = A.Fake<IMyGridProgramRuntimeInfo>();
            backend.Runtime = _runtime;
            backend.Echo = s => { };
            backend.Me = _me;
            backend.Storage = storage;
            backend.GridTerminalSystem = A.Fake<IMyGridTerminalSystem>();
            backend.IGC_ContextGetter = () => _igc;
            typeof(IngameScript.Program).GetConstructor(Type.EmptyTypes).Invoke(program, null);
            return (IngameScript.Program)program;
        }

        [Test]
        public void Constructor_DoesNotThrow_AndCompletesCustomData()
        {
            Build();
            Assert.That(_me.CustomData, Does.Contain("[Miner]"));
            Assert.That(_me.CustomData, Does.Contain("OnDamage=Home"), "enum names must be literal, not identifiers");
            Assert.That(_me.CustomData, Does.Contain("Channel=FM"));
        }

        [Test]
        public void Tick_RendersDiagnostics_NotSafe()
        {
            var p = Build();
            p.Main("", UpdateType.Update10 | UpdateType.Update100);
            Assert.That(_lastScreen, Does.Contain("No cockpit or remote control"));
            Assert.That(_lastScreen, Does.Not.Contain("SAFE"));
        }

        [Test]
        public void Start_OnEmptyGrid_NeverLeavesIdleOrHold()
        {
            var p = Build();
            p.Main("", UpdateType.Update10 | UpdateType.Update100);
            p.Main("START", UpdateType.Terminal);
            p.Main("", UpdateType.Update10 | UpdateType.Update100);
            var first = _lastScreen.Split('\n')[0];
            Assert.That(first, Does.Contain("Idle").Or.Contain("Hold"), first);
        }

        [Test]
        public void StatusBroadcast_IsAValidStatusLine()
        {
            var p = Build();
            p.Main("", UpdateType.Update10 | UpdateType.Update100);
            Assert.That(_broadcast, Is.Not.Null);
            var s = new IngameScript.Program.DroneStatus();
            Assert.That(IngameScript.Program.FleetLink.Decode(_broadcast, ref s), Is.True, _broadcast);
            Assert.That(s.Name, Is.EqualTo("Miner-01"));
            Assert.That(s.Flags & IngameScript.Program.FleetLink.FlagNotReady, Is.Not.Zero);
        }

        [Test]
        public void MenuAndSetCommands_EditCustomData()
        {
            var p = Build();
            p.Main("SET Miner Width 9", UpdateType.Terminal);
            Assert.That(_me.CustomData, Does.Contain("Width=9"));
            p.Main("SET Miner OnDamage stop", UpdateType.Terminal);
            Assert.That(_me.CustomData, Does.Contain("OnDamage=Stop"));
            p.Main("DOWN", UpdateType.Terminal);
            p.Main("DOWN", UpdateType.Terminal);
            p.Main("APPLY", UpdateType.Terminal);                 // Job settings page
            Assert.That(_lastScreen, Does.Contain("> Width           9"));
        }

        // Spec §10 R2 at unit level: the console's SET and GETCFG, delivered by unicast on the channel,
        // change the drone's Custom Data and are answered with the new config on the cfg tag.
        [Test]
        public void RemoteSetAndGetCfg_AnswerWithConfig()
        {
            var p = Build();
            var inbox = new System.Collections.Generic.Queue<MyIGCMessage>();
            var listener = A.Fake<IMyUnicastListener>();
            A.CallTo(() => listener.HasPendingMessage).ReturnsLazily(() => inbox.Count > 0);
            A.CallTo(() => listener.AcceptMessage()).ReturnsLazily(() => inbox.Dequeue());
            A.CallTo(() => _igc.UnicastListener).Returns(listener);
            var replies = new System.Collections.Generic.List<string>();
            A.CallTo(_igc).Where(c => c.Method.Name == "SendUnicastMessage")
                .Invokes(c => replies.Add(c.Arguments[1] + " " + c.Arguments[2]));

            inbox.Enqueue(new MyIGCMessage("SET Miner Depth 45", "FLEET/FM/cmd", 777));
            inbox.Enqueue(new MyIGCMessage("SET Miner Width 99", "FLEET/FM/cmd", 777));
            inbox.Enqueue(new MyIGCMessage("START", "FLEET/OTHER/cmd", 777));   // wrong channel: ignored
            p.Main("", UpdateType.Update10);

            Assert.That(_me.CustomData, Does.Contain("Depth=45"));
            Assert.That(replies[0], Does.StartWith("FLEET/FM/cfg ").And.Contain("Depth=45"));
            Assert.That(replies[1], Does.StartWith("FLEET/FM/ack ERR").And.Contain("out of range"));
            Assert.That(replies.Count, Is.EqualTo(2));
        }

        // A refused remote command must say why, not "OK" (review finding 21).
        [Test]
        public void RemoteRefusedCommand_AcksTheReason()
        {
            var p = Build();
            var inbox = new System.Collections.Generic.Queue<MyIGCMessage>();
            var listener = A.Fake<IMyUnicastListener>();
            A.CallTo(() => listener.HasPendingMessage).ReturnsLazily(() => inbox.Count > 0);
            A.CallTo(() => listener.AcceptMessage()).ReturnsLazily(() => inbox.Dequeue());
            A.CallTo(() => _igc.UnicastListener).Returns(listener);
            string ack = null;
            A.CallTo(_igc).Where(c => c.Method.Name == "SendUnicastMessage").Invokes(c => ack = c.Arguments[2] as string);
            p.Main("", UpdateType.Update10 | UpdateType.Update100);
            inbox.Enqueue(new MyIGCMessage("CONT", "FLEET/FM/cmd", 777));
            p.Main("", UpdateType.Update10);
            Assert.That(ack, Does.StartWith("CONT: not ready").And.Contain("No cockpit"));
        }

        // Slice 2: undocked, the home pose comes from the carrier's bay beacon; silence means LOST.
        [Test]
        public void CarrierBeacon_TrackedThenLost()
        {
            var beacons = new System.Collections.Generic.Queue<MyIGCMessage>();
            var p = Build("[Miner]\nv=3\nhomeId=42\nstate=0\n", igc =>
            {
                var bays = A.Fake<IMyBroadcastListener>();
                A.CallTo(() => bays.HasPendingMessage).ReturnsLazily(() => beacons.Count > 0);
                A.CallTo(() => bays.AcceptMessage()).ReturnsLazily(() => beacons.Dequeue());
                A.CallTo(() => igc.RegisterBroadcastListener("FLEET/FM/bay")).Returns(bays);
            });
            A.CallTo(() => _runtime.TimeSinceLastRun).Returns(System.TimeSpan.FromSeconds(0.5));
            var bay = new IngameScript.Program.BayPose
            {
                BayId = 42, Position = new VRageMath.Vector3D(0, 0, 0), Forward = new VRageMath.Vector3D(0, 0, -1),
                Up = new VRageMath.Vector3D(0, 1, 0), Velocity = new VRageMath.Vector3D(5, 0, 0)
            };
            beacons.Enqueue(new MyIGCMessage(IngameScript.Program.Beacon.Pack(ref bay), "FLEET/FM/bay", 900));
            p.Main("", UpdateType.Update10 | UpdateType.Update100);
            Assert.That(_lastScreen, Does.Contain("carrier tracked  5.0 m/s"));
            for (int i = 0; i < 5; i++) p.Main("", UpdateType.Update10 | UpdateType.Update100);   // 2.5 s of silence
            Assert.That(_lastScreen, Does.Contain("carrier beacon LOST"));
        }

        // Slice 2: version-2 storage kept the job local to the home connector; it is migrated to world space.
        [Test]
        public void StorageV2_JobMigratesToWorld()
        {
            var v2 = "[Miner]\nv=2\nhomeId=42\nhasJob=true\nwidth=1\nheight=1\nspacing=3\n"
                + "ox=0\noy=0\noz=-10\nfx=0\nfy=0\nfz=-1\nux=0\nuy=1\nuz=0\nrx=1\nry=0\nrz=0\n"
                + "hpx=100\nhpy=0\nhpz=0\nhfx=1\nhfy=0\nhfz=0\nhux=0\nhuy=1\nhuz=0\n";
            var p = Build(v2);
            p.Save();
            var v3 = ((Sandbox.ModAPI.IMyGridProgram)p).Storage;
            Assert.That(v3, Does.Contain("v=3"));
            // Home at (100,0,0) facing +X: local forward (0,0,-1) is world +X, so local (0,0,-10) is (110,0,0).
            Assert.That(v3, Does.Contain("ox=110"));
            Assert.That(v3, Does.Contain("fx=1"));
            Assert.That(v3, Does.Not.Contain("storage dropped"));
        }

        [Test]
        public void Storage_RoundTripsThroughSaveAndReload()
        {
            var p = Build();
            p.Main("", UpdateType.Update10 | UpdateType.Update100);
            p.Save();
            var storage = ((Sandbox.ModAPI.IMyGridProgram)p).Storage;
            Assert.That(storage, Does.Contain("[Miner]"));
            Build(storage).Main("", UpdateType.Update10 | UpdateType.Update100);
            Assert.That(_lastScreen, Does.Not.Contain("storage dropped"));
        }
    }
}

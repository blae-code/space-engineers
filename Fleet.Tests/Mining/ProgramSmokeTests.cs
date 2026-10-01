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

        IngameScript.Program Build(string storage = "")
        {
            _me = A.Fake<IMyProgrammableBlock>();
            A.CallTo(() => _me.CubeGrid).Returns(A.Fake<IMyCubeGrid>());
            _me.CustomData = "";
            var surface = A.Fake<IMyTextSurface>();
            A.CallTo(() => surface.WriteText(A<StringBuilder>._, A<bool>._))
                .Invokes((StringBuilder sb, bool append) => _lastScreen = sb.ToString());
            A.CallTo(() => _me.GetSurface(0)).Returns(surface);

            _igc = A.Fake<IMyIntergridCommunicationSystem>();
            A.CallTo(_igc).Where(c => c.Method.Name == "SendBroadcastMessage")
                .Invokes(c => _broadcast = c.Arguments[1] as string);

            var program = FormatterServices.GetUninitializedObject(typeof(IngameScript.Program));
            var backend = (Sandbox.ModAPI.IMyGridProgram)program;
            backend.Runtime = A.Fake<IMyGridProgramRuntimeInfo>();
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

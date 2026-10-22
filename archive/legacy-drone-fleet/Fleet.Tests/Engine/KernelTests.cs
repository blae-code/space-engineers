using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    class CountingSub : ISubsystem
    {
        public CountingSub(string name, List<string> calls) { Name = name; Calls = calls; }
        public string Name { get; private set; }
        public int StorageVersion { get { return 1; } }
        public List<string> Calls;
        public bool ThrowOn10;
        public void Update1() { Calls.Add(Name + ".1"); }
        public void Update10() { if (ThrowOn10) throw new InvalidOperationException("boom"); Calls.Add(Name + ".10"); }
        public void Update100() { Calls.Add(Name + ".100"); }
        public void HandleMessage(MyIGCMessage msg) { }
        public void Save(MyIni ini) { }
        public bool Load(MyIni ini, int savedVersion) { return true; }
        public void Status(StringBuilder sb) { }
    }

    [TestFixture]
    public class KernelTests
    {
        List<string> calls;
        CountingSub a, b;
        Profiler prof;
        Kernel k;

        [SetUp]
        public void Init()
        {
            calls = new List<string>();
            a = new CountingSub("A", calls);
            b = new CountingSub("B", calls);
            prof = new Profiler(4);
            k = new Kernel(new List<ISubsystem> { a, b }, prof, () => 123);
        }

        [Test]
        public void RoutesByFlag_InOrder()
        {
            k.Tick(UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100, 0.5);
            Assert.That(calls, Is.EqualTo(new[] { "A.1", "B.1", "A.10", "B.10", "A.100", "B.100" }));
        }

        [Test]
        public void OnlyRequestedFlags()
        {
            k.Tick(UpdateType.Update10, 0);
            Assert.That(calls, Is.EqualTo(new[] { "A.10", "B.10" }));
        }

        [Test]
        public void Records_Profile_EvenForTerminalRuns()
        {
            k.Tick(UpdateType.Terminal, 0.25);
            Assert.That(calls, Is.Empty);
            Assert.That(prof.Last, Is.EqualTo(123));
            Assert.That(prof.AverageMs, Is.EqualTo(0.25).Within(1e-9));
        }

        [Test]
        public void Exception_EntersSafeOnce_AndStopsTicking()
        {
            int safeCalls = 0;
            k.OnSafe = () => safeCalls++;
            b.ThrowOn10 = true;
            k.Tick(UpdateType.Update10, 0);
            Assert.That(k.IsSafe, Is.True);
            Assert.That(k.SafeReason, Is.EqualTo("B: boom"));
            Assert.That(safeCalls, Is.EqualTo(1));

            calls.Clear();
            k.Tick(UpdateType.Update10 | UpdateType.Update100, 0);
            Assert.That(calls, Is.Empty);
            k.EnterSafe("again");
            Assert.That(safeCalls, Is.EqualTo(1));
            Assert.That(k.SafeReason, Is.EqualTo("B: boom"));
        }

        [Test]
        public void ResetSafe_Resumes()
        {
            b.ThrowOn10 = true;
            k.Tick(UpdateType.Update10, 0);
            b.ThrowOn10 = false;
            k.ResetSafe();
            Assert.That(k.IsSafe, Is.False);
            Assert.That(k.SafeReason, Is.EqualTo(""));
            calls.Clear();
            k.Tick(UpdateType.Update10, 0);
            Assert.That(calls, Is.EqualTo(new[] { "A.10", "B.10" }));
        }

        [Test]
        public void OnSafe_MayBeNull()
        {
            b.ThrowOn10 = true;
            Assert.DoesNotThrow(() => k.Tick(UpdateType.Update10, 0));
            Assert.That(k.IsSafe, Is.True);
        }
    }
}
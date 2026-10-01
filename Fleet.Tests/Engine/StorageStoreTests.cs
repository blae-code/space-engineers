using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    class FakeSub : ISubsystem
    {
        public FakeSub(string name, int version) { Name = name; StorageVersion = version; }
        public string Name { get; private set; }
        public int StorageVersion { get; private set; }
        public int Value = -1;
        public bool Loaded;
        public void Update1() { }
        public void Update10() { }
        public void Update100() { }
        public void HandleMessage(MyIGCMessage msg) { }
        public void Save(MyIni ini) { ini.Set(Name, "x", Value); }
        public bool Load(MyIni ini, int savedVersion)
        {
            if (savedVersion != StorageVersion) return false;
            Value = ini.Get(Name, "x").ToInt32(-1);
            Loaded = true;
            return true;
        }
        public void Status(StringBuilder sb) { }
    }

    [TestFixture]
    public class StorageStoreTests
    {
        [Test]
        public void RoundTrip()
        {
            var a = new FakeSub("A", 1) { Value = 11 };
            var b = new FakeSub("B", 2) { Value = 22 };
            var text = StorageStore.Save(new List<ISubsystem> { a, b }, new MyIni());
            Assert.That(text, Does.Contain("[A]"));
            Assert.That(text, Does.Contain("v=2"));

            var a2 = new FakeSub("A", 1); var b2 = new FakeSub("B", 2);
            var dropped = new List<string>();
            StorageStore.Load(text, new List<ISubsystem> { a2, b2 }, new MyIni(), dropped);
            Assert.That(dropped, Is.Empty);
            Assert.That(a2.Value, Is.EqualTo(11));
            Assert.That(b2.Value, Is.EqualTo(22));
        }

        [Test]
        public void IncompatibleVersion_IsDropped_OthersStillLoad()
        {
            var text = StorageStore.Save(new List<ISubsystem> { new FakeSub("A", 1) { Value = 5 }, new FakeSub("B", 1) { Value = 6 } }, new MyIni());
            var a2 = new FakeSub("A", 1); var b2 = new FakeSub("B", 2);   // B's code moved to version 2
            var dropped = new List<string>();
            StorageStore.Load(text, new List<ISubsystem> { a2, b2 }, new MyIni(), dropped);
            Assert.That(dropped, Is.EqualTo(new[] { "B" }));
            Assert.That(a2.Value, Is.EqualTo(5));
            Assert.That(b2.Loaded, Is.False);
        }

        [Test]
        public void MissingSection_IsFreshStart_NotReported()
        {
            var text = StorageStore.Save(new List<ISubsystem> { new FakeSub("A", 1) { Value = 5 } }, new MyIni());
            var c = new FakeSub("C", 1);
            var dropped = new List<string>();
            StorageStore.Load(text, new List<ISubsystem> { c }, new MyIni(), dropped);
            Assert.That(dropped, Is.Empty);
            Assert.That(c.Loaded, Is.False);
        }

        [Test]
        public void EmptyStorage_LoadsNothing_NoError()
        {
            var a = new FakeSub("A", 1);
            var dropped = new List<string>();
            StorageStore.Load("", new List<ISubsystem> { a }, new MyIni(), dropped);
            Assert.That(dropped, Is.Empty);
            Assert.That(a.Loaded, Is.False);
        }

        [Test]
        public void GarbageStorage_ReportsStar_LoadsNothing()
        {
            var a = new FakeSub("A", 1);
            var dropped = new List<string>();
            StorageStore.Load("@@@", new List<ISubsystem> { a }, new MyIni(), dropped);
            Assert.That(dropped, Is.EqualTo(new[] { "*" }));
            Assert.That(a.Loaded, Is.False);
        }
    }
}

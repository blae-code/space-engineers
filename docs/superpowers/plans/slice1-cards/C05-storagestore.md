# C05 — StorageStore (versioned save/load of subsystem state)

Milestone A · Difficulty: easy · Executor: local

## Goal
Serialize every subsystem's state into the PB `Storage` string, one INI section per subsystem with its own
version number, and restore it after a reload or a script update. A subsystem that cannot read an old
version reports it; its section is dropped and named so the operator knows what was lost. Nothing throws.

## Files
- Create: `Fleet.Engine/Core/StorageStore.cs`
- Create: `Fleet.Tests/Engine/StorageStoreTests.cs`

## Attach in Continue
This card, `Fleet.Engine/Core/ISubsystem.cs`.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only. No LINQ. Allocation is allowed (runs on FSM transitions and at load).
- `MyIni` lives in `VRage.Game.ModAPI.Ingame.Utilities`.

## Interface (implement exactly)
```csharp
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class Program
    {
        public static class StorageStore
        {
            public const string VersionKey = "v";
            // Clears scratch, then for each subsystem: Set(sub.Name, "v", sub.StorageVersion) and sub.Save(scratch).
            public static string Save(List<ISubsystem> subs, MyIni scratch);
            // Parses storage into scratch. Unparseable -> dropped.Add("*"), no subsystem is loaded.
            // For each subsystem whose section exists: v = Get(Name,"v").ToInt32(0); if !sub.Load(scratch, v) -> dropped.Add(sub.Name).
            // A subsystem with no section is left untouched (fresh start) and is NOT reported.
            public static void Load(string storage, List<ISubsystem> subs, MyIni scratch, List<string> dropped);
        }
    }
}
```

## MyIni API you need
`scratch.Clear()`, `scratch.TryParse(text)` (bool; `""` parses fine; `"@@@"` fails),
`scratch.ContainsSection(name)`, `scratch.Set(section, key, int)`, `scratch.Get(section, key).ToInt32(0)`,
`scratch.ToString()`.

## Tests — create `Fleet.Tests/Engine/StorageStoreTests.cs` verbatim
```csharp
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
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~StorageStoreTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'StorageStore' does not exist".
- [ ] 3. Create `Fleet.Engine/Core/StorageStore.cs`.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Engine/Core/StorageStore.cs Fleet.Tests/Engine/StorageStoreTests.cs; git commit -m "feat(engine): versioned Storage store"`

## Done when
All StorageStoreTests pass.

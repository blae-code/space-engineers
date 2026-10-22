using System.Collections.Generic;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class ConfigLoaderTests
    {
        List<string> w;
        Settings s;
        [SetUp] public void Init() { w = new List<string>(); s = new Settings(); }

        [Test]
        public void EmptyText_AllDefaults_NoWarnings()
        {
            Assert.That(ConfigLoader.Load("", s, w), Is.True);
            Assert.That(w, Is.Empty);
            Assert.That(s.Width, Is.EqualTo(5));
            Assert.That(s.OnReload, Is.EqualTo(ReloadPolicy.Resume));
        }

        [Test]
        public void ReadsValues()
        {
            var text = "[Miner]\nWidth=7\nDepth=42.5\nLoop=false\nOnDamage=stop\nEject=Stone, Ice ,\n[Flight]\nSafeAltitude=300\n[Reload]\nOnReload=Hold\n[Fleet]\nName=Alpha\n";
            Assert.That(ConfigLoader.Load(text, s, w), Is.True);
            Assert.That(w, Is.Empty);
            Assert.That(s.Width, Is.EqualTo(7));
            Assert.That(s.Depth, Is.EqualTo(42.5));
            Assert.That(s.Loop, Is.False);
            Assert.That(s.OnDamage, Is.EqualTo(DamagePolicy.Stop));
            Assert.That(s.Eject, Is.EqualTo(new[] { "Stone", "Ice" }));
            Assert.That(s.SafeAltitude, Is.EqualTo(300));
            Assert.That(s.OnReload, Is.EqualTo(ReloadPolicy.Hold));
            Assert.That(s.Name, Is.EqualTo("Alpha"));
        }

        [Test]
        public void BadValues_KeepDefault_AndWarn()
        {
            var text = "[Miner]\nWidth=abc\nHeight=0\nWorkSpeed=fast\nLoop=maybe\nOnDamage=Explode\n[Fleet]\nName=\n";
            Assert.That(ConfigLoader.Load(text, s, w), Is.True);
            Assert.That(s.Width, Is.EqualTo(5));
            Assert.That(s.Height, Is.EqualTo(5));
            Assert.That(s.WorkSpeed, Is.EqualTo(1.5));
            Assert.That(s.Loop, Is.True);
            Assert.That(s.OnDamage, Is.EqualTo(DamagePolicy.Home));
            Assert.That(s.Name, Is.EqualTo("Miner-01"));
            Assert.That(w, Does.Contain("Miner.Width: not an integer"));
            Assert.That(w, Does.Contain("Miner.Height: out of range 1..50"));
            Assert.That(w, Does.Contain("Miner.WorkSpeed: not a number"));
            Assert.That(w, Does.Contain("Miner.Loop: not true/false"));
            Assert.That(w, Does.Contain("Miner.OnDamage: unknown value 'Explode'"));
            Assert.That(w, Does.Contain("Fleet.Name: must not be empty"));
        }

        [Test]
        public void EmptyEject_MeansEjectNothing()
        {
            ConfigLoader.Load("[Miner]\nEject=\n", s, w);
            Assert.That(s.Eject, Is.Empty);
        }

        [Test]
        public void MalformedIni_ReturnsFalse_AndKeepsSettings()
        {
            s.Width = 9;
            Assert.That(ConfigLoader.Load("this is not ini\n=== [", s, w), Is.False);
            Assert.That(s.Width, Is.EqualTo(9));
            Assert.That(w, Does.Contain("Custom Data: not valid INI"));
        }

        [Test]
        public void Write_FillsEveryKey_FromEmpty()
        {
            var text = ConfigLoader.Write("", s);
            foreach (var key in new[] { "Name=", "Tag=", "Width=5", "Height=5", "Depth=30", "StartDepth=0", "WorkSpeed=1.5",
                "RetractSpeed=4", "MaxLoad=90", "MinLiftMargin=1.3", "Eject=Stone", "Loop=true", "OnDamage=Home",
                "MaxSpeed=60", "ApproachSpeed=5", "DockSpeed=0.5", "SafeAltitude=150", "StuckSeconds=5",
                "MinBattery=20", "MinHydrogen=30", "MinUranium=5", "OnReload=Resume" })
                Assert.That(text, Does.Contain(key), key);
            foreach (var sec in new[] { "[Fleet]", "[Miner]", "[Flight]", "[Energy]", "[Reload]" })
                Assert.That(text, Does.Contain(sec), sec);
        }

        [Test]
        public void Write_PreservesUserValuesAndUnknownSections()
        {
            var original = "[Miner]\nWidth=7\n[Custom]\nMyKey=hello\n";
            ConfigLoader.Load(original, s, w);
            var text = ConfigLoader.Write(original, s);
            Assert.That(text, Does.Contain("Width=7"));
            Assert.That(text, Does.Contain("[Custom]"));
            Assert.That(text, Does.Contain("MyKey=hello"));
            var s2 = new Settings();
            Assert.That(ConfigLoader.Load(text, s2, new List<string>()), Is.True);
            Assert.That(s2.Width, Is.EqualTo(7));
        }
    }
}
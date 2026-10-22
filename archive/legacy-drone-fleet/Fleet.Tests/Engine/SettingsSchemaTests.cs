using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class SettingsSchemaTests
    {
        [Test]
        public void IndexConstants_MatchTableOrder()
        {
            Assert.That(SettingsSchema.Fields[SettingsSchema.Width].Key, Is.EqualTo("Width"));
            Assert.That(SettingsSchema.Fields[SettingsSchema.Loop].Key, Is.EqualTo("Loop"));
            Assert.That(SettingsSchema.Fields[SettingsSchema.OnReload].Key, Is.EqualTo("OnReload"));
            Assert.That(SettingsSchema.Fields[SettingsSchema.MinUranium].Key, Is.EqualTo("MinUranium"));
            Assert.That(SettingsSchema.Fields.Length, Is.EqualTo(SettingsSchema.MinUranium + 1));
        }

        [Test]
        public void GetSet_RoundTripsEveryField_AtMinAndMax()
        {
            for (int i = 0; i < SettingsSchema.Fields.Length; i++)
            {
                var f = SettingsSchema.Fields[i];
                foreach (var v in new[] { f.Min, f.Max })
                {
                    var s = new Settings();
                    Assert.That(SettingsSchema.Set(s, i, v), Is.True, f.Key);
                    Assert.That(SettingsSchema.Get(s, i), Is.EqualTo(v).Within(1e-9), f.Key);
                }
            }
        }

        [Test]
        public void Set_RejectsOutOfRange_AndLeavesSettingsUntouched()
        {
            var s = new Settings();
            Assert.That(SettingsSchema.Set(s, SettingsSchema.Width, 51), Is.False);
            Assert.That(SettingsSchema.Set(s, SettingsSchema.MinLiftMargin, 1.0), Is.False);
            Assert.That(SettingsSchema.Set(s, SettingsSchema.OnDamage, 3), Is.False);
            Assert.That(SettingsSchema.Set(s, 99, 1), Is.False);
            Assert.That(s.Width, Is.EqualTo(5));
            Assert.That(s.MinLiftMargin, Is.EqualTo(1.3));
        }

        // Every value the schema accepts must also be accepted by ConfigLoader (no warning), and every
        // value just outside the schema's range must be rejected by it — the two tables cannot drift.
        [Test]
        public void Ranges_AgreeWithConfigLoader()
        {
            for (int i = 0; i < SettingsSchema.Fields.Length; i++)
            {
                var f = SettingsSchema.Fields[i];
                if (f.Kind == FieldKind.Toggle || f.Kind == FieldKind.Choice) continue;
                foreach (var v in new[] { f.Min, f.Max })
                    Assert.That(Warnings(f, SettingsSchema.ToIniText(i, v)), Is.Empty, f.Key + "=" + v);
                double below = f.Min - (f.Kind == FieldKind.Int ? 1 : f.Step);
                double above = f.Max + (f.Kind == FieldKind.Int ? 1 : f.Step);
                Assert.That(Warnings(f, SettingsSchema.ToIniText(i, above)), Is.Not.Empty, f.Key + " above");
                if (below >= 0 || f.Min > 0)
                    Assert.That(Warnings(f, SettingsSchema.ToIniText(i, below)), Is.Not.Empty, f.Key + " below");
            }
        }

        static List<string> Warnings(SettingField f, string value)
        {
            var w = new List<string>();
            ConfigLoader.Load("[" + f.Section + "]\n" + f.Key + "=" + value + "\n", new Settings(), w);
            return w;
        }

        [Test]
        public void ChoicesAndToggles_RoundTripThroughConfigLoader()
        {
            var s = new Settings();
            SettingsSchema.Set(s, SettingsSchema.OnDamage, 2);
            SettingsSchema.Set(s, SettingsSchema.OnReload, 1);
            SettingsSchema.Set(s, SettingsSchema.Loop, 0);
            var back = new Settings();
            ConfigLoader.Load(ConfigLoader.Write("", s), back, new List<string>());
            Assert.That(back.OnDamage, Is.EqualTo(DamagePolicy.Stop));
            Assert.That(back.OnReload, Is.EqualTo(ReloadPolicy.ReturnHome));
            Assert.That(back.Loop, Is.False);
        }

        [Test]
        public void Channel_RoundTripsAndRejectsSlash()
        {
            var s = new Settings();
            var w = new List<string>();
            ConfigLoader.Load("[Fleet]\nChannel=Alpha\n", s, w);
            Assert.That(s.Channel, Is.EqualTo("Alpha"));
            Assert.That(ConfigLoader.Write("", s), Does.Contain("Channel=Alpha"));
            ConfigLoader.Load("[Fleet]\nChannel=a/b\n", s, w);
            Assert.That(s.Channel, Is.EqualTo("Alpha"));
            Assert.That(w, Is.Not.Empty);
        }

        [Test]
        public void Find_IsCaseInsensitive()
        {
            Assert.That(SettingsSchema.Find("miner", "width"), Is.EqualTo(SettingsSchema.Width));
            Assert.That(SettingsSchema.Find("Flight", "MaxSpeed"), Is.EqualTo(SettingsSchema.MaxSpeed));
            Assert.That(SettingsSchema.Find("Miner", "Eject"), Is.EqualTo(-1));
            Assert.That(SettingsSchema.Find("Fleet", "Name"), Is.EqualTo(-1));
        }

        [TestCase(SettingsSchema.Width, "6", 6)]
        [TestCase(SettingsSchema.WorkSpeed, " 2.5 ", 2.5)]
        [TestCase(SettingsSchema.Loop, "off", 0)]
        [TestCase(SettingsSchema.Loop, "TRUE", 1)]
        [TestCase(SettingsSchema.OnDamage, "job", 1)]
        [TestCase(SettingsSchema.OnReload, "Hold", 2)]
        public void TryParse_Valid(int field, string text, double expected)
        {
            double v;
            Assert.That(SettingsSchema.TryParse(field, text, out v), Is.True);
            Assert.That(v, Is.EqualTo(expected));
        }

        [TestCase(SettingsSchema.Width, "six")]
        [TestCase(SettingsSchema.Loop, "maybe")]
        [TestCase(SettingsSchema.OnDamage, "Explode")]
        [TestCase(-1, "1")]
        public void TryParse_Invalid(int field, string text)
        {
            double v;
            Assert.That(SettingsSchema.TryParse(field, text, out v), Is.False);
        }

        [Test]
        public void AppendValue_FormatsByKind()
        {
            var sb = new StringBuilder();
            SettingsSchema.AppendValue(sb, SettingsSchema.Loop, 1).Append('|');
            SettingsSchema.AppendValue(sb, SettingsSchema.OnDamage, 1).Append('|');
            SettingsSchema.AppendValue(sb, SettingsSchema.MinLiftMargin, 1.3).Append('|');
            SettingsSchema.AppendValue(sb, SettingsSchema.Width, 7);
            Assert.That(sb.ToString(), Is.EqualTo("On|Job|1.30|7"));
        }
    }
}

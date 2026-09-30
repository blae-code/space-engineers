# C01 — SbFormat (allocation-free number formatting)

Milestone A · Difficulty: easy · Executor: local

## Goal
Numbers must be written to LCD text every 100 ticks without allocating. `int.ToString()` and string
interpolation allocate, so this card provides helpers that append digits directly into a `StringBuilder`.

## Files
- Create: `Fleet.Engine/Util/SbFormat.cs`
- Create: `Fleet.Tests/Engine/SbFormatTests.cs`

## Attach in Continue
This card only.

## Constraints (always)
- Script code: `namespace IngameScript { public partial class Program { ... } }`, the class nested and `public`.
- C# 6 only (no `out var`, tuples, local functions, pattern matching, discards, `default` literal).
- No LINQ, no `System.Globalization`, no string concatenation or `ToString()` on numbers **inside these helpers**.

## Interface (implement exactly)
```csharp
using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        public static class SbFormat
        {
            public static StringBuilder AppendInt(StringBuilder sb, long value);
            public static StringBuilder AppendFixed(StringBuilder sb, double value, int decimals); // decimals 0..3
            public static StringBuilder AppendPad2(StringBuilder sb, int value);                   // 0..99 -> 2 digits
            public static StringBuilder AppendPercent(StringBuilder sb, double ratio);            // 0.905 -> "91%"
            public static StringBuilder AppendTime(StringBuilder sb, double seconds);             // mm:ss
        }
    }
}
```

## Behaviour
- All methods append to `sb` and return the same `sb`.
- `AppendInt`: decimal digits, leading `-` for negatives. Use a `static readonly char[] Buf = new char[20]`
  filled from the end, then `sb.Append(Buf, start, count)`. Handle `0`. (`long.MinValue` need not be supported.)
- `AppendFixed`: round `value * 10^decimals` with `Math.Round(x, MidpointRounding.AwayFromZero)` into a
  `long`; if the rounded value is < 0, write `-` and continue with its absolute value (so -0.25 → `-0.3`);
  if it is 0, write no sign; write the integer part with `AppendInt` logic, then `.`
  and exactly `decimals` digits (zero-padded). `decimals == 0` writes no `.`.
- `AppendPad2`: values 0..9 get a leading `0`; values ≥ 10 are written as-is.
- `AppendPercent`: `ratio * 100` rounded away from zero to an integer, then `%`.
- `AppendTime`: `total = (long)Math.Floor(seconds)`; minutes = total / 60 (any number of digits, but at
  least 2 via `AppendPad2` when < 100), seconds = total % 60 via `AppendPad2`; separator `:`.

## Tests — create `Fleet.Tests/Engine/SbFormatTests.cs` verbatim
```csharp
using System.Text;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Engine
{
    [TestFixture]
    public class SbFormatTests
    {
        static string Run(System.Func<StringBuilder, StringBuilder> f) { return f(new StringBuilder()).ToString(); }

        [TestCase(0, "0")]
        [TestCase(7, "7")]
        [TestCase(12345, "12345")]
        [TestCase(-42, "-42")]
        [TestCase(2147483647L, "2147483647")]
        public void AppendInt(long v, string expected) { Assert.That(Run(sb => SbFormat.AppendInt(sb, v)), Is.EqualTo(expected)); }

        [Test]
        public void AppendInt_AppendsToExistingContent()
        {
            var sb = new StringBuilder("x=");
            var ret = SbFormat.AppendInt(sb, 5);
            Assert.That(sb.ToString(), Is.EqualTo("x=5"));
            Assert.That(ret, Is.SameAs(sb));
        }

        [TestCase(1.25, 1, "1.3")]
        [TestCase(-1.25, 1, "-1.3")]
        [TestCase(3.14159, 2, "3.14")]
        [TestCase(2.0, 0, "2")]
        [TestCase(1234.5, 0, "1235")]
        [TestCase(0.05, 1, "0.1")]
        [TestCase(-0.04, 1, "0.0")]
        [TestCase(-0.25, 1, "-0.3")]
        [TestCase(7.0, 3, "7.000")]
        [TestCase(0.5, 2, "0.50")]
        public void AppendFixed(double v, int d, string expected) { Assert.That(Run(sb => SbFormat.AppendFixed(sb, v, d)), Is.EqualTo(expected)); }

        [TestCase(0, "00")]
        [TestCase(7, "07")]
        [TestCase(12, "12")]
        public void AppendPad2(int v, string expected) { Assert.That(Run(sb => SbFormat.AppendPad2(sb, v)), Is.EqualTo(expected)); }

        [TestCase(0.0, "0%")]
        [TestCase(0.905, "91%")]
        [TestCase(1.0, "100%")]
        public void AppendPercent(double v, string expected) { Assert.That(Run(sb => SbFormat.AppendPercent(sb, v)), Is.EqualTo(expected)); }

        [TestCase(0.0, "00:00")]
        [TestCase(125.7, "02:05")]
        [TestCase(3725.0, "62:05")]
        [TestCase(6000.0, "100:00")]
        public void AppendTime(double v, string expected) { Assert.That(Run(sb => SbFormat.AppendTime(sb, v)), Is.EqualTo(expected)); }
    }
}
```

## Steps
- [ ] 1. Create the test file above exactly.
- [ ] 2. Run `dotnet test Fleet.Tests --filter "FullyQualifiedName~SbFormatTests" 2>&1 | tail -n 25`
      Expected: build error "The name 'SbFormat' does not exist".
- [ ] 3. Create `Fleet.Engine/Util/SbFormat.cs` implementing the interface and behaviour.
- [ ] 4. Run the same command. Expected: `Passed!` with 0 failed.
- [ ] 5. Commit: `git add Fleet.Engine/Util/SbFormat.cs Fleet.Tests/Engine/SbFormatTests.cs; git commit -m "feat(engine): allocation-free SbFormat"`

## Done when
All SbFormatTests pass and `SbFormat.cs` contains no `ToString(`, `+ "`, `$"` or `using System.Linq`.

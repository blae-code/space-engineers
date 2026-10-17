using System.Text;
using NUnit.Framework;
using VRage.Game.ModAPI.Ingame.Utilities;
using static IngameScript.Program;

namespace Fleet.Tests.Mining
{
    [TestFixture]
    public class MiningStatsTests
    {
        static string Text(MiningStats m) { return m.Append(new StringBuilder()).ToString(); }

        [Test] public void Fresh_IsEmpty()
        {
            var m = new MiningStats();
            Assert.That(m.Any, Is.False);
            Assert.That(Text(m), Is.EqualTo("trips 0  ore 0kg  --kg/h  0.0h"));
        }

        [Test] public void IsWorking_ExcludesIdleHoldSafeRecording()
        {
            Assert.That(MiningStats.IsWorking(MinerState.Idle), Is.False);
            Assert.That(MiningStats.IsWorking(MinerState.Hold), Is.False);
            Assert.That(MiningStats.IsWorking(MinerState.Safe), Is.False);
            Assert.That(MiningStats.IsWorking(MinerState.Recording), Is.False);
            Assert.That(MiningStats.IsWorking(MinerState.Drill), Is.True);
            Assert.That(MiningStats.IsWorking(MinerState.Unload), Is.True);
            Assert.That(MiningStats.IsWorking(MinerState.Charge), Is.True);
        }

        [Test] public void Tick_CountsOnlyWorkingTime()
        {
            var m = new MiningStats();
            m.Tick(10, MinerState.Drill);          // first tick only records the time
            m.Tick(20, MinerState.Drill);          // +10
            m.Tick(50, MinerState.Hold);           // not working: +0
            m.Tick(55, MinerState.RouteBack);      // +5
            Assert.That(m.JobSeconds, Is.EqualTo(15).Within(1e-9));
            Assert.That(m.Any, Is.True);
        }

        [Test] public void Tick_TimeGoingBackwards_AddsNothing()
        {
            var m = new MiningStats();
            m.Tick(100, MinerState.Drill);
            m.Tick(3, MinerState.Drill);           // script restarted: Time counts from 0 again
            m.Tick(8, MinerState.Drill);           // +5
            Assert.That(m.JobSeconds, Is.EqualTo(5).Within(1e-9));
        }

        [Test] public void Unload_CountsTripAndMassDrop()
        {
            var m = new MiningStats();
            m.OnTransition(MinerState.Dock, MinerState.Unload, 5000);
            m.OnTransition(MinerState.Unload, MinerState.Charge, 3800);
            Assert.That(m.Trips, Is.EqualTo(1));
            Assert.That(m.OreKg, Is.EqualTo(1200).Within(1e-9));
            m.OnTransition(MinerState.Dock, MinerState.Unload, 4000);
            m.OnTransition(MinerState.Unload, MinerState.Undock, 4100);   // mass rose (refuelled): no negative ore
            Assert.That(m.Trips, Is.EqualTo(2));
            Assert.That(m.OreKg, Is.EqualTo(1200).Within(1e-9));
        }

        [Test] public void LeavingUnload_WithoutEntering_IsNotATrip()
        {
            var m = new MiningStats();
            m.OnTransition(MinerState.Unload, MinerState.Charge, 3000);   // e.g. a reload mid-unload
            Assert.That(m.Trips, Is.EqualTo(0));
            Assert.That(m.OreKg, Is.EqualTo(0));
        }

        [Test] public void Append_ShowsRateAfterOneMinute()
        {
            var m = new MiningStats();
            m.OnTransition(MinerState.Dock, MinerState.Unload, 2500);
            m.OnTransition(MinerState.Unload, MinerState.Charge, 1000);
            m.Tick(0, MinerState.Drill);
            m.Tick(1800, MinerState.Drill);
            Assert.That(m.KgPerHour, Is.EqualTo(3000).Within(1e-9));
            Assert.That(Text(m), Is.EqualTo("trips 1  ore 1500kg  3000kg/h  0.5h"));
        }

        [Test] public void Reset_ClearsEverything()
        {
            var m = new MiningStats();
            m.OnTransition(MinerState.Dock, MinerState.Unload, 2000);
            m.Tick(0, MinerState.Unload);
            m.Tick(100, MinerState.Unload);
            m.Reset();
            m.OnTransition(MinerState.Unload, MinerState.Charge, 1000);   // the unload start was forgotten too
            Assert.That(m.Any, Is.False);
            Assert.That(m.OreKg, Is.EqualTo(0));
        }

        [Test] public void SaveLoad_RoundTrips_AndBadValuesFallBackToZero()
        {
            var m = new MiningStats();
            m.OnTransition(MinerState.Dock, MinerState.Unload, 900);
            m.OnTransition(MinerState.Unload, MinerState.Charge, 400);
            m.Tick(0, MinerState.Drill);
            m.Tick(120, MinerState.Drill);
            var ini = new MyIni();
            m.Save(ini, "Miner.Stats");
            var n = new MiningStats();
            n.Load(ini, "Miner.Stats");
            Assert.That(n.Trips, Is.EqualTo(1));
            Assert.That(n.OreKg, Is.EqualTo(500).Within(1e-9));
            Assert.That(n.JobSeconds, Is.EqualTo(120).Within(1e-9));

            var bad = new MyIni();
            bad.TryParse("[Miner.Stats]\ntrips=-4\noreKg=junk\n");
            n.Load(bad, "Miner.Stats");
            Assert.That(n.Trips, Is.EqualTo(0));
            Assert.That(n.OreKg, Is.EqualTo(0));
            Assert.That(n.JobSeconds, Is.EqualTo(0));
        }
    }
}
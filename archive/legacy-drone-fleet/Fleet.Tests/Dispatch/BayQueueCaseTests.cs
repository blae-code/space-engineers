using System.Collections.Generic;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Dispatch
{
    [TestFixture]
    public class BayQueueCaseTests
    {
        // Runs a ';'-separated script against a fresh BayQueue (reservations last 90 s) and returns the trace.
        //   B n        the carrier has bays with ids 1..n
        //   R d t      drone d requests a bay at time t -> "d=G<bay>" or "d=W<slot>"
        //   O b v      bay b occupied (v = 1) or empty (v = 0)
        //   T t        tick at time t -> "<drone>><bay>" for a promotion, or "-"
        //   F d        forget drone d
        //   #          queue length -> "w<n>"
        static string Run(string script)
        {
            var q = new BayQueue();
            var trace = new List<string>();
            foreach (var raw in script.Split(';'))
            {
                var c = raw.Trim().Split(' ');
                long arg, drone, bay;
                switch (c[0])
                {
                    case "B":
                        var ids = new List<long>();
                        for (int i = 1; i <= int.Parse(c[1]); i++) ids.Add(i);
                        q.SetBays(ids);
                        break;
                    case "R":
                        int type = q.Request(long.Parse(c[1]), double.Parse(c[2]), 90, out arg);
                        trace.Add(c[1] + "=" + (type == FleetMsg.DockGo ? "G" : "W") + arg);
                        break;
                    case "O": q.SetOccupied(long.Parse(c[1]), c[2] == "1"); break;
                    case "T": trace.Add(q.Tick(double.Parse(c[1]), 90, out drone, out bay) ? drone + ">" + bay : "-"); break;
                    case "F": q.Forget(long.Parse(c[1])); break;
                    case "#": trace.Add("w" + q.Waiting); break;
                }
            }
            return string.Join(" ", trace);
        }

        [TestCase("K1", "B 2;R 1 0;R 2 0;R 3 0;#", "1=G1 2=G2 3=W0 w1")]
        [TestCase("K2", "B 1;R 1 0;R 1 89;R 1 90", "1=G1 1=G1 1=G1")]
        [TestCase("K3", "B 1;R 1 0;R 2 0;T 90;R 1 91", "1=G1 2=W0 2>1 1=W0")]
        [TestCase("K4", "B 1;O 1 1;R 1 0;T 1;O 1 0;T 2", "1=W0 - 1>1")]
        [TestCase("K5", "B 1;R 1 0;R 2 0;R 3 0;F 2;R 3 1;#", "1=G1 2=W0 3=W1 3=W0 w1")]
        [TestCase("K6", "B 1;O 9 1;R 1 0", "1=G1")]
        [TestCase("K7", "B 1;R 1 0;R 2 0;F 1;T 1", "1=G1 2=W0 2>1")]
        [TestCase("K8", "B 1;R 1 0;O 1 1;R 1 5", "1=G1 1=W0")]
        [TestCase("K9", "B 2;R 1 0;T 1;#", "1=G1 - w0")]
        [TestCase("K10", "B 0;R 1 0;R 2 0;#", "1=W0 2=W1 w2")]
        public void Script(string id, string script, string expected)
        {
            Assert.That(Run(script), Is.EqualTo(expected), id);
        }
    }
}

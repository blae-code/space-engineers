using System.Collections.Generic;
using NUnit.Framework;
using static IngameScript.Program;

namespace Fleet.Tests.Dispatch
{
    [TestFixture]
    public class LeaseTableCaseTests
    {
        // Runs a ';'-separated script against a fresh LeaseTable and returns the trace.
        //   S n            start job 1 with n holes
        //   G d t [size]   grant to drone d at time t (size default 3) -> "d=<leaseId>:i,i,…" or "d=none"
        //   H d id mask t  status from drone d naming lease id with done mask, at time t
        //   E t            expire leases silent for 300 s at time t
        //   #              counts -> "done/leased/free"
        static string Run(string script)
        {
            var t = new LeaseTable();
            var trace = new List<string>();
            foreach (var raw in script.Split(';'))
            {
                var c = raw.Trim().Split(' ');
                switch (c[0])
                {
                    case "S": t.Start(1, int.Parse(c[1])); break;
                    case "G":
                        var l = new Lease();
                        t.Grant(long.Parse(c[1]), double.Parse(c[2]), c.Length > 3 ? int.Parse(c[3]) : 3, ref l);
                        if (l.Count == 0) { trace.Add(c[1] + "=none"); break; }
                        var idx = new List<string>();
                        for (int k = 0; k < l.Count; k++) idx.Add(l[k].ToString());
                        trace.Add(c[1] + "=" + l.LeaseId + ":" + string.Join(",", idx));
                        break;
                    case "H": t.Heard(long.Parse(c[1]), int.Parse(c[2]), int.Parse(c[3]), double.Parse(c[4])); break;
                    case "E": t.Expire(double.Parse(c[1]), 300); break;
                    case "#": trace.Add(t.DoneCount + "/" + t.LeasedCount + "/" + t.FreeCount); break;
                }
            }
            return string.Join(" ", trace);
        }

        [TestCase("K1", "S 6;G 1 0;H 1 1 1 5;G 1 6;#", "1=1:0,1,2 1=1:0,1,2 1/2/3")]
        [TestCase("K2", "S 6;G 1 0;H 1 1 7 5;G 1 6;#", "1=1:0,1,2 1=2:3,4,5 3/3/0")]
        [TestCase("K3", "S 3;G 1 0;E 299;#;E 300;#", "1=1:0,1,2 0/3/0 0/0/3")]
        [TestCase("K4", "S 3;G 1 0;H 1 1 0 200;E 400;#;E 500;#", "1=1:0,1,2 0/3/0 0/0/3")]
        [TestCase("K5", "S 5;G 1 0 2;E 300;G 2 301 4;#", "1=1:0,1 2=2:2,3,4,0 0/4/1")]
        [TestCase("K6", "S 4;G 1 0 2;G 2 0 2;H 1 1 2 1;H 2 2 1 1;#", "1=1:0,1 2=2:2,3 2/2/0")]
        [TestCase("K7", "S 2;G 1 0;G 2 0;#", "1=1:0,1 2=none 0/2/0")]
        [TestCase("K8", "S 2;G 1 0;H 1 1 3 1;G 1 2;#", "1=1:0,1 1=none 2/0/0")]
        [TestCase("K9", "S 3;G 1 0;G 1 250;E 300;#", "1=1:0,1,2 1=1:0,1,2 0/3/0")]
        [TestCase("K10", "S 3;G 1 0;H 1 9 7 1;#", "1=1:0,1,2 0/3/0")]
        [TestCase("K11", "S 2;G 1 0 2;H 1 1 15 1;#", "1=1:0,1 2/0/0")]
        [TestCase("K12", "S 3;G 1 0;H 1 1 2 1;E 301;#", "1=1:0,1,2 1/0/2")]
        public void Script(string id, string script, string expected)
        {
            Assert.That(Run(script), Is.EqualTo(expected), id);
        }
    }
}

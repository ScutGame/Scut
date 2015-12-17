using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameworkUnitTest.Cache.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;
using ZyGames.Framework.Model;
using ZyGames.Framework.Redis;

namespace FrameworkUnitTest.Cache
{
    [TestClass]
    public class UtilsTest
    {
        public static Double Calculate(CounterSample oldSample, CounterSample newSample)
        {
            double difference = newSample.RawValue - oldSample.RawValue;
            double timeInterval = newSample.TimeStamp100nSec - oldSample.TimeStamp100nSec;
            if (timeInterval != 0) return 100 * (1 - (difference / timeInterval));
            return 0;
        }

        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void MathTest()
        {
            int count = 100;
            int[] indexs = new int[count];
            for (int i = 0; i < count; i++)
            {
                indexs[i] = RandomUtils.GetHitIndex(new[] { 20, 80 });
            }
            Trace.WriteLine("Indexs:" + string.Join(",", indexs));
        }

        [TestMethod]
        public void WatchCpuAndMemory()
        {
            var pc = new PerformanceCounter("Processor Information", "% Processor Time");
            var cat = new PerformanceCounterCategory("Processor Information");
            var cpuInstances = cat.GetInstanceNames();
            var cpus = new Dictionary<string, CounterSample>();

            var memoryCounter = new PerformanceCounter("Memory", "Available MBytes");

            foreach (var s in cpuInstances)
            {
                pc.InstanceName = s;
                cpus.Add(s, pc.NextSample());
            }
            var t = DateTime.Now;
            while (t.AddMinutes(1) > DateTime.Now)
            {
                Trace.WriteLine(string.Format("Memory:{0}MB", memoryCounter.NextValue()));

                foreach (var s in cpuInstances)
                {
                    pc.InstanceName = s;
                    Trace.WriteLine(string.Format("CPU:{0} - {1:f}", s, Calculate(cpus[s], pc.NextSample())));
                    cpus[s] = pc.NextSample();
                }

                //Trace.Flush();
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}

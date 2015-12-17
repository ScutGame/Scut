/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FrameworkUnitTest.Cache.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Event;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Threading;
using ZyGames.Framework.Common.Timing;

namespace FrameworkUnitTest.Cache
{
    [TestClass]
    public class MathTest
    {
        private readonly static byte[] SplitChar = Encoding.UTF8.GetBytes("\r\n\r\n");

        private void ParseData(byte[] data)
        {
            int paramIndex = MathUtils.IndexOf(data, SplitChar);
            byte[] paramArr;
            if (paramIndex >= 0)
            {
                paramArr = new byte[paramIndex];
                Buffer.BlockCopy(data, 0, paramArr, 0, paramArr.Length);
                InputStream = new byte[data.Length - paramIndex - SplitChar.Length];
                Buffer.BlockCopy(data, paramIndex + SplitChar.Length, InputStream, 0, InputStream.Length);
            }
            else
            {
                paramArr = data;
            }
            ParamString = Encoding.UTF8.GetString(paramArr);
        }

        public string ParamString { get; set; }


        public byte[] InputStream { get; set; }

        [TestMethod]
        public void CheckOutofTest()
        {
            int num = -1;
            uint a = num.ToUInt32();
            Trace.WriteLine("a=" + a);
        }

        [TestMethod]
        public void UnionTest()
        {
            string[] k1 = new[] {"aa", "bb"};
            string[] k2 = new[] { "bb", "cc" };
            Trace.WriteLine(string.Join(",",k1.Union(k2)));
        }

        [TestMethod]
        public void ParseTest()
        {
            Trace.WriteLine(typeof(ShareCacheStruct<KeyData>).FullName);
            string typeName = string.Format("{0}[[{1}, {2}]], {3}", typeof(ShareCacheStruct<>).FullName, typeof(KeyData).FullName, typeof(KeyData).Assembly.FullName, typeof(ShareCacheStruct<>).Assembly.FullName);
            Type type = Type.GetType(typeName, false, true);
            Trace.WriteLine(typeName);
            Assert.IsTrue(type != null);
            byte[] data = Encoding.UTF8.GetBytes("abc\r\n\r\n123");
            ParseData(data);
            Assert.IsTrue(InputStream.Length > 0);
        }

        [TestMethod]
        public void AddOrSubTest()
        {
            Assert.IsFalse(MathUtils.TryAdd(uint.MaxValue, 1, r => { }));
            Assert.IsTrue(MathUtils.TryAdd(uint.MaxValue, 0, r => { }));
            Assert.IsFalse(MathUtils.TryAdd(1, uint.MaxValue, r => { }));
            Assert.IsTrue(MathUtils.TryAdd(0, uint.MaxValue, r => { }));
            Assert.IsTrue(MathUtils.TryAdd((uint)0, 0, r => { }));
            Assert.IsTrue(MathUtils.TryAdd((uint)0, 1, r => { }));

            Assert.IsTrue(MathUtils.TrySub(uint.MaxValue, 1, r => { }));
            Assert.IsTrue(MathUtils.TrySub(uint.MaxValue, 0, r => { }));
            Assert.IsFalse(MathUtils.TrySub(1, uint.MaxValue, r => { }));
            Assert.IsFalse(MathUtils.TrySub(0, uint.MaxValue, r => { }));
            Assert.IsTrue(MathUtils.TrySub((uint)0, 0, r => { }));
            Assert.IsTrue(MathUtils.TrySub((uint)1, 0, r => { }));


            Assert.IsFalse(MathUtils.TryAdd(ushort.MaxValue, 1, r => { }));
            Assert.IsTrue(MathUtils.TryAdd(ushort.MaxValue, 0, r => { }));
            Assert.IsFalse(MathUtils.TryAdd(1, ushort.MaxValue, r => { }));
            Assert.IsTrue(MathUtils.TryAdd(0, ushort.MaxValue, r => { }));
            Assert.IsTrue(MathUtils.TryAdd((ushort)0, 0, r => { }));
            Assert.IsTrue(MathUtils.TryAdd((ushort)0, 1, r => { }));

            Assert.IsTrue(MathUtils.TrySub(ushort.MaxValue, 1, r => { }));
            Assert.IsTrue(MathUtils.TrySub(ushort.MaxValue, 0, r => { }));
            Assert.IsFalse(MathUtils.TrySub(1, ushort.MaxValue, r => { }));
            Assert.IsFalse(MathUtils.TrySub(0, ushort.MaxValue, r => { }));
            Assert.IsTrue(MathUtils.TrySub((ushort)0, 0, r => { }));
            Assert.IsTrue(MathUtils.TrySub((ushort)1, 0, r => { }));


        }

        [TestMethod]
        public void ThreadTest()
        {
            var _threadPools = new SmartThreadPool(180 * 1000, 100, 5);
            _threadPools.Start();

            var result = _threadPools.QueueWorkItem(() =>
            {
                Thread.Sleep(5000);
            });
            while (!result.IsCompleted)
            {
                Thread.Sleep(100);
                Trace.WriteLine("wait...");
            }
            Trace.WriteLine(result.Result);
        }

        //[TestMethod]
        public void TimerPlanTest()
        {
            TimeListener.Append(PlanConfig.EveryMinutePlan((p) =>
            {
                Trace.WriteLine(p.Name + " time:" + DateTime.Now.ToString("HH:mm:ss-ms"));
            }, "plan1", "", DateTime.Now.AddMinutes(1).ToString("HH:mm:ss"), 1));

            while (TimeListener.HasWaitPlan)
            {
                Thread.Sleep(1000);
            }
        }

        [TestMethod]
        public void SortTest()
        {
            var list = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(RandomUtils.GetRandom(0, 1000));
            }

            var watch = Stopwatch.StartNew();

            watch.Restart();
            for (int i = 0; i < 100; i++)
            {
                var copyList = list.ToList();
                copyList.QuickSort((a, b) => a.CompareTo(b));
            }
            Trace.WriteLine("QuickSort:" + watch.ElapsedMilliseconds);

            watch.Restart();
            for (int i = 0; i < 100; i++)
            {
                var copyList = list.ToList();
                var temp = copyList.OrderBy(t => t);
            }
            Trace.WriteLine("OrderBy:" + watch.ElapsedMilliseconds);

            watch.Restart();
            for (int i = 0; i < 100; i++)
            {
                var copyList = list.ToList();
                copyList.Sort((a, b) => a.CompareTo(b));
            }
            Trace.WriteLine("sort:" + watch.ElapsedMilliseconds);

        }

        [TestMethod]
        public void TimerTest()
        {
            //TimeListener.Append(PlanConfig.);
            int MaxRun = 100000;
            //int timeout = 60;
            int errorCount = 0;
            double errorTime = 0;
            double minTime = 0;
            double maxTime = 0;

            Trace.WriteLine("Thread creating...");
            var task = Task.Run(() =>
             {
                 for (int i = 0; i < MaxRun; i++)
                 {
                     var e = new TimeoutNotifyEventArgs(60);
                     e.Callback += (p) =>
                     {
                         var t = (DateTime.Now - p.ExpiredTime);
                         var sec = t.TotalSeconds;
                         //Trace.WriteLine(string.Format("Thread run:{0}ms", t.TotalMilliseconds));
                         errorTime += t.TotalMilliseconds;
                         if (minTime > t.TotalMilliseconds) minTime = t.TotalMilliseconds;
                         if (maxTime < t.TotalMilliseconds) maxTime = t.TotalMilliseconds;

                         if (sec > 1)
                         {
                             errorCount++;
                         }
                     };
                     EventNotifier.Put(e);
                 }
             });
            task.Wait();
            var task1 = Task.Run(() =>
            {
                while (EventNotifier.WaitEventCount > 0 || EventNotifier.TimerNum < MaxRun)
                {
                    Trace.WriteLine(string.Format("Thread {3} wait: {0}, run: {1}, timeout num:{2}", EventNotifier.WaitEventCount, EventNotifier.TimerNum, errorCount, EventNotifier.ActiveThreadCount));
                    Thread.Sleep(500);
                }
            });
            task1.Wait();
            var avg = MathUtils.RoundCustom(errorTime / MaxRun, 4);
            Trace.WriteLine(string.Format("Thread wait: {0}, run: {1}, timeout num:{2}", EventNotifier.WaitEventCount, EventNotifier.TimerNum, errorCount));

            Trace.WriteLine(string.Format("Thread min: {0}ms, avg: {1}ms, max:{2}ms", minTime, avg, maxTime));

            EventNotifier.Dispose();
            Trace.WriteLine("Thread end.");
        }

        [TestMethod]
        public void ConcurrentTask()
        {
            var tasks = new Task[10];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    DoConcurrentTask();
                });
            }
            Task.WaitAll(tasks);
        }

        [TestMethod]
        public void HttpTest()
        {
            var watch = Stopwatch.StartNew();
            string url = "http://pass1.36you.net/Default.ashx?MsgId=0&Sid=&Uid=0&ActionID=1001&St=&MobileType=1&Pid=Z25420&Pwd=%258D%2591%25F7BqDs%25C4%250B%2528%25EDC%255F%258A%25C2%2586&IMEI=&ScreenX=960&ScreenY=860&RetailID=0000&Handler=Login&sign=9de1cb788d6251635c30bf8e88fdcced";
            var response = HttpUtils.Get(url, "application/json", 100, null, null);
            Trace.WriteLine("time:" + watch.ElapsedMilliseconds);
            var responseStream = response.GetResponseStream();
            string json;
            using (var sr = new StreamReader(responseStream, Encoding.UTF8))
            {
                json = sr.ReadToEnd();
                responseStream.Close();
            }
            Trace.WriteLine("time:" + watch.ElapsedMilliseconds);
            Trace.WriteLine("text:" + json);
        }

        private static ConcurrentDictionary<string, Lazy<MyClass>> cacheStruct = new ConcurrentDictionary<string, Lazy<MyClass>>();

        private MyClass DoConcurrentTask()
        {
            string key = "ka";
            var c = cacheStruct.GetOrAdd(key, k => new Lazy<MyClass>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication));
            var r = c.Value;

            try
            {
                if (Monitor.TryEnter(r, 100))
                {
                    TryGetConcurrent(key);
                    Thread.Sleep(10);
                }

            }
            finally
            {
                Monitor.Exit(r);
            }
            return r;
        }


        private MyClass TryGetConcurrent(string key)
        {
            Lazy<MyClass> ly;
            if (cacheStruct.TryGetValue(key, out ly))
            {
                return ly.Value;
            }
            return null;
        }

        private MyClass valueFactory()
        {
            Trace.WriteLine("times");
            return new MyClass();
        }

        [ProtoContract]
        public class MyClass
        {
            public MyClass()
            {
                ItemIds = new uint[10];
            }

            [ProtoMember(1, OverwriteList = true)]
            public uint[] ItemIds { get; set; }
        }

        [TestMethod]
        public void SerializeTest()
        {
            char ch = '1';
            Assert.IsTrue((int)ch-48 == 1);
            var c = new MyClass();
            var b =ProtoBufUtils.Serialize(c);
            var t = ProtoBufUtils.Deserialize<MyClass>(b);
            Assert.IsTrue(t.ItemIds.Length == c.ItemIds.Length);
        }
    }
}

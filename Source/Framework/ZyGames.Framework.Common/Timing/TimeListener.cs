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
using System.Collections.Generic;
using System.Threading;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Timing
{
    /// <summary>
    /// 定时器监听管理
    /// 使用场合:对间隔时间比较精确
    /// </summary>
    public class TimeListener
    {
        private static Timer _timer;
        private static int _msInterval = 2000;
        private static int _dueTime = 10000;
        private static int _isRunning = 0;
        private static IMonitorStrategy _lockStrategy = new MonitorLockStrategy();
        private static List<PlanConfig> _listenerQueue = new List<PlanConfig>();
        private static int _isDisposed = 0;

        /// <summary>
        /// 设置定时器
        /// </summary>
        /// <param name="msInterval"></param>
        public static void SetTimer(int msInterval)
        {
            CheckDisposed();
            SetTimer(msInterval, 10000);
        }

        /// <summary>
        /// 设置定时器
        /// </summary>
        /// <param name="msInterval"></param>
        /// <param name="dueTime"></param>
        public static void SetTimer(int msInterval, int dueTime)
        {
            CheckDisposed();
            Interlocked.Exchange(ref _msInterval, msInterval);
            Interlocked.Exchange(ref _dueTime, dueTime);
            Interlocked.Exchange(ref _timer, null);
            Initialize();
        }

        private static void CheckDisposed()
        {
            if (_isDisposed == 1)
            {
                throw new Exception("TimeListener has be disposed.");
            }
        }

        /// <summary>
        /// 显示释放
        /// </summary>
        public static void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                Interlocked.Exchange(ref _timer, null);
                _lockStrategy = null;
                _listenerQueue = null;
                Interlocked.Exchange(ref _isDisposed, 1);
            }
        }
        /// <summary>
        /// 增加定时任务计划
        /// </summary>
        /// <param name="planConfig"></param>
        public static void Append(PlanConfig planConfig)
        {
            CheckDisposed();
            if (planConfig == null)
            {
                throw new ArgumentNullException("planConfig");
            }
            Initialize();
            _lockStrategy.TryEnterLock(() =>
            {
                planConfig.SetDiffInterval(_msInterval / 1000);
                _listenerQueue.Add(planConfig);
            });

        }
        /// <summary>
        /// 移除定时任务计划
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool Remove(Predicate<PlanConfig> match)
        {
            CheckDisposed();
            var index = _listenerQueue.FindIndex(match);
            if (index != -1)
            {
                _lockStrategy.TryEnterLock(() => _listenerQueue.RemoveAt(index));
                return true;
            }
            return false;
        }

        private static void Initialize()
        {
            if (_timer == null)
            {
                Interlocked.CompareExchange(ref _timer, new Timer(TimerCallback, null, _dueTime, _msInterval), null);
                if (_lockStrategy == null)
                {
                    _lockStrategy = new MonitorLockStrategy();
                }
                if (_listenerQueue == null)
                {
                    _listenerQueue = new List<PlanConfig>();
                }
            }
        }

        private static void TimerCallback(object state)
        {
            if (_isRunning == 1)
            {
                TraceLog.ReleaseWrite("TimerCallback is busy, The other timer is running.");
                return;
            }
            Interlocked.Exchange(ref _isRunning, 1);
            var tempList = new PlanConfig[0];
            _lockStrategy.TryEnterLock(() =>
            {
                tempList = new PlanConfig[_listenerQueue.Count];
                _listenerQueue.CopyTo(tempList, 0);
            });
            var expiredList = new Queue<PlanConfig>();
            foreach (var planConfig in tempList)
            {
                if (planConfig == null || planConfig.IsExpired)
                {
                    if (planConfig != null)
                    {
                        expiredList.Enqueue(planConfig);
                    }
                    continue;
                }
                if (planConfig.AutoStart())
                {
                    DoNotify(planConfig);
                }
            }
            _lockStrategy.TryEnterLock(() =>
            {
                while (expiredList.Count > 0)
                {
                    var p = expiredList.Dequeue();
                    _listenerQueue.Remove(p);

                    TraceLog.ReleaseWrite("{0}-{1}>>listener is remove ({2}).", DateTime.Now.ToLongTimeString(), p.Name, _listenerQueue.Count);
                }
            });
            Interlocked.Exchange(ref _isRunning, 0);
        }

        private static void DoNotify(PlanConfig planConfig)
        {
            try
            {
                if (planConfig != null && planConfig.Callback != null)
                {
                    //Console.WriteLine("{0}-{1}>>listener begin callback", DateTime.Now.ToLongTimeString(), planConfig.Name);
                    planConfig.Callback.BeginInvoke(planConfig, EndPlanAsync, planConfig);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("TimeListener notify error:{0}", ex);
            }

        }

        private static void EndPlanAsync(IAsyncResult ar)
        {
            //if (ar.AsyncState is PlanConfig)
            //{
            //    var planConfig = (PlanConfig)ar.AsyncState;
            //    Console.WriteLine("{0}-{1}>>listener end callback", DateTime.Now.ToLongTimeString(), planConfig.Name);
            //    Console.WriteLine("");
            //}
        }
    }
}
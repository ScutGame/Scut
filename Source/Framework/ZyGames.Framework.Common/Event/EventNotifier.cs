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
using System.Threading;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Event
{
    /// <summary>
    /// Event notifier
    /// </summary>
    public abstract class EventNotifier
    {
        private static readonly object syncRoot = new object();
        private static readonly ConcurrentQueue<NotifyEventArgs> _handlePools;
        private static readonly List<NotifyEventArgs> _asyncHandlePools;
        private static ManualResetEvent singal = new ManualResetEvent(false);
        private static Thread queueProcessThread;
        private static int _runningQueue;
        private static int _asyncQueue;
        private static Timer _notifyTimer;
        private static long _timerNum;

        /// <summary>
        /// process time
        /// </summary>
        public static long TimerNum
        {
            get { return _timerNum; }
        }

        static EventNotifier()
        {
            _handlePools = new ConcurrentQueue<NotifyEventArgs>();
            _asyncHandlePools = new List<NotifyEventArgs>();
            Interlocked.Exchange(ref _runningQueue, 1);
            queueProcessThread = new Thread(ProcessQueue);
            queueProcessThread.Start();
            _notifyTimer = new Timer(OnNotifyCallback, null, 100, 100);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public static void Dispose()
        {
            try
            {
                Interlocked.Exchange(ref _runningQueue, 0);
                _notifyTimer.Dispose();
                queueProcessThread.Abort();
            }
            catch
            { }
        }
        /// <summary>
        /// Not process time
        /// </summary>
        public static int WaitEventCount
        {
            get { return _asyncHandlePools.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int ActiveThreadCount
        {
            get
            {
                int n;
                int m;
                ThreadPool.GetAvailableThreads(out n, out m);
                return n;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static void Put(NotifyEventArgs handle)
        {
            _handlePools.Enqueue(handle);
            singal.Set();
        }

        private static void ProcessQueue()
        {
            try
            {
                while (_runningQueue == 1)
                {
                    singal.WaitOne();
                    while (_runningQueue == 1)
                    {
                        try
                        {
                            NotifyEventArgs handle;
                            if (_handlePools.TryDequeue(out handle))
                            {
                                var e = handle;
                                if (handle.Interrupt)
                                {
                                    continue;
                                }
                                if (handle.Check())
                                {
                                    ThreadPool.QueueUserWorkItem(obj => e.OnCallback());
                                    OnTimerNum();
                                }
                                else
                                {
                                    lock (syncRoot)
                                    {
                                        _asyncHandlePools.InsertSort(handle, (x, y) => x.ExpiredTime.CompareTo(y.ExpiredTime));
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            TraceLog.WriteError("ProcessQueue Event Notifier error:{0}", ex);
                        }
                    }
                    //if (isInStopping) break;
                    singal.Reset();
                }
            }
            catch (Exception er)
            {
                TraceLog.WriteError("ProcessQueue Event Notifier error:{0}", er);
            }
        }

        private static void OnTimerNum()
        {
            if (_timerNum < 0)
            {
                Interlocked.Exchange(ref _timerNum, 0);
            }
            Interlocked.Increment(ref _timerNum);
        }

        private static void OnNotifyCallback(object state)
        {
            try
            {
                if (Interlocked.CompareExchange(ref _asyncQueue, 1, 0) == 0)
                {
                    List<NotifyEventArgs> removeList = new List<NotifyEventArgs>();

                    lock (syncRoot)
                    {
                        foreach (var handle in _asyncHandlePools)
                        {
                            var e = handle;
                            if (handle.Interrupt)
                            {
                                removeList.Add(handle);
                                continue;
                            }
                            if (handle.Check())
                            {
                                ThreadPool.QueueUserWorkItem(obj => e.OnCallback());
                                OnTimerNum();
                                removeList.Add(handle);
                            }
                        }
                        foreach (var args in removeList)
                        {
                            _asyncHandlePools.Remove(args);
                        }
                    }

                }
            }
            catch (Exception er)
            {
                TraceLog.WriteError("ProcessQueue async event notifier error:{0}", er);
            }
            finally
            {
                Interlocked.Exchange(ref _asyncQueue, 0);
            }
        }

    }
}

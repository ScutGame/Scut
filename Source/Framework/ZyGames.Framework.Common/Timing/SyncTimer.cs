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
using System.Threading;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Timing
{
    /// <summary>
    /// Sync timer
    /// </summary>
    public class SyncTimer
    {
        private readonly TimerCallback _callback;
        private Timer _timer;
        private int isInTimer;
        private Thread _executeThread;
        private Timer _executeTimer;
        /// <summary>
        /// 
        /// </summary>
        public int DueTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ExecuteTimeout { get; set; }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="dueTime"></param>
        /// <param name="period"></param>
        /// <param name="executeTimeout"></param>
        public SyncTimer(TimerCallback callback, int dueTime, int period, int executeTimeout = 60000)
        {
            _callback = callback;
            DueTime = dueTime;
            Period = period;
            ExecuteTimeout = executeTimeout;
        }

        /// <summary>
        /// Start
        /// </summary>
        public virtual void Start()
        {
            _timer = new Timer(InternalDoWork, null, DueTime, Period);
            _executeTimer = new Timer(AbortExecute, null, Timeout.Infinite, Timeout.Infinite);
        }
        /// <summary>
        /// Stop
        /// </summary>
        public virtual void Stop()
        {
            _timer.Dispose();
            while (Interlocked.CompareExchange(ref isInTimer, 1, 0) == 1) Thread.Sleep(10);
        }

        private void AbortExecute(object state)
        {
            try
            {
                if (isInTimer == 1)
                {
                    var myExecuteThread = Interlocked.Exchange<Thread>(ref _executeThread, null);
                    if (myExecuteThread != null)
                    {
                        myExecuteThread.Abort();
                    }
                    isInTimer = 0;
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SyncTimer execute timeout error:{0}", ex);
            }
        }
        private void InternalDoWork(object state)
        {
            if (Interlocked.CompareExchange(ref isInTimer, 1, 0) == 1)
            {
                return;
            }
            try
            {
                _executeThread = Thread.CurrentThread;
                _executeTimer.Change(ExecuteTimeout, Timeout.Infinite);
                _callback(state);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SyncTimer execute error:{0}", ex);
            }
            finally
            {
                _executeThread = null;
                _executeTimer.Change(Timeout.Infinite, Timeout.Infinite);
                Interlocked.Exchange(ref isInTimer, 0);
            }
        }

    }
}

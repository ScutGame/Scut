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
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Timing
{
    /// <summary>
    /// 任务调度类
    /// </summary>
    public class TaskDispatch
    {
        private static object threadLock = new object();
        private static TaskDispatch _TaskDispatch = null;

        /// <summary>
        /// 
        /// </summary>
        public bool Running
        {
            get { return _running; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TaskDispatch StartTask()
        {
            if (_TaskDispatch == null)
            {
                lock (threadLock)
                {
                    if (_TaskDispatch == null)
                    {
                        _TaskDispatch = new TaskDispatch();
                    }
                }
            }

            _TaskDispatch.Start();

            return _TaskDispatch;
        }

        private Timer thread;
        private long intevalTicks = 100;
        private bool _running;
        private List<BaseTask> taskList = new List<BaseTask>();

        private TaskDispatch()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void Add(BaseTask task)
        {
            taskList.Add(task);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            taskList.Clear();
        }

        private void ThreadProcess(object state)
        {
            try
            {
                if (Monitor.TryEnter(threadLock, 100))
                {
                    try
                    {
                        long currTime = DateTime.Now.Ticks;
                        foreach (BaseTask task in taskList)
                        {
                            if (task == null) continue;
                            bool isRun = task.Running;
                            if (isRun || currTime < task.NextTriggerTime)
                            {
                                continue;
                            }
                            DateTime timing = task.GetTiming();
                            if (timing > DateTime.MinValue)
                            {
                                //定时在几点运行
                                TimeSpan tt = DateTime.Now - timing;
                                if (tt.TotalMilliseconds < 0 || tt.TotalMilliseconds >= (double)task.Interval)
                                {
                                    continue;
                                }
                            }
                            task.SetNextTrigger(currTime + task.IntevalTicks);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(task.Proccess), task.TaskName);
                        }
                    }
                    finally
                    {
                        Monitor.Exit(threadLock);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("TaskDispatch process error:{0}", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (thread == null)
            {
                thread = new Timer(ThreadProcess, null, 100, intevalTicks);
                _running = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            thread.Change(-1, Timeout.Infinite);
            _running = false;
        }
        /// <summary>
        /// 
        /// </summary>
        ~TaskDispatch()
        {
            _running = false;
            thread.Dispose();
        }
    }
}
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

        private Thread thread;
        private long clockFrequency;
        private long intevalTicks;
        private long nextTriggerTime;
        private bool running = false;
        private List<BaseTask> taskList = new List<BaseTask>();
        /// <summary>
        /// 计数器
        /// </summary>
        /// <param name="lpPerformanceCount"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        /// <summary>
        /// 频率次数
        /// </summary>
        /// <param name="lpFrequency"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out  long lpFrequency);

        private static bool GetTick(out long currentTickCount)
        {
            if (QueryPerformanceCounter(out currentTickCount) == false)
                throw new Win32Exception("任务计数器异常!");
            else
                return true;
        }

        private TaskDispatch()
        {
            if (QueryPerformanceFrequency(out clockFrequency) == false)
            {
                throw new Win32Exception("QueryPerformanceFrequency方法不支持!");
            }

            thread = new Thread(new ThreadStart(ThreadProcess));
            thread.Name = "TaskDispatch";
            //thread.Priority = ThreadPriority.Highest;
            Interval = 1000;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void Add(BaseTask task)
        {
            lock (threadLock)
            {
                task.IntevalTicks = GetIntevalTicks(task.Interval);
                taskList.Add(task);
            }
        }

        private int intervalMs;

        /// <summary>
        /// 间隔时间(毫秒)
        /// </summary>
        public int Interval
        {
            get { return intervalMs; }
            set
            {
                intervalMs = value;
                intevalTicks = GetIntevalTicks(value);
            }
        }

        private long GetIntevalTicks(int value)
        {
            return (long)((double)value * (double)clockFrequency / (double)1000);
        }


        private void ThreadProcess()
        {
            long currTime;
            GetTick(out currTime);
            nextTriggerTime = currTime + intevalTicks;
            while (running)
            {
                while (currTime < nextTriggerTime)
                {
                    Thread.Sleep((int)(Interval / 4));
                    GetTick(out currTime);
                }
                nextTriggerTime = currTime + intevalTicks;

                lock (threadLock)
                {
                    //Console.WriteLine("任务主线程正在执行中...");
                    //Loger.SaveLog("任务主线程正在执行中...");

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
                            TimeSpan tt = DateTime.Now - timing;
                            if (tt.TotalMilliseconds < 0 || tt.TotalMilliseconds >= (double)task.Interval)
                            {
                                continue;
                            }
                            else
                            {

                            }
                        }
                        task.SetNextTrigger(currTime + task.IntevalTicks);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(task.Proccess), task.TaskName);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            //Console.WriteLine("任务主线程启动!");
            //Loger.SaveLog("任务主线程启动!");
            lock (threadLock)
            {
                if (!running)
                {
                    running = true;
                }
            }
            thread.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            lock (threadLock)
            {
                running = false;
            }
            //Console.WriteLine("任务主线程停止!");
            //Loger.SaveLog("任务主线程停止!");
        }
        /// <summary>
        /// 
        /// </summary>
        ~TaskDispatch()
        {
            lock (threadLock)
            {
                running = false;
            }
            thread.Abort();
            //Console.WriteLine("任务主线程关闭!");
            //Loger.SaveLog("任务主线程关闭!");
        }
    }
}
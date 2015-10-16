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
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Timing
{
    /// <summary>
    /// 任务接口
    /// </summary>
    public abstract class BaseTask
    {
        private object threadLock = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        protected BaseTask(int interval)
        {
            Running = false;
            IntevalTicks = new TimeSpan(0, 0, 0, 0, interval).Ticks;
            Interval = 1000;
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaskName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Running
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Timing
        {
            get;
            set;
        }

        /// <summary>
        /// 间隔时间(毫秒)
        /// </summary>
        public int Interval
        {
            get;
            set;
        }

        /// <summary>
        /// 间隔刻度
        /// </summary>
        public long IntevalTicks
        {
            get;
            set;
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public long NextTriggerTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 更新下次执行时间
        /// </summary>
        /// <param name="nextTrigger"></param>
        public void SetNextTrigger(long nextTrigger)
        {
            lock (threadLock)
            {
                NextTriggerTime = nextTrigger;
            }
        }

        /// <summary>
        /// 获取定时时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetTiming()
        {
            DateTime time = DateTime.MinValue;
            if (DateTime.TryParse(Timing, out time))
            {
            }
            return time;
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="obj"></param>
        public void Proccess(object obj)
        {
            try
            {
                lock (threadLock)
                {
                    Running = true;
                    DoExecute(obj);
                    Running = false;
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("任务[" + TaskName + "]出错", ex.ToString());
            }
        }

        /// <summary>
        /// 执行任务处理
        /// </summary>
        /// <param name="obj"></param>
        protected abstract void DoExecute(object obj);

    }
}
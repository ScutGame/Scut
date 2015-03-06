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
using System.Diagnostics;

namespace ZyGames.Framework.Common.Timing
{
    /// <summary>
    /// 计划周期
    /// </summary>
    public enum PlanCycle
    {
        /// <summary>
        /// 
        /// </summary>
        Once,
        /// <summary>
        /// 
        /// </summary>
        Day,
        /// <summary>
        /// 
        /// </summary>
        Week
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="planConfig"></param>
    public delegate void PlanCallback(PlanConfig planConfig);

    /// <summary>
    /// 计划配置信息
    /// </summary>
    public class PlanConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PlanConfig OncePlan(PlanCallback callback, string name, string beginTime)
        {
            return CreatePlan(callback, name, PlanCycle.Once, beginTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PlanConfig EveryMinutePlan(PlanCallback callback, string name, string beginTime, string endTime, int secondInterval = 60)
        {
            return CreatePlan(callback, name, PlanCycle.Day, beginTime, endTime, true, secondInterval);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PlanConfig EveryDayPlan(PlanCallback callback, string name, string beginTime)
        {
            return CreatePlan(callback, name, PlanCycle.Day, beginTime, "23:59:59", true, (int)new TimeSpan(1, 0, 0, 0).TotalSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PlanConfig EveryWeekPlan(PlanCallback callback, string name, DayOfWeek week, string beginTime)
        {
            var plan = CreatePlan(callback, name, PlanCycle.Week, beginTime);
            plan.PlanWeek = week;
            return plan;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PlanConfig EveryWeekPlan(PlanCallback callback, string name, DayOfWeek week, string beginTime, string endTime, int secondInterval = 60)
        {
            var plan = CreatePlan(callback, name, PlanCycle.Week, beginTime, endTime, true, secondInterval);
            plan.PlanWeek = week;
            return plan;
        }

        private static PlanConfig CreatePlan(PlanCallback callback, string name, PlanCycle planCycle, string beginTime, string endTime = "", bool isCycle = false, int secondInterval = 0)
        {
            return new PlanConfig()
            {
                Callback = callback,
                Name = name,
                PlanCycle = planCycle,
                BeginTime = beginTime,
                EndTime = endTime,
                IsCycle = isCycle,
                SecondInterval = secondInterval
            };
        }

        class PlanState
        {
            public double OffsetSecond { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime StopDate { get; set; }
            public DateTime PreDate { get; private set; }

            /// <summary>
            /// 是否过期,过期的计划将被移除定时执行任务
            /// </summary>
            public bool IsExpired { get; set; }
            /// <summary>
            /// 是否当天结束
            /// </summary>
            public bool IsEnd { get; private set; }

            public void Reset(PlanConfig config)
            {
                StartDate = (string.IsNullOrEmpty(config.BeginTime) ? "00:00:00" : config.BeginTime).ToDateTime();
                StopDate = config.EndTime.ToDateTime(DateTime.MinValue);
                IsEnd = false;
            }

            public bool IsTriggerStop(DateTime currDate)
            {
                if (StopDate > DateTime.MinValue && currDate >= StopDate)
                {
                    IsEnd = true;
                }
                return false;
            }

            public bool IsTriggerStart(DateTime currDate)
            {
                if (currDate >= StartDate && currDate < StartDate.AddSeconds(OffsetSecond))
                {
                    PreDate = currDate;
                    IsEnd = true;
                    return true;
                }
                return false;
            }

            public bool IsTrigger(DateTime currDate, int secondInterval)
            {
                if (currDate < StartDate || currDate >= StopDate || secondInterval <= 0)
                {
                    return false;
                }

                bool result = false;
                var excuteDate = PreDate == DateTime.MinValue ? StartDate : PreDate.AddSeconds(secondInterval);
                var nextExcuteDate = excuteDate.AddSeconds(secondInterval);
                if (currDate < PreDate || currDate >= nextExcuteDate)
                {
                    //修正当前时间位置
                    var ts = StopDate - StartDate;
                    var numberTimes = (int)ts.TotalSeconds / secondInterval;
                    int times = FindIntervalTimes(currDate, 0, numberTimes, secondInterval);
                    PreDate = StartDate.AddSeconds(secondInterval * (times - 1));

                    excuteDate = PreDate.AddSeconds(secondInterval);
                    nextExcuteDate = excuteDate.AddSeconds(secondInterval);
                }
                //Trace.WriteLine(string.Format("timing pre:{0}, cur:{1}, next:{2}", PreDate, currDate, excuteDate));
                if (PreDate <= currDate && currDate < excuteDate)
                {
                    //时间还没有到
                }
                else if (excuteDate <= currDate && currDate < nextExcuteDate)
                {
                    PreDate = excuteDate;
                    result = currDate < excuteDate.AddSeconds(OffsetSecond);//整点处理
                }
                else
                {
                }
                return result;
            }

            private int FindIntervalTimes(DateTime currDate, int startIndex, int endIndex, int secondInterval)
            {
                int index = -1;

                while (endIndex >= startIndex)
                {
                    int middle = (startIndex + endIndex) / 2;
                    int nextMiddle = middle + 1;
                    var value = StartDate.AddSeconds(middle * secondInterval);
                    var nextValue = StartDate.AddSeconds(nextMiddle * secondInterval);

                    int result = value.CompareTo(currDate);
                    if (result <= 0 && (nextValue.CompareTo(currDate) > 0))
                    {
                        index = result == 0 ? middle : middle + 1;
                        break;
                    }
                    else if (result > 0)
                    {
                        endIndex = middle - 1;
                    }
                    else
                    {
                        startIndex = middle + 1;
                    }
                }
                return index;
            }
        }

        private PlanState _planState = new PlanState();

        private PlanConfig()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public DayOfWeek PlanWeek { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PlanCycle PlanCycle { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 回调委托
        /// </summary>
        public PlanCallback Callback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// 是否循环（每天）
        /// </summary>
        public bool IsCycle { get; set; }
        /// <summary>
        /// 间隔秒，0:为单次
        /// </summary>
        public int SecondInterval { get; set; }

        /// <summary>
        /// Expired time
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExpired { get { return _planState.IsExpired; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnd { get { return _planState.IsEnd; } }

        /// <summary>
        /// 
        /// </summary>
        public DateTime PreviousExecuteTime { get { return _planState.PreDate; } }

        /// <summary>
        /// 自动开始
        /// </summary>
        /// <returns></returns>
        public bool AutoStart(DateTime currDate)
        {
            if (!DateTime.Now.Date.Equals(_planState.StartDate.Date))
            {
                _planState.Reset(this);
            }
            if (_planState.IsExpired || _planState.IsEnd)
            {
                return false;
            }

            bool isStart = false;

            switch (PlanCycle)
            {
                case PlanCycle.Once:
                    isStart = _planState.IsTriggerStart(currDate);
                    break;
                case PlanCycle.Day:
                    isStart = !IsCycle ? _planState.IsTriggerStart(currDate) : _planState.IsTrigger(currDate, SecondInterval);
                    break;
                case PlanCycle.Week:
                    if (PlanWeek == currDate.DayOfWeek)
                    {
                        isStart = !IsCycle ? _planState.IsTriggerStart(currDate) : _planState.IsTrigger(currDate, SecondInterval);
                    }
                    break;
                default:
                    break;
            }
            _planState.IsTriggerStop(currDate);
            if ((PlanCycle == PlanCycle.Once && _planState.IsEnd) ||
                (ExpiredTime > DateTime.MinValue && currDate >= ExpiredTime))
            {
                _planState.IsExpired = true;
            }

            return isStart;

        }

        /// <summary>
        /// 设置误差间隔
        /// </summary>
        public void SetDiffInterval(double secondInterval)
        {
            _planState.OffsetSecond = secondInterval;
        }
    }
}
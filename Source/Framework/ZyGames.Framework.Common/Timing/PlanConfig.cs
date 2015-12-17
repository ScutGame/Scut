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
using ZyGames.Framework.Common.Log;

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
    /// Plan config
    /// </summary>
    public class PlanConfig
    {
        /// <summary>
        /// Only run once
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="name"></param>
        /// <param name="beginTime">equal this time, format:"2015/01/01 09:00:00", or "09:00:00"  is today time</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static PlanConfig OncePlan(PlanCallback callback, string name, string beginTime, object target = null)
        {
            return CreatePlan(callback, name, PlanCycle.Once, beginTime, null, false, 0, target);
        }

        /// <summary>
        /// Every minute run
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="name"></param>
        /// <param name="beginTime">more than or equal this time, format: "2015/01/01 09:00:00", or "09:00:00" is today time</param>
        /// <param name="endTime">less than this time, format: "2015/01/02 00:00:00", or "00:00:00" or "null" is tomorrow time</param>
        /// <param name="secondInterval">interval run times</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static PlanConfig EveryMinutePlan(PlanCallback callback, string name, string beginTime, string endTime, int secondInterval = 60, object target = null)
        {
            return CreatePlan(callback, name, PlanCycle.Day, beginTime, endTime, true, secondInterval, target);
        }

        /// <summary>
        /// Every day run
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="name"></param>
        /// <param name="beginTime">more than or equal this time, format: "2015/01/01 09:00:00", or "09:00:00" is today time</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static PlanConfig EveryDayPlan(PlanCallback callback, string name, string beginTime, object target = null)
        {
            return CreatePlan(callback, name, PlanCycle.Day, beginTime, null, true, (int)new TimeSpan(1, 0, 0, 0).TotalSeconds, target);
        }

        /// <summary>
        /// Every week run
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="name"></param>
        /// <param name="week">DayOfWeek</param>
        /// <param name="beginTime">more than or equal this time, format: "2015/01/01 09:00:00", or "09:00:00" is today time</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static PlanConfig EveryWeekPlan(PlanCallback callback, string name, DayOfWeek week, string beginTime, object target = null)
        {
            var plan = CreatePlan(callback, name, PlanCycle.Week, beginTime, null, false, 0, target);
            plan.PlanWeek = week;
            return plan;
        }

        /// <summary>
        /// Every week run
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="name"></param>
        /// <param name="week">DayOfWeek</param>
        /// <param name="beginTime">more than or equal this time, format: "2015/01/01 09:00:00", or "09:00:00" is today time</param>
        /// <param name="endTime">less than this time, format: "2015/01/02 00:00:00", or "00:00:00" or "null" is tomorrow time</param>
        /// <param name="secondInterval">interval run times</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static PlanConfig EveryWeekPlan(PlanCallback callback, string name, DayOfWeek week, string beginTime, string endTime, int secondInterval = 60, object target = null)
        {
            var plan = CreatePlan(callback, name, PlanCycle.Week, beginTime, endTime, true, secondInterval, target);
            plan.PlanWeek = week;
            return plan;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="name"></param>
        /// <param name="planCycle">PlanCycle</param>
        /// <param name="beginTime">more than or equal this time, format: "2015/01/01 09:00:00", or "09:00:00" is today time</param>
        /// <param name="endTime">less than this time, format: "2015/01/02 00:00:00", or "00:00:00" or "null" is tomorrow time</param>
        /// <param name="isCycle">is cycle run</param>
        /// <param name="secondInterval">interval run times</param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static PlanConfig CreatePlan(PlanCallback callback, string name, PlanCycle planCycle, string beginTime, string endTime, bool isCycle, int secondInterval, object target)
        {
            return new PlanConfig()
            {
                Callback = callback,
                Name = name,
                PlanCycle = planCycle,
                BeginTime = beginTime,
                EndTime = endTime ?? string.Empty,
                IsCycle = isCycle,
                SecondInterval = secondInterval,
                Target = target
            };
        }

        class PlanState
        {
            private readonly PlanConfig _config;

            public PlanState(PlanConfig config)
            {
                _config = config;
            }

            public DateTime StartDate { get; set; }
            public DateTime StopDate { get; set; }
            public DateTime PreExcuteDate { get; private set; }

            /// <summary>
            /// 是否过期,过期的计划将被移除定时执行任务
            /// </summary>
            public bool IsExpired { get; set; }
            /// <summary>
            /// 是否当天结束
            /// </summary>
            public bool IsEnd { get; private set; }

            public void Reset()
            {
                PreExcuteDate = DateTime.MinValue;
                StartDate = (string.IsNullOrEmpty(_config.BeginTime) ? "00:00:00" : _config.BeginTime).ToDateTime();
                StopDate = string.IsNullOrEmpty(_config.EndTime) || "00:00:00".Equals(_config.EndTime) ? DateTime.Now.Date.AddDays(1) : _config.EndTime.ToDateTime(DateTime.MinValue);
                IsEnd = false;
            }

            public bool IsTriggerStop(DateTime currDate)
            {
                if (StopDate > DateTime.MinValue && currDate > StopDate)
                {
                    IsEnd = true;
                    return true;
                }
                return false;
            }

            public bool IsTriggerStart(DateTime currDate)
            {
                if (currDate >= StartDate && currDate < StartDate.AddMilliseconds(TimeListener.OffsetMillisecond))
                {
                    PreExcuteDate = StartDate;
                    IsEnd = false;
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
                var excuteDate = PreExcuteDate == DateTime.MinValue ? StartDate : PreExcuteDate.AddSeconds(secondInterval);
                var nextExcuteDate = excuteDate.AddSeconds(secondInterval);
                //string info = string.Empty;
                if (currDate < PreExcuteDate || currDate >= nextExcuteDate)
                {
                    //修正当前时间位置
                    var ts = StopDate - StartDate;
                    var numberTimes = (int)ts.TotalSeconds / secondInterval;
                    int times = FindIntervalTimes(currDate, 0, numberTimes, secondInterval);
                    PreExcuteDate = StartDate.AddSeconds(secondInterval * (times - 1));
                    excuteDate = PreExcuteDate.AddSeconds(secondInterval);
                    nextExcuteDate = excuteDate.AddSeconds(secondInterval);
                }
                //info = string.Format("{0}>>timing pre:{1}, cur:{2}, next:{3}", _config.Name, PreExcuteDate, excuteDate, nextExcuteDate);
                if (excuteDate <= currDate && currDate < nextExcuteDate)
                {
                    PreExcuteDate = excuteDate;
                    result = currDate < excuteDate.AddMilliseconds(TimeListener.OffsetMillisecond);//整点处理
                }
                //if (result)
                //{
                //    TraceLog.WriteInfo(info);
                //}
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

        private PlanState _planState;
        internal int _isExcuting;


        private PlanConfig()
        {
            _planState = new PlanState(this);
        }

        /// <summary>
        /// Gets plan is excuting num.
        /// </summary>
        public int ExcutingNum { get { return _isExcuting; } }
        /// <summary>
        /// 
        /// </summary>
        internal DayOfWeek PlanWeek { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        internal string BeginTime { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        internal string EndTime { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        internal PlanCycle PlanCycle { get; private set; }

        /// <summary>
        /// Gets plan name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 回调委托
        /// </summary>
        internal PlanCallback Callback { get; private set; }

        /// <summary>
        /// Gets or sets target
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// 是否循环（每天）
        /// </summary>
        internal bool IsCycle { get; private set; }
        /// <summary>
        /// 间隔秒，0:为单次
        /// </summary>
        internal int SecondInterval { get; private set; }

        /// <summary>
        /// Gets or sets expired time
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        /// <summary>
        /// Gets expired exit run.
        /// </summary>
        public bool IsExpired { get { return _planState.IsExpired; } }

        /// <summary>
        /// Gets today is end run
        /// </summary>
        public bool IsEnd { get { return _planState.IsEnd; } }

        /// <summary>
        /// Gets pre excute time
        /// </summary>
        public DateTime PreviousExecuteTime { get { return _planState.PreExcuteDate; } }
        /// <summary>
        /// Gets next excute time
        /// </summary>
        public DateTime NexExcutetDate
        {
            get
            {
                return _planState.IsExpired ? DateTime.MinValue : _planState.PreExcuteDate == DateTime.MinValue
                    ? _planState.StartDate
                    : _planState.IsEnd
                        ? _planState.StartDate.AddDays(1)
                        : _planState.PreExcuteDate.AddSeconds(SecondInterval);
            }
        }

        /// <summary>
        /// 自动开始
        /// </summary>
        /// <returns></returns>
        internal bool AutoStart(DateTime currDate)
        {
            if (!DateTime.Now.Date.Equals(_planState.StartDate.Date))
            {
                _planState.Reset();
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

    }
}
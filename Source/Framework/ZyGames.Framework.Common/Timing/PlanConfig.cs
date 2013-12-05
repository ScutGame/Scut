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

namespace ZyGames.Framework.Common.Timing
{
    /// <summary>
    /// 计划配置信息
    /// </summary>
    public class PlanConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="planConfig"></param>
        public delegate void PlanCallback(PlanConfig planConfig);

        /// <summary>
        /// 计划周期
        /// </summary>
        private enum PlanCycle
        {
            Day,
            Week
        }

        private DateTime _beginDate;
        private DateTime _endDate;
        private int _currentTimes;

        private readonly DayOfWeek _week;
        private readonly string _beginTime;
        private readonly string _endTime;
        private readonly PlanCycle _planCycle;

        /// <summary>
        /// 第几周
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="week"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="secondInterval"></param>
        /// <param name="isCycle"></param>
        /// <param name="name"></param>
        public PlanConfig(PlanCallback callback, DayOfWeek week, string beginTime, string endTime, bool isCycle = true, int secondInterval = 0, string name = "")
            : this(callback, PlanCycle.Week, beginTime, endTime, isCycle, secondInterval, name)
        {
            _week = week;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="isCycle"></param>
        /// <param name="secondInterval"></param>
        /// <param name="name"></param>
        public PlanConfig(PlanCallback callback, bool isCycle, int secondInterval, string name = "")
            : this(callback, PlanCycle.Day, "00:00", null, isCycle, secondInterval, name)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="isCycle"></param>
        /// <param name="secondInterval"></param>
        /// <param name="name"></param>
        public PlanConfig(PlanCallback callback, string beginTime, string endTime, bool isCycle = false, int secondInterval = 0, string name = "")
            : this(callback, PlanCycle.Day, beginTime, endTime, isCycle, secondInterval, name)
        {
        }

        private PlanConfig(PlanCallback callback, PlanCycle planCycle, string beginTime, string endTime, bool isCycle, int secondInterval, string name)
        {
            Callback = callback;
            _planCycle = planCycle;
            _beginTime = beginTime;
            Name = name;
            _endTime = string.IsNullOrEmpty(endTime) ? "23:59:59" : endTime;
            IsCycle = isCycle;
            SecondInterval = secondInterval > 0 ? secondInterval : 0;

            _beginDate = _beginTime.ToDateTime();
            _endDate = _endTime.ToDateTime();
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 回调委托
        /// </summary>
        public PlanCallback Callback
        {
            get;
            set;
        }
        /// <summary>
        /// 目标对象
        /// </summary>
        public object Target
        {
            get;
            set;
        }

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool IsCycle
        {
            get;
            private set;
        }
        /// <summary>
        /// 是否过期,过期的计划将被移除定时执行任务
        /// </summary>
        public bool IsExpired
        {
            get;
            private set;
        }
        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsEnd
        {
            get;
            private set;
        }
        /// <summary>
        /// 间隔秒，0:为单次
        /// </summary>
        public int SecondInterval
        {
            get;
            private set;
        }
        /// <summary>
        /// 自动开始
        /// </summary>
        /// <returns></returns>
        public bool AutoStart()
        {
            ResetDate();
            if (IsExpired || (IsCycle && IsEnd))
            {
                return false;
            }
            DateTime currDate = DateTime.Now;
            if (_planCycle == PlanCycle.Week && currDate.DayOfWeek != _week)
            {
                return false;
            }
            if ((!IsCycle && SecondInterval == 0 && _currentTimes > 0) ||
                !IsCycle && IsEnd)
            {
                IsExpired = true;
                return false;
            }
            bool isStart = false;
            if ((SecondInterval == 0 && _currentTimes == 0 && currDate > _beginDate) ||
                (currDate >= _beginDate.AddSeconds(SecondInterval * _currentTimes) && currDate < _endDate))
            {
                //修正时间
                if (_currentTimes == 0 && _beginDate < currDate)
                {
                    _beginDate = currDate;
                }
                _currentTimes++;
                isStart = true;
            }
            else if (currDate > _endDate)
            {
                IsEnd = true;
            }
            return isStart;
        }

        /// <summary>
        /// 重置日期
        /// </summary>
        public void ResetDate()
        {
            if (!IsExpired && IsCycle && !DateTime.Now.Date.Equals(_beginDate.Date))
            {
                _beginDate = _beginTime.ToDateTime();
                _endDate = _endTime.ToDateTime();
                _currentTimes = 0;
                IsEnd = false;
            }
        }

        /// <summary>
        /// 设置误差间隔
        /// </summary>
        public void SetDiffInterval(int secondInterval)
        {
            _endDate = _endDate.AddSeconds(secondInterval);
        }
    }
}
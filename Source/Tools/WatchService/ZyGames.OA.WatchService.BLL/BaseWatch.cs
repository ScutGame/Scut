using System;
using System.Net;
using ZyGames.GameService.BaseService.LogService;
using ZyGames.OA.WatchService.BLL.Tools;

namespace ZyGames.OA.WatchService.BLL
{
    public abstract class BaseWatch
    {
        protected static BaseLog Logger = new BaseLog("BaseWatch");
        private int _currInterval = 0;

        public BaseWatch() : this(string.Empty)
        {
        }

        public BaseWatch(string timing)
        {
            IsRun = false;
            IsForLoop = true;
            TimeSpan ts;
            if (!string.IsNullOrEmpty(timing) && TimeSpan.TryParse(timing, out ts))
            {
                Timing = ts;
            }
            else
            {
                Timing = TimeSpan.Zero;
            }
        }
        /// <summary>
        /// 是否循环执行,默认为True
        /// </summary>
        public bool IsForLoop { get; set; }
        /// <summary>
        /// 定时时间
        /// </summary>
        public TimeSpan Timing
        {
            get;
            protected set;
        }
        /// <summary>
        /// 间隔时间(毫秒)
        /// </summary>
        public int Interval
        {
            get;
            protected set;
        }

        public bool IsRun { get; set; }

        public void Process(object obj)
        {
            IsRun = true;
            DoProcess(obj);

            IsRun = false;
        }

        public bool IsElapsed(int interval)
        {
            if (Interval <= 0)
            {
                TimeSpan ts = DateTime.Now.TimeOfDay;
                TimeSpan nextTs = Timing.Add(TimeSpan.FromMilliseconds(interval));

                //Logger.SaveLog(string.Format("Interval:{0},Timing:{1},result:{2}[{4}]/{3}[{5}]"
                //    , Interval
                //    , Timing.ToString()
                //    , ts.ToString()
                //    , nextTs.ToString()
                //    , ts.CompareTo(Timing)
                //    , ts.CompareTo(nextTs)
                //    ));

                if (ts.CompareTo(Timing) >= 0 && ts.CompareTo(nextTs) < 0)
                {
                    return true;
                }
            }
            else if (_currInterval >= Interval)
            {
                _currInterval = 0;
                return true;
            }
            else
            {
                _currInterval += interval;
                return false;
            }
            return false;
        }

        protected abstract bool DoProcess(object obj);

        public string GetServerIP()
        {
            return OaSimplePlanHelper.GetServerIP();
        }
    }
}

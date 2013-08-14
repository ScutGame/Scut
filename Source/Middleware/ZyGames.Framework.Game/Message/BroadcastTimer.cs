using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Model;

namespace ZyGames.Framework.Game.Message
{
    /// <summary>
    /// 广播定时器
    /// </summary>
    public class BroadcastTimer
    {
        public delegate void BroadcastCallback(NoticeMessage message);

        private int _isRunning;
        private readonly BroadcastCallback _callback;

        public BroadcastTimer(BroadcastCallback callback)
        {
            _callback = callback;
        }

        public void Add(NoticeMessage message, string beginTime, string endTime, bool isCycle, int secondInterval)
        {
            TimeListener.Append(new PlanConfig(DoBroadcast, beginTime, endTime, isCycle, secondInterval) { Name = "BroadcastTimer", Target = message });
        }

        public void Add(NoticeMessage message, DayOfWeek week, string beginTime, string endTime, bool isCycle, int secondInterval)
        {
            TimeListener.Append(new PlanConfig(DoBroadcast, week, beginTime, endTime, isCycle, secondInterval) { Name = "BroadcastTimer", Target = message });
        }

        public bool Remove(Guid messageId)
        {
            return TimeListener.Remove(m =>
             {
                 if (m.Target is NoticeMessage)
                 {
                     var temp = (NoticeMessage)m.Target;
                     return Equals(temp.Id, messageId);
                 }
                 return false;
             });
        }

        private void DoBroadcast(PlanConfig planConfig)
        {
            if (_isRunning == 1) return;
            Interlocked.Exchange(ref _isRunning, 1);
            //TraceLog.ReleaseWrite("{0}>>Broadcast listener start...", DateTime.Now.ToLongTimeString());
            if (planConfig.Target is NoticeMessage)
            {
                NoticeMessage message = planConfig.Target as NoticeMessage;
                if (_callback != null)
                {
                    _callback(message);
                }
            }
            Interlocked.Exchange(ref _isRunning, 0);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;

namespace ZyGames.Framework.Game.Context
{
    /// <summary>
    /// 请求的上下文对象
    /// </summary>
    public class GameContext
    {
        /// <summary>
        /// 超时1s
        /// </summary>
        private const int TimeOut = 1000;
        private readonly static CacheListener Listener;
        private static readonly CacheDictionary<string, GameContext> _contextSet = new CacheDictionary<string, GameContext>();

        static GameContext()
        {
            Listener = new CacheListener("gamecontext_listener", 1800, DoCacheDispose);
            Listener.Start();
        }

        private static void DoCacheDispose(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.Expired)
            {
                _contextSet.Foreach((ckey, context) =>
                {
                    if (context != null && MathUtils.DiffDate(context.ExpireDate).TotalSeconds > 3600)
                    {
                        _contextSet.Remove(ckey);
                    }
                    return true;
                });
            }
        }

        public static GameContext GetInstance(int actionId, int userId, int timeOut = TimeOut)
        {
            string key = string.Format("{0}_{1}", actionId, userId);
            if (!_contextSet.ContainsKey(key))
            {
                _contextSet.Add(key, new GameContext(actionId, userId, timeOut));
            }
            var context = _contextSet[key];
            if (context != null)
            {
                context.ExpireDate = MathUtils.Now;
            }
            return context;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Refresh()
        {
            _contextSet.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="userId"></param>
        public static void Refresh(int actionId, int userId)
        {
            string key = string.Format("{0}_{1}", actionId, userId);
            if (_contextSet.ContainsKey(key))
            {
                int timeOut = _contextSet[key].LockTimeOut;
                _contextSet[key] = new GameContext(actionId, userId, timeOut);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="userId"></param>
        /// <param name="timeOut">超时时间(毫秒)</param>
        private GameContext(int actionId, int userId, int timeOut)
        {
            LockTimeOut = timeOut;
            _monitorLock = new MonitorLockStrategy(timeOut);
            ActionId = actionId;
            UserId = userId;
            IsRequesting = false;
            ExpireDate = MathUtils.Now;
        }

        public int LockTimeOut { get; set; }

        private DateTime ExpireDate { get; set; }

        /// <summary>
        /// 是否正在请求中，避免多次
        /// </summary>
        public bool IsRequesting
        {
            get;
            private set;
        }

        private readonly IMonitorStrategy _monitorLock;
        public IMonitorStrategy MonitorLock
        {
            get { return _monitorLock; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ActionId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetUser<T>() where T : BaseUser
        {
            return (T) User;
        }

        public BaseUser User
        {
            get;
            set;
        }

    }
}

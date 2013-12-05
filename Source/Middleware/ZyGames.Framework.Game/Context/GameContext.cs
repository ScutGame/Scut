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

        private static void DoCacheDispose(string key, object value, CacheRemovedReason reason)
        {
			if (reason == CacheRemovedReason.Expired)
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

        /// <summary>
        /// 获得当前请求上下文
        /// </summary>
        /// <param name="ssid"></param>
        /// <param name="actionId"></param>
        /// <param name="userId"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static GameContext GetInstance(string ssid, int actionId, int userId, int timeOut = TimeOut)
        {
            if (string.IsNullOrEmpty(ssid))
            {
                ssid =  Guid.NewGuid().ToString("N");
            }
            string key = CreateContextKey(ssid, actionId);
            if (!_contextSet.ContainsKey(key))
            {
                _contextSet.Add(key, new GameContext(ssid, actionId, userId, timeOut));
            }
            var context = _contextSet[key];
            if (context != null)
            {
                context.UserId = userId;
                context.ExpireDate = MathUtils.Now;
            }
            return context;
        }

        /// <summary>
        /// 刷新清空
        /// </summary>
        public static void Refresh()
        {
            _contextSet.Clear();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="ssid"></param>
        /// <param name="actionId"></param>
        /// <param name="userId"></param>
        public static void Refresh(string ssid, int actionId, int userId)
        {
            string key = CreateContextKey(ssid, actionId);
            if (_contextSet.ContainsKey(key))
            {
                int timeOut = _contextSet[key].LockTimeOut;
                _contextSet[key] = new GameContext(ssid, actionId, userId, timeOut);
            }
        }

        private static string CreateContextKey(string ssid, int actionId)
        {
            return string.Format("{0}_{1}", ssid, actionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssid"></param>
        /// <param name="actionId"></param>
        /// <param name="userId"></param>
        /// <param name="timeOut">超时时间(毫秒)</param>
        private GameContext(string ssid, int actionId, int userId, int timeOut)
        {
            LockTimeOut = timeOut;
            _monitorLock = new MonitorLockStrategy(timeOut);
            SessionId = ssid;
            ActionId = actionId;
            UserId = userId;
            IsRequesting = false;
            ExpireDate = MathUtils.Now;
        }

        /// <summary>
        /// 锁超时时间
        /// </summary>
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
        /// <summary>
        /// 获得锁策略
        /// </summary>
        public IMonitorStrategy MonitorLock
        {
            get { return _monitorLock; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 当前玩家Id
        /// </summary>
        public int UserId
        {
            get;
            private set;
        }

        /// <summary>
        /// 请求的ActionId
        /// </summary>
        public int ActionId
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前玩家对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetUser<T>() where T : BaseUser
        {
            return (T)User;
        }

        /// <summary>
        /// 当前玩家对象
        /// </summary>
        public BaseUser User
        {
            get;
            set;
        }

        internal void SetValue(int userId)
        {
            UserId = userId;
        }
    }
}
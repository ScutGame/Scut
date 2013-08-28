using System;
using System.Collections.Generic;
using System.Web.Caching;
using ZyGames.Framework.Cache;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Game.Message;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Cache
{
    /// <summary>
    /// 广播缓存
    /// </summary>
    public class BroadcastCacheSet : ShareCacheStruct<NoticeMessage>
    {
        private readonly string _groupKey;
        private readonly static int MessageMaxCount;
        private readonly static int Timeout;

        static BroadcastCacheSet()
        {
            MessageMaxCount = ConfigUtils.GetSetting("broadcastcache_maxcount", "1000").ToInt();
            Timeout = ConfigUtils.GetSetting("broadcastcache_timeout", "1800").ToInt();
        }

        public BroadcastCacheSet()
            : this("10000")
        {
        }

        public BroadcastCacheSet(string groupKey)
        {
            _groupKey = groupKey;
        }

        public NoticeMessage[] GetBroadcast()
        {
            CacheQueue<NoticeMessage> cacheQueue;
            if (DataContainer.TryGetQueue(_groupKey, out cacheQueue))
            {
                return cacheQueue.ToArray();
            }
            return new NoticeMessage[0];
        }

        public void Send(NoticeMessage message)
        {
            if (DataContainer.TryAddQueue(_groupKey, message, 0, OnExpired))
            {
            }
        }

        protected override bool InitCache(List<NoticeMessage> dataList, int periodTime)
        {
            bool result = false;
            foreach (var data in dataList)
            {
                string key = data.GetKeyCode();
                result = DataContainer.TryAddQueue(key, data, periodTime, OnExpired);
                if (!result)
                {
                    TraceLog.WriteError("Load data:\"{0}\" tryadd key:\"{1}\" error.", DataContainer.RootKey, key);
                    return false;
                }
            }
            return result;
        }

        private static bool OnExpired(string groupKey, CacheQueue<NoticeMessage> messageQueue)
        {
            while (messageQueue != null && messageQueue.Count > 0)
            {
                NoticeMessage msg;
                if (messageQueue.TryPeek(out msg))
                {
                    if (msg != null && MathUtils.DiffDate(msg.SendDate).TotalSeconds > Timeout)
                    {
                        NoticeMessage temp;
                        messageQueue.TryDequeue(out temp);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return true;
        }
        /*private static void DoCacheDispose(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.Expired)
            {
                var cacheSet = new BroadcastCacheSet();
                var messageQueue = cacheSet.GetCache();
                while (messageQueue != null && messageQueue.Count > 0)
                {
                    var msg = messageQueue.Peek();
                    if (msg != null && MathUtils.DiffDate(msg.SendDate).TotalSeconds > Timeout)
                    {
                        messageQueue.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }*/

        /*public BroadcastCacheSet()
        {
        }

        protected override bool InitCache()
        {
            var cacheData = GetCacheObject() ?? CreateCacheStruct();
            AddCache(cacheData);
            return true;
        }

        public NoticeMessage[] GetBroadcast()
        {
            var cacheData = GetCache();
            NoticeMessage[] messages;
            cacheData.CopyTo(out messages, 0);
            return messages;
        }

        public void Send(NoticeMessage message)
        {
            var cacheData = GetCache();
            cacheData.Enqueue(message);
        }

        protected TQueue<NoticeMessage> GetCache()
        {
            return (TQueue<NoticeMessage>)GetCacheObject();
        }

        protected override TCollection<NoticeMessage> CreateCacheStruct()
        {
            return new TQueue<NoticeMessage>(MessageMaxCount);
        }*/

    }
}

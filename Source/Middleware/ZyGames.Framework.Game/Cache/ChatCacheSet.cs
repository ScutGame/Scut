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
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Cache
{
    /// <summary>
    /// 聊天缓存
    /// </summary>
    public class ChatCacheSet : ShareCacheStruct<ChatMessage>
    {
        private const string GroupKey = "10000";
        private readonly static int MessageMaxCount;
        private readonly static int Timeout;

        static ChatCacheSet()
        {
            MessageMaxCount = ConfigUtils.GetSetting("chatcache_maxcount", "3000").ToInt();
            Timeout = ConfigUtils.GetSetting("chatcache_timeout", "1800").ToInt(); //30分钟
        }
        
        protected override bool LoadFactory()
        {
            return true;
        }

        protected override bool LoadItemFactory(string key)
        {
            return true;
        }

        protected override bool InitCache(List<ChatMessage> dataList, int periodTime)
        {
            bool result = false;
            foreach (var data in dataList)
            {
                result = DataContainer.TryAddQueue(GroupKey, data, periodTime, OnExpired);
                if (!result)
                {
                    TraceLog.WriteError("Load data:\"{0}\" tryadd key:\"{1}\" error.", DataContainer.RootKey, GroupKey);
                    return false;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Add(ChatMessage message)
        {
            DataContainer.TryAddQueue(GroupKey, message, 0, OnExpired);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ChatMessage[] GetMessage()
        {
            CacheQueue<ChatMessage> chatQueue;
            if (DataContainer.TryGetQueue(GroupKey, out chatQueue))
            {
                return chatQueue.ToArray();
            }
            return new ChatMessage[0];
        }

        private static bool OnExpired(string groupKey, CacheQueue<ChatMessage> messageQueue)
        {
            while (messageQueue != null && messageQueue.Count > 0)
            {
                ChatMessage msg;
                if(messageQueue.TryPeek(out msg))
                {
                    if (msg != null && MathUtils.DiffDate(msg.SendDate).TotalSeconds > Timeout)
                    {
                        ChatMessage temp;
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
    }
}

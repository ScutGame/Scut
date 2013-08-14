using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// 私聊缓存
    /// </summary>
    public class WhisperCacheSet : ShareCacheStruct<ChatMessage>
    {
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
            foreach (ChatMessage data in dataList)
            {
                string groupKey = data.ToUserID.ToString();
                result = DataContainer.TryAddQueue(groupKey, data, periodTime, OnExpired);
                if (!result)
                {
                    TraceLog.WriteError("Load data:\"{0}\" tryadd key:\"{1}\" error.", DataContainer.RootKey, groupKey);
                    return false;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        public void Add(int userId, ChatMessage message)
        {
            string groupKey = userId.ToString();
            DataContainer.TryAddQueue(groupKey, message, 0, OnExpired);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool HasMessage(int userId)
        {
            string groupKey = userId.ToString();
            CacheQueue<ChatMessage> chatQueue;
            if (DataContainer.TryGetQueue(groupKey, out chatQueue))
            {
                return chatQueue.Count > 0;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ChatMessage[] GetMessage(int userId)
        {
            string groupKey = userId.ToString();
            CacheQueue<ChatMessage> chatQueue;
            if (DataContainer.TryGetQueue(groupKey, out chatQueue))
            {
                ChatMessage[] chatList;
                if (chatQueue.TryDequeueAll(out chatList))
                {
                    return chatList;
                }
            }
            return new ChatMessage[0];
        }

        private static bool OnExpired(string groupKey, CacheQueue<ChatMessage> messageQueue)
        {
            return true;
        }

    }
}

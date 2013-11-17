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
            MessageMaxCount = ConfigUtils.GetSetting("chatcache_maxcount", 3000);
            Timeout = ConfigUtils.GetSetting("chatcache_timeout", 1800); //30分钟
        }
        /// <summary>
        /// 加载数据工厂
        /// </summary>
        /// <returns></returns>
        protected override bool LoadFactory()
        {
            return true;
        }
		/// <summary>
		/// 加载子项数据工厂
		/// </summary>
		/// <returns></returns>
		/// <param name="key">Key.</param>
        protected override bool LoadItemFactory(string key)
        {
            return true;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataList"></param>
		/// <param name="periodTime"></param>
		/// <returns></returns>
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
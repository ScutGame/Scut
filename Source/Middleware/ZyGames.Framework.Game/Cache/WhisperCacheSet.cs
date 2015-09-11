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

using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Model;

namespace ZyGames.Framework.Game.Cache
{
    /// <summary>
    /// 私聊缓存
    /// </summary>
    public class WhisperCacheSet : ShareCacheStruct<ChatMessage>
    {
		/// <summary>
		/// 加载数据工厂
		/// </summary>
		/// <returns></returns>
        protected override bool LoadFactory(bool isReplace)
        {
            return true;
        }

        /// <summary>
        /// 加载子项数据工厂
        /// </summary>
        /// <returns></returns>
        /// <param name="key">Key.</param>
        /// <param name="isReplace"></param>
        protected override bool LoadItemFactory(string key, bool isReplace)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="periodTime"></param>
        /// <param name="isReplace"></param>
        /// <returns></returns>
        protected override bool InitCache(List<ChatMessage> dataList, int periodTime, bool isReplace)
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
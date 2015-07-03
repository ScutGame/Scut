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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Config;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatCacheSet : ChatCacheSet<ChatMessage>
    {
    }

    /// <summary>
    /// 聊天缓存
    /// </summary>
    public class ChatCacheSet<T> : ShareCacheStruct<T> where T : ShareEntity, new()
    {
        private const string Key = "Chat";

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
        protected override bool InitCache(List<T> dataList, int periodTime, bool isReplace)
        {
            bool result = false;
            foreach (var data in dataList)
            {
                result = DataContainer.TryAddQueue(Key, data, periodTime, OnExpired);
                if (!result)
                {
                    TraceLog.WriteError("Load data:\"{0}\" tryadd key:\"{1}\" error.", DataContainer.RootKey, Key);
                    return false;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Add(T message)
        {
            DataContainer.TryAddQueue(Key, message, 0, OnExpired);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T[] GetMessage()
        {
            CacheQueue<T> chatQueue;
            if (DataContainer.TryGetQueue(Key, out chatQueue))
            {
                return chatQueue.ToArray();
            }
            return new T[0];
        }

        private static bool OnExpired(string groupKey, CacheQueue<T> cache)
        {
            CacheQueue<T> messageQueue;
            if (new ChatCacheSet<T>().DataContainer.TryGetQueue(Key, out messageQueue))
            {
                var section = ConfigManager.Configger.GetFirstOrAddConfig<MiddlewareSection>();
                while (messageQueue != null && messageQueue.Count > 0)
                {
                    T msg;
                    if (messageQueue.TryPeek(out msg))
                    {
                        if (msg != null &&
                            msg.ExpiredTime > DateTime.MinValue &&
                            MathUtils.DiffDate(msg.ExpiredTime).TotalSeconds > section.ChatTimeout)
                        {
                            T temp;
                            messageQueue.TryDequeue(out temp);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return true;
        }
    }
}
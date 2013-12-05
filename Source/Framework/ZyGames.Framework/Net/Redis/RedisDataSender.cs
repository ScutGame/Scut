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
using ServiceStack.Redis.Support;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;
using ServiceStack.Redis;

namespace ZyGames.Framework.Net.Redis
{
    /// <summary>
    /// 储存格式：
    /// </summary>
    class RedisDataSender : IDataSender
    {
        private string _redisKey;

        public RedisDataSender(string redisKey)
        {
            _redisKey = redisKey;
            //string redisKey = string.Format("{0}_{1}", data.GetType().FullName, data.GetIdentityId());
        }

        #region IDataSender 成员

        public void Send<T>(T data) where T : AbstractEntity
        {
            Send(new T[] { data }, true, null, null);
        }

        public void Send<T>(T[] dataList) where T : AbstractEntity
        {
            Send(dataList, true, null, null);
        }

        public void Send<T>(T[] dataList, bool isChange) where T : AbstractEntity
        {
            Send(dataList, isChange, null, null);
        }

        public void Send<T>(T[] dataList, bool isChange, string connectKey, EntityBeforeProcess handle) where T : AbstractEntity
        {
            SetEntity(_redisKey, dataList);
        }

        public void Dispose()
        {
        }

        #endregion

        private void SetEntity<T>(string redisKey, T[] dataList) where T : AbstractEntity
        {
            if (RedisConnectionPool.EnableRedis)
            {
                using (var client = RedisConnectionPool.GetClient())
                {
                    if (client != null && client.IsConnected)
                    {
                        RedisManager.SetRedisCache(client, redisKey, dataList);
                    }
                    else
                    {
                        RedisConnectionPool.IncreaseError();
                    }
                }
            }
        }

    }
}
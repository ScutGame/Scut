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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Redis
{
    /// <summary>
    /// Redis管理者
    /// </summary>
    internal static class RedisManager
    {
        /// <summary>
        /// 增加到Redis的字典集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public static bool AppendToDict<T>(string redisKey, params T[] dataList) where T : AbstractEntity
        {
            bool result = false;
            Process(client =>
            {
                try
                {
                    var dataSet = GetDict<T>(client, redisKey);
                    List<T> removeList = new List<T>();
                    foreach (var data in dataList)
                    {
                        string dateKey = data.GetKeyCode();
                        if (data.IsDelete || data.IsRemoveFlag())
                        {
                            removeList.Add(data);
                            dataSet.Remove(dateKey);
                            TraceLog.ReleaseWriteDebug("Save to redis \"{0}\" remove key:\"{1}\"", typeof(T).FullName, dateKey);
                        }
                        else
                        {
                            dataSet[dateKey] = data;
                            data.Reset();
                        }
                    }
                    result = SetDict(client, redisKey, dataSet);
                    SetRemoveDict(client, redisKey, removeList);
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("The key:{0} AppendToDict error:{1}", redisKey, ex);
                }
            });
            return result;
        }

        private static void SetRemoveDict<T>(RedisConnection client, string redisKey, List<T> removeList) where T : AbstractEntity
        {
            if (removeList.Count == 0)
            {
                return;
            }
            string setId = redisKey + ":remove";
            var bytes = client.Get<byte[]>(setId) ?? new byte[0];
            var dict = ProtoBufUtils.Deserialize<Dictionary<string, T>>(bytes);
            foreach (var item in removeList)
            {
                dict[item.GetKeyCode()] = item;
            }
            client.Set(setId, ProtoBufUtils.Serialize(dict));
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="dataList"></param>
        /// <returns>返回是否加载成功</returns>
        public static bool TryGet<T>(string redisKey, out List<T> dataList)
        {
            bool result = false;
            List<T> list = null;
            Process(client =>
            {
                try
                {
                    byte[] buffer = client.Get<byte[]>(redisKey);
                    if (buffer != null)
                    {
                        var dataSet = ProtoBufUtils.Deserialize<Dictionary<string, T>>(buffer);
                        if (dataSet != null)
                        {
                            list = dataSet.Values.ToList();
                        }
                    }
                    if (list == null)
                    {
                        list = new List<T>();
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Get redis \"{0}\" key:\"{1}\" cache error:{2}", typeof(T).FullName, redisKey, ex);
                }
            });
            dataList = list;
            return result;
        }

        /// <summary>
        /// 自定处理
        /// </summary>
        /// <param name="func"></param>
        public static void Process(Action<RedisConnection> func)
        {
            var client = RedisConnectionPool.Pop();
            try
            {
                func(client);
            }
            finally
            {
                RedisConnectionPool.Put(client);
            }
        }

        /// <summary>
        /// 获取指定Key的下一个唯一编号
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetNextNo(string key)
        {
            var client = RedisConnectionPool.Pop();
            try
            {
                return client.Increment(key);
            }
            finally
            {
                RedisConnectionPool.Put(client);
            }
        }

        private static Dictionary<string, T> GetDict<T>(RedisConnection client, string redisKey)
        {
            Dictionary<string, T> dataSet = null;
            if (client != null)
            {
                byte[] buffer = new byte[0];
                try
                {
                    buffer = client.Get<byte[]>(redisKey) ?? new byte[0];
                    dataSet = ProtoBufUtils.Deserialize<Dictionary<string, T>>(buffer);
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Key:{0} byte length:{1} protobuf deserialize \"{2}\" error:{3}", redisKey,
                                        buffer.Length, typeof(T).FullName, ex);
                }
            }
            if (dataSet == null)
            {
                dataSet = new Dictionary<string, T>();
            }
            return dataSet;
        }

        private static bool SetDict<T>(RedisConnection client, string redisKey, Dictionary<string, T> dataSet)
        {
            byte[] buffer = new byte[0];
            try
            {
                buffer = ProtoBufUtils.Serialize(dataSet);
                if (buffer.Length > 0)
                {
                    //先设置为空
                    client.Set(redisKey, new byte[0]);
                    return client.Set(redisKey, buffer);
                }
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Set to key:\"{0}\" protobuf serialize \"{1}\" error:{2}", redisKey, typeof(T).FullName, ex);
                return false;
            }
        }

    }
}
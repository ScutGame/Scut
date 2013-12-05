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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Net.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisManager
    {
        private static int expiresMinutes = ConfigUtils.GetSetting("Redis.ExpireMinutes", 0);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="redisKey"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public static bool SetRedisCache<T>(RedisConnection client, string redisKey, T[] dataList) where T : AbstractEntity
        {
            try
            {
                if (client == null || Equals(dataList, default(T)))
                {
                    return false;
                }
                Dictionary<string, T> dataSet = GetRedisDataSet<T>(client, redisKey);
                AppendToDataSet(dataSet, dataList);
                if (SaveToRedis(client, redisKey, dataSet))
                {
                    string jsonData = "";
                    try
                    {
                        jsonData = JsonUtils.SerializeCustom(dataList);
                    }
                    catch { }
                    TraceLog.ReleaseWriteDebug("Save to redis \"{0}\" data:{1}", redisKey, jsonData);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Save to redis \"{0}\" key:\"{1}\" error:{2}", typeof(T).FullName, redisKey, ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSet"></param>
        /// <param name="dataList"></param>
        public static void AppendToDataSet<T>(Dictionary<string, T> dataSet, T[] dataList) where T : AbstractEntity
        {
            foreach (var data in dataList)
            {
                string dateKey = data.GetKeyCode();
                if (data.IsDelete || data.IsRemoveFlag())
                {
                    dataSet.Remove(dateKey);
                    TraceLog.ReleaseWriteDebug("Save to redis \"{0}\" remove key:\"{1}\"", typeof(T).FullName, dateKey);
                }
                else if (dataSet.ContainsKey(dateKey))
                {
                    dataSet[dateKey] = data;
                }
                else
                {
                    dataSet.Add(dateKey, data);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="redisKey"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static bool SaveToRedis<T>(RedisConnection client, string redisKey, Dictionary<string, T> dataSet)
        {
            byte[] buffer = new byte[0];
            try
            {
                buffer = ProtoBufUtils.ProtobufSerialize(dataSet);
                if (client != null && buffer.Length > 0)
                {
                    //先设置为空
                    client.Set(redisKey, new byte[0]);
                    if (expiresMinutes > 0)
                    {
                        return client.Set(redisKey, buffer, expiresMinutes);
                    }
                    return client.Set(redisKey, buffer);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Set to key:\"{0}\" protobuf serialize \"{1}\" error:{2}", redisKey, typeof(T).FullName, ex);
                return false;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static Dictionary<string, T> GetRedisDataSet<T>(RedisConnection client, string redisKey)
        {
            Dictionary<string, T> dataSet = null;
            if (client != null)
            {
                byte[] buffer = new byte[0];
                try
                {
                    buffer = client.Get<byte[]>(redisKey) ?? new byte[0];
                    dataSet = ProtoBufUtils.ProtobufDeserialize<Dictionary<string, T>>(buffer);
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
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static List<T> GetRedisCache<T>(RedisConnection client, string redisKey)
        {
            if (client == null)
            {
                return null;
            }
            try
            {
                byte[] buffer = client.Get<byte[]>(redisKey);
                if (buffer != null)
                {
                    var dataSet = ProtoBufUtils.ProtobufDeserialize<Dictionary<string, T>>(buffer);
                    if (dataSet != null)
                    {
                        List<T> list = new List<T>();
                        foreach (var pair in dataSet)
                        {
                            list.Add(pair.Value);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Get redis \"{0}\" key:\"{1}\" cache error:{2}", typeof(T).FullName, redisKey, ex);
            }
            return null;
        }
    }
}
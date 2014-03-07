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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Model;
using ZyGames.Framework.Redis;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 缓存改变键管理
    /// </summary>
    internal class CacheChangeManager
    {
        public static readonly string GlobalRedisKey = "__GLOBAL_SQL_CHANGE_KEYS";
        public static readonly string GlobalChangeKey = "__GLOBAL_CHANGE_KEYS";
        private static CacheChangeManager instance = new CacheChangeManager();

        /// <summary>
        /// 
        /// </summary>
        public static CacheChangeManager Current
        {
            get
            {
                return instance;
            }
        }

        private CacheChangeManager()
        {
        }

        /// <summary>
        /// Put to sql change keys
        /// </summary>
        /// <param name="key">The entity key</param>
        public void PutSql(string key)
        {
            lock (instance)
            {
                RedisManager.Process(client =>
                {
                    byte[] data = client.Get<byte[]>(GlobalRedisKey) ?? new byte[0];
                    var dict = ProtoBufUtils.Deserialize<Dictionary<string, bool>>(data) ??
                               new Dictionary<string, bool>();
                    dict[key] = true;
                    data = ProtoBufUtils.Serialize(dict);
                    client.Set(GlobalRedisKey, data);
                });
            }
        }

        /// <summary>
        /// pop sql change keys
        /// </summary>
        /// <param name="count">the count.</param>
        public List<string> PopSql(int count = 1)
        {
            Dictionary<string, bool> dict = null;
            List<string> keys = new List<string>();
            RedisManager.Process(client =>
            {
                string setId = GlobalRedisKey + "_temp";
                bool hasKey = client.ContainsKey(GlobalRedisKey);
                bool hasSetId = client.ContainsKey(setId);
                if (!hasSetId && !hasKey)
                {
                    return;
                }
                if (!hasSetId && hasKey)
                {
                    client.Rename(GlobalRedisKey, setId);
                }
                byte[] data = client.Get<byte[]>(setId) ?? new byte[0];
                dict = ProtoBufUtils.Deserialize<Dictionary<string, bool>>(data);

                keys = dict.Keys.Take(count).ToList();
                foreach (var key in keys)
                {
                    dict.Remove(key);
                }
                if (dict.Count == 0)
                {
                    client.Remove(setId);
                }
                else
                {
                    data = ProtoBufUtils.Serialize(dict);
                    client.Set(setId, data);
                }

            });
            return keys;
        }

        /// <summary>
        /// get entity keys
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, byte[]>> GetKeys(int min = 0, int max = 0)
        {
            if (max == 0)
            {
                max = int.MaxValue;
            }
            var list = new List<KeyValuePair<string, byte[]>>();
            string key = GlobalChangeKey;
            RedisManager.Process(client =>
            {
                string setId = key + "_temp";
                try
                {
                    if (!client.ContainsKey(setId) && client.ContainsKey(key))
                    {
                        client.Rename(key, setId);
                    }
                    byte[][] buffers = client.ZRange(setId, min, max);
                    if (buffers == null || buffers.Length == 0)
                    {
                        client.Remove(setId);
                        return;
                    }

                    foreach (var buffer in buffers)
                    {
                        list.Add(ProtoBufUtils.Deserialize<KeyValuePair<string, byte[]>>(buffer));
                        client.ZRemove(setId, buffer);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Entity change key Pop setId:{0} error:{1}", setId, ex);
                }
            });

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        public void SetEntity(params AbstractEntity[] entitys)
        {
            foreach (var entity in entitys)
            {
                string key = "";
                try
                {
                    if (entity != null)
                    {
                        if (entity.GetSchema().CacheType == CacheType.Entity)
                        {
                            key = string.Format("{0}_{1}", entity.GetType(), entity.GetKeyCode());
                        }
                        else
                        {
                            key = string.Format("{0}_{1}|{2}", entity.GetType(), entity.PersonalId, entity.GetKeyCode());
                        }
                        var data = ProtoBufUtils.Serialize(entity);
                        SetKey(key, data);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Post changed key:{0} error:{1}", key, ex);
                }
            }
        }

        private void SetKey(string key, byte[] data)
        {
            RedisManager.Process(client =>
            {
                try
                {
                    var pair = new KeyValuePair<string, byte[]>(key, data);
                    byte[] value = ProtoBufUtils.Serialize(pair);
                    client.ZAdd(GlobalChangeKey, value);
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Entity change key put to redis error:{0}\r\n{1}", key, ex);
                }
            });
        }

    }
}
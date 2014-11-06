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
using System.Net.NetworkInformation;
using System.Text;
using ServiceStack.Redis;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Model;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Redis
{

    /// <summary>
    /// 连接池管理
    /// </summary>
    public static class RedisConnectionPool
    {
        /// <summary>
        /// 
        /// </summary>
        public const string EntityKeyPreChar = "$";
        /// <summary>
        /// 
        /// </summary>
        internal const string EntityKeySplitChar = "_";
        //private static PooledRedisClientManager _pooledRedis;
        private static RedisPoolSetting _defaultSetting;
        private static ICacheSerializer _serializer;
        private static RedisPoolSetting _setting;
        private static ConcurrentDictionary<string, ObjectPoolWithExpire<RedisClient>> _poolCache = new ConcurrentDictionary<string, ObjectPoolWithExpire<RedisClient>>();

        static RedisConnectionPool()
        {
            _serializer = new ProtobufCacheSerializer();
            _defaultSetting = new RedisPoolSetting
            {
                Host = ConfigUtils.GetSetting("Redis.Host", "localhost"),
                MaxWritePoolSize = ConfigUtils.GetSetting("Redis.MaxWritePoolSize", 100),
                MaxReadPoolSize = ConfigUtils.GetSetting("Redis.MaxReadPoolSize", 100),
                ConnectTimeout = ConfigUtils.GetSetting("Redis.ConnectTimeout", 0),
                PoolTimeOut = ConfigUtils.GetSetting("Redis.PoolTimeOut", 0),
                DbIndex = ConfigUtils.GetSetting("Redis.Db", 0)
            };
            _defaultSetting.ReadOnlyHost = ConfigUtils.GetSetting("Redis.ReadHost", _defaultSetting.Host);
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="serializer"></param>
        public static void Initialize(ICacheSerializer serializer)
        {
            Initialize(_defaultSetting, serializer);
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="setting">pool setting</param>
        /// <param name="serializer"></param>
        public static void Initialize(RedisPoolSetting setting, ICacheSerializer serializer)
        {
            _setting = setting;
            _serializer = serializer;
            //string[] readWriteHosts = setting.Host.Split(',');
            //string[] readOnlyHosts = setting.ReadOnlyHost.Split(',');
            //var redisConfig = new RedisClientManagerConfig
            //{
            //    MaxWritePoolSize = setting.MaxWritePoolSize,
            //    MaxReadPoolSize = setting.MaxReadPoolSize,
            //    DefaultDb = setting.DbIndex,
            //    AutoStart = false
            //};
            //_pooledRedis = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, redisConfig);
            //if (setting.ConnectTimeout > 0)
            //{
            //    _pooledRedis.ConnectTimeout = setting.ConnectTimeout;
            //}
            //if (setting.PoolTimeOut > 0)
            //{
            //    _pooledRedis.PoolTimeout = setting.PoolTimeOut;
            //}
            //_pooledRedis.Start();
        }

        /// <summary>
        /// SetNo
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long SetNo(string key, long value)
        {
            long increment = 0;
            Process(client =>
            {
                var num = client.IncrementValue(key);
                if (value > 0 && num < value)
                {
                    increment = client.Increment(key, (value - num).ToUInt32());
                }
                else
                {
                    increment = num;
                }
            });
            return increment;
        }

        /// <summary>
        /// GetNo
        /// </summary>
        /// <param name="key"></param>
        /// <param name="increaseNum">increase num,defalut 1</param>
        /// <returns></returns>
        public static long GetNextNo(string key, uint increaseNum = 1)
        {
            long result = 0;
            Process(client =>
            {
                result = client.Increment(key, increaseNum);
            });
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="Exception"></exception>
        public static void ProcessTrans(Action<IRedisTransaction> func)
        {
            var client = GetClient();
            try
            {
                var trans = client.CreateTransaction();
                try
                {
                    func(trans);
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
            finally
            {
                PuttPool(client);
            }
        }
        /// <summary>
        /// Process delegate
        /// </summary>
        /// <param name="func"></param>
        public static void Process(Action<RedisClient> func)
        {
            var client = GetClient();
            try
            {
                func(client);
            }
            finally
            {
                PuttPool(client);
            }
        }
        /// <summary>
        /// Process ReadOnly delegate
        /// </summary>
        /// <param name="func"></param>
        public static void ProcessReadOnly(Action<RedisClient> func)
        {
            var client = GetReadOnlyClient();
            try
            {
                func(client);
            }
            finally
            {
                PuttPool(client);
            }
        }
        /// <summary>
        /// check connect to redis.
        /// </summary>
        /// <returns></returns>
        public static bool CheckConnect()
        {
            bool result = false;
            try
            {
                Process(client =>
                {
                    result = client.Ping();
                });

                ProcessReadOnly(client =>
                {
                    result = client.Ping();
                });
                return result;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Check Redis Connect error:{0}", ex);
                return result;
            }
        }

        /// <summary>
        /// Ping ip
        /// </summary>
        /// <returns></returns>
        public static bool Ping(string ip)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send(ip, intTimeout, buffer, objPinOptions);
                return objPinReply != null && objPinReply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Pool Count
        /// </summary>
        public static int PoolCount
        {
            get
            {
                return _poolCache.Sum(p => p.Value.PoolCount);
            }
        }

        /// <summary>
        /// Get read and write connection
        /// </summary>
        /// <returns></returns>
        public static RedisClient GetClient()
        {
            return GetPool();
            //return (RedisClient)_pooledRedis.GetClient();
        }
        /// <summary>
        /// Get read only connection
        /// </summary>
        /// <returns></returns>
        public static RedisClient GetReadOnlyClient()
        {
            return GetPool();
            //return (RedisClient)_pooledRedis.GetReadOnlyClient();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public static void PuttPool(RedisClient client)
        {
            var key = string.Format("{0}:{1}", client.Host, client.Port);
            ObjectPoolWithExpire<RedisClient> pool;
            if (_poolCache.TryGetValue(key, out pool))
            {
                if (client.HadExceptions)
                {
                    client.Dispose();
                    client = CreateRedisClient(_setting);//create new client modify by Seamoon
                }
                pool.Put(client);
            }
            else
            {
                client.Dispose();
            }
        }

        private static RedisClient GetPool()
        {
            string[] arrs = _setting.Host.Split('@', ':');
            var key = arrs.Length == 3 ? string.Format("{0}:{1}", arrs[1], arrs[2])
                : arrs.Length == 2 ? string.Format("{0}:{1}", arrs[0], arrs[1])
                : string.Format("{0}:{1}", arrs[0], 6379);

            ObjectPoolWithExpire<RedisClient> pool;
            do
            {
                if (!_poolCache.TryGetValue(key, out pool))
                {
                    pool = new ObjectPoolWithExpire<RedisClient>(() => CreateRedisClient(_setting), true);
                    if (_poolCache.TryAdd(key, pool))
                    {
                        //init pool
                        for (int i = 0; i < pool.MinPoolSize; i++)
                        {
                            pool.Put(CreateRedisClient(_setting));
                        }
                        break;
                    }
                }
                else break;
            } while (true);

            return pool.Get();
        }

        private static RedisClient CreateRedisClient(RedisPoolSetting setting)
        {
            var client = new RedisClient(setting.Host);
            if (setting.DbIndex > 0)
            {
                client.Db = setting.DbIndex;
            }
            return client;
        }

        /// <summary>
        /// Try get entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="table"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool TryGetEntity<T>(string redisKey, SchemaTable table, out List<T> list) where T : AbstractEntity
        {
            list = new List<T>();
            bool result = false;
            try
            {
                //修改存储统一Hash格式(TypeName, keyCode, value)
                var keys = redisKey.Split('_');
                string keyValue = "";
                string hashId = GetRedisEntityKeyName(keys[0]);
                byte[] keyCode = null;
                if (keys.Length > 1)
                {
                    keyValue = keys[1];
                    keyCode = ToByteKey(keyValue);
                }
                byte[][] valueBytes = null;
                byte[] value = null;
                bool isFilter = false;
                try
                {
                    ProcessReadOnly(client =>
                    {
                        if (keyCode == null)
                        {
                            if (client.ContainsKey(hashId))
                            {
                                valueBytes = client.HVals(hashId);//.Where((b, index) => index % 2 == 1).ToArray();
                            }
                        }
                        else
                        {
                            value = client.HGet(hashId, keyCode);
                            //修正未使用Persional作为Key,而是多个Key时,加载数据为空问题,修改成加载所有
                            if (value == null && table != null
                                && !string.IsNullOrEmpty(keyValue)
                                && typeof(T).IsSubclassOf(typeof(BaseEntity))
                                && table.Keys.Length > 1)
                            {
                                isFilter = true;
                                byte[][] resultKeys = client.HKeys(hashId).Where(k => ContainKey(k, keyCode, AbstractEntity.KeyCodeJoinChar)).ToArray();
                                if (resultKeys.Length > 0)
                                {
                                    valueBytes = client.HMGet(hashId, resultKeys);//.Where((b, index) => index % 2 == 1).ToArray();
                                }
                            }
                        }
                    });

                    if (valueBytes != null || value != null)
                    {
                        if (value != null)
                        {
                            list = new List<T> { (T)_serializer.Deserialize(value, typeof(T)) };
                            return true;
                        }
                        if (valueBytes != null)
                        {
                            if (isFilter)
                            {
                                list = valueBytes.Select(t => (T)_serializer.Deserialize(t, typeof(T))).Where(t => t.PersonalId == keyValue).ToList();
                            }
                            else
                            {
                                list = valueBytes.Select(t => (T)_serializer.Deserialize(t, typeof(T))).ToList();
                            }
                            return true;
                        }
                    }
                }
                catch (Exception er)
                {
                    list = null;
                    TraceLog.WriteError("Get redis \"{0}\" key:\"{1}\" cache error:{2}", typeof(T).FullName, redisKey, er);
                }

                try
                {

                    //从旧版本存储格式中查找
                    if (typeof(T).IsSubclassOf(typeof(ShareEntity)))
                    {
                        byte[][] buffers = new byte[0][];
                        List<string> keyList = new List<string>();
                        ProcessReadOnly(client =>
                        {
                            keyList = client.SearchKeys(string.Format("{0}_*", redisKey));
                            if (keyList != null && keyList.Count > 0)
                            {
                                buffers = client.MGet(keyList.ToArray());
                            }
                        });
                        list = new List<T>();
                        byte[][] keyCodes = new byte[buffers.Length][];
                        for (int i = 0; i < buffers.Length; i++)
                        {
                            T entity = (T)_serializer.Deserialize(buffers[i], typeof(T));
                            keyCodes[i] = ToByteKey(entity.GetKeyCode());
                            list.Add(entity);
                        }
                        if (keyCodes.Length > 0)
                        {
                            //转移到新格式
                            UpdateEntity(typeof(T).FullName, keyCodes, buffers);
                            if (keyList.Count > 0)
                            {
                                Process(client => client.RemoveAll(keyList));
                            }
                        }
                    }
                    else
                    {
                        byte[] buffers = new byte[0];
                        ProcessReadOnly(client =>
                        {
                            buffers = client.Get<byte[]>(redisKey) ?? new byte[0];
                        });

                        try
                        {
                            var dataSet = (Dictionary<string, T>)_serializer.Deserialize(buffers, typeof(Dictionary<string, T>));
                            if (dataSet != null)
                            {
                                list = dataSet.Values.ToList();
                            }
                        }
                        catch
                        {
                            //try get entity type data
                            list = new List<T>();
                            T temp = (T)_serializer.Deserialize(buffers, typeof(T));
                            list.Add(temp);
                        }
                        //转移到新格式
                        if (list != null)
                        {
                            byte[][] keyCodes = new byte[list.Count][];
                            byte[][] values = new byte[list.Count][];
                            for (int i = 0; i < list.Count; i++)
                            {
                                T entity = list[i];
                                keyCodes[i] = ToByteKey(entity.GetKeyCode());
                                values[i] = _serializer.Serialize(entity);
                            }
                            if (keyCodes.Length > 0)
                            {
                                UpdateEntity(typeof(T).FullName, keyCodes, values);
                                Process(client => client.Remove(redisKey));
                            }
                        }
                    }
                    result = true;
                }
                catch (Exception er)
                {
                    list = null;
                    TraceLog.WriteError("Get redis \"{0}\" key(old):\"{1}\" cache error:{2}", typeof(T).FullName, redisKey, er);
                }
            }
            catch (Exception ex)
            {
                list = null;
                TraceLog.WriteError("Get redis \"{0}\" key:\"{1}\" cache error:{2}", typeof(T).FullName, redisKey, ex);
            }
            return result;
        }

        private static bool ContainKey(byte[] bytes, byte[] pattern, char pre)
        {
            byte[] arr = MathUtils.CharToByte(pre);
            bytes = MathUtils.Join(arr, bytes);
            pattern = MathUtils.Join(arr, pattern);
            return MathUtils.IndexOf(bytes, pattern) > -1;
        }


        /// <summary>
        /// Try update entity
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public static bool TryUpdateEntity(IEnumerable<AbstractEntity> dataList)
        {
            var groupList = dataList.GroupBy(t => t.GetType().FullName);
            foreach (var g in groupList)
            {
                string typeName = g.Key;
                string redisKey = typeName;
                try
                {
                    var keys = new List<byte[]>();
                    var values = new List<byte[]>();
                    var removeKeys = new List<byte[]>();
                    var enm = g.GetEnumerator();
                    while (enm.MoveNext())
                    {
                        AbstractEntity entity = enm.Current;
                        string keyCode = entity.GetKeyCode();
                        var keybytes = ToByteKey(keyCode);
                        redisKey += EntityKeySplitChar + keyCode;
                        if (entity.IsDelete)
                        {
                            removeKeys.Add(keybytes);
                            continue;
                        }
                        entity.Reset();
                        keys.Add(keybytes);
                        values.Add(_serializer.Serialize(entity));
                    }
                    UpdateEntity(typeName, keys.ToArray(), values.ToArray(), removeKeys.ToArray());
                    return true;
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Update entity \"{0}\" error:{1}", redisKey, ex);
                }
            }
            return false;
        }

        /// <summary>
        /// Try update entity
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <param name="removeKeys"></param>
        /// <returns></returns>
        internal static void UpdateEntity(string typeName, byte[][] keys, byte[][] values, params byte[][] removeKeys)
        {
            var hashId = GetRedisEntityKeyName(typeName);
            Process(client =>
            {
                if (keys.Length > 0)
                {
                    client.HMSet(hashId, keys, values);
                }
                if (removeKeys.Length > 0)
                {
                    client.HDel(hashId, removeKeys);
                }
            });
        }

        /// <summary>
        /// Get key name of store redis entity 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string GetRedisEntityKeyName(string typeName)
        {
            typeName = GetRootKey(typeName);
            string hashId = typeName.StartsWith(EntityKeyPreChar)
                ? typeName
                : EntityKeyPreChar + typeName;
            return hashId;
        }

        internal static string GetRootKey(string redisKey)
        {
            return redisKey.Split('_')[0];
        }

        internal static byte[] ToByteKey(string key)
        {
            return Encoding.UTF8.GetBytes(key);
        }
        internal static string ToStringKey(byte[] keyBytes)
        {
            return Encoding.UTF8.GetString(keyBytes);
        }

        /// <summary>
        /// 从TypeName转成成Redis的Key
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        internal static string EncodeTypeName(string typeName)
        {
            return typeName.Replace(EntityKeySplitChar, "%11");
        }
        /// <summary>
        /// 从Redis的Key转成成TypeName
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string DecodeTypeName(string key)
        {
            return key.TrimStart(EntityKeyPreChar.ToCharArray()).Replace("%11", EntityKeySplitChar);
        }

    }
}
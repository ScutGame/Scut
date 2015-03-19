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
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using ServiceStack.Redis;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Config;
using ZyGames.Framework.Model;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Redis
{

    /// <summary>
    /// 连接池管理
    /// </summary>
    public static class RedisConnectionPool
    {
        internal const int EntityMinVersion = 5;
        /// <summary>
        /// 
        /// </summary>
        public const string EntityKeyPreChar = "$";
        /// <summary>
        /// 
        /// </summary>
        internal const string EntityKeySplitChar = "_";

        private static string RedisInfoKey = "__RedisInfo";
        private static ICacheSerializer _serializer;
        private static RedisPoolSetting _setting;
        private static ConcurrentDictionary<string, ObjectPoolWithExpire<RedisClient>> _poolCache;

        static RedisConnectionPool()
        {
            _poolCache = new ConcurrentDictionary<string, ObjectPoolWithExpire<RedisClient>>();
            _serializer = new ProtobufCacheSerializer();
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="serializer"></param>
        public static void Initialize(ICacheSerializer serializer)
        {
            Initialize(new RedisPoolSetting(), serializer);
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
            InitRedisInfo();
        }

        private static void InitRedisInfo()
        {
            ProcessTrans(RedisInfoKey, cli =>
            {
                RedisInfo = cli.GetValue(RedisInfoKey).ParseJson<RedisInfo>() ?? new RedisInfo();
                var ips = Dns.GetHostAddresses(Dns.GetHostName());
                string host = (ips.FirstOrDefault(t => t.AddressFamily == AddressFamily.InterNetwork) ?? ips[0]).ToString();
                string serverPath = MathUtils.RuntimePath;
                string hashCode = MathUtils.ToHexMd5Hash(host + serverPath);

                var slaveName = ConfigManager.Configger.GetFirstOrAddConfig<MessageQueueSection>().SlaveMessageQueue;
                var serializerType = _serializer is ProtobufCacheSerializer ? "Protobuf" : _serializer is JsonCacheSerializer ? "Json" : "";

                if (string.IsNullOrEmpty(slaveName) && string.IsNullOrEmpty(RedisInfo.HashCode))
                {
                    RedisInfo.HashCode = hashCode;
                    RedisInfo.ServerHost = host;
                    RedisInfo.ServerPath = serverPath;
                    RedisInfo.SerializerType = serializerType;
                    RedisInfo.ClientVersion = _setting.ClientVersion;
                    RedisInfo.StarTime = MathUtils.Now;
                }
                else if (string.IsNullOrEmpty(slaveName) && string.Equals(hashCode, RedisInfo.HashCode))
                {
                    RedisInfo.ClientVersion = _setting.ClientVersion;
                    RedisInfo.SerializerType = serializerType;
                    RedisInfo.StarTime = MathUtils.Now;
                }
                else if (!string.IsNullOrEmpty(slaveName))
                {
                    RedisInfo slaveInfo;
                    //allow a slave server connect.
                    if (!RedisInfo.SlaveSet.ContainsKey(slaveName))
                    {
                        slaveInfo = new RedisInfo();
                        slaveInfo.HashCode = hashCode;
                        slaveInfo.ServerHost = host;
                        slaveInfo.ServerPath = serverPath;
                        slaveInfo.SerializerType = serializerType;
                        slaveInfo.ClientVersion = _setting.ClientVersion;
                        slaveInfo.StarTime = MathUtils.Now;
                        RedisInfo.SlaveSet[slaveName] = slaveInfo;
                    }
                    else if (string.Equals(hashCode, RedisInfo.SlaveSet[slaveName].HashCode))
                    {
                        slaveInfo = RedisInfo.SlaveSet[slaveName];
                        slaveInfo.SerializerType = serializerType;
                        slaveInfo.ClientVersion = _setting.ClientVersion;
                        slaveInfo.StarTime = MathUtils.Now;
                    }
                    else
                    {
                        throw new Exception(string.Format("Redis server is using {0} slave \"{1}\" game server, it's path:{2}",
                           slaveName, RedisInfo.ServerHost, RedisInfo.ServerPath));
                    }
                }
                else
                {
                    throw new Exception(string.Format("Redis server is using \"{0}\" game server, it's path:{1}", RedisInfo.ServerHost, RedisInfo.ServerPath));
                }
                return true;
            }, trans => trans.QueueCommand(c => c.SetEntry(RedisInfoKey, MathUtils.ToJson(RedisInfo))));
        }

        /// <summary>
        /// 
        /// </summary>
        public static RedisInfo RedisInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static RedisPoolSetting Setting
        {
            get { return _setting; }
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
        /// <param name="watchKeys"></param>
        /// <param name="processFunc"></param>
        /// <param name="transFunc"></param>
        /// <returns></returns>
        public static bool ProcessTrans(string watchKeys, Func<RedisClient, bool> processFunc, Action<IRedisTransaction> transFunc)
        {
            return ProcessTrans(new[] { watchKeys }, processFunc, transFunc, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="watchKeys"></param>
        /// <param name="processFunc"></param>
        /// <param name="transFunc"></param>
        /// <param name="errorFunc"></param>
        public static bool ProcessTrans(string[] watchKeys, Func<RedisClient, bool> processFunc, Action<IRedisTransaction> transFunc, Action<IRedisTransaction, Exception> errorFunc)
        {
            bool result = false;
            Process(client =>
            {
                result = ProcessTrans(client, watchKeys, () => processFunc(client), transFunc, errorFunc);
            });
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="watchKeys"></param>
        /// <param name="processFunc"></param>
        /// <param name="transFunc"></param>
        /// <param name="errorFunc"></param>
        /// <returns></returns>
        public static bool ProcessTrans(RedisClient client, string[] watchKeys, Func<bool> processFunc, Action<IRedisTransaction> transFunc, Action<IRedisTransaction, Exception> errorFunc)
        {
            client.Watch(watchKeys);
            if (!processFunc())
            {
                client.UnWatch();
                return false;
            }
            var trans = client.CreateTransaction();
            try
            {
                transFunc(trans);
                return trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                if (errorFunc != null) errorFunc(trans, ex);
            }
            return false;
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
            string[] hostParts = _setting.Host.Split('@', ':');
            var key = hostParts.Length == 3 ? string.Format("{0}:{1}", hostParts[1], hostParts[2])
                : hostParts.Length == 2 ? string.Format("{0}:{1}", hostParts[0], hostParts[1])
                : string.Format("{0}:{1}", hostParts[0], 6379);


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
            string[] hostParts;
            RedisClient client = null;
            if (setting.Host.Contains("@"))
            {
                hostParts = setting.Host.Split('@', ':');
                client = new RedisClient(hostParts[1], hostParts[2].ToInt(), hostParts[0], setting.DbIndex);
            }
            else
            {
                hostParts = setting.Host.Split(':');
                int port = hostParts.Length > 1 ? hostParts[1].ToInt() : 6379;
                client = new RedisClient(hostParts[0], port, null, setting.DbIndex);
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
        public static bool TryGetEntity<T>(string redisKey, SchemaTable table, out List<T> list) where T : ISqlEntity
        {
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
                    TraceLog.WriteError("Get redis \"{0}\" key:\"{1}\" cache error:{2}", typeof(T).FullName, redisKey, er);
                }

                result = TryGetOlbValue(redisKey, out list);
            }
            catch (Exception ex)
            {
                list = null;
                TraceLog.WriteError("Get redis \"{0}\" key:\"{1}\" cache error:{2}", typeof(T).FullName, redisKey, ex);
            }
            return result;
        }

        private static bool TryGetOlbValue<T>(string redisKey, out List<T> list) where T : ISqlEntity
        {
            bool result = false;
            try
            {
                if (RedisInfo.ClientVersion >= EntityMinVersion)
                {
                    list = new List<T>();
                    return true;
                }
                //从旧版本存储格式中查找
                if (typeof(T).IsSubclassOf(typeof(ShareEntity)))
                {
                    var tempList = new List<T>();
                    Process(client =>
                    {
                        byte[][] buffers;
                        List<string> keyList = client.SearchKeys(string.Format("{0}_*", redisKey));
                        if (keyList == null || keyList.Count <= 0)
                        {
                            return;
                        }
                        ProcessTrans(client, keyList.ToArray(), () =>
                        {
                            buffers = client.MGet(keyList.ToArray());
                            byte[][] keyCodes = new byte[buffers.Length][];
                            for (int i = 0; i < buffers.Length; i++)
                            {
                                T entity = (T)_serializer.Deserialize(buffers[i], typeof(T));
                                keyCodes[i] = ToByteKey(entity.GetKeyCode());
                                tempList.Add(entity);
                            }
                            if (keyCodes.Length > 0)
                            {
                                //转移到新格式
                                if (!UpdateEntity(typeof(T).FullName, keyCodes, buffers))
                                {
                                    //转移失败
                                    return false;
                                }
                                if (keyList.Count > 0)
                                {
                                    return true;
                                }
                            }
                            return false;
                        }, trans => trans.QueueCommand(c => c.RemoveAll(keyList)), null);
                    });
                    list = tempList;
                }
                else
                {
                    var tempList = new List<T>();
                    byte[] buffers = new byte[0];
                    ProcessTrans(redisKey, client =>
                    {
                        try
                        {
                            buffers = client.Get<byte[]>(redisKey) ?? new byte[0];
                            var dataSet = (Dictionary<string, T>)_serializer.Deserialize(buffers, typeof(Dictionary<string, T>));
                            if (dataSet != null)
                            {
                                tempList = dataSet.Values.ToList();
                            }
                        }
                        catch
                        {
                            //try get entity type data
                            tempList = new List<T>();
                            T temp = (T)_serializer.Deserialize(buffers, typeof(T));
                            tempList.Add(temp);
                        }
                        //转移到新格式
                        if (tempList != null)
                        {
                            byte[][] keyCodes = new byte[tempList.Count][];
                            byte[][] values = new byte[tempList.Count][];
                            for (int i = 0; i < tempList.Count; i++)
                            {
                                T entity = tempList[i];
                                keyCodes[i] = ToByteKey(entity.GetKeyCode());
                                values[i] = _serializer.Serialize(entity);
                            }
                            if (keyCodes.Length > 0)
                            {
                                if (!UpdateEntity(typeof(T).FullName, keyCodes, values))
                                {
                                    return false;
                                }
                                return true;
                            }
                        }
                        return false;
                    }, trans => trans.QueueCommand(c => c.Remove(redisKey)));

                    list = tempList;
                }
                result = true;
            }
            catch (Exception er)
            {
                list = null;
                TraceLog.WriteError("Get redis \"{0}\" key(old):\"{1}\" cache error:{2}", typeof(T).FullName, redisKey, er);
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
        public static bool TryUpdateEntity(IEnumerable<ISqlEntity> dataList)
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
                        var entity = enm.Current;
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
        private static bool UpdateEntity(string typeName, byte[][] keys, byte[][] values, params byte[][] removeKeys)
        {
            if (keys.Length == 0 && removeKeys.Length > 0)
            {
                return false;
            }
            var hashId = GetRedisEntityKeyName(typeName);
            Process(cli =>
            {
                if (keys.Length > 0)
                {
                    cli.HMSet(hashId, keys, values);
                }
                if (removeKeys.Length > 0)
                {
                    cli.HDel(hashId, removeKeys);
                }
            });
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public static void TransUpdateEntity(IRedisTransaction trans, IEnumerable<ISqlEntity> dataList)
        {
            var groupList = dataList.GroupBy(t => t.GetType().FullName);
            foreach (var g in groupList)
            {
                string typeName = g.Key;
                var keys = new List<byte[]>();
                var values = new List<byte[]>();
                var removeKeys = new List<byte[]>();
                var enm = g.GetEnumerator();
                while (enm.MoveNext())
                {
                    var entity = enm.Current;
                    string keyCode = entity.GetKeyCode();
                    var keybytes = ToByteKey(keyCode);
                    if (entity.IsDelete)
                    {
                        removeKeys.Add(keybytes);
                        continue;
                    }
                    entity.Reset();
                    keys.Add(keybytes);
                    values.Add(_serializer.Serialize(entity));
                }
                TransUpdateEntity(trans, typeName, keys.ToArray(), values.ToArray(), removeKeys.ToArray());
            }
        }

        private static void TransUpdateEntity(IRedisTransaction trans, string hashId, byte[][] keys, byte[][] values, byte[][] removeKeys)
        {
            if (keys.Length > 0)
            {
                trans.QueueCommand(c =>
                {
                    var cli = (RedisClient)c;
                    cli.HMSet(hashId, keys, values);
                });
            }
            if (removeKeys.Length > 0)
            {
                trans.QueueCommand(c =>
                {
                    var cli = (RedisClient)c;
                    cli.HDel(hashId, removeKeys);
                });
            }
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
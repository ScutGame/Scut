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
using System.Reflection;
using System.Text;
using System.Web;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Redis;

namespace ZyGames.Framework.Game.Sns.Service
{
    /// <summary>
    /// 
    /// </summary>
    public static class HandlerManager
    {
        private const string AccountServerToken = "__AccountToken";
        /// <summary>
        /// 
        /// </summary>
        public static string SignKey;
        /// <summary>
        /// 
        /// </summary>
        public static string ClientDesDeKey;
        /// <summary>
        /// 
        /// </summary>
        public static string RedisHost;
        /// <summary>
        /// 
        /// </summary>
        public static int RedisDb;
        private static Dictionary<string, Type> handlerTypes;
        private static ConcurrentDictionary<string, UserToken> userTokenCache = new ConcurrentDictionary<string, UserToken>();
        private static ConcurrentDictionary<int, string> userHashCache = new ConcurrentDictionary<int, string>();
        private static bool RedisConnected = false;

        static HandlerManager()
        {
            SignKey = ConfigUtils.GetSetting("Product.SignKey", "");
            ClientDesDeKey = ConfigUtils.GetSetting("Product.ClientDesDeKey", "");
            RedisHost = ConfigUtils.GetSetting("Redis.Host", "");
            RedisDb = ConfigUtils.GetSetting("Redis.Db", 0);
            handlerTypes = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        public static void Init(Assembly assembly)
        {
            var htype = Type.GetType("ZyGames.Framework.Game.Sns.Service.IHandler`1", false, true);
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.GetInterfaces().Select(t => t.Name).Contains(htype.Name))
                {
                    handlerTypes[type.Name] = type;
                }
            }
            if (!string.IsNullOrEmpty(RedisHost))
            {
                try
                {
                    //use redis cache.
                    RedisConnectionPool.Initialize(new RedisPoolSetting() { Host = RedisHost, DbIndex = RedisDb }, new JsonCacheSerializer(Encoding.UTF8));
                    RedisConnected = true;
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Redis Initialize error:{0}", ex);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="userToken"></param>
        public static void SaveToken(string key, UserToken userToken)
        {
            string userKey = string.Format("{0}:{1}", AccountServerToken, userToken.UserId);
            string redisKey = string.Format("{0}@{1}", AccountServerToken, key);
            if (!string.IsNullOrEmpty(RedisHost) && RedisConnected)
            {
                //use redis cache.
                RedisConnectionPool.Process(client =>
                {
                    string oldToken = client.Get<string>(userKey);
                    if (!string.IsNullOrEmpty(oldToken))
                    {
                        client.Remove(string.Format("{0}@{1}", AccountServerToken, oldToken));
                    }
                    client.Set(redisKey, userToken, userToken.ExpireTime);
                    client.Set(userKey, key, userToken.ExpireTime);
                });
                return;
            }
            if (HttpContext.Current != null && HttpContext.Current.Cache != null)
            {
                var cache = HttpContext.Current.Cache;
                var oldToken = cache[userKey] as string;
                if (!string.IsNullOrEmpty(oldToken))
                {
                    cache.Remove(string.Format("{0}@{1}", AccountServerToken, oldToken));
                }
                cache[redisKey] = userToken;
                cache[userKey] = key;
            }
            else
            {
                string oldToken;
                if (userHashCache.TryGetValue(userToken.UserId, out oldToken))
                {
                    UserToken temp;
                    userTokenCache.TryRemove(oldToken, out temp);
                }
                userTokenCache[key] = userToken;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static UserToken GetUserToken(string key)
        {
            UserToken userToken = null;
            if (!string.IsNullOrEmpty(RedisHost) && RedisConnected)
            {
                //use redis cache.
                RedisConnectionPool.Process(client =>
                {
                    userToken = client.Get<UserToken>(string.Format("{0}@{1}", AccountServerToken, key));
                });
                return userToken;
            }
            if (HttpContext.Current != null && HttpContext.Current.Cache != null)
            {
                userToken = (UserToken)HttpContext.Current.Cache[key];
            }
            if (Equals(userToken, null) && userTokenCache.ContainsKey(key))
            {
                userToken = (UserToken)userTokenCache[key];
            }
            return userToken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handlerData"></param>
        /// <returns></returns>
        public static ResponseData Excute(HandlerData handlerData)
        {
            Type type;
            if (!handlerTypes.TryGetValue(handlerData.Name, out type))
            {
                throw new HandlerException(StateCode.NoHandler, "Not Found Handler");
            }
            dynamic instance = type.CreateInstance();
            Type paramType = type.GetInterfaces().First().GetGenericArguments().First();
            dynamic paramObj = ParseObject(handlerData, paramType);
            var result = instance.Excute(paramObj);
            return result as ResponseData;
        }

        private static object ParseObject(HandlerData handlerData, Type paramType)
        {
            var paramValues = handlerData.Params;
            var obj = paramType.CreateInstance();
            foreach (PropertyInfo prop in paramType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (paramValues.ContainsKey(prop.Name) && (prop.PropertyType.IsValueType || typeof(string) == prop.PropertyType))
                {
                    var value = Convert.ChangeType(paramValues[prop.Name], prop.PropertyType);
                    prop.SetValue(obj, value, null);
                }
            }
            return obj;
        }
    }
}
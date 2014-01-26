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
using ZyGames.Framework.Model;
using ZyGames.Framework.Net.Redis;
using ZyGames.Framework.Net.Sql;

namespace ZyGames.Framework.Net
{
    /// <summary>
    /// 数据同步管理类
    /// </summary>
    public static class DataSyncManager
    {
        #region SQL
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDataSender GetDataSender()
        {
            return new SqlDataSender();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IDataReceiver GetDataGetter(SchemaTable schema, DbDataFilter filter)
        {
            return GetDataGetter(schema, filter.Capacity, filter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="capacity"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IDataReceiver GetDataGetter(SchemaTable schema, int capacity, DbDataFilter filter)
        {
            return new SqlDataReceiver(schema, capacity, filter);
        }
        #endregion


        #region Redis
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IDataSender GetRedisSender(string redisKey)
        {
            return new RedisDataSender(redisKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IDataReceiver GetRedisGetter(SchemaTable schema, string redisKey)
        {
            return new RedisDataGetter(redisKey);
        }
        #endregion
    }

}
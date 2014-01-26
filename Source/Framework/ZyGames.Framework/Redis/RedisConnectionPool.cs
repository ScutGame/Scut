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
using System.Collections.Concurrent;
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Framework.Redis
{
    /// <summary>
    /// 连接池管理
    /// </summary>
    internal static class RedisConnectionPool
    {
        private static ConcurrentQueue<RedisConnection> _connectionPools = new ConcurrentQueue<RedisConnection>();
        private static readonly string Host;
        private static readonly string Password;
        private static readonly int DbIndex;
        private static int PoolMinSize;
        private static int PoolMaxSize;
        private static int Port;

        static RedisConnectionPool()
        {
            Host = ConfigUtils.GetSetting("Redis.Host", "localhost");
            Port = ConfigUtils.GetSetting("Redis.Port", 6379);
            Password = ConfigUtils.GetConnectionString("Redis.Password");
            DbIndex = ConfigUtils.GetSetting("Redis.Db", 0);
            PoolMinSize = ConfigUtils.GetSetting("Redis.Pool.MinSize", 20);
            PoolMaxSize = ConfigUtils.GetSetting("Redis.Pool.MaxSize", 200);

            DbIndex = DbIndex < 0 ? 0 : DbIndex;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            for (int i = 0; i < PoolMinSize; i++)
            {
                var connection = new RedisConnection(Host, Port, Password, DbIndex);
                _connectionPools.Enqueue(connection);
            }
        }

        /// <summary>
        /// 取出连接
        /// </summary>
        /// <returns></returns>
        public static RedisConnection Pop()
        {
            RedisConnection connection;
            if (_connectionPools.TryDequeue(out connection))
            {
                return connection;
            }
            return new RedisConnection(Host, Port, Password, DbIndex);
        }

        /// <summary>
        /// 放入连接池
        /// </summary>
        /// <param name="connection"></param>
        public static void Put(RedisConnection connection)
        {
            if (_connectionPools.Count > PoolMaxSize)
            {
                return;
            }
            _connectionPools.Enqueue(connection);
        }

    }
}
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using ServiceStack.Redis;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Net.Redis
{
    /// <summary>
    /// Redis连接对象
    /// </summary>
    public class RedisConnection : BaseDisposable
    {
        private readonly object _lockObj = new object();
        private IRedisClient _redisClient;
        private string _host;
        private int _port;
        private readonly string _password;
        private readonly int _db;
        private readonly int _poolTimeout;
        private readonly int _connectTimeout;
        private int _isActive;
        private DateTime _activeDate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        public RedisConnection(string host, int port, string password)
            : this(host, port, password, 0, 0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        /// <param name="db"></param>
        /// <param name="poolTimeout"></param>
        /// <param name="connectTimeout"></param>
        public RedisConnection(string host, int port, string password, int db, int poolTimeout, int connectTimeout = 1)
        {
            _host = host;
            _port = port;
            _password = password;
            _db = db;
            _poolTimeout = poolTimeout;
            _connectTimeout = connectTimeout;
        }

        private IRedisClient GetRedisClientInstance()
        {
            if (string.IsNullOrEmpty(_password))
            {
                return new RedisClient(_host, _port) { ConnectTimeout = _connectTimeout };
            }
            else
            {
                return new RedisClient(_host, _port, _password) { ConnectTimeout = _connectTimeout };
            }
        }

        /// <summary>
        /// 连接池ID
        /// </summary>
        public int PoolId { get; set; }

        /// <summary>
        /// 是否是只读的连接池
        /// </summary>
        public bool IsReadonlyPool { get; set; }

        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host
        {
            get
            {
                return _host;
            }
        }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
        }

        /// <summary>
        /// 活动状态,false:可被释放
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _isActive == 1;
            }
        }
        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsTimeout
        {
            get
            {
                return (DateTime.Now - _activeDate).TotalSeconds > _poolTimeout;
            }
        }

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected
        {
            get
            {
                CheckDisposed();
                Stopwatch watch = new Stopwatch();
                try
                {
                    watch.Start();
                    if (_redisClient == null)
                    {
                        lock (_lockObj)
                        {
                            if (_redisClient == null)
                            {
                                _redisClient = GetRedisClientInstance();
                            }
                        }
                    }
                    Interlocked.Exchange(ref _isActive, 1);
                    bool result = _redisClient.DbSize > -1;
                    if (!Equals(_redisClient.Db, _db))
                    {
                        _redisClient.Db = _db;
                    }
                    _activeDate = DateTime.Now;
                    return result;
                }
                catch
                {
                    watch.Stop();
                    TraceLog.WriteError("Connection to Redis host:\"{0}:{1}\" {2} error,Connect time:{3}ms.",
                        _host,
                        _port,
                        IsReadonlyPool ? "readonly-pool[" + PoolId + "]" : "pool[" + PoolId + "]",
                        watch.Elapsed.TotalMilliseconds
                        );
                    return false;
                }
            }
        }

        /// <summary>
        /// 清空DB
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool FlushDb(int db)
        {
            CheckDisposed();
            if (_redisClient.Db != db)
            {
                return false;
            }
            _redisClient.FlushDb();
            return true;
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            CheckDisposed();
            return _redisClient.Remove(key);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            CheckDisposed();
            return _redisClient.Get<T>(key);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresMinutes"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, int expiresMinutes = 0)
        {
            CheckDisposed();
            bool result = false;
            if (expiresMinutes > 0)
            {
                result = _redisClient.Set(key, value, new TimeSpan(0, expiresMinutes, 0));
            }
            else
            {
                result = _redisClient.Set(key, value);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_poolTimeout == 0)
                {
                    _redisClient.Dispose();
                    _redisClient = null;
                    Interlocked.Exchange(ref _isActive, 0);
                }
            }
            base.Dispose(disposing);
        }
    }
}
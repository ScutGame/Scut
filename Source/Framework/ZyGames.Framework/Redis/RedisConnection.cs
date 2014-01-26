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
using ServiceStack.Redis;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Redis
{
    /// <summary>
    /// Redis连接对象
    /// </summary>
    public class RedisConnection : BaseDisposable
    {
        private RedisClient _redisClient;
        private string _host;
        private int _port;
        private readonly string _password;
        private readonly int _db;
        private bool _isConnected;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        /// <param name="db"></param>
        public RedisConnection(string host, int port, string password, int db = 0)
        {
            _host = host;
            _port = port;
            _password = password;
            _db = db;
            _redisClient = string.IsNullOrEmpty(_password)
                               ? new RedisClient(_host, _port)
                               : new RedisClient(_host, _port, _password);
            
            _isConnected = _redisClient.DbSize > -1;

            if (_redisClient.Db < _db || _redisClient.Db > _db)
            {
                _redisClient.Db = _db;
            }
        }

        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host
        {
            get { return _host; }
        }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get { return _port; }
        }

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected
        {
            get { return _isConnected; }
        }

        /// <summary>
        /// 清空Redis
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool FlushDb(int db)
        {
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
        /// 获取自增Id
        /// </summary>
        /// <returns></returns>
        public long Increment(string key)
        {
            return _redisClient.Increment(key, 1);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int ZAdd(string setId, byte[] value)
        {
            return ZAdd(setId, value, DateTime.Now.Ticks);
        }

        /// <summary>
        /// 增加到指定setId的List表中
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public int ZAdd(string setId, byte[] value, long score)
        {
            return _redisClient.ZAdd(setId, score, value);
        }

        /// <summary>
        /// 是否存在Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _redisClient.ContainsKey(key);
        }

        /// <summary>
        /// 获得指定范围的字节数组
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public byte[][] ZRange(string setId, int min, int max)
        {
            return _redisClient.ZRange(setId, min, max);
        }

        /// <summary>
        /// 移除指定的setId键的项
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public int ZRemove(string setId, byte[] item)
        {
            return _redisClient.ZRem(setId, item);
        }
        /// <summary>
        /// 获得按score排序的指定范围的字节数组
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public byte[][] ZRangeByScore(string setId, long min, long max, int? skip, int? take)
        {
            return _redisClient.ZRangeByScore(setId, min, max, skip, take);
        }

        /// <summary>
        /// 重置Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newKey"></param>
        public void Rename(string key, string newKey)
        {
            _redisClient.Rename(key, newKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isConnected = false;
                _redisClient.Dispose();
                _redisClient = null;
            }
            base.Dispose(disposing);
        }
    }
}
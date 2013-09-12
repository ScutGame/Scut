using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Socket会话管理
    /// </summary>
    internal class SocketSessionPool
    {
        private ConcurrentDictionary<string, SocketAsyncEventArgs> _pools;

        /// <summary>
        /// 
        /// </summary>
        public SocketSessionPool()
        {
            _pools = new ConcurrentDictionary<string, SocketAsyncEventArgs>();
        }

        /// <summary>
        /// 连接池数
        /// </summary>
        public int Count
        {
            get
            {
                return _pools.Count;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteAddress"></param>
        /// <returns></returns>
        public SocketAsyncEventArgs Find(string remoteAddress)
        {
            SocketAsyncEventArgs saea;
            if (_pools.TryGetValue(remoteAddress, out saea))
            {
                return saea;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, SocketAsyncEventArgs>> GetEnumerator()
        {
            return _pools.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgs"></param>
        public bool Put(SocketAsyncEventArgs eventArgs)
        {
            var session = eventArgs.UserToken as SocketSession;
            if (session != null)
            {
                string key = session.RemoteAddress;
                SocketAsyncEventArgs oldval;
                if (_pools.TryGetValue(key, out oldval))
                {
                    _pools.TryUpdate(key, eventArgs, oldval);
                }
                return _pools.TryAdd(key, eventArgs);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgs"></param>
        public bool Remove(SocketAsyncEventArgs eventArgs)
        {
            var session = eventArgs.UserToken as SocketSession;
            if (session != null)
            {
                string key = session.RemoteAddress;
                SocketAsyncEventArgs oldval;
                return _pools.TryRemove(key, out oldval);
            }
            return false;
        }


    }
}

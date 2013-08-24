using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Socket会话管理
    /// </summary>
    public sealed class SocketSessionPool
    {
        private static SocketSessionPool _currentPool;

        /// <summary>
        /// 
        /// </summary>
        public static SocketSessionPool Current
        {
            get { return _currentPool; }
        }

        static SocketSessionPool()
        {
            _currentPool = new SocketSessionPool();
        }
        
        private List<SocketAsyncEventArgs> _pools;

        private SocketSessionPool()
        {
            _pools = new List<SocketAsyncEventArgs>();
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
        /// <returns></returns>
        public List<SocketAsyncEventArgs> ToList()
        {
            lock (_pools)
            {
                var list = new List<SocketAsyncEventArgs>();
                var er = _pools.GetEnumerator();
                while (er.MoveNext())
                {
                    list.Add(er.Current);
                }
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public void Add(SocketAsyncEventArgs session)
        {
            lock (_pools)
            {
                _pools.Add(session);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public void Remove(SocketAsyncEventArgs session)
        {
            lock (_pools)
            {
                _pools.Remove(session);
            }
        }


    }
}

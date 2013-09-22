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
        private ConcurrentDictionary<Guid, SocketObject> _pools;

        /// <summary>
        /// 
        /// </summary>
        public SocketSessionPool()
        {
            _pools = new ConcurrentDictionary<Guid, SocketObject>();
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
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public SocketObject Find(Guid sessionId)
        {
            SocketObject saea;
            if (_pools.TryGetValue(sessionId, out saea))
            {
                return saea;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<Guid, SocketObject>> GetEnumerator()
        {
            return _pools.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketObject"></param>
        public bool Put(SocketObject socketObject)
        {
            SocketObject oldval;
            if (_pools.TryGetValue(socketObject.SessionId, out oldval))
            {
                _pools.TryUpdate(socketObject.SessionId, socketObject, oldval);
            }
            return _pools.TryAdd(socketObject.SessionId, socketObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketObject"></param>
        public bool Remove(SocketObject socketObject)
        {
            SocketObject oldval;
            return _pools.TryRemove(socketObject.SessionId, out oldval);
        }


    }
}

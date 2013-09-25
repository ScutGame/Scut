using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 接收传送的异步操作池  
    /// </summary>
    internal class SocketAsyncPool : IDisposable
    {
        private readonly int _capacity;
        private ConcurrentStack<SocketAsyncEventArgs> _pool;

        /// <summary>
        /// 
        /// </summary>
        public SocketAsyncPool(int capacity)
        {
            _capacity = capacity;
            _pool = new ConcurrentStack<SocketAsyncEventArgs>();
        }

        public void Push(SocketAsyncEventArgs e)
        {
            _pool.Push(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool TryPop(out SocketAsyncEventArgs e)
        {
            return _pool.TryPop(out e);
        }
        /// <summary>
        /// 取出SocketAsyncEventArgs对象
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Pop()
        {
            SocketAsyncEventArgs e;
            if (TryPop(out e))
            {
                return e;
            }
            throw new Exception(string.Format("Outof capacity of SAEA pool:{0}", _capacity));
        }

        public void Dispose()
        {
            SocketAsyncEventArgs e;
            while (_pool.TryPop(out e))
            {
                e.Dispose();
            }
        }
    }
}

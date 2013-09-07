using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 缓存区管理类
    /// </summary>
    internal sealed class BufferManager
    {
        private Byte[] _buffer;
        private readonly Int32 _bufferSize;
        private readonly long _numSize;
        private Int32 _currentIndex;
        private Stack<Int32> _freeIndexPool;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numSize">缓存区的总大小</param>
        /// <param name="bufferSize">缓冲区大小</param>
        public BufferManager(long numSize, int bufferSize)
        {
            this._numSize = numSize;
            this._bufferSize = bufferSize;

        }

        public void Init()
        {
            _buffer = new byte[_numSize];
            _freeIndexPool = new Stack<int>((int)(_numSize / _bufferSize));
        }

        internal byte[] BufferData
        {
            get { return _buffer; }
        }
        /// <summary>
        /// 开放缓存区
        /// </summary>
        /// <param name="args"></param>
        internal void FreeBuffer(SocketAsyncEventArgs args)
        {
            int offset = args.Offset;
            args.SetBuffer(null, 0, 0);
            _freeIndexPool.Push(offset);
        }

        /// <summary>
        /// 设置Socket接收的缓冲区位置
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal Boolean SetBuffer(SocketAsyncEventArgs args)
        {
            int offset = 0;
            if (_freeIndexPool.Count > 0)
            {
                offset = _freeIndexPool.Pop();
                args.SetBuffer(_buffer, offset, _bufferSize);
            }
            else
            {
                if ((_numSize - _bufferSize) < _currentIndex)
                {
                    return false;
                }
                offset = _currentIndex;
                args.SetBuffer(_buffer, offset, _bufferSize);
                _currentIndex += _bufferSize;
            }
            return true;
        }
    }
}

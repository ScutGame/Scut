using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 缓冲区包
    /// </summary>
    public class BufferPacket
    {
        private readonly List<byte> _byteList;
        private const int PrexfixCount = 4;

        /// <summary>
        /// 
        /// </summary>
        public BufferPacket(int bufferSize = 4096)
        {
            _byteList = new List<byte>(bufferSize);
        }

        private int _offset;

        /// <summary>
        /// 
        /// </summary>
        public int Offset
        {
            get { return _offset; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            Interlocked.Exchange(ref RemainingByteCount, 0);
            Interlocked.Exchange(ref _offset, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _byteList.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void InsertByteArray(byte[] data)
        {
            lock (_byteList)
            {
                Interlocked.Exchange(ref RemainingByteCount, RemainingByteCount + data.Length);
                _byteList.AddRange(data);
            }
        }

        /// <summary>
        /// 是否有完整的包
        /// </summary>
        /// <returns></returns>
        public bool HasCompleteBytes
        {
            get
            {
                int prexfixBytesCount = GetPrefixByteCount();
                return prexfixBytesCount > -1 && prexfixBytesCount <= _byteList.Count - Offset;
            }
        }

        /// <summary>
        /// 剩余字节数
        /// </summary>
        internal int RemainingByteCount;

        /// <summary>
        /// 设置扣除剩余字节数
        /// </summary>
        /// <param name="count"></param>
        public void SetRemainingByteCount(int count)
        {
            Interlocked.Exchange(ref _offset, _offset + count);
            Interlocked.Exchange(ref RemainingByteCount, RemainingByteCount - count);
        }
        /// <summary>
        /// 读取一块缓冲区
        /// </summary>
        /// <returns></returns>
        internal byte[] ReadBlockData(int bufferSize)
        {
            lock (_byteList)
            {
                byte[] data = new byte[0];
                int count = RemainingByteCount;
                if (count <= 0)
                {
                    return data;
                }
                if (count <= bufferSize)
                {
                    data = new byte[count];
                }
                else
                {
                    data = new byte[bufferSize];
                }
                if (Offset < _byteList.Count && data.Length > 0)
                {
                    _byteList.CopyTo(Offset, data, 0, data.Length);
                }
                //Interlocked.Exchange(ref _offset, _offset + count);
                //Interlocked.Exchange(ref RemainingByteCount, RemainingByteCount - data.Length);
                return data;
            }
        }

        /// <summary>
        /// 获取指定长度数据
        /// </summary>
        /// <returns></returns>
        public bool TryGetData(out byte[] data)
        {
            data = null;
            if (_byteList.Count < PrexfixCount)
                return false;

            int res = GetPrefixByteCount();
            if (res <= 0)
            {
                ResetBuffer();
                return false;
            }

            if (res > (_byteList.Count - Offset))
                return false;
            res += PrexfixCount;
            data = new byte[res];

            _byteList.CopyTo(Offset, data, 0, data.Length);
            Interlocked.Exchange(ref _offset, _offset + res);
            //Interlocked.Exchange(ref RemainingByteCount, RemainingByteCount - data.Length);

            if (Offset == _byteList.Count)
            {
                ResetBuffer();
            }
            return true;
        }

        /// <summary>
        /// 获得包的大小
        /// </summary>
        /// <returns></returns>
        private int GetPrefixByteCount()
        {
            int res = 0;
            for (int i = 0; i < PrexfixCount; i++)
            {
                if (Offset + i < _byteList.Count)
                {
                    int temp = ((int)_byteList[Offset + i]) & 0xff;
                    temp <<= i * 8;
                    res = temp + res;
                }
            }
            return res;
        }

        private void ResetBuffer()
        {
            lock (_byteList)
            {
                Reset();
                _byteList.Clear();
            }
        }
    }
}

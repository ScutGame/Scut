using System;
using System.Collections.Concurrent;
using System.Threading;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 缓冲区包
    /// </summary>
    internal class BufferPacket
    {
        private ConcurrentQueue<byte> _messageQueue;
        private byte[] _byteArrayForPrefix;
        private int _prefixBytesDone;
        private const int PrexfixCount = 4;
        private int _offset;
        /// <summary>
        /// 完成包大小
        /// </summary>
        private int _messageLength;

        /// <summary>
        /// 消息被传送的字节数
        /// </summary>
        private int _messageBytesDone;

        /// <summary>
        /// 
        /// </summary>
        public BufferPacket()
        {
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            _offset = 0;
            _prefixBytesDone = 0;
            _messageBytesDone = 0;
            _byteArrayForPrefix = new byte[PrexfixCount];
            for (int i = 0; i < _byteArrayForPrefix.Length; i++)
            {
                _byteArrayForPrefix[i] = 0;
            }
            _messageQueue = new ConcurrentQueue<byte>();
        }

        public int Length
        {
            get { return _prefixBytesDone + _messageQueue.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void InsertByteArray(byte[] data)
        {
            PushToQueue(data);
            ProcessPrefixBytes();
        }


        private void PushToQueue(byte[] data)
        {
            foreach (var b in data)
            {
                _messageQueue.Enqueue(b);
            }
        }

        private byte[] ReadPrefixBytes()
        {
            byte[] buffer = new byte[PrexfixCount];
            for (int i = 0; i < buffer.Length; i++)
            {
                byte value;
                if (_messageQueue.TryDequeue(out value))
                {
                    buffer[i] = value;
                }
            }
            return buffer;
        }

        private void ProcessPrefixBytes()
        {
            if (_prefixBytesDone == PrexfixCount || RemainingByteCount < PrexfixCount)
            {
                return;
            }

            byte[] data = ReadPrefixBytes();
            int remainingBytesToProcess = data.Length;
            //设置消息包头长度
            if (remainingBytesToProcess >= PrexfixCount - _prefixBytesDone)
            {
                for (int i = 0; i < PrexfixCount - _prefixBytesDone; i++)
                {
                    _byteArrayForPrefix[_prefixBytesDone + i] = data[i];
                }
                remainingBytesToProcess = remainingBytesToProcess - PrexfixCount + _prefixBytesDone;
                _prefixBytesDone = PrexfixCount;
                _messageLength = BitConverter.ToInt32(_byteArrayForPrefix, 0);
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    _byteArrayForPrefix[_prefixBytesDone + i] = data[i];
                }
                _prefixBytesDone += remainingBytesToProcess;
                remainingBytesToProcess = 0;
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
                return _messageQueue.Count >= _messageLength;
            }
        }

        /// <summary>
        /// 剩余字节数
        /// </summary>
        internal int RemainingByteCount
        {
            get
            {
                return _messageQueue.Count;
            }
        }

        public int MessageByteDone
        {
            get { return _messageBytesDone; }
        }

        /// <summary>
        /// 设置处理的字节数
        /// </summary>
        /// <param name="count"></param>
        public void SetDoneByteCount(int count)
        {
            _messageBytesDone += count;
        }

        /// <summary>
        /// 读取一块缓冲区,增加偏移量，不区分完成包格式
        /// </summary>
        /// <returns></returns>
        internal byte[] ReadBlockData(int bufferSize)
        {
            int readLength = bufferSize <= Length ? bufferSize : Length;
            if (readLength <= 0)
            {
                return new byte[0];
            }
            int readBytesCount = 0;
            byte[] buffer = new byte[readLength];
            //读取头部
            if (readBytesCount < _prefixBytesDone)
            {
                Buffer.BlockCopy(_byteArrayForPrefix, 0, buffer, 0, _prefixBytesDone);
                readBytesCount += _prefixBytesDone;
                _prefixBytesDone = 0;
            }
            while (readBytesCount < buffer.Length)
            {
                byte value;
                if (_messageQueue.TryDequeue(out value))
                {
                    buffer[readBytesCount] = value;
                    readBytesCount++;
                }
            }
            return buffer;

        }

        /// <summary>
        /// 检查是否有完整的包，包格式: 包长度（int占4字节长度） + 包内容 
        /// </summary>
        /// <returns></returns>
        public bool CheckCompletePacket(out byte[] buffer)
        {
            buffer = null;
            try
            {
                if (!HasCompleteBytes)
                {
                    return false;
                }
                int readBytesCount = 0;
                buffer = new byte[_prefixBytesDone + _messageLength];
                //读取头部
                if (readBytesCount < _prefixBytesDone)
                {
                    Buffer.BlockCopy(_byteArrayForPrefix, 0, buffer, 0, _prefixBytesDone);
                    readBytesCount += _prefixBytesDone;
                    _prefixBytesDone = 0;
                }

                while (readBytesCount < buffer.Length)
                {
                    byte value;
                    if (_messageQueue.TryDequeue(out value))
                    {
                        buffer[readBytesCount] = value;
                        readBytesCount++;
                    }
                }

                //重新计算下个包
                ProcessPrefixBytes();
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("CheckCompletePacket packetLength:{0},remainingByte:{1},doneByte:{2}\r\nerror:{3}", Length, RemainingByteCount, _messageBytesDone, ex);
                return false;
            }
        }


    }
}

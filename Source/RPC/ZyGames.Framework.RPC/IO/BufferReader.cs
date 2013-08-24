using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 缓冲区读取器
    /// </summary>
    public class BufferReader
    {
        private readonly byte[] _buffer;
        private int _offset;
        private int _length;

        public BufferReader(byte[] buffer)
        {
            _offset = 0;
            _buffer = buffer;
            _length = buffer.Length;
        }

        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int Length
        {
            get { return _length; }
        }

        public void Reset()
        {
            _offset = 0;
        }

        /// <summary>
        /// 是否还有数据
        /// </summary>
        /// <returns></returns>
        public bool HasByte(out byte[] values)
        {
            values = new byte[0];
            if (_offset < Length)
            {
                values = new byte[Length - _offset];
                Array.Copy(_buffer, _offset, values, 0, values.Length);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取内存流中的头4位并转换成整型
        /// </summary>
        /// <param name="values">内存流</param>
        /// <returns></returns>
        public bool ReadInt64(out long values)
        {
            try
            {
                values = BitConverter.ToInt64(_buffer, _offset);
                _offset = Interlocked.Add(ref _offset, 8);
                return true;
            }
            catch
            {
                values = 0;
                return false;
            }
        }

        public bool ReadInt32(out int values)
        {
            try
            {
                values = BitConverter.ToInt32(_buffer, _offset);
                _offset = Interlocked.Add(ref _offset, 4);
                return true;
            }
            catch
            {
                values = 0;
                return false;
            }
        }


        /// <summary>
        /// 读取内存流中的头2位并转换成整型
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ReadInt16(out short values)
        {

            try
            {
                values = BitConverter.ToInt16(_buffer, _offset);
                _offset = Interlocked.Add(ref _offset, 2);
                return true;
            }
            catch
            {
                values = 0;
                return false;
            }
        }

        /// <summary>
        /// 读取内存流中的首位
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ReadByte(out byte values)
        {
            try
            {
                values = (byte)_buffer[_offset];
                _offset = Interlocked.Increment(ref _offset);
                return true;
            }
            catch
            {
                values = 0;
                return false;
            }
        }

        /// <summary>
        /// 读取分包字节流，头部前4位是分包的长度
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ReadPacket(out byte[] values)
        {
            int length;
            try
            {
                if (ReadInt32(out length))
                {
                    values = new Byte[length];
                    Array.Copy(_buffer, _offset, values, 0, values.Length);
                    _offset = Interlocked.Add(ref _offset, length);
                    return true;
                }
                else
                {
                    values = null;
                    return false;
                }
            }
            catch
            {
                values = null;
                return false;
            }
        }

        public string ReadString()
        {
            return ReadString(Encoding.UTF8);
        }

        public string ReadString(Encoding encoding)
        {
            byte[] values = new byte[_buffer.Length - _offset];
            Array.Copy(_buffer, _offset, values, 0, values.Length);
            return encoding.GetString(values, 0, values.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ReadPacketString(out string values)
        {
            return ReadPacketString(out values, Encoding.UTF8);
        }

        /// <summary>
        /// 读取内存流中一段字符串
        /// </summary>
        /// <param name="values"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public bool ReadPacketString(out string values, Encoding encoding)
        {
            byte[] data;
            if (ReadPacket(out data))
            {
                values = encoding.GetString(data, 0, data.Length);
                return true;
            }
            values = "";
            return false;
        }

    }
}

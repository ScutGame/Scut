using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 消息结构体
    /// </summary>
    public class MessageStructure
    {
        private const int DoubleSize = 8;
        private const int FloatSize = 4;
        private const int LongSize = 8;
        private const int IntSize = 4;
        private const int ShortSize = 2;
        private const int ByteSize = 1;
        private const int BoolSize = 1;
        private ConcurrentQueue<byte> _buffers;
        private ConcurrentQueue<object> _waitWriteBuffers = new ConcurrentQueue<object>();
        private int _offset;
        private Encoding _encoding = Encoding.UTF8;

        private int _currRecordPos;
        /// <summary>
        /// 当前循环体字节长度
        /// </summary>
        private int _currRecordSize;

        public MessageStructure()
        {
            _buffers = new ConcurrentQueue<byte>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public MessageStructure(byte[] buffer)
        {
            _buffers = new ConcurrentQueue<byte>(buffer);
        }

        /// <summary>
        /// 缓冲数据大小
        /// </summary>
        public int Length
        {
            get
            {
                return _buffers.Count;
            }
        }
        /// <summary>
        /// 读取的当前位置
        /// </summary>
        public int Offset
        {
            get { return _offset; }
        }

        #region ReadByte

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ReadBool()
        {
            byte[] bytes = ReadByte(BoolSize);
            return BitConverter.ToBoolean(bytes, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            byte[] bytes = ReadByte(FloatSize);
            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            byte[] bytes = ReadByte(DoubleSize);
            return BitConverter.ToDouble(bytes, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            byte[] bytes = ReadByte(ShortSize);
            return BitConverter.ToInt16(bytes, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            byte[] bytes = ReadByte(IntSize);
            return BitConverter.ToInt32(bytes, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long ReadLong()
        {
            byte[] bytes = ReadByte(LongSize);
            return BitConverter.ToInt64(bytes, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            int count = ReadInt();
            byte[] bytes = ReadByte(count);
            return _encoding.GetString(bytes);
        }

        /// <summary>
        /// 读取字节
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            byte[] bytes = ReadByte(ByteSize);
            return bytes[0];
        }

        /// <summary>
        /// 循环开始
        /// </summary>
        public void RecordStart()
        {
            _currRecordPos = 0;
            _currRecordSize = ReadInt();
        }

        /// <summary>
        /// 循环结束
        /// </summary>
        public void RecordEnd()
        {
            int count = _currRecordSize - _currRecordPos;
            byte[] bytes = ReadByte(count);
            _currRecordSize = 0;
            _currRecordPos = 0;
        }

        /// <summary>
        /// 读取字节
        /// </summary>
        /// <param name="count">读取的个数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public byte[] ReadByte(int count)
        {
            byte[] bytes = new byte[count];
            for (int i = 0; i < count; i++)
            {
                byte b;
                if (_buffers.TryDequeue(out b))
                {
                    bytes[i] = b;
                    Interlocked.Increment(ref _offset);
                    Interlocked.Increment(ref _currRecordPos);
                }
            }
            return bytes;
        }

        /// <summary>
        /// 读取缓冲数据
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBuffer()
        {
            return ReadByte(_buffers.Count);
        }

        /// <summary>
        /// 读取缓冲数据至结尾转为字符串
        /// </summary>
        /// <returns></returns>
        public string ReadEndAsString()
        {
            return ReadEndAsString(Encoding.UTF8);
        }

        /// <summary>
        /// 读取缓冲数据至结尾转为字符串
        /// </summary>
        /// <returns></returns>
        public string ReadEndAsString(Encoding encoding)
        {
            var bytes = ReadBuffer();
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 读取头部数据
        /// </summary>
        /// <returns></returns>
        public MessageHead ReadHead()
        {
            MessageHead head = new MessageHead(); 
            head.PacketLength = Length;
            head.TotalLength = ReadInt();
            head.ErrorCode = ReadInt();
            head.MsgId = ReadInt();
            head.ErrorInfo = ReadString();
            head.Action = ReadInt();
            head.St = ReadString();
            return head;
        }

        /// <summary>
        /// Gzip压缩
        /// </summary>
        /// <returns></returns>
        public MessageHead ReadHeadGzip()
        {
            MessageHead head = new MessageHead();
            head.HasGzip = true;
            head.PacketLength = Length;
            head.GzipLength = ReadInt();
            head.TotalLength = ReadInt();
            head.ErrorCode = ReadInt();
            head.MsgId = ReadInt();
            head.ErrorInfo = ReadString();
            head.Action = ReadInt();
            head.St = ReadString();
            return head;
        }

        #endregion ReadByte end

        #region WriteByte

        public void WriteByte(bool value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//1
            WriteByte(bytes);
        }

        public void WriteByte(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//4
            WriteByte(bytes);
        }

        public void WriteByte(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//8
            WriteByte(bytes);
        }
        /// <summary>
        /// 写入long
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            WriteByte(bytes);
        }

        /// <summary>
        /// 写入Int
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//4
            WriteByte(bytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//2
            WriteByte(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(string value)
        {
            value = value ?? "";
            byte[] bytes = _encoding.GetBytes(value);
            byte[] lengthBytes = BitConverter.GetBytes(bytes.Length);
            WriteByte(lengthBytes);
            if (bytes.Length > 0)
            {
                WriteByte(bytes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(byte value)
        {
            byte[] bytes = new byte[] { value };
            WriteByte(bytes);
        }

        /// <summary>
        /// 写入字节
        /// </summary>
        /// <param name="buffer"></param>
        public void WriteByte(byte[] buffer)
        {
            foreach (var b in buffer)
            {
                _buffers.Enqueue(b);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(bool value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(byte value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(short value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(int value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(long value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(float value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(double value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(string value)
        {
            value = value ?? "";
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// 放入缓冲队列中
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(MessageStructure value)
        {
            PushObjIntoQueue(value);
        }

        /// <summary>
        /// 将缓冲队列写入缓冲区
        /// </summary>
        /// <param name="head"></param>
        public void WriteBuffer(MessageHead head)
        {
            int stackSize = GetHeadByteSize(head);
            stackSize += GetStackByteSize();
            if (head.HasGzip)
            {
                if (head.GzipLength == 0)
                {
                    head.GzipLength = stackSize;
                }
                WriteByte(head.GzipLength);
            }
            //输出总字节长度
            WriteByte(stackSize);
            WriteHeadBufer(head);
            WriteStackToBuffer();
        }

        private void PushObjIntoQueue(object value)
        {
            _waitWriteBuffers.Enqueue(value);
        }

        private void WriteHeadBufer(MessageHead head)
        {
            WriteByte(head.ErrorCode);
            WriteByte(head.MsgId);
            WriteByte(head.ErrorInfo);
            WriteByte(head.Action);
            WriteByte(head.St);
        }

        private int GetHeadByteSize(MessageHead head)
        {
            int length = 0;
            length += GetByteSize(head.ErrorCode);
            length += GetByteSize(head.MsgId);
            length += GetByteSize(head.ErrorInfo);
            length += GetByteSize(head.Action);
            length += GetByteSize(head.St);
            return length;
        }

        /// <summary>
        /// 处理队列写入缓冲区
        /// </summary>
        private void WriteStackToBuffer()
        {
            while (_waitWriteBuffers.Count > 0)
            {
                object item;
                if (_waitWriteBuffers.TryDequeue(out item))
                {
                    if (item is string)
                    {
                        WriteByte(Convert.ToString(item));
                    }
                    else if (item is int)
                    {
                        WriteByte(Convert.ToInt32(item));
                    }
                    else if (item is short)
                    {
                        WriteByte(Convert.ToInt16(item));
                    }
                    else if (item is byte)
                    {
                        WriteByte(Convert.ToSByte(item));
                    }
                    else if (item is bool)
                    {
                        WriteByte(Convert.ToBoolean(item));
                    }
                    else if (item is double)
                    {
                        WriteByte(Convert.ToDouble(item));
                    }
                    else if (item is float)
                    {
                        WriteByte(Convert.ToSingle(item));
                    }
                    else if (item is MessageStructure)
                    {
                        var ds = item as MessageStructure;
                        int stackSize = ds.GetStackByteSize();
                        ds.WriteByte(stackSize);
                        ds.WriteStackToBuffer();
                        WriteByte(ds.ReadBuffer());
                    }
                }
            }
        }

        /// <summary>
        /// 获取占用字节大小
        /// </summary>
        /// <returns></returns>
        private int GetStackByteSize()
        {
            int length = 0;
            var er = _waitWriteBuffers.GetEnumerator();
            while (er.MoveNext())
            {
                var item = er.Current;
                length += GetByteSize(item);
            }
            return length + IntSize;
        }

        /// <summary>
        /// 占用字节大小
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private int GetByteSize(object item)
        {
            int length = 0;
            if (item is string)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(item.ToString());
                length += bytes.Length + IntSize;
            }
            else if (item is int)
            {
                length += IntSize;
            }
            else if (item is short)
            {
                length += ShortSize;
            }
            else if (item is byte)
            {
                length += ByteSize;
            }
            else if (item is bool)
            {
                length += BoolSize;
            }
            else if (item is double)
            {
                length += DoubleSize;
            }
            else if (item is float)
            {
                length += FloatSize;
            }
            else if (item is MessageStructure)
            {
                length += (item as MessageStructure).GetStackByteSize();
            }
            return length;
        }
        #endregion Writebyte
    }
}

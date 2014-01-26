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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using ZyGames.Framework.Common;

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
        /// <summary>
        /// 启用Gzip压缩的最小字节
        /// </summary>
        private const int EnableGzipMinByte = 1024;
        private ConcurrentQueue<byte> _buffers;
        private ConcurrentQueue<object> _waitWriteBuffers = new ConcurrentQueue<object>();
        private int _offset;
        private Encoding _encoding = Encoding.UTF8;

        private int _currRecordPos;
        /// <summary>
        /// 当前循环体字节长度
        /// </summary>
        private int _currRecordSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        public static MessageStructure Create(Stream stream, Encoding encoding = null)
        {
            List<byte> data = new List<byte>();
            BinaryReader readStream = encoding == null
                ? new BinaryReader(stream)
                : new BinaryReader(stream, encoding);
            int size = 0;
            while (true)
            {
                var buffer = new byte[512];
                size = readStream.Read(buffer, 0, buffer.Length);
                if (size == 0)
                {
                    break;
                }
                byte[] temp = new byte[size];
                Buffer.BlockCopy(buffer, 0, temp, 0, size);
                data.AddRange(temp);
            }
            return new MessageStructure(data);
        }

        /// <summary>
        /// 
        /// </summary>
        public MessageStructure()
        {
            EnableGzip = true;
            _buffers = new ConcurrentQueue<byte>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public MessageStructure(IEnumerable<byte> buffer)
        {
            EnableGzip = true;
            _buffers = new ConcurrentQueue<byte>(buffer);
        }

        /// <summary>
        /// 开启Gzip压缩
        /// </summary>
        public bool EnableGzip { get; set; }
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
        /// 获取缓冲区字节数据，不移除
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] PeekByte(int count)
        {
            byte[] data = new byte[count];
            int index = 0;
            foreach (var buffer in _buffers)
            {
                data[index] = buffer;
                index++;
                if (index >= count)
                {
                    break;
                }
            }
            return data;
        }

        /// <summary>
        /// 读取缓冲区从偏移量开始到结束的字节流
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
        /// 读取消息头
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
        /// 读取消息头，如果有Gzip压缩时自动解压字节流
        /// </summary>
        /// <returns></returns>
        public MessageHead ReadHeadGzip()
        {
            MessageHead head = new MessageHead();
            head.HasGzip = true;
            head.PacketLength = Length;
            //head.GzipLength = ReadInt();
            if (CheckGzipBuffer())
            {
                byte[] gzipData = ReadBuffer();
                //格式500+gzip( 1000+ XXXXXX)
                byte[] deZipData = GzipUtils.DeCompress(gzipData, 0, gzipData.Length);
                WriteByte(deZipData);
            }
            head.TotalLength = ReadInt();
            head.ErrorCode = ReadInt();
            head.MsgId = ReadInt();
            head.ErrorInfo = ReadString();
            head.Action = ReadInt();
            head.St = ReadString();
            return head;
        }

        /// <summary>
        /// 检查是否有压缩数据
        /// </summary>
        /// <returns></returns>
        private bool CheckGzipBuffer()
        {
            return CheckEnableGzip(PeekByte(IntSize));
        }

        /// <summary>
        /// 检查是否有Gzip压缩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool CheckEnableGzip(byte[] data)
        {
            if (data != null && data.Length > 3)
            {
                return data[0] == 0x1f && data[1] == 0x8b && data[2] == 0x08 && data[3] == 0x00;
            }
            return false;
        }

        #endregion ReadByte end

        #region WriteByte
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(bool value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//1
            WriteByte(bytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//4
            WriteByte(bytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
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
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public void WriteByte(byte[] buffer)
        {
            WriteByte(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 写入字节
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void WriteByte(byte[] buffer, int offset, int count)
        {
            if (buffer.Length < count)
            {
                throw new ArgumentOutOfRangeException("count", "buffer size outof range");
            }
            for (int i = offset; i < count; i++)
            {
                _buffers.Enqueue(buffer[i]);
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
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        public void PushIntoStack(Type type, object obj)
        {
            if (type == typeof(string))
            {
                PushIntoStack(obj.ToNotNullString());
            }
            else if (type == typeof(long))
            {
                PushIntoStack(obj.ToLong());
            }
            else if (type == typeof(int))
            {
                PushIntoStack(obj.ToInt());
            }
            else if (type == typeof(short))
            {
                PushIntoStack(obj.ToShort());
            }
            else if (type == typeof(byte))
            {
                PushIntoStack(obj.ToByte());
            }
            else if (type == typeof(bool))
            {
                PushIntoStack(obj.ToBool());
            }
            else if (type == typeof(double))
            {
                PushIntoStack(obj.ToDouble());
            }
            else if (type == typeof(float))
            {
                PushIntoStack((float)Convert.ChangeType(obj, type));
            }
            else if (type == typeof(decimal))
            {
                PushIntoStack(obj.ToNotNullString());
            }
            else if (type == typeof(DateTime))
            {
                PushIntoStack(obj.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (type.IsEnum)
            {
                PushIntoStack((int)obj);
            }
            else if (type == typeof(MessageStructure))
            {
                PushIntoStack(obj as MessageStructure);
            }
        }
        /// <summary>
        /// 写入启用Gzip压缩的字节流，格式： Len(4) + gzip(buffer)
        /// </summary>
        /// <param name="buffer">未压缩的字节流</param>
        public void WriteGzipBuffer(byte[] buffer)
        {
            int orgStackSize = BitConverter.ToInt32(buffer, 0);
            if (EnableGzip && orgStackSize > EnableGzipMinByte)
            {
                WriteGzipCompressBuffer(buffer);
            }
            else
            {
                WriteByte(orgStackSize);
                WriteByte(buffer);
            }
        }

        /// <summary>
        /// 自动压缩
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>返回压缩后的字节</returns>
        public byte[] CompressBuffer(byte[] buffer)
        {
            if (buffer.Length > EnableGzipMinByte)
            {
                return GzipUtils.EnCompress(buffer, 0, buffer.Length);
            }
            return buffer;
        }

        /// <summary>
        /// 将缓冲队列写入缓冲区,大于1024开始压缩
        /// </summary>
        /// <param name="head"></param>
        public void WriteBuffer(MessageHead head)
        {
            int stackSize = GetHeadByteSize(head);
            stackSize += GetStackByteSize();//包括了头部长度大小
            head.TotalLength = stackSize;
            bool result = false;
            if (head.HasGzip && (!EnableGzip || (EnableGzip && stackSize <= EnableGzipMinByte)))
            {
                result = true;
                head.GzipLength = stackSize;
                WriteByte(head.GzipLength);
            }
            //输出总字节长度,公式: stackSize = len(头) + len(内容) + 4(int)
            WriteByte(stackSize);
            WriteHeadBufer(head);
            WriteStackToBuffer();
            //判断是否有启用Gzip
            if (head.HasGzip && EnableGzip && !result && stackSize > EnableGzipMinByte)
            {
                byte[] orgData = ReadBuffer();
                head.GzipLength = WriteGzipCompressBuffer(orgData);
            }
            head.PacketLength = Length;
        }

        /// <summary>
        /// 写入Gzip压缩的Buffer，返回压缩后的长度
        /// </summary>
        /// <returns></returns>
        private int WriteGzipCompressBuffer(byte[] orgData)
        {
            byte[] buffer = GzipUtils.EnCompress(orgData, 0, orgData.Length);
            int gzipLength = buffer.Length;
            WriteByte(gzipLength);//分包时通过此长度计算来组包
            WriteByte(buffer);
            return gzipLength;
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
                    else if (item is long)
                    {
                        WriteByte(Convert.ToInt64(item));
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
            else if (item is long)
            {
                length += LongSize;
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
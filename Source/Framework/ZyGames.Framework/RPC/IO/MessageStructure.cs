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
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Serialization;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 消息结构体
    /// </summary>
    public class MessageStructure : BaseDisposable
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
        public static int EnableGzipMinByte { get; set; }

        /// <summary>
        /// 开启Gzip压缩
        /// </summary>
        public static bool EnableGzip { get; set; }

        static MessageStructure()
        {
            EnableGzip = true;
            EnableGzipMinByte = 10240;
        }

        /// <summary>
        /// Use stream create MessageStructure object.
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
                var buffer = new byte[1024];
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

        //private int _bufferLength;
        //private byte[] _buffers;
        //private ConcurrentQueue<byte> _buffersQueue;
        private MemoryStream _msBuffers;
        private ConcurrentQueue<object> _waitWriteObjects = new ConcurrentQueue<object>();
        //private int _offset;
        private Encoding _encoding = Encoding.UTF8;

        private Stack<int> _currRecordPos = new Stack<int>();
        /// <summary>
        /// 当前循环体字节长度
        /// </summary>
        private Stack<int> _currRecordSize = new Stack<int>();

        /// <summary>
        /// 
        /// </summary>
        public MessageStructure()
        {
            _msBuffers = new MemoryStream();
            //_buffersQueue = new ConcurrentQueue<byte>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public MessageStructure(IEnumerable<byte> buffer)
        {
            _msBuffers = new MemoryStream(buffer.ToArray());
            //_buffersQueue = new ConcurrentQueue<byte>(buffer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            _waitWriteObjects = null;
            Reset();
            _msBuffers.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 缓冲数据大小
        /// </summary>
        public long Length
        {
            get
            {
                return _msBuffers.Length;
                //return _buffersQueue.Count;
            }
        }
        /// <summary>
        /// 读取的当前位置
        /// </summary>
        public long Offset
        {
            get { return _msBuffers.Position; }
            set { _msBuffers.Position = value; }
        }

        #region ReadByte

        /// <summary>
        /// Reset from 0 pos start read.
        /// </summary>
        public void Reset()
        {
            _currRecordPos.Clear();
            _msBuffers.Position = 0;
        }

        /// <summary>
        /// Clear buffer.
        /// </summary>
        public void Clear()
        {
            _msBuffers = new MemoryStream();
            Reset();
        }

        /// <summary>
        /// Read bool
        /// </summary>
        /// <returns></returns>
        public bool ReadBool()
        {
            byte[] bytes = ReadByte(BoolSize);
            return BitConverter.ToBoolean(bytes, 0);
        }

        /// <summary>
        /// Read float
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
        /// Read short
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            byte[] bytes = ReadByte(ShortSize);
            return BitConverter.ToInt16(bytes, 0);
        }
        /// <summary>
        /// Read int
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            byte[] bytes = ReadByte(IntSize);
            return BitConverter.ToInt32(bytes, 0);
        }
        /// <summary>
        /// Read long
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
        public UInt16 ReadUInt16()
        {
            byte[] bytes = ReadByte(ShortSize);
            return BitConverter.ToUInt16(bytes, 0);
        }
        /// <summary>
        /// Read int
        /// </summary>
        /// <returns></returns>
        public UInt32 ReadUInt32()
        {
            byte[] bytes = ReadByte(IntSize);
            return BitConverter.ToUInt32(bytes, 0);
        }
        /// <summary>
        /// Read long
        /// </summary>
        /// <returns></returns>
        public UInt64 ReadUInt64()
        {
            byte[] bytes = ReadByte(LongSize);
            return BitConverter.ToUInt64(bytes, 0);
        }
        /// <summary>
        /// Read string
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            int count = ReadInt();
            byte[] bytes = ReadByte(count);
            return _encoding.GetString(bytes);
        }
        /// <summary>
        /// Read DateTime, use UTC time
        /// </summary>
        /// <returns></returns>
        public DateTime ReadDateTime()
        {
            long time = ReadLong();
            return MathUtils.ToTimeFromUnixEpoch(TimeSpan.FromSeconds(time)).ToLocalTime();
        }
        /// <summary>
        /// Read object of Protobuf serialize.
        /// </summary>
        /// <returns></returns>
        public object ReadObject(Type type)
        {
            int count = ReadInt();
            byte[] bytes = ReadByte(count);
            return ProtoBufUtils.Deserialize(bytes, type);
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
        /// Read record count.
        /// </summary>
        /// <returns></returns>
        public int ReadRecordCount()
        {
            return ReadInt();
        }

        /// <summary>
        /// 循环开始
        /// </summary>
        public void RecordStart()
        {
            _currRecordPos.Push(0);
            _currRecordSize.Push(ReadInt());
        }

        /// <summary>
        /// 循环结束
        /// </summary>
        public byte[] RecordEnd()
        {
            int length = _currRecordSize.Count > 0 ? _currRecordSize.Pop() : 0;
            int pos = _currRecordPos.Count > 0 ? _currRecordPos.Pop() : 0;
            int count = length - pos;
            byte[] bytes = new byte[0];
            if (count > 0)
            {
                bytes = ReadByte(count);
                if (_currRecordPos.Count > 0) _currRecordPos.Pop();
            }
            if (_currRecordPos.Count > 0)
            {
                int parentPos = _currRecordPos.Pop();
                _currRecordPos.Push(parentPos + length);
            }
            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBytes()
        {
            int len = ReadInt();
            return ReadByte(len);
        }

        /// <summary>
        /// 读取字节
        /// </summary>
        /// <param name="count">读取的个数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public byte[] ReadByte(int count)
        {
            VerifyBufferLength(count);
            byte[] bytes = new byte[count];
            int len = _msBuffers.Read(bytes, 0, count);
            int pos = _currRecordPos.Count > 0 ? _currRecordPos.Pop() : 0;
            pos += len;
            _currRecordPos.Push(pos);
            return bytes;
        }

        private void VerifyBufferLength(int count)
        {
            long len = Length - Offset;
            if (count < 0 || count > len)
            {
                throw new ArgumentOutOfRangeException(string.Format("Read {0} byte len overflow max {1} len.", count, len));
            }
        }

        /// <summary>
        /// 获取缓冲区字节数据，不移除
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public byte[] PeekByte(int count)
        {
            VerifyBufferLength(count);
            byte[] bytes = new byte[count];
            int len = _msBuffers.Read(bytes, 0, count);
            _msBuffers.Position = _msBuffers.Position - len;

            //int index = 0;
            //foreach (var buffer in _buffersQueue)
            //{
            //    data[index] = buffer;
            //    index++;
            //    if (index >= count)
            //    {
            //        break;
            //    }
            //}
            return bytes;
        }

        /// <summary>
        /// 从偏移量位置开始读取缓冲区字节流
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBuffer()
        {
            int count = (Length - Offset).ToInt();
            return ReadByte(count);
        }

        /// <summary>
        /// Read and remove buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] PopBuffer()
        {
            var bytes = _msBuffers.ToArray();
            Clear();
            return bytes;
        }

        /// <summary>
        /// Read and remove buffer of gzip.
        /// </summary>
        /// <returns></returns>
        public byte[] PosGzipBuffer()
        {
            var buffer = PopBuffer();
            if (EnableGzip && buffer.Length > EnableGzipMinByte)
            {
                buffer = CompressGzipBuffer(buffer);
            }
            return buffer;
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
        /// <exception cref="OverflowException"></exception>
        public MessageHead ReadHeadGzip()
        {
            try
            {
                MessageHead head = new MessageHead();
                head.HasGzip = true;
                head.PacketLength = Length;
                if (CheckGzipBuffer())
                {
                    byte[] gzipData = PopBuffer();
                    head.GzipLength = gzipData.Length;
                    //gzip格式500+gzip( 1000+ XXXXXX)
                    byte[] deZipData = GzipUtils.DeCompress(gzipData, 0, gzipData.Length);
                    WriteByte(deZipData);
                    Reset();
                }
                head.TotalLength = ReadInt();
                head.ErrorCode = ReadInt();
                head.MsgId = ReadInt();
                head.ErrorInfo = ReadString();
                head.Action = ReadInt();
                head.St = ReadString();
                return head;
            }
            catch (Exception ex)
            {
                string error = string.Format("read to {0}/{1}pos error,\r\nbytes:{2}", Offset, Length, ToHexString());
                throw new OverflowException(error, ex);
            }
        }

        /// <summary>
        /// To Hex(16) string
        /// </summary>
        /// <returns></returns>
        public string ToHexString()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                var buffer = _msBuffers.ToArray();
                if (buffer != null)
                {
                    int colIndex = 0;
                    int index = 0;
                    foreach (var b in buffer)
                    {
                        sb.AppendFormat(" {0}", b.ToString("X2"));
                        index++;
                        if (index % 8 == 0)
                        {
                            sb.Append(" ");
                            colIndex++;
                        }
                        if (colIndex == 2)
                        {
                            sb.AppendLine();
                            colIndex = 0;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return sb.ToString();
        }
        /// <summary>
        /// 检查是否有压缩数据
        /// </summary>
        /// <returns></returns>
        private bool CheckGzipBuffer()
        {
            byte[] gzipHead = PeekByte(IntSize);
            return CheckEnableGzip(gzipHead);
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
        /// Write bool
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(bool value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//1
            WriteByte(bytes);
        }
        /// <summary>
        /// Write float
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//4
            WriteByte(bytes);
        }
        /// <summary>
        /// Write byte
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//8
            WriteByte(bytes);
        }
        /// <summary>
        /// Write long
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            WriteByte(bytes);
        }

        /// <summary>
        /// Write Int
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//4
            WriteByte(bytes);
        }
        /// <summary>
        /// Write short
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//2
            WriteByte(bytes);
        }
        /// <summary>
        /// Write long
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(UInt64 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            WriteByte(bytes);
        }

        /// <summary>
        /// Write Int
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(UInt32 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//4
            WriteByte(bytes);
        }
        /// <summary>
        /// Write short
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(UInt16 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);//2
            WriteByte(bytes);
        }

        /// <summary>
        /// Write string
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
        /// Write object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useGzip"></param>
        public void WriteByte(object obj, bool useGzip = true)
        {
            try
            {
                var bytes = ProtoBufUtils.Serialize(obj, useGzip);
                byte[] lengthBytes = BitConverter.GetBytes(bytes.Length);
                WriteByte(lengthBytes);
                if (bytes.Length > 0)
                {
                    WriteByte(bytes);
                }
            }
            catch (Exception err)
            {
                throw new ArgumentOutOfRangeException("obj is not a valid data can be serialized.", err);
            }
        }

        /// <summary>
        /// Write byte
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(byte value)
        {
            byte[] bytes = new byte[] { value };
            WriteByte(bytes);
        }

        /// <summary>
        /// Write bytes
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
            _msBuffers.Write(buffer, offset, count);
            //for (int i = offset; i < count; i++)
            //{
            //    _buffersQueue.Enqueue(buffer[i]);
            //}
        }
        /// <summary>
        /// Push bool
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(bool value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// Push byte
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(byte value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// Push short
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(short value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// Push int
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(int value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// Push long
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(long value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// Push short
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(UInt16 value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// Push int
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(UInt32 value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// Push long
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(UInt64 value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// Push float
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(float value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// Push double
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(double value)
        {
            PushObjIntoQueue(value);
        }

        /// <summary>
        /// Push string
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(string value)
        {
            value = value ?? "";
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// 时间转为Ticks的long
        /// </summary>
        /// <param name="value"></param>
        public void PushIntoStack(DateTime value)
        {
            PushObjIntoQueue(value);
        }
        /// <summary>
        /// Push object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useGzip"></param>
        public void PushIntoStack(object obj, bool useGzip)
        {
            try
            {
                var bytes = ProtoBufUtils.Serialize(obj, useGzip);
                PushObjIntoQueue(bytes);
            }
            catch (Exception err)
            {
                throw new ArgumentOutOfRangeException("obj is not a valid data can be serialized.", err);
            }
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
            else if (type == typeof(UInt64))
            {
                PushIntoStack(obj.ToUInt64());
            }
            else if (type == typeof(UInt32))
            {
                PushIntoStack(obj.ToUInt32());
            }
            else if (type == typeof(UInt16))
            {
                PushIntoStack(obj.ToUInt16());
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
                PushIntoStack(obj.ToDateTime().Ticks);
            }
            else if (type.IsEnum)
            {
                PushIntoStack((int)obj);
            }
            else if (type == typeof(MessageStructure))
            {
                PushIntoStack(obj as MessageStructure);
            }
            else
            {
                PushIntoStack(obj, true);
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
        /// 将缓冲队列写入缓冲区,大于10k开始压缩
        /// </summary>
        /// <param name="head"></param>
        public void WriteBuffer(MessageHead head)
        {
            int stackSize = GetHeadByteSize(head);
            stackSize += GetStackByteSize();//包括了头部长度大小
            head.TotalLength = stackSize;
            bool hasGzipHead = false;
            if (head.HasGzip && (!EnableGzip || (EnableGzip && stackSize <= EnableGzipMinByte)))
            {
                hasGzipHead = true;
                head.GzipLength = stackSize;
                WriteByte(head.GzipLength);
            }
            //输出总字节长度,公式: stackSize = len(头) + len(内容) + 4(int)
            WriteByte(stackSize);
            WriteHeadBufer(head);
            WriteStackToBuffer();
            //判断是否有启用Gzip
            if (head.HasGzip && EnableGzip && !hasGzipHead && stackSize > EnableGzipMinByte)
            {
                byte[] orgData = PopBuffer();
                head.GzipLength = WriteGzipCompressBuffer(orgData);
            }
            head.PacketLength = Length;
            Reset();
        }

        /// <summary>
        /// 写入Gzip压缩的Buffer，返回压缩后的长度
        /// </summary>
        /// <returns></returns>
        private int WriteGzipCompressBuffer(byte[] orgData)
        {
            byte[] buffer = CompressGzipBuffer(orgData);
            int gzipLength = buffer.Length;
            //这里不需要再加Len长度，在socekt传送时会统一增加包的长度
            WriteByte(buffer);
            return gzipLength;
        }

        private static byte[] CompressGzipBuffer(byte[] data)
        {
            return GzipUtils.EnCompress(data, 0, data.Length);
        }

        private void PushObjIntoQueue(object value)
        {
            _waitWriteObjects.Enqueue(value);
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
            while (_waitWriteObjects.Count > 0)
            {
                object item;
                if (_waitWriteObjects.TryDequeue(out item))
                {
                    if (item is string)
                    {
                        WriteByte(Convert.ToString(item));
                    }
                    else if (item is UInt64)
                    {
                        WriteByte(Convert.ToUInt64(item));
                    }
                    else if (item is UInt32)
                    {
                        WriteByte(Convert.ToUInt32(item));
                    }
                    else if (item is UInt16)
                    {
                        WriteByte(Convert.ToUInt16(item));
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
                        WriteByte(Convert.ToByte(item));
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
                    else if (item is DateTime)
                    {
                        long ts = (item.ToDateTime().ToUniversalTime() - MathUtils.UnixEpochDateTime).TotalSeconds.ToLong();
                        WriteByte(ts);
                    }
                    else if (item is MessageStructure)
                    {
                        var ds = item as MessageStructure;
                        int stackSize = ds.GetStackByteSize();
                        ds.WriteByte(stackSize);
                        ds.WriteStackToBuffer();
                        WriteByte(ds.PopBuffer());
                    }
                    else if (item is byte[])
                    {
                        //已序列化好的内容，直接写入
                        var bytes = (byte[])(item);
                        WriteByte(bytes.Length);
                        WriteByte(bytes);
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
            var er = _waitWriteObjects.GetEnumerator();
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
            else if (item is DateTime)
            {
                length += LongSize;
            }
            else if (item is MessageStructure)
            {
                length += (item as MessageStructure).GetStackByteSize();
            }
            else if (item is byte[])
            {
                length += (item as byte[]).Length + 4;
            }
            return length;
        }
        #endregion Writebyte
    }
}
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
using System.Collections;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;

namespace ZyGames.Framework.Game.Service
{

    /// <summary>
    /// DataStruct 的摘要说明
    /// </summary>
    public class DataStruct
    {
        /// <summary>
        /// 写日志的对象
        /// </summary>
        protected static BaseLog oBaseLog = null;
        /// <summary>
        /// 数据输出栈
        /// </summary>
        protected ArrayList arrayList = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        protected DataStruct _propertyStruct;
        /// <summary>
        /// 临时记录holddata长度
        /// </summary>
        protected int htInfoHoldData = 0;
        /// <summary>
        /// 跟踪BUFFER
        /// </summary>
        private StringBuilder _traceBuffer;

        private bool _isDebug;

        /// <summary>
        /// 是否启用调试跟踪流
        /// </summary>
        private bool IsDebug
        {
            get { return _isDebug; }
            set
            {
                _isDebug = value;
                _traceBuffer = _isDebug ? new StringBuilder() : null;
            }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return arrayList.Count == 0; }
        }

        #region Push stack
        /// <summary>
        /// long类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(long obj)
        {
            PushIntoStackObj(obj);
        }

        /// <summary>
        /// Int类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(int obj)
        {
            PushIntoStackObj(obj);
        }
        /// <summary>
        /// 输出Short类型,Lua脚本只有int类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushShortIntoStack(int obj)
        {
            PushIntoStackObj((short)obj);
        }
        /// <summary>
        /// short类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(short obj)
        {
            PushIntoStackObj(obj);
        }
        /// <summary>
        /// 输出Byte类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushByteIntoStack(int obj)
        {
            PushIntoStackObj((byte)obj);
        }
        /// <summary>
        /// bool类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(bool obj)
        {
            PushIntoStackObj(obj);
        }
        /// <summary>
        /// byte类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(byte obj)
        {
            PushIntoStackObj(obj);
        }

        /// <summary>
        /// float类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(float obj)
        {
            PushIntoStackObj(obj);
        }

        /// <summary>
        /// double类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(double obj)
        {
            PushIntoStackObj(obj);
        }
        /// <summary>
        /// string类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(string obj)
        {
            obj = obj ?? "";
            PushIntoStackObj(obj);
        }
        /// <summary>
        /// DateTime类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(DateTime obj)
        {
            PushIntoStackObj(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(UInt16 obj)
        {
            PushIntoStackObj(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(UInt32 obj)
        {
            PushIntoStackObj(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(UInt64 obj)
        {
            PushIntoStackObj(obj);
        }

        /// <summary>
        /// put byte[]
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(byte[] obj)
        {
            PushIntoStackObj(obj);
        }

        /// <summary>
        /// 将数据加到栈尾
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(DataStruct obj)
        {
            PushIntoStackObj(obj);
        }

        /// <summary>
        /// 将可序列化的对象数据追加到栈尾
        /// By Seamoon
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useGzip"></param>
        public void PushIntoStack(object obj, bool useGzip)
        {
            //尝试进行protoBuf序列化
            try
            {
                //改为只序列化一次后保存起来，不会反复浪费性能!!!
                var bytes = ProtoBufUtils.Serialize(obj, useGzip);
                PushIntoStackObj(bytes);
            }
            catch (Exception err)
            {
                throw new ArgumentOutOfRangeException("obj is not a valid data can be serialized.", err);
            }
        }

        /// <summary>
        /// 将数据加到栈尾
        /// </summary>
        /// <param name="obj"></param>
        private void PushIntoStackObj(object obj)
        {
            if (IsDebug && _traceBuffer != null)
            {
                if (_traceBuffer.Length > 0)
                {
                    _traceBuffer.Append(",");
                }
                if (obj is DataStruct)
                {
                    _traceBuffer.AppendLine();
                    _traceBuffer.Append("{");
                    _traceBuffer.AppendFormat("\"Struct\":{0}", ((DataStruct)obj).GetTraceString());
                    _traceBuffer.Append("}");
                }
                else
                {
                    _traceBuffer.AppendFormat("\"Item\":\"{0}\"", obj);
                }
            }
            PushIntoStackObj(obj, -1);
        }
        #endregion

        /// <summary>
        /// 增加头部属性
        /// </summary>
        /// <param name="propertyStruct"></param>
        internal void PushHeadProperty(DataStruct propertyStruct)
        {
            _propertyStruct = propertyStruct;
        }

        /// <summary>
        /// 取出跟踪输出
        /// </summary>
        /// <returns></returns>
        public string GetTraceString()
        {
            return _traceBuffer != null ? _traceBuffer.ToString() : "";
        }
        /// <summary>
        /// 清空输出栈
        /// </summary>
        protected void CleanStack()
        {
            this.arrayList.Clear();
        }

        /// <summary>
        /// 将数据插入到栈中的指定位置，如果aIndex小于零，则加到栈尾
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="aIndex"></param>
        private void PushIntoStackObj(object obj, int aIndex)
        {
            if (aIndex < 0)
            {
                arrayList.Add(obj);
            }
            else
            {
                arrayList.Insert(aIndex, obj);
            }
            htInfoHoldData += GetObjLength(obj);
        }

        /// <summary>
        /// 计算并返回本协议内容的全部长度
        /// </summary>
        /// <returns></returns>
        protected int GetContentLen()
        {
            int iLen = 0;
            foreach (object obj in arrayList)
            {
                iLen += GetObjLength(obj);
            }
            return iLen + 4;
        }

        /// <summary>
        /// 判定传入对象类型，并计算其所占的字节数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>占用字节数</returns>
        protected int GetObjLength(object obj)
        {
            Type type = obj.GetType();
            if (type == typeof(String))
            {
                return GetStringLen(Convert.ToString(obj));
            }
            else if (type == typeof(double))
            {
                return GetDoubleLen();
            }
            else if (type == typeof(float))
            {
                return GetFloatLen();
            }
            else if (type == typeof(Int64) || type == typeof(UInt64) || type == typeof(DateTime))
            {
                return GetLongLen();
            }
            else if (type == typeof(Int32) || type == typeof(UInt32) || type.IsEnum)
            {
                return GetIntLen();
            }
            else if (type == typeof(Int16) || type == typeof(UInt16))
            {
                return GetShortLen();
            }
            else if (type == typeof(Byte))
            {
                return GetByteLen();
            }
            else if (type == typeof(Boolean))
            {
                return GetBoolLen();
            }
            else if (type == typeof(DataStruct))
            {
                return ((DataStruct)obj).GetContentLen();
            }
            else if (type == typeof(byte[]))
            {
                var bytes = (byte[])(obj);
                return bytes.Length + 4;
            }
            else
            {
                throw new ArgumentOutOfRangeException("obj", "The " + type.FullName + " type not be supported.");

                //return 0;
            }
        }
        /// <summary>
        /// 内部输出Action的值
        /// </summary>
        /// <returns>无</returns>
        protected void InternalWriteAction(BaseGameResponse response)
        {
            foreach (object obj in arrayList)
            {
                Type type = obj.GetType();
                if (type == typeof(String))
                {
                    WriteString(response, Convert.ToString(obj));
                }
                else if (type == typeof(double))
                {
                    WriteDouble(response, Convert.ToDouble(obj));
                }
                else if (type == typeof(float))
                {
                    WriteFloat(response, Convert.ToSingle(obj));
                }
                else if (type == typeof(Int64))
                {
                    WriteLong(response, Convert.ToInt64(obj));
                }
                else if (type == typeof(Int32) || type.IsEnum)
                {
                    WriteInt(response, Convert.ToInt32(obj));
                }
                else if (type == typeof(Int16))
                {
                    WriteShort(response, Convert.ToInt16(obj));
                }
                else if (type == typeof(UInt64))
                {
                    WriteULong(response, Convert.ToUInt64(obj));
                }
                else if (type == typeof(UInt32))
                {
                    WriteUInt(response, Convert.ToUInt32(obj));
                }
                else if (type == typeof(UInt16))
                {
                    WriteUShort(response, Convert.ToUInt16(obj));
                }
                else if (type == typeof(Byte))
                {
                    WriteByte(response, Convert.ToByte(obj));
                }
                else if (type == typeof(bool))
                {
                    WriteBool(response, Convert.ToBoolean(obj));
                }
                else if (type == typeof(DateTime))
                {
                    WriteDateTime(response, Convert.ToDateTime(obj));
                }
                else if (type == typeof(DataStruct))
                {
                    DataStruct ds = ((DataStruct)obj);
                    ds.WriteActionNum(response);
                    ds.InternalWriteAction(response);
                }
                else if (type == typeof(byte[]))
                {
                    //By Seamoon已序列化好的内容，直接写入
                    var bytes = (byte[])(obj);
                    WriteInt(response, bytes.Length);
                    response.Write(bytes);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="aAction"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorInfo"></param>
        /// <param name="msgId">消息初始ID</param>
        /// <param name="st"></param>
        public void WriteAction(BaseGameResponse response, int aAction, int errorCode, string errorInfo, int msgId, string st = "st")
        {
            WriteHead(response, aAction, errorCode, errorInfo, msgId, st);
        }

        /// <summary>
        /// 输出当前协议
        /// </summary>
        protected void WriteHead(BaseGameResponse response, int aAction, int errorCode, string errorInfo, int msgId, string st)
        {
            PushIntoStackObj(errorCode, 0);//int errorCode
            PushIntoStackObj(msgId, 1);//int msgid
            PushIntoStackObj(errorInfo, 2); //string info
            PushIntoStackObj(aAction, 3); //int actionId
            PushIntoStackObj(st, 4); //int St timespan
            //extent head
            if (_propertyStruct != null)
            {
                if (_propertyStruct.IsEmpty)
                {
                    PushIntoStackObj(0, 5);
                }
                else
                {
                    PushIntoStackObj(1, 5); //int property head length=1
                    PushIntoStackObj(_propertyStruct, 6);
                }
            }

            WriteActionNum(response); //total package length
            InternalWriteAction(response);
        }

        /// <summary>
        /// 计算Action协议占用的字节长度
        /// </summary>
        protected void WriteActionNum(BaseGameResponse response)
        {
            int iActionNum = GetContentLen();
            WriteInt(response, iActionNum);
        }

        #region Write stream
        /// <summary>
        /// 写入字节流
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="response"></param>
        protected static void WriteString(BaseGameResponse response, string aValue)
        {
            byte[] outputStream = System.Text.Encoding.UTF8.GetBytes(aValue);
            int iLen = outputStream.Length;
            byte[] outputStreamForLen = BitConverter.GetBytes(iLen);
            response.BinaryWrite(outputStreamForLen);
            if (outputStream.Length > 0)
            {
                response.BinaryWrite(outputStream);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="aValue"></param>
        protected static void WriteDouble(BaseGameResponse response, double aValue)
        {
            byte[] outputStream = BitConverter.GetBytes(aValue);
            response.Write(outputStream);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="aValue"></param>
        protected static void WriteFloat(BaseGameResponse response, float aValue)
        {
            byte[] outputStream = BitConverter.GetBytes(aValue);
            response.Write(outputStream);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="aValue"></param>
        protected static void WriteBool(BaseGameResponse response, Boolean aValue)
        {
            byte[] outputStream = BitConverter.GetBytes(aValue);
            response.Write(outputStream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="aValue"></param>
        protected static void WriteLong(BaseGameResponse response, Int64 aValue)
        {
            byte[] outputStream = BitConverter.GetBytes(aValue);
            response.Write(outputStream);
        }

        /// <summary>
        /// 写入字节流
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="response"></param>
        protected static void WriteInt(BaseGameResponse response, Int32 aValue)
        {
            byte[] outputStream = BitConverter.GetBytes(aValue);
            response.Write(outputStream);
        }
        /// <summary>
        /// 写入字节流
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="response"></param>
        protected static void WriteShort(BaseGameResponse response, Int16 aValue)
        {
            byte[] outputStream = BitConverter.GetBytes(aValue);
            response.Write(outputStream);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="aValue"></param>
        protected static void WriteULong(BaseGameResponse response, UInt64 aValue)
        {
            byte[] outputStream = BitConverter.GetBytes(aValue);
            response.Write(outputStream);
        }

        /// <summary>
        /// 写入字节流
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="response"></param>
        protected static void WriteUInt(BaseGameResponse response, UInt32 aValue)
        {
            byte[] outputStream = BitConverter.GetBytes(aValue);
            response.Write(outputStream);
        }
        /// <summary>
        /// 写入字节流
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="response"></param>
        protected static void WriteUShort(BaseGameResponse response, UInt16 aValue)
        {
            byte[] outputStream = BitConverter.GetBytes(aValue);
            response.Write(outputStream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="value"></param>
        protected static void WriteDateTime(BaseGameResponse response, DateTime value)
        {
            long ts = (value.ToUniversalTime() - MathUtils.UnixEpochDateTime).TotalSeconds.ToLong();
            byte[] outputStream = BitConverter.GetBytes(ts);
            response.Write(outputStream);
        }

        /// <summary>
        /// 写入字节流
        /// </summary>
        /// <param name="aByte"></param>
        /// <param name="response"></param>
        protected static void WriteByte(BaseGameResponse response, byte aByte)
        {
            byte[] outputStream = new byte[1];
            outputStream[0] = aByte;
            //m_Response.OutputStream.Write(outputStream, 0, outputStream.Length);
            response.Write(outputStream);
        }
        #endregion

        #region //计算输出对象长度
        /// <summary>
        /// 计算字符串下发时，占用的字节流长度，字符串长度+4个字节（标出字符串的实际长度）
        /// </summary>
        /// <returns></returns>
        public static int GetStringLen(string str)
        {
            byte[] outputStream = System.Text.Encoding.UTF8.GetBytes(str);
            return outputStream.Length + 4;
        }

        /// <summary>
        /// 计算Double型数据下发时，占用的字节长度
        /// </summary>
        /// <returns></returns>
        public static int GetDoubleLen()
        {
            return 8;
        }
        /// <summary>
        /// 计算Float型数据下发时，占用的字节长度
        /// </summary>
        /// <returns></returns>
        public static int GetFloatLen()
        {
            return 4;
        }

        /// <summary>
        /// 计算int64型数据下发时，占用的字节长度
        /// </summary>
        /// <returns></returns>
        public static int GetLongLen()
        {
            return 8;
        }

        /// <summary>
        /// 计算int型数据下发时，占用的字节长度
        /// </summary>
        /// <returns></returns>
        public static int GetIntLen()
        {
            return 4;
        }
        /// <summary>
        /// 计算short型数据下发时，占用的字节长度
        /// </summary>
        /// <returns></returns>
        public static int GetShortLen()
        {
            return 2;
        }
        /// <summary>
        /// 计算Byte型数据下发时，占用的字节长度
        /// </summary>
        /// <returns></returns>
        public static int GetByteLen()
        {
            return 1;
        }
        /// <summary>
        /// 计算Bool型数据下发时，占用的字节长度
        /// </summary>
        /// <returns></returns>
        public static int GetBoolLen()
        {
            return 1;
        }

        #endregion


    }
}
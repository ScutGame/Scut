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
using System.IO;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ZyGames.Framework.Common.Serialization
{
    /// <summary>
    /// 支持ProtoBuf的Object类型
    /// </summary>
    [Serializable, ProtoContract]
    public class ProtoObject : ISerializable
    {
        /// <summary>
        /// 
        /// </summary>
        public ProtoObject()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public ProtoObject(object obj)
        {
            Value = obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ProtoObject(SerializationInfo info, StreamingContext context)
        {
            byte[] buffer = (byte[])info.GetValue("proto", typeof(byte[]));
            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                Serializer.Merge(memoryStream, this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Serializer.Serialize(memoryStream, this);
                info.AddValue("proto", memoryStream.ToArray());
            }
        }

        /// <summary>
        /// 重载Tostring方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public object Value
        {
            get
            {
                if (_isnullValue.HasValue)
                    return null;
                if (_byteValue.HasValue)
                    return _byteValue.Value;
                if (_boolValue.HasValue)
                    return _boolValue.Value;
                if (_shortValue.HasValue)
                    return _shortValue.Value;
                if (_intValue.HasValue)
                    return _intValue.Value;
                if (_longValue.HasValue)
                    return _longValue.Value;
                if (_floatValue.HasValue)
                    return _floatValue.Value;
                if (_decimalValue.HasValue)
                    return _decimalValue.Value;
                if (_doubleValue.HasValue)
                    return _doubleValue.Value;
                if (_charValue.HasValue)
                    return _charValue.Value;
                if (_dateTimeValue.HasValue)
                    return _dateTimeValue.Value;
                if (_binaryValue != null)
                    return _binaryValue;
                if (_charsValue != null)
                    return _charsValue;
                return _stringValue;
            }
            set
            {
                if (value is byte)
                    _byteValue = (byte)value;
                else if (value is bool)
                    _boolValue = (bool)value;
                else if (value is short)
                    _shortValue = (short)value;
                else if (value is Enum)
                    _intValue = Convert.ToInt32(value);
                else if (value is int)
                    _intValue = (int)value;
                else if (value is long)
                    _longValue = (long)value;
                else if (value is float)
                    _floatValue = (float)value;
                else if (value is decimal)
                    _decimalValue = (decimal)value;
                else if (value is double)
                    _doubleValue = (double)value;
                else if (value is char)
                    _charValue = (char)value;
                else if (value is string)
                    _stringValue = (string)value;
                else if (value is DateTime)
                    _dateTimeValue = (DateTime)value;
                else if (value is byte[])
                    _binaryValue = (byte[])value;
                else if (value is char[])
                    _charsValue = (char[])value;
                else if (value == null)
                {
                    _isnullValue = true;
                }
                else
                {
                    string type = value.GetType().Name;
                    throw new NotImplementedException("Unexpected Type:" + type + " for value:" + value);
                }
            }
        }

        [ProtoMember(1)]
        private byte? _byteValue;

        [ProtoMember(2)]
        private bool? _boolValue;

        [ProtoMember(3)]
        private short? _shortValue;

        [ProtoMember(4)]
        private int? _intValue;

        [ProtoMember(5)]
        private long? _longValue;

        [ProtoMember(6)]
        private float? _floatValue;

        [ProtoMember(7)]
        private decimal? _decimalValue;

        [ProtoMember(8)]
        private double? _doubleValue;

        [ProtoMember(9)]
        private char? _charValue;

        [ProtoMember(10)]
        private string _stringValue;

        [ProtoMember(11)]
        private DateTime? _dateTimeValue;

        [ProtoMember(12)]
        private byte[] _binaryValue;

        [ProtoMember(13)]
        private char[] _charsValue;

        [ProtoMember(14)]
        private bool? _isnullValue;
        // etc


    }
}
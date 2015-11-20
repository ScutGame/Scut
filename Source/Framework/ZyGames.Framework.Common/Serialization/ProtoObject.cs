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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
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
            Serializer.Merge(info, this);
        }

        /// <summary>
        /// 重载Tostring方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var val = Value;
            return val == null ? null : val.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsValueType
        {
            get
            {
                return _isnullValue.HasValue ||
                    _byteValue.HasValue ||
                    _boolValue.HasValue ||
                    _shortValue.HasValue ||
                    _intValue.HasValue ||
                    _longValue.HasValue ||
                    _floatValue.HasValue ||
                    _decimalValue.HasValue ||
                    _doubleValue.HasValue ||
                    _charValue.HasValue ||
                    _dateTimeValue.HasValue ||
                    _ushortValue.HasValue ||
                    _uintValue.HasValue ||
                    _ulongValue.HasValue ||
                    _guidValue.HasValue;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool HasStringValue { get { return !string.IsNullOrEmpty(_stringValue); } }

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
                if (_ushortValue.HasValue)
                    return _ushortValue.Value;
                if (_uintValue.HasValue)
                    return _uintValue.Value;
                if (_ulongValue.HasValue)
                    return _ulongValue.Value;
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
                if (_binaryArrayValue != null)
                    return _binaryArrayValue;
                if (_guidValue != null)
                    return _guidValue;
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
                else if (value is ushort)
                    _ushortValue = (ushort)value;
                else if (value is uint)
                    _uintValue = (uint)value;
                else if (value is ulong)
                    _ulongValue = (ulong)value;
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
                else if (value is byte[][])
                    _binaryArrayValue = (byte[][])value;
                else if (value is Guid)
                    _guidValue = (Guid)value;
                else if (value == null)
                {
                    _isnullValue = true;
                }
                else
                {
                    string type = value.GetType().Name;
                    throw new NotImplementedException("Unexpected Type:\"" + type + "\" of value:\"" + value + "\" on ProtoObject.");
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

        [ProtoMember(15)]
        private byte[][] _binaryArrayValue;
        // etc

        [ProtoMember(16)]
        private ushort? _ushortValue;

        [ProtoMember(17)]
        private uint? _uintValue;

        [ProtoMember(18)]
        private ulong? _ulongValue;

        [ProtoMember(19)]
        private Guid? _guidValue;

        #region ISerializable Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Serializer.Serialize(info, this);
        }

        #endregion
    }
}
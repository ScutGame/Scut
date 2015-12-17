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
using System.Text;
using System.Text.RegularExpressions;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Common.Serialization;

namespace ZyGames.Framework.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// 
        /// </summary>
        public const int ZeroNum = 0;
        /// <summary>
        /// Sql最小时间范围
        /// </summary>
        public static DateTime SqlMinDate = new DateTime(1753, 1, 1);

        private const long UnixEpoch = 621355968000000000L;
        /// <summary>
        /// Unix timestamp, val:1970-01-01 00:00:00 UTC
        /// </summary>
        public static readonly DateTime UnixEpochDateTime = new DateTime(UnixEpoch);

        /// <summary>
        /// 
        /// </summary>
        public static DateTime Now
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Use Utc time
        /// </summary>
        public static TimeSpan UnixEpochTimeSpan
        {
            get { return (DateTime.UtcNow - UnixEpochDateTime); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static TimeSpan GetUnixEpochTimeSpan(DateTime date)
        {
            return date - UnixEpochDateTime; 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static DateTime ToTimeFromUnixEpoch(TimeSpan ts)
        {
            return UnixEpochDateTime.Add(ts);
        }

        /// <summary>
        /// 当天时间一年中是第几周
        /// </summary>
        public static int WeekOfYear
        {
            get { return ToWeekOfYear(Now); }
        }

        /// <summary>
        /// 指定时间一年中是第几周
        /// </summary>
        /// <returns></returns>
        public static int ToWeekOfYear(DateTime date)
        {
            int dayOfYear = date.DayOfYear;
            DateTime tempDate = new DateTime(date.Year, 1, 1);
            int tempDayOfWeek = (int)tempDate.DayOfWeek;
            tempDayOfWeek = tempDayOfWeek == 0 ? 7 : tempDayOfWeek;
            int index = (int)date.DayOfWeek;
            index = index == 0 ? 7 : index;
            DateTime retStartDay = date.AddDays(-(index - 1));
            DateTime retEndDay = date.AddDays(6 - index);
            int weekIndex = (int)Math.Ceiling(((double)dayOfYear + tempDayOfWeek - 1) / 7);
            if (retStartDay.Year < retEndDay.Year)
            {
                weekIndex = 1;
            }
            return weekIndex;
        }
        /// <summary>
        /// 获取与当前时间差异
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static TimeSpan DiffDate(DateTime date)
        {
            return DiffDate(Now, date);
        }
        /// <summary>
        /// 获取两个时间之间的差异
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static TimeSpan DiffDate(DateTime date1, DateTime date2)
        {
            return date1 - date2;
        }

        /// <summary>
        /// char转成两个字节
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static byte[] CharToByte(char c)
        {
            byte[] b = new byte[2];
            b[0] = (byte)((c & 0xFF00) >> 8);
            b[1] = (byte)(c & 0xFF);
            return b[0] == 0 ? new byte[] { b[1] } : b;
        }

        /// <summary>
        /// 使用两个字节转成char
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static char ByteToChar(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentOutOfRangeException("bytes");
            }
            if (startIndex > bytes.Length - 1)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            return bytes.Length > 1
                ? (char)(((bytes[startIndex] & 0xFF) << 8) | (bytes[startIndex + 1] & 0xFF))
                : (char)(bytes[startIndex] & 0xFF);
        }
        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static byte[] Join(params byte[][] args)
        {
            Int32 length = 0;
            foreach (byte[] tempbyte in args)
            {
                length += tempbyte.Length;
            }
            Byte[] bytes = new Byte[length];
            Int32 tempLength = 0;
            foreach (byte[] tempByte in args)
            {
                tempByte.CopyTo(bytes, tempLength);
                tempLength += tempByte.Length;
            }
            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static int IndexOf(byte[] bytes, byte[] pattern)
        {
            return IndexOf(bytes, 0, bytes.Length, pattern);
        }

        /// <summary>
        /// 查找数组中包含另一数组的启始索引
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        /// <param name="pattern"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int IndexOf(byte[] bytes, int offset, int length, byte[] pattern)
        {
            int index = -1;
            int pos = offset;
            if (length > bytes.Length)
            {
                length = bytes.Length;
            }
            while (pos < length)
            {
                if (bytes[pos] == pattern[0])
                {
                    index = pos;
                    for (int i = 1; i < pattern.Length; i++)
                    {
                        if (pos + i >= length || pattern[i] != bytes[pos + i])
                        {
                            index = -1;
                            break;
                        }
                    }
                    if (index > -1)
                    {
                        break;
                    }
                }
                pos++;
            }
            return index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool IsEquals(string a, string b, bool ignoreCase)
        {
            return ignoreCase ? string.Equals(a.ToLower(), b.ToLower(), StringComparison.Ordinal) : string.Equals(a, b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool StartsWith(string a, string b, bool ignoreCase)
        {
            if (a == null) throw new ArgumentNullException("a");
            return ignoreCase ? a.ToLower().StartsWith(b.ToLower(), StringComparison.Ordinal) : a.StartsWith(b);
        }

        /// <summary>
        /// Convert to default value by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : "";
        }

        private static string _runtimeBinPath;
        /// <summary>
        /// Get runtime bin path.
        /// </summary>
        public static string RuntimeBinPath
        {
            get { return _runtimeBinPath ?? (_runtimeBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath ?? RuntimePath); }
            set { _runtimeBinPath = value; }
        }

        private static string _runtimePath;
        /// <summary>
        /// Get runtime path.
        /// </summary>
        public static string RuntimePath
        {
            get
            {
                return _runtimePath ?? (_runtimePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHexMd5Hash(string str)
        {
            return CryptoHelper.ToMd5Hash(str);
        }

        /// <summary>
        /// 转换成16进制编码字串
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string ToHex(byte[] bytes)
        {
            return ToHex(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 转换成16进制编码字串
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string ToHex(byte[] bytes, int offset, int count)
        {
            char[] c = new char[count * 2];
            byte b;

            for (int bx = 0, cx = 0; bx < count; ++bx, ++cx)
            {
                b = ((byte)(bytes[offset + bx] >> 4));
                c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((byte)(bytes[offset + bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }

            return new string(c);
        }

        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimals">保留的小数位</param>
        /// <returns></returns>
        public static decimal RoundCustom(decimal value, int decimals)
        {
            return (decimal)RoundCustom((double)value, decimals);
        }

        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimals">保留的小数位</param>
        /// <returns></returns>
        public static double RoundCustom(double value, int decimals)
        {
            if (value < 0)
            {
                return Math.Round(value + 5 / Math.Pow(10, decimals + 1), decimals, MidpointRounding.AwayFromZero);
            }
            return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int RoundCustom(decimal value)
        {
            return (int)RoundCustom(value, 0);
        }
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int RoundCustom(double value)
        {
            return (int)RoundCustom(value, 0);
        }

        /// <summary>
        /// 指定的值是否匹配正则
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMach(string pattern, string value)
        {
            if (pattern != null && value != null)
            {
                return new Regex(pattern).IsMatch(value);
            }
            return false;
        }

        /// <summary>
        /// 指定的值是否匹配变量命名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMachVarName(string value)
        {
            return IsMach("[A-Za-z_][A-Za-z0-9_]*", value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        public static bool TryAdd(uint a, uint b, Action<uint> success)
        {
            uint increment;
            return TryAdd(a, b, success, out  increment);
        }

        /// <summary>
        /// 尝试相加,防止溢出
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="success"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static bool TryAdd(uint a, uint b, Action<uint> success, out uint increment)
        {
            increment = 0;
            uint r = a + b;
            if (r >= a)
            {
                increment = b;
                success(r);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        public static bool TryAdd(ushort a, ushort b, Action<ushort> success)
        {
            ushort increment;
            return TryAdd(a, b, success, out  increment);
        }

        /// <summary>
        /// 尝试相加,防止溢出
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="success"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static bool TryAdd(ushort a, ushort b, Action<ushort> success, out ushort increment)
        {
            increment = 0;
            if (a + b <= ushort.MaxValue)
            {
                var r = (ushort)(a + b);
                increment = b;
                success(r);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        public static bool TrySub(uint a, uint b, Action<uint> success)
        {
            uint decrement;
            return TrySub(a, b, success, out  decrement);
        }

        /// <summary>
        /// 尝试相减,防止溢出
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="success"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        public static bool TrySub(uint a, uint b, Action<uint> success, out uint decrement)
        {
            decrement = 0;
            if (a < b) return false;

            uint r = a - b;
            if (r <= a)
            {
                decrement = b;
                success(r);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        public static bool TrySub(ushort a, ushort b, Action<uint> success)
        {
            ushort decrement;
            return TrySub(a, b, success, out decrement);
        }

        /// <summary>
        /// 尝试相减,防止溢出
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="success"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        public static bool TrySub(ushort a, ushort b, Action<uint> success, out ushort decrement)
        {
            decrement = 0;
            if (a < b) return false;

            int r = a - b;
            if (r >= 0 && r <= a)
            {
                decrement = b;
                success((ushort)r);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <returns></returns>
        public static long Addition(long value, long addValue)
        {
            return Addition(value, addValue, long.MaxValue);
        }

        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue">增加的值</param>
        /// <param name="maxValue">最大值,超出取最大值</param>
        /// <returns></returns>
        public static long Addition(long value, long addValue, long maxValue)
        {
            long t = value + addValue;
            return t < -1 || t > maxValue ? maxValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <returns></returns>
        public static int Addition(int value, int addValue)
        {
            return Addition(value, addValue, int.MaxValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int Addition(int value, int addValue, int maxValue)
        {
            int t = value + addValue;
            return t < -1 || t > maxValue ? maxValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <returns></returns>
        public static short Addition(short value, short addValue)
        {
            return Addition(value, addValue, short.MaxValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static short Addition(short value, short addValue, short maxValue)
        {
            short t = (short)(value + addValue);
            return t < -1 || t > maxValue ? maxValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <returns></returns>
        public static byte Addition(byte value, byte addValue)
        {
            return Addition(value, addValue, byte.MaxValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static byte Addition(byte value, byte addValue, byte maxValue)
        {
            byte t = (byte)(value + addValue);
            return t > maxValue ? maxValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <returns></returns>
        public static float Addition(float value, float addValue)
        {
            return Addition(value, addValue, float.MaxValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static float Addition(float value, float addValue, float maxValue)
        {
            float t = value + addValue;
            return t < -1 || t > maxValue ? maxValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <returns></returns>
        public static double Addition(double value, double addValue)
        {
            return Addition(value, addValue, double.MaxValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static double Addition(double value, double addValue, double maxValue)
        {
            double t = value + addValue;
            return t < -1 || t > maxValue ? maxValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <returns></returns>
        public static decimal Addition(decimal value, decimal addValue)
        {
            return Addition(value, addValue, decimal.MaxValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static decimal Addition(decimal value, decimal addValue, decimal maxValue)
        {
            decimal t = value + addValue;
            return t < -1 || t > maxValue ? maxValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <returns></returns>
        public static long Subtraction(long value, long subValue)
        {
            return Subtraction(value, subValue, 0);
        }

        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue">减少的值</param>
        /// <param name="minValue">最小值,超出取最小值</param>
        /// <returns></returns>
        public static long Subtraction(long value, long subValue, long minValue)
        {
            long t = value - subValue;
            return t < minValue ? minValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <returns></returns>
        public static int Subtraction(int value, int subValue)
        {
            return Subtraction(value, subValue, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static int Subtraction(int value, int subValue, int minValue)
        {
            int t = value - subValue;
            return t < minValue ? minValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <returns></returns>
        public static short Subtraction(short value, short subValue)
        {
            return Subtraction(value, subValue, (short)0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static short Subtraction(short value, short subValue, short minValue)
        {
            short t = (short)(value - subValue);
            return t < minValue ? minValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <returns></returns>
        public static byte Subtraction(byte value, byte subValue)
        {
            return Subtraction(value, subValue, (byte)0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static byte Subtraction(byte value, byte subValue, byte minValue)
        {
            byte t = (byte)(value - subValue);
            return t < minValue ? minValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <returns></returns>
        public static float Subtraction(float value, float subValue)
        {
            return Subtraction(value, subValue, (float)0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static float Subtraction(float value, float subValue, float minValue)
        {
            float t = value - subValue;
            return t < minValue ? minValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <returns></returns>
        public static double Subtraction(double value, double subValue)
        {
            return Subtraction(value, subValue, (double)0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static double Subtraction(double value, double subValue, double minValue)
        {
            double t = value - subValue;
            return t < minValue ? minValue : t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <returns></returns>
        public static decimal Subtraction(decimal value, decimal subValue)
        {
            return Subtraction(value, subValue, (decimal)0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="subValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static decimal Subtraction(decimal value, decimal subValue, decimal minValue)
        {
            decimal t = value - subValue;
            return t < minValue ? minValue : t;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static List<T> GetPaging<T>(List<T> list, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            List<T> result = new List<T>();
            int recordNum = 0;
            int pageNum = 0;

            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 20 : pageSize;
            int fromIndex = (pageIndex - 1) * pageSize;
            int toIndex = pageIndex * pageSize;
            recordNum = list.Count;
            pageNum = (recordNum + pageSize - 1) / pageSize;

            if (recordNum > 0)
            {
                int size = 0;
                if (fromIndex < toIndex && toIndex <= recordNum)
                {
                    size = toIndex - fromIndex;
                }
                else if (fromIndex < recordNum && toIndex > recordNum)
                {
                    size = recordNum - fromIndex;
                }

                if (size > 0 && fromIndex < list.Count && (fromIndex + size) <= list.Count)
                {
                    result = new List<T>(list.GetRange(fromIndex, size));
                }
            }
            recordCount = recordNum;
            pageCount = pageNum > 0 ? pageNum : 1;
            return result;
        }

        /// <summary>
        /// 同string.IsNullOrEmpty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 判断对象是否为空或DBNull或new object()对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrDbNull(object value)
        {
            return value == null || string.IsNullOrEmpty(Convert.ToString(value)) || value.GetType() == typeof(object);
        }

        /// <summary>
        /// 插入排序,List是有序的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="comparison"></param>
        public static void InsertSort<T>(List<T> list, T item, Comparison<T> comparison)
        {
            if (list.Count == 0)
            {
                list.Add(item);
                return;
            }
            int index = 0;
            int startIndex = 0;
            int endIndex = list.Count - 1;

            while (endIndex >= startIndex)
            {
                int middle = (startIndex + endIndex) / 2;
                int nextMiddle = middle + 1;
                var value = list[middle];

                int result = comparison(value, item);
                if (result <= 0 &&
                    (nextMiddle >= list.Count || comparison(list[nextMiddle], item) > 0))
                {
                    startIndex = middle + 1;
                    index = nextMiddle;
                }
                else if (result > 0)
                {
                    endIndex = middle - 1;
                }
                else
                {
                    startIndex = middle + 1;
                }
            }
            list.Insert(index, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparison"></param>
        public static List<T> QuickSort<T>(List<T> list, Comparison<T> comparison)
        {
            DoQuickSort(list, 0, list.Count - 1, comparison);
            return list;
        }

        private static void DoQuickSort<T>(List<T> list, int low, int high, Comparison<T> compareTo)
        {
            T pivot;//存储分支点    
            int l, r;
            int mid;
            if (high < 0 || high <= low)
            {
                return;
            }
            if (high == low + 1)
            {
                if (compareTo(list[low], list[high]) > 0)
                {
                    QuickSwap(list, low, high);
                }
                return;
            }
            mid = (low + high) >> 1;
            pivot = list[mid];
            QuickSwap(list, low, mid);
            l = low + 1;
            r = high;
            do
            {
                while (l <= r && compareTo(list[l], pivot) <= 0)
                {
                    l++;
                }
                while (r > 0 && compareTo(list[r], pivot) > 0)
                {
                    r--;
                }
                if (l < r)
                {
                    QuickSwap(list, l, r);
                }
            } while (l < r);

            list[low] = list[r];
            list[r] = pivot;
            if (low + 1 < r)
            {
                DoQuickSort(list, low, r - 1, compareTo);
            }
            if (r + 1 < high)
            {
                DoQuickSort(list, r + 1, high, compareTo);
            }
        }

        private static void QuickSwap<T>(List<T> list, int low, int high)
        {
            T temp = list[low];
            list[low] = list[high];
            list[high] = temp;
        }


        #region 转换值

        /// <summary>
        /// 将对象转换成字符串，Null为string.Empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToNotNullString(object value)
        {
            return ToNotNullString(value, string.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ToNotNullString(object value, string defValue)
        {
            defValue = defValue.IsNullOrDbNull() ? string.Empty : defValue;
            return value.IsNullOrDbNull() || value.ToString().IsEmpty() ? defValue : value.ToString();
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToCeilingInt(object value)
        {
            if (value is decimal)
            {
                return (int)Math.Ceiling((decimal)value);
            }
            else if (value is double)
            {
                return (int)Math.Ceiling((double)value);
            }
            else if (value is float)
            {
                return (int)Math.Ceiling((float)value);
            }
            else
            {
                return (int)Math.Ceiling(ToDouble(value));
            }
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToFloorInt(object value)
        {
            if (value is decimal)
            {
                return (int)Math.Floor((decimal)value);
            }
            else if (value is double)
            {
                return (int)Math.Floor((double)value);
            }
            else if (value is float)
            {
                return (int)Math.Floor((float)value);
            }
            else
            {
                return (int)Math.Floor(ToDouble(value));
            }
        }

        /// <summary>
        /// 将对象转换成长整型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(object value)
        {
            try
            {
                return Convert.ToInt64(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type long fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成整型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(object value)
        {
            try
            {
                if (value.IsNullOrDbNull() || false.Equals(value))
                {
                    return 0;
                }
                if (true.Equals(value))
                {
                    return 1;
                }
                return Convert.ToInt32(value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type int fail.", value));
            }
        }

        /// <summary>
        /// 将对象转换成短整型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToShort(object value)
        {
            try
            {
                return Convert.ToInt16(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type short fail.", value));
            }
        }

        /// <summary>
        /// 将对象转换成双精度值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(object value)
        {
            try
            {
                return Convert.ToDouble(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type double fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成单精度值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(object value)
        {
            try
            {
                return Convert.ToDecimal(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type decimal fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成单精度浮点值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(object value)
        {
            try
            {
                return Convert.ToSingle(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type decimal fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成布尔值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(object value)
        {
            try
            {
                if (value.IsNullOrDbNull() || "0".Equals(value))
                {
                    return false;
                }
                if ("1".Equals(value))
                {
                    return true;
                }
                return Convert.ToBoolean(value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type bool fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成字节值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(object value)
        {
            try
            {
                return Convert.ToByte(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type byte fail.", value));
            }
        }

        /// <summary>
        /// 将对象转换成64位无符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(object value)
        {
            try
            {
                return Convert.ToUInt64(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type UInt64 fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成32位无符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(object value)
        {
            try
            {
                return Convert.ToUInt32(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type ToUInt32 fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成16位无符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(object value)
        {
            try
            {
                return Convert.ToUInt16(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type ToUInt16 fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成时间值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value)
        {
            return ToDateTime(value, MathUtils.SqlMinDate);
        }

        /// <summary>
        /// 将对象转换成时间值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value, DateTime defValue)
        {
            try
            {
                if (value.IsNullOrDbNull()) return defValue;
                return Convert.ToDateTime(value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type datetime fail.", value));
            }
        }

        /// <summary>
        /// 将对象转换成枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(object value)
        {
            return (T)ToEnum(value, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToEnum(object value, Type type)
        {
            try
            {
                if (value is string)
                {
                    string tempValue = value.ToNotNullString();
                    return tempValue.IsEmpty() ? 0 : Enum.Parse(type, tempValue, true);
                }
                return Enum.ToObject(type, (value == null || value == DBNull.Value) ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to Enum {1} fail.", value, type.Name));
            }
        }
        /// <summary>
        /// 解析Json成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T ParseJson<T>(string jsonStr)
        {
            return JsonUtils.DeserializeCustom<T>(jsonStr);
        }

        /// <summary>
        /// 将对象序列化成string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson(object value)
        {
            return JsonUtils.SerializeCustom(value);
        }

        #endregion
    }
}
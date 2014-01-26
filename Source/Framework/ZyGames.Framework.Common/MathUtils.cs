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

        /// <summary>
        /// 
        /// </summary>
        public static DateTime Now
        {
            get { return DateTime.Now; }
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
        /// Convert to default value by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : "";
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
                throw new ArgumentException(string.Format("值\"{0}\"转换成long异常!", value));
            }
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
                throw new ArgumentException(string.Format("值\"{0}\"转换成int异常!", value));
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
                throw new ArgumentException(string.Format("值\"{0}\"转换成short异常!", value));
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
                throw new ArgumentException(string.Format("值\"{0}\"转换成double异常!", value));
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
                throw new ArgumentException(string.Format("值\"{0}\"转换成decimal异常!", value));
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
                throw new ArgumentException(string.Format("值\"{0}\"转换成bool异常!", value));
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
                throw new ArgumentException(string.Format("值\"{0}\"转换成Byte异常!", value));
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
                throw new ArgumentException(string.Format("值\"{0}\"转换成DateTime异常!", value));
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
            try
            {
                string tempValue = value.ToNotNullString();
                return tempValue.IsEmpty() ? default(T) : (T)Enum.Parse(typeof(T), tempValue, false);
            }
            catch
            {
                throw new ArgumentException(string.Format("值\"{0}\"转换成Enum异常!", value));
            }
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
                string tempValue = value.ToNotNullString();
                return tempValue.IsEmpty() ? 0 : Enum.Parse(type, tempValue, false);
            }
            catch
            {
                throw new ArgumentException(string.Format("值\"{0}\"转换成Enum异常!", value));
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
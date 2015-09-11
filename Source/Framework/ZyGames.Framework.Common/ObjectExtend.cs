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
using ZyGames.Framework.Common.Reflect;
using ZyGames.Framework.Common.Serialization;

namespace ZyGames.Framework.Common
{
    /// <summary>
    /// 对象扩展方法
    /// </summary>
    public static class ObjectExtend
    {
        /// <summary>
        /// 快速创建实例
        /// </summary>
        /// <typeparam name="T">Object类型</typeparam>
        /// <param name="type">指定实例的类型</param>
        /// <param name="args">创建实体的构造参数</param>
        /// <returns>返回创建实例</returns>
        public static T CreateInstance<T>(this Type type, params object[] args)
        {
            return (T)FastActivator.Create(type, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateInstance(this Type type, params object[] args)
        {
            return FastActivator.Create(type, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool IsEquals(this string a, string b, bool ignoreCase)
        {
            return MathUtils.IsEquals(a, b, ignoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool StartsWith(this string a, string b, bool ignoreCase)
        {
            return MathUtils.StartsWith(a, b, ignoreCase);
        }
        /// <summary>
        /// 同string.IsNullOrEmpty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string value)
        {
            return MathUtils.IsEmpty(value);
        }

        /// <summary>
        /// 判断对象是否为空或DBNull或new object()对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrDbNull(this object value)
        {
            return MathUtils.IsNullOrDbNull(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static List<T> GetPaging<T>(this List<T> list, int pageIndex, int pageSize, out int pageCount)
        {
            int recordCount;
            return GetPaging(list, pageIndex, pageSize, out pageCount, out recordCount);
        }

        /// <summary>
        /// 获取数据分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static List<T> GetPaging<T>(this List<T> list, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            return MathUtils.GetPaging<T>(list, pageIndex, pageSize, out pageCount, out recordCount);
        }

        /// <summary>
        /// 插入排序,List是有序的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="comparison"></param>
        public static void InsertSort<T>(this List<T> list, T item, Comparison<T> comparison)
        {
            MathUtils.InsertSort<T>(list, item, comparison);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static List<T> QuickSort<T>(this List<T> list) where T : IComparable<T>
        {
            return MathUtils.QuickSort(list, (x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x == null && y != null) return -1;
                if (x != null && y == null) return 1;

                return x.CompareTo(y);
            });
        }
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparison"></param>
        public static void QuickSort<T>(this List<T> list, Comparison<T> comparison)
        {
            MathUtils.QuickSort(list, comparison);
        }

        /// <summary>
        /// 解析Json成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T ParseJson<T>(this string jsonStr)
        {
            return JsonUtils.DeserializeCustom<T>(jsonStr);
        }

        /// <summary>
        /// 将对象序列化成string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToJson<T>(this List<T> list)
        {
            return JsonUtils.SerializeCustom(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static int IndexOf(this byte[] bytes, byte[] pattern)
        {
            return MathUtils.IndexOf(bytes, pattern);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static int IndexOf(this byte[] bytes, int offset, int length, byte[] pattern)
        {
            return MathUtils.IndexOf(bytes, offset, length, pattern);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] RandomSort<T>(this T[] array)
        {
            return RandomUtils.RandomSort(array);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> RandomSort<T>(this List<T> list)
        {
            return RandomUtils.RandomSort(list);
        }


        #region 转换值

        /// <summary>
        /// 将对象转换成字符串，Null为string.Empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToNotNullString(this object value)
        {
            return MathUtils.ToNotNullString(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ToNotNullString(this object value, string defValue)
        {
            return MathUtils.ToNotNullString(value, defValue);
        }
        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToCeilingInt(this object value)
        {
            return MathUtils.ToCeilingInt(value);
        }
        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToFloorInt(this object value)
        {
            return MathUtils.ToFloorInt(value);
        }

        /// <summary>
        /// 将对象转换成整型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this object value)
        {
            return MathUtils.ToLong(value);
        }

        /// <summary>
        /// 将对象转换成整型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this object value)
        {
            return MathUtils.ToInt(value);
        }

        /// <summary>
        /// 将对象转换成短整型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToShort(this object value)
        {
            return MathUtils.ToShort(value);
        }

        /// <summary>
        /// 将对象转换成双精度值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this object value)
        {
            return MathUtils.ToDouble(value);
        }
        /// <summary>
        /// 将对象转换成单精度值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object value)
        {
            return MathUtils.ToDecimal(value);
        }

        /// <summary>
        /// 将对象转换成单精度浮点值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this object value)
        {
            return MathUtils.ToFloat(value);
        }

        /// <summary>
        /// 将对象转换成布尔值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this object value)
        {
            return MathUtils.ToBool(value);
        }
        /// <summary>
        /// 将对象转换成字节值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this object value)
        {
            return MathUtils.ToByte(value);
        }
        /// <summary>
        /// 将对象转换成64位无符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this object value)
        {
            return MathUtils.ToUInt64(value);
        }
        /// <summary>
        /// 将对象转换成32位无符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this object value)
        {
            return MathUtils.ToUInt32(value);
        }
        /// <summary>
        /// 将对象转换成16位无符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(this object value)
        {
            return MathUtils.ToUInt16(value);
        }
        /// <summary>
        /// 将对象转换成时间值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object value)
        {
            return ToDateTime(value, MathUtils.SqlMinDate);
        }

        /// <summary>
        /// 将对象转换成时间值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object value, DateTime defValue)
        {
            return MathUtils.ToDateTime(value, defValue);
        }

        /// <summary>
        /// 将对象转换成枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this object value)
        {
            return MathUtils.ToEnum<T>(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToEnum(this object value, Type type)
        {
            return MathUtils.ToEnum(value, type);
        }

        #endregion

        #region 检查值范围
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(this long value)
        {
            return IsValid(value, MathUtils.ZeroNum, long.MaxValue);
        }

        /// <summary>
        /// 检查值范围是否有效
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static bool IsValid(this long value, long minValue, long maxValue)
        {
            return value >= minValue && value <= maxValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(this int value)
        {
            return IsValid(value, MathUtils.ZeroNum, int.MaxValue);
        }
        /// <summary>
        /// 检查值范围是否有效
        /// </summary>
        public static bool IsValid(this int value, int minValue, int maxValue)
        {
            return value >= minValue && value <= maxValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(this short value)
        {
            return IsValid(value, (short)MathUtils.ZeroNum, short.MaxValue);
        }
        /// <summary>
        /// 检查值范围是否有效
        /// </summary>
        public static bool IsValid(this short value, short minValue, short maxValue)
        {
            return value >= minValue && value <= maxValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(this byte value)
        {
            return IsValid(value, (byte)MathUtils.ZeroNum, byte.MaxValue);
        }
        /// <summary>
        /// 检查值范围是否有效
        /// </summary>
        public static bool IsValid(this byte value, byte minValue, byte maxValue)
        {
            return value >= minValue && value <= maxValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(this double value)
        {
            return IsValid(value, MathUtils.ZeroNum, double.MaxValue);
        }
        /// <summary>
        /// 检查值范围是否有效
        /// </summary>
        public static bool IsValid(this double value, double minValue, double maxValue)
        {
            return value >= minValue && value <= maxValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(this decimal value)
        {
            return IsValid(value, MathUtils.ZeroNum, decimal.MaxValue);
        }
        /// <summary>
        /// 检查值范围是否有效
        /// </summary>
        public static bool IsValid(this decimal value, decimal minValue, decimal maxValue)
        {
            return value >= minValue && value <= maxValue;
        }

        #endregion


    }
}
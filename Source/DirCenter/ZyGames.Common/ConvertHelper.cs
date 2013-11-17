using System;

namespace ZyGames.Common
{
    /// <summary>
    /// 类型转换辅助类
    /// </summary>
    public static class ConvertHelper
    {
        public static DateTime SqlMinDate = new DateTime(1753, 1, 1);

        public static string ToString(object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        public static DateTime ToDateTime(object obj)
        {
            return ToDateTime(obj, SqlMinDate);
        }
        /// <summary>
        /// 转换时间
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue">默认时间</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj, DateTime defValue)
        {
            string val = ToString(obj);
            if (!string.IsNullOrEmpty(val))
            {
                return DateTime.Parse(val);
            }
            return defValue;
        }

        public static int ToInt(object obj)
        {
            string str = ToString(obj);
            return string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
        }
        public static decimal ToDecimal(object obj)
        {
            string str = ToString(obj);
            return string.IsNullOrEmpty(str) ? 0 : Convert.ToDecimal(str);
        }

        public static double ToDouble(object obj)
        {
            string str = ToString(obj);
            return string.IsNullOrEmpty(str) ? 0 : Convert.ToDouble(str);
        }

        public static bool ToBool(object obj)
        {
            string str = ToString(obj);
            return string.IsNullOrEmpty(str) ? false : Convert.ToBoolean(str);
        }

        public static int[] ToInt(object obj, char splitChar)
        {
            string[] list = ToString(obj).Split(new char[] { splitChar });
            int[] values = new int[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                values[i] = ToInt(list[i]);
            }
            return values;
        }

        public static T ToEnum<T>(object obj)
        {
            string str = ToString(obj);
            return (T)Enum.Parse(typeof(T), str, false);
        }
        /// <summary>
        /// 连接整数
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static int ConcatInt(int val1, int val2)
        {
            return val1 * (int)Math.Pow(10, val2.ToString().Length) + val2;
        }
    }
}

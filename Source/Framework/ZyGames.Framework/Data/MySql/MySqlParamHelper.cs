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
using System.Data;
using ZyGames.Framework.Common.Log;
using MySql.Data.MySqlClient;

namespace ZyGames.Framework.Data.MySql
{
    ///<summary>
    /// MySQL数据库参数辅助类
    ///</summary>
    internal class MySqlParamHelper
    {
        /// <summary>
        /// 参数前缀字符
        /// </summary>
        public const string PreParamChar = "?";

        ///<summary>
        ///</summary>
        ///<param name="paramName"></param>
        ///<param name="dbType"></param>
        ///<param name="size"></param>
        ///<param name="direction"></param>
        ///<param name="value"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentOutOfRangeException"></exception>
        public static MySqlParameter MakeParam(string paramName, MySqlDbType dbType, int size, ParameterDirection direction, object value)
        {
            MySqlParameter sqlParameter = null;
            try
            {
                paramName = paramName ?? string.Empty;

                //modify reason:验证花时比较大
                //if (!MathUtils.IsMachVarName(paramName))
                //{
                //    throw new ArgumentOutOfRangeException("paramName", "参数名格式不正确");
                //}
                if (size > 0)
                {
                    sqlParameter = new MySqlParameter(FormatParamName(paramName), dbType, size);
                }
                else
                {
                    sqlParameter = new MySqlParameter(FormatParamName(paramName), dbType);
                }
                sqlParameter.Direction = direction;
                if (direction != ParameterDirection.Output || value != null)
                {
                    sqlParameter.Value = value;
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("{0}", ex);
            }
            return sqlParameter;
        }
        ///<summary>
        ///</summary>
        ///<param name="paramName"></param>
        ///<param name="dbType"></param>
        ///<param name="size"></param>
        ///<returns></returns>
        public static MySqlParameter MakeOutParam(string paramName, MySqlDbType dbType, int size)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Output, null);
        }
        ///<summary>
        ///</summary>
        ///<param name="paramName"></param>
        ///<param name="dbType"></param>
        ///<param name="size"></param>
        ///<param name="value"></param>
        ///<returns></returns>
        public static MySqlParameter MakeInParam(string paramName, MySqlDbType dbType, int size, object value)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Input, value);
        }

        static MySqlDbType GetDbType(object value)
        {
            if (value is DateTime)
            {
                return MySqlDbType.DateTime;
            }
            else if (value is bool)
            {
                return MySqlDbType.Bit;
            }
            else if (value is Byte[])
            {
                return MySqlDbType.LongBlob;
            }
            else if (value is long || value is ulong)
            {
                return MySqlDbType.Int64;
            }
            else if (value is decimal)
            {
                return MySqlDbType.Decimal;
            }
            else if (value is double)
            {
                return MySqlDbType.Double;
            }
            else if (value is float)
            {
                return MySqlDbType.Float;
            }
            else if (value is int || value is uint)
            {
                return MySqlDbType.Int32;
            }
            else if (value is short || value is ushort)
            {
                return MySqlDbType.Int16;
            }
            else if (value is byte)
            {
                return MySqlDbType.Byte;
            }
            else if (value is Enum)
            {
                return MySqlDbType.Int32;
            }
            else if (value is Guid)
            {
                return MySqlDbType.Guid;
            }
            return MySqlDbType.VarChar;
        }

        ///<summary>
        ///</summary>
        ///<param name="paramName"></param>
        ///<param name="value"></param>
        ///<returns></returns>
        public static MySqlParameter MakeInParam(string paramName, object value)
        {
            MySqlDbType dbType = GetDbType(value);
            return MakeParam(paramName, dbType, 0, ParameterDirection.Input, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string FormatParamName(string paramName)
        {
            if (paramName.StartsWith("@"))
            {
                paramName = paramName.Substring(1);
            }
            if (paramName.StartsWith(PreParamChar))
            {
                return paramName;
            }
            return PreParamChar + paramName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="compareChar"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string FormatFilterParam(string fieldName, string compareChar = "", string paramName = "")
        {
            return string.Format("{0} {2} {1}",
                FormatName(fieldName),
                FormatParamName(string.IsNullOrEmpty(paramName) ? fieldName : paramName),
                string.IsNullOrEmpty(compareChar) ? "=" : compareChar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="splitChat"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static string FormatQueryColumn(string splitChat, ICollection<string> columns)
        {
            string str = "";
            foreach (var column in columns)
            {
                if (str.Length > 0)
                {
                    str += splitChat;
                }
                string temp = column.Trim();
                if (string.IsNullOrEmpty(temp)) continue;
                str += FormatName(temp);
            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FormatName(string name)
        {
            if (name.StartsWith("`") || name.Contains("("))
            {
                return name;
            }
            return string.Format("`{0}`", name);
        }
    }
}
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
using System.Data;
using System.Data.SqlClient;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using MySql.Data.MySqlClient;

namespace ZyGames.Framework.Data.MySql
{
    ///<summary>
    /// MySQL数据库参数辅助类
    ///</summary>
    public class MySqlParamHelper
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

                if (!MathUtils.IsMachVarName(paramName))
                {
                    throw new ArgumentOutOfRangeException("paramName", "参数名格式不正确");
                }
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
        ///<summary>
        ///</summary>
        ///<param name="paramName"></param>
        ///<param name="value"></param>
        ///<returns></returns>
        public static MySqlParameter MakeInParam(string paramName, object value)
        {
            return MakeParam(paramName, MySqlDbType.VarChar, 0, ParameterDirection.Input, value);
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
    }
}
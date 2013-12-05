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

namespace ZyGames.Framework.Data.Sql
{
    ///<summary>
    /// MSSQL数据库参数辅助类
    ///</summary>
    public class SqlParamHelper
    {

        /// <summary>
        /// 参数前缀字符
        /// </summary>
        public const string PreParamChar = "@";

        ///<summary>
        ///</summary>
        ///<param name="paramName"></param>
        ///<param name="dbType"></param>
        ///<param name="size"></param>
        ///<param name="direction"></param>
        ///<param name="value"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentOutOfRangeException"></exception>
        public static SqlParameter MakeParam(string paramName, SqlDbType dbType, int size, ParameterDirection direction, object value)
        {
            SqlParameter sqlParameter = null;
            try
            {
                paramName = paramName ?? string.Empty;

                if (!MathUtils.IsMachVarName(paramName))
                {
                    throw new ArgumentOutOfRangeException("paramName", "参数名格式不正确");
                }
                if (size > 0)
                {
                    sqlParameter = new SqlParameter(FormatParamName(paramName), dbType, size);
                }
                else
                {
                    sqlParameter = new SqlParameter(FormatParamName(paramName), dbType);
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
        public static SqlParameter MakeOutParam(string paramName, SqlDbType dbType, int size)
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
        public static SqlParameter MakeInParam(string paramName, SqlDbType dbType, int size, object value)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Input, value);
        }
        ///<summary>
        ///</summary>
        ///<param name="paramName"></param>
        ///<param name="value"></param>
        ///<returns></returns>
        public static SqlParameter MakeInParam(string paramName, object value)
        {
            return MakeParam(paramName, SqlDbType.VarChar, 0, ParameterDirection.Input, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string FormatParamName(string paramName)
        {
            if (paramName.StartsWith(PreParamChar))
            {
                return paramName;
            }
            return PreParamChar + paramName;
        }
    }
}
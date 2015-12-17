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

namespace ZyGames.Framework.Data.MySql
{
    /// <summary>
    /// 
    /// </summary>
    public class MySqlCommandFilter : CommandFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public MySqlCommandFilter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        public override void AddParam(string paramName, object value)
        {
            AddParam(MySqlParamHelper.MakeInParam(paramName, value));
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldName"></param>
		/// <param name="compareChar"></param>
		/// <param name="paramName"></param>
		/// <returns>The expression.</returns>
        public override string FormatExpression(string fieldName, string compareChar = "", string paramName = "")
        {
            return MySqlParamHelper.FormatFilterParam(fieldName, compareChar, paramName);
        }

        public override string FormatExpressionByIn(string fieldName, params object[] values)
        {
            if (values.Length == 0)
            {
                throw new ArgumentException("values len:0");
            }

            var paramNames = new string[values.Length];
            for (int i = 0; i < paramNames.Length; i++)
            {
                var paramName = MySqlParamHelper.FormatParamName(fieldName + (i + 1));
                paramNames[i] = paramName;
                AddParam(MySqlParamHelper.MakeInParam(paramName, values[i]));
            }
            return string.Format("{0} IN ({1})", MySqlParamHelper.FormatName(fieldName), string.Join(",", paramNames));

        }
    }
}
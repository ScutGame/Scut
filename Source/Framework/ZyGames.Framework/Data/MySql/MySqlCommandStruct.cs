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
using System.Text;
using MySql.Data.MySqlClient;

namespace ZyGames.Framework.Data.MySql
{
    /// <summary>
    /// 
    /// </summary>
    public class MySqlCommandStruct : CommandStruct
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="editType"></param>
        public MySqlCommandStruct(string tableName, CommandMode editType)
            : base(tableName, editType)
        {
        }

        public override void AddExpressParam(string paramName, int dbType, int size, object value)
        {
            AddExpressParam(MySqlParamHelper.MakeInParam(paramName,(MySqlDbType) dbType, size, value));
        }

        public override void AddParameter(string field, object value)
        {
            AddParameter(field, (int)MySqlDbType.VarChar, value);
        }

        public override void AddParameter(string field, int sqlDbType, int size, object value)
        {
            AddParameter(MySqlParamHelper.MakeInParam(field, (MySqlDbType)sqlDbType, size, value));
        }

        protected override string FormatFieldName(string field)
        {
            return string.Format("`{0}`", field);
        }

        protected override string FormatUpdateInsertSql(string tableName, string updateSets, string condition, string insertFields, string insertValues)
        {
            return string.Format(@"INSERT INTO {0}({1})VALUES({2})
 ON DUPLICATE KEY UPDATE {3}",
                          tableName,
                          insertFields,
                          insertValues,
                          updateSets);
        }

    }
}
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
        /// <param name="columns"></param>
        public MySqlCommandStruct(string tableName, CommandMode editType, string columns = "")
            : base(tableName, editType, columns)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        public override void AddExpressParam(string paramName, object value)
        {
            AddExpressParam(MySqlParamHelper.MakeInParam(paramName, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        protected override void AddExpressParam(string paramName, int dbType, int size, object value)
        {
            AddExpressParam(MySqlParamHelper.MakeInParam(paramName, (MySqlDbType)dbType, size, value));
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="value">参数</param>
        public override void AddParameter(string field, object value)
        {
            AddParameter(MySqlParamHelper.MakeInParam(field, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sqlDbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        protected override void AddParameter(string field, int sqlDbType, int size, object value)
        {
            AddParameter(MySqlParamHelper.MakeInParam(field, (MySqlDbType)sqlDbType, size, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override void AddParameterByGuid(string paramName, object value)
        {
            AddParameter(MySqlParamHelper.MakeInParam(paramName, MySqlDbType.VarChar, 32, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override void AddParameterByText(string paramName, object value)
        {
            AddParameter(MySqlParamHelper.MakeInParam(paramName, MySqlDbType.LongText, 0, value));
        }

        /// <summary>
        /// Formats the update insert sql.
        /// </summary>
        /// <returns>The update insert sql.</returns>
        /// <param name="tableName">Table name.</param>
        /// <param name="updateSets">Update sets.</param>
        /// <param name="condition">Condition.</param>
        /// <param name="insertFields">Insert fields.</param>
        /// <param name="insertValues">Insert values.</param>
        protected override string FormatUpdateInsertSql(string tableName, string updateSets, string condition, string insertFields, string insertValues)
        {
            return string.Format(@"INSERT INTO {0}({1})VALUES({2})
 ON DUPLICATE KEY UPDATE {3}",
                          tableName,
                          insertFields,
                          insertValues,
                          updateSets);
        }
        /// <summary>
        /// Parsers the inquiry.
        /// </summary>
        protected override void ParserInquiry()
        {
            List<IDataParameter> paramList = new List<IDataParameter>();
            string condition = Filter == null ? "" : Filter.Condition.Trim();
            if (condition.Trim().Length > 0)
            {
                if (!condition.StartsWith("WHERE", true, null))
                {
                    condition = "WHERE " + condition;
                }
                if (Filter != null && Filter.Parameters != null && Filter.Parameters.Length > 0)
                {
                    AppendToDataParam(paramList, Filter.Parameters);
                }
            }
            string orderBy = OrderBy ?? "";
            if (!string.IsNullOrEmpty(orderBy) && !orderBy.StartsWith("ORDER BY", true, null))
            {
                orderBy = " ORDER BY " + orderBy;
            }

            if (ToIndex > 0 && FromIndex > 0)
            {
                Sql = string.Format("SELECT * FROM (SELECT {1},(@rowNum:=@rowNum+1) as RowNumber FROM {0},(Select (@rowNum :=0)) temp {2} {3}) T {4}",
                                    TableName,
                                    Columns,
                                    condition,
                                    orderBy,
                                    (FromIndex == 0 && ToIndex > 0
                                         ? " limit " + ToIndex
                                         : FromIndex > 0 && ToIndex > 0 ? " limit " + FromIndex + "," + ToIndex : ""));
            }
            else
            {
                Sql = string.Format("SELECT {1} FROM {0} {2} {3}",
                                    TableName,
                                    Columns,
                                    condition,
                                    orderBy);
            }

            Parameters = paramList.ToArray();
        }

        /// <summary>
        /// 格式化Select语句中的列名
        /// </summary>
        /// <param name="splitChat"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        protected override string FormatQueryColumn(string splitChat, ICollection<string> columns)
        {
            return MySqlParamHelper.FormatQueryColumn(splitChat, columns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string FormatName(string name)
        {
            return MySqlParamHelper.FormatName(name);
        }
    }
}
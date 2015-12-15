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
using System.Data.SqlClient;
using ZyGames.Framework.Data.Sql;

namespace ZyGames.Framework.Data
{

    /// <summary>
    /// 执行命令结构
    /// </summary>
    public class CommandStruct
    {
        private Dictionary<string, IDataParameter> _fieldList = new Dictionary<string, IDataParameter>();
        private List<string> _expressList = new List<string>();
        private Dictionary<string, IDataParameter> _parameter = new Dictionary<string, IDataParameter>();
        private Dictionary<string, IDataParameter> _keyList = new Dictionary<string, IDataParameter>();
        protected HashSet<string> _increaseFields = new HashSet<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="editType"></param>
        /// <param name="columns"></param>
        public CommandStruct(string tableName, CommandMode editType, string columns = "")
            : this(tableName, editType, new CommandFilter(), columns)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="editType"></param>
        /// <param name="columns">Inquiry table columns</param>
        /// <param name="filter"></param>
        public CommandStruct(string tableName, CommandMode editType, CommandFilter filter, string columns = "")
        {
            TableName = tableName;
            EntityType = editType;
            Columns = columns;
            Filter = filter;
            Parameters = new IDataParameter[0];
            CommandType = CommandType.Text;
        }

        /// <summary>
        /// CommandType
        /// </summary>
        public CommandType CommandType { get; set; }

        private string _tableName;

        /// <summary>
        /// 
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = FormatName(value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public CommandMode EntityType
        {
            get;
            set;
        }

        private string _columns;

        /// <summary>
        /// 
        /// </summary>
        public string Columns
        {
            get { return _columns; }
            set { _columns = FormatQueryColumn(",", value.Split(',')); }
        }

        /// <summary>
        /// Set top record.
        /// </summary>
        public int Top
        {
            set
            {
                FromIndex = 0;
                ToIndex = value;
            }
        }

        /// <summary>
        /// 是否返回自增ID值
        /// </summary>
        public bool ReturnIdentity { get; set; }

        /// <summary>
        /// Gets or sets the index of the from.
        /// </summary>
        /// <value>The index of the from.</value>
        public int FromIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the to.
        /// </summary>
        /// <value>The index of the to.</value>
        public int ToIndex { get; set; }

        /// <summary>
        /// Gets or sets the order by.
        /// </summary>
        /// <value>The order by.</value>
        public string OrderBy
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        public void SetPage(int page, int pageSize)
        {
            if (page < 1) throw new ArgumentOutOfRangeException("page");
            if (pageSize < 1) throw new ArgumentOutOfRangeException("pageSize");

            FromIndex = (page - 1) * pageSize;
            ToIndex = page * pageSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="isIdentity">key is auto increase</param>
        public void AddKey(IDataParameter parameter, bool isIdentity)
        {
            if (parameter == null)
            {
                return;
            }
            string paramKey = parameter.GetFieldName();
            if (_keyList.ContainsKey(paramKey))
            {
                _keyList[paramKey] = parameter;
            }
            else
            {
                _keyList.Add(paramKey, parameter);
            }
            if (isIdentity) _increaseFields.Add(paramKey);
        }
        /// <summary>
        /// 
        /// </summary>
        public CommandFilter Filter
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Sql
        {
            get;
            protected set;
        }
        /// <summary>
        /// 
        /// </summary>
        public IDataParameter[] Parameters
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        public SqlParameter[] SqlParameters
        {
            get
            {
                SqlParameter[] list = new SqlParameter[Parameters.Length];
                for (int i = 0; i < Parameters.Length; i++)
                {
                    list[i] = (SqlParameter)Parameters[i];
                }
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="express"></param>
        public void AddExpress(string express)
        {
            _expressList.Add(express);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        public virtual void AddExpressParam(string paramName, object value)
        {
            AddExpressParam(SqlParamHelper.MakeInParam(paramName, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        protected virtual void AddExpressParam(string paramName, int dbType, int size, object value)
        {
            AddExpressParam(SqlParamHelper.MakeInParam(paramName, (SqlDbType)dbType, size, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public void AddExpressParam(IDataParameter param)
        {
            string paramKey = param.GetFieldName();
            if (_parameter.ContainsKey(paramKey))
            {
                _parameter[paramKey] = param;
            }
            else
            {
                _parameter.Add(paramKey, param);
            }
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="value">参数</param>
        public virtual void AddParameter(string field, object value)
        {
            AddParameter(SqlParamHelper.MakeInParam(field, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sqlDbType"></param>
        /// <param name="value"></param>
        [Obsolete]
        public void AddParameter(string field, SqlDbType sqlDbType, object value)
        {
            AddParameter(field, (int)sqlDbType, value);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="sqlDbType">数据类型</param>
        /// <param name="value">参数</param>
        protected void AddParameter(string field, int sqlDbType, object value)
        {
            AddParameter(field, sqlDbType, 0, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sqlDbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        [Obsolete]
        public void AddParameter(string field, SqlDbType sqlDbType, int size, object value)
        {
            AddParameter(field, (int)sqlDbType, size, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual void AddParameterByGuid(string paramName, object value)
        {
            AddParameter(SqlParamHelper.MakeInParam(paramName, SqlDbType.UniqueIdentifier, 0, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual void AddParameterByText(string paramName, object value)
        {
            AddParameter(SqlParamHelper.MakeInParam(paramName, SqlDbType.Text, 0, value));
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="sqlDbType">数据类型</param>
        /// <param name="size">长度</param>
        /// <param name="value">参数</param>
        protected virtual void AddParameter(string field, int sqlDbType, int size, object value)
        {
            AddParameter(SqlParamHelper.MakeInParam(field, (SqlDbType)sqlDbType, size, value));
        }

        /// <summary>
        /// MSSQL插入需要排除自增列
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual bool IgnoreIncreaseField(string field)
        {
            return _increaseFields.Contains(field);
        }


        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="param"></param>
        public void AddParameter(IDataParameter param)
        {
            if (param == null)
            {
                return;
            }
            string paramKey = param.GetFieldName();
            if (_fieldList.ContainsKey(paramKey))
            {
                _fieldList[paramKey] = param;
            }
            else
            {
                _fieldList.Add(paramKey, param);
            }

        }

        /// <summary>
        /// Is generate
        /// </summary>
        public bool IsGenerated { get; private set; }

        /// <summary>
        /// 提交
        /// </summary>
        public void Parser()
        {
            if (IsGenerated) return;

            switch (EntityType)
            {
                case CommandMode.Insert:
                    ParserInsert();
                    break;
                case CommandMode.Modify:
                    ParserUpdate();
                    break;
                case CommandMode.Delete:
                    ParserDelete();
                    break;
                case CommandMode.ModifyInsert:
                    ParserUpdateInsert();
                    break;
                case CommandMode.Inquiry:
                    ParserInquiry();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            IsGenerated = true;
        }
        /// <summary>
        /// Parsers the inquiry.
        /// </summary>
        protected virtual void ParserInquiry()
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
            bool isTop = false;
            if (FromIndex == 0 && ToIndex > 0)
            {
                isTop = true;
            }
            string orderBy = OrderBy ?? "";
            if (!string.IsNullOrEmpty(orderBy) && !orderBy.StartsWith("ORDER BY", true, null))
            {
                orderBy = " ORDER BY " + orderBy;
            }

            if (isTop)
            {
                Sql = string.Format("SELECT TOP {3} {1},ROW_NUMBER() OVER ({4})AS RowNumber FROM {0} {2}",
                    TableName,
                    Columns,
                    condition,
                    ToIndex,
                    orderBy);
            }
            else if (FromIndex > 0 && ToIndex > 0)
            {
                Sql = string.Format(@"SELECT * FROM (SELECT {1},ROW_NUMBER() OVER ({3})AS RowNumber FROM {0} {2}) AS TMPTABLE
    WHERE RowNumber BETWEEN {4} AND {5}",
                    TableName,
                    Columns,
                    condition,
                    OrderBy,
                    FromIndex,
                    ToIndex);
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
        /// 
        /// </summary>
        protected virtual void ParserInsert()
        {
            HashSet<string> insertFieldNames = new HashSet<string>();
            HashSet<string> insertParamNames = new HashSet<string>();
            List<IDataParameter> paramList = new List<IDataParameter>();

            foreach (string field in _keyList.Keys)
            {
                string fieldName = FormatName(field);
                string parmName = _keyList.ContainsKey(field) ? _keyList[field].ParameterName : string.Empty;
                if (!string.IsNullOrEmpty(parmName) &&
                    !IgnoreIncreaseField(field) &&
                    !insertFieldNames.Contains(fieldName))
                {
                    insertFieldNames.Add(fieldName);
                    insertParamNames.Add(parmName);
                    paramList.Add(_keyList[field]);
                }
            }
            foreach (string field in _fieldList.Keys)
            {
                string fieldName = FormatName(field);
                string parmName = _fieldList.ContainsKey(field) ? _fieldList[field].ParameterName : string.Empty;
                if (!string.IsNullOrEmpty(parmName) && !insertFieldNames.Contains(fieldName))
                {
                    insertFieldNames.Add(fieldName);
                    insertParamNames.Add(parmName);
                    paramList.Add(_fieldList[field]);
                }
            }

            Parameters = paramList.ToArray();
            var tempArray1 = new string[insertFieldNames.Count];
            var tempArray2 = new string[insertParamNames.Count];
            insertFieldNames.CopyTo(tempArray1, 0);
            insertParamNames.CopyTo(tempArray2, 0);

            Sql = string.Format("INSERT INTO {0}({1})VALUES({2}){3}",
                    TableName,
                    string.Join(",", tempArray1),
                    string.Join(",", tempArray2),
                    ReturnIdentity ? ";SELECT @@IDENTITY;" : "");
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void ParserUpdateInsert()
        {
            List<string> updateFieldNames = new List<string>();
            HashSet<string> insertFieldNames = new HashSet<string>();
            HashSet<string> insertParamNames = new HashSet<string>();
            List<IDataParameter> updateFieldParams = new List<IDataParameter>();
            string updateKeyExpress = string.Empty;

            foreach (string field in _keyList.Keys)
            {
                string fieldName = FormatName(field);
                string paramName = _keyList.ContainsKey(field) ? _keyList[field].ParameterName : string.Empty;
                if (!string.IsNullOrEmpty(paramName) &&
                    !IgnoreIncreaseField(field) &&
                    !insertFieldNames.Contains(fieldName))
                {
                    insertFieldNames.Add(fieldName);
                    insertParamNames.Add(paramName);
                    if (string.IsNullOrEmpty(updateKeyExpress))
                    {
                        updateKeyExpress = string.Format("{0} = {1}", fieldName, paramName);
                    }
                    AppendToDataParam(updateFieldParams, _keyList[field]);
                }
            }

            foreach (string field in _fieldList.Keys)
            {
                string fieldName = FormatName(field);
                string paramName = _fieldList.ContainsKey(field) ? _fieldList[field].ParameterName : string.Empty;
                //修改原因：如果是Key，已经处理过了需要排除
                if (!string.IsNullOrEmpty(paramName) && !_keyList.ContainsKey(field))
                {
                    insertFieldNames.Add(fieldName);
                    insertParamNames.Add(paramName);

                    updateFieldNames.Add(string.Format("{0} = {1}", fieldName, paramName));
                    //updateFieldParams.Add(_fieldList[field]);
                    AppendToDataParam(updateFieldParams, _fieldList[field]);
                }
            }

            foreach (string express in _expressList)
            {
                updateFieldNames.Add(express);
            }
            foreach (KeyValuePair<string, IDataParameter> param in _parameter)
            {
                //updateFieldParams.Add(param.Value);
                AppendToDataParam(updateFieldParams, param.Value);
            }

            string condition = Filter == null ? "" : Filter.Condition.Trim();
            if (condition.Trim().Length > 0)
            {
                if (!condition.StartsWith("WHERE", true, null))
                {
                    condition = "WHERE " + condition;
                }
                if (Filter != null && Filter.Parameters != null && Filter.Parameters.Length > 0)
                {
                    //updateFieldParams.AddRange(Filter.Parameters);
                    AppendToDataParam(updateFieldParams, Filter.Parameters);
                }
            }

            var tempArray1 = new string[insertFieldNames.Count];
            var tempArray2 = new string[insertParamNames.Count];
            insertFieldNames.CopyTo(tempArray1, 0);
            insertParamNames.CopyTo(tempArray2, 0);
            if (updateFieldNames.Count == 0)
            {
                //add key field
                updateFieldNames.Add(updateKeyExpress);
            }

            Sql = FormatUpdateInsertSql(
                TableName,
                string.Join(",", updateFieldNames.ToArray()),
                condition,
                string.Join(",", tempArray1),
                string.Join(",", tempArray2)
                );
            //TraceLog.WriteComplement(Sql);
            Parameters = updateFieldParams.ToArray();
        }
        /// <summary>
        /// Appends to data parameter.
        /// </summary>
        /// <param name="paramList">Parameter list.</param>
        /// <param name="dataParameters">Data parameters.</param>
        protected void AppendToDataParam(List<IDataParameter> paramList, params IDataParameter[] dataParameters)
        {
            foreach (var dataParameter in dataParameters)
            {
                paramList.Add(dataParameter);
            }
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
        protected virtual string FormatUpdateInsertSql(string tableName, string updateSets, string condition, string insertFields, string insertValues)
        {
            return string.Format(@"UPDATE {0} SET {1} {2}
if @@rowcount = 0
INSERT INTO {0}({3})VALUES({4})",
                                tableName,
                                updateSets,
                                condition,
                                insertFields,
                                insertValues);
        }
        /// <summary>
        /// Parsers the update.
        /// </summary>
        protected virtual void ParserUpdate()
        {
            List<string> updateFieldNames = new List<string>();
            List<IDataParameter> updateFieldParams = new List<IDataParameter>();
            foreach (string field in _fieldList.Keys)
            {
                string fieldName = FormatName(field);
                //修改原因：如果是Key，不允许更新需发排除
                if (!_keyList.ContainsKey(field))
                {
                    updateFieldNames.Add(string.Format("{0} = {1}", fieldName, _fieldList[field].ParameterName));
                    updateFieldParams.Add(_fieldList[field]);
                }
            }
            foreach (string express in _expressList)
            {
                updateFieldNames.Add(express);
            }
            foreach (KeyValuePair<string, IDataParameter> param in _parameter)
            {
                updateFieldParams.Add(param.Value);
            }

            string condition = Filter == null ? "" : Filter.Condition.Trim();
            if (condition != null && condition.Trim().Length > 0)
            {
                if (!condition.StartsWith("WHERE", true, null))
                {
                    condition = "WHERE " + condition;
                }
                if (Filter.Parameters != null && Filter.Parameters.Length > 0)
                {
                    updateFieldParams.AddRange(Filter.Parameters);
                }
            }

            Sql = string.Format("UPDATE {0} SET {1} {2}", TableName, string.Join(",", updateFieldNames.ToArray()), condition);
            Parameters = updateFieldParams.ToArray();
        }
        /// <summary>
        /// Parsers the delete.
        /// </summary>
        protected virtual void ParserDelete()
        {
            List<IDataParameter> fieldParams = new List<IDataParameter>();
            string condition = Filter == null ? "" : Filter.Condition.Trim();
            if (condition != null && condition.Trim().Length > 0)
            {
                if (!condition.StartsWith("WHERE", true, null))
                {
                    condition = "WHERE " + condition;
                }
                if (Filter.Parameters != null && Filter.Parameters.Length > 0)
                {
                    fieldParams.AddRange(Filter.Parameters);
                }
            }

            Sql = string.Format("DELETE FROM {0} {1}", TableName, condition);
            Parameters = fieldParams.ToArray();
        }

        /// <summary>
        /// 格式化Select语句中的列名
        /// </summary>
        /// <param name="splitChat"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        protected virtual string FormatQueryColumn(string splitChat, ICollection<string> columns)
        {
            return SqlParamHelper.FormatQueryColumn(splitChat, columns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string FormatName(string name)
        {
            return SqlParamHelper.FormatName(name);
        }
    }
}
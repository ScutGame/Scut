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
using System.Diagnostics;
using ZyGames.Framework.Common.Log;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="editType"></param>
        public CommandStruct(string tableName, CommandMode editType)
        {
            TableName = tableName;
            EntityType = editType;
            Filter = new CommandFilter();
            Parameters = new IDataParameter[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public string TableName
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public CommandMode EntityType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void AddKey(IDataParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }
            string paramKey = parameter.GetFieldName();
            //todo 修正：多个@参数
            //if (!parameter.ParameterName.StartsWith(_preParamChar))
            //{
            //    parameter.ParameterName = _preParamChar + parameter.ParameterName;
            //}
            if (_keyList.ContainsKey(paramKey))
            {
                _keyList[paramKey] = parameter;
            }
            else
            {
                _keyList.Add(paramKey, parameter);
            }
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
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        public IDataParameter[] Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
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
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        public virtual void AddExpressParam(string paramName, int dbType, int size, object value)
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
            //todo 修正：多个@参数
            //if (!param.ParameterName.StartsWith(_preParamChar))
            //{
            //    param.ParameterName = _preParamChar + param.ParameterName;
            //}
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
            AddParameter(field, (int)SqlDbType.VarChar, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sqlDbType"></param>
        /// <param name="value"></param>
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
        public void AddParameter(string field, int sqlDbType, object value)
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
        public void AddParameter(string field, SqlDbType sqlDbType, int size, object value)
        {
            AddParameter(field, (int)sqlDbType, size, value);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="sqlDbType">数据类型</param>
        /// <param name="size">长度</param>
        /// <param name="value">参数</param>
        public virtual void AddParameter(string field, int sqlDbType, int size, object value)
        {
            AddParameter(SqlParamHelper.MakeInParam(field, (SqlDbType)sqlDbType, size, value));
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
            //todo 修正：多个@参数
            //if (!param.ParameterName.StartsWith(_preParamChar))
            //{
            //    param.ParameterName = _preParamChar + param.ParameterName;
            //}
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
        /// 提交
        /// </summary>
        public void Parser()
        {
            if (EntityType == CommandMode.Insert)
            {
                ParserInsert();
            }
            else if (EntityType == CommandMode.Modify)
            {
                ParserUpdate();
            }
            else if (EntityType == CommandMode.Delete)
            {
                ParserDelete();
            }
            else if (EntityType == CommandMode.ModifyInsert)
            {
                ParserUpdateInsert();
            }
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
                string fieldName = FormatFieldName(field);
                string parmName = _keyList.ContainsKey(field) ? _keyList[field].ParameterName : string.Empty;
                if (!string.IsNullOrEmpty(parmName) && !insertFieldNames.Contains(fieldName))
                {
                    insertFieldNames.Add(fieldName);
                    insertParamNames.Add(parmName);
                    paramList.Add(_keyList[field]);
                }
            }
            foreach (string field in _fieldList.Keys)
            {
                string fieldName = FormatFieldName(field);
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

            Sql = string.Format("INSERT INTO {0}({1})VALUES({2})", TableName,
                string.Join(",", tempArray1), string.Join(",", tempArray2));
        }

        protected virtual string FormatFieldName(string field)
        {
            return string.Format("[{0}]", field);
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

            foreach (string field in _keyList.Keys)
            {
                string fieldName = FormatFieldName(field);
                string paramName = _keyList.ContainsKey(field) ? _keyList[field].ParameterName : string.Empty;
                if (!string.IsNullOrEmpty(paramName) && !insertFieldNames.Contains(fieldName))
                {
                    insertFieldNames.Add(fieldName);
                    insertParamNames.Add(paramName);
                    //updateFieldParams.Add(_keyList[field]);
                    AppendToDataParam(updateFieldParams, _keyList[field]);
                }
            }

            foreach (string field in _fieldList.Keys)
            {
                string fieldName = FormatFieldName(field);
                string paramName = _fieldList.ContainsKey(field) ? _fieldList[field].ParameterName : string.Empty;
                //修改原因：如果是Key，则排除
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

            Sql = FormatUpdateInsertSql(
                TableName,
                string.Join(",", updateFieldNames.ToArray()),
                condition,
                string.Join(",", tempArray1),
                string.Join(",", tempArray2)
                );
            //todo trace updateinsert sql
            //TraceLog.WriteComplement(Sql);
            Parameters = updateFieldParams.ToArray();
        }

        private void AppendToDataParam(List<IDataParameter> paramList, params IDataParameter[] dataParameters)
        {
            foreach (var dataParameter in dataParameters)
            {
                paramList.Add(dataParameter);
            }
        }

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

        protected virtual void ParserUpdate()
        {
            List<string> updateFieldNames = new List<string>();
            List<IDataParameter> updateFieldParams = new List<IDataParameter>();
            foreach (string field in _fieldList.Keys)
            {
                //修改原因：如果是Key，则排除
                if (!_keyList.ContainsKey(field))
                {
                    updateFieldNames.Add(string.Format("{0} = {1}", field, _fieldList[field].ParameterName));
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

    }
}
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using ZyGames.Core.Data;
using System.Collections;
using ZyGames.ActionMsmq;

namespace ZyGames.Common
{
    public enum EditType
    {
        Insert = 0,
        Update,
        Delete
    }

    public class CommandHelper
    {
        private const string PreParamChar = "@";
        private Dictionary<string, SqlParameter> fieldList = new Dictionary<string, SqlParameter>();
        private List<string> expressList = new List<string>();
        private List<SqlParameter> parameter = new List<SqlParameter>();

        public CommandHelper(string tableName, EditType editType)
        {
            TableName = tableName;
            EditType = editType;
        }

        public string TableName
        {
            get;
            set;
        }

        public EditType EditType
        {
            get;
            set;
        }

        public CommandFilter Filter
        {
            get;
            set;
        }

        public string Sql
        {
            get;
            private set;
        }

        public SqlParameter[] Parameters
        {
            get;
            private set;
        }

        public void AddExpress(string express)
        {
            expressList.Add(express);
        }


        public void AddExpressParam(string paramName, SqlDbType sqlType, int size, object value)
        {
            if (parameter.Exists(p => p.ParameterName == paramName))
            {
                SqlParameter sqlParam = parameter.Find(p => p.ParameterName == paramName);
                sqlParam.SqlDbType = sqlType;
                sqlParam.Value = value;
                sqlParam.Size = size;
            }
            else
            {
                parameter.Add(SqlParamHelper.MakeInParam(paramName, sqlType, size, value));
            }
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="value">参数</param>
        public void AddParameter(string field, object value)
        {
            AddParameter(field, SqlDbType.VarChar, value);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="sqlDbType">数据类型</param>
        /// <param name="value">参数</param>
        public void AddParameter(string field, SqlDbType sqlDbType, object value)
        {
            AddParameter(field, sqlDbType, 0, value);
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="sqlDbType">数据类型</param>
        /// <param name="size">长度</param>
        /// <param name="value">参数</param>
        public void AddParameter(string field, SqlDbType sqlDbType, int size, object value)
        {
            if (fieldList.ContainsKey(field))
            {
                fieldList[field] = SqlParamHelper.MakeInParam(PreParamChar + field, sqlDbType, size, value);
            }
            else
            {
                fieldList.Add(field, SqlParamHelper.MakeInParam(PreParamChar + field, sqlDbType, size, value));
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        public void Parser()
        {
            if (EditType == EditType.Insert)
            {
                ParserInsert();
            }
            else if (EditType == EditType.Update)
            {
                ParserUpdate();
            }
            else if (EditType == EditType.Delete)
            {
                ParserDelete();
            }
        }

        private void ParserInsert()
        {
            string[] fieldNames = new string[fieldList.Keys.Count];
            string[] paramNames = new string[fieldList.Keys.Count];
            Parameters = new SqlParameter[fieldList.Keys.Count];
            int index = 0;
            foreach (string field in fieldList.Keys)
            {
                fieldNames[index] = string.Format("[{0}]", field);
                paramNames[index] = fieldList[field].ParameterName;
                Parameters[index] = fieldList[field];
                index++;
            }
            Sql = string.Format("INSERT INTO {0}({1})VALUES({2})", TableName, string.Join(",", fieldNames), string.Join(",", paramNames));
        }

        
        private void ParserUpdate()
        {
            List<string> fieldNames = new List<string>();
            List<SqlParameter> fieldParams = new List<SqlParameter>();
            foreach (string field in fieldList.Keys)
            {
                fieldNames.Add(string.Format("[{0}] = {1}", field, fieldList[field].ParameterName));
                fieldParams.Add(fieldList[field]);
            }
            foreach (string express in expressList)
            {
                fieldNames.Add(express);
            }
            foreach (SqlParameter param in parameter)
            {
                fieldParams.Add(param);
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
                    fieldParams.AddRange(Filter.Parameters);
                }
            }

            Sql = string.Format("UPDATE {0} SET {1} {2}", TableName, string.Join(",", fieldNames.ToArray()), condition);
            Parameters = fieldParams.ToArray();
        }

        private void ParserDelete()
        {
            List<SqlParameter> fieldParams = new List<SqlParameter>();
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
        /// 操作数据库
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        public void SendSqlCmd(string connection, CommandType type, string sql, SqlParameter[] para)
        {
#if DEBUG
            SqlHelper.ExecuteNonQuery(connection, type, sql, para);
#else
            ActionMSMQ.Instance().SendSqlCmd(connection, type, sql, para);
#endif
        }

    }
}

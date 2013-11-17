using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using ZyGames.Core.Data;

namespace ZyGames
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

        public void AddParameter(string field, object value)
        {
            AddParameter(field, SqlDbType.VarChar, value);
        }

        public void AddParameter(string field, SqlDbType sqlDbType, object value)
        {
            AddParameter(field, sqlDbType, 0, value);
        }

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

        }

        private void ParserDelete()
        {
        }
    }
}

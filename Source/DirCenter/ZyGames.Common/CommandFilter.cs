using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using ZyGames.Core.Data;

namespace ZyGames.Common
{
    public class CommandFilter
    {
        private List<SqlParameter> parameter = new List<SqlParameter>();

        public string Condition
        {
            get;
            set;
        }

        public SqlParameter[] Parameters
        {
            get
            {
                return parameter.ToArray();
            }
        }


        public void AddParam(string paramName, SqlDbType sqlType, int size, object value)
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
    }
}

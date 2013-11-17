using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using ZyGames.Core.Data;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class SolutionDAL
    {
        public static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(SolutionModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Solutions(");
            strSql.Append("SlnName,Namespace,RefNamespace,Url,GameID)");
            strSql.Append(" values (");
            strSql.Append("@SlnName,@Namespace,@RefNamespace,@Url,@GameID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@SlnName", SqlDbType.VarChar, 0),
                    new SqlParameter("@Namespace", SqlDbType.VarChar, 0),
                    new SqlParameter("@RefNamespace", SqlDbType.VarChar, 0),
                    new SqlParameter("@Url", SqlDbType.VarChar, 0),
                     new SqlParameter("@GameID", SqlDbType.Int, 4)
					};
            parameters[0].Value = model.SlnName;
            parameters[1].Value = model.Namespace;
            parameters[2].Value = model.RefNamespace;
            parameters[3].Value = model.Url;
            parameters[4].Value = model.GameID;
            object obj = SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SlnID,SlnName,Namespace,RefNamespace,Url,GameID ");
            strSql.Append(" FROM Solutions ");
             if(!strWhere.Equals(""))
            {
                strSql.Append(" where "+strWhere );
            }
             strSql.Append("  Order By SlnID asc");
            return SqlHelper.ExecuteDataset(connectionString,CommandType.Text,strSql.ToString());
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetServerList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select [ID]
      ,[GameID]
      ,[ServerName]
      ,[BaseUrl]
      ,[ActiveNum]
      ,[Weight]
      ,[IsEnable]
      ,[TargetServer]
      ,[EnableDate] ");
            strSql.Append(" FROM [PayDB].[dbo].[ServerInfo] ");
            if (!strWhere.Equals(""))
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append("  Order By GameID asc");
            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, strSql.ToString());
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(SolutionModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Solutions set ");
            strSql.Append("SlnName=@SlnName,Namespace=@Namespace,RefNamespace=@RefNamespace,Url=@Url,GameID=@GameID");
            strSql.Append(" where SlnID=@SlnID");
            SqlParameter[] parameters = {
					new SqlParameter("@SlnID", SqlDbType.Int,4),
					new SqlParameter("@SlnName", SqlDbType.VarChar, 0),
					new SqlParameter("@Namespace", SqlDbType.VarChar, 0),
					new SqlParameter("@RefNamespace", SqlDbType.VarChar, 0),
					new SqlParameter("@Url", SqlDbType.VarChar, 0),
                    new SqlParameter("@GameID", SqlDbType.Int, 4)
            };
            parameters[0].Value = model.SlnID;
            parameters[1].Value = model.SlnName;
            parameters[2].Value = model.Namespace;
            parameters[3].Value = model.RefNamespace;
            parameters[4].Value = model.Url;
            parameters[5].Value = model.GameID;
            int rows = SqlHelper.ExecuteNonQuery(connectionString,CommandType.Text,strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Solutions ");
            strSql.Append(" where SlnID=@SlnID");
            SqlParameter[] parameters = {
					new SqlParameter("@SlnID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            int rows = SqlHelper.ExecuteNonQuery(connectionString,CommandType.Text,strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}

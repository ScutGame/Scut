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
    public class AgreementDAL
    {
        public static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(AgreementModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into AgreementClass(");
            strSql.Append("GameID,Title,Describe)");
            strSql.Append(" values (");
            strSql.Append("@GameID,@Title,@Describe)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@GameID", SqlDbType.Int, 4),
                    new SqlParameter("@Title", SqlDbType.VarChar, 0),
                    new SqlParameter("@Describe", SqlDbType.VarChar, 0)
					};
            parameters[0].Value = model.GameID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.Describe;

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
        public DataSet GetList(string strWhere, string SlnID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select [AgreementID]
      ,[GameID]
      ,[Title]
      ,[Describe]
      ,[CreateDate],(select top 1 SlnName from Solutions where SlnID=" + SlnID + ") SlnName");
            strSql.Append(" FROM AgreementClass ");
             if(!strWhere.Equals(""))
            {
                strSql.Append(" where "+strWhere );
            }
             strSql.Append("  Order By AgreementID asc");
            return SqlHelper.ExecuteDataset(connectionString,CommandType.Text,strSql.ToString());
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(AgreementModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update AgreementClass set ");
            strSql.Append("Title=@Title,Describe=@Describe");
            strSql.Append(" where AgreementID=@AgreementID");
            SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.VarChar, 0),
					new SqlParameter("@Describe", SqlDbType.VarChar, 0),new SqlParameter("@AgreementID", SqlDbType.VarChar, 0)
            };

            parameters[0].Value = model.Title;
            parameters[1].Value = model.Describe;
            parameters[2].Value = model.AgreementID;
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
            strSql.Append("delete from AgreementClass ");
            strSql.Append(" where AgreementID=@AgreementID");
            SqlParameter[] parameters = {
					new SqlParameter("@AgreementID", SqlDbType.Int,4)};
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

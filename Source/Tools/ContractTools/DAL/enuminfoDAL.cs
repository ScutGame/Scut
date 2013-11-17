using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using System.Data.SqlClient;
using System.Data;
using ZyGames.Core.Data;
using System.Configuration;

namespace DAL
{
    public class EnumInfoDAL
    {
        public static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(EnumInfoModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into enuminfo(");
            strSql.Append("SlnID,enumName,enumDescription,enumValueInfo)");
            strSql.Append(" values (");
            strSql.Append("@SlnID,@enumName,@enumDescription,@enumValueInfo)");
            SqlParameter[] parameters = {
					new SqlParameter("@SlnID", SqlDbType.Int,4),
					new SqlParameter("@enumName", SqlDbType.VarChar,50),
					new SqlParameter("@enumDescription", SqlDbType.VarChar,200),
					new SqlParameter("@enumValueInfo", SqlDbType.VarChar,2000)};
            parameters[0].Value = model.SlnID;
            parameters[1].Value = model.enumName;
            parameters[2].Value = model.enumDescription;
            parameters[3].Value = model.enumValueInfo;
            int rows = SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, strSql.ToString(), parameters);

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
        /// 得到一个对象实体
        /// </summary>
        public EnumInfoModel GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 enumName,enumDescription,enumValueInfo from enuminfo");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),};
            parameters[0].Value = ID;


            SqlDataReader dr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, strSql.ToString(), parameters);
            if (dr.Read())
            {
                EnumInfoModel model = new EnumInfoModel();
                model.enumDescription = dr["enumDescription"].ToString();
                model.enumName = dr["enumName"].ToString();
                model.enumValueInfo = dr["enumValueInfo"].ToString();
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public EnumInfoModel GetModel(string enumName, string slnid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id, slnid, enumName,enumDescription,enumValueInfo from enuminfo");
            strSql.Append(" where enumName=@enumName and slnid=@slnid");
            SqlParameter[] parameters = {
					new SqlParameter("@enumName", SqlDbType.VarChar,500),
                    new SqlParameter("@slnid", SqlDbType.Int,4)};
            parameters[0].Value = enumName;
            parameters[1].Value = slnid;

            SqlDataReader dr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, strSql.ToString(), parameters);
            if (dr.Read())
            {
                EnumInfoModel model = new EnumInfoModel();
                model.enumDescription = dr["enumDescription"].ToString();
                model.enumName = enumName;
                model.enumValueInfo = dr["enumValueInfo"].ToString();
                model.ID = int.Parse(dr["id"].ToString());
                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int slnid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,enumName,enumDescription,enumValueInfo");
            strSql.Append(" FROM enuminfo ");
            strSql.Append(" where slnid =@slnid");
            strSql.Append(" order by enumName ");
            SqlParameter[] parameters = {
					new SqlParameter("@slnid", SqlDbType.Int,4)
                                         };
            parameters[0].Value = slnid;
            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>enumName,enumDescription,enumValueInfo
        public bool Update(EnumInfoModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update enuminfo set ");
            strSql.Append("enumName=@enumName,");
            strSql.Append("enumDescription=@enumDescription,");
            strSql.Append("enumValueInfo=@enumValueInfo");

            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@enumName", SqlDbType.VarChar,50),
					new SqlParameter("@enumDescription", SqlDbType.VarChar,500),
					new SqlParameter("@enumValueInfo", SqlDbType.VarChar,2000),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.enumName;
            parameters[1].Value = model.enumDescription;
            parameters[2].Value = model.enumValueInfo;
            parameters[3].Value = model.ID;

            int rows = SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, strSql.ToString(), parameters);
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
            strSql.Append("delete from enuminfo ");
            strSql.Append(" where ID=@ID");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction trans = null;
                conn.Open();
                try
                {

                    SqlCommand cmd = new SqlCommand(strSql.ToString(), conn);
                    cmd.Transaction = trans;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
            return true;

        }

    }
}

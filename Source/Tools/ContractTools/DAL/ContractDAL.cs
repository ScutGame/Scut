using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ZyGames.Core.Data;
using System.Configuration;
using model;
namespace DAL
{
    public class ContractDAL
    {

        public static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(model.ContractModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Contract(");
            strSql.Append("ID,Descption,ParentID,SlnID,Complated,AgreementID)");
            strSql.Append(" values (");
            strSql.Append("@ID,@Descption,@ParentID,@SlnID,@Complated,@AgreementID)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Descption", SqlDbType.VarChar,100),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@SlnID", SqlDbType.Int,4),
					new SqlParameter("@Complated", SqlDbType.Bit,0),
                    new SqlParameter("@AgreementID", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Descption;
            parameters[2].Value = model.ParentID;
            parameters[3].Value = model.SlnID;
            parameters[4].Value = model.Complated;
            parameters[5].Value = model.AgreementID;
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
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select ID,Descption,ParentID,SlnID,Complated,convert(nvarchar,ID)+'_'+Descption+'【'+ (case when Complated = 1 then '完成' else '未完成' end) +'】' uname,AgreementID");
            strSql.Append(" FROM Contract ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by SlnID,ID ");
            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, strSql.ToString());
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(ContractModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Contract set ");
            strSql.Append("Descption=@Descption,");
            strSql.Append("ParentID=@ParentID,");
            strSql.Append("AgreementID=@AgreementID");
            strSql.Append(" where ID=@ID and SlnID=@SlnID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Descption", SqlDbType.VarChar,100),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@SlnID", SqlDbType.Int,4),
                        new SqlParameter("@AgreementID", SqlDbType.Int,4)};
            parameters[0].Value = model.Descption;
            parameters[1].Value = model.ParentID;
            parameters[2].Value = model.ID;
            parameters[3].Value = model.SlnID;
            parameters[4].Value = model.AgreementID;
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
        /// 更新状态
        /// </summary>
        public bool UpdateStatus(int id, int slnId, bool complated)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Contract set ");
            strSql.Append("Complated=@Complated");
            strSql.Append(" where ID=@ID and SlnID=@SlnID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@SlnID", SqlDbType.Int,4),
					new SqlParameter("@Complated", SqlDbType.Bit,1)};
            parameters[0].Value = id;
            parameters[1].Value = slnId;
            parameters[2].Value = complated;

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
        public bool Delete(int ID, int SlnID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Contract ");
            strSql.Append(" where ID=@ID and SlnID=@SlnID ");


            StringBuilder Sql = new StringBuilder();
            strSql.Append("delete from ParamInfo ");
            strSql.Append("where ContractID=@ID and SlnID=@SlnID");


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction trans = null;
                conn.Open();



                try
                {

                    SqlCommand cmd = new SqlCommand(strSql.ToString(), conn);
                    cmd.Transaction = trans;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@SlnID", SlnID);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    cmd.CommandText = Sql.ToString();
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@SlnID", SlnID);
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
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ContractModel GetModel(int ID, int SlnID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Descption,ParentID,SlnID,Complated,AgreementID from Contract ");
            strSql.Append(" where ID=@ID and SlnID=@SlnID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@SlnID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = SlnID;

            ContractModel model = new ContractModel();
            DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["SlnID"] != null && row["SlnID"].ToString() != "")
                {
                    model.SlnID = int.Parse(row["SlnID"].ToString());
                }
                if (row["Descption"] != null && row["Descption"].ToString() != "")
                {
                    model.Descption = row["Descption"].ToString();
                }
                if (row["ParentID"] != null && row["ParentID"].ToString() != "")
                {
                    model.ParentID = int.Parse(row["ParentID"].ToString());
                }
                if (row["Complated"] != null && row["Complated"].ToString() != "")
                {
                    model.Complated = bool.Parse(row["Complated"].ToString());
                }
                if (row["AgreementID"] != null && row["AgreementID"].ToString() != "")
                {
                    model.AgreementID = int.Parse(row["AgreementID"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public bool Copy(int slnID, int contractID, int copySlnID, int copyContractID)
        {
            string sql = string.Format("select count(*) count from Contract WHERE ID={0} AND SlnID={1}", copyContractID, copySlnID);
            if (Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sql)) > 0)
            {
                return false;
            }

            SqlParameter[] parameters = {
					new SqlParameter("@ContractID", SqlDbType.Int,4),
					new SqlParameter("@SlnID", SqlDbType.Int,4)};
            parameters[0].Value = contractID;
            parameters[1].Value = slnID;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"insert into Contract(ID,Descption,ParentID,SlnID) SELECT {1} ID,Descption,ParentID,{0} SlnID FROM Contract WHERE ID=@ContractID AND SlnID=@SlnID;
if @@rowcount > 0
insert into ParamInfo(ContractID,ParamType,Field,FieldType,Descption,FieldValue,Required,Remark,SortID,Creator,CreateDate,Modifier,SlnID,MinValue,MaxValue) select {1} ContractID,ParamType,Field,FieldType,Descption,FieldValue,Required,Remark,SortID,Creator,getdate() CreateDate,Modifier,{0} SlnID,MinValue,MaxValue from ParamInfo WHERE ContractID=@ContractID AND SlnID=@SlnID;",
                copySlnID, copyContractID);


            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, strSql.ToString(), parameters) > 0;

        }

        public DataSet Search(int slnID, string p)
        {
            if (slnID != 0)
            {
                string sql = "select id, slnid, descption from contract where slnid=@slnid and  (id in(select distinct [ContractID] from [paraminfo] where slnid=@slnid and ([Descption] like @Descption or remark like @Descption)) or id in (select id from contract where ([Descption] like @Descption or id like @Descption) and slnid=@slnid)) order by id";
                SqlParameter[] parameters = {
					new SqlParameter("@slnid", SqlDbType.Int,4),
					new SqlParameter("@Descption", SqlDbType.VarChar,200)};
                parameters[0].Value = slnID;
                parameters[1].Value = "%" + p + "%";
                return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql, parameters);
            }
            else
            {
                string sql = "select contract.id, contract.slnid, descption from contract inner join(select id,slnid from contract where [Descption] like @Descption union select distinct [ContractID], slnid from [paraminfo] where ([Descption] like @Descption or remark like @Descption))a on contract.id = a.id and contract.slnid=a.slnid order by contract.id";
                SqlParameter[] parameters = {
					new SqlParameter("@Descption", SqlDbType.VarChar,200)};
                parameters[0].Value = "%" + p + "%";
                return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql, parameters);
            }
        }
    }



}

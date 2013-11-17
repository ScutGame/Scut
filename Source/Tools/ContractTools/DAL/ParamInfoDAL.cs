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
    public  class ParamInfoDAL
    {
        public static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ParamInfoModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ParamInfo(");
            strSql.Append("SlnID,ContractID,ParamType,Field,FieldType,Descption,FieldValue,Required,Remark,SortID,Creator,Modifier,MinValue,MaxValue)");
            strSql.Append(" values (");
            strSql.Append("@SlnID,@ContractID,@ParamType,@Field,@FieldType,@Descption,@FieldValue,@Required,@Remark,@SortID,@Creator,@Modifier,@MinValue,@MaxValue)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@SlnID", SqlDbType.Int,4),
					new SqlParameter("@ContractID", SqlDbType.Int,4),
					new SqlParameter("@ParamType", SqlDbType.SmallInt,2),
					new SqlParameter("@Field", SqlDbType.VarChar,30),
					new SqlParameter("@FieldType", SqlDbType.SmallInt,2),
					new SqlParameter("@Descption", SqlDbType.VarChar,100),
					new SqlParameter("@FieldValue", SqlDbType.VarChar,100),
					new SqlParameter("@Required", SqlDbType.Bit,1),
					new SqlParameter("@Remark", SqlDbType.VarChar,500),
					new SqlParameter("@SortID", SqlDbType.Int,4),
					new SqlParameter("@Creator", SqlDbType.Int,4),
					new SqlParameter("@Modifier", SqlDbType.Int,4),
					new SqlParameter("@MinValue", SqlDbType.Int,4),
					new SqlParameter("@MaxValue", SqlDbType.Int,4)
					};
            parameters[0].Value = model.SlnID;
            parameters[1].Value = model.ContractID;
            parameters[2].Value = model.ParamType;
            parameters[3].Value = model.Field;
            parameters[4].Value = model.FieldType;
            parameters[5].Value = model.Descption;
            parameters[6].Value = model.FieldValue;
            parameters[7].Value = model.Required;
            parameters[8].Value = model.Remark;
            parameters[9].Value = model.SortID;
            parameters[10].Value = model.Creator;
            parameters[11].Value = model.Modifier;
            parameters[12].Value = model.MinValue;
            parameters[13].Value = model.MaxValue;

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
            strSql.Append("select ID,SlnID,ContractID,ParamType,Field,FieldType,Descption,FieldValue,Required,Remark,SortID,Creator,CreateDate,Modifier,ModifyDate,MinValue,MaxValue ");
            strSql.Append(" FROM ParamInfo ");
             if(!strWhere.Equals(""))
            {
                strSql.Append(" where "+strWhere );
            }
             strSql.Append("  Order By ParamType asc,SortID asc,ID asc");
            return SqlHelper.ExecuteDataset(connectionString,CommandType.Text,strSql.ToString());
        }

        public bool UpdateSort(int paramID, int sortID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParamInfo set ");
            strSql.Append("SortID=@SortID");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@SortID", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)
            };

            parameters[0].Value = sortID;
            parameters[1].Value = paramID;
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
        /// 更新一条数据
        /// </summary>
        public bool Update(ParamInfoModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParamInfo set ");
            strSql.Append("ContractID=@ContractID,");
            strSql.Append("ParamType=@ParamType,");
            strSql.Append("Field=@Field,");
            strSql.Append("FieldType=@FieldType,");
            strSql.Append("Descption=@Descption,");
            strSql.Append("FieldValue=@FieldValue,");
            strSql.Append("Required=@Required,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("SortID=@SortID,");
            strSql.Append("Creator=@Creator,");
            strSql.Append("Modifier=@Modifier,");
            strSql.Append("MinValue=@MinValue,");
            strSql.Append("MaxValue=@MaxValue,");
            strSql.Append("ModifyDate=@ModifyDate");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ContractID", SqlDbType.Int,4),
					new SqlParameter("@ParamType", SqlDbType.SmallInt,2),
					new SqlParameter("@Field", SqlDbType.VarChar,30),
					new SqlParameter("@FieldType", SqlDbType.SmallInt,2),
					new SqlParameter("@Descption", SqlDbType.VarChar,100),
					new SqlParameter("@FieldValue", SqlDbType.VarChar,100),
					new SqlParameter("@Required", SqlDbType.Bit,1),
					new SqlParameter("@Remark", SqlDbType.VarChar,500),
					new SqlParameter("@SortID", SqlDbType.Int,4),
					new SqlParameter("@Creator", SqlDbType.Int,4),
					new SqlParameter("@Modifier", SqlDbType.Int,4),
					new SqlParameter("@MinValue", SqlDbType.Int,4),
					new SqlParameter("@MaxValue", SqlDbType.Int,4),
                    new SqlParameter("@ModifyDate", SqlDbType.DateTime),
					new SqlParameter("@ID", SqlDbType.Int,4)
                    
            };
            parameters[0].Value = model.ContractID;
            parameters[1].Value = model.ParamType;
            parameters[2].Value = model.Field;
            parameters[3].Value = model.FieldType;
            parameters[4].Value = model.Descption;
            parameters[5].Value = model.FieldValue;
            parameters[6].Value = model.Required;
            parameters[7].Value = model.Remark;
            parameters[8].Value = model.SortID;
            parameters[9].Value = model.Creator;
            parameters[10].Value = model.Modifier;
            parameters[11].Value = model.MinValue;
            parameters[12].Value = model.MaxValue;
            parameters[13].Value = model.ModifyDate;
            parameters[14].Value = model.ID;

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
            strSql.Append("delete from ParamInfo ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
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
        /// <summary>
        /// 获得ID
        /// </summary>
        public DataSet GetID(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  max(SortID) as SortID  ");
            strSql.Append(" FROM ParamInfo ");
            if (!strWhere.Equals(""))
            {
                strSql.Append(" where " + strWhere);
            }
   
            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, strSql.ToString());
        }
    }
}

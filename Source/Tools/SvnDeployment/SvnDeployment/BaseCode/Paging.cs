using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using ZyGames.Core.Data;


namespace CitGame
{
    /// <summary>
    /// 分页存储过程调用类
    /// </summary>
    public class Paging
    {
        /// <summary>
        /// 分页存储过程名
        /// </summary>
        private const string CstProc_Paging = "Pg_Paging";

        public const int DoCount = 1;
        public const int DoReader = 0;
        public Paging()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 统计记录数
        /// </summary>
        /// <param name="Tables">表名,多红表是请使用 tA a inner join tB b On a.AID = b.AID</param>
        /// <param name="PK">主键，可以带表头 a.AID</param>
        /// <param name="Filter">Where条件，不带Where，如1=1</param>
        /// <returns></returns>
        //public int Pg_PageCount(SqlConnection aConnection, string Tables, string PK, string Filter)
        //{
        //    if (aConnection == null)
        //    {
        //        throw new Exception();
        //    }
        //    SqlParameter[] paramsCount = new SqlParameter[10];
        //    paramsCount[0] = this.MakeInParam("@Tables", SqlDbType.VarChar, 0, Tables);
        //    paramsCount[1] = this.MakeInParam("@PK", SqlDbType.VarChar, 0, PK);
        //    paramsCount[2] = this.MakeInParam("@Sort", SqlDbType.VarChar, 0, "");
        //    paramsCount[3] = this.MakeInParam("@PageNumber", SqlDbType.Int, 0, 0);
        //    paramsCount[4] = this.MakeInParam("@PageSize", SqlDbType.Int, 0, 0);
        //    paramsCount[5] = this.MakeInParam("@Fields", SqlDbType.VarChar, 0, "");
        //    paramsCount[6] = this.MakeInParam("@Filter", SqlDbType.VarChar, 0, Filter);
        //    paramsCount[7] = this.MakeInParam("@Group", SqlDbType.VarChar, 0, "");
        //    paramsCount[8] = this.MakeInParam("@isCount", SqlDbType.Bit, 0, 1);
        //    paramsCount[9] = this.MakeInParam("@RetFields", SqlDbType.VarChar, 0, "");
        //    Object oRet = this.ExecuteScalar(aConnection, CommandType.StoredProcedure, CstProc_Paging, paramsCount);
        //    return Convert.ToInt32(oRet);
        //}

        /// <summary>
        /// 统计记录数---更改方法
        /// </summary>
        /// <param name="Tables">表名,多红表是请使用 tA a inner join tB b On a.AID = b.AID</param>
        /// <param name="PK">主键，可以带表头 a.AID</param>
        /// <param name="Filter">Where条件，不带Where，如1=1</param>
        /// <returns></returns>
        public int Pg_PageCount(string  aConnection, string Tables, string PK, string Filter)
        {
            if (string.IsNullOrEmpty(aConnection))
            {
                throw new Exception();
            }
            SqlParameter[] paramsCount = new SqlParameter[10];
            paramsCount[0] = SqlParamHelper.MakeInParam("@Tables", SqlDbType.VarChar, 0, Tables);
            paramsCount[1] = SqlParamHelper.MakeInParam("@PK", SqlDbType.VarChar, 0, PK);
            paramsCount[2] = SqlParamHelper.MakeInParam("@Sort", SqlDbType.VarChar, 0, "");
            paramsCount[3] = SqlParamHelper.MakeInParam("@PageNumber", SqlDbType.Int, 0, 0);
            paramsCount[4] = SqlParamHelper.MakeInParam("@PageSize", SqlDbType.Int, 0, 0);
            paramsCount[5] = SqlParamHelper.MakeInParam("@Fields", SqlDbType.VarChar, 0, "");
            paramsCount[6] = SqlParamHelper.MakeInParam("@Filter", SqlDbType.VarChar, 0, Filter);
            paramsCount[7] = SqlParamHelper.MakeInParam("@Group", SqlDbType.VarChar, 0, "");
            paramsCount[8] = SqlParamHelper.MakeInParam("@isCount", SqlDbType.Bit, 0, 1);
            paramsCount[9] = SqlParamHelper.MakeInParam("@RetFields", SqlDbType.VarChar, 0, "");
            Object oRet = SqlHelper.ExecuteScalar(aConnection, CommandType.StoredProcedure, CstProc_Paging, paramsCount);
            return Convert.ToInt32(oRet);
        }

        //public int Pg_PageCount(SqlConnection aConnection, string aProcName, SqlParameter[] aparamsGet)
        //{
        //    if (aConnection == null) { throw new Exception(); }
        //    Object aRet = this.ExecuteScalar(aConnection, CommandType.StoredProcedure, aProcName, aparamsGet);
        //    return Convert.ToInt32(aRet);
           
        //}
        //--修改
        public int Pg_PageCount(string aConnection, string aProcName, SqlParameter[] aparamsGet)
        {
            if (string.IsNullOrEmpty(aConnection)) { throw new Exception(); }
            Object aRet = SqlHelper.ExecuteScalar(aConnection, CommandType.StoredProcedure, aProcName, aparamsGet);
            return Convert.ToInt32(aRet);

        }

        //public SqlDataReader Pg_Paging(SqlConnection aConnection, string aProcName, SqlParameter[] aParamsGet)
        //{
        //    if (aConnection == null) throw new Exception();
        //    return this.ExecuteReader(aConnection, CommandType.StoredProcedure, aProcName, aParamsGet);
        //}
        //-修改
        public SqlDataReader Pg_Paging(string aConnection, string aProcName, SqlParameter[] aParamsGet)
        {
            if (string.IsNullOrEmpty(aConnection)) throw new Exception();
            return SqlHelper.ExecuteReader(aConnection, CommandType.StoredProcedure, aProcName, aParamsGet);
        }

        //public DataSet Pg_PagingDst(SqlConnection aConnection, string aProcName, SqlParameter[] aParamaGet)
        //{
        //    if (aConnection == null) throw new Exception();
        //    return this.ExecuteCSop_DataCompressor(aConnection, CommandType.StoredProcedure, aProcName, aParamaGet);
        //}
        //-修改
        public DataSet Pg_PagingDst(string aConnection, string aProcName, SqlParameter[] aParamaGet)
        {
            if (string.IsNullOrEmpty(aConnection)) throw new Exception();
            return SqlHelper.ExecuteDataset(aConnection, CommandType.StoredProcedure, aProcName, aParamaGet);
        }
        /// <summary>
        /// 返回分页的数据
        /// </summary>
        /// <param name="Tables">表名,多红表是请使用 tA a inner join tB b On a.AID = b.AID</param>
        /// <param name="PK">主键，可以带表头 a.AID</param>
        /// <param name="Sort">排序字段</param>
        /// <param name="PageNumber">开始页码</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="Fields">读取字段</param>
        /// <param name="Filter">过滤条件</param>
        /// <param name="Group">分组</param>
        /// <param name="RetFields">实际返回字段，一般在多表关联查询且非第一页时指定，为空时，则按照Fields字段返回</param>
        /// <returns></returns>
        //public SqlDataReader Pg_Paging(SqlConnection aConnection, string Tables, string PK, string Sort, int PageNumber, int PageSize, string Fields, string Filter, string Group, string RetFields)
        //{
        //    if (aConnection == null)
        //    {
        //        throw new Exception();
        //    }
        //    SqlParameter[] paramsPaging = new SqlParameter[10];
        //    paramsPaging[0] = this.MakeInParam("@Tables", SqlDbType.VarChar, 0, Tables);
        //    paramsPaging[1] = this.MakeInParam("@PK", SqlDbType.VarChar, 0, PK);
        //    paramsPaging[2] = this.MakeInParam("@Sort", SqlDbType.VarChar, 0, Sort);
        //    paramsPaging[3] = this.MakeInParam("@PageNumber", SqlDbType.Int, 0, PageNumber + 1);
        //    paramsPaging[4] = this.MakeInParam("@PageSize", SqlDbType.Int, 0, PageSize);
        //    paramsPaging[5] = this.MakeInParam("@Fields", SqlDbType.VarChar, 0, Fields);
        //    paramsPaging[6] = this.MakeInParam("@Filter", SqlDbType.VarChar, 0, Filter);
        //    paramsPaging[7] = this.MakeInParam("@Group", SqlDbType.VarChar, 0, Group);
        //    paramsPaging[8] = this.MakeInParam("@isCount", SqlDbType.Bit, 0, 0);
        //    paramsPaging[9] = this.MakeInParam("@RetFields", SqlDbType.VarChar, 0, RetFields);
        //    return this.ExecuteReader(aConnection, CommandType.StoredProcedure, CstProc_Paging, paramsPaging);
        //}

        /// <summary>
        /// 返回分页的数据--修改
        /// </summary>
        /// <param name="Tables">表名,多红表是请使用 tA a inner join tB b On a.AID = b.AID</param>
        /// <param name="PK">主键，可以带表头 a.AID</param>
        /// <param name="Sort">排序字段</param>
        /// <param name="PageNumber">开始页码</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="Fields">读取字段</param>
        /// <param name="Filter">过滤条件</param>
        /// <param name="Group">分组</param>
        /// <param name="RetFields">实际返回字段，一般在多表关联查询且非第一页时指定，为空时，则按照Fields字段返回</param>
        /// <returns></returns>
        public SqlDataReader Pg_Paging(string aConnection, string Tables, string PK, string Sort, int PageNumber, int PageSize, string Fields, string Filter, string Group, string RetFields)
        {
            if (string.IsNullOrEmpty(aConnection))
            {
                throw new Exception();
            }
            SqlParameter[] paramsPaging = new SqlParameter[10];
            paramsPaging[0] = SqlParamHelper.MakeInParam("@Tables", SqlDbType.VarChar, 0, Tables);
            paramsPaging[1] = SqlParamHelper.MakeInParam("@PK", SqlDbType.VarChar, 0, PK);
            paramsPaging[2] = SqlParamHelper.MakeInParam("@Sort", SqlDbType.VarChar, 0, Sort);
            paramsPaging[3] = SqlParamHelper.MakeInParam("@PageNumber", SqlDbType.Int, 0, PageNumber + 1);
            paramsPaging[4] = SqlParamHelper.MakeInParam("@PageSize", SqlDbType.Int, 0, PageSize);
            paramsPaging[5] = SqlParamHelper.MakeInParam("@Fields", SqlDbType.VarChar, 0, Fields);
            paramsPaging[6] = SqlParamHelper.MakeInParam("@Filter", SqlDbType.VarChar, 0, Filter);
            paramsPaging[7] = SqlParamHelper.MakeInParam("@Group", SqlDbType.VarChar, 0, Group);
            paramsPaging[8] = SqlParamHelper.MakeInParam("@isCount", SqlDbType.Bit, 0, 0);
            paramsPaging[9] = SqlParamHelper.MakeInParam("@RetFields", SqlDbType.VarChar, 0, RetFields);
            return SqlHelper.ExecuteReader(aConnection, CommandType.StoredProcedure, CstProc_Paging, paramsPaging);
        }

        //public DataSet Pg_PagingDst(SqlConnection aConnection, string Tables, string PK, string Sort, int PageNumber, int PageSize, string Fields, string Filter, string Group, string RetFields)
        //{
        //    if (aConnection == null)
        //    {
        //        throw new Exception();
        //    }
        //    SqlParameter[] paramsPaging = new SqlParameter[10];
        //    paramsPaging[0] = this.MakeInParam("@Tables", SqlDbType.VarChar, 0, Tables);
        //    paramsPaging[1] = this.MakeInParam("@PK", SqlDbType.VarChar, 0, PK);
        //    paramsPaging[2] = this.MakeInParam("@Sort", SqlDbType.VarChar, 0, Sort);
        //    paramsPaging[3] = this.MakeInParam("@PageNumber", SqlDbType.Int, 0, PageNumber + 1);
        //    paramsPaging[4] = this.MakeInParam("@PageSize", SqlDbType.Int, 0, PageSize);
        //    paramsPaging[5] = this.MakeInParam("@Fields", SqlDbType.VarChar, 0, Fields);
        //    paramsPaging[6] = this.MakeInParam("@Filter", SqlDbType.VarChar, 0, Filter);
        //    paramsPaging[7] = this.MakeInParam("@Group", SqlDbType.VarChar, 0, Group);
        //    paramsPaging[8] = this.MakeInParam("@isCount", SqlDbType.Bit, 0, 0);
        //    paramsPaging[9] = this.MakeInParam("@RetFields", SqlDbType.VarChar, 0, RetFields);
        //    return this.ExecuteCSop_DataCompressor(aConnection, CommandType.StoredProcedure, CstProc_Paging, paramsPaging);
        //}
        //--修改
        public DataSet Pg_PagingDst(string aConnection, string Tables, string PK, string Sort, int PageNumber, int PageSize, string Fields, string Filter, string Group, string RetFields)
        {
            if (string.IsNullOrEmpty(aConnection))
            {
                throw new Exception();
            }
            SqlParameter[] paramsPaging = new SqlParameter[10];
            paramsPaging[0] = SqlParamHelper.MakeInParam("@Tables", SqlDbType.VarChar, 0, Tables);
            paramsPaging[1] = SqlParamHelper.MakeInParam("@PK", SqlDbType.VarChar, 0, PK);
            paramsPaging[2] = SqlParamHelper.MakeInParam("@Sort", SqlDbType.VarChar, 0, Sort);
            paramsPaging[3] = SqlParamHelper.MakeInParam("@PageNumber", SqlDbType.Int, 0, PageNumber + 1);
            paramsPaging[4] = SqlParamHelper.MakeInParam("@PageSize", SqlDbType.Int, 0, PageSize);
            paramsPaging[5] = SqlParamHelper.MakeInParam("@Fields", SqlDbType.VarChar, 0, Fields);
            paramsPaging[6] = SqlParamHelper.MakeInParam("@Filter", SqlDbType.VarChar, 0, Filter);
            paramsPaging[7] = SqlParamHelper.MakeInParam("@Group", SqlDbType.VarChar, 0, Group);
            paramsPaging[8] = SqlParamHelper.MakeInParam("@isCount", SqlDbType.Bit, 0, 0);
            paramsPaging[9] = SqlParamHelper.MakeInParam("@RetFields", SqlDbType.VarChar, 0, RetFields);
            return SqlHelper.ExecuteDataset(aConnection, CommandType.StoredProcedure, CstProc_Paging, paramsPaging);
        }
    }
}
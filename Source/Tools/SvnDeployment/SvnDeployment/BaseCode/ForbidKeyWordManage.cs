using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;

namespace CitGame
{
    /// <summary>
    /// ForbidKeyWordManage 的摘要说明
    /// </summary>
    public class ForbidKeyWordManage
    {
        public string[,] stat = new string[,] { { "启用", "1" }, { "关闭", "0" } };
        public ForbidKeyWordManage()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 添加新的关键字
        /// </summary>
        /// <param name="aKeyWord"></param>
        /// <param name="aStat"></param>
        /// <returns></returns>
        public bool AddNewKeyWord(string aKeyWord, int aStat)
        {
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
                string sGetSql = "select * from SnsCenter.dbo.ForbidWordKey where keyword=@aKeyWord";
                SqlParameter[] paramsGet = new SqlParameter[2];
                paramsGet[0] = SqlParamHelper.MakeInParam("@aKeyWord", SqlDbType.VarChar, 0, aKeyWord);
                using (SqlDataReader aReader = SqlHelper.ExecuteReader(con, CommandType.Text, sGetSql, paramsGet))
                {
                    if (aReader == null)
                        return false;

                    if (aReader.HasRows)
                    {
                        return false;
                    }
                }

                string sInsertSql = "insert into SnsCenter.dbo.forbidWordKey(keyWord, Stat)values(@aKeyWord, @aStat)";
                paramsGet[1] = SqlParamHelper.MakeInParam("@aStat", SqlDbType.Int, 0, aStat);
                if (SqlHelper.ExecuteNonQuery(con, CommandType.Text, sInsertSql, paramsGet) == 1)
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
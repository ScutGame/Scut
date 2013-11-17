using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;

/// <summary>
/// 用户活跃流失明细
/// </summary>
public abstract class UserActiveLossDetail : BasePage
{
    protected const string StoredProcedure = "SnsCenter.dbo.UserActiveLossDetail";
    public UserActiveLossDetail()
    {

    }

    protected DataSet GetLevelDS()
    {
        try
        {
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string strSql = "SELECT LevelRangeID,LevelName FROM  SnsCenter.dbo.ConfigLevelRange";
            return SqlHelper.ExecuteDataset(con, CommandType.Text, strSql);
        }
        catch
        {
            return null;
        }
    }

    protected DataSet GetDetailDS(int gameId, bool activeLoss, string levelRangeID, string beginTime, string endTime)
    {
        try
        {
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            SqlParameter[] param = new SqlParameter[5];
            param[0] = SqlParamHelper.MakeInParam("@GameId", SqlDbType.Int, 4, gameId);
            param[1] = SqlParamHelper.MakeInParam("@ActiveLoss", SqlDbType.Bit, 1, activeLoss);
            param[2] = SqlParamHelper.MakeInParam("@LevelRangeID", SqlDbType.VarChar, 5, levelRangeID);
            param[3] = SqlParamHelper.MakeInParam("@BeginTime", SqlDbType.VarChar, 50, beginTime);
            param[4] = SqlParamHelper.MakeInParam("@EndTime", SqlDbType.VarChar, 50, endTime);

            return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, StoredProcedure, param);
        }
        catch
        {
            return null;
        }
    }
}

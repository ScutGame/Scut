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


/// <summary>
/// LogVisit 的摘要说明
/// </summary>
public class LogVisit
{
	public LogVisit()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public static string AddLogToDB(HttpRequest aRequest, int aUserId, string aUserIp)
    {
        try
        {
            string sLogValue = "\r\n";

            sLogValue += "userid=" + aUserId.ToString() + "\r\n";
            //-------------------------------------------
            string aRequestType = aRequest.RequestType.ToLower();
            sLogValue += aRequestType + "\r\n";
            //----------------------------------------
            string sHostUrl = aRequest.Url.Host;
            sLogValue += sHostUrl + "\r\n";
            //-----------------------------------
            string aRawUrl = aRequest.Path;
            sLogValue += aRawUrl + "\r\n";
            //-----------------------------------
            string aReqValue = "";
            if (aRequestType == "get")
            {
                aReqValue = aRequest.QueryString.ToString();
            }
            else
            {
                string[] aFormKeys = aRequest.Form.AllKeys;
                for (int i = 0; i < aFormKeys.Length; i++)
                {
                    aReqValue += aFormKeys[i].ToString() + "=" + aRequest.Form[i].ToString();
                    if (i < aFormKeys.Length - 1)
                    {
                        aReqValue += "&";
                    }
                }
            }
            sLogValue += aReqValue + "\r\n";
            //-------------------
            string sInsertSql = "insert into GameOA.dbo.OA_VisitPageLog(userid, LogUrlHost, LogPath, LogRequestType, LogClientRequest, LogUserIp, LogTime)";
            sInsertSql += "values(@aUserId, @aLogUrlHost, @aLogPath, @aLogRequestType, @aLogClientRequest, @aLogUserId, getdate())";
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
                SqlParameter[] paramsInsert = new SqlParameter[6];
                paramsInsert[0] = SqlParamHelper.MakeInParam("@aUserId", SqlDbType.Int, 0, aUserId);
                paramsInsert[1] = SqlParamHelper.MakeInParam("@aLogUrlHost", SqlDbType.VarChar, 0, sHostUrl);
                paramsInsert[2] = SqlParamHelper.MakeInParam("@aLogPath", SqlDbType.VarChar, 0, aRawUrl);
                paramsInsert[3] = SqlParamHelper.MakeInParam("@aLogRequestType", SqlDbType.VarChar, 0, aRequestType);
                paramsInsert[4] = SqlParamHelper.MakeInParam("@aLogClientRequest", SqlDbType.VarChar, 0, aReqValue);
                paramsInsert[5] = SqlParamHelper.MakeInParam("@aLogUserId", SqlDbType.VarChar, 0, aUserIp);
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, sInsertSql, paramsInsert);
                //   paramsInsert[6] = conndb.MakeInParam("@aLogUrlHost", SqlDbType.VarChar, 0, sHostUrl);

            return sLogValue;
        }
        catch (Exception ex)
        {
            return ex.Message + ex.StackTrace;
        }
        //------------------
        
    }


}

using System;
using System.Data;
using System.Web;
using System.Collections.Generic;
using CitGame;
using System.Data.SqlClient;
using ZyGames.Common;

/// <summary>
/// 游戏OA后台，页面访问基类
/// </summary>
public abstract class BasePage : System.Web.UI.Page
{
    /// <summary>
    /// 返回分页总条目数
    /// </summary>
    public int RecordCount = 0;

    /// <summary>
    /// 登录的用户ID
    /// </summary>
    protected int iUserId;
    private HttpResponse E_Response;
    private HttpRequest E_Request;
    protected BaseLog oBaseLog;

    public BasePage()
    {
        this.LoadBasePage(true);
    }

    /// <summary>
    /// 加载页面并判断传入参数，是否需要检测登录Cookie存在
    /// </summary>
    /// <param name="aCheckLoginCookie">True - 检测Cookie</param>
    public BasePage(bool aCheckLoginCookie)
    {
        this.LoadBasePage(aCheckLoginCookie);
    }
    /// <summary>
    /// 当参数=True时，检测是否有登录Cookie存在
    /// </summary>
    /// <param name="aCheckLoginCookie"></param>
    private void LoadBasePage(bool aCheckLoginCookie)
    {
        this.oBaseLog = new BaseLog();

        E_Response = System.Web.HttpContext.Current.Response;
        E_Request = System.Web.HttpContext.Current.Request;
        iUserId = CstConfig.UserIDErr;

        //BaseCookie oBaseSession = new BaseCookie();
        //if (!oBaseSession.LoadCookie())
        //{
        //    if (aCheckLoginCookie)
        //    {
        //        this.LoginError("您未登录或登录已失效");
        //    }
        //}
        //iUserId = oBaseSession.UserId;
        //string thisPageUrl = E_Request.Url.ToString().Replace(UrlBase, "").ToLower();

        //if (aCheckLoginCookie)
        //{
        //    if (!IsPermitIp(oBaseLog.GetRealIP()))
        //    {
        //        this.LoginError("您的登录Ip不在允许的范围");
        //    }
        //    BaseEmployee oEmployee = null;
        //    CacheEmployee oCacheEmployee = new CacheEmployee();
        //    oEmployee = oCacheEmployee.GetEmployee(this.iUserId);

        //    //CachePurviewPage oCachePurviewPage = new CachePurviewPage();
        //    //List<BasePurviewPage> listPurview = oCachePurviewPage.GetAllPage();

        //    //BasePurviewPage purviewPage = null;
        //    //foreach (BasePurviewPage purview in listPurview)
        //    //{
        //    //    if (purview.PageUrl.ToLower() == thisPageUrl)
        //    //    {
        //    //        purviewPage = purview;
        //    //    }
        //    //}
        //    //if (purviewPage != null)
        //    //{
        //    //    if (!purviewPage.IsSetToGroup(oEmployee.MainGroupId))
        //    //    {
        //    //        this.LoginError("你没有访问该页面的权限");
        //    //    }
        //    //}
        //}
        //oBaseLog.SaveLog(LogVisit.AddLogToDB(E_Request, iUserId, oBaseLog.GetRealIP()));
    }

    protected bool IsAdmin
    {
        get
        {
            try
            {
                int userID = this.iUserId;
                CacheEmployee cacheEmployee = new CacheEmployee();
                BaseEmployee emp = cacheEmployee.GetEmployee(userID);
                if (emp == null)
                {
                    return false;
                }
                //CacheEmployeeGroup cacheEmployeeGroup = new CacheEmployeeGroup();
                //BaseEmployeeGroup baseEmployeeGroup = cacheEmployeeGroup.GetEmployeeGroup(emp.MainGroupId);
                //if (baseEmployeeGroup != null && baseEmployeeGroup.IsSuperMaster)
                //{
                //    return true;
                //}
                return false;
            }
            catch (Exception ex)
            {
                oBaseLog.SaveLog(ex);
                return false;
            }
        }
    }

    /// <summary>
    /// 检查是否在渠道用户组
    /// </summary>
    /// <returns></returns>
    protected bool IsChannel
    {
        get
        {
            try
            {
                int userID = this.iUserId;
                CacheEmployee cacheEmployee = new CacheEmployee();
                BaseEmployee emp = cacheEmployee.GetEmployee(userID);
                if (emp == null)
                {
                    return false;
                }
                //CacheEmployeeGroup cacheEmployeeGroup = new CacheEmployeeGroup();
                //BaseEmployeeGroup baseEmployeeGroup = cacheEmployeeGroup.GetEmployeeGroup(emp.MainGroupId);
                //if (baseEmployeeGroup != null)
                //{
                //    return (GroupType)baseEmployeeGroup.GroupLv == GroupType.Channel;
                //}
                return false;
            }
            catch (Exception ex)
            {
                oBaseLog.SaveLog(ex);
                return false;
            }
        }
    }

    /// <summary>
    /// 是否是允许的Ip(从192.168.1.101中取192.168)
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    protected bool IsPermitIp(string ip)
    {
        bool isPermit = true;
        CacheLimitIp oCacheLimitIp = new CacheLimitIp();
        List<string> PermitIps = oCacheLimitIp.getPermitLists();
        if (PermitIps.Count == 0)
        {
            return isPermit;
        }
        else
        {
            string temp = ip.Substring(0, ip.IndexOf('.'));
            ip = ip.Substring(ip.IndexOf('.') + 1);
            temp += "." + ip.Substring(0, ip.IndexOf('.'));

            isPermit = PermitIps.Contains(temp);
            return isPermit;
        }
    }

    protected bool ReadGetDateTime(string aKey, ref DateTime aDtValue)
    {
        try
        {
            aDtValue = Convert.ToDateTime(E_Request.QueryString[aKey]);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected bool ReadGetString(string aKey, ref string aStrValue)
    {
        try
        {
            aStrValue = E_Request.QueryString[aKey];
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected bool ReadGetInt(string aKey, ref int aIntValue)
    {
        try
        {
            string aTmp = E_Request.QueryString[aKey];
            return int.TryParse(aTmp, out aIntValue);
        }
        catch
        {
            return false;
        }
    }

    protected bool ReadPostDateTime(string aKey, ref DateTime aDtValue)
    {
        try
        {
            aDtValue = Convert.ToDateTime(E_Request.Form[aKey]);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected bool ReadPostString(string aKey, ref string aStrValue)
    {
        try
        {
            aStrValue = E_Request.Form[aKey];
            if (aStrValue == null)
                return false;
            else
                return true;
        }
        catch
        {
            return false;
        }
    }
    protected bool ReadPostInt(string akey, ref int aIntValue)
    {
        try
        {
            aIntValue = ConvertHelper.ToInt(E_Request.Form[akey]);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 跳转到错误页面
    /// </summary>
    /// <param name="aErrorDesc"></param>
    protected void PageError(string aErrorDesc)
    {
        this.PageError(aErrorDesc, E_Request.Url.ToString());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="aErrDesc"></param>
    /// <param name="aBackUrl">指定跳转的页面</param>
    protected void PageError(string aErrorDesc, string aBackUrl)
    {
        string tmpurl = "cont=" + Server.UrlEncode(aErrorDesc) + "&rdi=" + aBackUrl;
        Uri aUri = E_Request.Url;
        string sPath = E_Request.ApplicationPath;
        string sUrl = "http://" + aUri.Authority + "" + sPath + "" + "ErrorHtml.aspx?" + tmpurl;
        E_Response.Redirect(sUrl, true);
    }

    /// <summary>
    /// 获取服务器名及应用程序路径
    /// </summary>
    private static string UrlSuffix
    {
        get
        {
            string port = HttpContext.Current.Request.Url.Port.ToString();
            if (port != "80" || port != "")
                port = ":" + port;
            return HttpContext.Current.Request.Url.Host + port + HttpContext.Current.Request.ApplicationPath;
        }
    }

    /// <summary>
    /// 获取URLs
    /// </summary>
    public static String UrlBase
    {
        get
        {
            return @"http://" + UrlSuffix;
        }
    }

    protected virtual void LoginError(string aStrErr)
    {
        E_Response.Write("<script language='javascript'>alert('" + aStrErr + "');</script>");
        E_Response.Write("<script language='javascript'>window.top.location.replace('" + UrlBase + "login.aspx');</script>");
        E_Response.End();
    }

    protected int PageIndex;
    protected int PageCount = 50;

    protected virtual void BindData()
    {
    }

    public DataSet GetList(int PageSize, int PageIndex, string strWhere, out int totalCount, string connString, string tblName, string fldName, string procedureName)
    {
        SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@TotalPages", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
        parameters[0].Value = tblName;
        parameters[1].Value = fldName;
        parameters[2].Value = PageSize;
        parameters[3].Value = PageIndex;
        parameters[5].Value = 1;
        parameters[6].Value = strWhere;
        parameters[4].Direction = ParameterDirection.Output;
        DataSet ds = RunProcedure(connString, procedureName, parameters, "ds");
        totalCount = (int)parameters[4].Value;
        return ds;
    }


    public static DataSet RunProcedure(string connString, string storedProcName, IDataParameter[] parameters, string tableName)
    {
        using (SqlConnection connection = new SqlConnection(connString))
        {
            DataSet dataSet = new DataSet();
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
            //adapter.SelectCommand.CommandTimeout = Times;
            adapter.Fill(dataSet, tableName);
            connection.Close();
            return dataSet;
        }
    }

    private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
    {
        SqlCommand command = new SqlCommand(storedProcName, connection);
        command.CommandType = CommandType.StoredProcedure;
        foreach (SqlParameter parameter in parameters)
        {
            if (parameter != null)
            {
                if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }
                command.Parameters.Add(parameter);
            }
        }
        return command;
    }
}

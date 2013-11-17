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

/// <summary>
/// BaseSession 的摘要说明
/// </summary>
public class BaseCookie:System.Web.UI.Page
{
    private HttpResponse response;
    private HttpRequest request;
    private int _UserId;
    public int UserId { get { return _UserId; } }
    public BaseCookie()
    {
        response = System.Web.HttpContext.Current.Response;
        request = System.Web.HttpContext.Current.Request;
    }

    public void SaveCookie(SqlDataReader aReader)
    {
        SaveCookie(aReader, 60);
    }
    public void SaveCookie(SqlDataReader aReader, double minutes)
    {
        _UserId = Convert.ToInt32(aReader["userid"]);
        HttpCookie oCookie = new HttpCookie("userid", _UserId.ToString());
        oCookie.Expires = DateTime.Now.AddMinutes(minutes);
        response.Cookies.Add(oCookie);
    }
    public void CleanCookie()
    {
        if (request.Cookies["userid"] != null)
        {
            response.Cookies["userid"].Expires = DateTime.Now.AddDays(-1);    
        }
    }
    public bool LoadCookie()
    {
        bool iIsLogin = false;
        if (request.Cookies["userid"] != null)
        {
            string userid = request.Cookies["userid"].Value;
            if (int.TryParse(userid, out _UserId))
            {
                iIsLogin = true;
            }
        }
        else
        {
            _UserId = CitGame.CstConfig.UserIDErr;
        }
        return iIsLogin;
    }
}

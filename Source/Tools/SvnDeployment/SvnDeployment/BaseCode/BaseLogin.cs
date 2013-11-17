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
using System.Security.Cryptography;
using ZyGames.Core.Data;
using ZyGames.Core.Util;
using ZyGames.SimpleManager.Service.Common;
using ZyGames.Common;


/// <summary>
/// BaseLogin 的摘要说明
/// </summary>
public class BaseLogin : BasePage
{
    public BaseLogin()
        : base(false)
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    protected override void BindData()
    {
        throw new Exception("The method or operation is not implemented.");
    }
    /// <summary>
    /// 更换OA密码周期30天
    /// </summary>
    private static int oa_changepwd_day = ConvertHelper.ToInt(ConfigHelper.GetSetting("oa_changepwd_day"));
        
    /// <summary>
    /// 检测用户输入的登录用户名与密码是否一直
    /// </summary>
    /// <param name="aUserName"></param>
    /// <param name="aPwd"></param>
    /// <param name="isExpire">过期</param>
    /// <returns></returns>
    protected bool CheckUserLogin(string aUserName, string aPwd, out  bool isExpire)
    {
        isExpire = false;
        string aTmpPwdMd5 = this.DoMd5(aPwd);
        try
        {
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string sSqlExist = "select * from GameOA.dbo.OA_User where UserName=@aUserName";
            SqlParameter[] paramsLogin = new SqlParameter[1];
            paramsLogin[0] = SqlParamHelper.MakeInParam("@aUserName", SqlDbType.VarChar, 0, aUserName);
            using (SqlDataReader loginReader = SqlHelper.ExecuteReader(con, CommandType.Text, sSqlExist, paramsLogin))
            {
                if (loginReader == null)
                {
                    return false;
                }
                else
                {
                    if (loginReader.HasRows && loginReader.Read())
                    {
                        DateTime expireDate = ConvertHelper.ToDateTime(loginReader["pwdmodifydate"]);
                        if (expireDate.AddDays(oa_changepwd_day) < DateTime.Now)//30天过期
                        {
                            isExpire = true;
                        }
                        if (Convert.ToString(loginReader["UserPassword"]) == aTmpPwdMd5)
                        {
                            BaseCookie oBaseSession = new BaseCookie();
                            oBaseSession.SaveCookie(loginReader, 4 * 60);

                            string sql = @"INSERT INTO [GameOA].[dbo].[LoginLog]([LoginIP],[LoginID],[LoginTime])VALUES(@LoginIP,@LoginID,getdate())";
                            string LoginIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                            SqlParameter[] paramsList = new SqlParameter[]{
                                    SqlParamHelper.MakeInParam("@LoginIP", SqlDbType.VarChar, 0, LoginIP),
                                    SqlParamHelper.MakeInParam("@LoginID", SqlDbType.VarChar, 20, aUserName)
                                };
                            SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paramsList);

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

        }
        catch
        {
            return false;
        }
    }

    protected void DoLogOut()
    {
        BaseCookie oBaseCookie = new BaseCookie();
        oBaseCookie.CleanCookie();
    }

    /// <summary>
    /// 生成输入字符串的MD5值
    /// </summary>
    /// <param name="aSource">字符串</param>
    /// <returns>MD5值</returns>
    protected string DoMd5(string aSource)
    {
        string ret = "";
        MD5 md5 = MD5.Create();
        byte[] bs = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(aSource));
        for (int i = 0; i < bs.Length; i++)
        {
            ret += bs[i].ToString("X2");
        }

        md5.Clear();
        return ret.ToLower();
    }
    /// <summary>
    /// 跳转到错误页面
    /// </summary>
    /// <param name="aErrorDesc"></param>
    protected new void PageError(string aErrorDesc)
    {
        //string tmpurl = "cont=" + Server.UrlEncode(aErrorDesc) + "&rdi=" + Request.Url.ToString();
        //Response.Redirect("ErrorHtml.aspx?" + tmpurl, true);
        base.PageError(aErrorDesc, Request.Url.ToString());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="aErrDesc"></param>
    /// <param name="aBackUrl">指定跳转的页面</param>
    protected new void PageError(string aErrorDesc, string aBackUrl)
    {
        //string tmpurl = "cont=" + Server.UrlEncode(aErrorDesc) + "&rdi=" + aBackUrl;
        //Response.Redirect("ErrorHtml.aspx?" + tmpurl, true);
        base.PageError(aErrorDesc, aBackUrl);
    }
}

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
/// UserDelete 的摘要说明
/// </summary>
public class UserDelete
{
    public enum EMark
    {
        Close = 0,
        Open 
    }
    private string _deleteMobilePhone = "";
    private short _mark;
    private int _PhoneId;

    private string _sErrorMsg = "";

    public string ErrorMsg { get { return _sErrorMsg; } }
    public string MobilePhone { get { return _deleteMobilePhone; } }
    public short Mark { get { return _mark; } }
    public int PhoneId { get { return _PhoneId; } }

    public UserDelete()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        
        
	}

    public void InitData(SqlDataReader aReader)
    {
        if (aReader == null) throw new Exception();
        this._deleteMobilePhone = Convert.ToString(aReader["mobilephone"]);
        this._mark = Convert.ToInt16(aReader["mark"]);
        this._PhoneId = Convert.ToInt32(aReader["phoneid"]);
    }

    public bool IsExistUser()
    {
        return this.CheckPhone(this._deleteMobilePhone, false);
    }

    private bool CheckPhone(string aCheckPhone, bool isNew)
    {
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string sGetSql = "select * from mobilegame.dbo.userbasicinfo where mobilephone=@amobilephone";
            SqlParameter[] paramsGet = new SqlParameter[1];
            paramsGet[0] = SqlParamHelper.MakeInParam("@amobilephone", SqlDbType.VarChar, 0, aCheckPhone);
            using (SqlDataReader aReader = SqlHelper.ExecuteReader(con, CommandType.Text, sGetSql, paramsGet))
            {
                if (aReader == null)
                {
                    this._sErrorMsg = "查询号码" + aCheckPhone + "是否已注册出现异常";
                    return false;
                }
                else
                {
                    if (isNew)
                    {
                        if (aReader.HasRows)
                        {
                            this._sErrorMsg = "号码：" + aCheckPhone + "已注册";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (aReader.HasRows)
                        {
                            //aReader.Read();
                            //_UserId = Convert.ToInt32(aReader["userid"]);
                            return true;
                        }
                        else
                        {
                            this._sErrorMsg = "所要删除手机号码"+aCheckPhone+"未注册";
                            return false;
                        }
                    }
                }
            }

    }

    public bool NewPhoneIsReg(string sExistPhone)
    {
        return CheckPhone(sExistPhone, true);
    }

    public void DoDelete(string aNewPhoneNum)
    {
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string sUpdateSql = "update mobilegame.dbo.userbasicinfo set mobilephone=@aNewPhone where mobilephone=@amobilephone";
            SqlParameter[] paramsupdate = new SqlParameter[2];
            paramsupdate[0] = SqlParamHelper.MakeInParam("@aNewPhone", SqlDbType.VarChar, 0, aNewPhoneNum);
            paramsupdate[1] = SqlParamHelper.MakeInParam("@amobilephone", SqlDbType.VarChar, 0, this._deleteMobilePhone);
            if (SqlHelper.ExecuteNonQuery(con, CommandType.Text, sUpdateSql, paramsupdate) == 1)
            {
                this._sErrorMsg = "帐号删除成功";
            }
            else
            {
                this._sErrorMsg = "帐号删除失败";
            }

    }
}

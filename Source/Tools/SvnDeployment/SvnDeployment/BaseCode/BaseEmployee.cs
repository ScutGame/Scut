using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// 员工的几类
/// </summary>
public class BaseEmployee
{
    private int _UserId;
    private string _UserName;
    private int _MainGroupId;
    private string _Email;
    private string _UserCName;
    private string _UserInfo;

    public string UserInfo { get { return _UserInfo; } }
    public string Email { get { return _Email; } }
    public int UserId { get { return _UserId; } }
    public string UserName { get { return _UserName; } }
    public string UserCName { get { return _UserCName; } }
    public int MainGroupId { get { return _MainGroupId; } }

	public BaseEmployee(int aUserId)
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        this._UserId = aUserId;
	}

    public void InitData(SqlDataReader aReader)
    {
        _UserName = Convert.ToString(aReader["userName"]);
        _MainGroupId = Convert.ToInt32(aReader["userGroupid"]);
        _Email = Convert.ToString(aReader["useremail"]);
        _UserInfo = Convert.ToString(aReader["userinfo"]);
        _UserCName = Convert.ToString(aReader["userCName"]);
    }
}

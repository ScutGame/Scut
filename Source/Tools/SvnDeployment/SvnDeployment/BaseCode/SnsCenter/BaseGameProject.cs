using System;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// BaseGameProject 的摘要说明
/// </summary>
public class BaseGameProject
{
    private short _GameId;
    private string _GameName;

    public short GameId { get { return _GameId; } }
    public string GameName{ get { return _GameName; } }
	public BaseGameProject()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public void InitData(SqlDataReader aReader)
    {
        _GameId = Convert.ToInt16(aReader["GameId"]);
        _GameName = Convert.ToString(aReader["GameName"]);
    }
}

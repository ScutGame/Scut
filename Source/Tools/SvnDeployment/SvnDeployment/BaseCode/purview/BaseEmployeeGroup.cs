using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;

/// <summary>
/// 员工组定义类
/// </summary>
public class BaseEmployeeGroup
{
    private int _GroupId;
    private string _GroupName;
    private int _GroupLv;
    private bool _IsSuperMaster;
    private bool _GroupStat;
    private string _GameList;
    /// <summary>
    /// 该员工组所拥有的页面权限ID
    /// </summary>
    private List<int> _ListPageId;
	public BaseEmployeeGroup()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        _ListPageId = new List<int>();
	}
    public int GroupId { get { return _GroupId; } }
    public List<int> HasPurviewPageId { get { return _ListPageId; } }
    public string GroupName { get { return _GroupName; } }
    public bool IsSuperMaster { get { return _IsSuperMaster; } }
    public int GroupLv { get { return _GroupLv; } }
    public bool GroupStat { get { return _GroupStat; } }
    public string GameList { get { return _GameList; } }

    public void InitData(SqlDataReader aReader)
    {
        _GroupId = Convert.ToInt32(aReader["groupid"]);
        _GroupLv = Convert.ToInt32(aReader["grouplv"]);
        _GroupName = Convert.ToString(aReader["groupname"]);
        _IsSuperMaster = Convert.ToBoolean(aReader["IsSuperMaster"]);
        _GroupStat = Convert.ToBoolean(aReader["GroupStat"]);
        _GameList = Convert.ToString(aReader["GameList"]);
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string sGetSql = "select * from GameOA.dbo.oa_GroupPurview where GroupId=@aGetGroupid";
            SqlParameter[] paramsGet = new SqlParameter[1];
            paramsGet[0] = SqlParamHelper.MakeInParam("@aGetGroupId", SqlDbType.Int, 0, _GroupId);
            using (SqlDataReader oReader = SqlHelper.ExecuteReader(con, CommandType.Text, sGetSql, paramsGet))
            {
                if (oReader == null) throw new Exception();
                if (oReader.HasRows)
                {
                    while (oReader.Read())
                    {
                        _ListPageId.Add(Convert.ToInt32(oReader["pageid"]));
                    }
                }
            }

    }
}

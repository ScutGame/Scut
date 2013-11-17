using System;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// BasePurviewPage 的摘要说明
/// </summary>
public class BasePurviewPage
{
    private int _PageId;
    private string _PageTitle;
    private bool _DisplayLeft;
    private string _PageDesc;
    private int _fatherid;
    private int _menuid;
    private string _pageUrl;

    public int Pageid { get { return _PageId; } }
    public string PageTitle { get { return _PageTitle; } }
    public bool DisplayLeft { get { return _DisplayLeft; } }
    public int FatherId { get { return _fatherid; } }
    public int MenuId { get { return _menuid; } }
    public string PageUrl { get { return _pageUrl; } }

	public BasePurviewPage()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public void InitData(SqlDataReader aReader)
    {
        _PageId = Convert.ToInt32(aReader["pageid"]);
        _DisplayLeft = Convert.ToBoolean(aReader["DisplayLeft"]);
        _fatherid = Convert.ToInt32(aReader["FatherPid"]);
        _menuid = Convert.ToInt32(aReader["menuid"]);
        _PageDesc = Convert.ToString(aReader["pageDesc"]);
        _PageTitle = Convert.ToString(aReader["pagetitle"]);
        _pageUrl = Convert.ToString(aReader["pageUrl"]);
    }

    public bool IsSetToGroup(int aGroupid)
    {
        CacheEmployeeGroup oCacheEmployeeGroup = new CacheEmployeeGroup();
        BaseEmployeeGroup oBaseGroup = oCacheEmployeeGroup.GetEmployeeGroup(aGroupid);
        bool IsSetTo = false;
        for (int i = 0; i < oBaseGroup.HasPurviewPageId.Count; i++)
        {
            if (oBaseGroup.HasPurviewPageId[i] == this._PageId)
            {
                IsSetTo = true;
                break;
            }

        }
        return IsSetTo;
    }
}

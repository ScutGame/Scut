using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;


/// <summary>
/// 左边栏功能大菜单
/// </summary>
public class BaseLeftMenu
{
    private string _menuName;
    private int _menuId;
    private string _menuDesc;
    private int _fatherid;
    private List<BasePurviewPage> _listPurviewPage;
    private List<BaseLeftMenu> _childPages;

    public int MenuId { get { return _menuId; } set { _menuId = value; } }
    public string MenuName { get { return _menuName; } }
    public string MenuDesc { get { return _menuDesc; } }
    public int Fatherid { get { return _fatherid; } }

    public List<BasePurviewPage> PurviewPages { get { return _listPurviewPage; } }
    public List<BaseLeftMenu> ChildPages
    {
        get
        {
            if (_childPages == null)
                _childPages = new List<BaseLeftMenu>();
            return _childPages;
        }
    }
	
    public BaseLeftMenu()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public void InitData(SqlDataReader aReader)
    {
        this._menuDesc = Convert.ToString(aReader["menuDesc"]);
        this._menuId = Convert.ToInt32(aReader["menuid"]);
        this._menuName = Convert.ToString(aReader["menuname"]);
        this._fatherid = Convert.ToInt32(aReader["fatherid"]);
    }

    /// <summary>
    /// 重新获取该栏目下的所有页面
    /// </summary>
    public void GetPurviewPages()
    {
        CachePurviewPage oCachePurviewPage = new CachePurviewPage();
        _listPurviewPage = oCachePurviewPage.GetMenuPage(this._menuId);
    }

    public bool HasPurview(int aGroupid)
    {
        CacheEmployeeGroup oCacheEmployeeGroup = new CacheEmployeeGroup();
        BaseEmployeeGroup oGroup = oCacheEmployeeGroup.GetEmployeeGroup(aGroupid);
        for (int i = 0; i < _listPurviewPage.Count; i++)
        {
            for (int k = 0; k < oGroup.HasPurviewPageId.Count; k++)
            {
                if (_listPurviewPage[i].Pageid == oGroup.HasPurviewPageId[k])
                {
                    return true;
                }
            }
        }
        foreach (BaseLeftMenu child in ChildPages)
        {
            for (int i = 0; i < child.PurviewPages.Count; i++)
            {
                for (int k = 0; k < oGroup.HasPurviewPageId.Count; k++)
                {
                    if (child.PurviewPages[i].Pageid == oGroup.HasPurviewPageId[k])
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using CitGame;
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;

/// <summary>
/// CacheLeftMenu 的摘要说明
/// </summary>
public class CacheLeftMenu:BaseCache
{
    private const string cstCacheKey = "LOACacheLeftMenu";
	public CacheLeftMenu():base(cstCacheKey)
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        this.cachekey = cstCacheKey;
	}

    public List<BaseLeftMenu> getLeftMenu()
    {
        return (List<BaseLeftMenu>)this.getCache();
    }

    public BaseLeftMenu getLeftMenuById(int aMenuId)
    {
        List<BaseLeftMenu> listMenus = this.getLeftMenu();
        for (int i = 0; i < listMenus.Count; i++)
        {
            if (listMenus[i].MenuId == aMenuId)
            {
                return listMenus[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 获取最大栏目id
    /// </summary>
    /// <returns></returns>
    public int GetMaxMenuid()
    {
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
        string sGetSql = "Select max(menuid) from GameOA.dbo.oa_leftmenu ";
        object tobj = SqlHelper.ExecuteScalar(con, CommandType.Text, sGetSql);
        return Convert.ToInt32(tobj) + 1;
    }

    /// <summary>
    /// 获取最大栏目序号
    /// </summary>
    /// <returns></returns>
    public int GetMaxDisplayIndex()
    {
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
        string sGetSql = "Select max(DisplayIndex) from GameOA.dbo.oa_leftmenu ";
        object tobj = SqlHelper.ExecuteScalar(con, CommandType.Text, sGetSql);
        return Convert.ToInt32(tobj) + 1;
    }
    protected override bool InitCache()
    {
        try
        {
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string sGetSql = "select [menuid],[menuname],[menuDesc],[fatherid],[DisplayIndex] from GameOA.dbo.oa_leftmenu where fatherid = -1 order by DisplayIndex";
                using (SqlDataReader oReader = SqlHelper.ExecuteReader(con, CommandType.Text, sGetSql))
                {
                    if (oReader == null)
                    {
                        throw new Exception();
                    }
                    List<BaseLeftMenu> tmpList = new List<BaseLeftMenu>();
                    if (oReader.HasRows)
                    {
                        while (oReader.Read())
                        {
                            BaseLeftMenu oTmp = new BaseLeftMenu();
                            oTmp.InitData(oReader);
                            oTmp.GetPurviewPages();


                            sGetSql = "select [menuid],[menuname],[menuDesc],[fatherid],[DisplayIndex] from GameOA.dbo.oa_leftmenu where fatherid =" + oTmp.MenuId;

                            using (SqlDataReader childReader = SqlHelper.ExecuteReader(con, CommandType.Text, sGetSql))
                                {
                                    if (childReader == null)
                                    {
                                        throw new Exception();
                                    }
                                    if (childReader.HasRows)
                                    {
                                        while (childReader.Read())
                                        {
                                            BaseLeftMenu childMenu = new BaseLeftMenu();
                                            childMenu.InitData(childReader);
                                            childMenu.GetPurviewPages();
                                            tmpList.Add(childMenu);
                                            oTmp.ChildPages.Add(childMenu);
                                        }
                                    }
                                }


                            tmpList.Add(oTmp);
                        }
                    }
                    this.addCache(tmpList);
                    return true;
                }
        }
        catch (Exception ex)
        {
            this.SaveLog(ex);
            return false;
        }
    }
}

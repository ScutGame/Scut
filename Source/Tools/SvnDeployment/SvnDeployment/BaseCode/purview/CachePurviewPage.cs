using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using CitGame;
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;
/// <summary>
/// CachePuviewPage 的摘要说明
/// </summary>
public class CachePurviewPage:BaseCache
{
    private const string cstCacheKey = "LOACachePurviewPage";
	public CachePurviewPage():base(cstCacheKey)
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        this.cachekey = cstCacheKey;
	}
    public List<BasePurviewPage> GetMenuPage(int aMenuid)
    {
        List<BasePurviewPage> listPurviewPages = (List<BasePurviewPage>)this.getCache();
        List<BasePurviewPage> listRet = new List<BasePurviewPage>();
        if (listPurviewPages != null)
        {
            for (int i = 0; i < listPurviewPages.Count; i++)
            {
                if (listPurviewPages[i].MenuId == aMenuid && listPurviewPages[i].DisplayLeft)
                {
                    listRet.Add(listPurviewPages[i]);
                }
            }
        }
        return listRet;
    }

    public List<BasePurviewPage> GetAllPage()
    {
        List<BasePurviewPage> listPurviewPages = (List<BasePurviewPage>)this.getCache();
        return listPurviewPages;
    }

    public int GetMaxPageid()
    {
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string sGetSql = "select max(pageid) from GameOA.dbo.oa_purviewpage ";
            object tobj = SqlHelper.ExecuteScalar(con, CommandType.Text, sGetSql);
            return Convert.ToInt32(tobj) + 1;

    }
    protected override bool InitCache()
    {
        try
        {
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string sGetSql = "select [PageID],[PageTitle],[PageUrl],[DisplayLeft],[PageDesc],[FatherPid],[LastModifyTime],[menuID] from GameOA.dbo.oa_purviewpage";
                using (SqlDataReader oReader = SqlHelper.ExecuteReader(con, CommandType.Text, sGetSql))
                {
                    if (oReader == null)
                    {
                        throw new Exception();
                    }
                    List<BasePurviewPage> tmpList = new List<BasePurviewPage>();
                    if (oReader.HasRows)
                    {
                        while (oReader.Read())
                        {
                            BasePurviewPage oTmp = new BasePurviewPage();
                            oTmp.InitData(oReader);
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

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using CitGame;
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;
/// <summary>
/// 员工组缓存类
/// </summary>
public class CacheEmployeeGroup:BaseCache
{
    private const string cstCacheKey = "LOACacheEmployeeGroup";
	public CacheEmployeeGroup():base(cstCacheKey)
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        this.cachekey = cstCacheKey;
	}

    public BaseEmployeeGroup GetEmployeeGroup(int aGroupId)
    {
        List<BaseEmployeeGroup> listGroup = (List<BaseEmployeeGroup>)this.getCache();
        if (listGroup == null) throw new Exception();
        if (listGroup.Count == 0) throw new Exception();

        for (int i = 0; i < listGroup.Count; i++)
        {
            if (listGroup[i].GroupId == aGroupId)
            {
                return listGroup[i];
            }
        }
        throw new Exception();
    }

    public List<BaseEmployeeGroup> getLists()
    {
        return (List<BaseEmployeeGroup>)this.getCache();
    }

    /// <summary>
    /// 获取最大Id
    /// </summary>
    /// <returns></returns>
    public int GetMaxId()
    {
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string sGetSql = "select max(groupid) from GameOA.dbo.oa_UserGroup ";
            object tobj = SqlHelper.ExecuteScalar(con, CommandType.Text, sGetSql);
            return Convert.ToInt32(tobj) + 1;

    }

    protected override bool InitCache()
    {
        try
        {
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
                string sGetSql = "select * from GameOA.dbo.oa_usergroup";
                using (SqlDataReader rReader = SqlHelper.ExecuteReader(con, CommandType.Text, sGetSql))
                {
                    if (rReader == null) throw new Exception();
                    List<BaseEmployeeGroup> tmpList = new List<BaseEmployeeGroup>();
                    if (rReader.HasRows)
                    {
                        while (rReader.Read())
                        {
                            BaseEmployeeGroup oTmp = new BaseEmployeeGroup();
                            oTmp.InitData(rReader);
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

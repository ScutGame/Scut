using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using CitGame;
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;

/// <summary>
/// CacheGameProject 的摘要说明
/// </summary>
public class CacheGameProject:BaseCache
{
    private const string cacheKey = "LCacheOaGameProject";
	public CacheGameProject():base(cacheKey)
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        this.cachekey = cacheKey;
	}

    public List<BaseGameProject> getList()
    {
        return (List<BaseGameProject>)this.getCache();
    }

    protected override bool InitCache()
    {
        try
        {
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
                string sSql = "select * from SnsCenter.dbo.ConfigGameProject order by gameid";
                using (SqlDataReader reader = SqlHelper.ExecuteReader(con, CommandType.Text, sSql))
                {
                    if (reader == null) throw new Exception();
                    if (reader.HasRows)
                    {
                        List<BaseGameProject> tmplist = new List<BaseGameProject>();
                        while (reader.Read())
                        {
                            BaseGameProject oTmpBaseAsp = new BaseGameProject();
                            oTmpBaseAsp.InitData(reader);
                            tmplist.Add(oTmpBaseAsp);
                        }
                        this.addCache(tmplist);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

        }
        catch (Exception ex)
        {
            this.SaveLog(ex);
            return false;
        }
    }


}

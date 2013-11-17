using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using ZyGames.GameCache;
using System.Data.SqlClient;
using ZyGames.Core.Data;
using CitGame;
using ZyGames.SimpleManager.Service.Common;
/// <summary>
/// CacheConfig 的摘要说明
/// </summary>
public class CacheConfig : CitGame.BaseCache
{
    private const string cacheKey = "LCacheOaConfig";
    public CacheConfig()
        : base(cacheKey)
    {
        this.cachekey = cacheKey;
    }


    public Dictionary<string, string> getList()
    {
        return (Dictionary<string, string>)this.getCache();
    }

    protected override bool InitCache()
    {
        try
        {
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
                string sSql = "select * from GameOA.dbo.OA_Config order by Id";
                using (SqlDataReader reader = SqlHelper.ExecuteReader(con, CommandType.Text, sSql))
                {
                    if (reader == null) throw new Exception();
                    if (reader.HasRows)
                    {
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        while (reader.Read())
                        {
                            dict.Add(reader["Name"].ToString(), reader["Value"].ToString());
                        }
                        this.addCache(dict);
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


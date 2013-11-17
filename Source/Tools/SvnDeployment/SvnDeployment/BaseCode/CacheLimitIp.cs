using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;

namespace CitGame
{
    /// <summary>
    /// 允许访问的IP地址列表缓存
    /// </summary>
    public class CacheLimitIp:BaseCache
    {
        private const string cstCacheKey = "LOACacheLimitIP";
        public CacheLimitIp():base(cstCacheKey)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            this.cachekey = cstCacheKey;
        }

        public List<string> getPermitLists()
        {
            return (List<string>)this.getCache();
        }

        protected override bool InitCache()
        {
            try
            {
                string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
                    string sGetSql = "select * from GameOA.dbo.oa_PermitIp where PermitStat=1";
                    using (SqlDataReader aReader = SqlHelper.ExecuteReader(con, CommandType.Text, sGetSql))
                    {
                        if (aReader == null) throw new Exception();
                        List<string> permitIps = new List<string>();
                        if (aReader.HasRows)
                        {
                            while (aReader.Read())
                            {
                                permitIps.Add(aReader["PermitIp"].ToString());
                            }
                        }
                        this.addCache(permitIps);
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
}
using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Caching;
namespace CitGame
{
    /// <summary>
    /// BaseCache 的摘要说明
    /// </summary>
    public abstract class BaseCache:BaseLog
    {
        protected const int CstHalfHour = 30;
        protected const int CstOneDay = 24 * 60;
        protected int iCacheTimeMinute = 0;
        /// <summary>
        /// 缓存对象名称
        /// </summary>
        protected string cachekey;
        private Cache cache;
        public BaseCache(string _logFolder):base(_logFolder)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            this.cache = HttpContext.Current.Cache;
            iCacheTimeMinute = CstHalfHour;
        }

        protected Object getCache()
        {
            Object tmpList =  (Object)this.cache.Get(this.cachekey);
            if (tmpList == null)
            {
                if (this.InitCache())
                {
                    tmpList = (Object)this.cache.Get(this.cachekey);
                }
                else
                {
                    tmpList = null;
                }
            }
            return tmpList;
        }

        protected void addCache(Object aCacheValue)
        {
            this.cache.Add(this.cachekey, aCacheValue, null, DateTime.Now.AddSeconds(iCacheTimeMinute * 60 * 1000), TimeSpan.Zero, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// 重新加载配置数据
        /// </summary>
        /// <returns></returns>
        public bool ReLoad()
        {
            bool bRemove = true;
            try
            {
                this.cache.Remove(this.cachekey);                
            }
            catch
            {
                return false;
            }

            if (bRemove)
            {
                return this.InitCache();
            }
            else
            {
                return bRemove;
            }
        }

        /// <summary>
        /// 重载，初始化缓存方法
        /// </summary>
        /// <returns></returns>
        protected abstract bool InitCache();
    }
}
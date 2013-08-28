using System;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Framework.Game.Cache
{
    /// <summary>
    /// 配置库缓存集（只读）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigCacheSet<T> : ShareCacheStruct<T> where T : ShareEntity, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public ConfigCacheSet()
        {
        }
    }
}
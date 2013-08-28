using System;
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;
using ZyGames.Framework.Cache;
using ZyGames.Framework.Net;

namespace ZyGames.Framework.Game.Cache
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameDataCacheSet<T> : PersonalCacheStruct<T> where T : BaseEntity, new()
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;
using GameServer.Model;

namespace Game.Script
{
public class LuaFuncProxy
{
    private static LuaFuncProxy instance = new LuaFuncProxy();

    public static LuaFuncProxy GetIntance()
    {
        return instance;
    }

    private LuaFuncProxy()
    {

    }

    /// <summary>
    /// 获取Url参数
    /// </summary>
    /// <param name="actionGetter"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public string GetActionParam(ActionGetter actionGetter, string name)
    {
        return actionGetter != null ? actionGetter.GetString(name) : "";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public UserRanking[] GetUserRankingList()
    {
        var cache = new ShareCacheStruct<UserRanking>();
        var rankingList = cache.FindAll(false);
        //rankingList = MathUtils.QuickSort<UserRanking>(rankingList, compareTo);
        //rankingList = rankingList.GetPaging(PageIndex, PageSize, out PageCount);
        return rankingList.ToArray();
    }

    private int compareTo(UserRanking x, UserRanking y)
    {
        int result = y.Score - x.Score;
        if (result == 0)
        {
            result = y.UserID - x.UserID;
        }
        return result;
    }
}
}

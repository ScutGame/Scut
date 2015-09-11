/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
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
    /// 鑾峰彇Url鍙傛暟
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

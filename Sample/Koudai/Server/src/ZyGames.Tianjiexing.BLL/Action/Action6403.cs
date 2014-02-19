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
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6403_已报名公会列表接口
    /// </summary>
    public class Action6403 : BaseAction
    {
        private int pageIndex;
        private int pageSize;
        private int pageCount;
        private short rankID = 0;

        private List<ServerFight> fightList = new List<ServerFight>();

        public Action6403(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6403, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(pageCount);
            PushIntoStack(fightList.Count);
            foreach (var fight in fightList)
            {
                rankID = MathUtils.Addition(rankID, (short)1);
                UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(fight.GuildID);
                CityInfo cityInfo = new ConfigCacheSet<CityInfo>().FindKey(fight.CityID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(rankID);
                dsItem.PushIntoStack(guild == null ? string.Empty : guild.GuildName.ToNotNullString());
                dsItem.PushIntoStack(guild == null ? (short)0 : guild.GuildLv);
                dsItem.PushIntoStack(GuildFightCombat.GuildChairman(fight.GuildID).ToNotNullString());
                dsItem.PushIntoStack(cityInfo == null ? string.Empty : cityInfo.CityName.ToNotNullString());

                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref pageIndex)
                && httpGet.GetInt("PageSize", ref pageSize))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            FightCombat combat = new FightCombat();
            var serverFightList = new ShareCacheStruct<ServerFight>().FindAll(s => s.FastID == combat.FastID && !s.IsRemove);
            serverFightList.QuickSort((x, y) =>
            {
                int result;
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                result = x.CityID.CompareTo(y.CityID);
                if (result == 0)
                {
                    var userGuildA = new ShareCacheStruct<UserGuild>().FindKey(x.GuildID);
                    var userGuildB = new ShareCacheStruct<UserGuild>().FindKey(y.GuildID);
                    if (userGuildA != null && userGuildB != null)
                    {
                        result = userGuildB.GuildLv.CompareTo(userGuildA.GuildLv);
                        if (result == 0)
                        {
                            result = userGuildB.CurrExperience.CompareTo(userGuildA.CurrExperience);
                        }
                    }
                }
                return result;
            });
            fightList = serverFightList.GetPaging(pageIndex, pageSize, out pageCount);
            rankID = (short)(MathUtils.Subtraction(pageIndex, 1) * 5);
            return true;
        }
    }
}
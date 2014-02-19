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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6406_公会战报界面接口
    /// </summary>
    public class Action6406 : BaseAction
    {
        private int cityID;
        private string cityName;

        private List<ServerFightGroup> fightGroupList = new List<ServerFightGroup>();

        public Action6406(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6406, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(cityName.ToNotNullString());
            PushIntoStack(fightGroupList.Count);
            foreach (var againset in fightGroupList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(againset.Stage.ToInt());
                dsItem.PushIntoStack(againset.NO);
                dsItem.PushIntoStack(againset.ID.ToNotNullString());
                dsItem.PushIntoStack(againset.GuildIDA.ToNotNullString());
                dsItem.PushIntoStack(GuildFightCombat.GuildName(againset.GuildIDA).ToNotNullString());
                dsItem.PushIntoStack(againset.GuildIDB.ToNotNullString());
                dsItem.PushIntoStack(GuildFightCombat.GuildName(againset.GuildIDB).ToNotNullString());
                dsItem.PushIntoStack(FightGroupWinGuildID(againset));
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (true)
            {
                httpGet.GetInt("CityID", ref cityID);
                return true;
            }
        }

        public override bool TakeAction()
        {
            if (cityID == 0)
            {
                var fightList = new ConfigCacheSet<GuildFightInfo>().FindAll();
                if (fightList.Count > 0)
                {
                    cityID = fightList[0].CityID;
                }
            }
            var cityInfo = new ConfigCacheSet<CityInfo>().FindKey(cityID);
            if (cityInfo != null)
            {
                cityName = cityInfo.CityName;
            }
            FightCombat combat = new FightCombat();
            fightGroupList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == combat.FastID && s.CityID == cityID && !s.IsRemove);
            if (fightGroupList.Count == 0)
            {
                int fastID = MathUtils.Subtraction(combat.FastID, 1, 1);
                fightGroupList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == fastID && s.CityID == cityID && !s.IsRemove);
            }
            fightGroupList.QuickSort((x, y) =>
            {
                int result;
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                result = x.NO.CompareTo(y.NO);
                if (result == 0)
                {
                    var userGuildA = new ShareCacheStruct<UserGuild>().FindKey(x.GuildIDA);
                    var userGuildB = new ShareCacheStruct<UserGuild>().FindKey(y.GuildIDA);
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
            return true;
        }

        /// <summary>
        /// 判断公会战报的结果
        /// </summary>
        /// <param name="guildIDA"></param>
        /// <param name="guildIDB"></param>
        /// <returns></returns>
        public static int FightGroupWinGuildID(ServerFightGroup fightGroup)
        {
            if (string.IsNullOrEmpty(fightGroup.WinGuildID))
            {
                return 0;
            }
            else
            {
                if (fightGroup.GuildIDA == fightGroup.WinGuildID)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }

    }
}
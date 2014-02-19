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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6401_报名城市列表接口
    /// </summary>
    public class Action6401 : BaseAction
    {
        private string firstName;
        private int cityID = 0;
        private short postType;
        private short isApply;
        private string currCityName;
        private string rewardDesc;
        private string guildName = null;
        private int endData;
        private List<GuildFightInfo> fightList = new List<GuildFightInfo>();
        private short isChampion;

        public Action6401(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6401, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(firstName.ToNotNullString());
            this.PushIntoStack((short)postType);
            this.PushIntoStack((short)isApply);
            this.PushIntoStack(fightList.Count);
            foreach (var fight in fightList)
            {
                CityInfo cityInfo = new ConfigCacheSet<CityInfo>().FindKey(fight.CityID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(fight.CityID);
                dsItem.PushIntoStack(cityInfo == null ? string.Empty : cityInfo.CityName.ToNotNullString());
                dsItem.PushIntoStack(fight.CityHead.ToNotNullString());
                dsItem.PushIntoStack((short)fight.GuildLv);
                dsItem.PushIntoStack(fight.SkillNum);
                this.PushIntoStack(dsItem);
            }
            this.PushIntoStack(currCityName.ToNotNullString());
            this.PushIntoStack(rewardDesc.ToNotNullString());
            this.PushIntoStack(guildName.ToNotNullString());
            this.PushIntoStack(endData);
            this.PushIntoStack((short)isChampion);


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
            if (string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            FightCombat info = new FightCombat();
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
            if (guild == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            firstName = guild.GuildName.Substring(0, 1);
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID,
                                                                             ContextUser.UserID);
            postType = member == null ? (short)0 : (short)member.PostType;
            ServerFight serverFight = new ShareCacheStruct<ServerFight>().FindKey(info.FastID, ContextUser.MercenariesID);
            if (serverFight != null)
            {
                isApply = 1;
            }
            fightList = new ConfigCacheSet<GuildFightInfo>().FindAll();
            if (cityID == 0 && fightList.Count > 0)
            {
                cityID = fightList[0].CityID;
            }
            var fightCity = new ConfigCacheSet<GuildFightInfo>().FindKey(cityID);
            if (fightCity != null)
            {
                rewardDesc = fightCity.CityDesc;
            }
            CityInfo cityInfo = new ConfigCacheSet<CityInfo>().FindKey(cityID);
            currCityName = cityInfo == null ? string.Empty : cityInfo.CityName;
            endData = (int)(GuildFightCombat.CurrFightDate() - DateTime.Now).TotalSeconds;

            int fastID = MathUtils.Subtraction(info.FastID, 1);
            var serverFightList = new ShareCacheStruct<ServerFight>().FindAll(s => s.FastID == fastID && !s.IsRemove && s.RankID == 1);
            foreach (var fight in serverFightList)
            {
                if (fight.CityID == cityID)
                {
                    guildName = GuildFightCombat.GuildName(fight.GuildID);
                }

                if (ContextUser.MercenariesID == fight.GuildID)
                {
                    isChampion = 1;
                }
            }

            return true;
        }
    }
}
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
using System.Data;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6202_公会技能可升级列表接口
    /// </summary>
    public class Action6202 : BaseAction
    {
        private int pageIndex;
        private int pageSize;
        private int pageCount;
        private List<GuildAbility> abilityArray = new List<GuildAbility>();

        public Action6202(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6202, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(pageCount);
            this.PushIntoStack(abilityArray.Count);
            foreach (GuildAbility item in abilityArray)
            {
                GuildAbilityInfo gAbilityInfo = new ConfigCacheSet<GuildAbilityInfo>().FindKey(item.ID);
                GuildAbilityLvInfo abilityLvInfo = new ConfigCacheSet<GuildAbilityLvInfo>().FindKey(item.ID, item.Lv);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.ID);
                dsItem.PushIntoStack(gAbilityInfo == null ? string.Empty : gAbilityInfo.AbilityName.ToNotNullString());
                dsItem.PushIntoStack(gAbilityInfo == null ? string.Empty : gAbilityInfo.AbilityHead.ToNotNullString());
                dsItem.PushIntoStack(item.Lv);
                dsItem.PushIntoStack(abilityLvInfo == null ? string.Empty : abilityLvInfo.EffectNum.ToNotNullString());

                this.PushIntoStack(dsItem);
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
            if (!string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
                if (member != null && member.PostType == PostType.Member)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6007_AuditPermissions;
                    return false;
                }
                UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
                if (guild != null && guild.AbilityInfo.Count > 0)
                {
                    List<GuildAbility> guildAbilitiesList = new List<GuildAbility>();
                    List<GuildAbility> abilitiesList = guild.AbilityInfo.FindAll(m => m.Lv < 10);
                    foreach (GuildAbility ability in abilitiesList)
                    {
                        GuildAbilityLvInfo abilityLvInfo = new ConfigCacheSet<GuildAbilityLvInfo>().FindKey(ability.ID, ability.Lv);
                        if (abilityLvInfo != null && guild.CurrDonateNum >= abilityLvInfo.UpDonateNum)
                        {
                            guildAbilitiesList.Add(ability);
                        }
                    }
                    abilityArray = guildAbilitiesList.GetPaging(pageIndex, pageSize, out pageCount);
                }
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6203_GuildMemberNotEnough;
                return false;
            }
            return true;
        }
    }
}
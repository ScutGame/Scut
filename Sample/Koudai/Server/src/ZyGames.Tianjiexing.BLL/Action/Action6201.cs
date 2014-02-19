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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Cache.Generic;
using System.Collections.Generic;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6201_公会技能列表接口
    /// </summary>
    public class Action6201 : BaseAction
    {
        private int pageIndex;
        private int pageSize;
        private int pageCount;
        private int currExperience;
        private int donateCoin;
        private int donateGold;
        private List<GuildAbility> abilityArray = new List<GuildAbility>();
        private GuildAbilityInfo gAbilityInfo = null;
        private short isCityCombat = 0;

        public Action6201(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6201, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(pageCount);
            this.PushIntoStack(abilityArray.Count);
            foreach (GuildAbility ability in abilityArray)
            {
                GuildAbilityInfo abilityInfo = new ConfigCacheSet<GuildAbilityInfo>().FindKey(ability.ID);
                if (abilityInfo != null)
                {
                    isCityCombat = abilityInfo.IsCityCombat ? (short)1 : (short)0;
                }
                GuildAbilityLvInfo abilityLvInfo = new ConfigCacheSet<GuildAbilityLvInfo>().FindKey(ability.ID, ability.Lv);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(ability.ID);
                dsItem.PushIntoStack(abilityInfo == null ? string.Empty : abilityInfo.AbilityName.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo == null ? string.Empty : abilityInfo.AbilityHead.ToNotNullString());
                dsItem.PushIntoStack(ability.Lv);
                dsItem.PushIntoStack(abilityLvInfo == null ? string.Empty : abilityLvInfo.EffectNum.ToString().ToNotNullString());
                dsItem.PushIntoStack(abilityLvInfo == null ? 0 : abilityLvInfo.UpDonateNum);
                dsItem.PushIntoStack(isCityCombat);
                dsItem.PushIntoStack(abilityInfo == null ? (short)0 : (short)abilityInfo.GuildAbilityType);
                this.PushIntoStack(dsItem);
            }
            this.PushIntoStack(gAbilityInfo == null ? string.Empty : gAbilityInfo.AbilityName.ToNotNullString());
            this.PushIntoStack(currExperience);
            this.PushIntoStack(donateCoin);
            this.PushIntoStack(donateGold);
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
            int maxDonateGold = ConfigEnvSet.GetInt("UserGuild.MaxDonateGold");
            if (string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6203_GuildMemberNotEnough;
                return false;
            }
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
            if (guild == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            if (guild.AbilityInfo.Count == 0)
            {
                UserHelper.UserGuildAbilityList(ContextUser.MercenariesID);
            }
            CombatHelper.RepairGuildAbility(ContextUser.MercenariesID);
            currExperience = guild.CurrDonateNum;
            if (guild.AbilityInfo.Count > 0)
            {
                List<GuildAbility> abilitiesList = guild.AbilityInfo.ToList();
                abilityArray = abilitiesList.GetPaging(pageIndex, pageSize, out pageCount);
            }

            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
            if (member != null)
            {
                donateCoin = MathUtils.Subtraction(UserHelper.MaxDonateGameCoin(ContextUser.UserLv.ToInt()), member.DonateCoin);
                donateGold = MathUtils.Subtraction(maxDonateGold, member.DonateGold);
            }

            return true;
        }
    }
}
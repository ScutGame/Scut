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

using System.Data;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6205_公会技能升级接口
    /// </summary>
    public class Action6205 : BaseAction
    {
        private int guildSkillID;


        public Action6205(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6205, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GuildSkillID", ref guildSkillID))
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
                    GuildAbility ability = guild.AbilityInfo.Find(m => m.ID == guildSkillID);
                    if (ability != null)
                    {
                        GuildAbilityLvInfo abilityLvInfo = new ConfigCacheSet<GuildAbilityLvInfo>().FindKey(ability.ID, ability.Lv);
                        if (abilityLvInfo != null && ability.Lv < 10)
                        {
                            if (abilityLvInfo.UpDonateNum > guild.CurrDonateNum)
                            {
                                ErrorCode = LanguageManager.GetLang().ErrorCode;
                                ErrorInfo = LanguageManager.GetLang().St6205_GuildMemberDonateNotEnough;
                                return false;
                            }
                            GuildAbilityInfo abilityInfo = new ConfigCacheSet<GuildAbilityInfo>().FindKey(ability.ID);
                            if (abilityInfo != null)
                            {
                                guild.CurrDonateNum = MathUtils.Subtraction(guild.CurrDonateNum, abilityLvInfo.UpDonateNum, 0);
                                ability.Lv = MathUtils.Addition(ability.Lv, (short)1);
                                abilityLvInfo = new ConfigCacheSet<GuildAbilityLvInfo>().FindKey(ability.ID, ability.Lv);
                                ability.Type = abilityInfo.GuildAbilityType;
                                ability.Num = abilityLvInfo.EffectNum;
                                //guild.Update();
                                CombatHelper.UpGuildAbilityLv(ContextUser.MercenariesID, ability); //加载公会技能升级
                                ErrorCode = 0;
                                ErrorInfo = string.Format(LanguageManager.GetLang().St6205_GuildMemberJiNengShengJi, abilityInfo.AbilityName, ability.Lv);
                            }
                        }
                    }
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
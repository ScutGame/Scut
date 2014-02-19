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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6404_公会报名接口
    /// </summary>
    public class Action6404 : BaseAction
    {
        private int cityID;

        public Action6404(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6404, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("CityID", ref cityID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6203_GuildMemberNotEnough;
                return false;
            }
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
            if (member == null || member.PostType == PostType.Member)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6404_OrdinaryMemberNotCompetence;
                return false;
            }
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
            if (guild == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }

            FightCombat fightCombat = new FightCombat();
            var cacheSet = new ShareCacheStruct<ServerFight>();
            ServerFight fight = cacheSet.FindKey(fightCombat.FastID, ContextUser.MercenariesID);
            if (fight != null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6404_HaveSignedUp;
                return false;
            }

            DateTime nextDate;
            FightStage stage = GuildFightCombat.GetStage(out nextDate);
            if (GuildFightCombat.IsFightDate())
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6404_CityABattleTime;
                return false;
            }

            if (stage != FightStage.Apply)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6404_CityABattleTime;
                return false;
            }

            //if (DateTime.Now > fightCombat.GuildEndTime && DateTime.Now < fightCombat.GuildStartTime)
            //{
            //    ErrorCode = LanguageManager.GetLang().ErrorCode;
            //    ErrorInfo = LanguageManager.GetLang().St6404_OutRegistrationTime;
            //    return false;
            //}

            GuildFightInfo fightInfo = new ConfigCacheSet<GuildFightInfo>().FindKey(cityID);
            if (fightInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            if (guild.GuildLv < fightInfo.GuildLv)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6404_GuildLvNotEnough;
                return false;
            }
            if (guild.CurrDonateNum < fightInfo.SkillNum)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6404_CurrDonateNumNotEnough;
                return false;
            }
            guild.CurrDonateNum = MathUtils.Subtraction(guild.CurrDonateNum, fightInfo.SkillNum);

            fight = new ServerFight(fightCombat.FastID, ContextUser.MercenariesID);
            fight.GuildBanner = string.Empty;
            fight.CityID = cityID;
            fight.RankID = 0;
            fight.ApplyDate = DateTime.Now;
            fight.IsRemove = false;
            fight.IsBanner = false;
            cacheSet.Add(fight);
            ErrorCode = 0;
            ErrorInfo = LanguageManager.GetLang().St6401_SuccessfulRegistration;
            return true;
        }
    }
}
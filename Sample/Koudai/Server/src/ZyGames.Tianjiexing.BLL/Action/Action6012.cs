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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6012_公会成员祭神上香接口
    /// </summary>
    public class Action6012 : BaseAction
    {
        private int idolID = 0;


        public Action6012(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6012, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("IdolID", ref idolID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                return false;
            }
            if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.GuildMemberShangSuHeXiang) && idolID == 2)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_VipNotEnoughNotFuntion;
                return false;
            }

            if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.GuildMemberShangTianMuXiang) && idolID == 3)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_VipNotEnoughNotFuntion;
                return false;
            }

            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
            if (member != null)
            {
                GuildIdolInfo idolInfo = new ConfigCacheSet<GuildIdolInfo>().FindKey(idolID);
                var memberLog = new ShareCacheStruct<GuildMemberLog>().FindKey(ContextUser.MercenariesID) ?? new GuildMemberLog();
                List<MemberLog> guildLogArray = memberLog.GetLog(u => u.UserID == ContextUser.UserID && DateTime.Now.Date == u.InsertDate.Date);
                UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);
                if (guildLogArray.Count > 0 && userRestrain.Funtion6 >= VipHelper.GetVipUseNum(ContextUser.VipLv, RestrainType.BangPaiShangXiang) && DateTime.Now.Date == userRestrain.RefreshDate.Date)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6012_HasIncenseToday;
                    return false;
                }
                if (idolInfo.UseExpNum != 0 && ContextUser.ExpNum <= idolInfo.UseExpNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_ExpNumNotEnough;
                    return false;
                }
                else if (idolInfo.UseGold != 0 && ContextUser.GoldNum < idolInfo.UseGold)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                //UserGuild userGuild = new ShareCacheStruct<UserGuild>().FindKey(guildID);


                userRestrain.Funtion6 = MathUtils.Addition(userRestrain.Funtion6, 1, int.MaxValue);
                //userRestrain.Update();
                UpIdolLv(ContextUser.MercenariesID, idolInfo.GainAura);

                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, idolInfo.UseGold, int.MaxValue);
                ContextUser.ExpNum = MathUtils.Subtraction(ContextUser.ExpNum, idolInfo.UseExpNum, 0);
                ContextUser.ObtainNum = MathUtils.Addition(ContextUser.ObtainNum, idolInfo.GainObtion, int.MaxValue);
                //ContextUser.Update();

                UserHelper.UserGuildUpLv(ContextUser.MercenariesID, idolInfo.GainObtion);
                if (DateTime.Now.Date == member.IncenseDate.Date)
                {
                    member.GainBlessing = idolInfo.GainBlessing;
                    member.IncenseNum = MathUtils.Addition(member.IncenseNum, 1, int.MaxValue);
                    member.Contribution = MathUtils.Addition(member.Contribution, idolInfo.GainObtion, int.MaxValue);
                }
                else
                {
                    member.IncenseNum = 1;
                    member.GainBlessing = idolInfo.GainBlessing;
                    member.Contribution = idolInfo.GainObtion;
                }
                member.TotalContribution = MathUtils.Addition(member.TotalContribution, idolInfo.GainObtion, int.MaxValue);
                member.IncenseDate = DateTime.Now;
                //member.Update();

                GuildMemberLog.AddLog(member.GuildID, new MemberLog()
                {
                    UserID = ContextUser.UserID,
                    IdolID = idolID,
                    LogType = 2,
                    GainObtion = idolInfo.GainObtion,
                    Experience = idolInfo.GainObtion,
                    GainAura = idolInfo.GainAura,
                    InsertDate = DateTime.Now,
                });

            }

            return true;
        }

        /// <summary>
        /// 上香升级
        /// </summary>
        /// <param name="guildID"></param>
        /// <param name="gainAura"></param>
        public static void UpIdolLv(string guildID, int gainAura)
        {
            GuildIdol guildIdol = new ShareCacheStruct<GuildIdol>().FindKey(guildID);

            if (guildIdol != null)
            {
                guildIdol.CurrExperience = MathUtils.Addition(guildIdol.CurrExperience, gainAura, int.MaxValue);
                IdolLvInfo lvInfo = new ConfigCacheSet<IdolLvInfo>().FindKey(guildIdol.IdolLv);
                if (lvInfo != null)
                {
                    if (guildIdol.CurrExperience >= lvInfo.UpExperience)
                    {
                        if (lvInfo.IdolLv != 10)
                        {
                            guildIdol.CurrExperience = MathUtils.Subtraction(guildIdol.CurrExperience, lvInfo.UpExperience, 0);
                            guildIdol.IdolLv = MathUtils.Addition(guildIdol.IdolLv, (short)1, short.MaxValue);
                        }
                    }
                }
                //guildIdol.Update();
            }
        }
    }
}
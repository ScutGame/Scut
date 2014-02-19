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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6204_公会技能捐献接口
    /// </summary>
    public class Action6204 : BaseAction
    {
        /// <summary>
        /// 捐献类型1：金币捐献 2：晶石捐献
        /// </summary>
        private int donateType;
        /// <summary>
        /// 捐献数量
        /// </summary>
        private int donateNum;
        /// <summary>
        /// 1：捐献 2：确认捐献
        /// </summary>
        private int ops;

        private int totalDonateNum = 0;


        public Action6204(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6204, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("DonateType", ref donateType)
                 && httpGet.GetInt("DonateNum", ref donateNum)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int subdonateNum = 0;
            int mount = 0;
            int wholeNum = 0;

            if (string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6203_GuildMemberNotEnough;
                return false;
            }

            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
            if (guild == null)
            {
                return false;
            }
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
            if (member == null)
            {
                return false;
            }
            int result;
            int maxDonateCoin = UserHelper.MaxDonateGameCoin(ContextUser.UserLv.ToInt());
            if (donateType == 1)
            {
                int gameCoinProportion = ConfigEnvSet.GetInt("UserGuild.DonateGameCoinProportion");
                if (!IsNumberic(donateNum.ToString(), out result))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6204_GuildMemberDonateNum;
                    return false;
                }

                if (member.DonateCoin >= maxDonateCoin)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6204_OutMaxGuildMemberNum;
                    return false;
                }

                subdonateNum = MathUtils.Subtraction(maxDonateCoin, member.DonateCoin);
                mount = donateNum % gameCoinProportion;
                wholeNum = donateNum / gameCoinProportion;
                if (mount != 0 || donateNum > subdonateNum)
                {
                    totalDonateNum = (wholeNum * gameCoinProportion);
                    if (totalDonateNum > subdonateNum)
                    {
                        totalDonateNum = subdonateNum;
                        wholeNum = totalDonateNum / gameCoinProportion;
                    }
                }
                else
                {
                    totalDonateNum = donateNum;
                }

                if (ops == 1)
                {
                    if (donateNum < gameCoinProportion)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St6204_GuildMemberDonateNum;
                        return false;
                    }
                    ErrorCode = 1;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St6204_GuildMemberGameCoinDonate, totalDonateNum, wholeNum, wholeNum);
                    return false;
                }
                else if (ops == 2)
                {
                    if (totalDonateNum > ContextUser.GameCoin)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                        return false;
                    }

                    GetAddDonate(guild, member, wholeNum);
                    ErrorCode = 2;
                }
            }
            else if (donateType == 2)
            {
                int maxDonateGold = ConfigEnvSet.GetInt("UserGuild.MaxDonateGold");
                if (!IsNumberic(donateNum.ToString(), out result))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6204_GuildMemberDonateNumGold;
                    return false;
                }
                if (member.DonateGold >= maxDonateGold)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6204_OutMaxGuildMemberNum;
                    return false;
                }
                int goldProportion = ConfigEnvSet.GetInt("UserGuild.DonateGoldProportion");
                subdonateNum = MathUtils.Subtraction(maxDonateGold, member.DonateGold);
                mount = donateNum % goldProportion;
                wholeNum = donateNum / goldProportion;
                if (mount != 0 || donateNum > subdonateNum)
                {
                    totalDonateNum = (wholeNum * goldProportion);
                    if (totalDonateNum > subdonateNum)
                    {
                        totalDonateNum = subdonateNum;
                        wholeNum = totalDonateNum / goldProportion;
                    }
                }
                else
                {
                    totalDonateNum = donateNum;
                }

                if (ops == 1)
                {
                    if (donateNum < goldProportion)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St6204_GuildMemberDonateNumGold;
                        return false;
                    }
                    ErrorCode = 1;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St6204_GuildMemberGoldDonate, totalDonateNum, wholeNum, wholeNum);
                    return false;
                }
                else if (ops == 2)
                {

                    if (totalDonateNum > ContextUser.GoldNum)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }

                    GetAddDonate(guild, member, wholeNum);
                    ErrorCode = 2;
                }
            }


            return true;
        }

        /// <summary>
        /// 捐献数量，贡献声望
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="member"></param>
        /// <param name="wholeNum"></param>
        private void GetAddDonate(UserGuild guild, GuildMember member, int wholeNum)
        {
            guild.CurrDonateNum = MathUtils.Addition(guild.CurrDonateNum, wholeNum);
            //guild.Update();

            if (donateType == 1)
            {
                ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, totalDonateNum);
                member.DonateCoin = MathUtils.Addition(member.DonateCoin, totalDonateNum);
            }
            else if (donateType == 2)
            {
                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, totalDonateNum);
                member.DonateGold = MathUtils.Addition(member.DonateGold, totalDonateNum);
            }
            ContextUser.ObtainNum = MathUtils.Addition(ContextUser.ObtainNum, wholeNum);
            //ContextUser.Update();
            member.Contribution = MathUtils.Addition(member.Contribution, wholeNum);
            member.TotalContribution = MathUtils.Addition(member.TotalContribution, wholeNum);
            //member.Update();
            UserHelper.UserGuildUpLv(member.GuildID, wholeNum); //公会添加经验，升级
            GuildMemberLog.AddLog(member.GuildID, new MemberLog
            {
                UserID = ContextUser.UserID,
                IdolID = 0,
                LogType = 1,
                GainObtion = wholeNum,
                Experience = wholeNum,
                GainAura = 0,
                InsertDate = DateTime.Now
            });
        }

        private bool IsNumberic(string message, out int result)
        {
            result = -1;
            try
            {
                result = Convert.ToInt32(message);
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
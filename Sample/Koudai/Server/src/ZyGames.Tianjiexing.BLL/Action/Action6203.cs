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

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6203_公会技能捐献信息接口
    /// </summary>
    public class Action6203 : BaseAction
    {
        private int donateType;
        private int donateNum;
        private int maxDonateNum;
        private int proportion;


        public Action6203(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6203, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(donateNum);
            this.PushIntoStack(maxDonateNum);
            this.PushIntoStack(proportion);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("DonateType", ref donateType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (!string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                int maxDonateGold = ConfigEnvSet.GetInt("UserGuild.MaxDonateGold");
                int maxDonateCoin = UserHelper.MaxDonateGameCoin(ContextUser.UserLv.ToInt());
                int gameCoinProportion = ConfigEnvSet.GetInt("UserGuild.DonateGameCoinProportion");
                int goldProportion = ConfigEnvSet.GetInt("UserGuild.DonateGoldProportion");
                GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
                if (member != null)
                {
                    if (donateType == 1)
                    {
                        donateNum = member.DonateCoin;
                        maxDonateNum = maxDonateCoin;
                        proportion = gameCoinProportion;
                    }
                    else if (donateType == 2)
                    {
                        donateNum = member.DonateGold;
                        maxDonateNum = maxDonateGold;
                        proportion = goldProportion;
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
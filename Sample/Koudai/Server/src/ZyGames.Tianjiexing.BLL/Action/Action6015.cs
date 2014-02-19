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


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6015_公会成员召唤散仙接口
    /// </summary>
    public class Action6015 : BaseAction
    {
        private int ops = 1;


        public Action6015(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6015, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserHelper.ChecheDailyContribution(ContextUser.MercenariesID, ContextUser.UserID);
            int summonGold = ConfigEnvSet.GetInt("GuildMember.SummonSanxian");
            List<GuildMember> guildMemberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.UserID == ContextUser.UserID && m.IsDevil == 1 && DateTime.Now.Date == m.DevilDate.Date);
            if (guildMemberArray.Count > 0)
            {
                GuildMember member = guildMemberArray[0];
                if (ops == 1)
                {
                    ErrorCode = 1;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St6015_SummonSanxian, summonGold);
                    return false;
                }
                else if (ops == 2)
                {
                    if (ContextUser.GoldNum < summonGold)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                    member.SummonNum = MathUtils.Addition(member.SummonNum, 1, int.MaxValue);
                    member.CurrNum = MathUtils.Addition(member.CurrNum, 1, int.MaxValue);
                    //member.Update();

                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, summonGold, int.MaxValue);
                    //ContextUser.Update();
                }
            }
            return true;
        }
    }
}
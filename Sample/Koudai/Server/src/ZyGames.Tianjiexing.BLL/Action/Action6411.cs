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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6411_争夺战退出接口
    /// </summary>
    public class Action6411 : BaseAction
    {
        public Action6411(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6411, httpGet)
        {

        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (true)
            {
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
            var cacheSet = new ShareCacheStruct<ServerFight>();
            FightCombat combat = new FightCombat();
            ServerFight fight = cacheSet.FindKey(combat.FastID, ContextUser.MercenariesID);
            if (fight != null)
            {
                if (!string.IsNullOrEmpty(fight.CombatMember))
                {
                    fight.CombatMember = fight.CombatMember.Replace(ContextUser.UserID, "").Replace(",,", ",");
                    if (fight.CombatMember == ",")
                    {
                        fight.CombatMember = string.Empty;
                    }
                }
                FightUser fightUser = GuildFightCombat._fightUserList.Find(m => m.UserId == ContextUser.UserID);
                if (fightUser != null)
                {
                    GuildFightCombat._fightUserList.Remove(fightUser);
                }
                ContextUser.UserStatus = UserStatus.Normal;
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6411_FailedToExit;
                return false;
            }
            return true;
        }
    }
}
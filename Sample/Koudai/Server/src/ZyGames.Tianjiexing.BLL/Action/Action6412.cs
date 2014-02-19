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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6412_争夺战参战接口
    /// </summary>
    public class Action6412 : BaseAction
    {
        public Action6412(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6412, httpGet)
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
            FightCombat fightCombat = new FightCombat();
            var cacheSet = new ShareCacheStruct<ServerFight>();
            ServerFight fight = cacheSet.FindKey(fightCombat.FastID, ContextUser.MercenariesID);
            if (fight != null)
            {
                if (GuildFightCombat.IsFightDate())
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6412_FightWarDate;
                    return false;
                }

                var fightUser = GuildFightCombat._fightUserList.Find(m => !m.IsRemove && m.UserId == ContextUser.UserID);
                if (fightUser == null)
                {
                    fightUser = new FightUser();
                    fightUser.GuildID = fight.GuildID;
                    fightUser.UserId = ContextUser.UserID;
                    fightUser.UserName = ContextUser.NickName;
                    fightUser.WinCount = 0;
                    fightUser.CityID = fight.CityID;
                    fightUser.ObtainNum = 0;
                    fightUser.InspirePercent = 0;
                    fightUser.IsRemove = false;
                    fightUser.IsNotEnough = false;
                    GuildFightCombat._fightUserList.Add(fightUser);
                }
                if (!string.IsNullOrEmpty(fight.CombatMember) && !GuildFightCombat.IsFightWar(ContextUser.UserID, fight.CombatMember))
                {
                    fight.CombatMember = fight.CombatMember + ContextUser.UserID + ",";
                }
                else
                {
                    fight.CombatMember = ContextUser.UserID + ",";
                }
                ContextUser.UserStatus = UserStatus.FightCombat;
                ErrorCode = 0;
                ErrorInfo = LanguageManager.GetLang().St6412_FightWarSuccess;
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6412_HaveSignedUp;
                return false;
            }
            return true;
        }
    }
}
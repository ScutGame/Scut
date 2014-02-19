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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1462_法宝技能升级接口
    /// </summary>
    public class Action1462 : BaseAction
    {
        private int skillID;


        public Action1462(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1462, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("SkillID", ref skillID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump != null && userTrump.SkillInfo.Count > 0)
            {
                SkillInfo skillInfo = userTrump.SkillInfo.Find(m => m.AbilityID == skillID);
                if (skillInfo != null)
                {
                    if (skillInfo.AbilityLv >= GameConfigSet.MaxSkillLv)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1462_OutMaxTrumpLv;
                        return false;
                    }
                    SkillLvInfo skillLvInfo = new ConfigCacheSet<SkillLvInfo>().FindKey(skillInfo.AbilityID, skillInfo.AbilityLv);
                    if (skillLvInfo != null)
                    {
                        if (ContextUser.GameCoin < skillLvInfo.GameCoin)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                            return false;
                        }
                        if (ContextUser.ObtainNum < skillLvInfo.ObtainNum)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St_ObtainNumNotEnough;
                            return false;
                        }
                        int upitemNum = TrumpHelper.GetUserItemNum(ContextUser.UserID, skillLvInfo.ItemID);
                        if (upitemNum < skillLvInfo.ItemNum)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1462_ItemNumNotEnough;
                            return false;
                        }
                        if (TrumpHelper.IsMeetUpSkillLv(ContextUser, skillLvInfo))
                        {
                            ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, skillLvInfo.GameCoin, 0);
                            ContextUser.ObtainNum = MathUtils.Subtraction(ContextUser.ObtainNum, skillLvInfo.ObtainNum, 0);
                            UserItemHelper.UseUserItem(ContextUser.UserID, skillLvInfo.ItemID, skillLvInfo.ItemNum);
                            skillInfo.UpdateNotify(obj =>
                            {
                                skillInfo.AbilityLv = MathUtils.Addition(skillInfo.AbilityLv, (short)1, GameConfigSet.MaxTrumpLv.ToShort());
                                return true;
                            });
                            ErrorCode = 0;
                            ErrorInfo = LanguageManager.GetLang().St1464_UpgradeWasSsuccessful;
                        }
                    }
                }
            }
            return true;
        }
    }
}
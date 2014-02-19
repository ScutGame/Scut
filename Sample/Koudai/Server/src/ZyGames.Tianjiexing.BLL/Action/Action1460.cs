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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1460_法宝技能洗涤接口
    /// </summary>
    public class Action1460 : BaseAction
    {
        private int skillID;
        private int ops;


        public Action1460(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1460, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("SkillID", ref skillID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int useGold = ConfigEnvSet.GetInt("TrumpSkill.WashIngUseGold");
            if (ops == 1)
            {
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1460_WashingSkills, useGold);
                return false;
            }
            else if (ops == 2)
            {
                if (useGold > ContextUser.GoldNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1460_WashingSkillsNotEnough;
                    return false;
                }
                UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
                if (userTrump == null || userTrump.SkillInfo.Count == 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1466_WorshipPropertyNotEnough;
                    return false;
                }
                SkillInfo trumpSkill = userTrump.SkillInfo.Find(m => m.AbilityID == skillID);
                if (trumpSkill == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1460_SkillsNotEnough;
                    return false;
                }
                AbilityInfo abilityInfo = TrumpHelper.GetRandomAbility(ContextUser.UserID);
                if (abilityInfo != null)
                {
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold);
                    trumpSkill.UpdateNotify(obj =>
                    {
                        trumpSkill.AbilityID = abilityInfo.AbilityID;
                        trumpSkill.AbilityLv = 1;
                        return true;
                    });
                    ErrorCode = ops;
                    ErrorInfo = LanguageManager.GetLang().St1460_WashingSuccess;
                }
            }
            return true;
        }

        public static bool IsWash(string userID, int abilityID)
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            if (userTrump != null && userTrump.SkillInfo.Count > 0)
            {
                SkillInfo skillInfo = userTrump.SkillInfo.Find(m => m.AbilityID == abilityID);
                if (skillInfo != null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
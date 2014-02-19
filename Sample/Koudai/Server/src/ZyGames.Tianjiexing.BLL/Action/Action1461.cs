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
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1461_法宝技能升级消耗接口
    /// </summary>
    public class Action1461 : BaseAction
    {
        private short skillID;
        private string skillName;
        private short skillLv;
        private string itemName;
        private int itemNum;
        private int gameCoin;
        private int obtainNum;
        private short isMeet;
        private short isItem;
        private short isCoin;
        private short isObtain;

        public Action1461(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1461, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(skillName.ToNotNullString());
            PushIntoStack(skillLv);
            PushIntoStack(itemName.ToNotNullString());
            PushIntoStack(itemNum);
            PushIntoStack(gameCoin);
            PushIntoStack(obtainNum);
            PushIntoStack((short)isMeet);
            PushIntoStack((short)isItem);
            PushIntoStack((short)isCoin);
            PushIntoStack((short)isObtain);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetWord("SkillID", ref skillID))
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
                    AbilityInfo abilityInfo = new ConfigCacheSet<AbilityInfo>().FindKey(skillInfo.AbilityID);
                    if (abilityInfo != null)
                    {
                        skillName = abilityInfo.AbilityName;
                    }
                    skillLv = skillInfo.AbilityLv;

                    short upLv = MathUtils.Addition(skillInfo.AbilityLv, (short)1, GameConfigSet.MaxTrumpLv.ToShort());
                    if (upLv <= GameConfigSet.MaxTrumpLv)
                    {
                        SkillLvInfo skillLvInfo = new ConfigCacheSet<SkillLvInfo>().FindKey(skillInfo.AbilityID, skillInfo.AbilityLv);
                        if (skillLvInfo != null)
                        {
                            gameCoin = skillLvInfo.GameCoin;
                            obtainNum = skillLvInfo.ObtainNum;
                            int upItemNum = TrumpHelper.GetUserItemNum(ContextUser.UserID, skillLvInfo.ItemID);
                            if (upItemNum >= skillLvInfo.ItemNum)
                            {
                                isItem = 1;
                            }
                            if (ContextUser.GameCoin >= gameCoin)
                            {
                                isCoin = 1;
                            }
                            if (ContextUser.ObtainNum >= obtainNum)
                            {
                                isObtain = 1;
                            }
                            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(skillLvInfo.ItemID);
                            if (itemInfo != null)
                            {
                                itemName = itemInfo.ItemName;
                                itemNum = skillLvInfo.ItemNum;
                            }
                            if (TrumpHelper.IsMeetUpSkillLv(ContextUser, skillLvInfo))
                            {
                                isMeet = 1;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
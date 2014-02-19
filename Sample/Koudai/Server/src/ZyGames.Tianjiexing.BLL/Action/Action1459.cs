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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1459_法宝技能列表接口
    /// </summary>
    public class Action1459 : BaseAction
    {
        private short trumpLv = 0;
        private int isOpen = 0;
        private int skillcount = 0;
        private TrumpInfo[] trumpInfoArray = new TrumpInfo[0];
        private short isFull;

        public Action1459(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1459, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(trumpInfoArray.Length);
            foreach (var trumpInfo in trumpInfoArray)
            {
                isOpen = trumpLv >= trumpInfo.TrumpLv ? 1 : 0;
                SkillInfo skillInfo = TrumpHelper.GetSkillInfo(ContextUser.UserID, skillcount);
                if (skillInfo != null && skillInfo.AbilityLv >= GameConfigSet.MaxSkillLv)
                {
                    isFull = 1;
                }
                else
                {
                    isFull = 0;
                }
                AbilityInfo abilityInfo = TrumpHelper.GetAbilityInfo(ContextUser.UserID, skillcount);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(abilityInfo == null ? (short)0 : (short)abilityInfo.AbilityID);
                dsItem.PushIntoStack(abilityInfo == null ? string.Empty : abilityInfo.AbilityName.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo == null ? string.Empty : abilityInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(skillInfo == null ? (short)0 : skillInfo.AbilityLv);
                dsItem.PushIntoStack(abilityInfo == null ? string.Empty : abilityInfo.AbilityDesc.ToNotNullString());
                dsItem.PushIntoStack((short)isOpen);
                dsItem.PushIntoStack(trumpInfo.TrumpLv);
                dsItem.PushIntoStack(isFull);
                PushIntoStack(dsItem);
                skillcount++;
            }
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
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump != null)
            {
                trumpLv = userTrump.TrumpLv;
            }
            trumpInfoArray = new ConfigCacheSet<TrumpInfo>().FindAll(m => m.SkillID > 0 && m.TrumpID == TrumpInfo.CurrTrumpID).ToArray();
            TrumpHelper.RepairMagicSkills(ContextUser.UserID);
            return true;
        }
    }
}
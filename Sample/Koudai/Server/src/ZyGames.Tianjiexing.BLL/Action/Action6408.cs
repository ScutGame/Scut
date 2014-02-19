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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Cache.Generic;


namespace ZyGames.Tianjiexing.BLL.Action
{

    ///<summary>
    ///6408_公会成员战斗详情接口
    ///</summary>
    public class Action6408 : BaseAction
    {
        private string sportID = string.Empty;
        private int version = 0;
        private List<FightCombatProcess> processList = new List<FightCombatProcess>();
        private short isWin = 0;
        private UserStatus status;

        public Action6408(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6408, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(processList.Count > 0 ? processList[processList.Count - 1].Version : 0);
            this.PushIntoStack(processList.Count);
            foreach (var process in processList)
            {
                DataStruct ds = new DataStruct();
                ds.PushIntoStack((short)isWin);
                ds.PushIntoStack(process.WinObtainNum);
                ds.PushIntoStack(process.FailObtainNum);
                ds.PushIntoStack(process.ProcessContainer.AttackList.Count);
                foreach (CombatEmbattle combatEmbattle in process.ProcessContainer.AttackList)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(combatEmbattle.GeneralID);
                    dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                    dsItem.PushIntoStack(combatEmbattle.HeadID.ToNotNullString());
                    dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                    dsItem.PushIntoStack(combatEmbattle.LiveNum);
                    dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                    dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                    dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                    dsItem.PushIntoStack(combatEmbattle.AbilityID);
                    dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                    dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
                    ds.PushIntoStack(dsItem);
                }
                //防方阵形
                ds.PushIntoStack(process.ProcessContainer.DefenseList.Count);
                foreach (CombatEmbattle combatEmbattle in process.ProcessContainer.DefenseList)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(combatEmbattle.GeneralID);
                    dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                    dsItem.PushIntoStack(combatEmbattle.HeadID.ToNotNullString());
                    dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                    dsItem.PushIntoStack(combatEmbattle.LiveNum);
                    dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                    dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                    dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                    dsItem.PushIntoStack(combatEmbattle.AbilityID);
                    dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                    dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
                    ds.PushIntoStack(dsItem);
                }
                //战斗过程
                ds.PushIntoStack(process.ProcessContainer.ProcessList.Count);
                foreach (CombatProcess combatProcess in process.ProcessContainer.ProcessList)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(combatProcess.GeneralID);
                    dsItem.PushIntoStack(combatProcess.LiveNum);
                    dsItem.PushIntoStack(combatProcess.Momentum);
                    dsItem.PushIntoStack(combatProcess.AttackTaget.ToShort());
                    dsItem.PushIntoStack(combatProcess.AttackUnit.ToShort());
                    dsItem.PushIntoStack(combatProcess.AbilityProperty.ToShort());
                    dsItem.PushIntoStack(combatProcess.AttStatus.ToShort());
                    dsItem.PushIntoStack(combatProcess.DamageNum);
                    dsItem.PushIntoStack(combatProcess.AttEffectID.ToNotNullString());
                    dsItem.PushIntoStack(combatProcess.TargetEffectID.ToNotNullString());
                    dsItem.PushIntoStack(combatProcess.IsMove.ToShort());
                    dsItem.PushIntoStack(combatProcess.Position.ToShort());
                    dsItem.PushIntoStack(combatProcess.Role.ToShort());

                    dsItem.PushIntoStack(combatProcess.DamageStatusList.Count);
                    foreach (AbilityEffectStatus effectStatus in combatProcess.DamageStatusList)
                    {
                        DataStruct dsItem2 = new DataStruct();
                        dsItem2.PushIntoStack(effectStatus.AbilityType.ToShort());
                        dsItem2.PushIntoStack(effectStatus.DamageNum);
                        dsItem2.PushIntoStack(effectStatus.IsIncrease ? 1 : 0);

                        dsItem.PushIntoStack(dsItem2);
                    }

                    dsItem.PushIntoStack(combatProcess.TargetList.Count);
                    foreach (TargetProcess targetProcess in combatProcess.TargetList)
                    {
                        DataStruct dsItem1 = new DataStruct();
                        dsItem1.PushIntoStack(targetProcess.GeneralID);
                        dsItem1.PushIntoStack(targetProcess.LiveNum);
                        dsItem1.PushIntoStack(targetProcess.Momentum);
                        dsItem1.PushIntoStack(targetProcess.DamageNum);
                        dsItem1.PushIntoStack(targetProcess.IsShanBi.ToShort());
                        dsItem1.PushIntoStack(targetProcess.IsGeDang.ToShort());
                        dsItem1.PushIntoStack(targetProcess.IsBack.ToShort());
                        dsItem1.PushIntoStack(targetProcess.IsMove.ToShort());
                        dsItem1.PushIntoStack(targetProcess.BackDamageNum);
                        dsItem1.PushIntoStack(targetProcess.TargetStatus.ToShort());
                        dsItem1.PushIntoStack(targetProcess.Position.ToShort());
                        dsItem1.PushIntoStack(targetProcess.Role.ToShort());
                        //目标中招效果
                        dsItem1.PushIntoStack(targetProcess.DamageStatusList.Count);
                        foreach (AbilityEffectStatus effectStatus in targetProcess.DamageStatusList)
                        {
                            DataStruct dsItem12 = new DataStruct();
                            dsItem12.PushIntoStack(effectStatus.AbilityType.ToShort());
                            dsItem12.PushIntoStack(effectStatus.IsIncrease ? 1 : 0);

                            dsItem1.PushIntoStack(dsItem12);
                        }
                        dsItem1.PushIntoStack(targetProcess.IsBaoji.ToShort());
                        dsItem1.PushIntoStack(targetProcess.TrumpStatusList.Count);
                        foreach (var item in targetProcess.TrumpStatusList)
                        {
                            DataStruct dsItem13 = new DataStruct();
                            dsItem13.PushIntoStack(item.AbilityID);
                            dsItem13.PushIntoStack(item.Num);
                            dsItem1.PushIntoStack(dsItem13);
                        }
                        dsItem.PushIntoStack(dsItem1);
                    }
                    dsItem.PushIntoStack(combatProcess.TrumpStatusList.Count);
                    foreach (var item in combatProcess.TrumpStatusList)
                    {
                        DataStruct dsItem14 = new DataStruct();
                        dsItem14.PushIntoStack(item.AbilityID);
                        dsItem14.PushIntoStack(item.Num);
                        dsItem.PushIntoStack(dsItem14);
                    }
                    ds.PushIntoStack(dsItem);
                }
                PushIntoStack(ds);
            }
            PushIntoStack(status.ToShort());
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Version", ref version))
            {
                httpGet.GetString("SportID", ref sportID);
                return true;
            }
            return true;
        }

        public override bool TakeAction()
        {
            status = ContextUser.UserStatus;
            if (!string.IsNullOrEmpty(sportID))
            {
                MemberGroup memberGroup = new ShareCacheStruct<MemberGroup>().FindKey(sportID);
                if (memberGroup != null && !memberGroup.IsRemove)
                {
                    isWin = memberGroup.Win ? (short)1 : (short)0;
                    processList.Add(memberGroup.CombatProcess);
                }
            }
            else
            {
                DateTime nextDate;
                FightStage stage = GuildFightCombat.GetStage(out nextDate);
                if (stage == FightStage.quarter_final || stage == FightStage.semi_final || stage == FightStage.final)
                {
                    processList = GuildFightCombat.GetCombatProcess(ContextUser.UserID, stage);                   
                }
            }
            return true;
        }


    }
}
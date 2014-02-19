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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 4211_多人副本战斗详情接口
    /// </summary>
    public class Action4211 : BaseAction
    {
        private PlotInfo plotInfo = null;
        private MorePlotTeam teamPlot;
        private int plotID = 0;
        private int teamID = 0;
        private TeamCombatResult teamCombatResult = new TeamCombatResult();

        public Action4211(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4211, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(teamPlot.MorePlot.Experience);
            PushIntoStack(teamPlot.MorePlot.ItemId);
            PushIntoStack(teamPlot.MorePlot.ItemName.ToNotNullString());
            PushIntoStack(teamPlot.MorePlot.ItemNum);
            PushIntoStack(teamPlot.MorePlot.ExpNum);
            PushIntoStack(teamPlot.MorePlot.PlotID);
            PushIntoStack(teamPlot.MorePlot.PlotName.ToNotNullString());
            PushIntoStack(plotInfo == null ? string.Empty : plotInfo.BgScene.ToNotNullString());
            PushIntoStack(teamCombatResult.IsWin ? (short)1 : (short)0);
            PushIntoStack(teamCombatResult.ProcessList.Count);
            foreach (var countryProcess in teamCombatResult.ProcessList)
            {
                DataStruct ds = new DataStruct();
                //攻方阵形
                ds.PushIntoStack(countryProcess.ProcessContainer.AttackList.Count);
                foreach (CombatEmbattle combatEmbattle in countryProcess.ProcessContainer.AttackList)
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
                ds.PushIntoStack(countryProcess.ProcessContainer.DefenseList.Count);
                foreach (CombatEmbattle combatEmbattle in countryProcess.ProcessContainer.DefenseList)
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
                ds.PushIntoStack(countryProcess.ProcessContainer.ProcessList.Count);
                foreach (CombatProcess combatProcess in countryProcess.ProcessContainer.ProcessList)
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
                        DataStruct dsItem1 = new DataStruct();
                        dsItem1.PushIntoStack(effectStatus.AbilityType.ToShort());
                        dsItem1.PushIntoStack(effectStatus.DamageNum);
                        dsItem1.PushIntoStack(effectStatus.IsIncrease ? 1 : 0);

                        dsItem.PushIntoStack(dsItem1);
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
                            dsItem13.PushIntoStack((short)item.AbilityID);
                            dsItem13.PushIntoStack(item.Num);
                            dsItem1.PushIntoStack(dsItem13);
                        }
                        dsItem.PushIntoStack(dsItem1);
                    }
                    dsItem.PushIntoStack(combatProcess.TrumpStatusList.Count);
                    foreach (var item in combatProcess.TrumpStatusList)
                    {
                        DataStruct dsItem14 = new DataStruct();
                        dsItem14.PushIntoStack((short)item.AbilityID);
                        dsItem14.PushIntoStack(item.Num);
                        dsItem.PushIntoStack(dsItem14);
                    }
                    ds.PushIntoStack(dsItem);
                }

                this.PushIntoStack(ds);

            }
            PushIntoStack(plotInfo != null ? plotInfo.BgScene.ToNotNullString() : string.Empty);
            PushIntoStack(plotInfo != null ? plotInfo.FgScene.ToNotNullString() : string.Empty);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotID", ref plotID)
                && httpGet.GetInt("TeamID", ref teamID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var plotTeam = new PlotTeamCombat(ContextUser);
            teamCombatResult = plotTeam.GetCombatProcess(teamID);
            teamPlot = plotTeam.GetTeam(teamID);
            plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);

            if (teamCombatResult.IsWin)
            {
                //日常任务-多人副本
                TaskHelper.TriggerDailyTask(Uid, 4007);

                if (ContextUser.UserExtend != null && ContextUser.UserExtend.DailyInfo != null)
                {
                    if (ContextUser.UserExtend.DailyInfo.RefreshDate.Date == DateTime.Now.Date)
                    {
                        ContextUser.UserExtend.DailyInfo.MoreNum = MathUtils.Addition(ContextUser.UserExtend.DailyInfo.MoreNum, 1);
                    }
                    else
                    {
                        ContextUser.UserExtend.DailyInfo.RefreshDate = DateTime.Now;
                        ContextUser.UserExtend.DailyInfo.MoreNum = 1;
                    }
                }
                else if (ContextUser.UserExtend != null)
                {
                    DailyRefresh dailyRefresh = new DailyRefresh();
                    dailyRefresh.RefreshDate = DateTime.Now;
                    dailyRefresh.MoreNum = 1;
                    ContextUser.UserExtend.DailyInfo = dailyRefresh;
                }
                //ContextUser.Update();
            }

            return true;
        }
    }
}
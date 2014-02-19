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
using System.Diagnostics;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6105_Boss战战斗详情接口
    /// </summary>
    public class Action6105 : BaseAction
    {
        private BossCombatProcess bossProcess = new BossCombatProcess();
        private bool IsWin;
        private int _activeId;

        public Action6105(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6105, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(IsWin.ToShort());
            PushIntoStack(bossProcess.DamageNum);
            PushIntoStack(bossProcess.ObtainNum);
            PushIntoStack(bossProcess.GameCoin);
            PushIntoStack(bossProcess.KillGameCoin);

            //攻方阵形
            PushIntoStack(bossProcess.ProcessContainer.AttackList.Count);
            foreach (CombatEmbattle combatEmbattle in bossProcess.ProcessContainer.AttackList)
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
                PushIntoStack(dsItem);
            }
            //防方阵形
            PushIntoStack(bossProcess.ProcessContainer.DefenseList.Count);
            foreach (CombatEmbattle combatEmbattle in bossProcess.ProcessContainer.DefenseList)
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
                PushIntoStack(dsItem);
            }
            //战斗过程
            PushIntoStack(bossProcess.ProcessContainer.ProcessList.Count);
            foreach (CombatProcess combatProcess in bossProcess.ProcessContainer.ProcessList)
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
                PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ActiveId", ref _activeId))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (!string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                GuildBossCombat bossCombat = new GuildBossCombat(ContextUser.MercenariesID);
                UserGuild guild = bossCombat.UserGuild;
                if (guild != null)
                {
                    CombatStatus combatStatus = guild.CombatStatus;
                    if (combatStatus == CombatStatus.Killed || CombatHelper.GuildBossKill(ContextUser.MercenariesID))
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St5405_BossKilled;
                        return false;
                    }
                    else if (combatStatus == CombatStatus.Wait)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St5405_CombatWait;
                        return false;
                    }
                    else if (combatStatus == CombatStatus.Over)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St5405_CombatOver;
                        return false;
                    }
                    else if (combatStatus == CombatStatus.Combat)
                    {
                        var cuser = bossCombat.GetCombatUser(Uid);
                        if (cuser != null && cuser.CodeTime > 0)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St5402_IsReliveError;
                            return false;
                        }
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        IsWin = bossCombat.Combat(ContextUser, bossProcess);
                        stopwatch.Stop();
                        new BaseLog().SaveLog("公会boss战斗所需时间:" + stopwatch.Elapsed.TotalMilliseconds + "ms");

                    }
                    else
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St5402_CombatNoStart;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
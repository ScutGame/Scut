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
using System.Collections.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 5405_Boss战战斗详情接口
    /// </summary>
    public class Action5405 : BaseAction
    {
        private static readonly object thisLock = new object();
        private BossCombatProcess bossProcess = new BossCombatProcess();
        private List<SelfAbilityEffect> selfAbilityEffectList = new List<SelfAbilityEffect>();
        private bool IsWin;
        private int _activeId;
        private int _userTalPriority = 0;
        private int _npcTalPriority = 0;
        private ConfigCacheSet<GeneralInfo> _cacheSetGeneral = new ConfigCacheSet<GeneralInfo>();

        public Action5405(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5405, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(IsWin.ToShort());
            this.PushIntoStack(bossProcess.DamageNum);
            this.PushIntoStack(bossProcess.ObtainNum);
            this.PushIntoStack(bossProcess.GameCoin);
            this.PushIntoStack(bossProcess.KillGameCoin);

            //攻方阵形
            this.PushIntoStack(bossProcess.ProcessContainer.AttackList.Count);
            foreach (CombatEmbattle combatEmbattle in bossProcess.ProcessContainer.AttackList)
            {
                var general = _cacheSetGeneral.FindKey(combatEmbattle.GeneralID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(combatEmbattle.GeneralID);
                dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                dsItem.PushIntoStack(general == null ? string.Empty : general.BattleHeadID.ToNotNullString());
                //dsItem.PushIntoStack(combatEmbattle.BattleHead.ToNotNullString());
                dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                dsItem.PushIntoStack(combatEmbattle.LiveNum);
                dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                dsItem.PushIntoStack(combatEmbattle.AbilityID);
                dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
                // 增加佣兵品质
                dsItem.PushShortIntoStack(general == null ? 0 : general.GeneralQuality.ToShort());
                this.PushIntoStack(dsItem);
            }
            //防方阵形
            this.PushIntoStack(bossProcess.ProcessContainer.DefenseList.Count);
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
                // 增加佣兵品质
                var general = _cacheSetGeneral.FindKey(combatEmbattle.GeneralID);
                dsItem.PushShortIntoStack(general == null ? 0 : general.GeneralQuality.ToShort());
                this.PushIntoStack(dsItem);
            }
            //战斗过程
            this.PushIntoStack(bossProcess.ProcessContainer.ProcessList.Count);
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
                dsItem.PushIntoStack(combatProcess.FntHeadID.ToNotNullString());
                this.PushIntoStack(dsItem);
            }
            PushIntoStack(selfAbilityEffectList.Count);
            foreach (var selfAbilityEffect in selfAbilityEffectList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(selfAbilityEffect.GeneralID);
                dsItem.PushIntoStack(selfAbilityEffect.EffectID1.ToNotNullString());
                dsItem.PushIntoStack(selfAbilityEffect.FntHeadID.ToNotNullString());
                dsItem.PushIntoStack(selfAbilityEffect.IsIncrease ? 1.ToShort() : 0.ToShort());
                dsItem.PushIntoStack(selfAbilityEffect.Position);
                dsItem.PushIntoStack(selfAbilityEffect.Role.ToInt());
                PushIntoStack(dsItem);
            }
            PushIntoStack(_userTalPriority);
            PushIntoStack(_npcTalPriority);
           
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
            BossCombat bossCombat = new BossCombat(_activeId);
            GameActive gameActive = bossCombat.GameActive;
            CombatStatus combatStatus = gameActive.RefreshStatus();
            if (combatStatus == CombatStatus.Killed || CombatHelper.IsBossKill(_activeId))
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St5405_BossKilled;
                return false;
            }
            else if (combatStatus == CombatStatus.Wait)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St5405_CombatWait;
                return false;
            }
            else if (combatStatus == CombatStatus.Over)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St5405_CombatOver;
                return false;
            }
            else if (combatStatus == CombatStatus.Combat)
            {
                var cuser = bossCombat.GetCombatUser(Uid);
                if (cuser != null && cuser.CodeTime > 0)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St5402_IsReliveError;
                    return false;
                }
                var boss = bossCombat.Boss;
                if (boss.IsOver)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St5405_BossKilled;
                    return false;
                }
                lock (thisLock)
                {
                    if (boss.IsOver)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St5405_BossKilled;
                        return false;
                    }
                    IsWin = bossCombat.Combat(ContextUser, bossProcess);
                }
                _userTalPriority = CombatHelper.TotalPriorityNum(ContextUser.UserID, 0);
                _npcTalPriority = 0;
            }
            else
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St5402_CombatNoStart;
                return false;
            }
            selfAbilityEffectList = UserAbilityHelper.GetSelfAbilityEffectList(ContextUser.UserID, 0);
            return true;
        }
    }
}
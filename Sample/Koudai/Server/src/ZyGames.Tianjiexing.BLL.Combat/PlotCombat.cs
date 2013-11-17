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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Combat;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 取攻击目标委托
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public delegate EmbattleQueue TargetHandle(EmbattleRole role);

    /// <summary>
    /// 副本战斗
    /// </summary>
    public class PlotCombat : ISingleCombat
    {
        private CombatType _combatType;
        private static int MaxCount = ConfigEnvSet.GetInt("CombatMaxBout") > 0 ? ConfigEnvSet.GetInt("CombatMaxBout") : 20;
        private CombatProcessContainer processContainer = new CombatProcessContainer();
        private TargetHandle targerHandle;

        public PlotCombat(object combatType)
        {
            _combatType = (CombatType)combatType;
            targerHandle = new TargetHandle(TargetEmbattle);
        }

        private List<EmbattleQueue> priorityList = new List<EmbattleQueue>();

        #region ISingleCombat 成员

        /// <summary>
        /// 回合数
        /// </summary>
        public int BoutNum
        {
            get;
            private set;
        }

        public bool Doing()
        {
            List<EmbattleQueue> queueList = GetPriorityQueue();
            if (queueList.Count != 2) return false;
            EmbattleQueue roleA = queueList[0];
            EmbattleQueue roleD = queueList[1];
            //原因:空阵BUG
            if (roleA.GeneralCount == 0 && roleD.GeneralCount == 0)
            {
                new BaseLog().SaveLog("战斗双方佣兵不存在");
                return false;
            }
            if (roleA.Role == EmbattleRole.RoleA)
            {
                if (roleA.GeneralCount > 0 && roleD.GeneralCount == 0)
                {
                    return true;
                }
                if (roleA.GeneralCount == 0 && roleD.GeneralCount > 0)
                {
                    return false;
                }
            }
            else if (roleA.Role == EmbattleRole.RoleD)
            {
                if (roleA.GeneralCount > 0 && roleD.GeneralCount == 0)
                {
                    return false;
                }
                if (roleA.GeneralCount == 0 && roleD.GeneralCount > 0)
                {
                    return true;
                }
            }

            queueList.ForEach(item =>
            {
                var general = (CombatGeneral)item.NextGeneral();

                if (general != null)
                {
                    general.SelfAbilityInfoList.ForEach(SelfAbility =>
                    {
                        SelfAbilityEffect selfAbilityEffect = new SelfAbilityEffect();
                        selfAbilityEffect.GeneralID = general.GeneralID;
                        selfAbilityEffect.EffectID1 = SelfAbility.EffectID1;
                        selfAbilityEffect.FntHeadID = SelfAbility.FntHeadID;
                        selfAbilityEffect.IsIncrease = SelfAbility.IsIncrease;
                        selfAbilityEffect.Position = general.Position;
                        selfAbilityEffect.Role = item.Role.ToShort();
                        processContainer.SelfAbilityEffectList.Add(selfAbilityEffect);
                    });
                }
            });
            //原因：闪避高时死锁
            bool isCombatOut = false;
            int index = 0;
            while (roleA.HasCombat() && roleD.HasCombat())
            {
                if (index >= MaxCount)
                {
                    isCombatOut = true;
                    //防止死锁
                    new BaseLog().SaveLog(string.Format("[{0}]战斗超出最大回合数，判攻方输", _combatType));
                    break;
                }
                //设置重新一轮
                roleA.ResetGeneralQueue();
                roleD.ResetGeneralQueue();

                DoSingle(queueList);

                index++;
            }

            foreach (var item in priorityList)
            {
                if (item.Number > BoutNum) BoutNum = item.Number;
                //恢复血量
                double resumeLife = ConfigEnvSet.GetDouble("Combat.ResumeLifeNum");
                //领土战直接恢复100%
                if (_combatType == CombatType.Country && item.IsOver)
                {
                    resumeLife = 1;
                }

                if ((_combatType == CombatType.Country && item.IsOver) ||
                    (_combatType != CombatType.Country && item.IsOver))
                {
                    if (item.Role == EmbattleRole.RoleA)
                    {
                        DoResumeLife(processContainer.AttackList, resumeLife);
                    }
                    else
                    {
                        DoResumeLife(processContainer.DefenseList, resumeLife);
                    }

                }
            };
            //超出判断攻方输
            if (isCombatOut && roleA.Role == EmbattleRole.RoleA)
            {
                return false;
            }
            if (isCombatOut && roleA.Role == EmbattleRole.RoleD)
            {
                return true;
            }
            return !priorityList.Find(m => m.Role == EmbattleRole.RoleA).IsOver;
        }

        /// <summary>
        /// 处理单轮
        /// </summary>
        private void DoSingle(List<EmbattleQueue> queueList)
        {
            int index = 0;
            while (true)
            {
                if (index >= queueList.Count) index = 0;
                EmbattleQueue item = queueList[index];
                EmbattleQueue tagetEmbattle = queueList[(index == 0 ? 1 : 0)];

                if (!item.HasGeneral && !tagetEmbattle.HasGeneral)
                {
                    //当前轮双方阵列都打完才结束
                    break;
                }
                if (!tagetEmbattle.HasCombat())
                {
                    //对方阵列是否可战斗,原因：目标为空时，佣兵被跳过
                    break;
                }
                var general = (CombatGeneral)item.NextGeneral();

                if (general != null)
                {
                    CombatCenter combatCenter = new CombatCenter(item.Role, general, tagetEmbattle.Role, targerHandle);
                    if (combatCenter.HasDoing())
                    {
                        var combatLog = combatCenter.GetProcess();
                        if (combatLog != null)
                        {
                            processContainer.ProcessList.Add(combatLog);
                        }
                    }
                    else
                    {
                        TraceLog.ReleaseWriteFatal("General:{0}-{1}佣兵被跳过", general.GeneralName, general.GeneralID);
                        //是否结束
                        break;
                    }

                }
                index++;
            }
        }

        /// <summary>
        /// 恢复血量
        /// </summary>
        /// <param name="userEmbattleList"></param>
        private void DoResumeLife(CacheList<CombatEmbattle> userEmbattleList, double resumeLife)
        {
            foreach (CombatEmbattle combatEmbattle in userEmbattleList)
            {
                if (!string.IsNullOrEmpty(combatEmbattle.UserID))
                {
                    var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(combatEmbattle.UserID, combatEmbattle.GeneralID);
                    if (userGeneral != null && userGeneral.IsOver)
                    {
                        userGeneral.LifeNum = (int)Math.Floor(combatEmbattle.LiveMaxNum * resumeLife);
                        if (userGeneral.ReplacePosition > 0)
                        {
                            userGeneral.ResetEmbatleReplace();
                        }
                        //userGeneral.Update();
                    }
                }
            }
        }

        private List<EmbattleQueue> GetPriorityQueue()
        {
            //根据先攻值排先攻方
            priorityList.QuickSort((a, b) =>
            {
                var result = 0;
                if (a == null && b == null) return 0;
                if (a != null && b == null) return 1;
                if (a == null) return -1;

                if (a.PriorityNum > b.PriorityNum)
                {
                    result = -1;
                }
                else if (a.PriorityNum < b.PriorityNum)
                {
                    result = 1;
                }
                else
                {
                    if (a.Role < b.Role)
                    {
                        result = -1;
                    }
                    else if (a.Role > b.Role)
                    {
                        result = 1;
                    }
                }
                return result;
            });
            return priorityList;
        }

        internal EmbattleQueue TargetEmbattle(EmbattleRole role)
        {
            return priorityList.Find(m => m.Role == role);
        }

        public object GetProcessResult()
        {
            return processContainer;
        }

        public void SetAttack(EmbattleQueue combatGrid)
        {
            combatGrid.Role = EmbattleRole.RoleA;
            priorityList.Add(combatGrid);
            SetEmbattle(combatGrid, processContainer.AttackList);
        }

        public void SetDefend(EmbattleQueue combatGrid)
        {
            combatGrid.Role = EmbattleRole.RoleD;
            priorityList.Add(combatGrid);
            SetEmbattle(combatGrid, processContainer.DefenseList);
        }

        private void SetEmbattle(EmbattleQueue combatGrid, CacheList<CombatEmbattle> list)
        {
            IGeneral[] generalList = combatGrid.FindAll(true);
            foreach (IGeneral general in generalList)
            {
                if (general != null && general is CombatGeneral)
                {
                    CombatGeneral cgeneral = (CombatGeneral)general;
                    list.Add(new CombatEmbattle
                    {
                        UserID = cgeneral.UserID,
                        GeneralID = cgeneral.GeneralID,
                        GeneralName = cgeneral.GeneralName,
                        GeneralLv = cgeneral.Lv,
                        HeadID = cgeneral.HeadID,
                        AbilityID = cgeneral.TempAbility == null ? 0 : cgeneral.TempAbility.AbilityID,
                        LiveNum = cgeneral.LifeNum,
                        LiveMaxNum = cgeneral.LifeMaxNum,
                        MomentumNum = cgeneral.Momentum,
                        MaxMomentumNum = CombatGeneral.MomentumOut,
                        Position = cgeneral.Position,
                        IsWait = cgeneral.IsWait,
                        BattleHead =  cgeneral.BattleHeadID
                    });
                }
            }
            if (list.Count == 0)
            {
                new BaseLog().SaveDebugLog("战斗异常，未能加载佣兵数据");
            }
        }

        #endregion
    }
}
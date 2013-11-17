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
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Component;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 战斗攻击抽象类
    /// </summary>
    public abstract class CombatBaseAttack
    {
        /// <summary>
        /// 战斗爆击伤害加成
        /// </summary>
        public decimal BaojiAttack = ConfigEnvSet.GetDecimal("Combat.BaojiAttack");
        /// <summary>
        /// 战斗格挡伤害或反击效果
        /// </summary>
        public decimal GedangAttackPercent = ConfigEnvSet.GetDecimal("Combat.GedangAttackPercent");
        /// <summary>
        /// 战斗普通攻击加25点气势
        /// </summary>
        public short MomentumAttack = (short)ConfigEnvSet.GetInt("Combat.MomentumAttack");
        /// <summary>
        /// 战斗被击中增加25点气势
        /// </summary>
        public short MomentumDefense = (short)ConfigEnvSet.GetInt("Combat.MomentumDefense");
        /// <summary>
        /// 战斗产生绝对防御时伤害值
        /// </summary>
        public int JueduiDefenseDamage = ConfigEnvSet.GetInt("Combat.JueduiDefenseDamage");

        private bool isharm = false;
        private bool isPyharm = false;

        /// <summary>
        /// 检查攻击方式
        /// </summary>
        /// <returns></returns>
        public static CombatBaseAttack CheckAttack(CombatGeneral general)
        {
            //return !general.IsHunluanStatus && general.IsRelease
            //           ? (CombatBaseAttack)new CombatAbilityAttack(general)
            //           : new CombatPhysicalAttack(general);
            if(!general.IsHunluanStatus && general.IsRelease)
            {
                general.IsSelfAbility = 1;
                return (CombatBaseAttack) new CombatAbilityAttack(general);
            }
            else
            {
                var ability = AbilityDispose.TriggerAbilityList(general.AbilityInfoList);
                if (ability!=null)
                {
                    general.IsSelfAbility = 2;
                    general.TriggerAbility = ability;
                    return (CombatBaseAttack)new CombatAbilityAttack(general);
                }
                else
                {
                    return new CombatPhysicalAttack(general);
                }
                
            }
        }

        public CombatBaseAttack(CombatGeneral general)
        {
            General = general;
            ProcessLog = new CombatProcess();
            ProcessLog.GeneralID = general.GeneralID;
            ProcessLog.Position = general.Position.ToShort();
            ProcessLog.LiveNum = general.LifeNum;
            ProcessLog.Momentum = general.Momentum;
            ProcessLog.AttStatus = general.BattleStatus;
        }


        protected abstract AttackUnit GetAttackUnit();

        protected abstract AttackTaget GetAttackTaget();

        /// <summary>
        /// 攻击
        /// </summary>
        public abstract void Attack();

        public void  ProccessExtendAttack()
        {
            
        }

        /// <summary>
        /// 攻击将领
        /// </summary>
        public CombatGeneral General
        {
            get;
            private set;
        }

        /// <summary>
        /// 目标角色
        /// </summary>
        public EmbattleRole TagetRole
        {
            get;
            set;
        }
        /// <summary>
        /// 目标将领
        /// </summary>
        public CombatGeneral TagetGeneral
        {
            get;
            set;
        }

        public CombatProcess ProcessLog
        {
            get;
            protected set;
        }

        public TargetProcess TargetProcess { get; protected set; }

        /// <summary>
        /// 攻击之前处理，判断上一轮产生的伤害
        /// </summary>
        /// <returns>是否能够功击</returns>
        public bool BeforeAttack()
        {
            bool isBattle = true;
            //处理持续回合状态技能
          
            General.ForeachAbilityStatus(effectStatus =>
            {
                if (effectStatus.BoutNum > 0)
                {
                    effectStatus.BoutNum = MathUtils.Subtraction(effectStatus.BoutNum, 1, 0);
                }
                //静默状态
                if (effectStatus.IsSilentStatus)
                {
                    //轮空一次
                    isBattle = false;
                }
                //持续伤害
                else if (effectStatus.IsInjuredStatus)
                {
                    int damageNum = effectStatus.DamageNum;
                    General.TriggerDamageNum(damageNum);
                }
                else if (effectStatus.AbilityType == AbilityType.Jueduifangyu)
                {
                }

                //扣除回合数
                if (!effectStatus.IsRemove)
                {
                    //增减技能不加入
                    if (effectStatus.TotalBoutNum == effectStatus.BoutNum ||
                        (!effectStatus.IsZengyi && effectStatus.AbilityType != AbilityType.Qishi))
                    {
                        if (effectStatus.BoutNum > 0)
                        {
                            ProcessLog.DamageStatusList.Add(effectStatus.Copy());
                        }
                    }
                }
            });
            if (General.IsOver)
            {
                General.BattleStatus = BattleStatus.Over;
                isBattle = false;
            }
            return isBattle;
        }

        public void AfterAttack()
        {
            ProcessLog.LiveNum = General.LifeNum;
            ProcessLog.Momentum = General.Momentum;
            ProcessLog.AttStatus = General.BattleStatus;
        }

        /// <summary>
        /// 获取攻击目标阵营
        /// </summary>
        /// <param name="targerHandle"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public CombatGeneral[] GetTargetRoleUnit(TargetHandle targerHandle, EmbattleRole role)
        {

            AttackTaget attackTaget;
            AttackUnit attackUnit = GetAttackUnit();
            if (General.IsHunluanStatus)
            {
                attackTaget = AttackTaget.Friendly;
            }
            else
            {
                attackTaget = GetAttackTaget();
            }

            ProcessLog.AttackUnit = attackUnit;
            ProcessLog.AttackTaget = attackTaget;

            EmbattleQueue attackEmbattle;
            switch (attackTaget)
            {
                case AttackTaget.Self:
                case AttackTaget.Friendly:
                    attackEmbattle = targerHandle(role);
                    break;
                case AttackTaget.Enemy:
                    attackEmbattle = targerHandle(role == EmbattleRole.RoleA ? EmbattleRole.RoleD : EmbattleRole.RoleA);
                    break;
                default:
                    throw new Exception("未能找到攻击目标");
                //break;
            }

            TagetRole = attackEmbattle.Role;

            if (General.IsHunluanStatus)
            {
                var rgeneral = (CombatGeneral)attackEmbattle.RandomGeneral();
                return new[] { rgeneral };
            }
            return GetTargetUnit(General.Position, attackUnit, attackTaget, attackEmbattle);
        }

        /// <summary>
        /// 计算技能伤害公式,不产生负数
        /// </summary>
        /// <param name="tagetGeneral"></param>
        /// <returns></returns>
        protected int GetAbilityDamageNum(CombatGeneral tagetGeneral)
        {
            string traceStr = string.Empty;
            int damageNum;
            int attackNum;
            int defenseNum = 0;

            decimal harmNum = 0;  //法宝属相克制伤害 --技能攻击
            AbilityProperty property = General.AbilityProperty;

            ProcessLog.AbilityProperty = property;
            attackNum = General.GetAttackNum(AbilityProperty.Ability) + General.GetAttackNum(property);
            if (tagetGeneral != null)
            {
                //法宝属相克制伤害 --技能攻击
                harmNum = TrumpAbilityAttack.TrumpZodiacHarm(General, tagetGeneral.UserID);

                defenseNum = tagetGeneral.GetDefenseNum(AbilityProperty.Ability, tagetGeneral) + tagetGeneral.GetDefenseNum(property, tagetGeneral);
            }
            General.MinDamageNum = (int)Math.Ceiling(attackNum * ConfigEnvSet.GetDouble("Combat.MinDamagePencent"));
            damageNum = MathUtils.Subtraction(attackNum, defenseNum, General.MinDamageNum);

            decimal ratioNum = General.TempAbility.RatioNum > 0 ? General.TempAbility.RatioNum : 1;
            damageNum = (int)Math.Floor(damageNum * ratioNum); //计算伤害系数
            //气势加成=气势/{100-（气势-100）*2/3}*100% 
            if (General != null)
            {
                var qishiNum = General.Momentum == 0 ? (short)100 : General.Momentum;
                traceStr += "qishiNum:" + qishiNum;
                decimal qishiPercent = (decimal)qishiNum / 100;
                traceStr += "qishiPercent:" + qishiNum;
                damageNum = (int)Math.Floor(damageNum * qishiPercent);
                traceStr += "damageNum:" + qishiNum;
                //加固定伤害
                damageNum = MathUtils.Addition(damageNum, General.FixedDamageNum, int.MaxValue);
                traceStr += "FixedDamageNum:" + qishiNum;


            }
            //技能伤害，负数是增加，
            //damageNum = General != null && General.Ability.IsIncrease ? -damageNum : damageNum;
            if (damageNum == 0)
            {
                new BaseLog().SaveLog(new Exception("释放技能：" + General.TempAbility.AbilityName + General.TempAbility.AbilityID + "伤害为0,Trace:" + traceStr));
            }
            if (damageNum < General.MinDamageNum)
            {
                damageNum = General.MinDamageNum;
            }
            //法宝属相克制伤害 --技能攻击
            if (!isharm && harmNum > 0)
            {
                damageNum = (int)(damageNum * MathUtils.Addition(1, harmNum));
                isharm = true;
            }
            //魂技等级与自身魂技加成

            int abilityID = General.TempAbility != null ? General.TempAbility.AbilityID : 0;
            decimal effectValue = AbilityDispose.GetAbilityEffect(General.UserID, General.GeneralID, abilityID);
            if (effectValue > 0)
            {
                damageNum = MathUtils.ToCeilingInt(damageNum * MathUtils.Addition(1, (effectValue)));
            }
            return damageNum;
        }
        /// <summary>
        /// 计算伤害公式
        /// 攻击伤害 = 攻击力 - 物理防御力
        /// </summary>
        /// <param name="tagetGeneral">加血等辅助技能时可能为空值</param>
        /// <returns></returns>
        protected int GetPhysicalDamageNum(CombatGeneral tagetGeneral)
        {
            AbilityProperty property = General.AbilityProperty;
           
            ProcessLog.AbilityProperty = property;
            int damageNum;
            int attackNum;
            int defenseNum = 0;
            decimal harmNum = 0;  //法宝属相克制伤害 --技能攻击
            attackNum = General.GetAttackNum(property);
            if (tagetGeneral != null)
            {
                harmNum = TrumpAbilityAttack.TrumpZodiacHarm(General, tagetGeneral.UserID);
                defenseNum = tagetGeneral.GetDefenseNum(property, tagetGeneral);
                //普通攻击破防
                decimal normalAttackPoFang = TrumpAbilityAttack.GetEffect(General, AbilityType.NormalAttackPoFang);
                if (normalAttackPoFang > 0)
                {
                    ProcessLog.TrumpStatusList.Add(TrumpAbilityAttack.GetSkillprocess(AbilityType.NormalAttackPoFang, 0));
                }
                defenseNum = (int)Math.Floor(MathUtils.Subtraction(defenseNum, defenseNum * normalAttackPoFang));
            }
            General.MinDamageNum = (int)Math.Ceiling(attackNum * ConfigEnvSet.GetDouble("Combat.MinDamagePencent"));

            damageNum = MathUtils.Subtraction(attackNum, defenseNum, General.MinDamageNum);

            //加固定伤害
            damageNum = MathUtils.Addition(damageNum, General.FixedDamageNum, int.MaxValue);

            //法宝属相克制伤害 --技能攻击
            if (harmNum > 0 && !isPyharm)
            {
                damageNum = (int)(damageNum * MathUtils.Addition(1, (harmNum + General.EffectValue )));
                isPyharm = true;
            }
           
            return damageNum;
        }

        /// <summary>
        /// 目标防御处理
        /// </summary>
        /// <param name="damageNum">伤害值</param>
        /// <param name="targetProcess"></param>
        /// <param name="isAbility">是否是技能伤害</param>
        protected void DoTargetDefense(int damageNum, TargetProcess targetProcess, bool isAbility)
        {
            //城市公会争斗战疲劳值
            var fatigue = General.Fatigue * GameConfigSet.Fatigue;
            if (fatigue > 0 && General.UserStatus == UserStatus.FightCombat)
            {
                damageNum = (int)Math.Floor(damageNum * (MathUtils.Subtraction(1, fatigue)));
            }

            //生命低于x%，战力加成
            double inspire = (double)TrumpAbilityAttack.GetEffect(General, AbilityType.Furious);
            if (inspire > 0)
            {
                ProcessLog.TrumpStatusList.Add(TrumpAbilityAttack.GetSkillprocess(AbilityType.Furious, 0));
                damageNum = (int)Math.Floor(damageNum * (1 + inspire));
            }

            //鼓舞加成伤害
            if (General.InspirePercent > 0)
            {
                damageNum = (int)Math.Floor(damageNum * (1 + General.InspirePercent));
            }
          
            //是否有爆击
            decimal baojiPercent;
            bool isBaoji = General.TriggerBaojiPercent(TagetGeneral, out baojiPercent);
            ProcessLog.AddPercent(2, baojiPercent);
            if (isBaoji)
            {
                targetProcess.IsBaoji = true;
                damageNum = (int)Math.Floor(damageNum * (1 + BaojiAttack + General.BishaNum));
                //暴击加成
                decimal baojiJiaCheng = TrumpAbilityAttack.GetEffect(General, AbilityType.BaoJiJiaCheng);
                if (baojiJiaCheng > 1)
                {
                    ProcessLog.TrumpStatusList.Add(TrumpAbilityAttack.GetSkillprocess(AbilityType.BaoJiJiaCheng, 0));
                    damageNum = (int)Math.Floor(damageNum * baojiJiaCheng);
                }

                //被暴击减免
                decimal baoJiReduce = TrumpAbilityAttack.GetEffect(TagetGeneral, AbilityType.IsBaoJiReduce);
                if (baoJiReduce < 1 && baoJiReduce > 0)
                {
                    targetProcess.TrumpStatusList.Add(TrumpAbilityAttack.GetSkillprocess(AbilityType.IsBaoJiReduce, 0));
                    damageNum = (int)Math.Floor(damageNum * baoJiReduce);
                }

                if (damageNum >= 0)
                {
                    damageNum = MathUtils.Subtraction(damageNum, 0, General.MinDamageNum);
                }
                else
                {
                    damageNum = -MathUtils.Subtraction(damageNum, 0, General.MinDamageNum);
                }
                //爆击职业加成气势
                CareerAdditionInfo addition = new ConfigCacheSet<CareerAdditionInfo>().FindKey(General.CareerID, AbilityType.BaoJi);
                if (addition != null && !isAbility)
                {
                    General.Momentum = MathUtils.Addition(General.Momentum, addition.MomentumNum.ToShort(), short.MaxValue);
                }
            }

            //排除静默和混乱不能触发格档
            bool isTriggerGedang = TagetGeneral.IsSilentStatus || TagetGeneral.IsHunluanStatus;


            //先触发绝对防御  法宝破盾技能为开启时触发绝对防御
            if (TagetGeneral.IsJueduifangyuStatus)
            {
                if (TrumpAbilityAttack.AttackPoDun(General, AbilityType.AttackPoDun))
                {
                    ProcessLog.TrumpStatusList.Add(TrumpAbilityAttack.GetSkillprocess(AbilityType.AttackPoDun, 0));
                }
                else
                {
                    damageNum = JueduiDefenseDamage;
                }
            }
            else if (!isTriggerGedang)
            {
                //是否产生格挡[?]
                decimal gedangPercent;
                var fangjiNum = 0;
                var isGedang = TagetGeneral.TriggerGeDangPercent(General, out gedangPercent);
                ProcessLog.AddPercent(3, gedangPercent);
                if (isGedang)
                {
                    targetProcess.IsMove = TagetGeneral.IsAttrMove;
                    targetProcess.IsGeDang = true;
                    damageNum = (int)Math.Floor(damageNum * GedangAttackPercent);
                    if (damageNum >= 0)
                    {
                        damageNum = MathUtils.Subtraction(damageNum, 0, General.MinDamageNum);
                    }
                    else
                    {
                        damageNum = -MathUtils.Subtraction(damageNum, 0, General.MinDamageNum);
                    }
                    if (TagetGeneral.CareerType != CareerType.Mofashi &&
                        TagetGeneral.CareerType != CareerType.Shenqiangshou)
                    {
                        AbilityProperty property = General.AbilityProperty;
                        //普通攻击力的50%
                        int attackNum = TagetGeneral.GetAttackNum(property);
                        int defenseNum = General.GetDefenseNum(property, TagetGeneral);
                        int minDamageNum = (int)Math.Ceiling(attackNum * ConfigEnvSet.GetDouble("Combat.MinDamagePencent"));

                        fangjiNum = MathUtils.Subtraction(attackNum, defenseNum, 0);
                        fangjiNum = (int)Math.Floor(fangjiNum * GedangAttackPercent);
                        fangjiNum = MathUtils.Subtraction(fangjiNum, 0, minDamageNum);

                        //没死才可扣反击伤害
                        if (TagetGeneral.LifeNum > damageNum)
                        {
                            General.TriggerDamageNum(fangjiNum);
                            ProcessLog.DamageNum += fangjiNum;
                            ProcessLog.LiveNum = General.LifeNum;
                            ProcessLog.AttStatus = General.BattleStatus;
                            targetProcess.IsBack = true;
                            targetProcess.BackDamageNum = fangjiNum;
                        }
                    }

                    //格挡职业加成气势
                    if (!isAbility)
                    {
                        TagetGeneral.TriggerGeDang();
                    }
                }
            }
            //扣物理攻击伤害
            TagetGeneral.TriggerDamageNum(damageNum);
            targetProcess.DamageNum = damageNum;
            //解除静默状态
            TagetGeneral.RemoveSilentStatus();

            if (TagetGeneral.IsOver)
            {
                //复活
                decimal resurrect = TrumpAbilityAttack.GetEffect(TagetGeneral, AbilityType.Resurrect);
                if (resurrect > 0)
                {
                    targetProcess.TrumpStatusList.Add(TrumpAbilityAttack.GetSkillprocess(AbilityType.Resurrect, -(int)Math.Floor(TagetGeneral.LifeMaxNum * resurrect)));
                    TagetGeneral.LifeNum = (int)Math.Floor(TagetGeneral.LifeMaxNum * resurrect);
                }
                else
                {
                    TrumpAbilityAttack.GeneralOverTrumpLift(TagetGeneral.UserID, TagetGeneral.GeneralID);  //佣兵战斗死亡扣除N点寿命
                    targetProcess.TrumpStatusList = new List<SkillInfo>();
                }
            }

            //记录日志
            targetProcess.TargetStatus = TagetGeneral.BattleStatus;
            targetProcess.Momentum = TagetGeneral.Momentum;
            targetProcess.LiveNum = TagetGeneral.LifeNum;
            TargetProcess = targetProcess;
        }

        /// <summary>
        /// 获取攻击单位
        /// </summary>
        /// <param name="position">攻击方的位置</param>
        /// <param name="attackType">攻击方式</param>
        /// <param name="attackTaget">攻击目标</param>
        /// <param name="targetEmbattle"></param>
        private static CombatGeneral[] GetTargetUnit(int position, AttackUnit attackType, AttackTaget attackTaget, EmbattleQueue targetEmbattle)
        {
            IGeneral[] generalList = new IGeneral[0];
            List<CombatGeneral> targetGeneralList = new List<CombatGeneral>();
            switch (attackType)
            {
                case AttackUnit.Single:
                    CombatGeneral cg = null;
                    if (attackTaget == AttackTaget.Self)
                    {
                        cg = (CombatGeneral)targetEmbattle.GetGeneral(position);
                    }
                    else
                    {
                        cg = (CombatGeneral)targetEmbattle.FindGeneral(position);
                    }
                    if (cg != null)
                    {
                        generalList = new IGeneral[] { cg };
                    }
                    break;
                case AttackUnit.Horizontal:
                    generalList = targetEmbattle.FindHorizontal(position);
                    break;
                case AttackUnit.Vertical:
                    generalList = targetEmbattle.FindVertical(position);
                    break;
                case AttackUnit.All:
                    generalList = targetEmbattle.FindAll();
                    break;
                default:
                    break;
            }
            foreach (IGeneral general in generalList)
            {
                targetGeneralList.Add((CombatGeneral)general);
            }
            return targetGeneralList.ToArray();
        }

    }
}
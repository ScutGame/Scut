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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 技能攻击
    /// </summary>
    public class CombatAbilityAttack : CombatBaseAttack
    {
        private readonly AbilityInfo _ability;

        public CombatAbilityAttack(CombatGeneral general)
            : base(general)
        {
            if (general.IsSelfAbility==2)
            {
                _ability = General.TriggerAbility;
                General.TempAbility = General.TriggerAbility;
                
            }
            else
            {
                _ability = General.Ability;
                General.TempAbility = General.Ability;
            }

            general.IsSelfAbility = 0;

        }

        public override void Attack()
        {
            General.Momentum = 0;
            var targetProcess = new TargetProcess();
            targetProcess.GeneralID = TagetGeneral.GeneralID;
            targetProcess.Position = TagetGeneral.Position.ToShort();
            targetProcess.Role = TagetRole;
            targetProcess.LiveNum = TagetGeneral.LifeNum;
            targetProcess.Momentum = TagetGeneral.Momentum;

            ProcessLog.AttackUnit = _ability.AttackUnit;
            ProcessLog.AttackType = _ability.AttackType;
            ProcessLog.AttackTaget = _ability.AttackTaget;
            ProcessLog.AbilityProperty = _ability.AbilityProperty;
            ProcessLog.AttEffectID = _ability.EffectID1;
            ProcessLog.TargetEffectID = _ability.EffectID2;
            ProcessLog.FntHeadID = _ability.FntHeadID;
            ProcessLog.AbilityID = _ability.AbilityID;
            ProccessExtendAttack();
            DoAbilityStatus(targetProcess);

            TagetGeneral.ForeachAbilityStatus(effectStatus =>
            {
                //增减技能不加入
                if (effectStatus.TotalBoutNum == effectStatus.BoutNum ||
                    (!effectStatus.IsZengyi && effectStatus.AbilityType != AbilityType.Qishi))
                {
                    if (effectStatus.BoutNum > 0)
                    {
                        targetProcess.DamageStatusList.Add(effectStatus.Copy());
                    }
                }
            });
            targetProcess.TargetStatus = TagetGeneral.BattleStatus;
            
            ProcessLog.TargetList.Add(targetProcess);
            ProcessLog.Momentum = General.Momentum;
            ProcessLog.LiveNum = General.LifeNum;
            ProcessLog.AttStatus = General.BattleStatus;
            ProcessLog.IsMove = General.TempAbility.IsMove;
            string[] abilityTypeArray = _ability.AbilityType.Split(',');
            foreach (var s in abilityTypeArray)
            {

                General.Momentum = s.ToInt() == AbilityType.Qishi.ToInt()
                                       ? MathUtils.Addition(General.Momentum, _ability.BaseEffectNum.ToShort())
                                       : General.Momentum;
            }
        }

        private bool IsProcessHit(AbilityEffectStatus[] abilityEffectStatus)
        {
            foreach (AbilityEffectStatus effect in abilityEffectStatus)
            {
                if (effect.AbilityType == AbilityType.ShengMing ||
                    effect.AbilityType == AbilityType.Jueduifangyu)
                {
                    return false;
                }
            }
            return true;
        }

        private void DoAbilityStatus(TargetProcess targetProcess)
        {
            //处理技能伤害
            decimal hitPercent;
            bool isHit = General.TriggerHitPercent(TagetGeneral, out hitPercent);
            AbilityEffectStatus[] tempList = General.AbilityEffectList;
            decimal effectHit = General.TempAbility.HitPercent;//技能命中概率
            bool isEffectHit = IsProcessHit(tempList);//是否要处理命中
           
            ProcessLog.AddPercent(1, hitPercent);
            if (isEffectHit && isHit)
            {
                targetProcess.IsShanBi = false;
                DoHarmAttack(targetProcess);


                //处理技能效果
                if (effectHit == 0 || RandomUtils.IsHit(effectHit))
                {
                    ProcessAbilityEffect(targetProcess, tempList);
                }

            }
            else if (isEffectHit)
            {
                targetProcess.IsShanBi = true;
            }
            else
            {
                //处理技能效果
                if (effectHit == 0 || RandomUtils.IsHit(effectHit))
                {
                    ProcessAbilityEffect(targetProcess, tempList);
                }
            }

        }

        /// <summary>
        /// 处理技能效果
        /// </summary>
        /// <param name="targetProcess"></param>
        /// <param name="abilityEffectStatus"></param>
        private void ProcessAbilityEffect(TargetProcess targetProcess, AbilityEffectStatus[] abilityEffectStatus)
        {
            foreach (AbilityEffectStatus effect in abilityEffectStatus)
            {
                //立即生效技能

                //没有配置技能效果，处理普通功击
                if (effect.IsSilentStatus)
                {
                    AddDeamgeStatus(effect);
                }
                else if (effect.IsInjuredStatus)
                {
                    //计算持续伤害值
                    if (effect.BoutNum > 0)
                    {
                        if (General.TempAbility.RatioIncreaseNum > 0)
                        {
                            effect.DamageNum = (int)Math.Floor(TagetGeneral.DamageNum * General.TempAbility.RatioIncreaseNum);
                        }
                        else
                        {
                            new BaseLog().SaveLog(new Exception("技能ID:" + General.TempAbility.AbilityID + "续伤害值系数为0"));
                        }
                    }
                    AddDeamgeStatus(effect);
                }
                else if (effect.AbilityType == AbilityType.Hunluan)
                {
                    AddDeamgeStatus(effect);
                }
                else if (effect.AbilityType == AbilityType.ShengMing)
                {
                    effect.BoutNum = 1;//固定一会合
                    AddDeamgeStatus(effect);
                    //加血类不扣防御方,
                    var lifeNum = GetAbilityDamageNum(null);
                    if (effect.IsIncrease)
                    {
                        TagetGeneral.LifeNum += lifeNum;
                    }
                    else
                    {
                        TagetGeneral.LifeNum -= lifeNum;
                    }
                    TagetGeneral.LifeNum = MathUtils.Subtraction(TagetGeneral.LifeNum, 0, 0);
                    TagetGeneral.LifeNum = MathUtils.Addition(TagetGeneral.LifeNum, 0, TagetGeneral.LifeMaxNum);

                    targetProcess.LiveNum = TagetGeneral.LifeNum;
                    targetProcess.DamageNum = effect.IsIncrease ? -lifeNum : lifeNum;//负数客户端是加血
                }
                else if (effect.AbilityType == AbilityType.Qishi)
                {
                    effect.BoutNum = 1;//固定一会合
                    AddDeamgeStatus(effect);
                    //固定值
                    var momentum = effect.BaseNum.ToShort();
                    if (effect.IsIncrease)
                    {
                        TagetGeneral.Momentum += momentum;
                    }
                    else
                    {
                        TagetGeneral.Momentum -= momentum;
                    }
                    TagetGeneral.Momentum = MathUtils.Subtraction(TagetGeneral.Momentum, (short)0, (short)0);
                    TagetGeneral.Momentum = MathUtils.Addition(TagetGeneral.Momentum, (short)0, CombatGeneral.MomentumOut);
                    targetProcess.Momentum = TagetGeneral.Momentum;
                    effect.DamageNum = momentum;
                }
                else if (effect.AbilityType == AbilityType.Jueduifangyu)
                {
                    //固定伤害
                    AddDeamgeStatus(effect);
                }
                else if (effect.IsZengyi)
                {
                    //增加属性效果
                    if (effect.IsIncrease)
                    {
                        ProcessLog.DamageStatusList.Add(effect.Copy());
                        if (effect.BoutNum > 0)
                        {
                            effect.BoutNum = MathUtils.Subtraction(effect.BoutNum, 1, 0);
                        }
                        General.AddDamageStatus(effect);
                    }
                    else
                    {
                        AddDeamgeStatus(effect);
                    }
                }
            }

        }


        private void AddDeamgeStatus(AbilityEffectStatus abilityEffect)
        {
            TagetGeneral.AddDamageStatus(abilityEffect);
        }


        /// <summary>
        /// 伤害类型魂技
        /// </summary>
        /// <param name="targetProcess"></param>
        private void DoHarmAttack(TargetProcess targetProcess)
        {
            if (_ability.AttackType == AttackType.Harm)
            {
                int damageNum;
                if (_ability.AbilityProperty == AbilityProperty.Ability)
                {
                    damageNum = GetAbilityDamageNum(TagetGeneral);
                   
                   
                }
                else
                {
                    damageNum = GetPhysicalDamageNum(TagetGeneral);
                }
                DoTargetDefense(damageNum, targetProcess, true);

            }
        }
        public void GetAbility(string userId)
        {
            //var cacheSet = new gam
        }
        protected override AttackTaget GetAttackTaget()
        {
            return _ability.AttackTaget;
        }

        protected override AttackUnit GetAttackUnit()
        {
            return _ability.AttackUnit;
        }


    }
}
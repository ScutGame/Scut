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
using ZyGames.Base;
using ZyGames.Base.Model;
using ZyGames.Tianjiexing.Model;
using ZyGames.Base.Common;
using ZyGames.GameService.BaseService.LogService;
using System.Diagnostics;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 战斗佣兵对象
    /// </summary>
    public class CombatGeneral : IGeneral
    {
        private static decimal AttackRatio = ConfigEnvSet.GetDecimal("Combat.AttackRatio");
        private static decimal DefenseRatio = ConfigEnvSet.GetDecimal("Combat.DefenseRatio");

        public CombatGeneral()
        {
            _damageStatusList = new List<AbilityEffectStatus>();
        }

        public CombatType CombatType { get; set; }

        /// <summary>
        /// 玩家佣兵
        /// </summary>
        internal string UserID
        {
            get;
            set;
        }

        public int GeneralID
        {
            get;
            set;
        }

        public string GeneralName
        {
            get;
            set;
        }

        public string HeadID
        {
            get;
            set;
        }

        public int LifeNum
        {
            get;
            set;
        }

        public bool IsOver
        {
            get { return LifeNum <= 0; }
        }

        public int LifeMaxNum
        {
            get;
            set;
        }

        public bool IsMove
        {
            get;
            set;
        }

        public bool IsAttrMove
        {
            get;
            set;
        }

        public int LossLifeNum
        {
            get;
            set;
        }
        /// <summary>
        /// 当前回合伤害值
        /// </summary>
        public int DamageNum
        {
            get;
            set;
        }

        /// <summary>
        /// 最低伤害值
        /// </summary>
        public int MinDamageNum
        {
            get;
            set;
        }

        /// <summary>
        /// 固定伤害值（BOSS战使用）
        /// </summary>
        public int FixedDamageNum
        {
            get;
            set;
        }

        public short Lv
        {
            get;
            set;
        }
        /// <summary>
        /// 当前气势
        /// </summary>
        public short Momentum
        {
            get;
            set;
        }

        public int Position
        {
            get;
            set;
        }

        public short CareerID
        {
            get;
            set;
        }

        public CareerType CareerType
        {
            get;
            set;
        }

        public AbilityProperty AbilityProperty
        {
            get
            {
                return this.CareerType == CareerType.Mofashi ? AbilityProperty.Magic : AbilityProperty.Physical;
            }
        }
        /// <summary>
        /// 力量
        /// </summary>
        public short PowerNum
        {
            get;
            set;
        }

        /// <summary>
        /// 魂力
        /// </summary>
        public short SoulNum
        {
            get;
            set;
        }

        /// <summary>
        /// 智力
        /// </summary>
        public short IntellectNum
        {
            get;
            set;
        }

        /// <summary>
        /// 鼓舞加成
        /// </summary>
        public double InspirePercent
        {
            get;
            set;
        }
        /// <summary>
        /// 附加攻击属性：物理,魂技,魔法
        /// </summary>
        public CombatProperty ExtraAttack
        {
            get;
            set;
        }

        /// <summary>
        /// 附加防御属性：物理,魂技,魔法
        /// </summary>
        public CombatProperty ExtraDefense
        {
            get;
            set;
        }

        /// <summary>
        /// 概率修正
        /// </summary>
        public decimal CorrectPersent
        {
            get { return this.SoulNum / 1000; }
        }
        /// <summary>
        /// 技能
        /// </summary>
        public AbilityInfo Ability
        {
            get;
            set;
        }

        private AbilityEffectStatus[] _abilityEffectList;
        /// <summary>
        /// 可释放的技能
        /// </summary>
        public AbilityEffectStatus[] AbilityEffectList
        {
            get
            {
                if (_abilityEffectList == null || _abilityEffectList.Length == 0)
                {
                    AbilityType[] abilityList = SplitToObject<AbilityType>(Ability.AbilityType, str => { return str.ToEnum<AbilityType>(); });
                    int[] baseNumList = SplitToObject<int>(Ability.BaseEffectNum, str => { return str.ToInt(); });
                    decimal[] increaseNumList = SplitToObject<decimal>(Ability.IncreaseNum, str => { return str.ToDecimal(); });

                    _abilityEffectList = new AbilityEffectStatus[abilityList.Length];
                    for (int i = 0; i < abilityList.Length; i++)
                    {
                        //是否有修正概率
                        decimal incNum = increaseNumList.Length > i ? increaseNumList[i] : (decimal)0;
                        if (Ability.IsCorrect)
                        {
                            if (this.Ability.IsIncreaseCorrect)
                            {
                                incNum = incNum.Addition(this.CorrectPersent, decimal.MaxValue);
                            }
                            else
                            {
                                incNum = incNum.Subtraction(this.CorrectPersent, 0);
                            }
                        }
                        _abilityEffectList[i] = new AbilityEffectStatus
                        {
                            AbilityType = abilityList[i],
                            BaseNum = baseNumList.Length > i ? baseNumList[i] : 0,
                            IncreaseNum = incNum,
                            HitPercent = Ability.HitPercent,
                            BoutNum = Ability.EffectCount,
                            TotalBoutNum = Ability.EffectCount,
                            IsIncrease = Ability.IsIncrease,
                            DamageNum = 0,
                            IsRemove = false
                        };
                    }
                }
                var tempEffectList = new List<AbilityEffectStatus>();
                foreach (var effect in _abilityEffectList)
                {
                    tempEffectList.Add(effect.Copy());
                }
                return tempEffectList.ToArray();
            }
        }

        /// <summary>
        /// 佣兵状态
        /// </summary>
        public BattleStatus BattleStatus
        {
            get;
            set;
        }

        private List<AbilityEffectStatus> _damageStatusList;

        /// <summary>
        /// 佣兵释放或中招魂技状态
        /// </summary>
        public void AddDamageStatus(AbilityEffectStatus effectStatus)
        {
            if (effectStatus.BoutNum > 0)
            {
                lock (this)
                {
                    List<AbilityEffectStatus> effectList = _damageStatusList.FindAll(m => m.AbilityType.Equals(effectStatus.AbilityType));
                    if (effectList.Count > 0)
                    {
                        var effect = effectList[0];//合并状态
                        effect.IsRemove = effectStatus.IsRemove;
                        if (effect.BoutNum < effectStatus.BoutNum) effect.BoutNum = effectStatus.BoutNum;
                        if (effect.DamageNum < effectStatus.DamageNum) effect.DamageNum = effectStatus.DamageNum;
                    }
                    else
                    {
                        _damageStatusList.Add(effectStatus);
                    }
                }
            }
            else
            {
                Trace.WriteLine(string.Format(">>佣兵释放或中招魂技状态丢失"));
            }
        }

        public int PhyAttackNum
        {
            get;
            set;
        }

        public int PhyDefenseNum
        {
            get;
            set;
        }
        public int MagAttackNum
        {
            get;
            set;
        }
        public int MagDefenseNum
        {
            get;
            set;
        }
        public int AbiAttackNum
        {
            get;
            set;
        }
        public int AbiDefenseNum
        {
            get;
            set;
        }

        /// <summary>
        /// 暴击
        /// </summary>
        public decimal BaojiNum
        {
            get;
            set;
        }

        /// <summary>
        /// 命中
        /// </summary>
        public decimal HitNum
        {
            get;
            set;
        }

        /// <summary>
        /// 破击
        /// </summary>
        public decimal PojiNum
        {
            get;
            set;
        }

        /// <summary>
        /// 韧性
        /// </summary>
        public decimal RenxingNum
        {
            get;
            set;
        }

        /// <summary>
        /// 闪避
        /// </summary>
        public decimal ShanbiNum
        {
            get;
            set;
        }

        /// <summary>
        /// 格挡
        /// </summary>
        public decimal GedangNum
        {
            get;
            set;
        }

        /// <summary>
        /// 必杀
        /// </summary>
        public decimal BishaNum
        {
            get;
            set;
        }
        /// <summary>
        /// 气势释放启点,最大值
        /// </summary>
        public const short MomentumOut = 100;

        /// <summary>
        /// 是否有释放魂技
        /// </summary>
        /// <returns></returns>
        public bool IsRelease
        {
            get { return Momentum >= MomentumOut; }
        }

        /// <summary>
        /// 伤害处理
        /// </summary>
        public void TriggerDamageNum(int damageNum)
        {
            this.DamageNum = damageNum;
            this.LossLifeNum = this.LossLifeNum.Addition(LifeNum > damageNum ? damageNum : LifeNum, int.MaxValue);
            this.LifeNum = this.LifeNum.Subtraction(damageNum, 0);
            if (IsOver)
            {
                this.BattleStatus = BattleStatus.Over;
            }
            //玩家竞技不扣玩家的血量
            if (!string.IsNullOrEmpty(UserID) &&
                CombatType != CombatType.User &&
                CombatType != CombatType.BossPlot &&
                CombatType != CombatType.MultiPlot
            )
            {
                UserGeneral userGeneral = UserGeneral.FindKey(UserID, this.GeneralID);
                if (userGeneral != null)
                {
                    userGeneral.LifeNum = userGeneral.LifeNum.Subtraction(damageNum, 0);
                    userGeneral.Update();
                }
            }
        }

        /// <summary>
        /// 遍历处理已触发的技能
        /// </summary>
        /// <param name="action"></param>
        public void ForeachAbilityStatus(Action<AbilityEffectStatus> action)
        {
            foreach (var item in _damageStatusList)
            {
                if (item.BoutNum <= 0) item.IsRemove = true;
                if (item.IsRemove) continue;

                action(item);
            }
        }
        /// <summary>
        /// 是否混乱状态
        /// </summary>
        public bool IsHunluanStatus
        {
            get
            {
                bool result = false;
                ForeachAbilityStatus(status =>
                {
                    if (!result)
                    {
                        result = status.AbilityType == AbilityType.Hunluan;
                    }
                });
                return result;
            }
        }

        /// <summary>
        /// 是否绝对防御效果
        /// </summary>
        public bool IsJueduifangyuStatus
        {
            get
            {
                bool result = false;
                ForeachAbilityStatus(status =>
                {
                    if (!result)
                    {
                        result = status.AbilityType == AbilityType.Jueduifangyu;
                    }
                });
                return result;
            }
        }
        /// <summary>
        /// 是否静默状态
        /// </summary>
        public bool IsSilentStatus
        {
            get
            {
                bool result = false;
                ForeachAbilityStatus(status =>
                {
                    if (!result)
                    {
                        result = status.IsSilentStatus;
                    }
                });
                return result;
            }
        }

        /// <summary>
        /// 被攻击解除静默状态
        /// </summary>
        public void RemoveSilentStatus()
        {
            ForeachAbilityStatus(effectStaus =>
            {
                if (effectStaus.IsSilentStatus)
                {
                    effectStaus.BoutNum = 0;
                    effectStaus.IsRemove = true;
                }
            });
        }

        /// <summary>
        /// 是否是怪物 
        /// </summary>
        public bool IsMonster { get; set; }

        /// <summary>
        /// 获取攻击力（物,魂,魔）+ 触发的技能加成
        /// </summary>
        /// <returns></returns>
        public int GetAttackNum(AbilityProperty property)
        {
            //释放魂技加成
            int attackNum = 0;
            switch (property)
            {
                case AbilityProperty.Physical:
                    attackNum = IsMonster ? PhyAttackNum : (int)Math.Floor(this.PowerNum * this.Lv * AttackRatio);
                    attackNum = attackNum.Addition(ExtraAttack.WuliNum, int.MaxValue);
                    attackNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.WuLiGongJi, attackNum));
                    break;
                case AbilityProperty.Ability:
                    attackNum = IsMonster ? AbiAttackNum : (int)Math.Floor(this.SoulNum * this.Lv * AttackRatio);
                    attackNum = attackNum.Addition(ExtraAttack.HunjiNum, int.MaxValue);
                    attackNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.HunJiGongJi, attackNum));
                    break;
                case AbilityProperty.Magic:
                    attackNum = IsMonster ? MagAttackNum : (int)Math.Floor(this.IntellectNum * this.Lv * AttackRatio);
                    attackNum = attackNum.Addition(ExtraAttack.MofaNum, int.MaxValue);
                    attackNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.MoFaGongJi, attackNum));
                    break;
                default:
                    break;
            }
            return attackNum;
        }

        /// <summary>
        /// 获取防御力（物,魂,魔）+ 触发的技能加成
        /// </summary>
        /// <returns></returns>
        public int GetDefenseNum(AbilityProperty property)
        {
            int defenseNum = 0;
            switch (property)
            {
                case AbilityProperty.Physical:
                    defenseNum = IsMonster ? PhyDefenseNum : (int)Math.Floor(this.PowerNum * this.Lv * DefenseRatio);
                    defenseNum = defenseNum.Addition(ExtraDefense.WuliNum, int.MaxValue);
                    defenseNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.WuLiFangYu, defenseNum)).Validate();
                    break;
                case AbilityProperty.Ability:
                    defenseNum = IsMonster ? AbiDefenseNum : (int)Math.Floor(this.SoulNum * this.Lv * DefenseRatio);
                    defenseNum = defenseNum.Addition(ExtraDefense.HunjiNum, int.MaxValue);
                    defenseNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.HunJiFangYu, defenseNum)).Validate();
                    break;
                case AbilityProperty.Magic:
                    defenseNum = IsMonster ? MagDefenseNum : (int)Math.Floor(this.IntellectNum * this.Lv * DefenseRatio);
                    defenseNum = defenseNum.Addition(ExtraDefense.MofaNum, int.MaxValue);
                    defenseNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.MoFaFangYu, defenseNum)).Validate();
                    break;
                default:
                    break;
            }
            return defenseNum;
        }

        /// <summary>
        ///  获取已触发的魂技加成效果
        /// </summary>
        /// <param name="abilityType">技能类型</param>
        /// <param name="demageNum">伤害值</param>
        /// <returns>负数是减益效果，正数是增益效果</returns>
        public decimal TriggerAbilityEffectNum(AbilityType abilityType, int demageNum)
        {
            decimal effectNum = demageNum;
            //当前有的魂技加成
            ForeachAbilityStatus(item =>
            {
                if (item.AbilityType.Equals(abilityType))
                {
                    if (item.IsIncrease)
                    {
                        effectNum = effectNum.Addition(demageNum * item.IncreaseNum + item.BaseNum, int.MaxValue);
                    }
                    else
                    {
                        effectNum = effectNum.Subtraction(demageNum * item.IncreaseNum + item.BaseNum, 0);
                    }
                }
            });
            //有增加和减少的效果 effectNum = effectNum.Validate();
            return effectNum;
        }

        /// <summary>
        /// 触发命中概率
        /// </summary>
        /// <param name="general"></param>
        /// <param name="tpLvPercent"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public bool TriggerHitPercent(CombatGeneral tagetGeneral, out decimal hitPercent)
        {
            decimal tpLvPercent = GetLvPercent(tagetGeneral);
            hitPercent = HitNum + TriggerAbilityEffectNum(AbilityType.MingZhong, 1);
            decimal shanbiPercent = tagetGeneral.ShanbiNum + tagetGeneral.TriggerAbilityEffectNum(AbilityType.ShanBi, 1);
            hitPercent = hitPercent.Subtraction(shanbiPercent, 0).Subtraction(tpLvPercent, 0);

            return RandomHelper.IsHit(hitPercent);
        }
        /// <summary>
        /// 等级差压制
        /// </summary>
        /// <param name="tagetGeneral"></param>
        /// <returns></returns>
        private decimal GetLvPercent(CombatGeneral tagetGeneral)
        {
            return tagetGeneral.Lv.Subtraction(this.Lv, 0) * ConfigEnvSet.GetDecimal("Combat.LvPercentRate");
        }
        /// <summary>
        /// 触发闪避
        /// </summary>
        public void TriggerShanBi()
        {
            //闪避职业加成气势
            CareerAdditionInfo addition = CareerAdditionInfo.FindKey(this.CareerID, AbilityType.ShanBi);
            if (addition != null && addition.MomentumNum > 0)
            {
                this.Momentum = this.Momentum.Addition(addition.MomentumNum.ToShort(), short.MaxValue);
            }
        }

        /// <summary>
        /// 触发格挡概率
        /// </summary>
        /// <param name="tagetGeneral"></param>
        /// <param name="gedangPercent"></param>
        /// <returns></returns>
        public bool TriggerGeDangPercent(CombatGeneral tagetGeneral, out decimal gedangPercent)
        {
            decimal tpLvPercent = GetLvPercent(tagetGeneral);
            gedangPercent = this.GedangNum + TriggerAbilityEffectNum(AbilityType.GeDang, 1);
            decimal pojiNumPercent = tagetGeneral.PojiNum + tagetGeneral.TriggerAbilityEffectNum(AbilityType.PoJi, 1);
            gedangPercent = gedangPercent.Subtraction(pojiNumPercent, 0).Subtraction(tpLvPercent, 0);
            return RandomHelper.IsHit(gedangPercent);
        }

        /// <summary>
        /// 触发爆击概率
        /// </summary>
        /// <param name="tagetGeneral"></param>
        /// <param name="baojiPercent"></param>
        /// <returns></returns>
        public bool TriggerBaojiPercent(CombatGeneral tagetGeneral, out decimal baojiPercent)
        {
            decimal tpLvPercent = GetLvPercent(tagetGeneral);
            baojiPercent = this.BaojiNum + TriggerAbilityEffectNum(AbilityType.BaoJi, 1);
            decimal renxingPercent = tagetGeneral.RenxingNum + tagetGeneral.TriggerAbilityEffectNum(AbilityType.RenXing, 1);
            baojiPercent = baojiPercent.Subtraction(renxingPercent, 0).Subtraction(tpLvPercent, 0);
            return RandomHelper.IsHit(baojiPercent);
        }

        /// <summary>
        /// 触发格挡
        /// </summary>
        public void TriggerGeDang()
        {
            //格挡职业加成气势
            CareerAdditionInfo addition = CareerAdditionInfo.FindKey(this.CareerID, AbilityType.GeDang);
            if (addition != null && addition.MomentumNum > 0)
            {
                this.Momentum = this.Momentum.Addition(addition.MomentumNum.ToShort(), short.MaxValue);
            }
        }


        private T[] SplitToObject<T>(string str, Converter<string, T> converter)
        {
            string[] list = str.Split(new char[] { ',' });
            T[] resultList = new T[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                string temp = list[i];
                if (string.IsNullOrEmpty(temp))
                {
                    continue;
                }
                resultList[i] = converter.Invoke(temp);
            }
            return resultList;
        }

    }
}
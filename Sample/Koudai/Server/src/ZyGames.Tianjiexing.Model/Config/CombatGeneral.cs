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
using System.Diagnostics;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Model;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 战斗佣兵对象
    /// </summary>
    [Serializable, ProtoContract]
    public class CombatGeneral : IGeneral
    {
        public CombatGeneral()
        {
            _damageStatusList = new List<AbilityEffectStatus>();
        }

        [ProtoMember(1)]
        public CombatType CombatType { get; set; }

        /// <summary>
        /// 玩家佣兵
        /// </summary>
        [ProtoMember(2)]
        public string UserID
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public int GeneralID
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public string GeneralName
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public string HeadID
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public int LifeNum
        {
            get;
            set;
        }

        [ProtoMember(7)]
        public bool IsOver
        {
            get
            {
                return LifeNum <= 0;
            }
        }

        [ProtoMember(8)]
        public int LifeMaxNum
        {
            get;
            set;
        }

        [ProtoMember(9)]
        public bool IsMove
        {
            get;
            set;
        }

        [ProtoMember(10)]
        public bool IsAttrMove
        {
            get;
            set;
        }

        [ProtoMember(11)]
        public int LossLifeNum
        {
            get;
            set;
        }
        /// <summary>
        /// 当前回合伤害值
        /// </summary>
        [ProtoMember(12)]
        public int DamageNum
        {
            get;
            set;
        }

        /// <summary>
        /// 最低伤害值
        /// </summary>
        [ProtoMember(13)]
        public int MinDamageNum
        {
            get;
            set;
        }

        /// <summary>
        /// 固定伤害值（BOSS战使用）
        /// </summary>
        [ProtoMember(14)]
        public int FixedDamageNum
        {
            get;
            set;
        }

        [ProtoMember(15)]
        public short Lv
        {
            get;
            set;
        }
        /// <summary>
        /// 当前气势
        /// </summary>
        [ProtoMember(16)]
        public short Momentum
        {
            get;
            set;
        }

        [ProtoMember(17)]
        public int Position
        {
            get;
            set;
        }

        [ProtoMember(45)]
        public int ReplacePosition
        {
            get;
            set;
        }

        /// <summary>
        /// 疲劳值
        /// </summary>
        [ProtoMember(46)]
        public int Fatigue
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家状态
        /// </summary>
        [ProtoMember(47)]
        public UserStatus UserStatus
        {
            get;
            set;
        }


        [ProtoMember(48)]
        public bool IsWait
        {
            get;
            set;
        }

        [ProtoMember(18)]
        public short CareerID
        {
            get;
            set;
        }

        [ProtoMember(19)]
        public CareerType CareerType
        {
            get;
            set;
        }

        public AbilityProperty AbilityProperty
        {
            get
            {
                return CareerType == CareerType.Mofashi ? AbilityProperty.Magic : AbilityProperty.Physical;
            }
        }
        /// <summary>
        /// 力量
        /// </summary>
        [ProtoMember(20)]
        public int PowerNum
        {
            get;
            set;
        }

        /// <summary>
        /// 魂力
        /// </summary>
        [ProtoMember(21)]
        public int SoulNum
        {
            get;
            set;
        }

        /// <summary>
        /// 智力
        /// </summary>
        [ProtoMember(22)]
        public int IntellectNum
        {
            get;
            set;
        }

        /// <summary>
        /// 鼓舞加成
        /// </summary>
        [ProtoMember(23)]
        public double InspirePercent
        {
            get;
            set;
        }
        /// <summary>
        /// 附加攻击属性：物理,魂技,魔法
        /// </summary>
        [ProtoMember(24)]
        public CombatProperty ExtraAttack
        {
            get;
            set;
        }

        /// <summary>
        /// 附加防御属性：物理,魂技,魔法
        /// </summary>
        [ProtoMember(25)]
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
            get { return SoulNum / 1000; }
        }
        /// <summary>
        /// 技能
        /// </summary>
        [ProtoMember(26)]
        public AbilityInfo Ability
        {
            get;
            set;
        }

        [ProtoMember(27)]
        protected AbilityEffectStatus[] _abilityEffectList;
        /// <summary>
        /// 可释放的技能
        /// </summary>
        public AbilityEffectStatus[] AbilityEffectList
        {
            get
            {
                _abilityEffectList = null;
                if (_abilityEffectList == null || _abilityEffectList.Length == 0)
                {
                    AbilityType[] abilityList = SplitToObject<AbilityType>(TempAbility.AbilityType, str => { return ObjectExtend.ToEnum<AbilityType>(str); });
                    int[] baseNumList = SplitToObject<int>(TempAbility.BaseEffectNum, str => { return ObjectExtend.ToInt(str); });
                    decimal[] increaseNumList = SplitToObject<decimal>(TempAbility.IncreaseNum, str => { return str.ToDecimal(); });

                    _abilityEffectList = new AbilityEffectStatus[abilityList.Length];
                    for (int i = 0; i < abilityList.Length; i++)
                    {
                        //是否有修正概率
                        decimal incNum = increaseNumList.Length > i ? increaseNumList[i] : (decimal)0;
                        if (TempAbility.IsCorrect)
                        {
                            if (this.TempAbility.IsIncreaseCorrect)
                            {
                                incNum = MathUtils.Addition(incNum, this.CorrectPersent, decimal.MaxValue);
                            }
                            else
                            {
                                incNum = MathUtils.Subtraction(incNum, this.CorrectPersent, 0);
                            }
                        }
                        _abilityEffectList[i] = new AbilityEffectStatus
                        {
                            AbilityType = abilityList[i],
                            BaseNum = baseNumList.Length > i ? baseNumList[i] : 0,
                            IncreaseNum = incNum,
                            HitPercent = TempAbility.HitPercent,
                            BoutNum = TempAbility.EffectCount,
                            TotalBoutNum = TempAbility.EffectCount,
                            IsIncrease = TempAbility.IsIncrease,
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
        [ProtoMember(28)]
        public BattleStatus BattleStatus
        {
            get;
            set;
        }

        [ProtoMember(29)]
        protected List<AbilityEffectStatus> _damageStatusList;

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

        [ProtoMember(30)]
        public int PhyAttackNum
        {
            get;
            set;
        }

        [ProtoMember(31)]
        public int PhyDefenseNum
        {
            get;
            set;
        }
        [ProtoMember(32)]
        public int MagAttackNum
        {
            get;
            set;
        }
        [ProtoMember(33)]
        public int MagDefenseNum
        {
            get;
            set;
        }
        [ProtoMember(34)]
        public int AbiAttackNum
        {
            get;
            set;
        }
        [ProtoMember(35)]
        public int AbiDefenseNum
        {
            get;
            set;
        }

        /// <summary>
        /// 暴击
        /// </summary>
        [ProtoMember(36)]
        public decimal BaojiNum
        {
            get;
            set;
        }

        /// <summary>
        /// 命中
        /// </summary>
        [ProtoMember(37)]
        public decimal HitNum
        {
            get;
            set;
        }

        /// <summary>
        /// 破击
        /// </summary>
        [ProtoMember(38)]
        public decimal PojiNum
        {
            get;
            set;
        }

        /// <summary>
        /// 韧性
        /// </summary>
        [ProtoMember(39)]
        public decimal RenxingNum
        {
            get;
            set;
        }

        /// <summary>
        /// 闪避
        /// </summary>
        [ProtoMember(40)]
        public decimal ShanbiNum
        {
            get;
            set;
        }

        /// <summary>
        /// 格挡
        /// </summary>
        [ProtoMember(41)]
        public decimal GedangNum
        {
            get;
            set;
        }

        /// <summary>
        /// 必杀
        /// </summary>
        [ProtoMember(42)]
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
            this.LossLifeNum = MathUtils.Addition(LossLifeNum, LifeNum > damageNum ? damageNum : LifeNum, int.MaxValue);
            this.LifeNum = MathUtils.Subtraction(LifeNum, damageNum, 0);
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
                var cacheSet = new GameDataCacheSet<UserGeneral>();
                UserGeneral userGeneral = cacheSet.FindKey(UserID, this.GeneralID);
                if (userGeneral != null)
                {
                    userGeneral.LifeNum = MathUtils.Subtraction(userGeneral.LifeNum, damageNum, 0);
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
            //原因：静默技能被攻击不解除。
            //ForeachAbilityStatus(effectStaus =>
            //{
            //    if (effectStaus.IsSilentStatus)
            //    {
            //        effectStaus.BoutNum = 0;
            //        effectStaus.IsRemove = true;
            //    }
            //});
        }

        /// <summary>
        /// 是否是怪物 
        /// </summary>
        [ProtoMember(43)]
        public bool IsMonster { get; set; }

        /// <summary>
        ///替补佣兵是否出现
        /// </summary>
        [ProtoMember(44)]
        public bool IsAppear { get; set; }

        /// <summary>
        /// 获取攻击力（物,魂,魔）+ 触发的技能加成
        /// </summary>
        /// <returns></returns>
        public int GetAttackNum(AbilityProperty property)
        {
            //释放魂技加成
            decimal AttackRatio = ConfigEnvSet.GetDecimal("Combat.AttackRatio");
            int attackNum = 0;
            switch (property)
            {
                case AbilityProperty.Physical:
                    attackNum = IsMonster ? PhyAttackNum : (int)Math.Floor(this.PowerNum * this.Lv * AttackRatio);
                    attackNum = MathUtils.Addition(attackNum, ExtraAttack.WuliNum, int.MaxValue);
                    attackNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.WuLiGongJi, attackNum));
                    attackNum = MathUtils.Addition(attackNum, ExtraAttack.AdditionWuliNum*attackNum).ToInt();
                    break;
                case AbilityProperty.Ability:
                    attackNum = IsMonster ? AbiAttackNum : (int)Math.Floor(this.SoulNum * this.Lv * AttackRatio);
                    attackNum = MathUtils.Addition(attackNum, ExtraAttack.HunjiNum, int.MaxValue);
                    attackNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.HunJiGongJi, attackNum));
                    attackNum = MathUtils.Addition(attackNum, ExtraAttack.AdditionHunjiNum * attackNum).ToInt();
                    break;
                case AbilityProperty.Magic:
                    attackNum = IsMonster ? MagAttackNum : (int)Math.Floor(this.IntellectNum * this.Lv * AttackRatio);
                    attackNum = MathUtils.Addition(attackNum, ExtraAttack.MofaNum, int.MaxValue);
                    attackNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.MoFaGongJi, attackNum));
                    attackNum = MathUtils.Addition(attackNum, ExtraAttack.AdditionMofaNum * attackNum).ToInt();
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
        public int GetDefenseNum(AbilityProperty property, CombatGeneral tagetGeneral)
        {
            var DefenseRatio = ConfigEnvSet.GetDecimal("Combat.DefenseRatio");
            int defenseNum = 0;
            switch (property)
            {
                case AbilityProperty.Physical:
                    bool tType = false;
                    decimal tpLvPercent = GetLvPercent(tagetGeneral, out tType);
                    defenseNum = IsMonster ? PhyDefenseNum : (int)Math.Floor(this.PowerNum * this.Lv * DefenseRatio);
                    defenseNum = MathUtils.Addition(defenseNum, ExtraDefense.WuliNum, int.MaxValue);
                    defenseNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.WuLiFangYu, defenseNum));
                    if (!tType)
                    {
                        defenseNum = MathUtils.Subtraction(defenseNum, (tpLvPercent * defenseNum).ToInt());
                    }
                    else
                    {
                        defenseNum = MathUtils.Addition(defenseNum, (tpLvPercent * defenseNum).ToInt());
                    }
                    defenseNum = MathUtils.Addition(defenseNum, ExtraDefense.AdditionWuliNum * defenseNum).ToInt();
                    break;
                case AbilityProperty.Ability:
                    defenseNum = IsMonster ? AbiDefenseNum : (int)Math.Floor(this.SoulNum * this.Lv * DefenseRatio);
                    defenseNum = MathUtils.Addition(defenseNum, ExtraDefense.HunjiNum, int.MaxValue);
                    defenseNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.HunJiFangYu, defenseNum));
                    defenseNum = MathUtils.Addition(defenseNum, ExtraDefense.AdditionHunjiNum * defenseNum).ToInt();
                    break;
                case AbilityProperty.Magic:
                    defenseNum = IsMonster ? MagDefenseNum : (int)Math.Floor(this.IntellectNum * this.Lv * DefenseRatio);
                    defenseNum = MathUtils.Addition(defenseNum, ExtraDefense.MofaNum, int.MaxValue);
                    defenseNum = (int)Math.Floor(TriggerAbilityEffectNum(AbilityType.MoFaFangYu, defenseNum));
                    defenseNum = MathUtils.Addition(defenseNum, ExtraDefense.AdditionMofaNum * defenseNum).ToInt();
                    break;
                default:
                    break;
            }

            if (!defenseNum.IsValid()) defenseNum = 0;
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
                        effectNum = MathUtils.Addition(effectNum, demageNum * item.IncreaseNum + item.BaseNum, int.MaxValue);
                    }
                    else
                    {
                        effectNum = MathUtils.Subtraction(effectNum, demageNum * item.IncreaseNum + item.BaseNum, 0);
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
            //int tType = 0;
            //decimal tpLvPercent = GetLvPercent(tagetGeneral, out tType);
            hitPercent = HitNum + TriggerAbilityEffectNum(AbilityType.MingZhong, 1);
            decimal shanbiPercent = tagetGeneral.ShanbiNum + tagetGeneral.TriggerAbilityEffectNum(AbilityType.ShanBi, 1);
            hitPercent = MathUtils.Subtraction(hitPercent, shanbiPercent, 0);

            //原因：增加最小概率
            decimal minHitNum = ConfigEnvSet.GetDecimal("Combat.MinHitPercent");
            if (minHitNum == 0) minHitNum = (decimal)0.4;
            if (hitPercent < minHitNum)
            {
                hitPercent = minHitNum;
            }
            return RandomUtils.IsHit(hitPercent);
        }
        /// <summary>
        /// 等级差压制
        /// </summary>
        /// <param name="tagetGeneral"></param>
        /// <param name="tType">False:攻击方等级高，True：被攻击方等级高</param>
        /// <returns></returns>
        private decimal GetLvPercent(CombatGeneral tagetGeneral, out bool tType)
        {
            tType = false;
            var lvrate = ConfigEnvSet.GetDecimal("Combat.LvPercentRate");
            var subLv = 0;
            if (tagetGeneral.Lv > this.Lv)
            {
                tType = true;
                subLv = MathUtils.Subtraction(tagetGeneral.Lv, this.Lv, (short)0);
            }
            else
            {
                subLv = MathUtils.Subtraction(this.Lv, tagetGeneral.Lv, (short)0);
            }
            var val = subLv * lvrate;
            return val;
        }


        /// <summary>
        /// 触发闪避
        /// </summary>
        public void TriggerShanBi()
        {
            //闪避职业加成气势
            CareerAdditionInfo addition = new ConfigCacheSet<CareerAdditionInfo>().FindKey(this.CareerID, AbilityType.ShanBi);
            if (addition != null && addition.MomentumNum > 0)
            {
                this.Momentum = MathUtils.Addition(this.Momentum, addition.MomentumNum.ToShort(), short.MaxValue);
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
            //原因：等级压制
            bool tType = false;
            decimal tpLvPercent = GetLvPercent(tagetGeneral, out tType);
            gedangPercent = this.GedangNum + TriggerAbilityEffectNum(AbilityType.GeDang, 1);
            decimal pojiNumPercent = tagetGeneral.PojiNum + tagetGeneral.TriggerAbilityEffectNum(AbilityType.PoJi, 1);
            if (!tType)
            {
                pojiNumPercent = MathUtils.Subtraction(pojiNumPercent, tpLvPercent);
            }
            else
            {
                pojiNumPercent = MathUtils.Addition(pojiNumPercent, tpLvPercent);
            }
            gedangPercent = MathUtils.Subtraction(gedangPercent, pojiNumPercent, 0);
            return RandomUtils.IsHit(gedangPercent);
        }

        /// <summary>
        /// 触发爆击概率
        /// </summary>
        /// <param name="tagetGeneral"></param>
        /// <param name="baojiPercent"></param>
        /// <returns></returns>
        public bool TriggerBaojiPercent(CombatGeneral tagetGeneral, out decimal baojiPercent)
        {
            bool tType = false;
            decimal tpLvPercent = GetLvPercent(tagetGeneral, out tType);
            baojiPercent = this.BaojiNum + TriggerAbilityEffectNum(AbilityType.BaoJi, 1);
            decimal renxingPercent = tagetGeneral.RenxingNum + tagetGeneral.TriggerAbilityEffectNum(AbilityType.RenXing, 1);
            baojiPercent = MathUtils.Subtraction(baojiPercent, renxingPercent, 0);
            if (!tType)
            {
                baojiPercent = MathUtils.Addition(baojiPercent, tpLvPercent);
            }
            else
            {
                baojiPercent = MathUtils.Subtraction(baojiPercent, tpLvPercent, 0);
            }
            return RandomUtils.IsHit(baojiPercent);
        }

        /// <summary>
        /// 触发格挡
        /// </summary>
        public void TriggerGeDang()
        {
            //格挡职业加成气势
            CareerAdditionInfo addition = new ConfigCacheSet<CareerAdditionInfo>().FindKey(this.CareerID, AbilityType.GeDang);
            if (addition != null && addition.MomentumNum > 0)
            {
                this.Momentum = MathUtils.Addition(this.Momentum, addition.MomentumNum.ToShort(), short.MaxValue);
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
        /// <summary>
        /// 固有技能等级加成
        /// </summary>
        public decimal EffectValue { get; set; }
        /// <summary>
        /// 中等头像
        /// </summary>
        public String BattleHeadID { get; set; }
        /// <summary>
        /// 玩家魂技列表
        /// </summary>
        public List<AbilityInfo> AbilityInfoList { get; set; }
        /// <summary>
        /// 玩家自身魂技列表
        /// </summary>
        public List<AbilityInfo> SelfAbilityInfoList { get; set; }
        /// <summary>
        /// 0:无1：自有魂技2：出发魂技
        /// </summary>
        public Int16 IsSelfAbility { get; set; }
        /// <summary>
        /// 触发魂技
        /// </summary>
        public AbilityInfo TriggerAbility { get; set; }
        /// <summary>
        /// 自身魂技加成值
        /// </summary>
        public Decimal SelfEffectValue { get; set; }
        /// <summary>
        /// 当前释放的魂技
        /// </summary>
        public AbilityInfo TempAbility { get; set; }
        /// <summary>
        /// 是否变更魂技
        /// </summary>
        public bool ChangeAbility { get; set; }

    }
}
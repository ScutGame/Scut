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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Model;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 怪物列表
    /// </summary>
    public class SJTMonsterQueue : EmbattleQueue
    {

        public SJTMonsterQueue(int plotNpcID)
        {
            var embattleList = new ConfigCacheSet<SJTPlotEmbattleInfo>().FindAll(m => m.PlotNpcID == plotNpcID);
            foreach (SJTPlotEmbattleInfo embattle in embattleList)
            {
                int index = embattle.GridSeqNo - 1;
                IGeneral general = Create(embattle);
                if (general != null)
                {
                    SetQueue(index, general);
                }
            }

            this.PriorityNum = 0;
        }

        public SJTMonsterQueue(IGeneral general)
        {
            if (general != null)
            {
                SetQueue(general.Position - 1, general);
            }
        }

        public static IGeneral Create(SJTPlotEmbattleInfo embattle)
        {
            var monster = new ConfigCacheSet<SJTMonsterInfo>().FindKey(embattle.MonsterID);
            if (monster == null) throw new Exception("Plot monster:" + embattle.MonsterID + " is not exist");

            CareerInfo career = new ConfigCacheSet<CareerInfo>().FindKey(monster.CareerID);
            AbilityInfo ability = new ConfigCacheSet<AbilityInfo>().FindKey(monster.AbilityID);
            var abilityLv = new ConfigCacheSet<AbilityLvInfo>().FindKey(monster.AbilityLv);

            if (career == null || ability == null || abilityLv == null)
            {
                throw new Exception("career or ability or AbilityLv is null.");
            }

            CombatGeneral general = new CombatGeneral()
            {
                UserID = null,
                Position = embattle.GridSeqNo,
                GeneralID = embattle.MonsterID,
                GeneralName = monster.GeneralName,
                HeadID = monster.HeadID,
                CareerID = monster.CareerID,
                CareerType = career.CareerType,
                IsMove = career.IsMove,
                LifeNum = monster.LifeNum,
                LifeMaxNum = monster.LifeNum,
                Lv = monster.GeneralLv,
                Momentum = (short)monster.MomentumNum,
                Ability = ability,
                IsAttrMove = ability.IsMove,
                BaojiNum = monster.BaojiNum,
                BishaNum = monster.BishaNum,
                RenxingNum = monster.RenxingNum,
                HitNum =
                    monster.HitNum == 0
                        ? ConfigEnvSet.GetDecimal("Combat.HitiNum")
                        : monster.HitNum,
                ShanbiNum = monster.ShanbiNum,
                GedangNum = monster.GedangNum,
                PojiNum = monster.PojiNum,
                BattleStatus = BattleStatus.Normal,
                PowerNum = monster.PowerNum,
                SoulNum = monster.SoulNum,
                IntellectNum = monster.IntellectNum,
                FixedDamageNum = 0,
                //怪物没有附加属性
                ExtraAttack = new CombatProperty(),
                ExtraDefense = new CombatProperty(),
                IsMonster = true,
                IsWait = false,
                PhyAttackNum = monster.PhyAttackNum,
                PhyDefenseNum = monster.PhyDefenseNum,
                MagAttackNum = monster.MagAttackNum,
                MagDefenseNum = monster.MagDefenseNum,
                AbiAttackNum = monster.AbiAttackNum,
                AbiDefenseNum = monster.AbiDefenseNum

            };
            general.PhyAttackNum = MathUtils.Addition(general.PhyAttackNum,
                                                      (general.PhyAttackNum*abilityLv.EffectValue)).ToInt();
            general.PhyDefenseNum = MathUtils.Addition(general.PhyDefenseNum,
                                                      (general.PhyDefenseNum * abilityLv.EffectValue)).ToInt();
            general.MagAttackNum = MathUtils.Addition(general.MagAttackNum,
                                                      (general.MagAttackNum * abilityLv.EffectValue)).ToInt();
            general.MagDefenseNum = MathUtils.Addition(general.MagDefenseNum,
                                                      (general.MagDefenseNum * abilityLv.EffectValue)).ToInt();
            general.AbiAttackNum = MathUtils.Addition(general.AbiAttackNum,
                                                      (general.AbiAttackNum * abilityLv.EffectValue)).ToInt();
            general.AbiDefenseNum = MathUtils.Addition(general.AbiDefenseNum,
                                                      (general.AbiDefenseNum * abilityLv.EffectValue)).ToInt();
            return general;
        }
    }
}
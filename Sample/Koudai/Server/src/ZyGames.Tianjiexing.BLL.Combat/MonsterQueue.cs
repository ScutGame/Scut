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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Model;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 怪物列表
    /// </summary>
    public class MonsterQueue : EmbattleQueue
    {
        public MonsterQueue(int plotNpcID)
        {
            var embattleList = new ConfigCacheSet<PlotEmbattleInfo>().FindAll(m => m.PlotNpcID == plotNpcID);
            foreach (PlotEmbattleInfo embattle in embattleList)
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

        public MonsterQueue(IGeneral general)
        {
            if (general != null)
            {
                SetQueue(general.Position - 1, general);
            }
        }

        /// <summary>
        /// 圣吉塔战役
        /// </summary>
        /// <param name="plotNpcID"></param>
        /// <param name="difficultNum"></param>
        public MonsterQueue(int plotNpcID, double difficultNum)
        {
            var sjtEmbattleList = new ConfigCacheSet<SJTPlotEmbattleInfo>().FindAll();
            sjtEmbattleList = sjtEmbattleList.FindAll(m => m.PlotNpcID == plotNpcID);
            foreach (SJTPlotEmbattleInfo embattle in sjtEmbattleList)
            {
                int index = embattle.GridSeqNo - 1;

                IGeneral general = Create(embattle, difficultNum);
                if (general != null)
                {
                    SetQueue(index, general);
                }
            }
            this.PriorityNum = 0;
        }

        public static IGeneral Create(SJTPlotEmbattleInfo embattle, double difficultNum)
        {

            SJTMonsterInfo monster = new ConfigCacheSet<SJTMonsterInfo>().FindKey(embattle.MonsterID);
            if (monster == null) throw new Exception("Plot monster:" + embattle.MonsterID + " is not exist");

            CareerInfo career = new ConfigCacheSet<CareerInfo>().FindKey(monster.CareerID);
            AbilityInfo ability = new ConfigCacheSet<AbilityInfo>().FindKey(monster.AbilityID);
            if (career == null || ability == null)
            {
                throw new Exception("career or ability is null.");
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
                //todo 圣吉塔怪物表未配置固定伤害值
                FixedDamageNum = 1, //monster.DamageNum,
                //怪物没有附加属性
                ExtraAttack = new CombatProperty(),
                ExtraDefense = new CombatProperty(),
                IsMonster = true,
                IsWait = false,
                PhyAttackNum = (monster.PhyAttackNum * difficultNum).ToInt(),
                PhyDefenseNum = (monster.PhyDefenseNum * difficultNum).ToInt(),
                MagAttackNum = (monster.MagAttackNum * difficultNum).ToInt(),
                MagDefenseNum = (monster.MagDefenseNum * difficultNum).ToInt(),
                AbiAttackNum = (monster.AbiAttackNum * difficultNum).ToInt(),
                AbiDefenseNum = (monster.AbiDefenseNum * difficultNum).ToInt()

            };
            return general;
        }

        public static IGeneral Create(PlotEmbattleInfo embattle)
        {
            MonsterInfo monster = new ConfigCacheSet<MonsterInfo>().FindKey(embattle.MonsterID);
            if (monster == null) throw new Exception("Plot monster:" + embattle.MonsterID + " is not exist");

            CareerInfo career = new ConfigCacheSet<CareerInfo>().FindKey(monster.CareerID);
            AbilityInfo ability = new ConfigCacheSet<AbilityInfo>().FindKey(monster.AbilityID);
            if (career == null || ability == null)
            {
                throw new Exception("career or ability is null.");
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
                //PowerNum = monster.PowerNum,
                //SoulNum = monster.SoulNum,
                //IntellectNum = monster.IntellectNum,
                FixedDamageNum = monster.DamageNum,
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

            return general;
        }

        /// <summary>
        ///  boss 每提升一级的属性
        /// </summary>
        /// <param name="plotnpcID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static decimal BossProperty(int plotnpcID, AbilityType type)
        {
            decimal propertyNum = 0;
            bool isBoss = false;
            string bossPro = string.Empty;
            var npcInfoList = new ConfigCacheSet<PlotNPCInfo>().FindAll(s => s.PlotNpcID == plotnpcID);
            foreach (var info in npcInfoList)
            {
                var activeList = new ShareCacheStruct<GameActive>().FindAll(s => s.ActiveType == FunctionEnum.Booszhang && s.BossPlotID == info.PlotID);
                if (activeList.Count > 0)
                {
                    bossPro = GameConfigSet.BossProperty;
                    isBoss = true;
                }
                else
                {
                    activeList = new ShareCacheStruct<GameActive>().FindAll(s => s.ActiveType == FunctionEnum.Gonghui && s.ActiveStyle == 4 && s.BossPlotID == info.PlotID);
                    if (activeList.Count > 0)
                    {
                        bossPro = GameConfigSet.GuildBossProperty;
                        isBoss = true;
                    }
                }
            }

            if (isBoss && !string.IsNullOrEmpty(bossPro))
            {
                var propertyList = JsonUtils.Deserialize<CacheList<GeneralProperty>>(bossPro);
                var property = propertyList.Find(s => s.AbilityType == type);
                if (property != null)
                {
                    propertyNum = property.AbilityValue;
                }
            }
            return propertyNum;
        }
    }
}
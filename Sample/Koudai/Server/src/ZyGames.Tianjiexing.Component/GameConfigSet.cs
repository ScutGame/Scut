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
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.Component
{
    public static class GameConfigSet
    {
        public static void Initialize()
        {
            GameEnvironment.Global.Clear();
        }
        
        /// <summary>
        /// 附魔符培养上限
        /// </summary>
        public static int MaxEnchantMature { get { return ConfigEnvSet.GetInt("Enchant.MaxEnchantMature"); } }

        /// <summary>
        /// 附魔符背包初始值
        /// </summary>
        public static int BasePackageNum { get { return ConfigEnvSet.GetInt("Enchant.EnchantBasePackageNum"); } }

        /// <summary>
        /// 附魔符背包最大格子数
        /// </summary>
        public static int MaxPackageNum { get { return ConfigEnvSet.GetInt("Enchant.EnchantMaxPackageNum"); } }

        /// <summary>
        /// 开启附魔符背包格子花费晶石初始值
        /// </summary>
        public static int OpenGoldNum { get { return ConfigEnvSet.GetInt("Enchant.OpenPackageGoldNum"); } }

        /// <summary>
        /// 附魔符最高等级
        /// </summary>
        public static int MaxEnchantLv { get { return ConfigEnvSet.GetInt("Enchant.MaxEnchantLv"); } }

        /// <summary>
        /// 法宝技能最高等级
        /// </summary>
        public static int MaxSkillLv { get { return ConfigEnvSet.GetInt("Trump.SkillMaxLv"); } }

        /// <summary>
        /// 法宝祭祀最高等级
        /// </summary>
        public static short MaxWorshipLv { get { return ConfigEnvSet.GetInt("Worship.MaxWorshipLv").ToShort(); } }


        /// <summary>
        /// 法宝最大等级
        /// </summary>
        public static int MaxTrumpLv { get { return ConfigEnvSet.GetInt("Trump.MaxTrumpLv"); } }
        /// <summary>
        /// 法宝最大成长率
        /// </summary>
        public static int MaxMatureNum { get { return ConfigEnvSet.GetInt("Trump.MaxMatrueNum"); } }
        /// <summary>
        /// 战斗次数
        /// </summary>
        public static int TrumpCombatNum { get { return ConfigEnvSet.GetInt("Trump.TrumpCombatNum"); } }
        /// <summary>
        /// 扣除法宝寿命
        /// </summary>
        public static int TrumpLifeNum { get { return ConfigEnvSet.GetInt("Trump.TrumpLifeNum"); } }
        /// <summary>
        /// 佣兵死亡扣除法宝寿命
        /// </summary>
        public static int GeneralOverLife { get { return ConfigEnvSet.GetInt("Trump.GeneralOverLife"); } }

        /// <summary>
        /// 免费抽奖次数
        /// </summary>
        public static int FreeSweepstakes { get { return ConfigEnvSet.GetInt("Dail.FreeSweepstakes"); } }
        /// <summary>
        ///  抽奖一次所需晶石
        /// </summary>
        public static int SweepstakesRequiredGold { get { return ConfigEnvSet.GetInt("Dail.SweepstakesRequiredGold"); } }

        /// <summary>
        /// 抽奖五次所需晶石
        /// </summary>
        public static int FiveRequiredGold { get { return ConfigEnvSet.GetInt("Dail.FiveRequiredGold"); } }
        /// <summary>
        /// 我的宝藏显示的数量
        /// </summary>
        public static int Treasure { get { return ConfigEnvSet.GetInt("Dail.Treasure"); } }
        /// <summary>
        /// 我的宝藏显示的数量
        /// </summary>
        public static int UserTreasureNum { get { return ConfigEnvSet.GetInt("Dail.UserTreasureNum"); } }

        /// <summary>
        /// 公会争斗战时间
        /// </summary>
        public static string BattleTime { get { return ConfigEnvSet.GetString("ServerGuild.BattleTime"); } }

        /// <summary>
        /// 公会争斗战活动开始前n分钟结束报名
        /// </summary>
        public static int BattleEndBeforeDate { get { return ConfigEnvSet.GetInt("ServerGuild.BattleEndBeforeDate"); } }

        /// <summary>
        /// 公会争斗战活动开始前n分钟开始提示参战
        /// </summary>
        public static int BattleBroadcast { get { return ConfigEnvSet.GetInt("ServerGuild.BattleBroadcast"); } }

        /// <summary>
        /// 公会争斗战等待时间(分钟)
        /// </summary>
        public static int BattleWaitDateTime { get { return ConfigEnvSet.GetInt("ServerGuild.BattleWaitDateTime"); } }

        /// <summary>
        /// 公会争斗战每个阶段的时间(分钟)
        /// </summary>
        public static int BattleIntervalDate { get { return ConfigEnvSet.GetInt("ServerGuild.BattleIntervalDate"); } }

        /// <summary>
        /// 城市公会争斗战冠军每日登陆城市广播次数
        /// </summary>
        public static int SantoVisitNum { get { return ConfigEnvSet.GetInt("FightCombat.SantoVisitNum"); } }


        /// <summary>
        /// 每层疲劳值减少该玩家8%所有类型的总攻击和防御
        /// </summary>
        public static decimal Fatigue { get { return ConfigEnvSet.GetDecimal("FightCombat.FatigueReducingProperties"); } }




        /// <summary>
        ///人物默认每升一级给与20晶石的奖励
        /// </summary>
        public static int GoldReward { get { return ConfigEnvSet.GetInt("GameUser.CharacterUpgrade"); } }

        /// <summary>
        /// 当前可达到的最高等级
        /// </summary>
        public static int CurrMaxLv = ConfigEnvSet.GetInt("User.CurrMaxLv");

        /// <summary>
        /// 相同佣兵获得经验加成
        /// </summary>
        public static double ExpMultiple = ConfigEnvSet.GetDouble("General.ExpMultiple");

        /// <summary>
        /// 佣兵传承类型
        /// </summary>
        public static string HeritageList = ConfigEnvSet.GetString("UserGeneral.HeritageList");
        
        /// <summary>
        /// 每次使用精力药剂补充的精力数值
        /// </summary>
        public static int CurrEnergyNum = ConfigEnvSet.GetInt("User.CurrEnergyNum");
        /// <summary>
        /// 恢复精力数值
        /// </summary>
        public static int RecoverEnergy { get { return ConfigEnvSet.GetInt("User.RecoveryEnergy"); } }

        /// <summary>
        /// boss 每提升一级的属性
        /// </summary>
        public static string BossProperty { get { return ConfigEnvSet.GetString("Boss.AdditionalProperty"); } }

        /// <summary>
        /// 公会boss 每提升一级的属性
        /// </summary>
        public static string GuildBossProperty { get { return ConfigEnvSet.GetString("GuildBoss.AdditionalProperty"); } }
    }
}
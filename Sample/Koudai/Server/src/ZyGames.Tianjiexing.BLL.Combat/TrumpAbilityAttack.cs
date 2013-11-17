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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 法宝攻击
    /// </summary>
    public class TrumpAbilityAttack
    {

        public static List<SkillLvInfo> CreateSkillLvInfo(CombatGeneral general)
        {
            List<SkillLvInfo> _skillLvList = new List<SkillLvInfo>();
            if (general.GeneralID == LanguageManager.GetLang().GameUserGeneralID)
            {
                var abilityCacheSet = new ConfigCacheSet<AbilityInfo>();
                var skillLvSet = new ConfigCacheSet<SkillLvInfo>();
                UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(general.UserID, TrumpInfo.CurrTrumpID) ?? new UserTrump();
                if (userTrump.LiftNum > 0)
                {
                    userTrump.SkillInfo.Foreach(obj =>
                    {
                        var abilityInfo = abilityCacheSet.FindKey(obj.AbilityID) ?? new AbilityInfo();
                        if (abilityInfo.AttackType == AttackType.Trigger)
                        {
                            var temp = skillLvSet.FindKey(obj.AbilityID, obj.AbilityLv);
                            if (temp != null)
                            {
                                _skillLvList.Add(temp);
                            }
                        }
                        return true;
                    });
                }
            }
            return _skillLvList;
        }

        public static decimal GetEffect(CombatGeneral general, AbilityType abilityType)
        {
            decimal effNum = 1;
            if (general.GeneralID != LanguageManager.GetLang().GameUserGeneralID)
            {
                return 0;
            }
            switch (abilityType)
            {
                case AbilityType.BaoJiJiaCheng:
                    effNum = MathUtils.Addition(effNum, GetEffTypeNum(general, abilityType));
                    break;
                case AbilityType.IsBaoJiReduce:
                    effNum = MathUtils.Subtraction(effNum, GetEffTypeNum(general, abilityType));
                    break;
                case AbilityType.Resurrect:
                    effNum = GetEffTypeNum(general, abilityType);
                    break;
                case AbilityType.AttackLife:
                    effNum = GetEffTypeNum(general, abilityType);
                    break;
                case AbilityType.Furious:
                    effNum = LifeLowerTnumEffNum(general, abilityType);
                    break;
                case AbilityType.NormalAttackPoFang:
                    effNum = GetEffTypeNum(general, abilityType);
                    break;
                case AbilityType.AttackPoDun:
                    //effNum = GetEffTypeNum(general, abilityType);
                    break;
                case AbilityType.FanShang:
                    effNum = GetEffTypeNum(general, abilityType);
                    break;
                default:
                    return effNum;
            }
            return effNum;
        }

        /// <summary>
        /// 破盾
        /// </summary>
        /// <param name="general"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static bool AttackPoDun(CombatGeneral general, AbilityType abilityType)
        {
            SkillLvInfo skillLvInfo = CreateSkillLvInfo(general).Find(m => m.EffType == abilityType);
            if (skillLvInfo != null && RandomUtils.IsHit(skillLvInfo.Probability))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 血量低于特定值时加成
        /// </summary>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        private static decimal LifeLowerTnumEffNum(CombatGeneral general, AbilityType abilityType)
        {
            SkillLvInfo skillLvInfo = CreateSkillLvInfo(general).Find(m => m.EffType == abilityType);
            if (skillLvInfo != null)
            {
                if (skillLvInfo.Tnum > 1 && general.LifeNum <= skillLvInfo.Tnum && RandomUtils.IsHit(skillLvInfo.Probability))
                {
                    return skillLvInfo.EffNum;
                }
                else if (skillLvInfo.Tnum < 1)
                {
                    decimal tnum = skillLvInfo.Tnum * general.LifeMaxNum;
                    if (tnum >= general.LifeNum && RandomUtils.IsHit(skillLvInfo.Probability))
                    {
                        return skillLvInfo.EffNum;
                    }
                }
            }
            return 0;
        }

        private static decimal GetEffTypeNum(CombatGeneral general, AbilityType abilityType)
        {
            List<SkillLvInfo> skillLvList = CreateSkillLvInfo(general);
            foreach (SkillLvInfo info in skillLvList)
            {
                if (info.EffType == abilityType && RandomUtils.IsHit(info.Probability))
                {
                    return info.EffNum;
                }
            }
            return 0;
        }

        /// <summary>
        /// 法宝技能——属性转换
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<GeneralProperty> AttributeConversion(string userID, decimal powerNum, decimal soulNum, decimal intellectNum)
        {
            decimal changeNum = 0;
            List<GeneralProperty> propertyList = new List<GeneralProperty>();
            UserGeneral general = UserGeneral.GetMainGeneral(userID);
            if (general == null)
            {
                return propertyList;
            }
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            if (userTrump != null && userTrump.LiftNum > 0 && userTrump.SkillInfo.Count > 0)
            {
                foreach (SkillInfo skillInfo in userTrump.SkillInfo)
                {
                    AbilityInfo abilityInfo = new ConfigCacheSet<AbilityInfo>().FindKey(skillInfo.AbilityID);
                    if (abilityInfo != null && abilityInfo.AttackType == AttackType.Change)
                    {
                        SkillLvInfo skillLvInfo = new ConfigCacheSet<SkillLvInfo>().FindKey(skillInfo.AbilityID, skillInfo.AbilityLv);
                        if (skillLvInfo == null)
                        {
                            break;
                        }
                        switch (abilityInfo.ChangeAbility)
                        {
                            case AbilityType.PowerNum:
                                changeNum = powerNum;
                                break;
                            case AbilityType.SoulNum:
                                changeNum = soulNum;
                                break;
                            case AbilityType.IntelligenceNum:
                                changeNum = intellectNum;
                                break;
                            default:
                                changeNum = 0;
                                break;
                        }
                        GeneralProperty property = new GeneralProperty();
                        property.AbilityType = abilityInfo.AfterAbility;
                        property.AbilityValue = changeNum * skillLvInfo.Coefficient;
                        propertyList.Add(property);
                    }
                }
            }
            return propertyList;
        }

        /// <summary>
        /// 法宝技能——属性转换获得的值
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="changeNum"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static decimal ConversionPropertyNum(string userID, decimal powerNum, decimal soulNum, decimal intellectNum, AbilityType abilityType)
        {
            decimal abilityNum = 0;
            List<GeneralProperty> propertyList = new List<GeneralProperty>(AttributeConversion(userID, powerNum, soulNum, intellectNum));
            foreach (var property in propertyList)
            {
                if (property.AbilityType == abilityType)
                {
                    abilityNum = MathUtils.RoundCustom(MathUtils.Addition(abilityNum, property.AbilityValue));
                }
            }
            return abilityNum;
        }

        /// <summary>
        /// 法宝每战斗M次就扣除N点寿命
        /// </summary>
        /// <param name="userID"></param>
        public static void CombatTrumpLift(string userID)
        {
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            if (userTrump != null && user != null && user.UserExtend != null)
            {
                user.UserExtend.UpdateNotify(obj =>
                {
                    user.UserExtend.TrumpCombat = MathUtils.Addition(user.UserExtend.TrumpCombat, 1);
                    return true;
                });
                if (user.UserExtend.TrumpCombat >= GameConfigSet.TrumpCombatNum)
                {
                    userTrump.LiftNum = MathUtils.Subtraction(userTrump.LiftNum, GameConfigSet.TrumpLifeNum);
                    user.UserExtend.UpdateNotify(obj =>
                    {
                        user.UserExtend.TrumpCombat = 0;
                        return true;
                    });
                }
                var usergeneral = UserGeneral.GetMainGeneral(userID);
                if (userTrump.LiftNum <= 0 && usergeneral != null)
                {
                    usergeneral.RefreshMaxLife();
                }
            }
        }

        /// <summary>
        /// 佣兵战斗死亡扣除N点寿命
        /// </summary>
        /// <param name="userID"></param>
        public static void GeneralOverTrumpLift(string userID, int generalID)
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            if (userTrump != null && generalID == LanguageManager.GetLang().GameUserGeneralID)
            {
                userTrump.LiftNum = MathUtils.Subtraction(userTrump.LiftNum, GameConfigSet.GeneralOverLife);
                var usergeneral = UserGeneral.GetMainGeneral(userID);
                if (userTrump.LiftNum <= 0 && usergeneral != null)
                {
                    usergeneral.RefreshMaxLife();
                }
            }
        }

        /// <summary>
        /// 属相克制伤害
        /// </summary>
        /// <param name="userID1"></param>
        /// <param name="userID2"></param>
        /// <returns></returns>
        public static decimal TrumpZodiacHarm(CombatGeneral general, string userID2)
        {
            if (general.GeneralID != LanguageManager.GetLang().GameUserGeneralID)
            {
                return 0;
            }
            ZodiacType zodiacType1 = GetZodiacType(general.UserID);
            ZodiacType zodiacType2 = GetZodiacType(userID2);
            return ZodiacRestraint(zodiacType1, zodiacType2);
        }


        public static ZodiacType GetZodiacType(string userID)
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            if (userTrump != null)
            {
                return userTrump.Zodiac;
            }
            return ZodiacType.NoZodiac;
        }

        /// <summary>
        /// 克制属相
        /// </summary>
        /// <param name="zodiacType1">玩家属相1</param>
        /// <param name="zodiacType2">玩家属相2</param>
        /// <returns></returns>
        public static decimal ZodiacRestraint(ZodiacType zodiacType1, ZodiacType zodiacType2)
        {
            string[] zodiacArray = ConfigEnvSet.GetString("Trump.ZodiacRestraint").Split(',');
            foreach (string s in zodiacArray)
            {
                string[] zodiacStr = s.Split('=');

                if (zodiacStr.Length > 2 && zodiacType1 == zodiacStr[0].ToEnum<ZodiacType>())
                {
                    string[] zodiacList = zodiacStr[1].Split('|');
                    foreach (string s1 in zodiacList)
                    {
                        if (s1.ToEnum<ZodiacType>() == zodiacType2)
                        {
                            return zodiacStr[2].ToDecimal();
                        }
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 法宝等级属性数值
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static short TrumpPropertyNum(string userID, int generalID, AbilityType abilityType)
        {
            short proNum = 0;
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            if (userTrump != null && userTrump.LiftNum > 0 && generalID == LanguageManager.GetLang().GameUserGeneralID)
            {
                TrumpInfo trumpInfo = new ConfigCacheSet<TrumpInfo>().FindKey(TrumpInfo.CurrTrumpID, userTrump.TrumpLv);
                if (trumpInfo != null && trumpInfo.Property.Count > 0)
                {
                    decimal mature = (decimal)userTrump.MatureNum / GameConfigSet.MaxMatureNum;
                    foreach (var property in trumpInfo.Property)
                    {
                        if (property.AbilityType == abilityType)
                        {
                            proNum = (short)MathUtils.RoundCustom(MathUtils.Addition(proNum, mature * property.AbilityValue));
                        }
                    }
                }
            }
            return proNum;
        }

        /// <summary>
        /// 法宝附加属性
        /// </summary>
        /// <param name="trumpInfo"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static decimal TrumpGeneralProperty(string userID, int generalID, AbilityType abilityType)
        {
            decimal propertyNum = 0;
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            if (userTrump != null && userTrump.LiftNum > 0 && generalID == LanguageManager.GetLang().GameUserGeneralID && userTrump.PropertyInfo.Count > 0)
            {
                GeneralProperty property = userTrump.PropertyInfo.Find(m => m.AbilityType == abilityType);
                if (property != null)
                {
                    propertyNum = property.AbilityValue;
                }
            }
            return propertyNum;
        }

        /// <summary>
        /// 法宝触发技能施放
        /// </summary>
        /// <param name="abilityType"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static SkillInfo GetSkillprocess(AbilityType abilityType, int num)
        {
            SkillInfo skillInfo = new SkillInfo();
            skillInfo.AbilityID = (int)abilityType;
            skillInfo.Num = num;
            return skillInfo;
            //var skillLvList = new ConfigCacheSet<SkillLvInfo>().FindAll(m => m.EffType == abilityType);
            //if (skillLvList.Count > 0)
            //{
            //    skillInfo.AbilityID = skillLvList[0].SkillID;
            //    skillInfo.Num = num;
            //}
            //return skillInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static short GetprocessAbility(AbilityType abilityType)
        {
            var skillLvList = new ConfigCacheSet<SkillLvInfo>().FindAll(m => m.EffType == abilityType);
            if (skillLvList.Count > 0)
            {
                return skillLvList[0].SkillID;
            }
            return (short)0;
        }
    }
}
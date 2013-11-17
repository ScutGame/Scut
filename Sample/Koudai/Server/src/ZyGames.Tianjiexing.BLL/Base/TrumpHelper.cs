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
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Combat;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 法宝帮助
    /// </summary>
    public class TrumpHelper
    {
        /// <summary>
        /// 法宝是可修炼
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool IsTrumpPractice(string userID)
        {
            StoryTaskInfo[] storyTaskArray = new ConfigCacheSet<StoryTaskInfo>().FindAll(m => m.TaskType == TaskType.Trump).ToArray();
            foreach (StoryTaskInfo taskInfo in storyTaskArray)
            {
                int collectNum = GetUserItemNum(userID, taskInfo.TargetItemID);
                if (collectNum < taskInfo.TargetItemNum)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 法宝基础属性
        /// </summary>
        /// <param name="trumpInfo"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static short GetTrumpProperty(TrumpInfo trumpInfo, AbilityType abilityType)
        {
            short propertyNum = 0;
            if (trumpInfo.Property.Count > 0)
            {
                GeneralProperty property = trumpInfo.Property.Find(m => m.AbilityType == abilityType);
                if (property != null)
                {
                    propertyNum = (short)property.AbilityValue;
                }
            }
            return propertyNum;
        }

        /// <summary>
        /// 收集到物品的数量
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static int GetUserItemNum(string userID, int itemID)
        {
            int itemNum = 0;
            var package = UserItemPackage.Get(userID);
            var useritem = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemID == itemID);
            foreach (var info in useritem)
            {
                itemNum = MathUtils.Addition(itemNum, info.Num);
            }
            return itemNum;
        }

        /// <summary>
        /// 法宝全部开启的技能
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, short> GetSkillList()
        {
            Dictionary<int, short> skillList = new Dictionary<int, short>();
            TrumpInfo[] trumpInfoArray = new ConfigCacheSet<TrumpInfo>().FindAll(m => m.TrumpID == TrumpInfo.CurrTrumpID && m.SkillID > 0).ToArray();
            foreach (TrumpInfo info in trumpInfoArray)
            {
                if (!skillList.ContainsKey(info.SkillID.ToInt()))
                {
                    skillList.Add(info.SkillID.ToInt(), info.TrumpLv);
                }
            }
            return skillList;
        }

        /// <summary>
        /// 法宝全部开启的属性
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, short> GetPropertyList()
        {
            Dictionary<int, short> propertyList = new Dictionary<int, short>();
            WorshipInfo[] worshipInfoInfoArray = new ConfigCacheSet<WorshipInfo>().FindAll(m => m.TrumpID == TrumpInfo.CurrTrumpID && m.IsOpen).ToArray();
            int skID = 0;
            foreach (WorshipInfo info in worshipInfoInfoArray)
            {
                skID++;
                if (!propertyList.ContainsKey(skID))
                {
                    propertyList.Add(skID, info.WorshipLv);
                }
            }
            return propertyList;
        }

        /// <summary>
        /// 成长消耗
        /// </summary>
        /// <param name="matureID"></param>
        /// <returns></returns>
        public static WashConsumeInfo GetWashConsumeInfo(short matureID)
        {
            short matureType = 0;
            var washconsumeList = new ConfigCacheSet<WashConsumeInfo>().FindAll(m => m.TrumpID == TrumpInfo.CurrTrumpID);
            int mID = 0;
            foreach (var washConsume in washconsumeList)
            {
                mID = MathUtils.Addition(mID, 1);
                if (matureID >= washConsume.MatureType)
                {
                    continue;
                }
                matureType = washConsume.MatureType;
                break;
            }
            WashConsumeInfo consumeInfo = new ConfigCacheSet<WashConsumeInfo>().FindKey(TrumpInfo.CurrTrumpID, matureType);
            return consumeInfo;
        }

        /// <summary>
        /// 成长值枚举
        /// </summary>
        /// <param name="matureID"></param>
        /// <returns></returns>
        public static MatureType GetEnumMatureType(short matureID)
        {
            MatureType matureType = MatureType.Ordinary;
            var washconsumeList = new ConfigCacheSet<WashConsumeInfo>().FindAll(m => m.TrumpID == TrumpInfo.CurrTrumpID);
            int mID = 0;
            foreach (var washConsume in washconsumeList)
            {
                mID = MathUtils.Addition(mID, 1);
                if (matureID > washConsume.MatureType)
                {
                    continue;
                }
                matureType = mID.ToEnum<MatureType>();
                break;
            }
            return matureType;
        }

        /// <summary>
        /// 成长值数值
        /// </summary>
        /// <param name="matureNum"></param>
        /// <returns></returns>
        public static decimal GetMatureNum(short matureNum)
        {
            return (decimal)matureNum / GameConfigSet.MaxMatureNum;
        }

        /// <summary>
        /// 获取玩家法宝的技能
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        public static SkillInfo GetSkillInfo(string userID, int skillKey)
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            SkillInfo skillInfo = null;
            if (userTrump != null && userTrump.SkillInfo.Count > skillKey)
            {
                skillInfo = userTrump.SkillInfo[skillKey];
            }
            return skillInfo;
        }

        /// <summary>
        /// 获取玩家法宝的技能
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        public static AbilityInfo GetAbilityInfo(string userID, int skillKey)
        {
            AbilityInfo abilityInfo = null;
            SkillInfo skillInfo = GetSkillInfo(userID, skillKey);
            if (skillInfo != null)
            {
                abilityInfo = new ConfigCacheSet<AbilityInfo>().FindKey(skillInfo.AbilityID);
            }
            return abilityInfo;
        }

        /// <summary>
        /// 修复玩家法宝技能
        /// </summary>
        /// <param name="userID"></param>
        public static void RepairMagicSkills(string userID)
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            if (userTrump != null)
            {
                Dictionary<int, short> skillList = GetSkillList();
                while (userTrump.SkillInfo.Count > skillList.Count)
                {
                    userTrump.SkillInfo.Remove(userTrump.SkillInfo[skillList.Count]);
                }
                int i = 0;
                foreach (KeyValuePair<int, short> pair in skillList)
                {
                    if (userTrump.TrumpLv >= pair.Value && userTrump.SkillInfo.Count <= i)
                    {
                        SkillInfo skillInfo = new SkillInfo();
                        skillInfo.AbilityID = pair.Key;
                        skillInfo.AbilityLv = 1;
                        userTrump.SkillInfo.Add(skillInfo);
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// 法宝加经验，升级
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="experience"></param>
        public static void CheckTrumpEscalate(string userID, int experience)
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            if (userTrump != null)
            {
                userTrump.Experience = MathUtils.Addition(userTrump.Experience, experience);
                while (userTrump.TrumpLv < GameConfigSet.MaxTrumpLv)
                {
                    short upLv = MathUtils.Addition(userTrump.TrumpLv, (short)1, GameConfigSet.MaxTrumpLv.ToShort());
                    TrumpInfo trumpInfo = new ConfigCacheSet<TrumpInfo>().FindKey(TrumpInfo.CurrTrumpID, upLv);
                    if (trumpInfo != null && userTrump.Experience >= trumpInfo.Experience)
                    {
                        userTrump.TrumpLv = MathUtils.Addition(userTrump.TrumpLv, (short)1, GameConfigSet.MaxTrumpLv.ToShort());
                        userTrump.Experience = MathUtils.Subtraction(userTrump.Experience, trumpInfo.Experience);
                        if (trumpInfo.SkillID > 0)
                        {
                            SkillInfo skillInfo = new SkillInfo();
                            skillInfo.AbilityID = trumpInfo.SkillID.ToInt();
                            skillInfo.AbilityLv = 1;
                            userTrump.SkillInfo.Add(skillInfo);
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                if (userTrump.TrumpLv >= GameConfigSet.MaxTrumpLv)
                {
                    userTrump.Experience = 0;
                }
            }
        }

        /// <summary>
        /// 技能升级是否满足条件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="skillLvInfo"></param>
        /// <returns></returns>
        public static bool IsMeetUpSkillLv(GameUser user, SkillLvInfo skillLvInfo)
        {
            int upItemNum = GetUserItemNum(user.UserID, skillLvInfo.ItemID);
            if (skillLvInfo != null && user.GameCoin >= skillLvInfo.GameCoin && user.ObtainNum >= skillLvInfo.ObtainNum && upItemNum >= skillLvInfo.ItemNum)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 附加属性升级是否满足条件
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsMeetUpPropertyLv(GameUser user, TrumpPropertyInfo trumpProperty)
        {
            int upItemNum = GetUserItemNum(user.UserID, trumpProperty.ItemID);
            if (trumpProperty != null && user.GameCoin >= trumpProperty.GameCoin && user.ObtainNum >= trumpProperty.ObtainNum && upItemNum >= trumpProperty.ItemNum)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否可祭祀
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsUpWorshLv(GameUser user)
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(user.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump != null)
            {
                short upWorshLv = MathUtils.Addition(userTrump.WorshipLv, (short)1, (short)10);
                WorshipInfo worshipInfo = new ConfigCacheSet<WorshipInfo>().FindKey(TrumpInfo.CurrTrumpID, upWorshLv);
                if (worshipInfo != null)
                {
                    int upItemNum = GetUserItemNum(user.UserID, worshipInfo.ItemID);
                    if (user.GameCoin >= worshipInfo.GameCoin && user.ObtainNum >= worshipInfo.ObtainNum &&
                        upItemNum >= worshipInfo.ItemNum)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 附加属性是否可学习
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsLearnProperty(GameUser user)
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(user.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump != null)
            {
                WorshipInfo[] worshipInfoArray = new ConfigCacheSet<WorshipInfo>().FindAll(m => m.TrumpID == TrumpInfo.CurrTrumpID && m.WorshipLv <= userTrump.WorshipLv && m.IsOpen).ToArray();
                if (worshipInfoArray.Length > userTrump.PropertyInfo.Count)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 被克制属相
        /// </summary>
        /// <param name="zodiacType">玩家属相</param>
        /// <param name="typeID">类型 1.克制 2 被克制 3.全部属相</param>
        /// <returns></returns>
        public static Dictionary<short, decimal> ZodiacRestraint(ZodiacType zodiacType, int typeID)
        {
            Dictionary<short, decimal> zodiacTDList = new Dictionary<short, decimal>();
            string[] zodiacArray = ConfigEnvSet.GetString("Trump.ZodiacRestraint").Split(',');
            foreach (string s in zodiacArray)
            {
                string[] zodiacStr = s.Split('=');
                if (typeID == 1)
                {
                    if (zodiacStr.Length > 2 && zodiacType == zodiacStr[0].ToEnum<ZodiacType>())
                    {
                        string[] zodiacList = zodiacStr[1].Split('|');
                        foreach (string s1 in zodiacList)
                        {
                            zodiacTDList.Add(s1.ToShort(), zodiacStr[2].ToDecimal());
                        }
                    }
                }
                if (typeID == 2)
                {
                    if (zodiacStr.Length > 2)
                    {
                        string[] zodiacList = zodiacStr[1].Split('|');
                        foreach (string s1 in zodiacList)
                        {
                            if (s1 != ZodiacType.NoZodiac.ToString() && s1.ToEnum<ZodiacType>() == zodiacType)
                            {
                                zodiacTDList.Add(s1.ToShort(), zodiacStr[0].ToDecimal());
                            }
                        }
                    }
                }
                if (typeID == 3)
                {
                    if (zodiacStr.Length > 2 && zodiacStr[0].ToEnum<ZodiacType>() != zodiacType)
                    {
                        zodiacTDList.Add(s.ToShort(), zodiacStr[2].ToDecimal());
                    }
                }
            }
            return zodiacTDList;
        }

        /// <summary>
        /// 随机一个属相
        /// </summary>
        /// <param name="zodiacType"></param>
        /// <returns></returns>
        public static ZodiacType GetZodiacType(ZodiacType zodiacType)
        {
            ZodiacType zType = new ZodiacType();
            Array zodiacTDArray = Enum.GetValues(typeof(ZodiacType));
            List<ZodiacType> zodiacTDList = new List<ZodiacType>();
            foreach (ZodiacType array in zodiacTDArray)
            {
                if (array.ToEnum<ZodiacType>() == zodiacType || array.ToEnum<ZodiacType>() == ZodiacType.NoZodiac)
                {
                    continue;
                }
                zodiacTDList.Add(array);
            }
            if (zodiacTDList.Count > 0)
            {
                int index2 = RandomUtils.GetRandom(0, zodiacTDList.Count);
                zType = zodiacTDList[index2].ToEnum<ZodiacType>();
            }
            return zType;
        }

        /// <summary>
        /// 随机一个技能
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static AbilityInfo GetRandomAbility(string userID)
        {
            AbilityInfo changeAbility = new AbilityInfo();
            int abilityStyle = 1;
            var abilityInfoList = new ConfigCacheSet<AbilityInfo>().FindAll(m => m.AbilityStyle == abilityStyle);
            if (abilityInfoList.Count > 0)
            {
                UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
                if (userTrump != null && userTrump.SkillInfo.Count > 0)
                {
                    foreach (SkillInfo info in userTrump.SkillInfo)
                    {
                        AbilityInfo abilityInfo = new ConfigCacheSet<AbilityInfo>().FindKey(info.AbilityID);
                        if (abilityInfo != null)
                        {
                            abilityInfoList.Remove(abilityInfo);
                        }
                    }
                }
                int randomNum = RandomUtils.GetRandom(0, abilityInfoList.Count);
                changeAbility = abilityInfoList[randomNum];
            }
            return changeAbility;
        }

        /// <summary>
        /// 法宝技能--随机技能不包括玩家身上的
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static AbilityInfo IsRandomAbility(string userID)
        {
            AbilityInfo abilityInfo = null;
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            foreach (SkillInfo info in userTrump.SkillInfo)
            {
                abilityInfo = new ConfigCacheSet<AbilityInfo>().FindKey(info.AbilityID);
            }
            return abilityInfo;
        }

        /// <summary>
        /// 小于0的数据加%
        /// </summary>
        /// <returns></returns>
        public static string GetTransformData(decimal dataNum)
        {
            string dateStr = string.Empty;
            if (dataNum >= 1)
            {
                dateStr = Math.Floor(dataNum).ToString();
            }
            if (dataNum > 0 && dataNum < 1)
            {
                double str = (double)dataNum * 100;
                dateStr = MathUtils.RoundCustom(str, 3) + "%";
            }
            return dateStr;
        }
    }
}
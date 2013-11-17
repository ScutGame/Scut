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
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Common;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class GiftHelper
    {
        public static short maxFeelLv = ConfigEnvSet.GetInt("Gift.MaxFeelLv").ToShort(); //好感度最大等级
        /// <summary>
        /// 礼物类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGiftTypeName(GiftType type)
        {
            string result = string.Empty;
            switch (type)
            {
                case GiftType.Food:
                    result = LanguageManager.GetLang().GiftType_Food;
                    break;
                case GiftType.Kitchenware:
                    result = LanguageManager.GetLang().GiftType_Kitchenware;
                    break;
                case GiftType.Mechanical:
                    result = LanguageManager.GetLang().GiftType_Mechanical;
                    break;
                case GiftType.Books:
                    result = LanguageManager.GetLang().GiftType_Books;
                    break;
                case GiftType.MusicalInstruments:
                    result = LanguageManager.GetLang().GiftType_MusicalInstruments;
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 好感度晶石剩余赠送次数
        /// </summary>
        /// <returns></returns>
        public static Int16 SurplusGoldNum(string userID)
        {
            short goldNum = 0;
            UserDailyRestrain userDaily = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            DailyRestrainSet restrainSet =
                new ShareCacheStruct<DailyRestrainSet>().FindKey(RestrainType.PresentationGoldNum);
            if (restrainSet != null)
            {
                if (userDaily != null && userDaily.UserExtend != null && userDaily.RefreshDate.Date == DateTime.Now.Date)
                {
                    goldNum = (short)MathUtils.Subtraction(restrainSet.MaxNum, userDaily.UserExtend.GoldNum, 0);
                }
                else
                {
                    goldNum = (short)restrainSet.MaxNum;
                }
            }
            return goldNum;
        }

        /// <summary>
        /// 下一级属性数值
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static int FeelUpPropertyNum(string userID, int generalID, AbilityType abilityType)
        {
            int upPropertyNum = 0;
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            if (userGeneral != null)
            {
                short maxFeelLv = ConfigEnvSet.GetInt("Gift.MaxFeelLv").ToShort();
                short feelLv = MathUtils.Addition(userGeneral.FeelLv, (short)1, maxFeelLv);
                FeelLvInfo upfeelLvInfo = new ConfigCacheSet<FeelLvInfo>().FindKey(feelLv);
                if (upfeelLvInfo != null && upfeelLvInfo.Property.Count > 0)
                {
                    GeneralProperty property = upfeelLvInfo.Property.Find(m => m.AbilityType == abilityType);
                    if (property != null)
                    {
                        upPropertyNum = property.AbilityValue.ToInt();
                    }
                }
            }
            return upPropertyNum;
        }

        /// <summary>
        /// 好感度升级
        /// </summary>
        /// <param name="general"></param>
        /// <param name="experience"></param>
        public static void GeneralFeelUpgrade(UserGeneral general, int experience, short saturationNum)
        {
            if (general != null)
            {
                int maxSatiationNum = ConfigEnvSet.GetInt("User.FeelMaxSatiationNum");
                general.FeelExperience = MathUtils.Addition(general.FeelExperience, experience);
                general.SaturationNum = MathUtils.Addition(general.SaturationNum, saturationNum, (short)maxSatiationNum);
                while (true)
                {
                    short feelLv = MathUtils.Addition(general.FeelLv, (short)1, maxFeelLv);
                    FeelLvInfo upfeelLvInfo = new ConfigCacheSet<FeelLvInfo>().FindKey(feelLv);
                    if (general.FeelLv < feelLv && upfeelLvInfo != null)
                    {
                        if (general.FeelExperience >= upfeelLvInfo.Experience)
                        {
                            general.FeelLv = MathUtils.Addition(general.FeelLv, (short)1, maxFeelLv);
                            general.FeelExperience = MathUtils.Subtraction(general.FeelExperience, upfeelLvInfo.Experience, 0);
                            general.RefreshMaxLife();
                            GeneralInfo generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(general.GeneralID);
                            if (generalInfo != null && generalInfo.ReplaceSkills != null)
                            {
                                if (general.FeelLv >= generalInfo.ReplaceSkills.FeelLv && general.AbilityID != generalInfo.ReplaceSkills.AbilityID)
                                {
                                    general.AbilityID = generalInfo.ReplaceSkills.AbilityID;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
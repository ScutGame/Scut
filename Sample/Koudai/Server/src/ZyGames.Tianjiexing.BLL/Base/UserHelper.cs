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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model.DataModel;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Com;
using ZyGames.Tianjiexing.Component;
using ZyGames.Framework.Game.Com.Rank;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class UserHelper
    {
        public static short _currMaxLv = ObjectExtend.ToShort(ConfigEnvSet.GetInt("User.CurrMaxLv")); //玩家最大等级
        /// <summary>
        /// 获取生命上限,等级*职业初始值+装备+命运水晶(未完成)
        /// </summary>
        /// <returns></returns>
        public static int GetMaxLife(string userID, int generalID)
        {
            //修改者wuzf 2012-04-19
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);

            return general == null ? 0 : general.LifeMaxNum;
        }

        /// <summary>
        /// 修复命运水晶等级
        /// </summary>
        public static void VerificationCrystalLv(string userID)
        {
            var packageCrystal = UserCrystalPackage.Get(userID);
            UserCrystalInfo[] userCrystalArray = packageCrystal.CrystalPackage.FindAll(m => m.CrystalLv > 1).ToArray();
            foreach (UserCrystalInfo userCrystal in userCrystalArray)
            {
                for (int i = 0; i < 10; i++)
                {
                    CrystalLvInfo crystalLvInfo = new ConfigCacheSet<CrystalLvInfo>().FindKey(userCrystal.CrystalID, userCrystal.CrystalLv);
                    if (crystalLvInfo != null && crystalLvInfo.UpExperience > userCrystal.CurrExprience)
                    {
                        userCrystal.CrystalLv = MathUtils.Subtraction(userCrystal.CrystalLv, (short)1, (short)1);
                        packageCrystal.SaveCrystal(userCrystal);
                        //packageCrystal.DelayChange();
                    }
                }
            }
        }

        /// <summary>
        /// 命运水晶升级
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        public static void CheckCrystalEscalate(string userID, string userCrystalID)
        {
            var packageCrystal = UserCrystalPackage.Get(userID);

            UserCrystalInfo userCrystal = packageCrystal.CrystalPackage.Find(m => m.UserCrystalID.Equals(userCrystalID));
            if (userCrystal != null)
            {
                for (int i = userCrystal.CrystalLv; i <= 10; i++)
                {
                    int upLv = MathUtils.Addition((int)userCrystal.CrystalLv, 1);
                    CrystalLvInfo crystalLvInfo = new ConfigCacheSet<CrystalLvInfo>().FindKey(userCrystal.CrystalID, upLv);

                    if (crystalLvInfo != null && userCrystal.CurrExprience >= crystalLvInfo.UpExperience)
                    {
                        userCrystal.CrystalLv = MathUtils.Addition(userCrystal.CrystalLv, (short)1, short.MaxValue);
                        packageCrystal.SaveCrystal(userCrystal);
                        //packageCrystal.DelayChange();
                    }

                }
                if (userCrystal.CrystalLv > 10)
                {
                    userCrystal.CrystalLv = 10;
                    packageCrystal.SaveCrystal(userCrystal);
                    //packageCrystal.DelayChange();
                }
            }
        }


        /// <summary>
        /// 普通培养用钱
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static int GetCultureMoney(string userID, int generalID)
        {
            int cultureMoney = 0;
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            if (general != null && !string.IsNullOrEmpty(ConfigEnvSet.GetString("UserGeneral.GameCoinCulture")))
            {
                string[] coinCulture = ConfigEnvSet.GetString("UserGeneral.GameCoinCulture").Split(',');
                foreach (string s in coinCulture)
                {
                    string[] strCulture = s.Split('=');
                    if (strCulture.Length > 1 && strCulture[0].ToInt() < general.GeneralLv.ToInt())
                    {
                        cultureMoney = strCulture[1].ToInt();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return cultureMoney;
        }

        /// <summary>
        /// 培养类型
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static List<TrainingInfo> GetCultureType(string userID, int generalID)
        {
            List<TrainingInfo> cultureList = new List<TrainingInfo>();
            cultureList.Add(GetTrainingType(userID, generalID, CultureType.PuTong));
            cultureList.Add(GetTrainingType(userID, generalID, CultureType.JiaQiang));
            cultureList.Add(GetTrainingType(userID, generalID, CultureType.BaiJin));
            cultureList.Add(GetTrainingType(userID, generalID, CultureType.ZuanShi));
            cultureList.Add(GetTrainingType(userID, generalID, CultureType.ZhiZun));
            return cultureList;
        }

        public static TrainingInfo GetTrainingType(string userID, int generalID, CultureType cultureType)
        {
            string jiaQiang = ConfigEnvSet.GetString("User.JiaQiangTraining");
            string BaiJin = ConfigEnvSet.GetString("User.BaiJinTraining");
            string ZuanShi = ConfigEnvSet.GetString("User.ZuanShiTraining");
            string ZhiZun = ConfigEnvSet.GetString("User.ZhiZunTraining");
            string var = string.Empty;
            if (cultureType == CultureType.PuTong)
            {
                var = GetCultureMoney(userID, generalID).ToString() + LanguageManager.GetLang().GameMoney_Coin;
            }
            else if (cultureType == CultureType.JiaQiang)
            {
                var = jiaQiang + LanguageManager.GetLang().GameMoney_Gold;
            }
            else if (cultureType == CultureType.BaiJin)
            {
                var = BaiJin + LanguageManager.GetLang().GameMoney_Gold;
            }
            else if (cultureType == CultureType.ZuanShi)
            {
                var = ZuanShi + LanguageManager.GetLang().GameMoney_Gold;
            }
            else if (cultureType == CultureType.ZhiZun)
            {
                var = ZhiZun + LanguageManager.GetLang().GameMoney_Gold;
            }
            return new TrainingInfo() { CultureID = cultureType, CultureNum = var };
        }

        /// <summary>
        /// 公会boss挑战时间列表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static List<BossDate> GetBossDate()
        {
            List<BossDate> bossDates = new List<BossDate>();
            bossDates.Add(GetBossDate(BossDateType.Monday));
            bossDates.Add(GetBossDate(BossDateType.Tuesday));
            bossDates.Add(GetBossDate(BossDateType.Wednesday));
            bossDates.Add(GetBossDate(BossDateType.Thursday));
            bossDates.Add(GetBossDate(BossDateType.Friday));
            bossDates.Add(GetBossDate(BossDateType.Saturday));
            bossDates.Add(GetBossDate(BossDateType.SundayAfternoon));
            bossDates.Add(GetBossDate(BossDateType.Sunday));
            return bossDates;
        }

        public static BossDate GetBossDate(BossDateType bossDate)
        {
            string bossTime = string.Empty;

            if (bossDate == BossDateType.SundayAfternoon)
            {
                bossTime = "16:00:00";
            }
            else if (bossDate == BossDateType.Sunday)
            {
                bossTime = "22:00:00";
            }
            else
            {
                bossTime = "21:00:00";
            }
            return new BossDate() { EnableWeek = bossDate, EnablePeriod = bossTime };
        }

        /// <summary>
        /// 是否本周时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsCurrentWeek(DateTime dateTime)
        {
            DateTime currDt = DateTime.Now.Date;
            int currWeek = currDt.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)currDt.DayOfWeek;
            var fromDate = currDt.AddDays((int)DayOfWeek.Monday - currWeek);
            var toDate = fromDate.AddDays(7);
            if (fromDate <= dateTime.Date && toDate > dateTime.Date)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 玩家战力
        /// </summary>
        /// <param name="userID"></param>
        public static void GetGameUserCombat(string userID)
        {
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (user != null)
            {
                user.CombatNum = GetTotalCombatNum(userID);
            }
        }

        /// <summary>
        /// 玩家战力
        /// </summary>
        /// <returns></returns>
        public static int GetTotalCombatNum(string userID)
        {
            int totalCombatNum = 0;
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (user == null)
            {
                UserCacheGlobal.LoadOffline(userID);
                user = new GameDataCacheSet<GameUser>().FindKey(userID);
            }
            if (user != null)
            {
                UserMagic[] magicsArray = new GameDataCacheSet<UserMagic>().FindAll(userID, u => u.IsEnabled && u.MagicType == MagicType.MoFaZhen).ToArray();
                if (magicsArray.Length > 0)
                {
                    int magicID = magicsArray[0].MagicID;
                    int combatNum = 0;
                    IList<UserEmbattle> embattleArray = new GameDataCacheSet<UserEmbattle>().FindAll(user.UserID, m => m.MagicID == magicID);
                    if (embattleArray.Count > 0)
                    {
                        foreach (UserEmbattle embattle in embattleArray)
                        {
                            var wuliNum = CombatHelper.GetPropertyNum(user.UserID, embattle.GeneralID, AbilityType.WuLiGongJi);
                            var hunjiNum = CombatHelper.GetPropertyNum(user.UserID, embattle.GeneralID, AbilityType.HunJiGongJi);
                            var mofaNum = CombatHelper.GetPropertyNum(userID, embattle.GeneralID, AbilityType.MoFaGongJi);
                            combatNum = MathUtils.Addition(combatNum, wuliNum);
                            combatNum = MathUtils.Addition(combatNum, hunjiNum);
                            combatNum = MathUtils.Addition(combatNum, mofaNum);
                        }
                    }
                    totalCombatNum = (combatNum / 3);//;
                    user.CombatNum = totalCombatNum;
                    //todo CacheRank
                    //CacheRank cacheRank = new CacheRank();
                    //UserRank rankInfo = cacheRank.GetRankUserID(user.UserID);
                    //if (rankInfo != null)
                    //{
                    //    rankInfo.TotalCombatNum = totalCombatNum;
                    //}
                }
            }
            return totalCombatNum;
        }

        /// <summary>
        /// 命运水晶系统-佣兵开启的格子
        /// </summary>
        /// <param name="generalLv"></param>
        /// <returns></returns>
        public static short GetOpenNum(short generalLv)
        {
            short openNum = 0;
            if (generalLv >= 20 && generalLv < 30)
            {
                openNum = 1;
            }
            else if (generalLv >= 30 && generalLv < 40)
            {
                openNum = 2;
            }
            else if (generalLv >= 40 && generalLv < 50)
            {
                openNum = 3;
            }
            else if (generalLv >= 50 && generalLv < 60)
            {
                openNum = 4;
            }
            else if (generalLv >= 60 && generalLv < 70)
            {
                openNum = 5;
            }
            else if (generalLv >= 70 && generalLv < 80)
            {
                openNum = 6;
            }
            else if (generalLv >= 80 && generalLv < 90)
            {
                openNum = 7;
            }
            else if (generalLv >= 90)
            {
                openNum = 8;
            }
            return openNum;
        }

        public static DateTime GetShopsSparRefrshDate(GameUser user)
        {
            return user.UserExtend != null && user.UserExtend.UserShops.Count > 0 ?
                user.UserExtend.UserShops[0].NextDate :
                DateTime.MinValue;
        }
        /// <summary>
        /// 刷新神秘商店物品
        /// </summary>
        public static void RefrshShopsSparData(GameUser user, bool isGold)
        {
            int intervalDate = ConfigEnvSet.GetInt("MysteryShops.IntervalDate");
            DateTime nextDate = new DateTime();

            if (user.UserExtend == null) user.UserExtend = new GameUserExtend();

            if (isGold)
            {
                nextDate = nextDate.AddHours(intervalDate);
            }
            else
            {
                nextDate = user.UserExtend.UserShops.Count > 0 ? user.UserExtend.UserShops[0].NextDate : DateTime.Now;

                if (nextDate < DateTime.Now)
                {
                    int intervalHours = (int)(DateTime.Now - nextDate).TotalHours;
                    int tntervalNum = MathUtils.Addition((intervalHours / intervalDate), 1) * intervalDate;
                    nextDate = nextDate.AddHours(tntervalNum);
                }
            }
            CacheList<MysteryShops> mysteryList = new CacheList<MysteryShops>();
            var itemList = new List<ItemType>();
            itemList.Add(ItemType.CaiLiao);
            itemList.Add(ItemType.CaiLiao);
            itemList.Add(ItemType.ZhuangBei);
            itemList.Add(ItemType.DaoJu);
            itemList.Add(ItemType.TuZhi);
            itemList.Add(ItemType.DaoJu);
            SetRandomItem(mysteryList, itemList, nextDate);

            if (mysteryList.Count > 0)
            {
                user.UserExtend.UpdateNotify(obj =>
                    {
                        user.UserExtend.UserShops = mysteryList;
                        return true;
                    });
                //user.Update();
            }
        }

        private static void SetRandomItem(CacheList<MysteryShops> mysteryList, List<ItemType> itemList, DateTime nextDate)
        {
            foreach (ItemType itemType in itemList)
            {
                List<ItemBaseInfo> itemArray = new ConfigCacheSet<ItemBaseInfo>().FindAll(u => u.IsMystery == 1 && u.ItemType == itemType);
                if (itemArray.Count == 0)
                {
                    throw new Exception("刷新神秘商店出错：物品" + itemType + "类型不存在");
                }
                ItemBaseInfo itemInfo = itemArray[RandomUtils.GetRandom(0, itemArray.Count)];

                if (mysteryList.Exists(m => m.ItemID == itemInfo.ItemID))
                {
                    itemArray = new ConfigCacheSet<ItemBaseInfo>().FindAll(u => u.IsMystery == 1 && u.ItemID != itemInfo.ItemID && u.ItemType == itemType);
                    if (itemArray.Count == 0)
                    {
                        throw new Exception("刷新神秘商店出错：物品" + itemType + "类型不存在");
                    }
                    itemInfo = itemArray[RandomUtils.GetRandom(0, itemArray.Count)];
                }

                mysteryList.Add(new MysteryShops() { ItemID = itemInfo.ItemID, NextDate = nextDate, BuyNum = 0, ItemNum = itemInfo.MysteryNum });


            }
        }


        /// <summary>
        /// 处理聊天内容，屏蔽禁言
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string ProChat(string Content)
        {
            string result = Content;
            var chatKeyWordArray = new ConfigCacheSet<ChatKeyWord>().FindAll();
            foreach (ChatKeyWord item in chatKeyWordArray)
            {
                result = result.Replace(item.KeyWord, new string('*', item.KeyWord.Length));
            }
            return result;
        }



        /// <summary>
        /// 佣兵加血
        /// </summary>
        public static void GetGeneralLife(string userID)
        {
            List<UserProps> userPropsArray = new GameDataCacheSet<UserProps>().FindAll(userID, m => m.SurplusNum > 0 && m.ItemID != 7003);
            List<UserGeneral> generalArray = new GameDataCacheSet<UserGeneral>().FindAll(userID, u => u.GeneralStatus == GeneralStatus.DuiWuZhong);
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (userPropsArray.Count > 0)
            {
                UserProps userProps = userPropsArray[0];
                foreach (UserGeneral general in generalArray)
                {
                    if (general.LifeNum < general.LifeMaxNum)
                    {
                        int subLifeNum = MathUtils.Subtraction(general.LifeMaxNum, general.LifeNum, 0);
                        general.LifeNum = MathUtils.Addition(general.LifeNum, userProps.SurplusNum, general.LifeMaxNum);
                        //general.Update();
                        userProps.SurplusNum = MathUtils.Subtraction(userProps.SurplusNum, subLifeNum, 0);
                    }
                }
                if (userProps.SurplusNum == 0 && user != null)
                {
                    user.IsUseupItem = true;
                }
                //userProps.Update();
            }
        }
        /// <summary>
        /// 恢复最大生命值 
        /// </summary>
        /// <param name="userID"></param>
        public static void RegainGeneralLife(string userID)
        {
            var generalList = new GameDataCacheSet<UserGeneral>().FindAll(userID, u => u.GeneralStatus == GeneralStatus.DuiWuZhong);
            generalList.ForEach(general =>
            {
                general.LifeNum = MathUtils.Addition(general.LifeNum, general.LifeMaxNum, general.LifeMaxNum);
            });
        }

        /// <summary>
        /// 命运水晶开启点亮人物
        /// </summary>
        public static void GetUserLightOpen(string userID)
        {
            var cacheSet = new GameDataCacheSet<UserLight>();
            var userLightArray = cacheSet.FindAll(userID);
            if (userLightArray.Count == 0)
            {
                AddUserLight(cacheSet, userID, 1001, 1);
                AddUserLight(cacheSet, userID, 1002, 2);
                AddUserLight(cacheSet, userID, 1003, 2);
                AddUserLight(cacheSet, userID, 1004, 2);
                AddUserLight(cacheSet, userID, 1005, 2);
            }
            else
            {
                foreach (UserLight light in userLightArray)
                {
                    UserLight uLight = new GameDataCacheSet<UserLight>().FindKey(userID, light.HuntingID);
                    if (uLight == null || uLight.IsLight == 0)
                    {
                        if (uLight != null && uLight.HuntingID == 1001)
                        {
                            AddUserLight(cacheSet, userID, light.HuntingID, 1);
                        }
                        else if (uLight != null && uLight.IsLight == 0)
                        {
                            uLight.IsLight = 2;
                        }
                        else
                        {
                            AddUserLight(cacheSet, userID, light.HuntingID, 2);
                        }
                    }
                }
            }
        }

        private static void AddUserLight(GameDataCacheSet<UserLight> cacheSet, string userID, int huntingID, int isLight)
        {
            var light = new UserLight()
            {
                UserID = userID,
                HuntingID = huntingID,
                IsLight = isLight
            };
            cacheSet.Add(light);
        }


        /// <summary>
        /// 佣兵升级
        /// </summary>
        /// <param name="general"></param>
        /// <param name="exprience"></param>
        public static void TriggerGeneral(UserGeneral general, int exprience)
        {
            if (general == null)
            {
                return;
            }
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(general.UserID);
            short generalMaxLv = user == null ? _currMaxLv : (user.UserLv * 3).ToShort();
            if (general.GeneralLv >= generalMaxLv)
            {
                return;
            }
            short nextLv = MathUtils.Addition(general.GeneralLv, 1.ToShort());
            var generalEscalateInfo = new ConfigCacheSet<GeneralEscalateInfo>().FindKey(nextLv, GeneralType.YongBing);
            int tempExpri = generalEscalateInfo.UpExperience - general.CurrExperience;
            if ((general.GeneralLv+1).ToShort() >= (user.UserLv * 3).ToShort() && exprience > tempExpri)
            {
                general.CurrExperience = MathUtils.Addition(general.CurrExperience, tempExpri);
            }
            else
            {

                general.CurrExperience = MathUtils.Addition(general.CurrExperience, exprience);
            }


            while (nextLv <= generalMaxLv)
            {
                GeneralEscalateInfo generalEscalate = new ConfigCacheSet<GeneralEscalateInfo>().FindKey(nextLv, GeneralType.YongBing);
                if (general.GeneralLv >= _currMaxLv)
                {
                    break;
                }
                else if (generalEscalate != null && general.CurrExperience >= generalEscalate.UpExperience)
                {
                    general.RefreshMaxLife();
                    general.SetEscalate(generalEscalate.UpExperience);
                    general.GeneralLv = MathUtils.Addition(general.GeneralLv, (short)1, short.MaxValue);
                    general.LifeNum = general.LifeMaxNum;
                    GeneralHelper.GeneralUpgradeproperty(general); //佣兵升级加属性
                    nextLv = MathUtils.Addition(general.GeneralLv, 1.ToShort());
                    if (general.GeneralLv >= _currMaxLv)
                    {
                        general.CurrExperience = 0;
                    }
                }
                else
                {
                    break;
                }
            }
            if (general.GeneralLv >= generalMaxLv)
            {
                general.CurrExperience = 0;
            }
        }

        /// <summary>
        /// 判断是否超过玩家等级*3
        /// </summary>
        /// <param name="general"></param>
        /// <param name="exprience"></param>
        /// <returns></returns>
        public static int GeneralLvIsUserLv(string userID, int generalID, int exprience, int userLv)
        {
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            int isLv = 0;
            if (general == null)
            {
                return 3;
            }
            short generalMaxLv = _currMaxLv;
            if (general.GeneralLv >= generalMaxLv)
            {
                isLv = 1;
            }
            short generalLv = general.GeneralLv;
            if (generalLv >= (userLv * 3))
            {
                isLv = 2;
            }
            return isLv;
        }
        /// <summary>
        /// 判断佣兵等级是否大于玩家等级
        /// </summary>
        /// <returns></returns>
        public static bool GeneralIsUpLv(string userID, int generalID, short userLv, int postion)
        {
            bool isUp = false;
            userLv = (userLv * 3).ToShort();
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            if (userGeneral == null || userGeneral.GeneralLv >= userLv || userGeneral.GeneralLv >= _currMaxLv)
            {
                return true;
            }
            short genLv = userGeneral.GeneralLv;
            int experience = 0;
            double addNum = FestivalHelper.SurplusPurchased(userID, FestivalType.ManorAddition);//活动加成
            List<UserLand> landArray = new GameDataCacheSet<UserLand>().FindAll(userID, u => u.GeneralID.Equals(generalID) && u.IsGain == 1);
            foreach (var land in landArray)
            {
                experience += GetLandExperience(land, userLv);
            }
            UserPlantQuality userPlantQuality = new GameDataCacheSet<UserPlantQuality>().FindKey(userID, generalID, PlantType.Experience);
            UserLand userLand = new GameDataCacheSet<UserLand>().FindKey(userID, postion);
            if (userPlantQuality != null && userLand != null)
            {
                experience += GetLandExperience(userLand, userLv);
            }

            experience = (experience * addNum).ToInt();
            experience = MathUtils.Addition(experience, userGeneral.CurrExperience);
            while (true)
            {
                GeneralEscalateInfo generalEscalate = new ConfigCacheSet<GeneralEscalateInfo>().FindKey(genLv);
                if (generalEscalate != null && experience >= generalEscalate.UpExperience)
                {
                    experience = MathUtils.Subtraction(experience, generalEscalate.UpExperience);
                    genLv = MathUtils.Addition(genLv, (short)1, short.MaxValue);
                }
                else
                {
                    break;
                }
            }
            if (genLv >= userLv)
            {
                isUp = true;
            }
            return isUp;
        }


        /// <summary>
        /// 种植获得经验
        /// </summary>
        /// <param name="land"></param>
        /// <param name="userLv"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        private static int GetLandExperience(UserLand land, short userLv)
        {
            int experience = 0;
            if (land != null)
            {
                PlantInfo plantInfo = new ConfigCacheSet<PlantInfo>().FindKey(userLv, (short)PlantType.Experience, land.PlantQuality);
                if (plantInfo != null)
                {
                    experience = plantInfo.GainNum;
                    if (land.IsRedLand == 1)
                    {
                        experience = MathUtils.Addition(experience, (int)(experience * 0.2), int.MaxValue);
                    }
                    if (land.IsBlackLand == 1)
                    {
                        experience = MathUtils.Addition(experience, (int)(experience * 0.4), int.MaxValue);
                    }
                }
            }
            return experience;
        }

        /// <summary>
        /// 玩家升级开启魔术、阵法
        /// </summary>
        public static void OpenMagic(string userID, int userLv)
        {
            var magicCacheSet = new GameDataCacheSet<UserMagic>();
            var embattleCacheSet = new GameDataCacheSet<UserEmbattle>();
            var magicArray = new ConfigCacheSet<MagicInfo>().FindAll(u => u.DemandLv <= userLv);
            foreach (MagicInfo magicInfo in magicArray)
            {
                UserMagic userMagic = magicCacheSet.FindKey(userID, magicInfo.MagicID);
                if (userMagic == null)
                {
                    UserMagic magic = new UserMagic()
                                         {
                                             UserID = userID,
                                             MagicID = magicInfo.MagicID,
                                             MagicLv = magicInfo.MagicLv,
                                             MagicType = magicInfo.MagicType,
                                             IsEnabled = false
                                         };
                    magicCacheSet.Add(magic);

                    if (magicInfo.MagicType == MagicType.MoFaZhen)
                    {
                        MagicLvInfo lvInfo = new ConfigCacheSet<MagicLvInfo>().FindKey(magicInfo.MagicID, magicInfo.MagicLv);
                        if (lvInfo != null)
                        {
                            string[] magicRang = lvInfo.GridRange.Split(',');
                            if (magicRang.Length > 0)
                            {
                                UserEmbattle embattle = new UserEmbattle()
                                                            {
                                                                UserID = userID,
                                                                MagicID = magicInfo.MagicID,
                                                                Position = Convert.ToInt16(magicRang[0]),
                                                                GeneralID = 0 // LanguageManager.GetLang().GameUserGeneralID
                                                            };
                                embattleCacheSet.Add(embattle);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 处理玩家变更数据
        /// </summary>
        /// <param name="property"></param>
        /// <param name="userID"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        public static void TriggerUserCallback(string property, string userID, object oldValue, object value)
        {
            int useNum = MathUtils.Subtraction(value.ToInt(), oldValue.ToInt(), 0);
            int consumeNum = MathUtils.Subtraction(oldValue.ToInt(), value.ToInt(), 0);
            if (property == "UseGold")
            {
                //晶石消费
                TaskHelper.TriggerDailyTask(userID, 4003);
                FestivalHelper.TriggerFestivalConsume(userID, useNum, FestivalType.SparConsumption);
                FestivalHelper.PayAccumulation(userID, useNum);
            }
            if (property == "EnergyNum" && consumeNum > 0)
            {
                FestivalHelper.TriggerFestivalConsume(userID, consumeNum, FestivalType.Energy);
            }
            if (property == "GameCoin" && consumeNum > 0)
            {
                FestivalHelper.TriggerFestivalConsume(userID, consumeNum, FestivalType.GameCoin);
            }
            UserDataChangeLog.SaveLog<GameUser>(property, userID, oldValue, value);
        }

        /// <summary>
        ///  每日刷新 --每日限制次数
        /// </summary>
        public static void ChechDailyRestrain(string userID)
        {
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (user != null)
            {
                if (user.UserExtend != null)
                {
                    if (user.UserExtend.RefreashDate.Date != DateTime.Now.Date)
                    {
                        DailyRefresh dailyRefresh = user.UserExtend.DailyInfo;
                        if (dailyRefresh == null)
                        {
                            dailyRefresh = new DailyRefresh();
                            dailyRefresh.RefreshDate = DateTime.Now;
                            dailyRefresh.MoreNum = 0;
                        }
                        else
                        {
                            dailyRefresh.RefreshDate = DateTime.Now;
                            dailyRefresh.MoreNum = 0;
                        }
                        user.UserExtend.UpdateNotify(obj =>
                        {
                            user.UserExtend.DailyInfo = dailyRefresh;
                            user.UserExtend.RefreashDate = DateTime.Now;
                            user.UserExtend.ItemList.Clear();
                            return true;
                        });
                        //user.Update();
                    }
                }
                else
                {
                    if (user.UserExtend == null) user.UserExtend = new GameUserExtend();
                    user.UserExtend.RefreashDate = DateTime.Now;
                    if (user.UserExtend.DailyInfo == null)
                    {
                        user.UserExtend.UpdateNotify(obj =>
                        {
                            user.UserExtend.DailyInfo = new DailyRefresh();
                            user.UserExtend.DailyInfo.RefreshDate = DateTime.Now;
                            user.UserExtend.DailyInfo.MoreNum = 0;
                            return true;
                        });
                    }
                }
            }
            UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (userRestrain != null && DateTime.Now.Date != userRestrain.RefreshDate.Date)
            {
                userRestrain.Funtion1 = 0;
                userRestrain.Funtion2 = 0;
                userRestrain.Funtion3 = 0;
                userRestrain.Funtion4 = 0;
                userRestrain.Funtion5 = 0;
                userRestrain.Funtion6 = 0;
                userRestrain.Funtion7 = 0;
                userRestrain.Funtion8 = 0;
                userRestrain.Funtion9 = 0;
                userRestrain.Funtion10 = 0;
                userRestrain.FunPlot.Clear();
                userRestrain.UserExtend = new DailyUserExtend();
                userRestrain.RefreshDate = DateTime.Now;
                //userRestrain.Update();
            }
        }

        /// <summary>
        /// 公会成员当日公会贡献、上香、封魔清空
        /// </summary>
        /// <param name="guilid"></param>
        /// <param name="userID"></param>
        public static void ChecheDailyContribution(string guilid, string userID)
        {
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(guilid, userID);
            if (member != null)
            {
                if (DateTime.Now.Date != member.RefreshDate.Date)
                {
                    member.DonateGold = 0;
                    member.DonateCoin = 0;
                    member.Contribution = 0;
                    member.RefreshDate = DateTime.Now;
                }
                if (DateTime.Now.Date != member.DevilDate.Date)
                {
                    member.IsDevil = 2;
                    member.DevilNum = 0;
                    member.SummonNum = 0;
                    member.DevilDate = DateTime.Now;
                }
                if (DateTime.Now.Date != member.IncenseDate.Date)
                {
                    member.IncenseNum = 0;
                    member.IncenseDate = DateTime.Now;
                }
                //member.Update();
            }
        }

        /// <summary>
        /// 公会添加经验，升级
        /// </summary>
        /// <param name="guildID"></param>
        public static void UserGuildUpLv(string guildID, int experience)
        {
            if (!string.IsNullOrEmpty(guildID))
            {
                UserGuild userGuild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
                if (userGuild != null)
                {
                    GuildLvInfo lvInfo = new ConfigCacheSet<GuildLvInfo>().FindKey(MathUtils.Addition(userGuild.GuildLv, (short)1));
                    userGuild.CurrExperience = MathUtils.Addition(userGuild.CurrExperience, experience, int.MaxValue);
                    if (lvInfo != null)
                    {
                        if (userGuild.CurrExperience >= lvInfo.UpExperience)
                        {
                            userGuild.CurrExperience = MathUtils.Subtraction(userGuild.CurrExperience, lvInfo.UpExperience, 0);
                            userGuild.GuildLv = MathUtils.Addition(userGuild.GuildLv, (short)1);
                        }
                    }
                    //userGuild.Update();
                }

            }
        }

        /// <summary>
        /// 公会贡献
        /// </summary>
        public static void Contribution(string userID, int experience)
        {

            List<GuildMember> memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.UserID == userID);
            if (memberArray.Count > 0)
            {
                GuildMember member = memberArray[0];
                UserGuildUpLv(member.GuildID, experience); //公会添加经验，升级
                if (DateTime.Now.Date == member.RefreshDate.Date)
                {
                    member.Contribution = MathUtils.Addition(member.Contribution, experience, int.MaxValue);
                }
                else
                {
                    member.Contribution = experience;
                }
                member.TotalContribution = MathUtils.Addition(member.TotalContribution, experience, int.MaxValue);
                //member.Update();
            }
        }

        /// <summary>
        /// 强化后装备卖出价格
        /// </summary>
        /// <returns></returns>
        public static int StrongEquPayPrice(string userId, string userItemID)
        {
            int equPrice = 0;
            int baseNum = 0;
            var package = UserItemPackage.Get(userId);
            var userItemList = package.ItemPackage.FindAll(m => !m.IsRemove && m.UserItemID.Equals(userItemID));
            foreach (var userItem in userItemList)
            {
                if (userItem != null)
                {
                    if (userItem.ItemStatus == ItemStatus.Sell || userItem.SoldDate > MathUtils.SqlMinDate)
                    {
                        package.RemoveItem(userItem);
                        continue;
                    }
                    else if (userItem.ItemStatus != ItemStatus.Sell && userItem.SoldDate > MathUtils.SqlMinDate)
                    {
                        userItem.SoldDate = MathUtils.SqlMinDate;
                        //package.Update();
                        continue;
                    }
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
                    if (itemInfo != null && itemInfo.ItemType == ItemType.ZhuangBei)
                    {
                        baseNum = MathUtils.Addition(baseNum, itemInfo.SalePrice, int.MaxValue);
                        for (int i = 2; i < userItem.ItemLv; i++)
                        {
                            baseNum = MathUtils.Addition(baseNum, UserItemHelper.GetStrongMoney(userItem.ItemID, i), int.MaxValue);
                        }
                    }
                }
            }
            equPrice = (baseNum / 4);
            return equPrice;
        }


        /// <summary>
        /// 修炼获得经验
        /// </summary>
        /// <param name="userID"></param>
        public static void XiuLianGianExperience(string userID)
        {
            GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(userID);
            int totalTime = 0;
            int experience = 0;
            int genlv = 0;
            if (gameUser.UserLv > _currMaxLv)
            {
                genlv = _currMaxLv;
            }
            else
            {
                genlv = gameUser.UserLv;
            }
            GeneralPracticeInfo generalpractice = new ConfigCacheSet<GeneralPracticeInfo>().FindKey(genlv);
            if (generalpractice != null)
            {
                if (VipHelper.GetVipOpenFun(gameUser.VipLv, ExpandType.XiuLianYanChangErShiSiXiaoShi))
                {
                    totalTime = ConfigEnvSet.GetInt("User.XiuLianDate");
                }
                else
                {
                    totalTime = (generalpractice.MaxHour * 60 * 60);
                }

                if (gameUser.UserStatus == UserStatus.XiuLian)
                {
                    List<UserQueue> userQueueArray = new GameDataCacheSet<UserQueue>().FindAll(userID, m => m.QueueType == QueueType.XiuLian);
                    if (userQueueArray.Count > 0 && userQueueArray[0].DoRefresh() <= 0)
                    {
                        List<UserMagic> userMagicArray = new GameDataCacheSet<UserMagic>().FindAll(userID, u => u.IsEnabled && u.MagicType == MagicType.MoFaZhen);
                        if (userMagicArray.Count > 0)
                        {
                            List<UserEmbattle> userEmbattleArray = new GameDataCacheSet<UserEmbattle>().FindAll(userID, m => m.MagicID == userMagicArray[0].MagicID);
                            int practiceTime = 0; //修炼时间
                            if (userQueueArray.Count > 0)
                            {
                                UserQueue queueInfo = userQueueArray[0];
                                TimeSpan ts = DateTime.Now - queueInfo.Timing;
                                DateTime endDate = queueInfo.Timing.AddSeconds(totalTime);
                                practiceTime = (int)ts.TotalSeconds;
                                if (practiceTime >= totalTime && queueInfo.Timing <= DateTime.Now)
                                {
                                    experience = ((totalTime / generalpractice.IntervalTime) * generalpractice.Exprience);
                                }
                                foreach (UserEmbattle embattle in userEmbattleArray)
                                {
                                    UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(gameUser.UserID, embattle.GeneralID);
                                    if (general != null)
                                    {
                                        if (general.GeneralID == LanguageManager.GetLang().GameUserGeneralID)
                                        {
                                            general.CurrExperience = MathUtils.Addition(general.CurrExperience, MathUtils.Subtraction(experience, queueInfo.StrengNum, 0), int.MaxValue);
                                        }
                                        else
                                        {
                                            general.CurrExperience = MathUtils.Addition(general.CurrExperience, experience, int.MaxValue);
                                        }
                                        general.Experience1 = MathUtils.Addition(general.Experience1, experience, int.MaxValue);
                                        //general.Update();

                                    }
                                }
                                if (gameUser.UserStatus != UserStatus.Normal)
                                {
                                    gameUser.UserStatus = UserStatus.Normal;
                                    //gameUser.Update();
                                }
                                UserLogHelper.AppenPracticeLog(userID, userQueueArray[0].Timing, endDate, practiceTime, experience);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 竞技场公告
        /// </summary>
        public static void SprostSystemChat(UserRank rank, UserRank toRank)
        {
            if (rank == null || toRank == null)
            {
                return;
            }
            int topId = rank.RankId;
            int totopID = toRank.RankId;
            int winNum = 0;
            int winMaxNum = 0;
            string content = string.Empty;
            string winMaxUserID = string.Empty;
            var chatService = new TjxChatService();
            if (topId == 1 && topId > totopID)
            {
                content = string.Format(LanguageManager.GetLang().St5107_JingJiChangOneRank, rank.NickName, toRank.NickName);
                chatService.SystemSend(ChatType.World, content);
            }
            GameUser gameUser = UserCacheGlobal.LoadOffline(rank.UserID);
            if (gameUser != null)
            {
                IList<SportsCombat> userSportsCombats = gameUser.GetSportsCombat();

                if (userSportsCombats.Count > 0)
                {
                    if (totopID == 1 && userSportsCombats[userSportsCombats.Count - 1].IsWin)
                    {
                        content = string.Format(LanguageManager.GetLang().St5107_JingJiChangOneRank, toRank.NickName,
                                                rank.NickName);
                        chatService.SystemSend(ChatType.World, content);
                    }
                    winNum = userSportsCombats[userSportsCombats.Count - 1].WinNum;
                }
                //连胜第一名
                winMaxUserID = ServerEnvSet.Get(ServerEnvKey.SportFirstUser, "");
                winMaxNum = ServerEnvSet.Get(ServerEnvKey.SportFirstWinNum, 0).ToInt();

                Ranking<UserRank> ranking = RankingFactory.Get<UserRank>(CombatRanking.RankingKey);
                UserRank rankInfo = ranking.Find(s => s.UserID == winMaxUserID);
                //打破全服最高连胜的发通知
                if (winNum > winMaxNum)
                {
                    ServerEnvSet.Set(ServerEnvKey.SportFirstUser, rank.UserID);
                    ServerEnvSet.Set(ServerEnvKey.SportFirstWinNum, winNum);
                    if (rankInfo != null && rank.UserID != winMaxUserID && !string.IsNullOrEmpty(winMaxUserID))
                    {
                        content = string.Format(LanguageManager.GetLang().St5107_ZuiGaoLianSha, rank.NickName,
                                                rankInfo.NickName);
                        chatService.SystemSend(ChatType.World, content);
                    }
                    else if (winMaxNum > 5)
                    {
                        content = string.Format(LanguageManager.GetLang().St5107_JingJiChangWinNum, rank.NickName,
                                                winNum);
                        chatService.SystemSend(ChatType.World, content);
                    }
                }
                //int upRankID = MathUtils.Subtraction(totopID, topId, 0);
                //if (upRankID > 20)
                //{
                //    UserRank urankInfo = ranking.Find(s => s.UserID == rank.UserID);
                //    if (urankInfo != null && !string.IsNullOrEmpty(urankInfo.UserID))
                //    {
                //        content = string.Format(LanguageManager.GetLang().St5107_JingJiChangMoreNum, rank.NickName,
                //                                upRankID);
                //        chatService.SystemSend(ChatType.World, content);
                //    }
                //}
            }
        }

        /// <summary>
        /// 是否提示补血
        /// </summary>
        public static bool IsPromptBlood(string userID)
        {
            bool result = false;
            List<UserProps> propsesArray = new GameDataCacheSet<UserProps>().FindAll(userID, u => u.PropType == 1);
            List<UserGeneral> generalsArray = new GameDataCacheSet<UserGeneral>().FindAll(userID, u => u.GeneralStatus == GeneralStatus.DuiWuZhong);
            if (propsesArray.Count > 0)
            {
                if (propsesArray[0].SurplusNum == 0)
                {
                    foreach (UserGeneral userGeneral in generalsArray)
                    {
                        if (userGeneral.LifeNum < userGeneral.LifeMaxNum)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (UserGeneral userGeneral in generalsArray)
                {
                    if (userGeneral.LifeNum < userGeneral.LifeMaxNum)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 佣兵自动使用绷带补血
        /// </summary>
        public static void UserGeneralPromptBlood(GameUser user)
        {
            int subLifeNum = 0;
            if (IsPromptBlood(user.UserID))
            {
                var userItemArray = UserItemHelper.GetItems(user.UserID).FindAll(u => u.ItemType == ItemType.BengDai && u.ItemStatus != ItemStatus.Sell);
                if (userItemArray.Count > 0)
                {
                    int itemID = userItemArray[0].ItemID;
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
                    var propsCacheSet = new GameDataCacheSet<UserProps>();
                    List<UserProps> propsArray = propsCacheSet.FindAll(user.UserID, u => u.PropType == 1);
                    if (propsArray.Count > 0 && propsArray[0].SurplusNum == 0)
                    {
                        foreach (UserProps userPropse in propsArray)
                        {
                            if (userPropse.SurplusNum == 0)
                            {
                                propsCacheSet.Delete(userPropse);
                            }
                        }
                    }
                    //给佣兵补血
                    List<UserGeneral> userGeneralArray = new GameDataCacheSet<UserGeneral>().FindAll(user.UserID, u => u.GeneralStatus == GeneralStatus.DuiWuZhong);
                    int effectNum = itemInfo.EffectNum;
                    foreach (var userGeneral in userGeneralArray)
                    {
                        int maxLifeNum = GetMaxLife(user.UserID, userGeneral.GeneralID);
                        if (userGeneral.LifeNum < maxLifeNum)
                        {
                            subLifeNum = MathUtils.Subtraction(maxLifeNum, userGeneral.LifeNum, 0);
                            userGeneral.LifeNum = MathUtils.Addition(userGeneral.LifeNum, effectNum, maxLifeNum);
                            //userGeneral.Update();
                            effectNum = MathUtils.Subtraction(effectNum, subLifeNum, 0);
                        }
                    }
                    UserProps props = new UserProps(user.UserID, itemInfo.ItemID)
                    {
                        SurplusNum = effectNum
                    };
                    propsCacheSet.Add(props);
                    user.IsUseupItem = false;
                    UserItemHelper.UseUserItem(user.UserID, itemInfo.ItemID, 1);
                }
            }
        }

        /// <summary>
        /// 竞技场连胜奖励
        /// </summary>
        public static void WinsReward(GameUser user, int winsNum)
        {
            if (!IsGainReward(user.UserID, winsNum))
            {
                UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
                if (dailyRestrain != null)
                {
                    ArenaWinsNum(user, winsNum);
                    if (!string.IsNullOrEmpty(dailyRestrain.Funtion12))
                    {
                        dailyRestrain.Funtion12 = dailyRestrain.Funtion12 + winsNum + ",";
                    }
                    else
                    {
                        dailyRestrain.Funtion12 = winsNum + ",";
                    }
                    //dailyRestrain.Update();
                }
            }
        }

        /// <summary>
        /// 竞技场连胜奖励是否已领取
        /// </summary>
        public static bool IsGainReward(string userID, int winNum)
        {
            bool isGain = false;
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && !string.IsNullOrEmpty(dailyRestrain.Funtion12))
            {
                string[] winsType = dailyRestrain.Funtion12.Split(',');
                if (winsType.Length > 0)
                {
                    foreach (string win in winsType)
                    {
                        if (!string.IsNullOrEmpty(win) && win == winNum.ToString())
                        {
                            isGain = true;
                            break;
                        }
                    }
                }
            }
            return isGain;
        }


        /// <summary>
        /// 竞技场连胜奖励数值
        /// </summary>
        /// <param name="user"></param>
        /// <param name="winsNum"></param>
        public static void ArenaWinsNum(GameUser user, int winsNum)
        {
            int gameCoin = 0;
            int giftGold = 0;
            if (winsNum == 3)
            {
                gameCoin = 100000;
                giftGold = 20;
            }
            else if (winsNum == 5)
            {
                gameCoin = 200000;
                giftGold = 40;
            }
            else if (winsNum == 7)
            {
                gameCoin = 300000;
                giftGold = 60;
            }
            else if (winsNum == 9)
            {
                gameCoin = 400000;
                giftGold = 80;
            }
            else if (winsNum == 12)
            {
                gameCoin = 500000;
                giftGold = 100;
            }
            else if (winsNum == 20)
            {
                gameCoin = 2000000;
                giftGold = 200;
            }
            user.GameCoin = MathUtils.Addition(user.GameCoin, gameCoin, int.MaxValue);
            user.GiftGold = MathUtils.Addition(user.GiftGold, giftGold, int.MaxValue);
            //user.Update();
            string content = string.Format(LanguageManager.GetLang().St5107_ArenaWinsNum, user.NickName, winsNum, gameCoin, giftGold);
            new TjxChatService().SystemSend(ChatType.World, content);
        }

        /// <summary>
        /// 上阵佣兵加经验
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="experience"></param>
        public static void UserGeneralExp(string userID, int experience)
        {
            List<UserMagic> userMagicArray = new GameDataCacheSet<UserMagic>().FindAll(userID, u => u.IsEnabled && u.MagicType == MagicType.MoFaZhen);
            if (userMagicArray.Count > 0)
            {
                UserMagic magic = userMagicArray[0];
                List<UserEmbattle> userEmbattleArray = new GameDataCacheSet<UserEmbattle>().FindAll(userID, m => m.MagicID == magic.MagicID);

                foreach (UserEmbattle embattle in userEmbattleArray)
                {
                    GeneralHelper.UserGeneralExp(userID, embattle.GeneralID, experience);
                    //UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(userID, embattle.GeneralID);
                    //if (general != null && general.GeneralLv < _currMaxLv)
                    //{
                    //    general.CurrExperience = MathUtils.Addition(general.CurrExperience, experience, int.MaxValue);
                    //    //general.Update();
                    //}
                    //if (general != null && general.GeneralLv >= _currMaxLv)
                    //{
                    //    general.CurrExperience = 0;
                    //}
                }
            }
        }

        /// <summary>
        /// 根据周几取出时间
        /// </summary>
        /// <param name="dateType"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(BossDateType dateType)
        {
            DateTime currDt = DateTime.Now.Date;
            int currWeek = currDt.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)currDt.DayOfWeek;
            DateTime bossDate = currDt.AddDays((int)DayOfWeek.Monday - currWeek);
            //if (dateType == BossDateType.Monday)
            //{
            //    bossDate = bossDate;
            //}
            //else
            if (dateType == BossDateType.Tuesday)
            {
                bossDate = bossDate.AddDays(1);
            }
            else if (dateType == BossDateType.Wednesday)
            {
                bossDate = bossDate.AddDays(2);
            }
            else if (dateType == BossDateType.Thursday)
            {
                bossDate = bossDate.AddDays(3);
            }
            else if (dateType == BossDateType.Friday)
            {
                bossDate = bossDate.AddDays(4);
            }
            else if (dateType == BossDateType.Saturday)
            {
                bossDate = bossDate.AddDays(5);
            }
            else if (dateType == BossDateType.SundayAfternoon || dateType == BossDateType.Sunday)
            {
                bossDate = bossDate.AddDays(6);
            }
            return bossDate;
        }

        /// <summary>
        /// 公会boss挑战时间
        /// </summary>
        /// <param name="bossInfo"></param>
        /// <returns></returns>
        public static DateTime GuildBossDate(GuildBossInfo bossInfo)
        {
            BossDateType bossDateType = (BossDateType)Enum.Parse(typeof(BossDateType), bossInfo.EnableWeek.ToString());
            DateTime weekDateTime = GetDateTime(bossDateType).Date;
            DateTime mdate = bossInfo.EnablePeriod.ToDateTime();
            return weekDateTime.AddHours(mdate.Hour).AddMinutes(mdate.Minute);
        }

        /// <summary>
        /// 公会boss挑战时间
        /// </summary>
        /// <param name="bossInfo"></param>
        /// <returns></returns>
        public static DateTime GuildEnableWeek(BossDateType bossDateType)
        {
            string bossTime = string.Empty;
            if (bossDateType == BossDateType.SundayAfternoon)
            {
                bossTime = "16:00:00";
            }
            else if (bossDateType == BossDateType.Sunday)
            {
                bossTime = "22:00:00";
            }
            else
            {
                bossTime = "21:00:00";
            }
            // BossDateType bossDateType = (BossDateType)Enum.Parse(typeof(BossDateType), bossInfo.EnableWeek.ToString());
            string weekDateTime = GetDateTime(bossDateType).ToString("d");
            string hourDate = DateTime.Parse(bossTime).ToString("T");
            string currDate = weekDateTime + " " + hourDate;
            DateTime priod = DateTime.Parse(currDate);
            return priod;
        }

        /// <summary>
        /// 玩家祝福次数,使用后剩余次数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int GainBlessing(GameUser user, int subGain)
        {
            int gainNum = 0;
            int subNum = 0;
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(user.MercenariesID, user.UserID);
            if (member != null)
            {
                subNum = MathUtils.Subtraction(subGain, member.GainBlessing, 0);
                member.GainBlessing = MathUtils.Subtraction(member.GainBlessing, subGain, 0);
                gainNum = MathUtils.Addition(gainNum, member.GainBlessing, int.MaxValue);
                //member.Update();
            }
            if (user.UserExtend != null)
            {
                user.UserExtend.GainBlessing = MathUtils.Subtraction(user.UserExtend.GainBlessing, subNum, 0);
                gainNum = MathUtils.Addition(gainNum, user.UserExtend.GainBlessing, int.MaxValue);
                //user.Update();
            }
            return gainNum;
        }

        /// <summary>
        /// 背包是否已满
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsBeiBaoFull(GameUser user)
        {
            //var userItemsArry = UserItemHelper.GetItems(user.UserID).FindAll(m => m.ItemStatus == ItemStatus.BeiBao);
            //if (userItemsArry.Count >= user.GridNum) return true;
            //return false;
            return IsBeiBaoFull(user, 0);
        }

        /// <summary>
        /// 背包是否已满
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsBeiBaoFull(GameUser user, int num)
        {
            //原因：当num大于0且userItemsArry.Count + num= user.GridNum是提示背包已满
            string str;
            return UserPackHelper.PackIsFull(user, BackpackType.BeiBao, num, out str);
        }

        /// <summary>
        /// 命运背包是否已满
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsCrystalBeiBaoFull(GameUser user)
        {
            //var crystalsArray = new GameDataCacheSet<UserCrystal>().FindAll(UserCrystal.Index_UserID, u => u.IsSale == 2 && u.GeneralID == 0, user.UserID);
            //if (crystalsArray.Length >= user.CrystalNum) return true;
            ////var userItemsArry = UserItemHelper.GetItems(user.UserID).FindAll(m => m.ItemStatus == ItemStatus.BeiBao);
            ////if (userItemsArry.Count >= user.GridNum) return true;
            //return false;
            return IsCrystalBeiBaoFull(user, 0);
        }

        public static bool IsCrystalBeiBaoFull(GameUser user, int num)
        {
            var packageCrystal = UserCrystalPackage.Get(user.UserID);
            UserCrystalInfo[] crystalsArray =
                packageCrystal.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(0)).ToArray();
            if (crystalsArray.Length + num >= user.CrystalNum) return true;
            return false;
        }

        public static void CrystalAppend(string userID, int crystalID, short isSale)
        {
            var packageCrystal = UserCrystalPackage.Get(userID);
            UserCrystalInfo userCrystal = new UserCrystalInfo()
            {
                UserCrystalID = Guid.NewGuid().ToString(),
                CrystalID = crystalID,
                CrystalLv = 1,
                CurrExprience = 0,
                GeneralID = 0,
                IsSale = 2,
                CreateDate = DateTime.Now,
            };
            packageCrystal.SaveCrystal(userCrystal);
        }

        internal static void ProcessPetPrize(PetRunPool petRunPool)
        {
            if (petRunPool == null) return;
            if (petRunPool.PetID > 0)
            {
                var user = UserCacheGlobal.LoadOffline(petRunPool.UserID);
                if (user != null)
                {

                    if (user.UserExtend == null) user.UserExtend = new GameUserExtend();
                    //问题：在赛跑时有重刷点亮宠物后，等赛跑完服务端与客户端记录宠物ID不一致，原因是赛跑完有将宠物ID清除
                    //user.UserExtend.UpdateChildren(user.UserExtend, obj => obj.LightPetID = 0);
                    user.GameCoin = MathUtils.Addition(user.GameCoin, petRunPool.GameCoin, int.MaxValue);
                    user.ObtainNum = MathUtils.Addition(user.ObtainNum, petRunPool.ObtainNum, int.MaxValue);
                    //user.Update();
                    var chatService = new TjxChatService();
                    chatService.SystemSendWhisper(user, string.Format(LanguageManager.GetLang().Chat_PetRunSucess,
                   (new ConfigCacheSet<PetInfo>().FindKey(petRunPool.PetID) ?? new PetInfo()).PetName, petRunPool.GameCoin, petRunPool.ObtainNum));
                }
                petRunPool.PetID = 0;
                //petRunPool.Update();
            }
        }

        /// <summary>
        /// 获取玩家副本进度
        /// </summary>
        /// <param name="gameUser"></param>
        /// <returns></returns>
        public static PlotInfo GetUserPlotInfo(GameUser gameUser)
        {
            PlotInfo plotInfo = new PlotInfo();
            if (gameUser != null)
            {
                var taskInfo = new ConfigCacheSet<StoryTaskInfo>().FindKey(gameUser.TaskProgress) ?? new StoryTaskInfo();
                plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(taskInfo.PlotID);
            }
            return plotInfo;
        }

        /// <summary>
        /// 增加灵件
        /// </summary>
        public static bool AddSparePart(GameUser user, UserSparePart sparePart)
        {
            int currNum = user.SparePartList.FindAll(m => string.IsNullOrEmpty(m.UserItemID)).Count;
            if (currNum < user.UserExtend.SparePartGridNum)
            {
                user.AppendSparePart(sparePart);
                UserLogHelper.AppendSparePartLog(user.UserID, sparePart, 1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 灵件背包是否已满 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsSpareGridNum(GameUser user, int num)
        {
            //原因：当num大于0且currNum = user.UserExtend.SparePartGridNum是提示背包已满
            int currNum = user.SparePartList.FindAll(m => string.IsNullOrEmpty(m.UserItemID)).Count;
            if (num > 0)
            {
                currNum = MathUtils.Addition(currNum, num);
                if (currNum <= user.UserExtend.SparePartGridNum)
                {
                    return false;
                }
            }
            if (currNum < user.UserExtend.SparePartGridNum)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 装备封灵属性
        /// </summary>
        /// <param name="userItemID"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        private static SparePartProperty GetSparePartProperty(string userId, string userItemID, AbilityType abilityType)
        {
            double equSumNum = 0;
            var package = UserItemPackage.Get(userId);
            if (package == null)
            {
                return new SparePartProperty();
            }
            var userItemList = package.ItemPackage.FindAll(m => !m.IsRemove && m.UserItemID.Equals(userItemID));
            foreach (var userItem in userItemList)
            {
                if (userItem != null && (userItem.ItemStatus == ItemStatus.Sell || userItem.SoldDate > MathUtils.SqlMinDate))
                {
                    continue;
                }
                else if (userItem != null && userItem.ItemStatus != ItemStatus.Sell && userItem.SoldDate > MathUtils.SqlMinDate)
                {
                    userItem.SoldDate = MathUtils.SqlMinDate;
                    //package.Update();
                    continue;
                }

                //灵件配置
                var user = new GameDataCacheSet<GameUser>().FindKey(userId);
                if (user != null)
                {
                    var sparepartList = user.SparePartList.FindAll(m => m.UserItemID.Equals(userItemID));
                    foreach (var sparepart in sparepartList)
                    {
                        foreach (var property in sparepart.Propertys)
                        {
                            if (property.AbilityType == abilityType)
                            {
                                equSumNum = MathUtils.Addition(equSumNum, property.Num.ToDouble(), double.MaxValue);
                            }
                        }
                    }
                }
                return new SparePartProperty() { AbilityType = abilityType, Num = equSumNum };
            }
            return new SparePartProperty();
        }

        /// <summary>
        /// 灵件属性
        /// </summary>
        /// <param name="userItemID"></param>
        /// <returns></returns>
        public static SparePartProperty[] SparePartPropertyList(string userId, string userItemID)
        {
            List<SparePartProperty> propertyList = new List<SparePartProperty>();
            //物理攻击
            SparePartProperty property = null;
            property = GetSparePartProperty(userId, userItemID, AbilityType.WuLiGongJi);
            if (property != null && (int)property.Num != 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.WuLiGongJi));
            }
            //物理防御
            property = GetSparePartProperty(userId, userItemID, AbilityType.WuLiFangYu);
            if (property != null && (int)property.Num != 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.WuLiFangYu));
            }
            //魂技攻击
            property = GetSparePartProperty(userId, userItemID, AbilityType.HunJiGongJi);
            if (property != null && (int)property.Num != 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.HunJiGongJi));
            }
            //魂技防御 
            property = GetSparePartProperty(userId, userItemID, AbilityType.HunJiFangYu);
            if (property != null && (int)property.Num != 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.HunJiFangYu));
            }
            //魔法攻击
            property = GetSparePartProperty(userId, userItemID, AbilityType.MoFaGongJi);
            if (property != null && (int)property.Num != 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.MoFaGongJi));
            }
            //魔法防御
            property = GetSparePartProperty(userId, userItemID, AbilityType.MoFaFangYu);
            if (property != null && (int)property.Num != 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.MoFaFangYu));
            }
            //暴击
            property = GetSparePartProperty(userId, userItemID, AbilityType.BaoJi);
            if (property != null && property.Num < 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.BaoJi));
            }
            //命中
            property = GetSparePartProperty(userId, userItemID, AbilityType.MingZhong);
            if (property != null && property.Num < 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.MingZhong));
            }
            //破击 
            property = GetSparePartProperty(userId, userItemID, AbilityType.PoJi);
            if (property != null && property.Num < 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.PoJi));
            }
            //韧性
            property = GetSparePartProperty(userId, userItemID, AbilityType.RenXing);
            if (property != null && property.Num < 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.RenXing));
            }
            //闪避
            property = GetSparePartProperty(userId, userItemID, AbilityType.ShanBi);
            if (property != null && property.Num < 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.ShanBi));
            }
            //格挡
            property = GetSparePartProperty(userId, userItemID, AbilityType.GeDang);
            if (property != null && property.Num < 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.GeDang));
            }
            //必杀
            property = GetSparePartProperty(userId, userItemID, AbilityType.BiSha);
            if (property != null && property.Num < 0)
            {
                propertyList.Add(GetSparePartProperty(userId, userItemID, AbilityType.BiSha));
            }

            return propertyList.ToArray();
        }

        /// <summary>
        /// 属性显示
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string PropertyAbility(double num)
        {
            string propertyNum = string.Empty;
            if (num > 1)
            {
                propertyNum = num.ToString().Replace(".0000", "");
            }
            else
            {
                propertyNum = (num * 100).ToString() + "%";
            }
            return propertyNum;
        }


        public static bool IsKill(GameUser user, int plotID)
        {
            bool isKill = false;
            PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
            if (plotInfo != null && plotInfo.PlotType == PlotType.Kalpa)
            {
                UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
                if (userRestrain != null && userRestrain.UserExtend != null)
                {
                    if (userRestrain.UserExtend.KalpaDate.Date == DateTime.Now.Date ||
                        (IsLastLayer(plotInfo) && plotInfo.PlotSeqNo < user.UserExtend.HurdleNum) ||
                        (!IsLastLayer(plotInfo) && plotInfo.PlotSeqNo == 24))
                    {
                        FunPlot funPlot = userRestrain.UserExtend.KalpaPlot.FindLast(m => m.PlotID == plotID);
                        if (funPlot != null)
                        {
                            isKill = true;
                        }
                    }
                    else if (plotInfo.PlotSeqNo < user.UserExtend.HurdleNum || (IsLastLayer(plotInfo) && plotInfo.PlotSeqNo == 24))
                    {
                        isKill = true;
                    }
                }
                else
                {
                    if (plotInfo.PlotSeqNo < user.UserExtend.HurdleNum)
                    {
                        isKill = true;
                    }
                }
            }
            return isKill;
        }

        /// <summary>
        /// 是否是最后一层
        /// </summary>
        /// <param name="plotInfo"></param>
        /// <returns></returns>
        public static bool IsLastLayer(PlotInfo plotInfo)
        {
            return new ConfigCacheSet<PlotInfo>().FindKey(plotInfo == null ? 0 : plotInfo.AftPlotID) == null;
        }

        /// <summary>
        /// 公会技能开启
        /// </summary>
        /// <param name="guildID"></param>
        public static void UserGuildAbilityList(string guildID)
        {
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guild == null)
            {
                return;
            }
            var guildAbilityInfoArray = new ConfigCacheSet<GuildAbilityInfo>().FindAll();
            if (guild.AbilityInfo.Count > 0 && guild.AbilityInfo.Count >= guildAbilityInfoArray.Count)
            {
                return;
            }
            foreach (GuildAbilityInfo abilityInfo in guildAbilityInfoArray)
            {
                GuildAbility ability = guild.AbilityInfo.Find(m => m.ID == abilityInfo.ID);
                if (ability == null && IsAbilityOpen(guild.AbilityInfo, abilityInfo))
                {
                    guild.AbilityInfo.Add(new GuildAbility() { ID = abilityInfo.ID, Lv = 0 });
                    TraceLog.ReleaseWriteFatal("公会技能重新开启;公会ID{0}", guildID);
                }
            }
        }

        /// <summary>
        /// 公会技能是否开启
        /// </summary>
        /// <param name="abilityList"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsAbilityOpen(CacheList<GuildAbility> abilityList, GuildAbilityInfo info)
        {
            GuildAbility ability = abilityList.Find(m => m.ID == info.ID);
            if (ability != null)
            {
                return false;
            }
            //没有前提条件
            if (info.PreAbility.Count == 0 && info.PreAbilityLv.Length == 0)
            {
                return true;
            }
            bool result = true;
            for (int i = 0; i < info.PreAbility.Count; i++)
            {
                GuildAbility preAbi = abilityList.Find(m => m.ID.ToString() == info.PreAbility[i]);
                if (preAbi == null || preAbi.Lv < Convert.ToInt32(info.PreAbilityLv[i]))
                {
                    return false;
                }
            }
            return result;
        }

        /// <summary>
        /// 公会技能捐献最大金币上限
        /// </summary>
        /// <param name="userLv"></param>
        /// <returns></returns>
        public static int MaxDonateGameCoin(int userLv)
        {
            int maxGameCoin = 0;
            string[] tempList = ConfigEnvSet.GetString("UserGuild.MaxDonateGameCoin").ToNotNullString().Split(',');
            foreach (var temp in tempList)
            {
                if (temp == null) continue;
                string[] lvList = temp.Split('=');
                if (lvList.Length == 2)
                {
                    int lv = lvList[0].ToInt();
                    if (userLv >= lv) maxGameCoin = lvList[1].ToInt();
                }
            }
            return maxGameCoin;
        }


        /// <summary>
        /// 装备上是否有灵件
        /// </summary>
        /// <returns></returns>
        public static bool IsItemEquSpare(GameUser user, string _userEquID)
        {
            if (user != null && user.SparePartList != null)
            {
                var tempPartList = user.SparePartList.FindAll(m => m.UserItemID.Equals(_userEquID));
                if (tempPartList.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 功能是否开启
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="funEnum"></param>
        /// <returns></returns>
        public static bool IsOpenFunction(string userID, FunctionEnum funEnum)
        {
            var cacheSet = new GameDataCacheSet<UserFunction>();
            UserFunction function = cacheSet.FindKey(userID, funEnum);
            if (function != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 当前玩家所在的层、关的副本
        /// </summary>
        /// <returns></returns>
        public static PlotInfo CheckUserPlotKalpa(GameUser user)
        {
            PlotInfo plotInfo = null;
            List<UserPlotInfo> userPlotArray = UserPlotHelper.UserPlotFindAll(user.UserID).FindAll(s => s.PlotType == PlotType.Kalpa);
            if (userPlotArray.Count == 0)
            {
                PlotHelper.EnablePlot(user.UserID, 8000);
                userPlotArray = UserPlotHelper.UserPlotFindAll(user.UserID).FindAll(s => s.PlotType == PlotType.Kalpa);
            }
            if (userPlotArray.Count > 0)
            {
                userPlotArray.QuickSort((x, y) =>
                {
                    int result = 0;
                    if (x == null && y == null) return 0;
                    if (x != null && y == null) return 1;
                    if (x == null) return -1;
                    result = new ConfigCacheSet<PlotInfo>().FindKey(y.PlotID).LayerNum.CompareTo(
                        new ConfigCacheSet<PlotInfo>().FindKey(x.PlotID).LayerNum);
                    if (result == 0)
                    {
                        result = new ConfigCacheSet<PlotInfo>().FindKey(y.PlotID).PlotSeqNo.CompareTo(
                            new ConfigCacheSet<PlotInfo>().FindKey(x.PlotID).PlotSeqNo);
                    }
                    return result;
                });
                plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(userPlotArray[0].PlotID);
            }
            return plotInfo;
        }

        /// <summary>
        /// 当前玩家道具列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<BlessingInfo> BlessingInfoList(GameUser user)
        {
            List<BlessingInfo> _blessingList = new List<BlessingInfo>();
            //公会上香加金币
            int gameCoinNum = GainBlessing(user, 0);
            if (gameCoinNum > 0)
            {
                _blessingList.Add(new BlessingInfo()
                                      {
                                          BlessingType = (short)BlessingType.GameCoin,
                                          BlessingNum = gameCoinNum,
                                          PropDate = 0,
                                          PropDesc = string.Format(LanguageManager.GetLang().St1008_GameCoinSurplusNum, gameCoinNum),
                                      });
            }
            List<UserProps> userPropsList = new GameDataCacheSet<UserProps>().FindAll(user.UserID);
            if (userPropsList.Count > 0)
            {
                foreach (UserProps props in userPropsList)
                {
                    BlessingInfo blessingInfo = new BlessingInfo();
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(props.ItemID);
                    if (itemInfo == null)
                    {
                        break;
                    }

                    if (props.PropType == 1 && props.SurplusNum > 0)
                    {
                        if (IsBlessingInfo(_blessingList, (short)BlessingType.BloodBag))
                        {
                            continue;
                        }
                        blessingInfo.BlessingType = (short)BlessingType.BloodBag;
                        blessingInfo.BlessingNum = props.SurplusNum;
                        blessingInfo.PropDate = 0;
                        blessingInfo.PropDesc = string.Format(LanguageManager.GetLang().St1008_BloodBagSurplusNum,
                                                              props.SurplusNum);
                    }
                    else if (props.PropType == 3 && props.SurplusNum > 0 && props.ItemID == 7003)
                    {
                        if (IsBlessingInfo(_blessingList, (short)BlessingType.DoubleItem))
                        {
                            continue;
                        }
                        blessingInfo.BlessingType = (short)BlessingType.DoubleItem;
                        blessingInfo.BlessingNum = props.SurplusNum;
                        blessingInfo.PropDate = 0;
                        blessingInfo.PropDesc = string.Format(LanguageManager.GetLang().St1008_DoubleItemSurplusNum,
                                                              props.SurplusNum);
                    } //原因：战力图标不显示
                    else if ((props.PropType == 9 && props.DoRefresh() > 0) || (props.ItemID == 5200 && props.SurplusNum > 0))
                    {
                        if (IsBlessingInfo(_blessingList, (short)BlessingType.CombatNum))
                        {
                            continue;
                        }
                        blessingInfo.BlessingType = (short)BlessingType.CombatNum;
                        blessingInfo.BlessingNum = 0;
                        if (props.ItemID == 5200)
                        {
                            blessingInfo.BlessingNum = props.DoRefresh();
                        }
                        blessingInfo.PropDate = props.DoRefresh();
                        blessingInfo.PropDesc = string.Format(LanguageManager.GetLang().St1008_CombatNumDate, props.DoRefresh());
                    }
                    else if (props.PropType == 3 && props.DoRefresh() > 0)
                    {
                        continue;
                        //blessingInfo.BlessingType = (short)BlessingType.Transfiguration;
                        //blessingInfo.BlessingNum = 0;
                        //blessingInfo.PropDate = props.DoRefresh();
                        //blessingInfo.PropDesc = string.Format(LanguageManager.GetLang().St1008_TransfigurationDate, props.DoRefresh());
                    }
                    _blessingList.Add(blessingInfo);
                }
            }
            //充值返还额度
            UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(user.UserID);
            if (userDial != null && userDial.ReturnRatio > 0)
            {
                string returnRatio = DialHelper.GetTransformData(userDial.ReturnRatio);
                _blessingList.Add(new BlessingInfo()
                {
                    BlessingType = (short)BlessingType.ReturnRatio,
                    BlessingNum = gameCoinNum,
                    PropDate = 0,
                    PropDesc = string.Format(LanguageManager.GetLang().St12004_RechargeReturn, returnRatio),
                });
            }
            return _blessingList;
        }

        /// <summary>
        /// 是否有图标
        /// </summary>
        /// <param name="_blessingList"></param>
        /// <param name="blessingType"></param>
        /// <returns></returns>
        public static bool IsBlessingInfo(List<BlessingInfo> _blessingList, short blessingType)
        {
            BlessingInfo info = _blessingList.Find(m => m.BlessingType == blessingType);
            if (info != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否空阵
        /// </summary>
        /// <returns></returns>
        public static bool IsUserEmbattle(string userID, int magicID)
        {
            var embattlesArray = new GameDataCacheSet<UserEmbattle>().FindAll(userID, m => m.GeneralID != 0 && m.MagicID == magicID);
            if (embattlesArray.Count > 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 传承操作类型
        /// </summary>
        public static List<OpsInfo> HeritageOpsTypeList()
        {
            int vip4UserGold = ConfigEnvSet.GetInt("UserGeneral.vip4UserGold");
            int vip7UserGold = ConfigEnvSet.GetInt("UserGeneral.vip7UserGold");
            List<OpsInfo> opsInfoList = new List<OpsInfo>();
            OpsInfo info = new OpsInfo();
            info.Type = 1;
            info.VipLv = 0;
            info.UseGold = 0;
            info.Num = 0.5;
            opsInfoList.Add(info);

            info = new OpsInfo();
            info.Type = 2;
            info.VipLv = 4;
            info.Num = 0.7;
            info.UseGold = vip4UserGold;
            opsInfoList.Add(info);

            info = new OpsInfo();
            info.Type = 3;
            info.VipLv = 7;
            info.Num = 0.9;
            info.UseGold = vip7UserGold;
            opsInfoList.Add(info);
            return opsInfoList;
        }

        /// <summary>
        /// 传承人或被传承人、传承类型选择
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalHeritagesList"></param>
        /// <param name="type"></param>
        public static void HeritageGeneral(string userID, List<GeneralHeritage> generalHeritagesList, int type)
        {
            GeneralHeritage heritage = generalHeritagesList.Find(m => m.Type == HeritageType.Heritage);
            if (heritage != null)
            {
                var cacheSet = new GameDataCacheSet<UserGeneral>();
                UserGeneral ugeneral = cacheSet.FindKey(userID, heritage.GeneralID);
                if (ugeneral != null)
                {
                    heritage.GeneralLv = ugeneral.GeneralLv;
                    heritage.PowerNum = ugeneral.TrainingPower;
                    heritage.SoulNum = ugeneral.TrainingSoul;
                    heritage.IntellectNum = ugeneral.TrainingIntellect;
                    GeneralHeritage hGernal = generalHeritagesList.Find(m => m.Type == HeritageType.IsHeritage);
                    if (hGernal != null)
                    {
                        UserGeneral general = cacheSet.FindKey(userID, hGernal.GeneralID);
                        if (general != null)
                        {
                            List<OpsInfo> opsInfoList = HeritageOpsTypeList();
                            OpsInfo info = opsInfoList.Find(m => m.Type == type);
                            if (info != null)
                            {
                                hGernal.GeneralLv = MathUtils.RoundCustom(heritage.GeneralLv * info.Num).ToShort();
                                if (hGernal.GeneralLv < general.GeneralLv)
                                {
                                    hGernal.GeneralLv = general.GeneralLv;
                                }
                                decimal trpownum = (heritage.PowerNum * info.Num).ToDecimal();
                                hGernal.PowerNum = MathUtils.RoundCustom(trpownum).ToShort();
                                if (hGernal.PowerNum < general.TrainingPower)
                                {
                                    hGernal.PowerNum = general.TrainingPower;
                                }
                                hGernal.SoulNum = MathUtils.RoundCustom(heritage.SoulNum * info.Num).ToShort();
                                if (hGernal.SoulNum < general.TrainingSoul)
                                {
                                    hGernal.SoulNum = general.TrainingSoul;
                                }
                                hGernal.IntellectNum = MathUtils.RoundCustom(heritage.IntellectNum * info.Num).ToShort();
                                if (hGernal.IntellectNum < general.TrainingIntellect)
                                {
                                    hGernal.IntellectNum = general.TrainingIntellect;
                                }
                                hGernal.opsType = type;
                                //generalHeritagesList.Add(heritageGernal);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 饱食度恢复
        /// </summary>
        public static void GetFeelHunger(string userID, int generalID)
        {
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            if (userGeneral != null)
            {
                if (userGeneral.HungerDate.Date <= MathUtils.SqlMinDate.Date)
                {
                    userGeneral.HungerDate = DateTime.Now;
                }
                int restorationDate = ConfigEnvSet.GetInt("UserQueue.FeelHungerRestorationDate"); //一小时
                int restorationNum = ConfigEnvSet.GetInt("UserQueue.FeelHungerRestorationNum"); //恢复5点
                int timeCount = (int)(DateTime.Now - userGeneral.HungerDate).TotalSeconds / restorationDate;
                if (timeCount > 0)
                {
                    short hungerNum = (short)(timeCount * restorationNum);
                    userGeneral.SaturationNum = MathUtils.Subtraction(userGeneral.SaturationNum, hungerNum, (short)0);
                    userGeneral.HungerDate = DateTime.Now;
                }
            }
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
            if (package != null)
            {
                var useritem = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemID == itemID);
                foreach (var info in useritem)
                {
                    itemNum = MathUtils.Addition(itemNum, info.Num);
                }
            }
            return itemNum;
        }

        /// <summary>
        /// 竞技场称号
        /// </summary>
        /// <param name="obtainNum"></param>
        /// <returns></returns>
        public static string SportTitleName(int obtainNum)
        {
            string sportsName = string.Empty;
            var sportsInfoArray = new ConfigCacheSet<SportsTitleInfo>().FindAll(u => u.Obtian > 0 && u.Obtian < obtainNum);
            if (sportsInfoArray.Count > 0)
            {
                sportsName = sportsInfoArray[sportsInfoArray.Count - 1].SprotsName;
            }
            else
            {
                var sportsInfo = new ConfigCacheSet<SportsTitleInfo>().Find(s => s.Obtian == 0);
                sportsName = sportsInfo == null ? string.Empty : sportsInfo.SprotsName;
            }
            return sportsName;
        }

        /// <summary>
        /// 屏蔽敏感词
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public static bool GetKeyWordSubstitution(string keyWord)
        {
            keyWord = keyWord.Trim();
            bool IsWord = false;
            ConfigCacheSet<ChatKeyWord> cacheSet = new ConfigCacheSet<ChatKeyWord>();
            List<ChatKeyWord> chatKeyWordArray = cacheSet.FindAll();
            foreach (ChatKeyWord chatKeyWord in chatKeyWordArray)
            {
                if (keyWord.IndexOf(chatKeyWord.KeyWord) >= 0)
                {
                    IsWord = true;
                    break;
                }

            }
            return IsWord;
        }
    }
}
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
using System.Web;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Model;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 神秘事件帮助
    /// </summary>
    public class MysteryHelper
    {
        public static void IsTriggerMyStery(GameUser user, MysteryType mysteryType, out string prizecontent)
        {
            prizecontent = string.Empty;
            int mysteryNum = GetMySteryNum(user.UserID, mysteryType);
            MysteryInfo mysteryInfo = new ConfigCacheSet<MysteryInfo>().FindKey(mysteryType);
            if (mysteryInfo != null && mysteryNum > 0)
            {
                var randomNum = RandomUtils.GetRandom(0, 1000);
                if (randomNum < mysteryInfo.Probability * 1000)
                {
                    CacheList<PrizeInfo> prizeInfoList = mysteryInfo.PrizeInfo;
                    double[] probability = new double[prizeInfoList.Count];
                    for (int i = 0; i < prizeInfoList.Count; i++)
                    {
                        probability[i] = (double)prizeInfoList[i].Probability;
                    }
                    int index2 = RandomUtils.GetHitIndex(probability);
                    PrizeInfo prizeInfo = prizeInfoList[index2];
                    prizecontent = MysteryRewardName(user, prizeInfo);
                    UpdateRestrainExplore(user.UserID, mysteryType);
                }
            }
        }

        /// <summary>
        /// 神秘事件剩余次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="mysteryType"></param>
        /// <returns></returns>
        public static int GetMySteryNum(string userID, MysteryType mysteryType)
        {
            int mysteryNum = 0;
            MysteryInfo mysteryInfo = new ConfigCacheSet<MysteryInfo>().FindKey(mysteryType);
            if (mysteryInfo != null)
            {
                mysteryNum = mysteryInfo.DailyNum;
            }
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && dailyRestrain.UserExtend != null && dailyRestrain.RefreshDate.Date == DateTime.Now.Date)
            {
                if (mysteryType == MysteryType.Meiritanxian)
                {
                    mysteryNum = MathUtils.Subtraction(mysteryNum, dailyRestrain.UserExtend.ExploreNum);
                }
                else if (mysteryType == MysteryType.Jingjichang)
                {
                    mysteryNum = MathUtils.Subtraction(mysteryNum, dailyRestrain.UserExtend.sportsNum);
                }
            }
            return mysteryNum;
        }

        /// <summary>
        /// 神秘事件更新
        /// </summary>
        /// <param name="userID"></param>
        public static void UpdateRestrainExplore(string userID, MysteryType mysteryType)
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null)
            {
                if (dailyRestrain.UserExtend == null || dailyRestrain.RefreshDate.Date != DateTime.Now.Date)
                {
                    dailyRestrain.UserExtend = new DailyUserExtend();
                }
                if (mysteryType == MysteryType.Meiritanxian)
                {
                    dailyRestrain.UserExtend.ExploreNum = MathUtils.Addition(dailyRestrain.UserExtend.ExploreNum, 1);
                }
                else if (mysteryType == MysteryType.Jingjichang)
                {
                    dailyRestrain.UserExtend.sportsNum = MathUtils.Addition(dailyRestrain.UserExtend.sportsNum, 1);
                }
            }
        }

        /// <summary>
        /// 获得奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="prize"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static UserTakePrize GetUserTake(string userID, PrizeInfo prize, string content)
        {
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (user == null)
            {
                return null;
            }
            UserTakePrize userPrize = new UserTakePrize();
            userPrize.CreateDate = DateTime.Now;
            userPrize.ID = Guid.NewGuid().ToString();
            userPrize.UserID = Convert.ToInt32(userID);
            userPrize.MailContent = content;
            userPrize.IsTasked = false;
            userPrize.TaskDate = MathUtils.SqlMinDate;
            userPrize.ItemPackage = string.Empty;
            userPrize.SparePackage = string.Empty;
            userPrize.CrystalPackage = string.Empty;
            userPrize.EnchantPackage = string.Empty;
            userPrize.OpUserID = 10000;
            string prizeContent = string.Empty;
            switch (prize.Type)
            {
                case RewardType.GameGoin:
                    userPrize.GameCoin = prize.Num;
                    prizeContent = string.Format(LanguageManager.GetLang().St_GameCoin, prize.Num);
                    break;
                case RewardType.Obtion:
                    userPrize.ObtainNum = prize.Num;
                    prizeContent = string.Format(LanguageManager.GetLang().St_ObtionNum, prize.Num);
                    break;
                case RewardType.ExpNum:
                    userPrize.ExpNum = prize.Num;
                    prizeContent = string.Format(LanguageManager.GetLang().St_ExpNum, prize.Num);
                    break;
                case RewardType.EnergyNum:
                    userPrize.EnergyNum = prize.Num;
                    prizeContent = string.Format(LanguageManager.GetLang().St_EnergyNum, prize.Num);
                    break;
                case RewardType.Experience:
                    break;
                case RewardType.Gold:
                    userPrize.Gold = prize.Num;
                    prizeContent = string.Format(LanguageManager.GetLang().St_GiftGoldNum, prize.Num);
                    break;
                case RewardType.Item:
                    userPrize.ItemPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.UserLv, prize.Num);
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prize.ItemID);
                    if (itemInfo != null)
                    {
                        prizeContent = string.Format("{0}*{1}", itemInfo.ItemName, prize.Num);
                    }
                    break;
                case RewardType.CrystalId:

                    userPrize.CrystalPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.UserLv, prize.Num);
                    CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(prize.ItemID);
                    if (crystal != null)
                    {
                        prizeContent = string.Format("{0}*{1}", crystal.CrystalName, prize.Num);
                    }
                    break;
                case RewardType.Spare:
                    userPrize.SparePackage = string.Format("{0}={1}={2}", prize.ItemID, prize.UserLv, prize.Num);
                    SparePartInfo spare = new ConfigCacheSet<SparePartInfo>().FindKey(prize.ItemID);
                    if (spare != null)
                    {
                        prizeContent = string.Format("{0}*{1}", spare.Name, prize.Num);
                    }
                    break;
                case RewardType.Enchant:
                    userPrize.EnchantPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.UserLv, prize.Num);
                    EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(prize.ItemID);
                    if (enchantInfo != null)
                    {
                        prizeContent = string.Format("{0}*{1}", enchantInfo.EnchantName, prize.Num);
                    }
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(prizeContent))
            {
                userPrize.MailContent = string.Format(content, user.NickName, prizeContent);
            }
            return userPrize;
        }

        /// <summary>
        /// 奖励物品名称，数量
        /// </summary>
        /// <param name="prize"></param>
        /// <returns></returns>
        public static string MysteryRewardName(GameUser user, PrizeInfo prize)
        {
            string prizeContent = string.Empty;
            switch (prize.Type)
            {
                case RewardType.GameGoin:
                    prizeContent = string.Format(LanguageManager.GetLang().St_GameCoin, prize.Num);
                    user.GameCoin = MathUtils.Addition(user.GameCoin, prize.Num);
                    break;
                case RewardType.Obtion:
                    prizeContent = string.Format(LanguageManager.GetLang().St_ObtionNum, prize.Num);
                    user.ObtainNum = MathUtils.Addition(user.ObtainNum, prize.Num);
                    break;
                case RewardType.ExpNum:
                    prizeContent = string.Format(LanguageManager.GetLang().St_ExpNum, prize.Num);
                    user.ExpNum = MathUtils.Addition(user.ExpNum, prize.Num);
                    break;
                case RewardType.EnergyNum:
                    prizeContent = string.Format(LanguageManager.GetLang().St_EnergyNum, prize.Num);
                    user.EnergyNum = MathUtils.Addition(user.EnergyNum, prize.Num.ToShort());
                    break;
                case RewardType.Experience:
                    UserHelper.UserGeneralExp(user.UserID, prize.Num);
                    break;
                case RewardType.Gold:
                    prizeContent = string.Format(LanguageManager.GetLang().St_GiftGoldNum, prize.Num);
                    user.GiftGold = MathUtils.Addition(user.GiftGold, prize.Num);
                    break;
                case RewardType.Item:
                    short itemLv = prize.UserLv > 0 ? prize.UserLv : 1.ToShort();
                    int priNum = prize.Num > 0 ? prize.Num : 1;
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prize.ItemID);
                    if (itemInfo != null)
                    {
                        UserItemHelper.AddUserItem(user.UserID, itemInfo.ItemID, priNum, itemLv);
                        prizeContent = string.Format("{0}*{1}", itemInfo.ItemName, prize.Num);
                    }
                    break;
                case RewardType.CrystalId:
                    short cLv = prize.UserLv > 0 ? prize.UserLv : 1.ToShort();
                    CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(prize.ItemID);
                    if (crystal != null)
                    {
                        CrystalHelper.AppendCrystal(user.UserID, crystal.CrystalID, cLv);
                        prizeContent = string.Format("{0}*{1}", crystal.CrystalName, prize.Num);
                    }
                    break;
                case RewardType.Spare:
                    break;
                case RewardType.Enchant:
                    break;
                default:
                    break;
            }
            return prizeContent;
        }
    }
}
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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class PrizeHelper
    {
        /// <summary>
        /// 获取奖励
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public static CacheList<PrizeInfo> GetPrizeInfo(CacheList<PrizeInfo> List)
        {
            if (List[0].Reward == 1)
            {
                return new CacheList<PrizeInfo> { List[RandomUtils.GetRandom(0, List.Count)] };
            }
            else if (List[0].Reward == 2)
            {
                CacheList<PrizeInfo> prizeInfosArray = List;
                double[] probability = new double[prizeInfosArray.Count];
                for (int i = 0; i < prizeInfosArray.Count; i++)
                {
                    probability[i] = (double)prizeInfosArray[i].Probability;
                }
                int index2 = RandomUtils.GetHitIndex(probability);
                return new CacheList<PrizeInfo> { prizeInfosArray[index2] };
            }
            else if (List[0].Reward == 3)
            {
                return List;
            }
            return new CacheList<PrizeInfo>();
        }


        public static UserTakePrize GetUserTake(PrizeInfo prize, string userID, FestivalInfo info)
        {
            UserTakePrize userPrize = new UserTakePrize();
            userPrize.CreateDate = DateTime.Now;
            userPrize.ID = Guid.NewGuid().ToString();
            userPrize.UserID = Convert.ToInt32(userID);
            userPrize.MailContent = info.FestivalExtend.KeyWord;
            userPrize.IsTasked = false;
            userPrize.TaskDate = MathUtils.SqlMinDate;
            userPrize.ItemPackage = string.Empty;
            userPrize.SparePackage = string.Empty;
            userPrize.CrystalPackage = string.Empty;
            userPrize.EnchantPackage = string.Empty;
            userPrize.OpUserID = 10000;
            switch (prize.Type)
            {
                case RewardType.GameGoin:
                    userPrize.GameCoin = prize.Num;
                    userPrize.MailContent += string.Format(LanguageManager.GetLang().St_GameCoin, prize.Num);
                    break;
                case RewardType.Obtion:
                    userPrize.ObtainNum = prize.Num;
                    userPrize.MailContent += string.Format(LanguageManager.GetLang().St_ObtionNum, prize.Num);
                    break;
                case RewardType.ExpNum:
                    userPrize.ExpNum = prize.Num;
                    userPrize.MailContent += string.Format(LanguageManager.GetLang().St_ExpNum, prize.Num);
                    break;
                case RewardType.EnergyNum:
                    userPrize.EnergyNum = prize.Num;
                    userPrize.MailContent += string.Format(LanguageManager.GetLang().St_EnergyNum, prize.Num);
                    break;
                case RewardType.Experience:
                    break;
                case RewardType.Gold:
                    userPrize.Gold = prize.Num;
                    userPrize.MailContent += string.Format(LanguageManager.GetLang().St_GiftGoldNum, prize.Num);
                    break;
                case RewardType.Item:
                    userPrize.ItemPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.UserLv, prize.Num);
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prize.ItemID);
                    if (itemInfo != null)
                    {
                        userPrize.MailContent += string.Format("{0}*{1}", itemInfo.ItemName, prize.Num);
                    }
                    break;
                case RewardType.CrystalType:
                    //List<CrystalInfo> crystalArray2 = new ConfigCacheSet<CrystalInfo>().FindAll(CrystalInfo.Index_CrystalQuality, prize.CrystalType);
                    //userPrize.CrystalPackage = string.Format("{0}={1}={2}", itemID, prize.UserLv, itemNum);
                    break;
                case RewardType.CrystalId:

                    userPrize.CrystalPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.UserLv, prize.Num);
                    CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(prize.ItemID);
                    if (crystal != null)
                    {
                        userPrize.MailContent += string.Format("{0}*{1}", crystal.CrystalName, prize.Num);
                    }
                    break;
                case RewardType.Spare:
                    userPrize.SparePackage = string.Format("{0}={1}={2}", prize.ItemID, prize.Num, prize.Num);
                    SparePartInfo spare = new ConfigCacheSet<SparePartInfo>().FindKey(prize.ItemID);
                    if (spare != null)
                    {
                        userPrize.MailContent += string.Format("{0}*{1}", spare.Name, prize.Num);
                    }
                    break;
                case RewardType.Enchant:
                    userPrize.EnchantPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.UserLv, prize.Num);
                    EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(prize.ItemID);
                    if (enchantInfo != null)
                    {
                        userPrize.MailContent += string.Format("{0}*{1}", enchantInfo.EnchantName, prize.Num);
                    }
                    break;
                default:
                    break;
            }
            return userPrize;
        }

        public static string PrizeContent(GameUser userInfo, List<PrizeInfo> prizeList)
        {
            string content = string.Empty;
            int beibaonum = 0;
            int crystalid = 0;
            int sparenum = 0;
            foreach (PrizeInfo info in prizeList)
            {
                if (info.Type == RewardType.CrystalId || info.Type == RewardType.CrystalType)
                {
                    if (info.Reward == 2 && crystalid > 0)
                    {
                        continue;
                    }
                    crystalid++;
                }
                else if (info.Type == RewardType.Item)
                {
                    if (info.Reward == 2 && beibaonum > 0)
                    {
                        continue;
                    }
                    beibaonum++;
                }
            }

            if (beibaonum > 0 && crystalid > 0 && sparenum > 0)
            {
                if (UserHelper.IsBeiBaoFull(userInfo, 0))
                {
                    content = LanguageManager.GetLang().St1107_GridNumFull;
                    return content;
                }

                if (!UserCrystalInfo.CheckFull(userInfo.UserID, 0))
                {
                    content = LanguageManager.GetLang().St1307_FateBackpackFull;
                    return content;
                }

                if (UserHelper.IsSpareGridNum(userInfo, 0))
                {
                    content = LanguageManager.GetLang().St1213_GridNumFull;
                    return content;
                }
            }

            if (beibaonum > 0 && UserHelper.IsBeiBaoFull(userInfo, beibaonum))
            {
                content = LanguageManager.GetLang().St1107_GridNumFull;
                return content;
            }

            if (crystalid > 0 && !UserCrystalInfo.CheckFull(userInfo.UserID, crystalid))
            {
                content = LanguageManager.GetLang().St1307_FateBackSpaceFull;
                return content;
            }

            if (sparenum > 0 && UserHelper.IsSpareGridNum(userInfo, sparenum))
            {
                content = LanguageManager.GetLang().St1213_GridNumFull;
                return content;
            }
            return content;
        }

        /// <summary>
        /// 玩家获得奖励
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="prizeList"></param>
        /// <returns></returns>
        public static string GetPrizeUserTake(GameUser userInfo, List<PrizeInfo> prizeList)
        {
            string prizeContent = string.Empty;
            foreach (var prize in prizeList)
            {
                switch (prize.Type)
                {
                    case RewardType.GameGoin:
                        userInfo.GameCoin = MathUtils.Addition(userInfo.GameCoin, prize.Num);
                        prizeContent += string.Format(LanguageManager.GetLang().St_GameCoin, prize.Num) + ",";
                        break;
                    case RewardType.ExpNum:
                        userInfo.ExpNum = MathUtils.Addition(userInfo.ExpNum, prize.Num);
                        prizeContent += string.Format(LanguageManager.GetLang().St_ExpNum, prize.Num) + ",";
                        break;
                    case RewardType.EnergyNum:
                        userInfo.EnergyNum = MathUtils.Addition(userInfo.EnergyNum, prize.Num.ToShort());
                        prizeContent += string.Format(LanguageManager.GetLang().St_EnergyNum, prize.Num) + ",";
                        break;
                    case RewardType.HonourNum:
                        GeneralEscalateHelper.AddUserLv(userInfo, prize.Num);
                        prizeContent += string.Format(LanguageManager.GetLang().St_HonourNum, prize.Num) + ",";
                        break;
                    case RewardType.Experience:
                        UserHelper.UserGeneralExp(userInfo.UserID, prize.Num);
                        prizeContent += string.Format(LanguageManager.GetLang().St_Experience, prize.Num) + ",";
                        break;
                    case RewardType.Gold:
                        userInfo.GiftGold = MathUtils.Addition(userInfo.GiftGold, prize.Num);
                        prizeContent += string.Format(LanguageManager.GetLang().St_GiftGoldNum, prize.Num) + ",";
                        break;
                    case RewardType.Item:
                        short itemLv = prize.UserLv > 0 ? prize.UserLv : 1.ToShort();
                        int priNum = prize.Num > 0 ? prize.Num : 1;
                        ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prize.ItemID);
                        if (itemInfo != null)
                        {
                            UserItemHelper.AddUserItem(userInfo.UserID, itemInfo.ItemID, priNum, itemLv);
                            prizeContent += string.Format("{0}*{1}", itemInfo.ItemName, priNum) + ",";
                        }
                        break;
                    case RewardType.CrystalType:
                        List<CrystalInfo> crystalArray = new ConfigCacheSet<CrystalInfo>().FindAll(s => s.CrystalQuality == prize.CrystalType);
                        if (crystalArray.Count > 0)
                        {
                            int index1 = RandomUtils.GetRandom(0, crystalArray.Count);
                            CrystalInfo crystalInfo = crystalArray[index1];
                            short crystalLv = prize.UserLv > 0 ? prize.UserLv : 1.ToShort();
                            CrystalHelper.AppendCrystal(userInfo.UserID, crystalInfo.CrystalID, crystalLv);
                        }
                        break;
                    case RewardType.CrystalId:
                        short cLv = prize.UserLv > 0 ? prize.UserLv : 1.ToShort();
                        CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(prize.ItemID);
                        if (crystal != null)
                        {
                            CrystalHelper.AppendCrystal(userInfo.UserID, crystal.CrystalID, cLv);
                            prizeContent += string.Format("{0}*{1}", crystal.CrystalName, prize.Num) + ",";
                        }
                        break;
                    case RewardType.Spare:
                        break;
                    case RewardType.Enchant:
                        break;
                    case RewardType.PayGold:
                        userInfo.PayGold = MathUtils.Addition(userInfo.PayGold, prize.Num);
                        prizeContent += string.Format(LanguageManager.GetLang().St_PayGoldNum, prize.Num) + ",";
                        break;
                    default:
                        break;
                }
            }
            return prizeContent;
        }
    }
}
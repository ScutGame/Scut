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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 礼包使用
    /// </summary>
    public class ItemUseHelper
    {
        /// <summary>
        /// 提示内容
        /// </summary>
        public string content = string.Empty;
        /// <summary>
        /// 使用结果
        /// </summary>
        public bool result = false;

        public static int ItemNum(string userID, int itemId)
        {
            var package = UserItemPackage.Get(userID);
            int num = 0;
            if (package == null)
                return 0;
            var useritemList = package.ItemPackage.FindAll(s => !s.IsRemove && s.ItemID == itemId);
            if (useritemList == null)
            {
                return 0;
            }
            useritemList.ForEach(obj =>
            {
                num = MathUtils.Addition(num, obj.Num);
            });
            return num;
        }
        public bool UseItem(string userItemID, string userID)
        {
            var package = UserItemPackage.Get(userID);
            if (package == null)
                return false;
            UserItemInfo useritem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(userItemID));
            //UserItem useritem = UserItem.FindKey(userItemID);
            if (useritem == null || useritem.Num <= 0)
            {
                content = LanguageManager.GetLang().St1107_UserItemNotEnough;
                return false;
            }
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (userInfo == null)
            {
                content = LanguageManager.GetLang().Load_User_Error;
                return false;
            }
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(useritem.ItemID);
            if (itemInfo == null)
            {
                content = LanguageManager.GetLang().St1107_UserItemNotEnough;
                return false;
            }
            if (itemInfo.ItemType != ItemType.VipItem && itemInfo.DemandLv > userInfo.UserLv)
            {
                content = LanguageManager.GetLang().St_LevelNotEnough;
                return false;
            }
            int beibaonum = 0;
            int crystalid = 0;
            int sparenum = 0;

            List<PrizeInfo> prizeInfosArray = itemInfo.ItemPack.ToList();
            foreach (PrizeInfo info in prizeInfosArray)
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
                else if (info.Type == RewardType.Spare)
                {
                    if (info.Reward == 2 && sparenum > 0)
                    {
                        continue;
                    }
                    sparenum++;
                }
                //原因：开启礼包消耗的物品
                if (info.DemandItem > 0)
                {
                    int demandItemNum = UserHelper.GetUserItemNum(userID, info.DemandItem);
                    if (demandItemNum < info.DemandNum)
                    {
                        ItemBaseInfo itembaseInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(info.DemandItem);
                        if (itembaseInfo != null)
                        {
                            content = string.Format(LanguageManager.GetLang().St1606_OpenPackLackItem, itembaseInfo.ItemName, itemInfo.ItemName);
                            return false;
                        }
                    }
                }
            }

            if (beibaonum > 0 && crystalid > 0 && sparenum > 0)
            {
                if (UserHelper.IsBeiBaoFull(userInfo, 0))
                {
                    content = LanguageManager.GetLang().St1107_GridNumFull;
                    return false;
                }

                if (!UserCrystalInfo.CheckFull(userInfo.UserID, 0))
                {
                    content = LanguageManager.GetLang().St1307_FateBackpackFull;
                    return false;
                }

                if (UserHelper.IsSpareGridNum(userInfo, 0))
                {
                    content = LanguageManager.GetLang().St1213_GridNumFull;
                    return false;
                }
            }

            if (beibaonum > 0 && UserHelper.IsBeiBaoFull(userInfo, beibaonum))
            {
                content = LanguageManager.GetLang().St1107_GridNumFull;
                return false;
            }

            if (crystalid > 0 && !UserCrystalInfo.CheckFull(userInfo.UserID, crystalid))
            {
                content = LanguageManager.GetLang().St1307_FateBackSpaceFull;
                return false;
            }

            if (sparenum > 0 && UserHelper.IsSpareGridNum(userInfo, sparenum))
            {
                content = LanguageManager.GetLang().St1213_GridNumFull;
                return false;
            }

            List<PrizeInfo> prize = GetPrizeInfo(itemInfo);
            if (prize.Count == 0)
            {
                content = LanguageManager.GetLang().St1107_UserItemNotEnough;
                return false;
            }

            if (DoPrize(userInfo, prize))
            {
                UserItemHelper.UseUserItem(userInfo.UserID, useritem.ItemID, 1);
                result = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="itemInfo"></param>
        /// <returns></returns>
        public static List<PrizeInfo> GetPrizeInfo(ItemBaseInfo itemInfo)
        {
            if (itemInfo.ItemPack == null || itemInfo.ItemPack.Count <= 0)
                return null;
            return PrizeHelper.GetPrizeInfo(itemInfo.ItemPack).ToList();
        }

        /// <summary>
        /// 获得物品
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="prize"></param>
        /// <returns></returns>
        public bool DoPrize(GameUser userInfo, List<PrizeInfo> prizeList)
        {
            try
            {
                bool isDeduct = false;
                int crystalNum = 0;
                CrystalInfo crystal = new CrystalInfo();
                content = LanguageManager.GetLang().St_SummerThreeGameCoinNotice.Substring(0, 5);
                foreach (PrizeInfo prize in prizeList)
                {
                    int num = prize.Num;
                    switch (prize.Type)
                    {
                        case RewardType.GameGoin:
                            userInfo.GameCoin = MathUtils.Addition(userInfo.GameCoin, num);
                            if (content.Length == 0)
                                content = string.Format(LanguageManager.GetLang().St_SummerThreeGameCoinNotice, num);
                            else
                                content += string.Format(LanguageManager.GetLang().St_GameCoin, num) + ",";
                            break;
                        case RewardType.Obtion:
                            userInfo.ObtainNum = MathUtils.Addition(userInfo.ObtainNum, num);
                            if (content.Length == 0)
                                content = string.Format(LanguageManager.GetLang().St_SummerThreeObtionNotice, num);
                            else
                                content += string.Format(LanguageManager.GetLang().St_ObtionNum, num) + ",";
                            break;
                        case RewardType.ExpNum:
                            userInfo.ExpNum = MathUtils.Addition(userInfo.ExpNum, num);
                            if (content.Length == 0)
                                content = string.Format(LanguageManager.GetLang().St_SummerThreeExpNumNotice, num);
                            else
                                content += string.Format(LanguageManager.GetLang().St_ExpNum, num) + ",";
                            break;
                        case RewardType.EnergyNum:
                            userInfo.EnergyNum = MathUtils.Addition(userInfo.EnergyNum, (short)num, short.MaxValue);
                            if (content.Length == 0)
                                content = string.Format(LanguageManager.GetLang().St_SummerThreeEnergyNotice, num);
                            else
                                content += string.Format(LanguageManager.GetLang().St_EnergyNum, num) + ",";
                            break;
                        case RewardType.Experience:
                            UserHelper.UserGeneralExp(userInfo.UserID, num);
                            if (content.Length == 0)
                                content = string.Format(LanguageManager.GetLang().St_SummerThreeExperienceNotice, num);
                            else
                                content += string.Format(LanguageManager.GetLang().St_Experience, num) + ",";
                            break;
                        case RewardType.Gold:
                            userInfo.ItemGold = MathUtils.Addition(userInfo.ItemGold, num);
                            if (content.Length == 0)
                                content = string.Format(LanguageManager.GetLang().St_SummerThreeGoldNotice, num);
                            else
                                content += string.Format(LanguageManager.GetLang().St_GiftGoldNum, num) + ",";
                            break;
                        case RewardType.Item:
                            if (UserHelper.IsBeiBaoFull(userInfo))
                            {
                                content = LanguageManager.GetLang().St1107_GridNumFull;
                                return false;
                            }
                            UserItemHelper.AddUserItem(userInfo.UserID, prize.ItemID, num);
                            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prize.ItemID);
                            if (itemInfo != null)
                            {
                                if (content.Length == 0)
                                    content = string.Format(LanguageManager.GetLang().St_SummerThreeItemNotice, num);
                                else
                                    content += string.Format("{0}*{1}", itemInfo.ItemName, num) + ",";
                            }
                            break;
                        case RewardType.CrystalType:
                            if (!UserCrystalInfo.CheckFull(userInfo.UserID, 0))
                            {
                                content = LanguageManager.GetLang().St1307_FateBackpackFull;
                                return false;
                            }
                            List<CrystalInfo> crystalArray2 = new ConfigCacheSet<CrystalInfo>().FindAll(m => m.CrystalQuality == prize.CrystalType);
                            if (crystalArray2.Count > 0)
                            {
                                int randomNum = RandomUtils.GetRandom(0, crystalArray2.Count);
                                int crystalID = crystalArray2[randomNum].CrystalID;
                                UserHelper.CrystalAppend(userInfo.UserID, crystalID, 2);
                                CrystalHelper.SendChat(crystalArray2[randomNum].CrystalID, userInfo);
                                crystal = new ConfigCacheSet<CrystalInfo>().FindKey(crystalID);
                                if (crystal != null)
                                {
                                    content += string.Format(LanguageManager.GetLang().St_Crystal, CrystalHelper.GetQualityName(crystal.CrystalQuality), crystal.CrystalName) + ",";
                                }
                            }
                            else
                            {
                                TraceLog.WriteError("配置出错");
                                return false;
                            }
                            break;
                        case RewardType.CrystalId:
                            if (!UserCrystalInfo.CheckFull(userInfo.UserID, 0))
                            {
                                content = LanguageManager.GetLang().St1307_FateBackpackFull;
                                return false;
                            }
                            UserHelper.CrystalAppend(userInfo.UserID, prize.ItemID, 2);
                            CrystalHelper.SendChat(prize.ItemID, userInfo);
                            crystal = new ConfigCacheSet<CrystalInfo>().FindKey(prize.ItemID);
                            if (crystal != null)
                            {
                                content += string.Format(LanguageManager.GetLang().St_Crystal, CrystalHelper.GetQualityName(crystal.CrystalQuality), crystal.CrystalName) + ",";
                            }
                            break;
                        case RewardType.Spare:
                            int currNum = userInfo.SparePartList.FindAll(m => string.IsNullOrEmpty(m.UserItemID)).Count;
                            if (currNum >= userInfo.UserExtend.SparePartGridNum)
                            {
                                content = LanguageManager.GetLang().St1213_GridNumFull;
                                return false;
                            }
                            UserSparePart sparePart = UserSparePart.GetRandom(prize.ItemID);
                            SparePartInfo partInfo = new ConfigCacheSet<SparePartInfo>().FindKey(prize.ItemID);
                            if (partInfo != null && sparePart != null && UserHelper.AddSparePart(userInfo, sparePart))
                            {
                                SendChat(prize, userInfo.NickName, partInfo.Name);
                            }
                            content = string.Empty;
                            break;
                        default:
                            break;
                    }
                    if (prize.Reward == 3 && !string.IsNullOrEmpty(prize.Desc))
                    {
                        content = prize.Desc;
                    }
                    //原因：开启礼包消耗的物品
                    if (prize.DemandItem > 0 && !isDeduct)
                    {
                        UserItemHelper.UseUserItem(userInfo.UserID, prize.DemandItem, prize.DemandNum);
                        isDeduct = true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                content = "使用礼包出错!";
                TraceLog.WriteError("使用礼包出错!", ex);
            }
            return false;
        }

        public void SendChat(PrizeInfo prize, string NickName, string ItemName)
        {
            if (prize.Desc != null && prize.Desc != string.Empty)
            {
                new TjxChatService().SystemSend(ChatType.World, string.Format(prize.Desc,
                                                NickName, ItemName));
            }
        }

        /// <summary>
        /// 剧情任务奖励背包判断
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="prizeList"></param>
        /// <returns></returns>
        public bool GeneralPrize(GameUser userInfo, List<PrizeInfo> prizeList)
        {
            int beibaonum = 0;
            List<PrizeInfo> prizeInfosArray = prizeList;
            foreach (PrizeInfo info in prizeInfosArray)
            {
                if (info.Type == RewardType.Item)
                {
                    if ((info.Reward == 1 || info.Reward == 2) && beibaonum > 0)
                    {
                        continue;
                    }
                    beibaonum++;
                }
            }

            if (UserHelper.IsBeiBaoFull(userInfo, beibaonum))
            {
                content = LanguageManager.GetLang().St1107_GridNumFull;
                return false;
            }
            return true;
        }
    }
}
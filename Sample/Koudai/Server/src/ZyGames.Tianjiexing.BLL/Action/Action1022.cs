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
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1022_补偿领取接口
    /// </summary>
    public class Action1022 : BaseAction
    {
        public Action1022(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1022, httpGet)
        {

        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            List<UserTakePrize> userPrizeList = new ShareCacheStruct<UserTakePrize>().FindAll(m => !m.IsTasked && m.UserID == ObjectExtend.ToInt(Uid));
            if (userPrizeList.Count > 0)
            {
                var userPrize = userPrizeList[0];
                if (userPrize.ItemPackage.Length > 0)
                {
                    if (UserItemHelper.CheckItemOut(ContextUser, ItemStatus.BeiBao, PutItemNum(userPrize)))
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1107_GridNumFull;
                        return false;
                    }
                }
                if (userPrize.SparePackage.Length > 0)
                {
                    int currNum = ContextUser.SparePartList.FindAll(m => string.IsNullOrEmpty(m.UserItemID)).Count;
                    currNum = MathUtils.Addition(currNum, PutCrystalSpareNum(userPrize.SparePackage.Split(','), 1));
                    //原因：零件等于开启格子时提示背包已满
                    if (currNum > ContextUser.UserExtend.SparePartGridNum)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1213_GridNumFull;
                        return false;
                    }
                }

                if (userPrize.CrystalPackage.Length > 0)
                {
                    var package = UserCrystalPackage.Get(ContextUser.UserID);
                    UserCrystalInfo[] crystalArrayErr =
                        package.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(0)).ToArray();
                    int crystalNum = MathUtils.Addition(crystalArrayErr.Length, PutCrystalSpareNum(userPrize.CrystalPackage.Split(','), 2));
                    //原因：命运水晶等于开启格子时提示背包已满
                    if (crystalNum > ContextUser.CrystalNum)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1307_FateBackpackFull;
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(userPrize.EnchantPackage) && userPrize.EnchantPackage.Length > 0)
                {
                    var package = UserEnchant.Get(ContextUser.UserID);
                    if (package != null)
                    {
                        var enchantList = package.EnchantPackage.FindAll(m => string.IsNullOrEmpty(m.UserItemID));
                        int enchantNum = MathUtils.Addition(enchantList.Count, PutCrystalSpareNum(userPrize.EnchantPackage.Split(','), 2));
                        if (ContextUser.UserExtend != null && enchantNum > ContextUser.UserExtend.EnchantGridNum)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1259_EnchantGridNumFull;
                            return false;
                        }
                    }
                }

                if (doProcessPrize(userPrize))
                {
                    ErrorInfo = userPrize.MailContent;
                }
                else
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
            }
            return true;
        }

        private bool doProcessPrize(UserTakePrize userPrize)
        {
            Base.BaseLog log = new Base.BaseLog();
            if (userPrize == null) return false;
            string userID = userPrize.UserID.ToString();
            var gameUser = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (gameUser == null) return false;

            log.SaveDebugLog(string.Format("玩家{0}领取奖励{1}:{2}", userID, userPrize.ID, userPrize.MailContent));
            userPrize.IsTasked = true;
            userPrize.TaskDate = DateTime.Now;
            //userPrize.Update();

            gameUser.ObtainNum = MathUtils.Addition(gameUser.ObtainNum, userPrize.ObtainNum);
            gameUser.EnergyNum = MathUtils.Addition(gameUser.EnergyNum, userPrize.EnergyNum.ToShort());
            gameUser.GameCoin = MathUtils.Addition(gameUser.GameCoin, userPrize.GameCoin);
            gameUser.GiftGold = MathUtils.Addition(gameUser.GiftGold, userPrize.Gold);
            gameUser.ExpNum = MathUtils.Addition(gameUser.ExpNum, userPrize.ExpNum);
            if (gameUser.UserExtend == null)
            {
                gameUser.UserExtend = new GameUserExtend();
            }
            gameUser.UserExtend.GainBlessing = MathUtils.Addition(gameUser.UserExtend.GainBlessing, userPrize.GainBlessing);
            if (userPrize.VipLv > 0)
            {
                var vipLv = new ConfigCacheSet<VipLvInfo>().FindKey(userPrize.VipLv.ToShort()) ?? new VipLvInfo();
                gameUser.ExtGold = vipLv.PayGold;
            }
            //gameUser.Update();

            PutItemPackage(userPrize.ItemPackage.Split(','), userID);
            PutCrystal(userPrize.CrystalPackage.Split(','), userID);
            PutSparePackage(userPrize.SparePackage.Split(','), userID);
            if (!string.IsNullOrEmpty(userPrize.EnchantPackage))
            {
                PutEnchant(userPrize.EnchantPackage.Split(','), userID);
            }
            var chatService = new TjxChatService();
            chatService.SystemSendWhisper(gameUser, string.Format("{0}", userPrize.MailContent));

            return true;
        }

        /// <summary>
        /// 灵件ID=数量=属性ID:区间|属性ID:区间
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userID"></param>
        private static void PutSparePackage(string[] list, string userID)
        {
            var chatService = new TjxChatService();
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item)) continue;
                string[] itemList = item.Split(new char[] { '=' });

                int spareID = itemList.Length > 0 ? Convert.ToInt32(itemList[0]) : 0;
                int spareNum = itemList.Length > 1 ? Convert.ToInt32(itemList[1]) : 0;
                string[] propertys = itemList.Length > 2 ? itemList[2].ToNotNullString().Split('|') : new string[0];
                var sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(spareID);
                if (spareNum > 0 && propertys.Length > 0 && sparePartInfo != null)
                {
                    for (int i = 0; i < spareNum; i++)
                    {
                        UserSparePart sparePart = UserSparePart.CreateSparePart(spareID, propertys, ':');
                        if (sparePart == null) continue;
                        GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
                        if (!UserHelper.AddSparePart(user, sparePart))
                        {
                            chatService.SystemSendWhisper(user, string.Format(LanguageManager.GetLang().St4303_SparePartFalling, sparePartInfo.Name) + "*" + spareNum);
                        }
                    }
                }
                else
                {
                    new Base.BaseLog().SaveLog("领取灵件异常", new Exception(string.Format("userID:{3},spareID:{0},spareNum:{1},propertys:{2}", spareID, spareNum, propertys.Length, userID)));

                }
            }
        }

        /// <summary>
        /// 物品ID=等级=数量
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userID"></param>
        private static void PutItemPackage(string[] list, string userID)
        {
            foreach (string item in list)
            {
                if (string.IsNullOrEmpty(item)) continue;
                string[] itemList = item.Split(new char[] { '=' });

                int itemID = itemList.Length > 0 ? Convert.ToInt32(itemList[0]) : 0;
                short itemLv = itemList.Length > 1 ? Convert.ToInt16(itemList[1]) : (short)0;
                int itemNum = itemList.Length > 2 ? Convert.ToInt32(itemList[2]) : 0;

                if (itemNum > 0 && new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID) != null)
                {
                    UserItemHelper.AddUserItem(userID, itemID, itemNum, ItemStatus.BeiBao, itemLv);
                }
                else
                {
                    new Base.BaseLog().SaveLog("领取物品异常", new Exception(string.Format("userID:{3},itemID:{0},itemNum:{1},itemLv:{2}", itemID, itemNum, itemLv, userID)));
                }
            }
        }

        /// <summary>
        /// 物品个数
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userID"></param>
        private static int PutItemNum(UserTakePrize userPrize)
        {
            int packNum = 0;
            int subNum = 0;
            int sumNum = 0;
            string[] list = userPrize.ItemPackage.Split(',');
            foreach (string item in list)
            {
                if (string.IsNullOrEmpty(item)) continue;
                string[] itemList = item.Split(new char[] { '=' });

                int itemID = itemList.Length > 0 ? Convert.ToInt32(itemList[0]) : 0;
                int itemNum = itemList.Length > 2 ? Convert.ToInt32(itemList[2]) : 0;
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
                if (itemInfo != null)
                {
                    packNum = itemNum / itemInfo.PackMaxNum;
                    subNum = itemNum % itemInfo.PackMaxNum;
                }
                if (subNum > 0)
                {
                    sumNum = MathUtils.Addition(sumNum, packNum);
                    sumNum = MathUtils.Addition(sumNum, 1);
                }
                else
                {
                    sumNum = MathUtils.Addition(sumNum, packNum);
                }
            }
            return sumNum;
        }

        /// <summary>
        /// 1: 灵石个数 2:命运水晶个数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int PutCrystalSpareNum(string[] list, int typeID)
        {
            int packNum = 0;
            int itemNum = 0;
            foreach (string item in list)
            {
                if (string.IsNullOrEmpty(item)) continue;
                string[] itemList = item.Split(new char[] { '=' });
                if (typeID == 1)
                {
                    itemNum = itemList.Length > 1 ? Convert.ToInt32(itemList[1]) : 0;
                }
                else if (typeID == 2)
                {
                    itemNum = itemList.Length > 2 ? Convert.ToInt32(itemList[2]) : 0;
                }
                packNum = MathUtils.Addition(packNum, itemNum);
            }
            return packNum;
        }

        /// <summary>
        /// 命运水晶ID=等级=数量
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userID"></param>
        private static void PutCrystal(string[] list, string userID)
        {
            var package = UserCrystalPackage.Get(userID);
            foreach (string crystal in list)
            {
                if (string.IsNullOrEmpty(crystal)) continue;
                string[] crystalList = crystal.Split(new char[] { '=' });

                int crystalID = crystalList.Length > 0 ? Convert.ToInt32(crystalList[0]) : 0;
                short crystalLv = crystalList.Length > 1 ? Convert.ToInt16(crystalList[1]) : (short)0;
                int crystalNum = crystalList.Length > 2 ? Convert.ToInt32(crystalList[2]) : 0;

                CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(crystalID);
                var crystalLvInfo = new ConfigCacheSet<CrystalLvInfo>().FindKey(crystalID, crystalLv);
                if (crystalNum > 0 && new ConfigCacheSet<CrystalInfo>().FindKey(crystalID) != null && crystalLvInfo != null)
                {
                    for (int i = 0; i < crystalNum; i++)
                    {
                        UserCrystalInfo userCrystal = new UserCrystalInfo();
                        userCrystal.UserCrystalID = Guid.NewGuid().ToString();
                        userCrystal.CrystalID = crystalInfo.CrystalID;
                        userCrystal.CrystalLv = crystalLv;
                        userCrystal.GeneralID = 0;
                        userCrystal.IsSale = 2;
                        userCrystal.Position = 0;
                        userCrystal.CurrExprience = crystalLvInfo.UpExperience;
                        package.SaveCrystal(userCrystal);
                    }
                }
                else
                {
                    new Base.BaseLog().SaveLog("领取命运水晶异常", new Exception(string.Format("userID:{3},crystalID:{0},crystalNum:{1},crystalLv:{2}", crystalID, crystalNum, crystalLv, userID)));
                }
            }
        }

        /// <summary>
        /// 附魔符ID=等级=数量
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userID"></param>
        private static void PutEnchant(string[] list, string userID)
        {
            var package = UserEnchant.Get(userID);
            foreach (string enchant in list)
            {
                if (string.IsNullOrEmpty(enchant)) continue;
                string[] crystalList = enchant.Split(new char[] { '=' });

                int enchantID = crystalList.Length > 0 ? Convert.ToInt32(crystalList[0]) : 0;
                short enchantLv = crystalList.Length > 1 ? Convert.ToInt16(crystalList[1]) : (short)0;
                int enchantNum = crystalList.Length > 2 ? Convert.ToInt32(crystalList[2]) : 0;

                EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enchantID);
                var enchantLvInfo = new ConfigCacheSet<EnchantLvInfo>().FindKey(enchantID, enchantLv);
                if (enchantInfo != null && package != null && enchantLvInfo != null)
                {
                    for (int i = 0; i < enchantNum; i++)
                    {
                        UserEnchantInfo userenchant = EnchantHelper.GetUserEnchantInfo(enchantID, enchantLv);
                        if (userenchant != null)
                        {
                            UserLogHelper.AppenEnchantLog(userID, 2, userenchant, new CacheList<SynthesisInfo>());
                            package.SaveEnchant(userenchant);
                        }
                    }
                }
                else
                {
                    new Base.BaseLog().SaveLog("领取附魔符异常", new Exception(string.Format("userID:{3},enchantID:{0},enchantNum:{1},enchantLv:{2}", enchantID, enchantNum, enchantLv, userID)));
                }
            }
        }
    }
}
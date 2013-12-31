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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Net;
using ZyGames.Framework.SyncThreading;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.Model
{

    public class UserItemHelper
    {
        private UserItemInfo _item;
        private ItemBaseInfo _itemInfo;

        private static int _strongCount = 1;  // 强化次数

        public UserItemHelper(UserItemInfo item)
        {
            _item = item;
            _itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(_item.ItemID) ?? new ItemBaseInfo();
        }

        // 重载构造函数，用于强化 10 次
        public UserItemHelper(UserItemInfo item, int count)
        {
            _item = item;
            _itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(_item.ItemID) ?? new ItemBaseInfo();
            _strongCount = count;
        }

        public bool CheckCareer(int careerId)
        {
            return _itemInfo.CheckCareer(careerId);
        }

        /// <summary>
        /// 佣兵状态
        /// </summary>
        public GeneralStatus GeneralStatus(string userID)
        {
            var cacheSet = new GameDataCacheSet<UserGeneral>();
            if (cacheSet.FindKey(userID, _item.GeneralID) != null)
            {
                return _item.IsNotUsed ? Model.GeneralStatus.YinCang : cacheSet.FindKey(userID, _item.GeneralID).GeneralStatus;
            }
            return Model.GeneralStatus.YinCang;
        }

        /// <summary>
        /// 物品类型
        /// </summary>
        public short PropType
        {
            get
            {
                return _itemInfo.PropType;
            }
        }
        /// <summary>
        /// 售出价格
        /// </summary>
        public int SalePrice
        {
            get
            {
                return (_itemInfo.SalePrice * _item.Num / 4);
            }
        }

        /// <summary>
        /// 需求等级
        /// </summary>
        public int DemandLv
        {
            get
            {
                return _itemInfo.DemandLv;
            }
        }

        public int MedicineType
        {
            get
            {
                return (int)_itemInfo.MedicineType;
            }
        }

        public QualityType QualityType
        {
            get
            {
                return _itemInfo.QualityType;
            }
        }

        /// <summary>
        /// [只读]强化用钱
        /// </summary>
        public int StrongMoney
        {
            get
            {
                return GetStrongMoney(_item.ItemID, (int)_item.ItemLv);
            }
        }

        /// <summary>
        /// 装备部位
        /// </summary>
        public int EquPartsID
        {
            get
            {
                return (int)_itemInfo.EquParts;
            }
        }

        public static List<UserItemInfo> GetItems(string userId)
        {
            var package = UserItemPackage.Get(userId);
            return package.ItemPackage.FindAll(m => !m.IsRemove);

        }
        /// <summary>
        /// 检查物品
        /// </summary>
        /// <returns></returns>
        public static int CheckItemNum(string userID, int itemID)
        {
            int result = 0;
            var package = UserItemPackage.Get(userID);
            var userItems = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemID == itemID);
            foreach (var userItem in userItems)
            {
                result += userItem.Num;
            }
            return result;
        }

        /// <summary>
        /// 检查物品是否已满
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemStatus"></param>
        /// <returns></returns>
        public static bool CheckItemOut(GameUser user, ItemStatus itemStatus)
        {
            //原因：背包差一格满时掉落
            return CheckItemOut(user, itemStatus, 0);
        }

        public static bool CheckItemOut(GameUser user, ItemStatus itemStatus, int gridNum)
        {
            if (user == null) return false;
            int gridMaxNum = 0;
            if (itemStatus == ItemStatus.BeiBao)
            {
                gridMaxNum = user.GridNum;
            }
            else if (itemStatus == ItemStatus.CangKu)
            {
                gridMaxNum = user.WarehouseNum;
            }

            var package = UserItemPackage.Get(user.UserID);
            var itemList = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus.Equals(itemStatus));
            int currGridNum = gridMaxNum - itemList.Count;
            //原因：物品数量等于开启格子时提示背包已满
            if (gridNum > 0)
            {
                return currGridNum < gridNum;
            }
            return currGridNum <= gridNum;
        }



        public static void AddUserItem(string userID, int itemID, int itemNum, short itemLv)
        {
            AddUserItem(userID, itemID, itemNum, ItemStatus.BeiBao, itemLv);
        }

        public static void AddUserItem(string userID, int itemID, int itemNum, List<UniversalInfo> universalInfoList = null)
        {
            AddUserItem(userID, itemID, itemNum, ItemStatus.BeiBao, universalInfoList);
        }

        public static void AddUserItem(string userID, int itemID, int itemNum, ItemStatus itemStatus, List<UniversalInfo> universalInfoList = null)
        {
            AddUserItem(userID, itemID, itemNum, itemStatus, 1, universalInfoList);
        }

        /// <summary>
        /// 判断背包是否已满
        /// </summary>
        /// <param name="backpackType"></param>
        /// <returns></returns>
        public static bool PackIsFull(string userId, BackpackType backpackType, int buyNum)
        {
            var _cacheSetUserPack = new GameDataCacheSet<UserPack>();

            bool isFull = false;
            var userPack = _cacheSetUserPack.FindKey(userId);
            var pack = userPack != null && userPack.PackTypeList != null
                           ? userPack.PackTypeList.Find(s => s.BackpackType == backpackType)
                           : null;
            int position = pack != null ? pack.Position : 0;
            int num = 0;
            return isFull;
        }
        /// <summary>
        /// 添加玩家魂技
        /// </summary>
        /// <param name="abilityId"></param>
        /// <param name="userId"></param>
        public static void AddUserAbility(int abilityId, int userId, int generalID, int position)
        {
            var cacheSetAbility = new GameDataCacheSet<UserAbility>();
            var userAbility = cacheSetAbility.FindKey(userId.ToString());
            var ability = userAbility != null && userAbility.AbilityList != null
                              ? userAbility.AbilityList.Find(s => s.AbilityID == abilityId)
                              : null;
            var abilityLv = new ConfigCacheSet<AbilityLvInfo>().FindKey(abilityId, 1);
            int experienceNum = abilityLv != null ? abilityLv.Experience : 0;
            if (userAbility == null)
            {
                userAbility = new UserAbility(userId);
                ability = new Ability();
                userAbility.CreateDate = DateTime.Now;
                ability.UserItemID = Guid.NewGuid().ToString();
                ability.AbilityID = abilityId;
                ability.AbilityLv = 1;
                ability.GeneralID = generalID;
                ability.ExperienceNum = experienceNum;
                ability.Position = position;
                userAbility.AbilityList.Add(ability);
                cacheSetAbility.Add(userAbility);
                // 添加到玩家集邮册
                UserAlbumHelper.AddUserAlbum(userId.ToString(), AlbumType.Ability, abilityId);
            }
            else
            {
                ability = new Ability();
                ability.UserItemID = Guid.NewGuid().ToString();
                userAbility.CreateDate = DateTime.Now;
                ability.AbilityID = abilityId;
                ability.AbilityLv = 1;
                ability.GeneralID = generalID;
                ability.Position = position;
                ability.ExperienceNum = experienceNum;
                userAbility.AbilityList.Add(ability);
                // 添加到玩家集邮册
                UserAlbumHelper.AddUserAlbum(userId.ToString(), AlbumType.Ability, abilityId);
            }
        }
        public static void AddUniversalInfo(List<UniversalInfo> universalInfoList, ItemBaseInfo itemInfo,int num)
        {
            if (universalInfoList != null)
            {
                var universalInfo = universalInfoList.Find(s => s.HeadID == itemInfo.HeadID);
                if (universalInfo != null)
                {
                    universalInfo.UpdateNotify(obj =>
                    {
                        universalInfo.Num = MathUtils.Addition(universalInfo.Num, num);
                        return true;
                    }
                    );
                }
                else
                {
                    universalInfo = new UniversalInfo();
                    universalInfo.Name = itemInfo.ItemName;
                    universalInfo.HeadID = itemInfo.HeadID;
                    universalInfo.Num = num;
                    universalInfo.ItemID = itemInfo.ItemID;
                    universalInfo.MaxHeadID = itemInfo.MaxHeadID;
                    universalInfo.ItemDesc = itemInfo.ItemDesc;
                    universalInfo.QualityType = itemInfo.QualityType.ToInt();
                    universalInfoList.Add(universalInfo);
                }
            }
        }

        /// <summary>
        /// 添加装备到玩家集邮表
        /// </summary>
        /// <param name="userID">玩家ID</param>
        /// <param name="itemID">物品ID</param>
        private static void AddUserAlbum(string userID, int itemID)
        {
            // 判断物品时否是装备
            var itemBaseinfo = new ShareCacheStruct<ItemBaseInfo>().Find(s => s.ItemID == itemID && s.ItemType == ItemType.ZhuangBei);
            if (itemBaseinfo != null)
            {
                UserAlbumHelper.AddUserAlbum(userID, AlbumType.Item, itemID);
            }
        }

        /// <summary>
        /// 增加物品
        /// </summary>
        public static void AddUserItem(string userID, int itemID, int itemNum, ItemStatus itemStatus, short itemLv, List<UniversalInfo> universalInfoList = null)
        {
            // 增加玩家集邮册
            AddUserAlbum(userID,itemID);

            var chatService = new TjxChatService();
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);

            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            if (user == null || itemInfo == null)
            {
                return;
            }
            AddUniversalInfo(universalInfoList, itemInfo, itemNum);

            if (itemInfo.ItemType == ItemType.DaoJu && itemInfo.PropType == 17)
            {
                for (int i = 0; i < itemNum; i++)
                {

                    if (!PackIsFull(userID, BackpackType.HunJi, 1))
                    {

                        AddUserAbility(itemInfo.EffectNum, userID.ToInt(), 0, 0);
                    }
                }

            }
            else
            {
                if (itemInfo.ItemType == ItemType.DaoJu && itemInfo.PropType == 20)
                {
                    AddGeneralSoul(userID, itemInfo.ItemID, itemNum);

                }
                else
                {
                    int gridMaxNum = 0;
                    if (itemInfo.ItemType == ItemType.ZhuangBei)
                    {
                        gridMaxNum = GetPackTypePositionNum(BackpackType.ZhuangBei, userID);
                    }
                    else
                    {
                        gridMaxNum = GetPackTypePositionNum(BackpackType.BeiBao, userID);
                    }
                    int packNum = itemNum / itemInfo.PackMaxNum;
                    int num = itemNum % itemInfo.PackMaxNum;
                    ////判断背包是否有空位
                    //bool isOut = false;
                    //if (itemStatus == ItemStatus.BeiBao)
                    //{
                    //    isOut = CheckItemOut(user, itemStatus);
                    //}

                    var package = UserItemPackage.Get(userID);
                    var itemList = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus.Equals(ItemStatus.BeiBao));
                    //背包中是否有余
                    var packItemList = package.ItemPackage.FindAll(
                        m => !m.IsRemove && m.ItemStatus.Equals(itemStatus) &&
                             m.ItemID == itemID &&
                             m.Num < itemInfo.PackMaxNum);

                    //当前空位
                    int currGridNum = gridMaxNum - itemList.Count;
                    // if ((isOut && packItemList.Count == 0) || currGridNum < packNum)
                    if (packItemList.Count == 0 && currGridNum <= 0)
                    {
                        AddItemLog(userID, itemID, itemNum, 1, 0, null);
                        chatService.SystemSendWhisper(user, string.Format("掉落物品{0}*{1}", itemInfo.ItemName, itemNum));
                        return;
                    }
                    currGridNum = MathUtils.Subtraction(currGridNum, packNum, 0);

                    for (int i = 0; i < packNum; i++)
                    {
                        //增加整包
                        UserItemInfo userItem = new UserItemInfo();
                        string uitemID = SetUserItem(userItem, userID, itemInfo, itemStatus, itemLv);
                        userItem.Num = itemInfo.PackMaxNum;

                        package.SaveItem(userItem);
                        AddItemLog(userID, itemID, userItem.Num, userItem.ItemLv, 1, uitemID);
                    }
                    //背包中是否有余
                    foreach (var item in packItemList)
                    {
                        if (num == 0) break;

                        int gridNum = itemInfo.PackMaxNum - item.Num;
                        int tempNum = gridNum > num ? num : gridNum;

                        item.Num = MathUtils.Addition(item.Num, tempNum, itemInfo.PackMaxNum);

                        package.SaveItem(item);
                        num = num > tempNum ? num - tempNum : 0;
                    }
                    //数据是否有余
                    if (num > 0)
                    {
                        if (currGridNum <= 0)
                        {
                            AddItemLog(userID, itemInfo.ItemID, num, 1, 0, null);
                            chatService.SystemSendWhisper(user, string.Format("掉落物品{0}*{1}", itemInfo.ItemName, num));
                        }
                        else
                        {
                            var userItem = new UserItemInfo();
                            string uitemID = SetUserItem(userItem, userID, itemInfo, itemStatus, itemLv);
                            userItem.Num = num;
                            package.SaveItem(userItem);

                            AddItemLog(userID, itemID, userItem.Num, userItem.ItemLv, 1, uitemID);
                        }
                    }
                }
            }
        }

        public static bool IsEnoughBeiBaoItem(string userID, int itemID, int itemNum)
        {
            int maxNum = 0;
            var package = UserItemPackage.Get(userID);
            var itemList = package.ItemPackage.FindAll(
                m => !m.IsRemove && m.ItemStatus.Equals(ItemStatus.BeiBao) &&
                     m.ItemID == itemID && m.IsNotUsed);
            foreach (var userItem in itemList)
            {
                maxNum = MathUtils.Addition(maxNum, userItem.Num, int.MaxValue);
            }
            if (maxNum >= itemNum) return true;
            return false;
        }
        /// 获得灵魂
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="num"></param>
        public static void AddGeneralSoul(string userId, int itemId, int num)
        {
            var cacheSetGeneral = new GameDataCacheSet<UserGeneral>();
            var cacheSetGeneralInfo = new ConfigCacheSet<GeneralInfo>();
            var generalInfo = cacheSetGeneralInfo.Find(s => s.SoulID == itemId);
            var userGeneral = cacheSetGeneral.FindKey(userId, itemId);
            if (userGeneral != null && generalInfo != null)
            {
                userGeneral.AtmanNum = MathUtils.Addition(userGeneral.AtmanNum, num).ToShort();
            }
            else
            {
                if (generalInfo != null)
                {
                    userGeneral = new UserGeneral();
                    userGeneral.UserID = userId;
                    userGeneral.GeneralID = itemId;
                    userGeneral.AtmanNum = num;
                    userGeneral.GeneralName = generalInfo.GeneralName;
                    userGeneral.HeadID = generalInfo.HeadID;
                    userGeneral.PicturesID = generalInfo.PicturesID;
                    userGeneral.GeneralLv = generalInfo.GeneralLv;
                    userGeneral.LifeNum = generalInfo.LifeNum;
                    userGeneral.GeneralType = generalInfo.GeneralType;
                    userGeneral.CareerID = generalInfo.CareerID;
                    userGeneral.PowerNum = generalInfo.PowerNum;
                    userGeneral.SoulNum = generalInfo.SoulNum;
                    userGeneral.IntellectNum = generalInfo.IntellectNum;
                    userGeneral.TrainingPower = 0;
                    userGeneral.TrainingSoul = 0;
                    userGeneral.TrainingIntellect = 0;
                    userGeneral.HitProbability = ConfigEnvSet.GetDecimal("Combat.HitiNum");
                    userGeneral.AbilityID = generalInfo.AbilityID;
                    userGeneral.Momentum = 0;
                    userGeneral.Description = generalInfo.Description;
                    userGeneral.AddGeneralStatus(2);
                    userGeneral.CurrExperience = 0;
                    userGeneral.Experience1 = 0;
                    userGeneral.Experience2 = 0;
                    userGeneral.AbilityNum = 3;
                    cacheSetGeneral.Add(userGeneral);
                }
            }
        }
        /// <summary>
        /// 使用物品
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemID"></param>
        /// <param name="itemNum"></param>
        /// <returns>false:物品不够</returns>
        public static bool UseUserItem(string userID, int itemID, int itemNum)
        {
            var loger = new BaseLog();
            bool result = false;
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            if (itemInfo == null) return result;
            var package = UserItemPackage.Get(userID);

            AddItemLog(userID, itemID, itemNum, 1, 2, itemID.ToString());

            int packNum = itemNum / itemInfo.PackMaxNum;
            int num = itemNum % itemInfo.PackMaxNum;

            var itemList = new List<UserItemInfo>();
            if (IsEnoughBeiBaoItem(userID, itemID, itemNum))
            {
                itemList = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemID == itemID && m.IsNotUsed && m.ItemStatus == ItemStatus.BeiBao);
            }
            else
            {
                itemList = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemID == itemID && m.IsNotUsed && m.ItemStatus != ItemStatus.Sell);
            }

            List<UserItemInfo> removeItem = new List<UserItemInfo>();
            List<UserItemInfo> updateItem = new List<UserItemInfo>();
            UserItemInfo packItem = null;
            foreach (UserItemInfo item in itemList)
            {
                if (packNum > 0 && item.Num == itemInfo.PackMaxNum)
                {
                    //删除整包的
                    item.Num = 0;
                    package.SaveItem(item);
                    removeItem.Add(item);
                    //package.ItemPackage.UpdateChildren(item, obj =>
                    //{
                    //    obj.Num = 0;
                    //    removeItem.Add(obj);
                    //});
                    packNum--;
                }
                else if (num > 0 && item.Num < itemInfo.PackMaxNum)
                {
                    //删除散的
                    if (item.Num > num)
                    {
                        item.Num = MathUtils.Subtraction(item.Num, num, 0);
                        num = 0;
                        updateItem.Add(item);
                        package.SaveItem(item);
                    }
                    else
                    {
                        num = MathUtils.Subtraction(num, item.Num, 0);
                        item.Num = 0;
                        removeItem.Add(item);
                        package.SaveItem(item);
                    }
                }
                else if (item.Num == itemInfo.PackMaxNum)
                {
                    //取剩余第一个整包
                    if (packItem == null)
                    {
                        packItem = item;
                    }
                }
            }
            //若散的不足再从整包中扣除
            if (num > 0 && packItem != null)
            {
                packItem.Num = MathUtils.Subtraction(packItem.Num, num, 0);
                num = 0;
                package.SaveItem(packItem);
                updateItem.Add(packItem);
            }

            if (packNum == 0 && num == 0)
            {
                foreach (UserItemInfo item in removeItem)
                {
                    package.RemoveItem(item);
                }

                //foreach (UserItemInfo item in updateItem)
                //{
                //    package.DelayChange();
                //}
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 传入仓库或取出物品后合并物品
        /// </summary>
        public static void MergerUserItem(string userID, string userItemID, ItemStatus itemStatus)
        {
            var package = UserItemPackage.Get(userID);
            UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && string.Equals(m.UserItemID, userItemID));
            if (userItem != null)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
                if (itemInfo == null) return;
                if (userItem.Num == itemInfo.PackMaxNum)
                {
                    userItem.ItemStatus = itemStatus;
                    package.SaveItem(userItem);
                }
                else
                {
                    int itemNum = userItem.Num;
                    var itemArray = package.ItemPackage.FindAll(u => !u.IsRemove && u.ItemID == userItem.ItemID && u.ItemStatus.Equals(itemStatus));
                    foreach (var item in itemArray)
                    {
                        if (item.Num < itemInfo.PackMaxNum)
                        {

                            int subNum = MathUtils.Subtraction(itemInfo.PackMaxNum, item.Num, 0);
                            item.Num = MathUtils.Addition(item.Num, itemNum, itemInfo.PackMaxNum);
                            itemNum = MathUtils.Subtraction(itemNum, subNum, 0);
                            package.SaveItem(item);
                            //item.Update();
                        }
                    }
                    if (itemNum > 0)
                    {
                        userItem.Num = itemNum;
                        userItem.ItemStatus = itemStatus;
                        package.SaveItem(userItem);
                    }
                    else
                    {
                        AddItemLog(userID, userItem.ItemID, itemNum, userItem.ItemLv, 7, userItemID);

                        package.RemoveItem(userItem);
                    }
                }
            }
        }

        private static string SetUserItem(UserItemInfo userItem, string userID, ItemBaseInfo itemInfo, ItemStatus itemStatus, short itemLv)
        {
            string uitemID = System.Guid.NewGuid().ToString();
            userItem.UserItemID = uitemID;
            userItem.ItemID = itemInfo.ItemID;
            userItem.ItemStatus = itemStatus;
            userItem.GeneralID = itemStatus == ItemStatus.YongBing ? UserGeneral.MainID : 0;
            userItem.ItemLv = itemLv;
            userItem.ItemType = itemInfo.ItemType;
            return uitemID;
        }

        /// <summary>
        /// -1:掉落，1增加，2使用,3出售，4出售删除 5合成,6购回
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemID"></param>
        /// <param name="num"></param>
        /// <param name="itemLv"></param>                     
        /// <param name="itemStatus"></param>
        public static void AddItemLog(string userID, int itemID, int num, short itemLv, int itemStatus, string useritemID)
        {
            var itemLog = new UserItemLog();
            itemLog.LogID = Guid.NewGuid().ToString();
            itemLog.ItemID = itemID;
            itemLog.UserID = userID;
            itemLog.ItemStatus = (short)itemStatus;
            itemLog.ItemLv = itemLv;
            itemLog.Num = num;
            itemLog.CreateDate = DateTime.Now;
            itemLog.UserItemID = useritemID;
            var sender = DataSyncManager.GetDataSender();
            sender.Send(itemLog);
        }

        public static int GetStrongMoney(int itemID, int itemLv)
        {
            int sumNum = 0;
            //(lv3 + bnum ) * incrNum1 * incrNum2
            for (int i = 0; i < _strongCount; i++)
            {
                double num = Math.Pow(MathUtils.Addition(itemLv, 1, int.MaxValue), 3);
                itemLv += 1;
                double baseNum = MathUtils.Addition(num, 80, double.MaxValue);
                double incrNum1 = 0.3;
                double incrNum2 = 10/3;
                double yellowNum = 1.7;
                double redNum = 1.7*1.85;

                ItemBaseInfo _itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
                QualityType quality = _itemInfo.QualityType;
                if (_itemInfo.EquParts == EquParts.WuQi && quality == QualityType.BaiSe)
                {
                    sumNum += (int) (baseNum*incrNum1);
                }
                else if (_itemInfo.EquParts == EquParts.HunQi && quality == QualityType.BaiSe)
                {
                    sumNum += (int) (baseNum*2*incrNum1);
                }
                else if (_itemInfo.EquParts == EquParts.XiongJia && quality == QualityType.BaiSe)
                {
                    sumNum += (int) (baseNum*0.55*incrNum1);
                }
                else if (_itemInfo.EquParts == EquParts.XiangLian && quality == QualityType.BaiSe)
                {
                    sumNum += (int) (baseNum*0.8*incrNum1);
                }
                else if (_itemInfo.EquParts == EquParts.HuJian && quality == QualityType.BaiSe)
                {
                    sumNum += (int) (baseNum*0.45*incrNum1);
                }
                else if (_itemInfo.EquParts == EquParts.XieZi && quality == QualityType.BaiSe)
                {
                    sumNum += (int) (baseNum*0.5*incrNum1);
                } //蓝色装备强化价格
                else if (_itemInfo.EquParts == EquParts.WuQi && quality == QualityType.LanSe)
                {
                    sumNum += (int) (baseNum);
                }
                else if (_itemInfo.EquParts == EquParts.HunQi && quality == QualityType.LanSe)
                {
                    sumNum += (int) (baseNum*2);
                }
                else if (_itemInfo.EquParts == EquParts.XiongJia && quality == QualityType.LanSe)
                {
                    sumNum += (int) (baseNum*0.55);
                }
                else if (_itemInfo.EquParts == EquParts.XiangLian && quality == QualityType.LanSe)
                {
                    sumNum += (int) (baseNum*0.8);
                }
                else if (_itemInfo.EquParts == EquParts.HuJian && quality == QualityType.LanSe)
                {
                    sumNum += (int) (baseNum*0.45);
                }
                else if (_itemInfo.EquParts == EquParts.XieZi && quality == QualityType.LanSe)
                {
                    sumNum += (int) (baseNum*0.5);
                } //紫色装备强化价格
                else if (_itemInfo.EquParts == EquParts.WuQi && quality == QualityType.ZiSe)
                {
                    sumNum += (int) (baseNum*incrNum2);
                }
                else if (_itemInfo.EquParts == EquParts.HunQi && quality == QualityType.ZiSe)
                {
                    sumNum += (int) (baseNum*2*incrNum2);
                }
                else if (_itemInfo.EquParts == EquParts.XiongJia && quality == QualityType.ZiSe)
                {
                    sumNum += (int) (baseNum*0.55*incrNum2);
                }
                else if (_itemInfo.EquParts == EquParts.XiangLian && quality == QualityType.ZiSe)
                {
                    sumNum += (int) (baseNum*0.8*incrNum2);
                }
                else if (_itemInfo.EquParts == EquParts.HuJian && quality == QualityType.ZiSe)
                {
                    sumNum += (int) (baseNum*0.45*incrNum2);
                }
                else if (_itemInfo.EquParts == EquParts.XieZi && quality == QualityType.ZiSe)
                {
                    sumNum += (int) (baseNum*0.5*incrNum2);
                } //红色装备强化价格
                else if (_itemInfo.EquParts == EquParts.WuQi && quality == QualityType.HongSe)
                {
                    sumNum += (int) (baseNum*incrNum2*redNum);
                }
                else if (_itemInfo.EquParts == EquParts.HunQi && quality == QualityType.HongSe)
                {
                    sumNum += (int) (baseNum*2*incrNum2*redNum);
                }
                else if (_itemInfo.EquParts == EquParts.XiongJia && quality == QualityType.HongSe)
                {
                    sumNum += (int) (baseNum*0.55*incrNum2*redNum);
                }
                else if (_itemInfo.EquParts == EquParts.XiangLian && quality == QualityType.HongSe)
                {
                    sumNum += (int) (baseNum*0.8*incrNum2*redNum);
                }
                else if (_itemInfo.EquParts == EquParts.HuJian && quality == QualityType.HongSe)
                {
                    sumNum += (int) (baseNum*0.45*incrNum2*redNum);
                }
                else if (_itemInfo.EquParts == EquParts.XieZi && quality == QualityType.HongSe)
                {
                    sumNum += (int) (baseNum*0.5*incrNum2*redNum);
                } //黄色装备强化价格
                else if (_itemInfo.EquParts == EquParts.WuQi && quality == QualityType.HuangSe)
                {
                    sumNum += (int) (baseNum*incrNum2*yellowNum);
                }
                else if (_itemInfo.EquParts == EquParts.HunQi && quality == QualityType.HuangSe)
                {
                    sumNum += (int) (baseNum*2*incrNum2*yellowNum);
                }
                else if (_itemInfo.EquParts == EquParts.XiongJia && quality == QualityType.HuangSe)
                {
                    sumNum += (int) (baseNum*0.55*incrNum2*yellowNum);
                }
                else if (_itemInfo.EquParts == EquParts.XiangLian && quality == QualityType.HuangSe)
                {
                    sumNum += (int) (baseNum*0.8*incrNum2*yellowNum);
                }
                else if (_itemInfo.EquParts == EquParts.HuJian && quality == QualityType.HuangSe)
                {
                    sumNum += (int) (baseNum*0.45*incrNum2*yellowNum);
                }
                else if (_itemInfo.EquParts == EquParts.XieZi && quality == QualityType.HuangSe)
                {
                    sumNum += (int) (baseNum*0.5*incrNum2*yellowNum);
                }
            }
            if (sumNum.IsValid())
            {
                return sumNum;
            }
            throw new ArgumentOutOfRangeException("强化价格溢出:" + sumNum);
        }

        /// <summary>
        /// 当前物品的数量
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static int UserItemNum(string userID, int itemID)
        {
            int itemNum = 0;
            var package = UserItemPackage.Get(userID);
            if (package != null)
            {
                var useritem = package.ItemPackage.FindAll(s => !s.IsRemove && s.ItemID == itemID);
                foreach (var item in useritem)
                {
                    itemNum += item.Num;
                }
            }
            return itemNum;
        }

        /// <summary>
        /// 以玩家物品ID取出物品ID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userItemID"></param>
        /// <returns></returns>
        public static int GetUserItemInfoID(string userID, string userItemID)
        {
            var package = UserItemPackage.Get(userID);
            if (package != null)
            {
                var useritem = package.ItemPackage.Find(s => !s.IsRemove && s.UserItemID == userItemID);
                if (useritem != null)
                {
                    return useritem.ItemID;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取某种类型背包格子数
        /// </summary>
        /// <param name="abilityId"></param>
        /// <param name="userId"></param>
        public static int GetPackTypePositionNum(BackpackType backpackType, string userId)
        {
            GameDataCacheSet<UserPack> _cacheSetUserPack = new GameDataCacheSet<UserPack>();
            var userPack = _cacheSetUserPack.FindKey(userId);
            var packType = userPack != null && userPack.PackTypeList != null
                              ? userPack.PackTypeList.Find(s => s.BackpackType == backpackType)
                              : null;
            int position = 0;
            if (packType != null)
            {
                position = packType.Position;
            }
            return position;
        }
    }
}
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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 7004_商店物品购买接口
    /// </summary>
    public class Action7004 : BaseAction
    {
        private int _itemId;
        private ShopType _commandType;
        private const int CityId = 0;
        private int _num = 1;

        public Action7004(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action7004, httpGet)
        {
        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ItemID", ref _itemId) &&
                httpGet.GetEnum("MallType", ref _commandType))
            {
                httpGet.GetInt("Num", ref _num, 0, 1000);
                if (_num <= 0)
                    return false;
                return true;
            }
            return false;

        }


        public override bool TakeAction()
        {
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(_itemId);
            //UserItemHelper.AddUserItem(ContextUser.UserID, 1702, 1);
            //UserItemHelper.AddUserItem(ContextUser.UserID, 1701, 1);
            //UserItemHelper.AddUserItem(ContextUser.UserID, 1213, 1);
            if (itemInfo == null)
            {
                return false;
            }
            //判断背包是否已满

            string fullTitle = string.Empty;
            bool isFull = false;
            if (itemInfo.ItemType == ItemType.ZhuangBei)
            {
                isFull = UserPackHelper.PackIsFull(ContextUser, BackpackType.ZhuangBei, _num, out fullTitle);
            }
            else
            {
                isFull = UserPackHelper.PackIsFull(ContextUser, BackpackType.BeiBao, _num, out fullTitle);
            }

            if (isFull)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = fullTitle;
                //ErrorInfo = LanguageManager.GetLang().St7004_BeiBaoTimeOut;
                return false;
            }
            //读取物品信息
            MallItemsInfo mallItemInfo = new ConfigCacheSet<MallItemsInfo>().FindKey(_itemId, CityId, _commandType);
            if (mallItemInfo == null)
            {
                return false;
            }
            //物品价格
            int mallPrice = mallItemInfo.Price;
            if (mallItemInfo.MallType == ShopType.Props || mallItemInfo.MallType == ShopType.PresendBox)
            {
                mallPrice = (FestivalHelper.StoreDiscount() * mallPrice).ToInt();
            }
            else if (mallItemInfo.SpecialPrice > 0)
            {
                mallPrice = mallItemInfo.SpecialPrice;
            }
            //根据物品类型进行扣钱
            if (_commandType == ShopType.Props || _commandType == ShopType.PresendBox)
            {
                int useGold = mallPrice * _num;
                //if (mallItemInfo.MallType == ShopType.PresendBox)
                //{
                if (ContextUser.GoldNum < useGold)
                {
                    ErrorCode = 3;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }

                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold);
                //增加道具日志统计
                var mallItemLog = new MallItemLog
                                      {
                                          Amount = useGold,
                                          CreateDate = DateTime.Now,
                                          ItemID = _itemId,
                                          ItemName = itemInfo.ItemName,
                                          ItemNum = _num,
                                          MobileType = ContextUser.MobileType,
                                          Pid = ContextUser.Pid,
                                          PropType = itemInfo.PropType,
                                          RetailID = ContextUser.RetailID,
                                          Uid = ContextUser.UserID.ToInt()
                                      };

                var sender = DataSyncManager.GetDataSender();
                sender.Send(mallItemLog);
                UserItemHelper.AddUserItem(ContextUser.UserID, _itemId, _num);
                UserLogHelper.AppenUseGoldLog(ContextUser.UserID, 8, 1, 1, useGold, ContextUser.GoldNum, MathUtils.Addition(ContextUser.GoldNum, useGold, int.MaxValue));
                //}
                //预留判断消耗的是金币还是晶石
                //else
                //{
                //    if (ContextUser.GameCoin <useGold)
                //    {
                //        ErrorCode = LanguageManager.GetLang().ErrorCode;
                //        ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                //        return false;
                //    }
                //    ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, useGold);
                //    UserItemHelper.AddUserItem(ContextUser.UserID, _itemId, _num);
                //}

            }
            return true;
        }

        //判断背包是否已满
        public static bool IsBeiBaoFull(GameUser user, int itembaseId)
        {
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itembaseId);
            if (itemInfo != null)
            {
                var package = UserItemPackage.Get(user.UserID);

                //查找出所有已用的格子
                var userItemArray = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus == ItemStatus.BeiBao);

                //查找出已用格子中是相同物品格子的数量
                var itemmountArray = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus == ItemStatus.BeiBao && m.ItemID == itembaseId);

                int totalNum = 0;
                //累加所有的该物品的数量
                foreach (UserItemInfo userItemInfo in itemmountArray)
                {
                    totalNum = MathUtils.Addition(totalNum, userItemInfo.Num);
                }
                //是否有空余位置
                int mount = totalNum % itemInfo.PackMaxNum;

                if (userItemArray.Count >= user.GridNum && mount == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
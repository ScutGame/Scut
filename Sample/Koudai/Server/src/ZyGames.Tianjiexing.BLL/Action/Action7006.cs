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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 7006_背包物品出售接口
    /// </summary>
    public class Action7006 : BaseAction
    {
        private string _userItemId = string.Empty;
        private int _salePrice;
        public Action7006(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action7006, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_salePrice);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserItemID", ref _userItemId))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var package = UserItemPackage.Get(Uid);
            var userItemList = package.ItemPackage.FindAll(m => !m.IsRemove && m.UserItemID.Equals(_userItemId));
            if (userItemList.Count == 0)
            {
                return false;
            }
            //原因：卖出装备，装备上有灵件时提示
            int currNum = ContextUser.SparePartList.FindAll(m => m.UserItemID.Equals(_userItemId)).Count;
            if (currNum > 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St7006_UserItemHaveSpare;
                return false;
            }
            foreach (var userItem in userItemList)
            {
                if (ItemCheck.check(_userItemId))
                {
                    continue;
                }
                if (userItem.ItemStatus == ItemStatus.Sell)
                {
                    package.RemoveItem(userItem);
                    continue;
                }
                if (userItem.ItemStatus != ItemStatus.Sell && userItem.SoldDate > MathUtils.SqlMinDate)
                {
                    userItem.SoldDate = MathUtils.SqlMinDate;
                    //package.Update();
                    continue;
                }
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
                if (itemInfo != null)
                {
                    int salePrice;
                    if (itemInfo.ItemType == ItemType.ZhuangBei)
                    {
                        salePrice = UserHelper.StrongEquPayPrice(Uid, userItem.UserItemID);
                    }
                    else
                    {
                        salePrice = (itemInfo.SalePrice  * userItem.Num);
                    }
                    _salePrice = salePrice;
                    if (userItem.ItemStatus != ItemStatus.Sell)
                    {
                        ItemCheck.add(_userItemId);
                        UserItemHelper.AddItemLog(ContextUser.UserID, userItem.ItemID, userItem.Num, userItem.ItemLv, 3, userItem.UserItemID);
                        //package.RemoveItem(userItem);
                        package.ItemPackage.Remove(userItem);
                        ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, salePrice, int.MaxValue);
                        //ContextUser.Update();
                    }
                }
            }
            return true;
        }
    }
}
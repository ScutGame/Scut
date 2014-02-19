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
using System.Collections.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 7005_神秘商店物品购买接口
    /// </summary>
    public class Action7005 : BaseAction
    {
        private int itemID = 0;
        private int itemNum = 0;

        public Action7005(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action7005, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(itemID);

        }

        public override bool GetUrlElement()
        {

            if (httpGet.GetInt("ItemID", ref itemID)
                 && httpGet.GetInt("ItemNum", ref itemNum))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            var package = UserItemPackage.Get(Uid);
            var userItemArray = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus == ItemStatus.BeiBao);
            if (userItemArray.Count >= ContextUser.GridNum)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St7004_BeiBaoTimeOut;
                return false;
            }

            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            int mallPrice = 0;
            if (itemInfo != null)
            {
                mallPrice = itemInfo.MysteryPrice; //itemInfo.PackMaxNum * itemInfo.SalePrice;
            }
            if (itemInfo != null && mallPrice > ContextUser.GameCoin)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                return false;
            }
            if (ContextUser.UserExtend != null && ContextUser.UserExtend.UserShops != null)
            {
                List<MysteryShops> mysteryList = ContextUser.UserExtend.UserShops.ToList();
                MysteryShops shops = mysteryList.Find(m => m.ItemID == itemID);
                if (shops != null)
                {
                    if (shops.BuyNum > 0)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St7005_HavePurchasedItem;
                        return false;
                    }

                    if (shops.BuyNum > 0)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St7005_HavePurchasedItem;
                        return false;
                    }
                    shops.BuyNum = 1;
                    ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, mallPrice, 0);
                    //ContextUser.Update();

                    UserItemHelper.AddUserItem(ContextUser.UserID, itemID, itemNum);
                }
            }
            return true;
        }
    }
}
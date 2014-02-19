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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1203_佣兵装备更换接口
    /// </summary>
    public class Action1203 : BaseAction
    {
        private int generalID = 0;
        private string userItemID = string.Empty;
        private int ops = 0;

        public Action1203(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1203, httpGet)
        {

        }

        public override bool TakeAction()
        {
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (userGeneral == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            if (ops == 0)
            {
                //用户装备更换
                var package = UserItemPackage.Get(Uid);
                if (package == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
                UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(userItemID));
                if (userItem == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
                if (itemInfo != null)
                {
                    if (string.IsNullOrEmpty(itemInfo.CareerRange) || itemInfo.CareerRange.IndexOf(userGeneral.CareerID.ToString()) == -1)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1203_CareerError;
                        return false;
                    }
                    //佣兵装备部位为空才能更换
                    var generalItem = package.ItemPackage.Find(s => !s.IsRemove && s.Equparts == itemInfo.EquParts && s.GeneralID == userGeneral.GeneralID && s.ItemStatus == ItemStatus.YongBing);
                    if (generalItem != null)
                    {
                        generalItem.GeneralID = 0;
                        generalItem.ItemStatus = ItemStatus.BeiBao;
                        package.SaveItem(generalItem);
                        userItem.GeneralID = generalID;
                        userItem.ItemStatus = ItemStatus.YongBing;
                        package.SaveItem(userItem);
                    }
                    else
                    {
                        userItem.GeneralID = generalID;
                        userItem.ItemStatus = ItemStatus.YongBing;
                        package.SaveItem(userItem);
                    }
                    //int equCount = UserItemHelper.GetItems(Uid).FindAll(
                    //    m => new UserItemHelper(m).EquPartsID == (int)itemInfo.EquParts && m.GeneralID == userGeneral.GeneralID && m.ItemStatus == ItemStatus.YongBing).Count;
                    //if (equCount == 0)
                    //{
                    //    userItem.GeneralID = generalID;
                    //    userItem.ItemStatus = ItemStatus.YongBing;
                    //    package.SaveItem(userItem);
                    //}
                }
            }
            else if (ops == 1)
            {
                //卸下
                //if (UserItemHelper.CheckItemOut(ContextUser, ItemStatus.BeiBao))
                //{
                //    ErrorCode = LanguageManager.GetLang().ErrorCode;
                //    ErrorInfo = LanguageManager.GetLang().St1606_GridNumNotEnough;
                //    return false;
                //}
                string str = string.Empty;
                if (UserPackHelper.PackIsFull(ContextUser, BackpackType.ZhuangBei, 1, out str))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1606_GridNumNotEnough;
                    ErrorInfo = str;
                    return false;
                }
                var package = UserItemPackage.Get(Uid);
                UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(userItemID));

                if (userItem != null && userItem.GeneralID > 0 && userItem.ItemStatus == ItemStatus.YongBing)
                {
                    userItem.GeneralID = 0;
                    userItem.ItemStatus = ItemStatus.BeiBao;
                    package.SaveItem(userItem);
                }

            }
            userGeneral.RefreshMaxLife();
            return true;
        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetString("UserItemID", ref userItemID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }
    }
}
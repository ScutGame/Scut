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
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1259_武器替换附魔符接口
    /// </summary>
    public class Action1259 : BaseAction
    {
        private string userEnchantID;
        private string userItemID;
        private int potion;
        private int ops;


        public Action1259(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1259, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserEnchantID", ref userEnchantID)
                 && httpGet.GetString("UserItemID", ref userItemID)
                 && httpGet.GetInt("Potion", ref potion)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserItemInfo useritem = null;
            var itemPackage = UserItemPackage.Get(ContextUser.UserID);
            var package = UserEnchant.Get(ContextUser.UserID);
            if (itemPackage == null || package == null)
            {
                return false;
            }
            useritem = itemPackage.ItemPackage.Find(m => !m.IsRemove && m.UserItemID == userItemID);
            if (ops == 0)
            {
                int currNum = package.EnchantPackage.FindAll(m => string.IsNullOrEmpty(m.UserItemID)).Count;
                if (currNum >= ContextUser.UserExtend.EnchantGridNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1259_EnchantGridNumFull;
                    return false;
                }
                var uEnchantInfo = package.EnchantPackage.Find(m => m.UserEnchantID == userEnchantID);
                if (uEnchantInfo != null && !string.IsNullOrEmpty(uEnchantInfo.UserItemID))
                {
                    useritem = itemPackage.ItemPackage.Find(m => !m.IsRemove && m.UserItemID == uEnchantInfo.UserItemID);
                    uEnchantInfo.Position = 0;
                    uEnchantInfo.UserItemID = string.Empty;
                    package.SaveEnchant(uEnchantInfo);
                    if (useritem != null && useritem.ItemStatus.Equals(ItemStatus.YongBing))
                    {
                        var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(Uid, useritem.GeneralID);
                        if (userGeneral != null) userGeneral.RefreshMaxLife();
                    }
                }
            }
            else if (ops == 1)
            {
                if (useritem == null || useritem.Equparts != EquParts.WuQi)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1259_UserItemNotWuQi;
                    return false;
                }
                int openGridNum = EnchantHelper.EnchantOpenGridNum(useritem.ItemLv);
                if (potion > openGridNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1213_GridNumNotEnough;
                    return false;
                }
                var userEnchantArray = package.EnchantPackage.FindAll(m => m.UserItemID == userItemID);
                if (userEnchantArray.Count >= openGridNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1213_OpenNumNotEnough;
                    return false;
                }
                foreach (var info in userEnchantArray)
                {
                    if (info.Position == potion)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1259_EnchantOpenGridFull;
                        return false;
                    }
                }
                var uEnchantInfo = package.EnchantPackage.Find(m => m.UserEnchantID == userEnchantID);
                if (uEnchantInfo != null && string.IsNullOrEmpty(uEnchantInfo.UserItemID))
                {
                    uEnchantInfo.Position = (short)potion;
                    uEnchantInfo.UserItemID = userItemID;
                    package.SaveEnchant(uEnchantInfo);
                    var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(Uid, useritem.GeneralID);
                    if (userGeneral != null) userGeneral.RefreshMaxLife();
                }
            }
            return true;
        }
    }
}
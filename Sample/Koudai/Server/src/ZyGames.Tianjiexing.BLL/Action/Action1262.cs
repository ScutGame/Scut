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
using System.Data;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Cache;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1261_可合成附魔符列表接口
    /// </summary>
    public class Action1262 : BaseAction
    {
        private int pageIndex;
        private int pageSize;
        private string userEnchantID;
        private int PageCount;
        private string abilityNum;
        private UserEnchantInfo[] enchantArray = new UserEnchantInfo[0];

        public Action1262(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1262, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(PageCount);
            PushIntoStack(enchantArray.Length);
            foreach (var enchant in enchantArray)
            {
                EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enchant.EnchantID);
                abilityNum = EnchantHelper.EnchantAbilityStr(enchant.EnchantID, enchant.EnchantLv);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(enchant.UserEnchantID.ToNotNullString());
                dsItem.PushIntoStack(enchantInfo == null ? string.Empty : enchantInfo.EnchantName.ToNotNullString());
                dsItem.PushIntoStack(enchantInfo == null ? string.Empty : enchantInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack((short)enchant.EnchantLv);
                dsItem.PushIntoStack(enchantInfo == null ? (short)0 : (short)enchantInfo.ColorType);
                dsItem.PushIntoStack(enchantInfo == null ? (short)0 : (short)enchantInfo.AbilityType);
                dsItem.PushIntoStack(abilityNum.ToNotNullString());
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref pageIndex)
                 && httpGet.GetInt("PageSize", ref pageSize)
                 && httpGet.GetString("UserEnchantID", ref userEnchantID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {

            var package = UserEnchant.Get(ContextUser.UserID);
            if (package == null)
                return false;

            var userEnchantInfo = package.EnchantPackage.Find(m => m.UserEnchantID == userEnchantID);
            if (userEnchantInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1256_EnchantNotEnough;
                return false;
            }
            List<UserEnchantInfo> enchantList = new List<UserEnchantInfo>();
            var enchantInfoArray = package.EnchantPackage.FindAll(m => string.IsNullOrEmpty(m.UserItemID) && m.EnchantLv <= userEnchantInfo.EnchantLv);
            if (enchantInfoArray.Count == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1262_EnchantSynthesisNotEnough;
                return false;
            }
            foreach (var info in enchantInfoArray)
            {
                if (info.UserEnchantID == userEnchantID || info.CurrExprience > userEnchantInfo.CurrExprience)
                {
                    continue;
                }
                enchantList.Add(info);
            }
            enchantArray = enchantList.GetPaging(pageIndex, pageSize, out PageCount).ToArray();
            return true;
        }
    }
}
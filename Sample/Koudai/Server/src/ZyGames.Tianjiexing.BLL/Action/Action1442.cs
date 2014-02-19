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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1442_佣兵升级接口
    /// </summary>
    public class Action1442 : BaseAction
    {
        private int generalID;

        public Action1442(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1442, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var package = UserItemPackage.Get(ContextUser.UserID);
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            GeneralInfo general = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
            if (package == null || general == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            if (userGeneral == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1442_SelectMercenaryUpgrade;
                return false;
            }
            if (string.IsNullOrEmpty(userGeneral.GeneralCard))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1442_SelectTheExperienceCard;
                return false;
            }
            string[] itemStr = userGeneral.GeneralCard.Split(',');
            bool isUse = false;
            foreach (var str in itemStr)
            {
                var useritem = package.ItemPackage.Find(s => !s.IsRemove && s.UserItemID == str);
                if (useritem == null)
                {
                    continue;
                }
                int exprience = 0;
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(useritem.ItemID);
                if (itemInfo != null)
                {
                    exprience = itemInfo.EffectNum;
                    if (general.ItemID == itemInfo.ItemID)
                    {
                        exprience = (exprience * GameConfigSet.ExpMultiple).ToInt();
                    }
                    //GeneralHelper.UserGeneralExp(ContextUser.UserID, generalID, exprience);
                    UserItemHelper.UseUserItem(ContextUser.UserID, useritem.ItemID, 1);
                    isUse = true;
                }
            }
            if (isUse)
            {
                userGeneral.GeneralCard = string.Empty;
            }
            return true;
        }
    }
}
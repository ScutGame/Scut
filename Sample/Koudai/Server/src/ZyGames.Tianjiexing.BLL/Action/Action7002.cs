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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;

using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 7002_神秘商店物品列表接口
    /// </summary>
    public class Action7002 : BaseAction
    {
        private static int intervalDate = (ConfigEnvSet.GetInt("MysteryShops.IntervalDate") * 60 * 60);
        private int codeTime = 0;
        public List<MysteryShops> mysteryShopsList = null;

        public Action7002(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action7002, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(codeTime);
            PushIntoStack(ContextUser.GameCoin);
            PushIntoStack(ContextUser.GoldNum);
            PushIntoStack(mysteryShopsList.Count);
            foreach (MysteryShops shops in mysteryShopsList)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(shops.ItemID);
                int mallPrice = 0;
                if (itemInfo != null)
                {
                    mallPrice = itemInfo.MysteryPrice;
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(shops.ItemID);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.QualityType);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(mallPrice);
                dsItem.PushIntoStack(shops.ItemNum);
                dsItem.PushIntoStack(shops.BuyNum == 0 ? 1 : 2);

                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            if (new GameDataCacheSet<UserFunction>().FindKey(Uid, FunctionEnum.Shengmishangdian) == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_NoFun;
                return false;
            }

            if (ContextUser.UserExtend == null) ContextUser.UserExtend = new GameUserExtend();
            DateTime nextRefreshDate = UserHelper.GetShopsSparRefrshDate(ContextUser);
            if (nextRefreshDate < DateTime.Now)
            {
                nextRefreshDate = UserHelper.GetShopsSparRefrshDate(ContextUser);
                if (nextRefreshDate < DateTime.Now)
                {
                    UserHelper.RefrshShopsSparData(ContextUser, false);
                }
            }
            nextRefreshDate = UserHelper.GetShopsSparRefrshDate(ContextUser);
            codeTime = nextRefreshDate > DateTime.Now ? (int)(nextRefreshDate - DateTime.Now).TotalSeconds : 0;
            mysteryShopsList = ContextUser.UserExtend.UserShops.ToList();

            return true;
        }
    }
}
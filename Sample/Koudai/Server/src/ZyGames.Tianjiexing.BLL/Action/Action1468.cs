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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1468_法宝附加属性升级消耗接口
    /// </summary>
    public class Action1468 : BaseAction
    {
        private AbilityType propertyID;
        private string itemName = "";
        private string successNum = "";
        private TrumpPropertyInfo trumpProperty = null;
        private short isItem;
        private short isCoin;
        private short isObtain;
        private short abilityLv = 0;

        public Action1468(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1468, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(abilityLv);
            PushIntoStack(itemName.ToNotNullString());
            PushIntoStack(trumpProperty == null ? 0 : trumpProperty.ItemNum);
            PushIntoStack(trumpProperty == null ? 0 : trumpProperty.GameCoin);
            PushIntoStack(trumpProperty == null ? 0 : trumpProperty.ObtainNum);
            PushIntoStack(successNum.ToNotNullString());
            PushIntoStack(TrumpHelper.IsMeetUpPropertyLv(ContextUser, trumpProperty) ? (short)1 : (short)0);
            PushIntoStack((short)isItem);
            PushIntoStack((short)isCoin);
            PushIntoStack((short)isObtain);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("PropertyID", ref propertyID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump != null && userTrump.PropertyInfo.Count > 0)
            {
                GeneralProperty property = userTrump.PropertyInfo.Find(m => m.AbilityType == propertyID);
                if (property != null)
                {
                    abilityLv = property.AbilityLv;
                    // short upLv = MathUtils.Addition(property.AbilityLv, (short)1, TrumpPropertyInfo.MaxTrumpPropertyLv);
                    //trumpProperty = new ConfigCacheSet<TrumpPropertyInfo>().FindKey(property.AbilityType, upLv);
                    trumpProperty = new ConfigCacheSet<TrumpPropertyInfo>().FindKey(property.AbilityType, property.AbilityLv);
                    if (trumpProperty != null)
                    {
                        int upItemNum = TrumpHelper.GetUserItemNum(ContextUser.UserID, trumpProperty.ItemID);
                        if (upItemNum >= trumpProperty.ItemNum)
                        {
                            isItem = 1;
                        }
                        if (ContextUser.GameCoin >= trumpProperty.GameCoin)
                        {
                            isCoin = 1;
                        }
                        if (ContextUser.ObtainNum >= trumpProperty.ObtainNum)
                        {
                            isObtain = 1;
                        }
                        successNum = trumpProperty.SuccessNum.ToString();
                        ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(trumpProperty.ItemID);
                        if (itemInfo != null)
                        {
                            itemName = itemInfo.ItemName;
                        }
                    }
                }
            }
            return true;
        }
    }
}
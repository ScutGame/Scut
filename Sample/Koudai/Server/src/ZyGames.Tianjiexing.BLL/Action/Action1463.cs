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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1463_法宝附加属性列表接口
    /// </summary>
    public class Action1463 : BaseAction
    {
        private short worshipLv;
        private string itemName;
        private int totalNum = 0;
        private WorshipInfo worshipInfo = null;
        private short isItem;
        private short isCoin;
        private short isObtain;
        private string successNum;
        private WorshipInfo[] worshipInfoInfoArray = new WorshipInfo[0];

        public Action1463(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1463, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(worshipLv);
            PushIntoStack(itemName.ToNotNullString());
            PushIntoStack(worshipInfo == null ? 0 : worshipInfo.ItemNum);
            PushIntoStack(worshipInfo == null ? 0 : worshipInfo.GameCoin);
            PushIntoStack(worshipInfo == null ? 0 : worshipInfo.ObtainNum);
            PushIntoStack(TrumpHelper.IsUpWorshLv(ContextUser) ? (short)1 : (short)0);
            PushIntoStack(worshipInfoInfoArray.Length);
            int propertycount = 0;
            foreach (var item in worshipInfoInfoArray)
            {
                short isFull = 0;
                GeneralProperty property = GetPropertyType(ContextUser.UserID, propertycount);
                if (property != null && property.AbilityLv >= TrumpPropertyInfo.MaxTrumpPropertyLv)
                {
                    isFull = 1;
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(property == null ? 0 : (int)property.AbilityType);
                dsItem.PushIntoStack(property == null ? (short)0 : (short)property.AbilityLv);
                dsItem.PushIntoStack(totalNum > propertycount ? (short)1 : (short)0);
                dsItem.PushIntoStack(worshipLv >= item.WorshipLv ? (short)1 : (short)0);
                dsItem.PushIntoStack(isFull);
                PushIntoStack(dsItem);
                propertycount++;
            }
            this.PushIntoStack((short)isItem);
            this.PushIntoStack((short)isCoin);
            this.PushIntoStack((short)isObtain);
            PushIntoStack(successNum.ToNotNullString());
        }

        public override bool GetUrlElement()
        {
            if (true)
            {
                return true;
            }
        }

        public override bool TakeAction()
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump != null)
            {
                worshipLv = userTrump.WorshipLv;
                short upWorshLv = MathUtils.Addition(userTrump.WorshipLv, (short)1, (short)10);
                totalNum = userTrump.PropertyInfo.Count;

                worshipInfo = new ConfigCacheSet<WorshipInfo>().FindKey(TrumpInfo.CurrTrumpID, upWorshLv);
                if (worshipInfo != null)
                {
                    successNum = TrumpHelper.GetTransformData(worshipInfo.SuccessNum);
                    int upItemNum = TrumpHelper.GetUserItemNum(ContextUser.UserID, worshipInfo.ItemID);
                    if (upItemNum >= worshipInfo.ItemNum)
                    {
                        isItem = 1;
                    }
                    if (ContextUser.GameCoin >= worshipInfo.GameCoin)
                    {
                        isCoin = 1;
                    }
                    if (ContextUser.ObtainNum >= worshipInfo.ObtainNum)
                    {
                        isObtain = 1;
                    }
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(worshipInfo.ItemID);
                    if (itemInfo != null)
                    {
                        itemName = itemInfo.ItemName;
                    }
                }
            }
            worshipInfoInfoArray = new ConfigCacheSet<WorshipInfo>().FindAll(m => m.IsOpen && m.TrumpID == TrumpInfo.CurrTrumpID).ToArray();
            return true;
        }

        public GeneralProperty GetPropertyType(string userID, int procount)
        {
            GeneralProperty property = null;
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(userID, TrumpInfo.CurrTrumpID);
            if (userTrump != null && userTrump.PropertyInfo.Count > procount)
            {
                property = userTrump.PropertyInfo[procount];
            }
            return property;
        }
    }
}
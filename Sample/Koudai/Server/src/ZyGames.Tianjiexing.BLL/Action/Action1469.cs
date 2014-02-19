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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1469_法宝附加属性升级接口
    /// </summary>
    public class Action1469 : BaseAction
    {
        private AbilityType propertyID;


        public Action1469(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1469, httpGet)
        {

        }

        public override void BuildPacket()
        {

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
                    if (property.AbilityLv >= TrumpPropertyInfo.MaxTrumpPropertyLv)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1466_OutPropertyMaxLv;
                        return false;
                    }
                    TrumpPropertyInfo trumpProperty = new ConfigCacheSet<TrumpPropertyInfo>().FindKey(property.AbilityType, property.AbilityLv);
                    if (trumpProperty != null)
                    {
                        int upItemNum = TrumpHelper.GetUserItemNum(ContextUser.UserID, trumpProperty.ItemID);
                        if (upItemNum < trumpProperty.ItemNum)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1462_ItemNumNotEnough;
                            return false;
                        }
                        if (ContextUser.GameCoin < trumpProperty.GameCoin)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                            return false;
                        }
                        if (ContextUser.ObtainNum < trumpProperty.ObtainNum)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St_ObtainNumNotEnough;
                            return false;
                        }
                        ContextUser.ObtainNum = MathUtils.Subtraction(ContextUser.ObtainNum, trumpProperty.ObtainNum);
                        ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, trumpProperty.GameCoin);
                        UserItemHelper.UseUserItem(ContextUser.UserID, trumpProperty.ItemID, trumpProperty.ItemNum);
                        short upLv = MathUtils.Addition(property.AbilityLv, (short)1, (short)TrumpPropertyInfo.MaxTrumpPropertyLv);
                        TrumpPropertyInfo uptrumpProperty = new ConfigCacheSet<TrumpPropertyInfo>().FindKey(property.AbilityType, upLv);
                        property.UpdateNotify(obj =>
                        {
                            property.AbilityLv = MathUtils.Addition(property.AbilityLv, (short)1, TrumpPropertyInfo.MaxTrumpPropertyLv);
                            property.AbilityValue = uptrumpProperty.PropertyNum;
                            return true;
                        });
                        var usergeneral = UserGeneral.GetMainGeneral(ContextUser.UserID);
                        if (usergeneral != null)
                        {
                            usergeneral.RefreshMaxLife();
                        }
                        ErrorCode = 0;
                        ErrorInfo = LanguageManager.GetLang().St1464_UpgradeWasSsuccessful;

                    }
                }
            }
            return true;
        }
    }
}
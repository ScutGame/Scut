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
using ServiceStack.ServiceInterface.ServiceModel;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1466_法宝附加属性学习接口
    /// </summary>
    public class Action1466 : BaseAction
    {
        private string userItemID;

        public Action1466(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1466, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserItemID", ref userItemID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (!TrumpHelper.IsLearnProperty(ContextUser))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1466_WorshipPropertyNotEnough;
                return false;
            }
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump == null)
            {
                return false;
            }
            UserItemInfo userItem = null;
            var package = UserItemPackage.Get(ContextUser.UserID);
            if (package != null)
            {
                userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID == userItemID);
                if (userItem == null || (userItem.ItemType != ItemType.DaoJu && userItem.PropType == 13))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1466_ItemPropertyNotEnough;
                    return false;
                }
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
                if (itemInfo != null)
                {
                    GeneralProperty property = null;
                    if (userTrump.PropertyInfo.Count > 0)
                    {
                        property = userTrump.PropertyInfo.Find(m => m.AbilityType == itemInfo.AbilityType);
                        if (property != null)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1466_ItemPropertyExite;
                            return false;
                        }
                    }
                    else
                    {
                        userTrump.PropertyInfo.Clear();
                    }
                    short baseLv = 1;
                    TrumpPropertyInfo trumpProperty =
                        new ConfigCacheSet<TrumpPropertyInfo>().FindKey(itemInfo.AbilityType, baseLv);
                    if (trumpProperty != null)
                    {
                        property = new GeneralProperty();
                        property.AbilityType = itemInfo.AbilityType;
                        property.AbilityLv = 1;
                        property.AbilityValue = trumpProperty.PropertyNum;
                        userTrump.PropertyInfo.Add(property);
                        TraceProperty(userTrump);
                        UserItemHelper.UseUserItem(ContextUser.UserID, userItem.ItemID, trumpProperty.ItemNum);

                        var usergeneral = UserGeneral.GetMainGeneral(ContextUser.UserID);
                        if (usergeneral != null)
                        {
                            usergeneral.RefreshMaxLife();
                        }

                        ErrorCode = 0;
                        ErrorInfo = LanguageManager.GetLang().St1466_LearningSuccess;
                    }
                }
            }

            return true;
        }
        private void TraceProperty(UserTrump userTrump)
        {
            var worshipInfoInfoArray = new ConfigCacheSet<WorshipInfo>().FindAll(m => m.IsOpen && m.TrumpID == TrumpInfo.CurrTrumpID).ToArray();
            int index = 0;

            int worshipLv = userTrump.WorshipLv;
            string str = "";
            foreach (var item in worshipInfoInfoArray)
            {
                var generalProperty = GetPropertyType(ContextUser.UserID, index);
                str += string.Format("add propertyinfo userTrump-{0} is {1},worshipLv:{2} \r\n",
                    index,
                    generalProperty == null ? "false" : "true",
                    worshipLv >= item.WorshipLv ? (short)1 : (short)0);

                index++;
            }
            TraceLog.ReleaseWriteFatal(str);
        }

        private GeneralProperty GetPropertyType(string userID, int procount)
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
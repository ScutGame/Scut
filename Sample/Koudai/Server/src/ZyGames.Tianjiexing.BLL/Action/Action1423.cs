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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1423_消除饱食度接口
    /// </summary>
    public class Action1423 : BaseAction
    {
        private int generalID;
        private int ops;


        public Action1423(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1423, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserHelper.ChechDailyRestrain(ContextUser.UserID);
            int maxNum = 0;
            int useNum = 0;
            DailyRestrainSet restrainSet = new ShareCacheStruct<DailyRestrainSet>().FindKey(RestrainType.DragonHolyWater);
            if (restrainSet != null)
            {
                maxNum = restrainSet.MaxNum;
            }

            int _itemID = 5050;
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (general == null || general.SaturationNum == 0)
            {
                return false;
            }

            var package = UserItemPackage.Get(ContextUser.UserID);
            var userItemArray = package.ItemPackage.FindAll(m => m.ItemID.Equals(_itemID));
            UserItemInfo useritem = new UserItemInfo();
            if (userItemArray.Count > 0)
            {
                useritem = userItemArray[0];
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1423_UserItemNotEnough;
                return false;
            }
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(useritem.ItemID);
            if (itemInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1107_UserItemNotEnough;
                return false;
            }
            var cacheSet = new GameDataCacheSet<UserDailyRestrain>();
            UserDailyRestrain userRestrain = cacheSet.FindKey(ContextUser.UserID);
            if (userRestrain != null && userRestrain.UserExtend != null && userRestrain.UserExtend.WaterNum.Count > 0)
            {
                List<DailyRestrain> daliyrestrainList = userRestrain.UserExtend.WaterNum.FindAll(m => m.ID == generalID);
                useNum = daliyrestrainList.Count;
            }
            if (ops == 1)
            {
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1423_ClearCurrSatiation, itemInfo.ItemName);
                return false;
            }
            else if (ops == 2)
            {
                if (useNum >= maxNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1423_DragonHolyWater;
                    return false;
                }
                if (general.SaturationNum > 0)
                {
                    ErrorCode = ops;
                    general.SaturationNum = 0; //general.SaturationNum.Subtraction(itemInfo.SatiationNum, 0);
                    DailyRestrain restrain = new DailyRestrain();
                    restrain.ID = generalID;
                    if (userRestrain != null)
                    {
                        userRestrain.UserExtend.UpdateNotify(obj =>
                        {
                            userRestrain.UserExtend.WaterNum.Add(restrain);
                            return true;
                        });
                    }
                    UserItemHelper.UseUserItem(ContextUser.UserID, itemInfo.ItemID, 1);
                }
            }
            return true;
        }
    }
}
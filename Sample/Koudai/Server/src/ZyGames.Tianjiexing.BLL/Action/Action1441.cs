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
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using System.Data;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1441_佣兵升级界面接口
    /// </summary>
    public class Action1441 : BaseAction
    {
        private int generalID;
        private short nextLv;
        private short isUp;
        private string[] strUserItemID = new string[0];

        private UserGeneral userGeneral = null;

        public Action1441(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1441, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(strUserItemID.Length);
            foreach (var str in strUserItemID)
            {
                ItemBaseInfo itemInfo = string.IsNullOrEmpty(str) ? null : GetItemBaseInfo(ContextUser.UserID, str);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(str.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? 0 : itemInfo.ItemID);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                PushIntoStack(dsItem);
            }
            this.PushIntoStack(userGeneral == null ? string.Empty : userGeneral.GeneralName.ToNotNullString());
            this.PushIntoStack(userGeneral == null ? (short)0 : userGeneral.GeneralLv);
            this.PushIntoStack((short)nextLv);
            this.PushIntoStack(userGeneral == null ? string.Empty : userGeneral.PicturesID.ToNotNullString());
            this.PushIntoStack(userGeneral == null ? 0 : userGeneral.LifeNum);
            this.PushIntoStack(userGeneral == null ? 0 : userGeneral.CurrExperience);
            this.PushIntoStack(userGeneral == null ? 0 : userGeneral.AbilityID);
            this.PushIntoStack((short)isUp);

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
            var userGeneralList = new GameDataCacheSet<UserGeneral>().FindAll(ContextUser.UserID,
                                                                              s =>
                                                                              s.GeneralID != generalID &&
                                                                              s.GeneralType == GeneralType.YongBing && s.GeneralType != GeneralType.Soul);
            foreach (var general in userGeneralList)
            {
                general.GeneralCard = string.Empty;
            }
            userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (userGeneral != null)
            {
                nextLv = MathUtils.Addition(userGeneral.GeneralLv, (short)1);
                if (!string.IsNullOrEmpty(userGeneral.GeneralCard))
                {
                    strUserItemID = userGeneral.GeneralCard.TrimEnd(',').Split(',');
                }
            }
            var generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
            if (generalInfo != null)
            {
                isUp = 1;
            }
            return true;
        }

        /// <summary>
        /// 佣兵卡片物品
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userItemID"></param>
        /// <returns></returns>
        public static ItemBaseInfo GetItemBaseInfo(string userID, string userItemID)
        {
            var package = UserItemPackage.Get(userID);
            if (package != null)
            {
                var useritem = package.ItemPackage.Find(s => !s.IsRemove && s.UserItemID == userItemID);
                if (useritem != null)
                {
                    return new ConfigCacheSet<ItemBaseInfo>().FindKey(useritem.ItemID);
                }
            }
            return null;
        }
    }
}
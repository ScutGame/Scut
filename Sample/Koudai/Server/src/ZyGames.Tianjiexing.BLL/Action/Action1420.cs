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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using System.Collections.Generic;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Enum;
using System;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1420_礼物界面接口
    /// </summary>
    public class Action1420 : BaseAction
    {
        private int generalID;
        private int upExperience;
        private string giftName;
        private short goldNum;
        private string skillName;
        private short demandLv;
        private short isLearn;
        private string skillDesc;
        private short isSaturation;
        public UserGeneral userGeneral = null;
        public GeneralInfo generalInfo = null;
        public List<GeneralProperty> propertyList = new List<GeneralProperty>();


        public Action1420(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1420, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(userGeneral == null ? string.Empty : userGeneral.GeneralName.ToNotNullString());
            this.PushIntoStack(userGeneral == null ? string.Empty : userGeneral.HeadID.ToNotNullString());
            this.PushIntoStack(userGeneral == null ? (short)0 : userGeneral.FeelLv);
            this.PushIntoStack(userGeneral == null ? 0 : userGeneral.FeelExperience);
            this.PushIntoStack(upExperience);
            this.PushIntoStack(giftName.ToNotNullString());
            this.PushIntoStack(userGeneral == null ? (short)0 : userGeneral.SaturationNum);
            this.PushIntoStack(goldNum);
            this.PushIntoStack(skillName.ToNotNullString());
            this.PushIntoStack(demandLv);
            this.PushIntoStack(isLearn);
            this.PushIntoStack(skillDesc.ToNotNullString());
            this.PushIntoStack(isSaturation);
            this.PushIntoStack(propertyList.Count);
            foreach (var item in propertyList)
            {
                int upNum = GiftHelper.FeelUpPropertyNum(ContextUser.UserID, generalID, item.AbilityType);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((int)item.AbilityType);
                dsItem.PushIntoStack((int)item.AbilityValue);
                dsItem.PushIntoStack(upNum);
                this.PushIntoStack(dsItem);
            }
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
            UserHelper.GetFeelHunger(ContextUser.UserID, generalID);
            UserHelper.ChechDailyRestrain(ContextUser.UserID);
            generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
            if (generalInfo != null)
            {
                giftName = GiftHelper.GetGiftTypeName(generalInfo.GiftType);
                if (generalInfo.ReplaceSkills != null)
                {
                    demandLv = generalInfo.ReplaceSkills.FeelLv;
                    AbilityInfo abilityInfo =
                        new ConfigCacheSet<AbilityInfo>().FindKey(generalInfo.ReplaceSkills.AbilityID);
                    if (abilityInfo != null)
                    {
                        skillName = abilityInfo.AbilityName;
                        skillDesc = abilityInfo.AbilityDesc;
                    }
                }
            }
            userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (userGeneral != null)
            {
                FeelLvInfo feelLvInfo = new ConfigCacheSet<FeelLvInfo>().FindKey(userGeneral.FeelLv);
                if (feelLvInfo != null)
                {
                    propertyList = feelLvInfo.Property.ToList();
                }
                short feelLv = MathUtils.Addition(userGeneral.FeelLv, (short)1, GiftHelper.maxFeelLv);
                FeelLvInfo upfeelLvInfo = new ConfigCacheSet<FeelLvInfo>().FindKey(feelLv);
                if (upfeelLvInfo != null)
                {
                    upExperience = upfeelLvInfo.Experience;
                }
                if (demandLv > 0 && userGeneral.FeelLv >= demandLv)
                {
                    isLearn = 1;
                }
                else
                {
                    isLearn = 0;
                }
            }

            goldNum = GiftHelper.SurplusGoldNum(ContextUser.UserID);
            int _itemid = 5050; //消除饱食度物品ID
            var package = UserItemPackage.Get(ContextUser.UserID);
            var itemArray =
                package.ItemPackage.FindAll(
                    m => m.ItemStatus.Equals(ItemStatus.BeiBao) && m.ItemID.Equals(_itemid));
            if (itemArray.Count > 0)
            {
                isSaturation = 1;
            }
            else
            {
                isSaturation = 0;
            }

            return true;
        }
    }
}
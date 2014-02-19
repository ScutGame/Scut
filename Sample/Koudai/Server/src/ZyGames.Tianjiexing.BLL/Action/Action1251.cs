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
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1251_装备附魔界面接口
    /// </summary>
    public class Action1251 : BaseAction
    {
        private int pageIndex;
        private int pageSize;
        private int pageCount;
        private string toUserID;
        private int enchantPackNum;
        private int moShiNum;
        private int goldNum;
        private short openNum;
        private UserGeneral[] generalArray = new UserGeneral[0];
        private UserEnchantInfo[] enchantPackageArray = new UserEnchantInfo[0];
        private UserEnchant package = null;
        private MosaicInfo[] mosaicList = new MosaicInfo[0];

        public Action1251(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1251, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(enchantPackNum);
            PushIntoStack(moShiNum);
            PushIntoStack(goldNum);
            PushIntoStack(generalArray.Length);
            foreach (var general in generalArray)
            {
                UserEnchantInfo[] enchantGeneralArray = new UserEnchantInfo[0];
                UserItemInfo useritem = EnchantHelper.GetGeneralWeapon(general.UserID, general.GeneralID);
                ItemBaseInfo itemInfo = null;
                if (useritem != null)
                {
                    itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(useritem.ItemID);
                    openNum = EnchantHelper.EnchantOpenGridNum(useritem.ItemLv);
                    if (package != null)
                    {
                        enchantGeneralArray = package.EnchantPackage.FindAll(m => m.UserItemID == useritem.UserItemID).ToArray();
                    }
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(general.GeneralID);
                dsItem.PushIntoStack(general.GeneralName.ToNotNullString());
                dsItem.PushIntoStack((short)general.CareerID);
                dsItem.PushIntoStack(useritem == null ? string.Empty : useritem.UserItemID.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack((short)openNum);
                dsItem.PushIntoStack(enchantGeneralArray.Length);
                foreach (var enchant in enchantGeneralArray)
                {
                    EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enchant.EnchantID);
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack((short)enchant.Position);
                    dsItem1.PushIntoStack(enchant.UserEnchantID.ToNotNullString());
                    dsItem1.PushIntoStack(enchantInfo == null ? string.Empty : enchantInfo.EnchantName.ToNotNullString());
                    dsItem1.PushIntoStack(enchantInfo == null ? string.Empty : enchantInfo.HeadID.ToNotNullString());
                    dsItem1.PushIntoStack((short)enchant.EnchantLv);
                    dsItem1.PushIntoStack(enchantInfo == null ? (short)0 : (short)enchantInfo.ColorType);
                    dsItem.PushIntoStack(dsItem1);
                }

                PushIntoStack(dsItem);
            }
            this.PushIntoStack(pageCount);
            this.PushIntoStack(enchantPackageArray.Length);
            foreach (var enchant in enchantPackageArray)
            {
                EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enchant.EnchantID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(enchant.UserEnchantID.ToNotNullString());
                dsItem.PushIntoStack(enchantInfo == null ? string.Empty : enchantInfo.EnchantName.ToNotNullString());
                dsItem.PushIntoStack(enchantInfo == null ? string.Empty : enchantInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack((short)enchant.EnchantLv);
                dsItem.PushIntoStack(enchantInfo == null ? (short)0 : (short)enchantInfo.ColorType);
                PushIntoStack(dsItem);
            }
            this.PushIntoStack(mosaicList.Length);
            foreach (var mosaic in mosaicList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((short)mosaic.Position);
                dsItem.PushIntoStack(mosaic.MosaicColor.ToNotNullString());
                dsItem.PushIntoStack((short)mosaic.DemandLv);

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref pageIndex)
                 && httpGet.GetInt("PageSize", ref pageSize))
            {
                httpGet.GetString("ToUserID", ref toUserID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            GameUser user = null;
            string puUserID = string.Empty;
            if (!string.IsNullOrEmpty(toUserID))
            {
                puUserID = toUserID;
                user = UserCacheGlobal.LoadOffline(toUserID);
            }
            else
            {
                puUserID = ContextUser.UserID;
                user = new GameDataCacheSet<GameUser>().FindKey(ContextUser.UserID);
                UserFunction userFunction = new GameDataCacheSet<UserFunction>().FindKey(ContextUser.UserID,
                                                                                         FunctionEnum.Enchant);
                if (userFunction != null)
                {
                    EnchantHelper.EnchantFunctionOpen(ContextUser);
                }
                else
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_NoFun;
                    return false;
                }
            }
            generalArray = new GameDataCacheSet<UserGeneral>().FindAll(puUserID, m => m.GeneralStatus == GeneralStatus.DuiWuZhong && m.GeneralType != GeneralType.Soul).ToArray();
            if (user != null)
            {
                goldNum = user.GoldNum;
                if (user.UserExtend != null)
                {
                    enchantPackNum = user.UserExtend.EnchantGridNum;
                    moShiNum = user.UserExtend.MoJingNum;
                }
            }
            package = UserEnchant.Get(puUserID);
            if (package != null && string.IsNullOrEmpty(toUserID))
            {
                var enchantsArray = package.EnchantPackage.FindAll(m => string.IsNullOrEmpty(m.UserItemID));
                enchantPackageArray = enchantsArray.GetPaging(pageIndex, pageSize, out pageCount).ToArray();
            }
            mosaicList = new ConfigCacheSet<MosaicInfo>().FindAll().ToArray();
            return true;
        }
    }
}
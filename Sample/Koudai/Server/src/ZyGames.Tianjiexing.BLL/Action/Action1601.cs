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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1601_装备、丹药合成材料接口
    /// </summary>
    public class Action1601 : BaseAction
    {
        private int itemID = 0;
        private string userItemID;
        private int synthesisMinLv = 0;
        private ItemBaseInfo itemInfo = null;
        private List<ItemSynthesisInfo> synthesisArray = new List<ItemSynthesisInfo>();
        private List<ItemEquAttrInfo> equAttrInfo = new List<ItemEquAttrInfo>();

        public Action1601(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1601, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(itemInfo == null ? 0 : itemInfo.ItemID);
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
            PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.QualityType);
            PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : itemInfo.DemandLv);
            PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : itemInfo.MedicineLv);
            PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : itemInfo.MedicineType);
            PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : itemInfo.MedicineNum);

            PushIntoStack(equAttrInfo.Count);
            foreach (ItemEquAttrInfo equ in equAttrInfo)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(equ.AttributeID.ToShort());
                dsItem.PushIntoStack(equ.BaseNum);

                PushIntoStack(dsItem);
            }

            PushIntoStack(synthesisArray.Count);
            foreach (ItemSynthesisInfo synthesis in synthesisArray)
            {
                ItemBaseInfo item = new ConfigCacheSet<ItemBaseInfo>().FindKey(synthesis.SynthesisID);
                var userItemArray = UserItemHelper.GetItems(Uid).FindAll(u => u.ItemID.Equals(synthesis.SynthesisID) && u.ItemStatus != ItemStatus.Sell && new UserItemHelper(u).GeneralStatus(Uid) != GeneralStatus.LiDui);
                int maxNum = 0;
                int sumNum = 0;
                foreach (var userItem in userItemArray)
                {
                    sumNum += userItem.Num;
                }
                if (sumNum >= synthesis.DemandNum)
                {
                    maxNum = synthesis.DemandNum;
                }
                else
                {
                    maxNum = sumNum;
                }
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(synthesis.SynthesisID);
                ds.PushIntoStack(item == null ? string.Empty : item.ItemName.ToNotNullString());
                ds.PushIntoStack(item == null ? string.Empty : item.HeadID.ToNotNullString());
                ds.PushIntoStack(maxNum);
                ds.PushIntoStack(synthesis.DemandNum);
                ds.PushIntoStack(0);
                this.PushIntoStack(ds);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ItemID", ref itemID))
            {
                httpGet.GetString("UserItemID", ref userItemID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int uItemID = 0;
            if (itemID == 0 && userItemID != "")
            {
                var package = UserItemPackage.Get(Uid);
                UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(userItemID));
                if (userItem != null)
                {
                    uItemID = userItem.ItemID;
                }
            }
            else
            {
                uItemID = itemID;
            }
            List<ItemSynthesisInfo> itemSynthesisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(u => u.SynthesisID.Equals(uItemID));
            if (itemSynthesisArray.Count > 0)
            {
                itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemSynthesisArray[0].ItemID); //合成物品的信息
                //synthesisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(u => u.ItemID.Equals(itemInfo.ItemID) && u.SynthesisID != itemID);
                synthesisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(u => u.ItemID.Equals(itemInfo.ItemID));
                equAttrInfo = new ConfigCacheSet<ItemEquAttrInfo>().FindAll(u => u.ItemID.Equals(itemInfo.ItemID));
            }

            synthesisMinLv = ConfigEnvSet.GetInt("ItemInfo.SynthesisMinLv");
            return true;
        }
    }
}
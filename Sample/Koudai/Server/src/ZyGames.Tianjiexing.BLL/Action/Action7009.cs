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
using ZyGames.Tianjiexing.Model;

using ZyGames.Tianjiexing.Model.Enum;

using ZyGames.Tianjiexing.Lang;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 7009_物品详情接口
    /// </summary>
    public class Action7009 : BaseAction
    {
        private int itemID = 0;
        private ShopType mallType = 0;
        private ItemBaseInfo itemInfo = null;
        private List<ItemEquAttrInfo> itemEquArray = new List<ItemEquAttrInfo>();
        private List<ItemSynthesisInfo> itemSynthsisArray = new List<ItemSynthesisInfo>();
        private Dictionary<int, int> userItemDict = new Dictionary<int, int>();
        private short curLevel = 0;
        private int price = 0;
        private string qiShiName = string.Empty;
        private int cityID = 0;

        public Action7009(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action7009, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
            PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.ItemType);
            PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.QualityType);
            PushIntoStack(curLevel);
            PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.DemandLv);
            PushIntoStack(itemEquArray.Count);
            foreach (ItemEquAttrInfo equ in itemEquArray)
            {
                int StrengNum = MathUtils.Addition(equ.BaseNum, (equ.IncreaseNum * curLevel), int.MaxValue);
                DataStruct ds = new DataStruct();
                ds.PushIntoStack((int)equ.AttributeID);
                ds.PushIntoStack(StrengNum);
                PushIntoStack(ds);
            }
            string[] careerList = new string[0];
            if (!string.IsNullOrEmpty(itemInfo.CareerRange))
            {
                careerList = itemInfo.CareerRange.Split(',');
            }
            PushIntoStack(careerList.Length);
            foreach (string career in careerList)
            {
                DataStruct ds = new DataStruct();
                CareerInfo careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(career);
                ds.PushIntoStack(careerInfo == null ? 0 : (int)careerInfo.CareerID);
                ds.PushIntoStack(careerInfo == null ? string.Empty : careerInfo.CareerName.ToNotNullString());
                PushIntoStack(ds);
            }
            PushIntoStack(qiShiName.ToNotNullString());
            PushIntoStack(price);
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemDesc.ToNotNullString());
            PushIntoStack(itemSynthsisArray.Count);
            foreach (ItemSynthesisInfo synthesis in itemSynthsisArray)
            {
                int totalNum = userItemDict.ContainsKey(synthesis.SynthesisID) ? userItemDict[synthesis.SynthesisID] : 0;
                ItemBaseInfo tempItem = new ConfigCacheSet<ItemBaseInfo>().FindKey(synthesis.SynthesisID);

                DataStruct ds = new DataStruct();
                ds.PushIntoStack(synthesis.SynthesisID);
                ds.PushIntoStack(tempItem != null ? tempItem.ItemName.ToNotNullString() : string.Empty);
                ds.PushIntoStack((short)synthesis.DemandNum);
                ds.PushIntoStack((short)totalNum);

                PushIntoStack(ds);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ItemID", ref itemID)
                 && httpGet.GetEnum("MallType", ref mallType)
                 && httpGet.GetInt("CityID", ref cityID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            curLevel = 1;
            itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            if (itemInfo == null)
            {
                return false;
            }

            itemEquArray = new ConfigCacheSet<ItemEquAttrInfo>().FindAll(m => m.ItemID ==  itemID);

            if (mallType == ShopType.Blackmarket)
            {
                price = itemInfo.SalePrice;
            }
            else
            {
                MallItemsInfo mallitemsInfo = new ConfigCacheSet<MallItemsInfo>().FindKey(itemID, cityID, mallType);
                if (mallitemsInfo != null)
                {
                    ItemBaseInfo qishiItemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(mallitemsInfo.QishiID);
                    if (qishiItemInfo != null)
                    {
                        qiShiName = qishiItemInfo.ItemName;
                    }
                    price = mallitemsInfo.Price;
                }
            }
            if (itemInfo.ItemType == ItemType.TuZhi || itemInfo.ItemType == ItemType.ZhuangBei || itemInfo.ItemType == ItemType.TuZhi)
            {
                List<ItemSynthesisInfo> synthsisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(s => s.SynthesisID.Equals(itemID));
                if (synthsisArray.Count > 0)
                {
                    itemSynthsisArray =
                        new ConfigCacheSet<ItemSynthesisInfo>().FindAll(
                            s => s.ItemID.Equals(synthsisArray[0].ItemID) && s.SynthesisID != itemID);
                }
            }
            return true;
        }
    }
}
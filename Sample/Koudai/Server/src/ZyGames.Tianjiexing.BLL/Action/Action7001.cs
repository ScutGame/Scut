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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 7001_商店物品列表接口
    /// </summary>
    public class Action7001 : BaseAction
    {
        private ShopType _commandType;
        private int _pageIndex;
        private int _pageSize;
        private int _pageCount;
        private List<MallItemsInfo> _mallItemsInfoArray = new List<MallItemsInfo>();

        public Action7001(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action7001, httpGet)
        {

        }
        public override void BuildPacket()
        {
            PushIntoStack(_pageCount);
            PushIntoStack(ContextUser.GameCoin);
            PushIntoStack(ContextUser.GoldNum);
            PushIntoStack(_mallItemsInfoArray.Count);
            foreach (MallItemsInfo mallItems in _mallItemsInfoArray)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(mallItems.ItemID);
                int mallPrice = 0;
                int specilPrice = 0;
                if (itemInfo != null)
                {
                    mallPrice = mallItems.Price;
                    if (mallItems.MallType == ShopType.Props || mallItems.MallType == ShopType.PresendBox)
                    {
                        mallPrice = (FestivalHelper.StoreDiscount() * mallPrice).ToInt();
                    }
                    specilPrice = mallItems.SpecialPrice;
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(mallItems.ItemID);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.MaxHeadID.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemDesc.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.QualityType);
                dsItem.PushIntoStack(mallPrice);
                dsItem.PushIntoStack(specilPrice);
                dsItem.PushIntoStack(mallItems.SeqNO);
                dsItem.PushIntoStack((short)_commandType);
                PushIntoStack(dsItem);
            }
            PushIntoStack(ContextUser.ObtainNum);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("MallType", ref _commandType)
                 && httpGet.GetInt("PageIndex", ref _pageIndex)
                 && httpGet.GetInt("PageSize", ref _pageSize))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            _mallItemsInfoArray = new ConfigCacheSet<MallItemsInfo>().FindAll(m => m.MallType == _commandType);
            _mallItemsInfoArray.GetPaging(_pageIndex, _pageSize, out _pageCount);
            return true;
        }
    }
}
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
using System.Collections.Generic;
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1211_装备灵件背包列表接口
    /// </summary>
    public class Action1211 : BaseAction
    {
        private int _lingshiNum;
        private int _pageIndex;
        private int _pageSize;
        private int _pageCount;
        private short _gridNum;
        private List<UserSparePart> _sparePartList = new List<UserSparePart>();
        private string toUserID;

        public Action1211(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1211, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_pageCount);
            this.PushIntoStack((int)_gridNum);
            this.PushIntoStack((int)new GameUser().SparePartMaxGridNum);
            this.PushIntoStack(_lingshiNum);
            this.PushIntoStack(_sparePartList.Count);
            foreach (var sparePart in _sparePartList)
            {
                var sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(sparePart.SparePartId) ?? new SparePartInfo();

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(sparePart.UserSparepartID.ToNotNullString());
                dsItem.PushIntoStack(sparePartInfo.Name.ToNotNullString());
                dsItem.PushIntoStack(sparePartInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(sparePartInfo.QualityType.ToShort());
                dsItem.PushIntoStack(sparePartInfo.CoinPrice);
                dsItem.PushIntoStack(sparePartInfo.LingshiPrice);


                dsItem.PushIntoStack(sparePart.Propertys.Count);
                for (int i = 0; i < sparePart.Propertys.Count; i++)
                {
                    var property = sparePart.Propertys[i];
                    short proPos = MathUtils.Addition(property.ValueIndex, (short)1, short.MaxValue);
                    short isEnable = 0;
                    if (property.IsEnable) isEnable = 1;
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(property.AbilityType.ToShort());
                    dsItem1.PushIntoStack(property.Num.ToNotNullString());
                    dsItem1.PushIntoStack(property.HitMinValue.ToNotNullString());
                    dsItem1.PushIntoStack(property.HitMaxValue.ToNotNullString());
                    dsItem1.PushIntoStack(isEnable);
                    dsItem1.PushIntoStack(proPos);

                    dsItem.PushIntoStack(dsItem1);
                }
                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref _pageIndex)
                 && httpGet.GetInt("PageSize", ref _pageSize))
            {
                httpGet.GetString("ToUserID", ref toUserID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            string publicUserID = string.Empty;
            if (string.IsNullOrEmpty(toUserID))
            {
                publicUserID = ContextUser.UserID;
            }
            else
            {
                publicUserID = toUserID;
                UserCacheGlobal.LoadOffline(publicUserID);
            }
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(publicUserID);
            if (user == null)
            {
                return false;
            }
            if (user.UserExtend == null)
            {
                user.UserExtend = new GameUserExtend();
            }
            _lingshiNum = user.UserExtend.LingshiNum;
            _gridNum = user.UserExtend.SparePartGridNum;
            if (_gridNum == 0)
            {
                _gridNum = new GameUser().SparePartMinGridNum;
                user.UserExtend.UpdateNotify(obj =>
                {
                    user.UserExtend.SparePartGridNum = _gridNum;
                    return true;
                 });
            }
            if (string.IsNullOrEmpty(toUserID))
            {
                List<UserSparePart> tempList = user.SparePartList.FindAll(m => string.IsNullOrEmpty(m.UserItemID));
                _sparePartList = tempList.GetPaging(_pageIndex, _pageSize, out _pageCount);
            }
            return true;
        }
    }
}
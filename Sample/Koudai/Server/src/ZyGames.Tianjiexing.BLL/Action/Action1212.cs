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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1212_装备镶嵌灵件列表接口
    /// </summary>
    public class Action1212 : BaseAction
    {
        private int _equPart;
        private int _genealID;
        private string toUserID;
        private UserSparePart[] _sparePartList = new UserSparePart[UserSparePart.PartMaxGrid];
        private GameUser user = null;

        public Action1212(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1212, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_sparePartList.Length);
            for (int i = 0; i < _sparePartList.Length; i++)
            {
                var sparePart = _sparePartList[i] ?? new UserSparePart();
                if (sparePart.Position == 0) sparePart.SetPosition((short)(i + 1));
                short enableStatus = 0;
                if (sparePart.CheckEnable(user.UserExtend.MaxLayerNum)) enableStatus = 1;
                var sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(sparePart.SparePartId) ?? new SparePartInfo();

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(sparePart.Position);
                dsItem.PushIntoStack(enableStatus);
                dsItem.PushIntoStack(sparePart.UserSparepartID.ToNotNullString());
                dsItem.PushIntoStack(sparePartInfo.Name.ToNotNullString());
                dsItem.PushIntoStack(sparePartInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(sparePartInfo.QualityType.ToShort());
                dsItem.PushIntoStack(sparePartInfo.CoinPrice);
                dsItem.PushIntoStack(sparePartInfo.LingshiPrice);


                dsItem.PushIntoStack(sparePart.Propertys.Count);
                for (int r = 0; r < sparePart.Propertys.Count; r++)
                {
                    var property = sparePart.Propertys[r];
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
            if (httpGet.GetInt("EquPart", ref _equPart)
                 && httpGet.GetInt("GenealID", ref _genealID))
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
            user = new GameDataCacheSet<GameUser>().FindKey(publicUserID);
            if (user == null)
            {
                return false;
            }
            if (user.UserExtend == null)
            {
                user.UserExtend = new GameUserExtend();
            }
            var userItemArray = UserItemHelper.GetItems(publicUserID).FindAll(
                u => u.GeneralID.Equals(_genealID) && new UserItemHelper(u).EquPartsID.Equals(_equPart) && u.ItemStatus == ItemStatus.YongBing);

            if (userItemArray.Count > 0)
            {
                string itemID = userItemArray[0].UserItemID ?? "";
                var tempPartList = user.SparePartList.FindAll(m => m.UserItemID.Equals(itemID));
                for (int i = 0; i < tempPartList.Count; i++)
                {
                    var part = tempPartList[i];
                    if (part != null && part.Position > 0 && part.Position - 1 < _sparePartList.Length)
                    {
                        part.UpdateNotify(obj =>
                        {
                            _sparePartList[part.Position - 1] = part;
                            return true;
                        });
                       //_sparePartList[part.Position - 1] = part;
                    }
                    else if (part != null && part.Position == 0 && !string.IsNullOrEmpty(part.UserItemID))
                    {
                        //修正灵石在装备上位置为同一的
                        part.UpdateNotify(obj =>
                        {
                            part.UserItemID = string.Empty;
                            return true;
                        });
                    }
                }
            }
            return true;
        }
    }
}
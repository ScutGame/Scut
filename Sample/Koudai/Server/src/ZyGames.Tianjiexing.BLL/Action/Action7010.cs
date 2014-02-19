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

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 7010_物品配置信息下发接口
    /// </summary>
    public class Action7010 : BaseAction
    {
        private int ClientVersion;
        private List<ItemBaseInfo> itemBaseList = new List<ItemBaseInfo>();

        public Action7010(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action7010, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(itemBaseList.Count);
            foreach (ItemBaseInfo itemInfo in itemBaseList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(itemInfo.ItemID);
                dsItem.PushIntoStack(itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(itemInfo.ItemDesc.ToNotNullString());
                dsItem.PushIntoStack(itemInfo.ItemType.ToShort());
                dsItem.PushIntoStack(itemInfo.EquParts.ToShort());
                dsItem.PushIntoStack(itemInfo.QualityType.ToShort());
                dsItem.PushIntoStack(itemInfo.DemandLv.ToShort());

                string[] careerList = itemInfo.CareerRange.Split(new char[] { ',' });
                dsItem.PushIntoStack(careerList.Length);
                foreach (string CareerID in careerList)
                {
                    CareerInfo careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(CareerID.ToInt());
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(careerInfo != null ? careerInfo.CareerID.ToInt() : (int)0);
                    dsItem1.PushIntoStack(careerInfo != null ? careerInfo.CareerName.ToNotNullString() : string.Empty);

                    dsItem.PushIntoStack(dsItem1);
                }

                var equList = new ConfigCacheSet<ItemEquAttrInfo>().FindAll(m => m.ItemID == itemInfo.ItemID);
                dsItem.PushIntoStack(equList.Count);
                foreach (ItemEquAttrInfo equ in equList)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(equ.AttributeID.ToInt());
                    dsItem1.PushIntoStack(equ.BaseNum);
                    dsItem1.PushIntoStack(equ.IncreaseNum);

                    dsItem.PushIntoStack(dsItem1);
                }

                dsItem.PushIntoStack(itemInfo.MedicineType);
                dsItem.PushIntoStack(itemInfo.MedicineLv);
                dsItem.PushIntoStack(itemInfo.MedicineNum);
                dsItem.PushIntoStack(itemInfo.PropType);
                dsItem.PushIntoStack(itemInfo.EffectNum.ToNotNullString());

                var itemSynthesisList = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(m => m.ItemID == itemInfo.ItemID);
                dsItem.PushIntoStack(itemSynthesisList.Count);
                foreach (ItemSynthesisInfo synthesisInfo in itemSynthesisList)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(synthesisInfo.SynthesisID);
                    dsItem1.PushIntoStack(synthesisInfo.DemandNum);
                    dsItem1.PushIntoStack(synthesisInfo.SynthesisNum);

                    dsItem.PushIntoStack(dsItem1);
                }

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ClientVersion", ref ClientVersion))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int currVersion = new ConfigCacheSet<ConfigVersion>().FindKey(VersionType.Item).CurVersion;
            itemBaseList = new ConfigCacheSet<ItemBaseInfo>().FindAll(m => m.Version > ClientVersion && m.Version <= currVersion);
            return true;
        }
    }
}
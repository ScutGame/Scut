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

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1414_Vip用户佣兵可服用丹药列表接口
    /// </summary>
    public class Action1414 : BaseAction
    {
        private int medicineType = 0;
        private string[] medicineArray = new string[0];

        public Action1414(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1414, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(medicineArray.Length);
            foreach (string item in medicineArray)
            {
                short mType = (short)medicineType;
                string[] mArray = item.Replace("{", "").Replace("}", "").Split('=');
                short medicineLv = 0;
                int useGold = 0;
                List<ItemBaseInfo> itemInfoArray = new List<ItemBaseInfo>();
                if (mArray.Length > 0)
                {
                    string q = mArray[0];
                    string b = mArray[1];
                    medicineLv = q.ToShort();
                    useGold = b.ToShort();
                    itemInfoArray = new ConfigCacheSet<ItemBaseInfo>().FindAll(u => u.MedicineLv.Equals(medicineLv) && u.MedicineType.Equals(mType) && u.ItemType == ItemType.YaoJi);
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(itemInfoArray.Count == 0 ? 0 : itemInfoArray[0].ItemID);
                dsItem.PushIntoStack(itemInfoArray.Count == 0 ? string.Empty : itemInfoArray[0].ItemName.ToNotNullString());
                dsItem.PushIntoStack(medicineLv);
                dsItem.PushIntoStack(itemInfoArray.Count == 0 ? string.Empty : itemInfoArray[0].HeadID.ToNotNullString());
                dsItem.PushIntoStack(useGold);

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("MedicineType", ref medicineType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            string medicineInfo = "{ 1 = 30, 2 = 150, 3 = 300, 4 = 600, 5 = 1200, 6 = 2400 }";
            medicineArray = medicineInfo.Split(',');

            return true;
        }
    }
}
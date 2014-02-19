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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1406_丹药列表接口
    /// </summary>
    public class Action1406 : BaseAction
    {
        private int generalID = 0;
        private short gridStatus = 0;
        private short powerNum;
        private short soulNum;
        private short intellectNum;
        private string toUserID;

        private UserGeneral generalInfo = null;
        private List<GeneralMedicine> generalMedicineArray = new List<GeneralMedicine>();

        public Action1406(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1406, httpGet)
        {

        }

        public override void BuildPacket()
        {

            PushIntoStack(generalInfo == null ? string.Empty : generalInfo.GeneralName.ToNotNullString());
            PushIntoStack(generalMedicineArray.Count);
            foreach (GeneralMedicine item in generalMedicineArray)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.MedicineID);

                int itemcount = TrumpHelper.GetUserItemNum(item.UserID, item.MedicineID);
                if (itemcount > 0)
                {
                    gridStatus = 1;
                }
                else
                {
                    gridStatus = 2;
                }

                DataStruct ds = new DataStruct();
                ds.PushIntoStack(item.MedicineID);
                ds.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                ds.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : itemInfo.MedicineLv);
                ds.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : itemInfo.MedicineType);
                ds.PushIntoStack((short)GetGeneralMedicine(item));
                ds.PushIntoStack(gridStatus);
                ds.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                PushIntoStack(ds);
            }
            this.PushIntoStack(powerNum);
            this.PushIntoStack(soulNum);
            this.PushIntoStack(intellectNum);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID))
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
            generalInfo = UserGeneral.GetMainGeneral(publicUserID);
            generalMedicineArray = new GameDataCacheSet<GeneralMedicine>().FindAll(publicUserID, m => m.GeneralID == generalID);
            foreach (GeneralMedicine medicine in generalMedicineArray)
            {
                if (medicine.MedicineType == 1)
                {
                    powerNum = MathUtils.Addition(powerNum, (short)medicine.BaseNum, short.MaxValue);
                }
                else if (medicine.MedicineType == 2)
                {
                    soulNum = MathUtils.Addition(soulNum, (short)medicine.BaseNum, short.MaxValue);
                }
                else if (medicine.MedicineType == 3)
                {
                    intellectNum = MathUtils.Addition(intellectNum, (short)medicine.BaseNum, short.MaxValue);
                }
            }
            return true;
        }

        /// <summary>
        /// 药剂数量
        /// </summary>
        /// <param name="medicine"></param>
        /// <returns></returns>
        public static int GetGeneralMedicine(GeneralMedicine medicine)
        {
            var medicineArray = new GameDataCacheSet<GeneralMedicine>().FindAll(medicine.UserID, u => u.MedicineID.Equals(medicine.MedicineID) && u.GeneralID == medicine.GeneralID);
            return medicineArray.Count;
        }
    }
}
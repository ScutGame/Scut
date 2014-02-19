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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1407_丹药服用接口
    /// </summary>
    public class Action1407 : BaseAction
    {
        private int generalID = 0;
        private int medicineID = 0;
        private int ops = 0;

        public Action1407(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1407, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetInt("MedicineID", ref medicineID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(medicineID);
            var userItemArray = UserItemHelper.GetItems(Uid).FindAll(u => (u.ItemStatus == ItemStatus.BeiBao || u.ItemStatus == ItemStatus.CangKu));
            if (userItemArray.Count == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1407_MedicineNum;
                return false;
            }
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (general == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St10006_DoesNotExistTheGeneral;
                return false;
            }
            if (itemInfo.DemandLv > general.GeneralLv)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_LevelNotEnough;
                return false;
            }

            int sum = 0;
            short baseNum = 0;
            //佣兵服用ID为medicineID的丹药
            var generalMedicineArray = new GameDataCacheSet<GeneralMedicine>().FindAll(ContextUser.UserID, g => g.MedicineID.Equals(medicineID) && g.GeneralID == generalID);
            if (generalMedicineArray.Count > 0)
            {
                int mLv = itemInfo.MedicineLv;
                int mNum = generalMedicineArray.Count;
                if (mLv == 1 && mNum >= 2 || mLv == 2 && mNum >= 3 || mLv == 3 && mNum >= 4 || mLv == 4 && mNum >= 5 || mLv == 5 && mNum >= 6 || mLv == 6 && mNum >= 7)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1407_MedicineNumFull;
                    return false;
                }

                sum = mNum * 5; //
                baseNum = MathUtils.Subtraction(itemInfo.MedicineNum, (short)sum, (short)0);
            }
            else
            {
                baseNum = itemInfo.MedicineNum;
            }

            //vip
            int useGold = 0;
            int[] vipMeArray = { 30, 150, 300, 600, 1200, 2400 };
            for (int i = 1; i <= vipMeArray.Length; i++)
            {
                if (itemInfo.MedicineLv == i)
                {
                    useGold = vipMeArray[i - 1];
                }
            }

            if (ops == 0)
            {

                GeneralMedicine generalMedicine = new GeneralMedicine()
                {
                    GeneralMedicineID = Guid.NewGuid().ToString(),
                    UserID = ContextUser.UserID,
                    GeneralID = generalID,
                    MedicineID = medicineID,
                    BaseNum = (int)baseNum
                };
                var cacheSet = new GameDataCacheSet<GeneralMedicine>();
                cacheSet.Add(generalMedicine);

                UserItemHelper.UseUserItem(ContextUser.UserID, itemInfo.ItemID, 1);

                if (itemInfo.MedicineType == 1)
                {
                    general.PowerNum = MathUtils.Addition(general.PowerNum, baseNum, short.MaxValue);
                }
                else if (itemInfo.MedicineType == 2)
                {
                    general.SoulNum = MathUtils.Addition(general.SoulNum, baseNum, short.MaxValue);
                }
                else if (itemInfo.MedicineType == 3)
                {
                    general.IntellectNum = MathUtils.Addition(general.IntellectNum, baseNum, short.MaxValue);
                }
                //general.Update();
            }
            else if (ops == 1)
            {
                ErrorCode = 1;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1407_MedicineUseGold, useGold);
                return false;
            }
            else if (ops == 2)
            {
                if (ContextUser.GoldNum < useGold)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }

                GeneralMedicine generalMedicine = new GeneralMedicine()
                {
                    GeneralMedicineID = Guid.NewGuid().ToString(),
                    UserID = ContextUser.UserID,
                    GeneralID = generalID,
                    MedicineID = medicineID,
                    BaseNum = (int)baseNum
                };
                var cacheSet = new GameDataCacheSet<GeneralMedicine>();
                cacheSet.Add(generalMedicine);

                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold, int.MaxValue);
                //ContextUser.Update();

                if (itemInfo.MedicineType == 1)
                {
                    general.PowerNum = MathUtils.Addition(general.PowerNum, baseNum, short.MaxValue);
                }
                else if (itemInfo.MedicineType == 2)
                {
                    general.SoulNum = MathUtils.Addition(general.SoulNum, baseNum, short.MaxValue);
                }
                else if (itemInfo.MedicineType == 3)
                {
                    general.IntellectNum = MathUtils.Addition(general.IntellectNum, baseNum, short.MaxValue);
                }
                //general.Update();
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().UrlElement;
                return false;
            }
            return true;
        }
    }
}
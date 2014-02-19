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
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 药剂摘除
    /// </summary>
    public class Action1415 : BaseAction
    {
        private int generalID = 0;
        private int medicineID = 0;
        private const int itemID = 7005;//药剂粉末
        private int ops = 1;

        private string[] medicineArray = new string[0];

        public Action1415(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1415, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetInt("MedicineID", ref medicineID))
            {
                httpGet.GetInt("ops", ref ops);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(medicineID);
            if (itemInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            if (ops == 1)
            {
                ErrorCode = 1;
                ErrorInfo = string.Format(LanguageManager.GetLang().St11415_ClearMedicine, itemInfo.ItemName, itemInfo.MedicineLv, itemInfo.EffectNum);
                return false;
            }

            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (general == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St10006_DoesNotExistTheGeneral;
                return false;
            }
            var cacheSet = new GameDataCacheSet<GeneralMedicine>();
            List<GeneralMedicine> generalMedicineArray = cacheSet.FindAll(ContextUser.UserID, g => g.MedicineID.Equals(medicineID) && g.GeneralID == generalID);
            generalMedicineArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return x.BaseNum.CompareTo(y.BaseNum);
            });
            if (generalMedicineArray.Count <= 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1415_MedicineNum;
                return false;
            }
            if (UserHelper.IsBeiBaoFull(ContextUser))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1415_GridNumNotEnough;
                return false;
            }

            //UserItem[] userItemArray = UserItem.FindAll(UserItem.Index_UserID, u => (u.ItemStatus == ItemStatus.BeiBao || u.ItemStatus == ItemStatus.CangKu) && u.ItemID == itemID, ContextUser.UserID);
            var package = UserItemPackage.Get(Uid);
            List<UserItemInfo> userItemArray = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemID == itemID);
            //药剂摘除道不足
            int Num = itemInfo.MedicineLv;
            if (userItemArray.Count <= 0 || UserItemMedicineNum(userItemArray) < Num)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1415_MedicineNum;
                return false;
            }
            UserItemHelper.UseUserItem(ContextUser.UserID, itemID, Num);
            if (RandomUtils.GetRandom() <= itemInfo.EffectNum)
            {
                short propertyNum = MedicinePropertyNum(ContextUser.UserID, medicineID, generalID);
                switch (itemInfo.MedicineType)
                {
                    case 1:
                        general.PowerNum = MathUtils.Subtraction(general.PowerNum, propertyNum);
                        break;
                    case 2:
                        general.SoulNum = MathUtils.Subtraction(general.SoulNum, propertyNum);
                        break;
                    case 3:
                        general.IntellectNum = MathUtils.Subtraction(general.IntellectNum, propertyNum);
                        break;
                    default:
                        general.PowerNum = general.PowerNum; ;
                        break;
                }
                //general.Update();
                cacheSet.Delete(generalMedicineArray[0]);
                ErrorCode = 2;
                UserItemHelper.AddUserItem(ContextUser.UserID, medicineID, 1);
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St11415_Clearfail;
                return false;
            }

            return true;
        }

        /// <summary>
        ///  当前背包的奇幻粉末
        /// </summary>
        /// <returns></returns>
        private static int UserItemMedicineNum(List<UserItemInfo> userItemArray)
        {
            int sumNum = 0;
            foreach (UserItemInfo itemInfo in userItemArray)
            {
                sumNum = MathUtils.Addition(sumNum, itemInfo.Num);
            }
            return sumNum;
        }

        /// <summary>
        /// 摘除后扣除属性数值
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="medicineID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static short MedicinePropertyNum(string userID, int medicineID, int generalID)
        {
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(medicineID);
            short baseNum = 0;
            int sumNum = 0;
            //佣兵服用ID为medicineID的丹药
            List<GeneralMedicine> generalMedicineArray = new GameDataCacheSet<GeneralMedicine>().FindAll(userID, g => g.MedicineID.Equals(medicineID) && g.GeneralID == generalID);
            if (generalMedicineArray.Count > 0)
            {
                int mLv = itemInfo.MedicineLv;
                int mNum = generalMedicineArray.Count;
                switch (mLv)
                {
                    case 1:
                        sumNum = 2;
                        break;
                    case 2:
                        sumNum = 3;
                        break;
                    case 3:
                        sumNum = 4;
                        break;
                    case 4:
                        sumNum = 5;
                        break;
                    case 5:
                        sumNum = 6;
                        break;
                    case 6:
                        sumNum = 7;
                        break;
                    default:
                        sumNum = 0;
                        break;
                }
                int exciseNum = MathUtils.Subtraction(mNum, 1); //摘除后药剂的数量
                if (MathUtils.Subtraction(sumNum, exciseNum) > 0)
                {
                    baseNum = (MathUtils.Subtraction(sumNum, exciseNum) * 5).ToShort();
                }
                else
                {
                    baseNum = 5;
                }
            }
            else
            {
                baseNum = itemInfo.MedicineNum;
            }
            return baseNum;
        }

    }
}
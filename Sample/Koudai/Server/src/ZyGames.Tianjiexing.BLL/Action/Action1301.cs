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
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 1301_命运水晶列表接口
    /// </summary>
    public class Action1301 : BaseAction
    {
        private int crystalPackNum = 0;
        private List<UserGeneral> userGeneralArray = new List<UserGeneral>();
        private List<UserCrystalInfo> userCrystalArray1 = new List<UserCrystalInfo>();
        private List<UserCrystalInfo> userCrystalArray2 = new List<UserCrystalInfo>();
        private string toUserID;
        private short genQuality = 0;

        public Action1301(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1301, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(crystalPackNum);
            PushIntoStack(userGeneralArray.Count);
            foreach (UserGeneral general in userGeneralArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(general.GeneralID);
                dsItem.PushIntoStack(general.GeneralName.ToNotNullString());
                dsItem.PushIntoStack(general.GeneralLv);
                dsItem.PushIntoStack(general.HeadID.ToNotNullString());
                dsItem.PushIntoStack(UserHelper.GetOpenNum(general.GeneralLv));
                dsItem.PushIntoStack(general.GeneralQuality.ToShort());
                this.PushIntoStack(dsItem);
            }
            this.PushIntoStack(userGeneralArray.Count <= 0 ? string.Empty : userGeneralArray[0].PicturesID);
            this.PushIntoStack(userCrystalArray1.Count);
            foreach (UserCrystalInfo crystal in userCrystalArray1)
            {
                CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(crystal.CrystalID);

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(crystal.UserCrystalID);
                dsItem.PushIntoStack(crystalInfo == null ? 0 : crystalInfo.CrystalID);
                dsItem.PushIntoStack(crystalInfo == null ? string.Empty : crystalInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(crystal.Position);
                dsItem.PushIntoStack(crystalInfo == null ? string.Empty : crystalInfo.CrystalName.ToNotNullString());
                dsItem.PushIntoStack(crystal.CrystalLv);
                dsItem.PushIntoStack(crystalInfo == null ? LanguageManager.GetLang().shortInt : (short)crystalInfo.CrystalQuality);
                this.PushIntoStack(dsItem);
            }
            this.PushIntoStack(userCrystalArray2.Count);
            foreach (UserCrystalInfo crystal in userCrystalArray2)
            {
                CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(crystal.CrystalID);

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(crystal.UserCrystalID);
                dsItem.PushIntoStack(crystalInfo == null ? 0 : crystalInfo.CrystalID);
                dsItem.PushIntoStack(crystalInfo == null ? string.Empty : crystalInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(crystalInfo == null ? string.Empty : crystalInfo.CrystalName.ToNotNullString());
                dsItem.PushIntoStack(crystal.CrystalLv);
                dsItem.PushIntoStack(crystalInfo == null ? LanguageManager.GetLang().shortInt : (short)crystalInfo.CrystalQuality);
                this.PushIntoStack(dsItem);
            }
            PushIntoStack(genQuality);
        }

        public override bool GetUrlElement()
        {
            httpGet.GetString("ToUserID", ref toUserID);
            return true;
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
            if (new GameDataCacheSet<UserFunction>().FindKey(publicUserID, FunctionEnum.Mingyunshuijing) == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_NoFun;
                return false;
            }
            UserHelper.GetUserLightOpen(publicUserID);

            var package = UserCrystalPackage.Get(publicUserID);

            crystalPackNum = user.CrystalNum;
            userGeneralArray = new GameDataCacheSet<UserGeneral>().FindAll(publicUserID, u => u.GeneralStatus == GeneralStatus.DuiWuZhong && u.GeneralType != GeneralType.Soul);
            // 佣兵排序
            GeneralSortHelper.GeneralSort(ContextUser.UserID, userGeneralArray);
            
            if (userGeneralArray.Count > 0)
            {
                genQuality = userGeneralArray[0].GeneralQuality.ToShort();
                userCrystalArray1 = package.CrystalPackage.FindAll(m => m.GeneralID == userGeneralArray[0].GeneralID);
                if (string.IsNullOrEmpty(toUserID))
                {
                    userCrystalArray2 = package.CrystalPackage.FindAll(m => m.GeneralID == 0 && m.IsSale == 2);
                    userCrystalArray2.QuickSort((x, y) =>
                    {
                        int result;
                        if (x == null && y == null) return 0;
                        if (x != null && y == null) return 1;
                        if (x == null) return -1;
                        result = y.CrystalQuality.CompareTo(x.CrystalQuality);
                        if (result == 0)
                        {
                            result = y.CurrExprience.CompareTo(x.CurrExprience);
                        }
                        return result;
                    });
                }
            }
            return true;
        }
    }
}
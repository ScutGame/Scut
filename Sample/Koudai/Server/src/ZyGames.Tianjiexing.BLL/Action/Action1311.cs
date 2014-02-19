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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1311_命运背包水晶列表接口
    /// </summary>
    public class Action1311 : BaseAction
    {
        private List<UserCrystalInfo> userCrystalArray = new List<UserCrystalInfo>();

        public Action1311(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1311, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(ContextUser.CrystalNum);
            PushIntoStack(userCrystalArray.Count);
            foreach (UserCrystalInfo crystal in userCrystalArray)
            {
                UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, crystal.GeneralID);
                CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(crystal.CrystalID);

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(crystal.UserCrystalID);
                dsItem.PushIntoStack(crystal.CrystalID);
                dsItem.PushIntoStack(crystalInfo == null ? string.Empty : crystalInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(crystalInfo == null ? string.Empty : crystalInfo.CrystalName.ToNotNullString());
                dsItem.PushIntoStack(crystal.CrystalLv);
                dsItem.PushIntoStack(crystalInfo == null ? LanguageManager.GetLang().shortInt : (short)crystalInfo.CrystalQuality);
                dsItem.PushIntoStack(crystalInfo == null ? LanguageManager.GetLang().shortInt : (short)crystalInfo.AbilityID);
                dsItem.PushIntoStack(CrystalAbilityNum(crystal.CrystalID, crystal.CrystalLv).ToNotNullString());
                dsItem.PushIntoStack(general == null ? string.Empty : general.GeneralName.ToNotNullString());
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            var package = UserCrystalPackage.Get(Uid);
            userCrystalArray = package.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(0));
            userCrystalArray.QuickSort((x, y) =>
            {
                int result = 0;
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
            return true;
        }

        /// <summary>
        /// 命运水晶属性
        /// </summary>
        /// <param name="crystalID"></param>
        /// <param name="cryLv"></param>
        /// <returns></returns>
        public static string CrystalAbilityNum(int crystalID, short cryLv)
        {
            string _attrNum = string.Empty;
            CrystalLvInfo currCrystal = new ConfigCacheSet<CrystalLvInfo>().FindKey(crystalID, cryLv);
            if (currCrystal != null)
            {
                //_attrNum = (currCrystal.AbilityNum * 100) + "%";
                if (currCrystal.AbilityNum > 1)
                {
                    _attrNum = currCrystal.AbilityNum.ToString().Replace(".0000", "");
                }else
                {
                    _attrNum = Math.Round((currCrystal.AbilityNum * 100), 1, MidpointRounding.AwayFromZero).ToString();
                    _attrNum = _attrNum.Replace(".0", "") + "%";
                }
            }
            return _attrNum;
        }
    }
}
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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1308_合成命运水晶接口
    /// </summary>
    public class Action1308 : BaseAction
    {
        private string userCrystalID1 = string.Empty;
        private string userCrystalID2 = string.Empty;
        private int ops = 0;


        public Action1308(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1308, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserCrystalID1", ref userCrystalID1)
                 && httpGet.GetString("UserCrystalID2", ref userCrystalID2)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var package = UserCrystalPackage.Get(Uid);
            if (ops == 1)
            {
                UserCrystalInfo userCryStal1 = package.CrystalPackage.Find(m => m.UserCrystalID.Equals(userCrystalID1));
                UserCrystalInfo userCryStal2 = package.CrystalPackage.Find(m => m.UserCrystalID.Equals(userCrystalID2));
                // UserCrystal userCryStal1 = UserCrystal.FindKey(userCrystalID1);
                //UserCrystal userCryStal2 = UserCrystal.FindKey(userCrystalID2);
                if (userCryStal1 == null || userCryStal2 == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1308_CrystalNotEnough;
                    return false;
                }
                if (userCryStal1.CrystalLv > 9)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1308_CrystalLvFull;
                    return false;
                }

                CrystalInfo crystalInfo1 = new ConfigCacheSet<CrystalInfo>().FindKey(userCryStal1.CrystalID);
                CrystalInfo crystalInfo2 = new ConfigCacheSet<CrystalInfo>().FindKey(userCryStal2.CrystalID);
                int cryExprience1 = 0;
                int cryExprience2 = 0;
                if (crystalInfo1 != null && crystalInfo2 != null)
                {
                    cryExprience1 = crystalInfo1.Experience;
                    cryExprience2 = crystalInfo2.Experience;

                    if (crystalInfo1.CrystalQuality == crystalInfo2.CrystalQuality)
                    {
                        if (userCryStal1.CurrExprience >= userCryStal2.CurrExprience)
                        {
                            UpdateCrystal(userCrystalID1, userCrystalID2, cryExprience2);
                        }
                        else
                        {
                            UpdateCrystal(userCrystalID2, userCrystalID1, cryExprience1);
                        }
                    }
                    else if (crystalInfo1.CrystalQuality > crystalInfo2.CrystalQuality)
                    {
                        UpdateCrystal(userCrystalID1, userCrystalID2, cryExprience2);
                    }
                    else
                    {
                        UpdateCrystal(userCrystalID2, userCrystalID1, cryExprience1);
                    }

                }
            }
            else if (ops == 2)
            {
                CacheList<SynthesisInfo> SynList = new CacheList<SynthesisInfo>();
                int experience = 0;
                var userCrystalArray = package.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(0));
                if (userCrystalArray.Count == 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1308_CrystalNotEnough;
                    return false;
                }
                userCrystalArray.QuickSort((x, y) =>
                {
                    int result = 0;
                    if (x == null && y == null) return 0;
                    if (x != null && y == null) return 1;
                    if (x == null) return -1;
                    result = (int)new ConfigCacheSet<CrystalInfo>().FindKey(y.CrystalID).CrystalQuality.CompareTo(
                        new ConfigCacheSet<CrystalInfo>().FindKey(x.CrystalID).CrystalQuality);
                    if (result == 0)
                    {
                        result = y.CurrExprience.CompareTo(x.CurrExprience);
                    }
                    return result;
                });

                UserCrystalInfo userCrystal1 =
                    package.CrystalPackage.Find(m => m.UserCrystalID == userCrystalArray[0].UserCrystalID);
                int maxExprience = 0;
                if (userCrystal1 == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1308_CrystalNotEnough;
                    return false;
                }
                short upLv = 10;
                CrystalLvInfo crystalLvInfo = new ConfigCacheSet<CrystalLvInfo>().FindKey(userCrystal1.CrystalID, upLv);
                maxExprience = crystalLvInfo == null ? 0 : crystalLvInfo.UpExperience;


                foreach (UserCrystalInfo crystal in userCrystalArray)
                {
                    CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(crystal.CrystalID);
                    if (crystalInfo != null)
                    {
                        experience = crystalInfo.Experience;
                    }

                    if (userCrystal1.UserCrystalID != crystal.UserCrystalID)
                    {
                        SynList.Add(new SynthesisInfo() { DemandID = crystal.CrystalID, Num = crystal.CurrExprience });
                        if (userCrystal1.CurrExprience >= maxExprience)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1308_CrystalLvFull;
                            break;
                        }
                        experience = MathUtils.Addition(experience, userCrystal1.CurrExprience);
                        experience = MathUtils.Addition(experience, crystal.CurrExprience);
                        userCrystal1.CurrExprience = experience;
                        package.SaveCrystal(userCrystal1);
                        //package.DelayChange();
                        package.RemoveCrystal(crystal);
                    }
                }
                UserLogHelper.AppenCtystalLog(ContextUser.UserID, 3, userCrystal1.CrystalID, 0, 0, SynList, userCrystal1.CrystalLv, userCrystal1.CurrExprience);
                UserHelper.CheckCrystalEscalate(ContextUser.UserID, userCrystalArray[0].UserCrystalID);
            }
            return true;
        }

        /// <summary>
        /// 合成水晶
        /// </summary>
        /// <param name="userCryStal1"></param>
        /// <param name="userCryStal2"></param>
        /// <param name="cryExprience2"></param>
        private void UpdateCrystal(string _userCrystalID1, string _userCrystalID2, int cryExprience2)
        {
            var package = UserCrystalPackage.Get(Uid);
            UserCrystalInfo userCryStalInfo1 = package.CrystalPackage.Find(m => m.UserCrystalID.Equals(_userCrystalID1));
            UserCrystalInfo userCryStalInfo2 = package.CrystalPackage.Find(m => m.UserCrystalID.Equals(_userCrystalID2));
            CacheList<SynthesisInfo> SynList = new CacheList<SynthesisInfo>();
            SynList.Add(new SynthesisInfo() { DemandID = userCryStalInfo2.CrystalID, Num = userCryStalInfo2.CurrExprience });
            userCryStalInfo1.CurrExprience = MathUtils.Addition(userCryStalInfo1.CurrExprience, cryExprience2);
            userCryStalInfo1.CurrExprience = MathUtils.Addition(userCryStalInfo1.CurrExprience,userCryStalInfo2.CurrExprience);
            package.SaveCrystal(userCryStalInfo1);
            //package.DelayChange();
            UserHelper.CheckCrystalEscalate(ContextUser.UserID, userCryStalInfo1.UserCrystalID);
            UserLogHelper.AppenCtystalLog(ContextUser.UserID, 3, userCryStalInfo1.CrystalID, 0, 0, SynList, userCryStalInfo1.CrystalLv, userCryStalInfo1.CurrExprience);
            package.RemoveCrystal(userCryStalInfo2);

        }
    }
}
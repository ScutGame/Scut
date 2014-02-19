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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1307_拾取命运水晶接口
    /// </summary>
    public class Action1307 : BaseAction
    {
        private string userCrystalID = string.Empty;
        private int ops = 0;


        public Action1307(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1307, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops))
            {
                httpGet.GetString("UserCrystalID", ref userCrystalID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var package = UserCrystalPackage.Get(Uid);
            var userCrystalArray = package.CrystalPackage.FindAll(m => m.IsSale == 1 && m.GeneralID.Equals(0));
            //命格背包
            var crystalArray = package.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(0));
            if (ops == 1)
            {
                UserCrystalInfo userCrystal = package.CrystalPackage.Find(m => m.UserCrystalID.Equals(userCrystalID));
                if (crystalArray.Count >= ContextUser.CrystalNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1307_FateBackpackFull;
                    return false;
                }
                userCrystal.IsSale = 2;
                package.SaveCrystal(userCrystal);
                ErrorCode = 1;
            }
            else if (ops == 2)
            {
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
                int subNum = ContextUser.CrystalNum;
                if (subNum <= crystalArray.Count)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1307_FateBackpackFull;
                    return false;
                }

                foreach (UserCrystalInfo crystal in userCrystalArray)
                {
                    UserCrystalInfo[] crystalArray1 = package.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(0)).ToArray();
                    if (subNum <= crystalArray1.Length)
                    {
                        ErrorCode = 1;
                        ErrorInfo = LanguageManager.GetLang().St1307_FateBackpackFull;
                        return false;
                    }
                    CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(crystal.CrystalID);
                    if (crystalInfo != null && crystalInfo.IsTelegrams == 1 && crystal.IsSale == 1)
                    {
                        crystal.IsSale = 2;
                        package.SaveCrystal(crystal);
                        //package.DelayChange();
                        ErrorCode = 1;
                    }
                }
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().ServerBusy;
                return false;
            }
            return true;
        }
    }
}
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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1309_佣兵替换命运水晶接口
    /// </summary>
    public class Action1309 : BaseAction
    {
        private int generalID = 0;
        private string userCrystalID = string.Empty;
        private short potion = 0;
        private int ops = 0;

        public Action1309(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1309, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetString("UserCrystalID", ref userCrystalID)
                 && httpGet.GetWord("Potion", ref potion)
                && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var package = UserCrystalPackage.Get(Uid);
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            UserCrystalInfo userCrystal = package.CrystalPackage.Find(m => m.UserCrystalID.Equals(userCrystalID));
            if (userCrystal == null || userGeneral == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }

            var userCrystalArray = package.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(0));
            var crystalArray = package.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(generalID));
            if (ops == 0)
            {
                if (userCrystal.IsSale == 2 && userCrystal.GeneralID != 0)
                {
                    if (userCrystalArray.Count >= ContextUser.CrystalNum)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1307_FateBackpackFull;
                        return false;
                    }

                    if (userCrystalArray.Count < ContextUser.CrystalNum &&
                       userCrystal.GeneralID > 0 &&
                       userCrystal.Position > 0)
                    {
                        userCrystal.GeneralID = 0;
                        userCrystal.Position = 0;
                        package.SaveCrystal(userCrystal);
                        //package.DelayChange();
                    }
                }
            }
            if (potion > IsGridOpen(userGeneral.GeneralLv))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1213_GridNumNotEnough;
                return false;
            }
            else if (ops == 1)
            {
                if (userCrystal.IsSale == 2)
                {
                    CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(userCrystal.CrystalID);
                    if (crystalInfo == null)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        return false;
                    }

                    if (crystalArray.Count >= UserHelper.GetOpenNum(userGeneral.GeneralLv))
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1309_OpenNumNotEnough;
                        return false;
                    }

                    foreach (UserCrystalInfo crystal in crystalArray)
                    {
                        CrystalInfo crystalInfo2 = new ConfigCacheSet<CrystalInfo>().FindKey(crystal.CrystalID);
                        if (crystalInfo2 != null && crystalInfo.AbilityID == crystalInfo2.AbilityID)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1309_TheSameFate;
                            return false;
                        }
                    }

                    UserCrystalInfo[] potionCrystalArray = package.CrystalPackage.FindAll(m => m.GeneralID == generalID && m.Position == potion).ToArray();
                    if (potionCrystalArray.Length > 0)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1309_TheGridFullSameFate;
                        return false;
                    }

                    if (userCrystal.IsSale == 2 &&
                         userCrystal.GeneralID == 0 &&
                         userCrystal.Position == 0)
                    {
                        userCrystal.GeneralID = generalID;
                        userCrystal.Position = potion;
                        package.SaveCrystal(userCrystal);
                    }
                }
            }
            userGeneral.RefreshMaxLife();
            return true;
        }

        /// <summary>
        /// 玩家佣兵开启的命运水晶格子
        /// </summary>
        /// <param name="userLv"></param>
        /// <returns></returns>
        public static int IsGridOpen(short userLv)
        {
            int gridPotion = 0;
            if (userLv >= 20 && userLv < 30)
            {
                gridPotion = 1;
            }
            else if (userLv >= 30 && userLv < 40)
            {
                gridPotion = 2;
            }
            else if (userLv >= 40 && userLv < 50)
            {
                gridPotion = 3;
            }
            else if (userLv >= 50 && userLv < 60)
            {
                gridPotion = 4;
            }
            else if (userLv >= 60 && userLv < 70)
            {
                gridPotion = 5;
            }
            else if (userLv >= 70 && userLv < 80)
            {
                gridPotion = 6;
            }
            else if (userLv >= 80 && userLv < 90)
            {
                gridPotion = 7;
            }
            else if (userLv >= 90)
            {
                gridPotion = 8;
            }
            return gridPotion;
        }
    }
}
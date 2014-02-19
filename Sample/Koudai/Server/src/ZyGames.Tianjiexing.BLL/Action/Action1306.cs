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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1306_卖出命运水晶接口
    /// </summary>
    public class Action1306 : BaseAction
    {
        private string userCrystalID = string.Empty;
        private int ops = 0;
        private int totalPrice = 0;

        public Action1306(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1306, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(totalPrice);
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
            int saleNum = 0;
            if (ops == 1)
            {
                var package = UserCrystalPackage.Get(Uid);
                UserCrystalInfo userCrystal = package.CrystalPackage.Find(m => m.UserCrystalID.Equals(userCrystalID));
                if (userCrystal != null)
                {
                    totalPrice = (new ConfigCacheSet<CrystalInfo>().FindKey(userCrystal.CrystalID) ?? new CrystalInfo()).Price;

                    ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, totalPrice, int.MaxValue);
                    //ContextUser.Update();
                    UserLogHelper.AppenCtystalLog(ContextUser.UserID, 4, userCrystal.CrystalID, totalPrice, 0, null, userCrystal.CrystalLv, userCrystal.CurrExprience);
                    package.RemoveCrystal(userCrystal);

                }
                else
                {
                    CrystalHelper.SellGrayCrystal(ContextUser, userCrystalID, out saleNum);
                }
            }
            else if (ops == 2)
            {
                CrystalHelper.SellGrayCrystal(ContextUser, null, out saleNum);
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
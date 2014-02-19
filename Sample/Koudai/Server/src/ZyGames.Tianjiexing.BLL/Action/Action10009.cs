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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 10009_购买接口
    /// </summary>
    public class Action10009 : BaseAction
    {
        private int ops = 0;
        private int dewNum = 0;
        public Action10009(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action10009, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(dewNum);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            //UserHelper.ChechDailyRestrain(ContextUser.UserID);
            if(ContextUser.VipLv<5)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St10009_NotPayDew;
                return false;
            }
            int sumGold = 0;
            int payDewTime = 0;
            UserPlant plant = new GameDataCacheSet<UserPlant>().FindKey(ContextUser.UserID);
            if (plant.DewNum == 8)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St10009_DewNumFull;
                return false;
            }
            if (plant != null && DateTime.Now.Date == plant.DewDate.Date)
            {
                payDewTime = MathUtils.Addition(plant.PayDewTime, 1, int.MaxValue);
                sumGold = (payDewTime * 2);
            }
            else
            {
                payDewTime = 1;
                sumGold = 2;
            }
            if (ops == 1)
            {
                ErrorCode = 1;
                ErrorInfo = string.Format(LanguageManager.GetLang().St10009_PayDewUseGold, sumGold);
                return false;
            }
            else if (ops == 2)
            {
                if (ContextUser.GoldNum < sumGold)
                {
                    ErrorCode = 2;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }

                if (ContextUser.GoldNum >= sumGold)
                {
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, sumGold, int.MaxValue);
                    //ContextUser.Update();
                    dewNum = MathUtils.Addition(plant.DewNum, 1, int.MaxValue);
                    plant.DewNum = dewNum;
                    plant.PayDewTime = payDewTime;
                    plant.DewDate = DateTime.Now;
                    //plant.Update();
                    UserLogHelper.AppenUseGoldLog(ContextUser.UserID, 3, 0, dewNum, sumGold,
                                                    ContextUser.GoldNum,
                                                    MathUtils.Addition(ContextUser.GoldNum, sumGold, int.MaxValue));
                    UserLogHelper.AppenLandLog(ContextUser.UserID, 8, 0, 0, sumGold, 0, 0, dewNum);
                }
            }

            return true;
        }
    }
}
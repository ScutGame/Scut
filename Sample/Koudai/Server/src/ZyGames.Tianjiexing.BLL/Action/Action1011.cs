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
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1011_挖金矿接口
    /// </summary>
    public class Action1011 : BaseAction
    {
        private int payType = 0;
        private int ops = 0;


        public Action1011(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1011, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PayType", ref payType)
                   && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserHelper.ChechDailyRestrain(ContextUser.UserID);
            int wjkNum = 0;
            int subNum = 0;
            int maxNum = VipHelper.GetVipUseNum(ContextUser.VipLv, RestrainType.WaJinKuang);
            UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);
            if (userRestrain != null && DateTime.Now.Date == userRestrain.RefreshDate.Date)
            {
                wjkNum = userRestrain.Funtion3;
                subNum = MathUtils.Subtraction(maxNum, wjkNum);
            }
            else
            {
                wjkNum = 0;
                subNum = maxNum;
            }

            if ((userRestrain != null && userRestrain.Funtion3 >= VipHelper.GetVipUseNum(ContextUser.VipLv, RestrainType.WaJinKuang) && DateTime.Now.Date == userRestrain.RefreshDate.Date))
            {
                ErrorCode = 2;
                ErrorInfo = LanguageManager.GetLang().St1011_WaJinKuangFull;
                return false;
            }

            int useGold = GetPayCoinGold(ContextUser.UserID, ContextUser.VipLv, payType);
            int payCoinNum = GetPayCoinNum(ContextUser.UserID, ContextUser.VipLv, payType, ContextUser.UserLv);

            if (payType == 1)
            {
                if (ops == 1)
                {
                    this.ErrorCode = 1;
                    this.ErrorInfo = string.Format(LanguageManager.GetLang().St1011_PayUseGold, useGold, payCoinNum, subNum);
                    return false;
                }
                else if (ops == 2)
                {
                    if (ContextUser.GoldNum < useGold)
                    {
                        this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }

                    if (ContextUser.GoldNum >= useGold)
                    {
                        int goldNum = ContextUser.GoldNum;
                        ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold);
                        ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, payCoinNum);
                        //ContextUser.Update();

                        if (userRestrain != null)
                        {
                            userRestrain.Funtion3 = MathUtils.Addition(wjkNum, 1);
                            //userRestrain.Update();
                            UserLogHelper.AppenUseGoldLog(ContextUser.UserID, 2, 0, userRestrain.Funtion3, useGold,
                                                        ContextUser.GoldNum, goldNum);
                        }
                    }
                    this.ErrorCode = 0;
                    this.ErrorInfo = string.Format(LanguageManager.GetLang().St1011_PayCoinUseGold, useGold, payCoinNum, subNum-1);
                    return false;

                }
            }
            else if (payType == 2)
            {
                if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.ZiDongJinRuJinKuangDong))
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_VipNotEnoughNotFuntion;
                    return false;
                }
                if (ops == 1)
                {
                    this.ErrorCode = 0;
                    this.ErrorInfo = string.Format(LanguageManager.GetLang().St1011_PayCoinUseGold, useGold, payCoinNum, subNum);
                    return false;
                }
                else if (ops == 2)
                {
                    if (ContextUser.VipLv < 6)
                    {
                        this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        this.ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
                        return false;
                    }
                    if (ContextUser.GoldNum < useGold)
                    {
                        this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }


                    if (userRestrain != null)
                    {
                        userRestrain.Funtion3 = MathUtils.Addition(wjkNum, 1);
                        //userRestrain.Update();
                    }
                    int gamecoin = GetPayCoinNum(ContextUser.UserID, ContextUser.VipLv, payType, ContextUser.UserLv);
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold);
                    ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, gamecoin);
                    //ContextUser.Update();

                }
            }
            return true;
        }

        /// <summary>
        /// 挖金矿获得的金币
        /// </summary>
        /// <param name="vipLv"></param>
        /// <param name="payType"></param>
        /// <param name="userLv"></param>
        /// <returns></returns>
        public static int GetPayCoinNum(string userID, int vipLv, int payType, int userLv)
        {
            int baseNum = MathUtils.Addition((userLv * 2000), 3000);
            int totalCoinNum = 0;
            UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            int maxNum = VipHelper.GetVipUseNum(vipLv, RestrainType.WaJinKuang);

            int currNum = 0;
            if (DateTime.Now.Date == userRestrain.RefreshDate.Date)
            {
                currNum = MathUtils.Subtraction(maxNum, userRestrain.Funtion3); 
            }
            else
            {
                currNum = maxNum;
            }
            if (payType == 1)
            {
                totalCoinNum = baseNum;
            }
            else if (payType == 2)
            {
                totalCoinNum = (currNum * baseNum);
            }
            return totalCoinNum;
        }

        /// <summary>
        /// 挖金矿花费晶石
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="payType"></param>
        /// <param name="vipLv"></param>
        /// <returns></returns>
        public static int GetPayCoinGold(string userID, int vipLv, int payType)
        {
            UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            int maxNum = VipHelper.GetVipUseNum(vipLv, RestrainType.WaJinKuang);
            int subNum = 0;
            int currNum = 0;
            if (DateTime.Now.Date == userRestrain.RefreshDate.Date)
            {
                subNum = MathUtils.Addition(userRestrain.Funtion3, 1, int.MaxValue);

                currNum = MathUtils.Subtraction(maxNum, userRestrain.Funtion3, 0);
            }
            else
            {
                currNum = 0;
            }
            int useGold = 0;
            if (payType == 1)
            {
                if (subNum >= 1 && subNum <= 9)
                {
                    useGold = (subNum * 2);
                }
                else if (subNum >= 10 && subNum <= 30)
                {
                    useGold = 20;
                }
                else if (subNum >= 31 && subNum <= 50)
                {
                    useGold = 50;
                }
                else if (subNum >= 51 && subNum <= 100)
                {
                    useGold = 100;
                }
                else if (subNum >= 101 && subNum <= 200)
                {
                    useGold = 200;
                }
                else
                {
                    useGold = 2;
                }
            }
            else if (payType == 2)
            {
                for (int i = currNum; i < maxNum; i++)
                {
                    if (currNum >= 1 && currNum <= 9)
                    {
                        useGold += (currNum * 2);
                    }
                    else if (currNum > 10 && currNum <= 30)
                    {
                        useGold += 20;
                    }
                    else if (currNum > 31 && currNum <= 50)
                    {
                        useGold += 50;
                    }
                    else if (currNum > 51 && currNum <= 100)
                    {
                        useGold += 100;
                    }
                    else if (currNum > 101 && currNum <= 200)
                    {
                        useGold += 200;
                    }
                }
            }
            return useGold;
        }
    }
}
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
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1010_购买体力接口
    /// </summary>
    public class Action1010 : BaseAction
    {
        private int payType = 0;
        private int ops = 0;


        public Action1010(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1010, httpGet)
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
            int energyNum = ConfigEnvSet.GetInt("User.EnergyNum");
            UserHelper.ChechDailyRestrain(ContextUser.UserID);
            var cacheSet = new GameDataCacheSet<UserDailyRestrain>();
            double addNum = FestivalHelper.SurplusPurchased(ContextUser.UserID, FestivalType.PurchasedEnergy);//活动加成
            int payNum = FestivalHelper.SurplusEnergyNum(ContextUser.UserID);
            UserDailyRestrain userRestrain = cacheSet.FindKey(ContextUser.UserID);
            if (payNum == 0)
            {
                if (userRestrain != null && DateTime.Now.Date != userRestrain.RefreshDate.Date)
                {
                    userRestrain.Funtion4 = 0;
                }

                if (userRestrain != null && userRestrain.Funtion4 >= VipHelper.GetVipUseNum(ContextUser.VipLv, RestrainType.GouMaiJingLi) && DateTime.Now.Date == userRestrain.RefreshDate.Date)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St1010_JingliFull;
                    return false;
                }
            }

            int useGold = (GetPayEnergyGold(ContextUser.UserID, ContextUser.VipLv, payType) * addNum).ToInt();
            if (payType == 1)
            {
                if (ops == 1)
                {
                    this.ErrorCode = 1;
                    this.ErrorInfo = string.Format(LanguageManager.GetLang().St1010_PayEnergyUseGold, useGold, energyNum);
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

                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold, int.MaxValue);
                    ContextUser.EnergyNum = MathUtils.Addition(ContextUser.EnergyNum, GetEnergyNum(ContextUser.UserID, ContextUser.VipLv, payType), short.MaxValue);
                    if (payNum == 0)
                    {
                        if (userRestrain != null)
                        {
                            userRestrain.Funtion4 = MathUtils.Addition(userRestrain.Funtion4, 1, int.MaxValue);
                        }
                        else
                        {
                            userRestrain = new UserDailyRestrain();
                            userRestrain.UserID = ContextUser.UserID;
                            userRestrain.RefreshDate = DateTime.Now;
                            userRestrain.Funtion4 = 1;
                            cacheSet.Add(userRestrain);
                        }
                    }

                    UserLogHelper.AppenUseGoldLog(ContextUser.UserID, 1, 0, userRestrain.Funtion4, useGold, ContextUser.GoldNum, MathUtils.Addition(ContextUser.GoldNum, useGold, int.MaxValue));
                    FestivalHelper.PurchasedEnergy(ContextUser.UserID);
                }
            }
            else if (payType == 2)
            {
                short payEnergy = GetEnergyNum(ContextUser.UserID, ContextUser.VipLv, payType);
                if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.ZiDongGouMaiJingLi))
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_VipNotEnoughNotFuntion;
                    return false;
                }
                if (ops == 1)
                {
                    this.ErrorCode = 1;
                    this.ErrorInfo = string.Format(LanguageManager.GetLang().St1010_PayEnergyUseGold, useGold, payEnergy);
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

                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold, int.MaxValue);
                    ContextUser.EnergyNum = MathUtils.Addition(ContextUser.EnergyNum, payEnergy, short.MaxValue);

                    if (userRestrain != null)
                    {
                        userRestrain.Funtion4 = MathUtils.Addition(userRestrain.Funtion4, 1, int.MaxValue);
                    }
                    else
                    {
                        userRestrain = new UserDailyRestrain();
                        userRestrain.UserID = ContextUser.UserID;
                        userRestrain.RefreshDate = DateTime.Now;
                        userRestrain.Funtion4 = 1;
                        cacheSet.Add(userRestrain);
                    }

                    UserLogHelper.AppenUseGoldLog(ContextUser.UserID, 1, userRestrain.Funtion4, 1, useGold, ContextUser.GoldNum, MathUtils.Addition(ContextUser.GoldNum, useGold, int.MaxValue));
                }
            }
            return true;
        }

        public static int GetPayEnergyGold(string userID, int vipLv, int payType)
        {
            int payNum = FestivalHelper.SurplusEnergyNum(userID);
            int currNum = 0;
            int maxNum = 0;

            if (payNum == 0)
            {
                UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
                maxNum = VipHelper.GetVipUseNum(vipLv, RestrainType.GouMaiJingLi);

                if (DateTime.Now.Date == dailyRestrain.RefreshDate.Date)
                {
                    currNum = dailyRestrain.Funtion4;
                }
                else
                {
                    currNum = 0;
                }
            }
            int useGold = 0;
            if (payType == 1)
            {
                if (currNum == 0)
                {
                    useGold = 20;
                }
                else if (currNum > 0 && currNum <= 5)
                {
                    useGold = 20;
                }
                else if (currNum > 5 && currNum <= 20)
                {
                    useGold = 80;
                }
                else if (currNum > 20 && currNum <= 45)
                {
                    useGold = 200;
                }
                else if (currNum > 45 && currNum <= 80)
                {
                    useGold = 400;
                }
            }
            else if (payType == 2)
            {
                for (int i = currNum; i < maxNum; i++)
                {
                    if (currNum == 1)
                    {
                        useGold = 20;
                    }
                    else if (currNum > 1 && currNum <= 5)
                    {
                        useGold += 40;
                    }
                    else if (currNum > 5 && currNum <= 20)
                    {
                        useGold += 80;
                    }
                    else if (currNum > 20 && currNum <= 45)
                    {
                        useGold += 200;
                    }
                    else if (currNum > 45 && currNum <= 80)
                    {
                        useGold += 400;
                    }
                }
            }
            return useGold;
        }

        /// <summary>
        /// 购买精力次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="vipLv"></param>
        /// <returns></returns>
        private static int GetPayEnergyNum(string userID)
        {
            int currNum = 0;
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && DateTime.Now.Date == dailyRestrain.RefreshDate.Date)
            {
                currNum = dailyRestrain.Funtion4;
            }
            return currNum;
        }

        /// <summary>
        /// 获得精力
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="vipLv"></param>
        /// <param name="payType"></param>
        /// <returns></returns>
        public static short GetEnergyNum(string userID, int vipLv, int payType)
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            int maxNum = VipHelper.GetVipUseNum(vipLv, RestrainType.GouMaiJingLi);
            int currNum = 0;
            if (DateTime.Now.Date == dailyRestrain.RefreshDate.Date)
            {
                currNum = MathUtils.Subtraction(maxNum, dailyRestrain.Funtion4, 0);
            }
            else
            {
                currNum = maxNum;
            }
            short energyNum = 0;
            if (payType == 1)
            {
                energyNum = ConfigEnvSet.GetInt("User.EnergyNum").ToShort();
            }
            else if (payType == 2)
            {
                energyNum = (short)(currNum * 40);
            }
            return energyNum;
        }
    }
}
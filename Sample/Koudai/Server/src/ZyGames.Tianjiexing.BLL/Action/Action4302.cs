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
    /// 4302_刷新天地劫副本接口
    /// </summary>
    public class Action4302 : BaseAction
    {
        private short refreshType;
        private int ops;


        public Action4302(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4302, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {

            if (httpGet.GetWord("RefreshType", ref refreshType)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            UserHelper.ChechDailyRestrain(ContextUser.UserID);
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);

            if (refreshType == 1)
            {
                if (dailyRestrain != null)
                {
                    if (!IsRefreshKalpa(dailyRestrain, RestrainType.RefreshKalpa, 0))
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St4302_PlotRefresh;
                        return false;
                    }
                }
            }
            else if (refreshType == 2)
            {
                if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.SecondRefreshKalpa))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
                    return false;
                }
                int vipRefeshGold = ConfigEnvSet.GetInt("UserPlot.SecondRefreshKalpa");
                if (ops == 1)
                {
                    ErrorCode = 1;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St4302_SecondRefreshKalpa, vipRefeshGold);
                    return false;
                }
                else if (ops == 2)
                {
                    if (vipRefeshGold > ContextUser.GoldNum)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                }
                if (dailyRestrain != null)
                {
                    if (!IsRefreshKalpa(dailyRestrain, RestrainType.RefreshKalpa, vipRefeshGold))
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St4302_PlotRefresh;
                        return false;
                    }
                }
            }
            else if (refreshType == 3)
            {
                int lastRefeshGold = ConfigEnvSet.GetInt("UserPlot.LastRefreshKalpa");
                if (ops == 1)
                {
                    ErrorCode = 1;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St4302_LastRefreshKalpa, lastRefeshGold);
                    return false;
                }
                else if (ops == 2)
                {
                    if (ContextUser.UserExtend != null && ContextUser.UserExtend.LayerNum == 1)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St4302_LastRefreshKalpaNotEnough;
                        return false;
                    }
                    if (lastRefeshGold > ContextUser.GoldNum)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                }
                if (dailyRestrain != null)
                {
                    if (!IsRefreshKalpa(dailyRestrain, RestrainType.RefreshLastKalpa, lastRefeshGold))
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St4302_PlotRefresh;
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsRefreshKalpa(UserDailyRestrain dailyRestrain, RestrainType refresh, int useGold)
        {
            if (dailyRestrain.UserExtend != null)
            {
                int kalpaNum = 0;
                if (refresh == RestrainType.RefreshKalpa)
                {
                    kalpaNum = dailyRestrain.UserExtend.KalpaNum;
                }
                else if (refresh == RestrainType.RefreshLastKalpa)
                {
                    kalpaNum = dailyRestrain.UserExtend.LastKalpaNum;
                }
                if (VipHelper.DailyRestrainSurplusNum(ContextUser, refresh, kalpaNum) > 0)
                {
                    //DailyUserExtend userExtend = dailyRestrain.UserExtend;
                    dailyRestrain.UserExtend.UpdateNotify(obj =>
                    {
                        dailyRestrain.UserExtend.KalpaDate = DateTime.Now;
                        if (refresh == RestrainType.RefreshKalpa)
                        {
                            dailyRestrain.UserExtend.KalpaNum = MathUtils.Addition(dailyRestrain.UserExtend.KalpaNum, 1, int.MaxValue);
                        }
                        else if (refresh == RestrainType.RefreshLastKalpa)
                        {
                            dailyRestrain.UserExtend.LastKalpaNum = MathUtils.Addition(dailyRestrain.UserExtend.LastKalpaNum, 1, int.MaxValue);
                        }
                        dailyRestrain.UserExtend.KalpaPlot = new CacheList<FunPlot>();
                        return true;
                    });
                    //dailyRestrain.Update();
                    if (ContextUser.UserExtend != null)
                    {
                        ContextUser.UserExtend.UpdateNotify(obj =>
                        {
                            if (refresh == RestrainType.RefreshLastKalpa)
                            {
                                ContextUser.UserExtend.LayerNum = MathUtils.Subtraction(ContextUser.UserExtend.LayerNum, 1, 1);
                            }
                            ContextUser.UserExtend.HurdleNum = 1;
                            return true;
                        });
                        ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold);
                        //ContextUser.Update();
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
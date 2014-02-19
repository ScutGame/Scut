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
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    /// 4014_重置英雄副本接口
    /// </summary>
    public class Action4014 : BaseAction
    {
        private int cityID;
        private int ops;


        public Action4014(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4014, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("CityID", ref cityID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int totalNum = 0;
            UserHelper.ChechDailyRestrain(ContextUser.UserID);
            totalNum = PlotHelper.HeroSurplusNum(ContextUser.UserID, cityID, ContextUser.VipLv);
            if (totalNum <= 0 || PlotHelper.HeroRefreshNum(ContextUser.UserID, cityID) > totalNum)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4014_HeroRefreshPlotFull;
                return false;
            }
            int useGold = ConfigEnvSet.GetInt("UserPlot.HeroRefreshGoldNum");
            if (ops == 1)
            {
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St4014_HeroRefreshPlot, useGold);
                return false;
            }
            else if (ops == 2)
            {
                if (ContextUser.GoldNum < useGold)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);
                if (dailyRestrain != null)
                {
                    //z增加刷新次数 （未完成）
                    HeroRefreshNum(ContextUser.UserID, cityID);
                }
                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold, int.MaxValue);
                //ContextUser.Update();
            }
            return true;
        }



        /// <summary>
        /// 增加刷新次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cityID"></param>
        /// <returns></returns>
        public static void HeroRefreshNum(string userID, int cityID)
        {
            var cacheSet = new GameDataCacheSet<UserDailyRestrain>();
            UserDailyRestrain dailyRestrain = cacheSet.FindKey(userID);
            if (dailyRestrain != null)
            {
                DailyUserExtend userExtend = new DailyUserExtend();
                List<HeroPlot> heroList = new List<HeroPlot>();
                if (dailyRestrain.UserExtend != null)
                {
                    userExtend = dailyRestrain.UserExtend;
                    if (dailyRestrain.UserExtend.HeroPlot.Count > 0)
                    {
                        heroList = dailyRestrain.UserExtend.HeroPlot;
                    }
                }
                HeroPlot plot = heroList.Find(m => m.CityID.Equals(cityID));
                if (plot != null)
                {
                    plot.HeroNum = MathUtils.Addition(plot.HeroNum, 1);
                }
                else
                {
                    heroList.Add(new HeroPlot() { CityID = cityID, HeroNum = 1, HeroList = new List<FunPlot>() });
                }
                userExtend.HeroPlot = heroList;
                dailyRestrain.UserExtend = userExtend;
            }
            cacheSet.Update();
        }
    }
}
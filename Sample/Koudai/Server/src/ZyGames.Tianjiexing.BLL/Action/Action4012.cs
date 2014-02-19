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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 4012_重置精英副本接口
    /// </summary>
    public class Action4012 : BaseAction
    {
        private int ops;


        public Action4012(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4012, httpGet)
        {

        }

        public override void BuildPacket()
        {

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
            int totalNum = 0;
            UserHelper.ChechDailyRestrain(ContextUser.UserID);
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);
            if (VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.ZhongZhiJingYingPlot))
            {
                totalNum = MathUtils.Addition(totalNum, 1, int.MaxValue);
            }

            if (VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.SecondZhongZhiJingYingPlot))
            {
                totalNum = MathUtils.Addition(totalNum, 1, int.MaxValue);
            }
            if (totalNum <= 0 || (dailyRestrain != null && dailyRestrain.Funtion10 >= totalNum))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4012_JingYingPlotFull;
                return false;
            }
            int useGold = ConfigEnvSet.GetInt("ResetElitePlot");
            if (ops == 1)
            {
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St4012_JingYingPlot, useGold);
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

                if (dailyRestrain != null)
                {
                    dailyRestrain.Funtion10 = MathUtils.Addition(dailyRestrain.Funtion10, 1, int.MaxValue);
                    //dailyRestrain.Update();
                }
                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold, int.MaxValue);
                //ContextUser.Update();
            }
            return true;
        }
    }
}
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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 4203_可创建副本列表接口
    /// </summary>
    public class Action4203 : BaseAction
    {
        private MorePlot[] morePlotArray = new MorePlot[0];
        private FunctionEnum funEnum;

        public Action4203(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4203, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(morePlotArray.Length);
            foreach (MorePlot morePlot in morePlotArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(morePlot.PlotID);
                dsItem.PushIntoStack(morePlot.PlotName.ToNotNullString());
                dsItem.PushIntoStack(morePlot.ItemId);
                dsItem.PushIntoStack(morePlot.ItemName.ToNotNullString());
                dsItem.PushIntoStack(morePlot.ItemNum);
                dsItem.PushIntoStack(morePlot.ExpNum);
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("FunEnum", ref funEnum))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            if (!PlotTeamCombat.IsMorePlotDate(ContextUser.UserID,funEnum))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4202_OutMorePlotDate;
                return false;
            }
            var plotTeam = new PlotTeamCombat(ContextUser);
            if (!string.IsNullOrEmpty(plotTeam.ToString()))
            {
                if (funEnum == FunctionEnum.Multiplot)
                {
                    morePlotArray = plotTeam.GetMorePlotList();
                }
                else
                {
                    morePlotArray = plotTeam.GetMorePlotFestivalList(funEnum);
                }
            }
            return true;
        }
    }
}
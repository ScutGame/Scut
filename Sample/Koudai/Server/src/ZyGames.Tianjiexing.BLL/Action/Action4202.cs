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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 4202_多人副本组队列表接口
    /// </summary>
    public class Action4202 : BaseAction
    {
        private List<MorePlotTeam> morePlotArray = new List<MorePlotTeam>();
        private FunctionEnum funEnum;

        public Action4202(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4202, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(morePlotArray.Count);
            foreach (MorePlotTeam plotTeam in morePlotArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(plotTeam.MorePlot.PlotID);
                dsItem.PushIntoStack(plotTeam.MorePlot.PlotName.ToNotNullString());
                dsItem.PushIntoStack(plotTeam.TeamID);
                dsItem.PushIntoStack(plotTeam.MorePlot.ItemId);
                dsItem.PushIntoStack(plotTeam.MorePlot.ItemName.ToNotNullString());
                dsItem.PushIntoStack(plotTeam.MorePlot.ItemNum);
                dsItem.PushIntoStack(plotTeam.MorePlot.ExpNum);
                dsItem.PushIntoStack(plotTeam.TeamUser.UserId.ToNotNullString());
                dsItem.PushIntoStack(plotTeam.TeamUser.NickName.ToNotNullString());
                dsItem.PushIntoStack(plotTeam.UserList.Count);
                dsItem.PushIntoStack(PlotTeamCombat.TeamMaxPeople);

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

            if (!PlotTeamCombat.IsMorePlotDate(ContextUser.UserID, funEnum))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4202_OutMorePlotDate;
                return false;
            }
            CombatHelper.RemoveDailyMorePlot(ContextUser.UserID, funEnum); //清空副本次数
            var plotTeam = new PlotTeamCombat(ContextUser);
            if (!string.IsNullOrEmpty(plotTeam.ToString()))
            {
                if (funEnum == FunctionEnum.Multiplot)
                {
                    morePlotArray = plotTeam.ToTeamList();
                }
                else
                {
                    morePlotArray = plotTeam.ToMoreTeamList();
                }
            }

            return true;
        }
    }
}
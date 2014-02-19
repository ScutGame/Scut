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
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 4205_创建副本接口
    /// </summary>
    public class Action4205 : BaseAction
    {
        private int plotID = 0;
        private int teamID = 0;
        private FunctionEnum funEnum;

        public Action4205(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4205, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(teamID);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotID", ref plotID))
            {
                httpGet.GetEnum("FunEnum", ref funEnum);
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
            if (UserHelper.IsBeiBaoFull(ContextUser))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1107_GridNumFull;
                return false;
            }
            var plotTeam = new PlotTeamCombat(ContextUser);
            teamID = plotTeam.GetTeamId(Uid);
            if (teamID != -1)
            {
                return true;
            }
            if (!plotTeam.CreateTeam(plotID, out teamID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            return true;
        }
    }
}
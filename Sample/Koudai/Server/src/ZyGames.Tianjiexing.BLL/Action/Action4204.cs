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
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 4204_副本组队列表接口
    /// </summary>
    public class Action4204 : BaseAction
    {
        private int teamID = 0;
        private MorePlotTeam moreTeam = null;

        public Action4204(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4204, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(moreTeam.MorePlot.PlotID);
            PushIntoStack(moreTeam.MorePlot.PlotName.ToNotNullString());
            PushIntoStack(moreTeam.UserList.Count);
            PushIntoStack(PlotTeamCombat.TeamMaxPeople);
            PushIntoStack(moreTeam.MorePlot.ItemId);
            PushIntoStack(moreTeam.MorePlot.ItemName.ToNotNullString());
            PushIntoStack(moreTeam.MorePlot.ItemNum);
            PushIntoStack(moreTeam.MorePlot.ExpNum);
            PushIntoStack(moreTeam.TeamUser != null && moreTeam.TeamUser.UserId.ToNotNullString().Equals(Uid) ? 1 : 0);
            PushIntoStack(moreTeam.Status);

            PushIntoStack(moreTeam.UserList.Count);
            foreach (var teamUser in moreTeam.UserList)
            {
                var gameUser = new GameDataCacheSet<GameUser>().FindKey(teamUser.UserId);
                UserGeneral general = UserGeneral.GetMainGeneral(teamUser.UserId);
                CareerInfo careerInfo = null;
                if (general != null)
                {
                    careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(general.CareerID);
                }

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(teamUser.UserId);
                dsItem.PushIntoStack(teamUser.NickName.ToNotNullString());
                dsItem.PushIntoStack(careerInfo == null ? 0 : (int)careerInfo.CareerID);
                dsItem.PushIntoStack(careerInfo == null ? string.Empty : careerInfo.CareerName.ToNotNullString());
                dsItem.PushIntoStack(gameUser == null ? (short)0 : gameUser.UserLv);
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("TeamID", ref teamID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var plotTeam = new PlotTeamCombat(ContextUser);
            moreTeam = plotTeam.GetTeam(teamID);
            return true;
        }
    }
}
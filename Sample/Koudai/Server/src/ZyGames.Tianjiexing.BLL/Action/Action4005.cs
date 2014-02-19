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
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 4005_离开副本接口
    /// </summary>
    public class Action4005 : BaseAction
    {
        private int plotID;

        public Action4005(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4005, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(ContextUser.CityID);
            PushIntoStack(ContextUser.PointX);
            PushIntoStack(ContextUser.PointY);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotID", ref plotID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {

            if (ContextUser.TempEnergyNum > 0 && ContextUser.UserStatus != UserStatus.Combat)
            {
                List<UserPlotCombat> plotCombatList = new GameDataCacheSet<UserPlotCombat>().FindAll(ContextUser.UserID, m => m.PlotID == plotID);
                //没发生战斗或上次战斗失败都返还精力
                if (plotCombatList.Count == 0 || (ContextUser.TempEnergyNum > 0 && IsNotCombat(plotCombatList)))
                {
                    if (ContextUser.SurplusEnergy == 0 && !ContextUser.IsSurplus) //策划要求：领取的不返还
                    {
                        if (ContextUser.UserExtend != null && ContextUser.UserExtend.PlotStatusID > 0 && ContextUser.UserExtend.MercenarySeq <= 1)
                        {
                            ContextUser.TempEnergyNum = 0;
                            ContextUser.EnergyNum = MathUtils.Addition(ContextUser.EnergyNum, PlotInfo.BattleEnergyNum, short.MaxValue);
                        }
                        //ContextUser.Update();
                    }
                }
            }
            if (ContextUser.UserExtend != null)
            {
                ContextUser.UserExtend.UpdateNotify(obj =>
                {
                    ContextUser.UserExtend.PlotStatusID = 0;
                    ContextUser.UserExtend.PlotNpcID = -1;
                    ContextUser.UserExtend.MercenarySeq = 0;
                    ContextUser.UserExtend.IsBoss = false;
                    return true;
                });
            }
            return true;
        }

        private bool IsNotCombat(List<UserPlotCombat> plotCombatList)
        {
            //只要有战斗就不返还精力
            foreach (var plotCombat in plotCombatList)
            {
                if (plotCombat.CombatDate > ContextUser.InPlotDate)//&& plotCombat.IsWin
                {
                    return false;
                }
            }
            return true;
        }
    }
}
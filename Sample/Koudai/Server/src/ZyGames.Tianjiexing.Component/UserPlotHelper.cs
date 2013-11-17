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
using System.Linq;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.ConfigModel;

namespace ZyGames.Tianjiexing.Component
{
    public class UserPlotHelper
    {
        /// <summary>
        /// 获取一个副本
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        /// <returns></returns>
        public static UserPlotInfo GetUserPlotInfo(string userID, int plotID)
        {
            UserPlotInfo userPlotInfo = null;
            var package = UserPlotPackage.Get(userID);
            if (package != null)
            {
                userPlotInfo = package.PlotPackage.Find(s => s.PlotID == plotID);
            }
            return userPlotInfo;
        }

        /// <summary>
        ///  获取全部副本
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<UserPlotInfo> UserPlotFindAll(string userID)
        {
            List<UserPlotInfo> userPlotList = new List<UserPlotInfo>();
            var package = UserPlotPackage.Get(userID);
            if (package != null)
            {
                userPlotList = package.PlotPackage.ToList();
            }
            return userPlotList;
        }

        /// <summary>
        /// 副本是否开启
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userPlot"></param>
        /// <returns></returns>
        public static short GetPlotStatus(string userID, UserPlotInfo userPlot)
        {
            if (userPlot != null)
            {
                PlotInfo plotInfo1 = new ConfigCacheSet<PlotInfo>().FindKey(userPlot.PlotID);
                if (plotInfo1 != null && plotInfo1.PrePlotID > 0)
                {
                    UserPlotInfo preUserPlot = GetUserPlotInfo(userID, plotInfo1.PrePlotID);
                    if (preUserPlot == null || preUserPlot.PlotStatus != PlotStatus.Completed)
                    {
                        return (short)PlotStatus.Locked;
                    }
                }
                return (short)userPlot.PlotStatus;
            }
            return (short)PlotStatus.Locked;
        }
        
    }
}
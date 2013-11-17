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
using System.Web;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class UserArchaeologyHelper
    {
        public static void ShuffleArray(ref int[] arr)
        {
            var ran = new Random();
            int k = 0;
            int temp = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                k = ran.Next(1, 10);
                if (k != i)
                {
                    temp = arr[i];
                    arr[i] = arr[k];
                    arr[k] = temp;
                }
            }
        }

        /// <summary>
        /// 获取当前周一的时间
        /// </summary>
        /// <returns></returns>
        public static DateTime MondayDate()
        {
            DateTime currentTime;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                currentTime = DateTime.Now.AddDays(-6);
            }
            else
            {
                int diffDays = (Convert.ToInt32(DateTime.Now.DayOfWeek) - 1);
                currentTime = DateTime.Now.AddDays(-diffDays);
            }
            return currentTime;
        }

        public static void InitializeMapInfo(string userID)
        {
            var userPlotPackage = new GameDataCacheSet<UserPlotPackage>().FindKey(userID);
            if (userPlotPackage != null)
            {
                userPlotPackage.PlotPackage.Add(new UserPlotInfo() { PlotID = 1222, BossChallengeCount = 5 });
                userPlotPackage.PlotPackage.Add(new UserPlotInfo() { PlotID = 1223, BossChallengeCount = 5 });
                userPlotPackage.PlotPackage.Add(new UserPlotInfo() { PlotID = 1224, BossChallengeCount = 5 });
            }
        }
    }
}
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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class VipHelper
    {
        /// <summary>
        /// 每日限制次数
        /// </summary>
        /// <returns></returns>
        public static int GetVipUseNum(int vipLv, RestrainType restrainType)
        {
            int rType = (int)restrainType;
            int baseNum = 0;
            int MaxNum = 0;
            int vipNum = 0;
            DailyRestrainSet restrainSet = new ShareCacheStruct<DailyRestrainSet>().FindKey(restrainType);
            if (restrainSet != null)
            {
                baseNum = restrainSet.MaxNum;
            }
            VipLvInfo lvInfo = new ConfigCacheSet<VipLvInfo>().FindKey(vipLv);
            var restrainArray = new CacheList<DailyRestrain>();
            if (lvInfo != null)
            {
                restrainArray = lvInfo.DailyRestrain;
            }
            foreach (DailyRestrain daily in restrainArray)
            {
                if (daily.ID == rType)
                {
                    vipNum = daily.Num;
                }
            }
            MaxNum = MathUtils.Addition(baseNum, vipNum, int.MaxValue);
            return MaxNum;
        }

        /// <summary>
        /// 每日限制剩余刷新次数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int DailyRestrainSurplusNum(GameUser user, RestrainType restrainType, int restrainNum)
        {
            int surplusNum = 0;
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
            DailyRestrainSet dailyRestrainSet = new ShareCacheStruct<DailyRestrainSet>().FindKey(restrainType);
            if (dailyRestrainSet != null && dailyRestrain != null)
            {
                if (dailyRestrain.UserExtend != null && DateTime.Now.Date == dailyRestrain.RefreshDate.Date)
                {
                    surplusNum = MathUtils.Subtraction(GetVipUseNum(user.VipLv, restrainType), restrainNum, 0);
                }
                else
                {
                    surplusNum = GetVipUseNum(user.VipLv, restrainType);
                }
            }
            return surplusNum;
        }


        /// <summary>
        /// 是否开启该功能
        /// </summary>
        /// <returns></returns>
        public static bool GetVipOpenFun(int vipLv, ExpandType expandType)
        {
            bool isOpen = false;
            VipLvInfo lvInfo = new ConfigCacheSet<VipLvInfo>().FindKey(vipLv);
            string eType = ((short)expandType).ToString();
            if (lvInfo != null)
            {
                string[] ExpandArray = lvInfo.ExpandFun.Split(',');
                foreach (string expand in ExpandArray)
                {
                    if (eType == expand)
                    {
                        isOpen = true;
                        break;
                    }
                }
            }
            return isOpen;
        }


    }
}
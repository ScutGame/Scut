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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    public class GeneralReplaceAttack
    {
        /// <summary>
        /// 当前玩家开启的阵法
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int EnabledUserMagicID(string userID)
        {
            int magicID = 0;
            var magicList = new GameDataCacheSet<UserMagic>().FindAll(userID, m => m.MagicType == MagicType.MoFaZhen && m.IsEnabled);
            if (magicList.Count > 0)
            {
                magicID = magicList[0].MagicID;
            }
            return magicID;
        }

        /// <summary>
        /// 替换佣兵位置
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="magicID"></param>
        /// <returns></returns>
        public static short CheckReplacePostion(string userID)
        {
            short replacePostion = 0;
            var magicList = new GameDataCacheSet<UserMagic>().FindAll(userID, m => m.MagicType == MagicType.MoFaZhen && m.IsEnabled);
            if (magicList.Count > 0)
            {
                var userMagic = magicList[0];
                MagicLvInfo magicLv = new ConfigCacheSet<MagicLvInfo>().FindKey(userMagic.MagicID, userMagic.MagicLv);
                if (magicLv != null && magicLv.ReplacePostion > 0)
                {
                    replacePostion = magicLv.ReplacePostion.ToShort();
                }
            }
            return replacePostion;
        }

    }
}
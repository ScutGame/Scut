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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class EmbattleHelper
    {
        /// <summary>
        /// 玩家当前出战佣兵列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="isSome">0：当前阵法出战佣兵总数 1：当前阵法佣兵</param>
        /// <returns></returns>
        public static List<UserEmbattle> CurrEmbattle(string userID, bool isSome)
        {
            List<UserEmbattle> embattleList = new List<UserEmbattle>();
            var userMagic = new GameDataCacheSet<UserMagic>().Find(userID, s => s.IsEnabled);
            if (userMagic != null)
            {
                if (!isSome)
                {
                    embattleList = new GameDataCacheSet<UserEmbattle>().FindAll(userID, s => s.MagicID == userMagic.MagicID);
                }
                else
                {
                    embattleList = new GameDataCacheSet<UserEmbattle>().FindAll(userID, s => s.MagicID == userMagic.MagicID && s.GeneralID > 0);
                }
            }
            return embattleList;
        }

        /// <summary>
        /// 是否在启用的阵法中
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static bool IsEmbattleGeneral(string userID, int generalID)
        {
            var embattleList = CurrEmbattle(userID, true);
            foreach (var embattle in embattleList)
            {
                if (embattle.GeneralID == generalID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
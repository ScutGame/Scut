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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 佣兵列表排序
    /// </summary>
    public class GeneralSortHelper
    {
        public static void GeneralSort(string userId, List<UserGeneral> userGenerals)
        {
            if (userGenerals.Count > 0)
            {
                var _userMagicArray = new GameDataCacheSet<UserMagic>().FindAll(userId);

                var userMagic =
                    _userMagicArray.Find(
                        x => x.UserID == userId && x.MagicType == MagicType.MoFaZhen && x.IsEnabled == true);
                var userEmbattleInfo = new GameDataCacheSet<UserEmbattle>().FindAll(userId,
                                                                                    x =>
                                                                                    x.MagicID == userMagic.MagicID);
                if (userEmbattleInfo != null)
                {
                    userEmbattleInfo.ForEach(x =>
                                                 {
                                                     foreach (var userGeneral in userGenerals)
                                                     {
                                                         if (x.GeneralID == userGeneral.GeneralID)
                                                         {
                                                             userGeneral.Position = x.Position;
                                                         }
                                                     }
                                                 });
                }

                userGenerals.QuickSort((x, y) =>
                                           {
                                               int result = 0;
                                               if (x == null && y == null) return 0;
                                               if (x != null && y == null) return 1;
                                               if (x == null) return -1;
                                               // 1、是否上阵(出手先后) 2、品质高低 
                                               result = y.IsBattle.CompareTo(x.IsBattle);
                                               if (result == 0)
                                               {
                                                   result = x.Position.CompareTo(y.Position);
                                               }
                                               if (result == 0)
                                               {
                                                   result = y.GeneralQuality.CompareTo(x.GeneralQuality);
                                               }
                                               if (result == 0)
                                               {
                                                   result = x.GeneralID.CompareTo(y.GeneralID);
                                               }
                                               return result;
                                           });
            }
        }
    }
}
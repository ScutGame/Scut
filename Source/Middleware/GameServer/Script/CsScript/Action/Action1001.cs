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
using GameServer.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;

namespace GameServer.CsScript.Action
{
    public class Action1001 : BaseStruct
    {
        private int PageIndex;
        private int PageSize;
        private int PageCount;
        private List<UserRanking> rankingList;


        public Action1001(HttpGet httpGet)
            : base(1001, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(PageCount);
            this.PushIntoStack(rankingList.Count);
            foreach (var item in rankingList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.UserName);
                dsItem.PushIntoStack(item.Score); 
                //Console.WriteLine("Num count:{0}", item.Items.Count);
                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref PageIndex)
                 && httpGet.GetInt("PageSize", ref PageSize))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var cache = new ShareCacheStruct<UserRanking>();
            rankingList = cache.FindAll(false);
            rankingList = MathUtils.QuickSort<UserRanking>(rankingList, compareTo);
            rankingList = rankingList.GetPaging(PageIndex, PageSize, out PageCount);
            return true;
        }

        private int compareTo(UserRanking x, UserRanking y)
        {
            int result = y.Score - x.Score;
            if (result == 0)
            {
                result = y.UserID - x.UserID;
            }
            return result;
        }
    }
}

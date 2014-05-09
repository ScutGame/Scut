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
using GameRanking.Pack;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.RPC.IO;

namespace GameServer.CsScript.Action
{
    public class Action1001 : BaseAction
    {
        private Request1001Pack requestPack;
        private Response1001Pack responsePack;

        public Action1001(ActionGetter actionGetter)
            : base(1001, actionGetter)
        {

        }

        public override bool GetUrlElement()
        {
            byte[] data = (byte[])actionGetter.GetMessage();
            if (data.Length > 0)
            {
                requestPack = ProtoBufUtils.Deserialize<Request1001Pack>(data);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int pageCount;
            var cache = new ShareCacheStruct<UserRanking>();
            var rankingList = cache.FindAll(false);
            rankingList = MathUtils.QuickSort<UserRanking>(rankingList, compareTo);
            rankingList = rankingList.GetPaging(requestPack.PageIndex, requestPack.PageSize, out pageCount);

            responsePack = new Response1001Pack();
            responsePack.PageCount = pageCount;
            responsePack.Items = new List<RankData>();
            foreach (var item in rankingList)
            {
                responsePack.Items.Add(new RankData() { UserName = item.UserName, Score = item.Score });
            }
            return true;
        }

        protected override byte[] BuildResponsePack()
        {
            return ProtoBufUtils.Serialize(responsePack);           
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

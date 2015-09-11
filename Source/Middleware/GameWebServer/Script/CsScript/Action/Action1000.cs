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

using GameServer.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;

namespace GameServer.CsScript.Action
{
    public class Action1000 : BaseStruct
    {
        private string UserName;
        private int Score;


        public Action1000(HttpGet httpGet)
            : base(1000, httpGet)
        {
        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserName", ref UserName)
                 && httpGet.GetInt("Score", ref Score))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var cache = new ShareCacheStruct<UserRanking>();
            var ranking = cache.Find(m => m.UserName == UserName);
            if (ranking == null)
            {
                var user = new GameUser() { UserId = (int)cache.GetNextNo(), NickName = UserName};
                new PersonalCacheStruct<GameUser>().Add(user);
                ranking = new UserRanking();
                ranking.UserID = user.UserId;
                ranking.UserName = UserName;
                ranking.Score = Score;
                cache.Add(ranking);
            }
            else
            {
                ranking.UserName = UserName;
                ranking.Score = Score;
            }
            return true;
        }

    }
}
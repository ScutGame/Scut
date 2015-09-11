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
using System.Collections.Generic;
using ZyGames.DirCenter.CacheData;
using ZyGames.DirCenter.Model;
using ZyGames.Framework.Game.Service;

namespace ZyGames.DirCenter.Action
{
    public class Action1001 : BaseStruct
    {
        private int gameID = 0;
        private List<ServerInfo> serverList;

        public Action1001(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1001, httpGet)
        {
        }

        public override bool TakeAction()
        {
            CacheServer cacheServer = new CacheServer();
            serverList = cacheServer.GetClientServerList(gameID);
            return true;

        }

        public override void BuildPacket()
        {
            PushIntoStack(serverList.Count);
            foreach (ServerInfo serverInfo in serverList)
            {
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(serverInfo.ID);
                ds.PushIntoStack(serverInfo.ServerName);
                ds.PushIntoStack(serverInfo.Status);
                ds.PushIntoStack(serverInfo.ServerUrl);
                ds.PushIntoStack(serverInfo.Weight);
                ds.PushIntoStack(serverInfo.TargetServer);
                PushIntoStack(ds);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GameID", ref gameID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
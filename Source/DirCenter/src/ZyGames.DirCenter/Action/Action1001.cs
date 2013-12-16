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
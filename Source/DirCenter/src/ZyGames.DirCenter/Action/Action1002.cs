using ZyGames.DirCenter.CacheData;
using ZyGames.DirCenter.Model;
using ZyGames.Framework.Game.Service;

namespace ZyGames.DirCenter.Action
{
    public class Action1002 : BaseStruct
    {
        private int _gameId = 0;
        private int _serverid = 0;
        private int _status = 0;
        private ServerInfo[] serverList;

        public Action1002(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1002, httpGet)
        {
        }

        public override bool TakeAction()
        {
            CacheServer cacheServer = new CacheServer();
            cacheServer.ServerStatus(_gameId, _serverid, _status);
            return true;

        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GameID", ref _gameId) &&
                httpGet.GetInt("ServerID", ref _serverid) && 
                httpGet.GetInt("Status", ref _status))
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
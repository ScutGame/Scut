using System;

namespace ZyGames.Framework.Game.SocketServer.Context
{
    public class GameSession
    {
        private string _sid;
        private int _gameId;
        private int _serverId;

        public static string GenerateSid(int gameId, int serverId)
        {
            return string.Format("{0}-{1}", gameId, serverId);
        }

        public GameSession(int gameId, int serverId)
        {
            _gameId = gameId;
            _serverId = serverId;
            _sid = GenerateSid(_gameId, _serverId);
        }

        public string Sid
        {
            get { return _sid; }
        }

        public int GameId
        {
            get { return _gameId; }
        }

        public int ServerId
        {
            get { return _serverId; }
        }

        public string GameAddress { get; set; }

        public DateTime AccessTime { get; set; }
    }
}

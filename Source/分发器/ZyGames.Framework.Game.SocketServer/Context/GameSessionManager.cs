using System.Collections.Concurrent;

namespace ZyGames.Framework.Game.SocketServer.Context
{
    /// <summary>
    /// 游戏服连接会话管理
    /// </summary>
    public class GameSessionManager
    {
        private static ConcurrentDictionary<string, GameSession> _sessionDict = new ConcurrentDictionary<string, GameSession>();

        public static bool Register(GameSession session)
        {
            GameSession old;
            if (_sessionDict.TryGetValue(session.Sid, out old))
            {
                return _sessionDict.TryUpdate(session.Sid, session, old);
            }
            return _sessionDict.TryAdd(session.Sid, session);
        }

        public static GameSession GetSession(int gameId, int serverId)
        {
            string sid = GameSession.GenerateSid(gameId, serverId);
            GameSession session;
            if (_sessionDict.TryGetValue(sid, out session))
            {
                return session;
            }
            return null;
        }

        public static void Remove(GameSession session)
        {
            GameSession value;
            _sessionDict.TryRemove(session.Sid, out value);
        }
    }
}

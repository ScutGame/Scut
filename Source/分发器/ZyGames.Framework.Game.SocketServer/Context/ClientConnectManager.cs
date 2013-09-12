using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ZyGames.Framework.Game.SocketServer.Context
{
    /// <summary>
    /// 客户连接管理
    /// </summary>
    public static class ClientConnectManager
    {
        private static ConcurrentDictionary<string, UserToken> _clientDict = new ConcurrentDictionary<string, UserToken>();
        private static ConcurrentDictionary<string, List<string>> _groupDict = new ConcurrentDictionary<string, List<string>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remotePoint"></param>
        /// <param name="token"></param>
        public static void Push(string remotePoint, UserToken token)
        {
            string key = GameSession.GenerateSid(token.GameId, token.ServerId);
            if (_clientDict.ContainsKey(remotePoint))
            {
                return;
            }
            _clientDict.TryAdd(remotePoint, token);

            List<string> list;
            if (!_groupDict.TryGetValue(key, out list))
            {
                list = new List<string>();
                list.Add(remotePoint);
                _groupDict.TryAdd(key, list);
            }
            else
            {
                lock (list)
                {
                    list.Add(remotePoint);
                }
            }
        }

        public static UserToken GetToken(string remotePoint)
        {
            UserToken val;
            if (_clientDict.TryGetValue(remotePoint, out val))
            {
                return val;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public static List<string> FindAll(int gameId, int serverId)
        {
            string key = GameSession.GenerateSid(gameId, serverId);
            List<string> list = new List<string>();
            List<string> items;
            if (_groupDict.TryGetValue(key, out items))
            {
                list.AddRange(items);
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remotePoint"></param>
        public static void Remove(string remotePoint)
        {
            if (_clientDict.ContainsKey(remotePoint))
            {
                UserToken token;
                if (_clientDict.TryRemove(remotePoint, out token))
                {
                    string key = GameSession.GenerateSid(token.GameId, token.ServerId);
                    List<string> list;
                    if (_groupDict.TryGetValue(key, out list))
                    {
                        lock (list)
                        {
                            int index = list.FindIndex(m => Equals(m, remotePoint));
                            if (index > -1 && index < list.Count)
                            {
                                list.RemoveAt(index);
                            }
                        }
                    }
                }
            }
        }
    }
}

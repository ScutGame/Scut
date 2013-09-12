using System;
using System.Collections.Concurrent;
using ZyGames.Framework.RPC.Web;

namespace ZyGames.Framework.Game.SocketServer.Context
{
    public class HttpConnectionManager
    {
        private static ConcurrentDictionary<string, HttpConnection> _dict = new ConcurrentDictionary<string, HttpConnection>();

        public static void Push(HttpConnection connection)
        {
            string ssid = connection.SSID.ToString("N");
            HttpConnection old;
            if (_dict.TryGetValue(ssid, out old))
            {
                _dict.TryUpdate(ssid, connection, old);
            }
            else
            {
                _dict.TryAdd(ssid, connection);
            }
        }

        public static HttpConnection Get(string ssid)
        {
            HttpConnection val;
            if (_dict.TryGetValue(ssid, out val))
            {
                return val;
            }
            return null;
        }

        public static void Remove(HttpConnection connection)
        {
            string ssid = connection.SSID.ToString("N");
            HttpConnection val;
            _dict.TryRemove(ssid, out val);
        }
    }
}
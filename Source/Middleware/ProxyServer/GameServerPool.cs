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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ProxyServer.com.scutgame.dir;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace ProxyServer
{
    internal enum ActionEnum
    {
        /// <summary>
        /// 心跳
        /// </summary>
        Heartbeat = 1,
        /// <summary>
        /// 中断
        /// </summary>
        Interrupt = 2
    }

    internal static class GameServerListManager
    {
        private static ConcurrentDictionary<string, ServerInfo> serversPool = new ConcurrentDictionary<string, ServerInfo>();

        public static void Initialize()
        {
            Initialize(s => { });
        }

        public static void Initialize(Action<ServerInfo> updateCallback)
        {
            try
            {
                string[] gameIds = ConfigUtils.GetSetting("GameIds", "").Split(',');
                DirServiceSoapClient dirClient = new DirServiceSoapClient();
                ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;

                string keyCode = "";
                List<GameInfo> gameList = new List<GameInfo>();
                foreach (var gameId in gameIds)
                {
                    if (!string.IsNullOrEmpty(gameId))
                    {
                        var gameInfo = dirClient.GetGameObj(gameId.ToInt());
                        if (gameInfo != null)
                        {
                            gameList.Add(gameInfo);
                        }
                    }
                }
                if (gameList.Count == 0)
                {
                    gameList.AddRange(dirClient.GetGame());
                }
                foreach (var gameInfo in gameList)
                {
                    var serverList = dirClient.GetServers(gameInfo.ID, false, false);
                    foreach (var serverInfo in serverList)
                    {
                        keyCode = string.Format("{0}_{1}", gameInfo.ID, serverInfo.ID);
                        ServerInfo temp;
                        if (!serversPool.TryGetValue(keyCode, out temp))
                        {
                            serversPool.TryAdd(keyCode, serverInfo);
                        }
                        else if (temp.IsEnable != serverInfo.IsEnable ||
                            temp.IntranetAddress != serverInfo.IntranetAddress ||
                            temp.ServerUrl != serverInfo.ServerUrl)
                        {
                            temp.IsEnable = serverInfo.IsEnable;
                            temp.IntranetAddress = serverInfo.IntranetAddress;
                            temp.ServerUrl = serverInfo.ServerUrl;
                            if (updateCallback != null)
                            {
                                updateCallback(temp);
                            }
                        }
                    }
                }
                TraceLog.ReleaseWrite("load game server:{0}", string.Join(",", serversPool.Keys));
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("load game server error:{0}", ex);
            }
        }

        public static ServerInfo Find(int gameId, int serverId)
        {
            string keyCode = string.Format("{0}_{1}", gameId, serverId);
            ServerInfo server;
            if (serversPool.TryGetValue(keyCode, out server))
            {
                return server;
            }
            return null;
        }
    }

    class GSConnectionManager
    {
        private ConcurrentDictionary<string, GameServerConnection> serversPool = new ConcurrentDictionary<string, GameServerConnection>();
        private Timer _timer;
        private int _dueRefleshTime = ConfigUtils.GetSetting("ServerHostRefleshTime", 120000); //2m
        private GameProxy proxy;

        public GSConnectionManager(GameProxy proxy)
        {
            this.proxy = proxy;
            _timer = new Timer(DoRefreshGameServer, null, 60000, _dueRefleshTime);
        }

        private void DoRefreshGameServer(object state)
        {
            try
            {
                GameServerListManager.Initialize(s =>
                {
                    string key = GetKey(s.GameID, s.ID);
                    TraceLog.ReleaseWrite("Reflesh server:{0} setting.", key);
                    GameServerConnection value;
                    if (serversPool.TryRemove(key, out value))
                    {
                        value.Stop();
                    }
                });
            }
            catch (Exception)
            {
            }
        }

        public bool Send(int gameId, int serverId, byte[] data)
        {
            GameServerConnection connection;
            string key = GetKey(gameId, serverId);
            if (!serversPool.TryGetValue(key, out connection))
            {
                lock (serversPool)
                {
                    if (!serversPool.TryGetValue(key, out connection))
                    {
                        var serverInfo = GameServerListManager.Find(gameId, serverId);
                        //需要实现的,加载服务器列表到proxy pool中,必须实现
                        if (serverInfo == null)
                            throw new ApplicationException(string.Format("游戏id={0}服务器id={1}不存在。", gameId, serverId));
                        if (!serverInfo.IsEnable)
                            throw new ApplicationException(string.Format("游戏id={0}服务器id={1}未开放。", gameId, serverId));
                        var arr = !string.IsNullOrEmpty(serverInfo.IntranetAddress)
                            ? serverInfo.IntranetAddress.Split(':')
                            : serverInfo.ServerUrl.Split(':');
                        var ip = arr[0];
                        var port = Convert.ToInt32(arr[1]);
                        connection = new GameServerConnection(ip, port, proxy);
                        serversPool[key] = connection;
                    }
                }
            }

            return connection.Send(data);
        }

        private string GetKey(int gameId, int serverId)
        {
            return string.Format("{0}_{1}", gameId, serverId);
        }
    }

    class GameServerConnection
    {
        private Timer _timer;
        private Guid _ssid;
        private ClientSocket clientSocket;
        private GameProxy proxy;
        private IPEndPoint remoteEndPoint;
        private object syncRoot = new object();
        private static int bufferSize = ConfigUtils.GetSetting("BufferSize", 8192);

        public GameServerConnection(string ip, int port, GameProxy proxy)
        {
            _ssid = Guid.NewGuid();
            this.proxy = proxy;

            remoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(ip)[0], port);
            ClientSocketSettings settings = new ClientSocketSettings(bufferSize, remoteEndPoint);
            clientSocket = new ClientSocket(settings);
            clientSocket.DataReceived += new SocketEventHandler(DataReceived);
            clientSocket.Disconnected += new SocketEventHandler(Disconnected);
            EnsureConnected();
            _timer = new Timer(DoCheckHeartbeat, null, 1000, 30 * 1000); //30s
        }

        public void Stop()
        {
            try
            {
                _timer.Dispose();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 心跳包
        /// </summary>
        /// <param name="state"></param>
        private void DoCheckHeartbeat(object state)
        {
            try
            {
                NameValueCollection requestParam = new NameValueCollection();
                requestParam["actionid"] = ((int)ActionEnum.Heartbeat).ToString();
                requestParam["ssid"] = _ssid.ToString("N");
                requestParam["msgid"] = "0";
                requestParam["isproxy"] = "true";
                string paramStr = RequestParse.ToQueryString(requestParam);
                byte[] paramData = Encoding.ASCII.GetBytes(paramStr);
                try
                {
                    Send(paramData);
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("连接游戏服中断error:{0}\r\nParam:{1}", ex, paramStr);
                }
            }
            catch (Exception ex)
            {
                TraceLog.ReleaseWriteDebug("心跳包连接游戏服失败,error:{0}", ex);
            }
        }

        private bool EnsureConnected()
        {
            if (!clientSocket.Connected)
            {
                lock (syncRoot)
                {
                    if (!clientSocket.Connected)
                    {
                        try
                        {
                            clientSocket.Connect();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            TraceLog.WriteError("连接到游戏服失败，地址:{0}, error:{1}", remoteEndPoint, ex);
                        }
                    }
                }
                return false;
            }
            return true;
        }

        void Disconnected(object sender, SocketEventArgs e)
        {
            proxy.FlushConnected();
        }

        void DataReceived(object sender, SocketEventArgs e)
        {
            try
            {
                var bytes = new byte[16];
                Buffer.BlockCopy(e.Data, 0, bytes, 0, 16);
                var ssid = new Guid(bytes);
                var sendResult = proxy.SendDataBack(ssid, e.Data, 16, e.Data.Length - 16);
                if (!sendResult)
                {
                    Task.Factory.StartNew(() =>
                    {
                        string paramStr = "";
                        try
                        {
                            NameValueCollection requestParam = new NameValueCollection();
                            //连接中断通知游戏服
                            requestParam["actionid"] = ((int)ActionEnum.Interrupt).ToString();
                            requestParam["ssid"] = ssid.ToString("N");
                            requestParam["msgid"] = "0";
                            paramStr = RequestParse.ToQueryString(requestParam);

                            byte[] paramData = Encoding.ASCII.GetBytes(paramStr);

                            Send(paramData);
                        }
                        catch (Exception ex)
                        {
                            TraceLog.WriteError("连接中断通知游戏服error:{0}\r\nParam:{1}", ex, paramStr);
                        }
                    });
                }
            }
            catch (Exception er)
            {
                TraceLog.WriteError("DataReceived error:{0}", er);
            }
        }

        public bool Send(byte[] data)
        {
            if (EnsureConnected())
            {
                clientSocket.PostSend(data, 0, data.Length);
                return true;
            }
            return false;
        }
    }
}
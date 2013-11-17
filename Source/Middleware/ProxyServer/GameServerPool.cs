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
using System.Net.Security;
using System.Text;
using NLog;
using ProxyServer.net._36you.dir;
using ZyGames.Framework.RPC.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace ProxyServer
{
    internal class GameServerListManager
    {
        private Logger logger = LogManager.GetLogger("GameServerListManager");
        private static GameServerListManager intance = new GameServerListManager();
        private ConcurrentDictionary<string, ServerInfo> serversPool = new ConcurrentDictionary<string, ServerInfo>();

        public static GameServerListManager Current
        {
            get { return intance; }
        }

        GameServerListManager()
        {
            try
            {

                DirServiceSoapClient dirClient = new DirServiceSoapClient();
                ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;

                string keyCode = "";
                var gameList = dirClient.GetGame();
                foreach (var gameInfo in gameList)
                {
                    var serverList = dirClient.GetServers(gameInfo.ID, false, false);
                    foreach (var serverInfo in serverList)
                    {
                        keyCode = string.Format("{0}_{1}", gameInfo.ID, serverInfo.ID);
                        serversPool.TryAdd(keyCode, serverInfo);
                    }
                }
                logger.Info("load game server:{0}", string.Join(",", serversPool.Keys));
            }
            catch (Exception ex)
            {
                logger.Error("load game server error:{0}", ex);
            }
        }

        public ServerInfo Find(int gameId, int serverId)
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
        private Dictionary<string, GameServerConnection> serversPool = new Dictionary<string, GameServerConnection>();
        private GameProxy proxy;
        private Logger logger = LogManager.GetLogger("GSConnectionManager");

        public GSConnectionManager(GameProxy proxy)
        {
            this.proxy = proxy;
        }

        public void Send(int gameId, int serverId, byte[] data)
        {
            GameServerConnection connection;
            var key = string.Format("{0}_{1}", gameId, serverId);
            if (!serversPool.TryGetValue(key, out connection))
            {
                lock (serversPool)
                {
                    if (!serversPool.TryGetValue(key, out connection))
                    {
                        var serverInfo = GameServerListManager.Current.Find(gameId, serverId);
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

            connection.Send(data);
        }
    }

    class GameServerConnection
    {
        private ClientSocket clientSocket;
        private GameProxy proxy;
        private static readonly Logger logger = LogManager.GetLogger("GameServerConnection");
        private IPEndPoint remoteEndPoint;
        private object syncRoot = new object();
        private static int bufferSize = Util.GetAppSetting<int>("BufferSize", 8192);

        public GameServerConnection(string ip, int port, GameProxy proxy)
        {
            this.proxy = proxy;

            remoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(ip)[0], port);
            ClientSocketSettings settings = new ClientSocketSettings(bufferSize, remoteEndPoint);
            clientSocket = new ClientSocket(settings);
            clientSocket.DataReceived += new SocketEventHandler(DataReceived);
            clientSocket.Disconnected += new SocketEventHandler(Disconnected);
        }

        private void EnsureConnected()
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
                        }
                        catch (Exception ex)
                        {
                            logger.Error("连接到游戏服失败，地址:{0}", remoteEndPoint);
                            throw ex;
                        }
                    }
                }
            }
        }

        void Disconnected(object sender, SocketEventArgs e)
        {
            proxy.FlushConnected();
        }

        void DataReceived(object sender, SocketEventArgs e)
        {
            var bytes = new byte[16];
            Buffer.BlockCopy(e.Data, 0, bytes, 0, 16);
            var ssid = new Guid(bytes);
            var data = e.Data;
            var sendResult = proxy.SendDataBack(ssid, e.Data, 16, e.Data.Length - 16);
            if (!sendResult)
            {
                Task.Factory.StartNew(() =>
                {
                    NameValueCollection requestParam = new NameValueCollection();
                    requestParam["actionid"] = "2";
                    requestParam["ssid"] = ssid.ToString("N");
                    requestParam["msgid"] = "0";

                    byte[] paramData = Encoding.ASCII.GetBytes(RequestParse.ToQueryString(requestParam));

                    try
                    {
                        Send(paramData);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("发送tcp连接断开通知失败。", ex);
                    }
                });
            }
        }

        public void Send(byte[] data)
        {
            EnsureConnected();
            clientSocket.PostSend(data, 0, data.Length);
        }
    }
}
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
using System.Text;
using System.Net;
using System.Web;
using System.Threading;
using System.Collections.Specialized;
using NLog;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.Sockets;

namespace ProxyServer
{
    class SocketProxy
    {
        private static int ListenPort = ConfigUtils.GetSetting("Port", 9500);
        private static string errmsg = ConfigUtils.GetSetting("ErrMsg", "");
        private static int expireTime = ConfigUtils.GetSetting("ExpireTime", 300000);
        private static int expireInterval = ConfigUtils.GetSetting("ExpireInterval", 60000);
        private static int maxConnections = ConfigUtils.GetSetting("MaxConnections", 40000);
        private static int backlog = ConfigUtils.GetSetting("Backlog", 100);
        private static int maxAcceptOps = ConfigUtils.GetSetting("MaxAcceptOps", 100);
        private static int bufferSize = ConfigUtils.GetSetting("BufferSize", 2048);
        private static int proxyCheckPeriod = ConfigUtils.GetSetting("ProxyCheckPeriod", 60000);

        private SocketListener listener;
        private GSConnectionManager gsConnectionManager;
        private MultiKeyDictionary<Guid, ExSocket, ClientConnection> clientConnections = new MultiKeyDictionary<Guid, ExSocket, ClientConnection>();
        private Timer timer;

        public SocketProxy(GSConnectionManager gsConnectionManager)
        {
            this.gsConnectionManager = gsConnectionManager;

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, ListenPort);
            var socketSettings = new SocketSettings(maxConnections, backlog, maxAcceptOps, bufferSize, localEndPoint, expireInterval, expireTime);
            listener = new SocketListener(socketSettings);
            listener.DataReceived += new ConnectionEventHandler(socketLintener_DataReceived);
            listener.Connected += new ConnectionEventHandler(socketLintener_Connected);
            listener.Disconnected += new ConnectionEventHandler(socketLintener_Disconnected);
            listener.StartListen();
            TraceLog.ReleaseWrite("TCP listent is started, The port:{0}.", ListenPort);

            timer = new Timer(Check, null, proxyCheckPeriod, proxyCheckPeriod);
        }

        private void Check(object state)
        {
            try
            {
                TraceLog.ReleaseWriteDebug("Tcp connect count：{0}", clientConnections.Count);

            }
            catch (Exception)
            {
            }
        }

        void socketLintener_Disconnected(object sender, ConnectionEventArgs e)
        {
            try
            {
                ClientConnection clientConnection;
                if (clientConnections.TryGetValue(e.Socket, out clientConnection))
                {
                    clientConnections.Remove(clientConnection.SSID);
                    TraceLog.ReleaseWriteDebug("断开 IP:{0},ssid:{1}", clientConnection.Socket.RemoteEndPoint, clientConnection.SSID);

                    if (clientConnection.ServerId != 0)
                    {
                        NameValueCollection requestParam = new NameValueCollection();
                        requestParam["actionid"] = ((int)ActionEnum.Interrupt).ToString();
                        requestParam["ssid"] = clientConnection.SSID.ToString("N");
                        requestParam["msgid"] = "0";
                        string paramStr = RequestParse.ToQueryString(requestParam);
                        byte[] paramData = Encoding.ASCII.GetBytes(paramStr);
                        try
                        {
                            gsConnectionManager.Send(clientConnection.GameId, clientConnection.ServerId, paramData);
                        }
                        catch (Exception ex)
                        {
                            TraceLog.WriteError("Send to tcp disconnected notify failed:{0}\r\nparam:{1}", ex, paramStr);
                        }
                    }
                }
                else
                {
                    TraceLog.ReleaseWriteDebug("断开 IP:{0}。", e.Socket.RemoteEndPoint);
                }
            }
            catch (Exception err)
            {
                TraceLog.WriteError("Disconnected:{0}", err);
            }
        }

        void socketLintener_Connected(object sender, ConnectionEventArgs e)
        {
            try
            {
                var ssid = Guid.NewGuid();
                var clientConnection = new ClientConnection { SSID = ssid, Socket = e.Socket };
                TraceLog.ReleaseWriteDebug("连接 IP:{0},ssid:{1}", e.Socket.RemoteEndPoint, ssid);
                clientConnections.Add(ssid, e.Socket, clientConnection);
            }
            catch (Exception)
            {
            }
        }

        void socketLintener_DataReceived(object sender, ConnectionEventArgs e)
        {
            var data = Encoding.ASCII.GetString(e.Data);
            try
            {

                string routeName = string.Empty;
                int index = data.LastIndexOf("?d=");
                if (index > 0)
                {
                    if (data.StartsWith("route:", StringComparison.CurrentCultureIgnoreCase))
                    {
                        routeName = data.Substring(6, index - 6);
                    }
                    data = data.Substring(index, data.Length - index);
                }
                data = HttpUtility.ParseQueryString(data)["d"];
                int gameId, serverId, statuscode;
                var ip = e.Socket.RemoteEndPoint.ToString().Split(new char[] { ':' })[0];
                var requestParam = RequestParse.Parse(ip, "", data, out gameId, out serverId, out statuscode);
                if (statuscode != (int)HttpStatusCode.OK)
                {// 接收到非法数据
                    listener.CloseSocket(e.Socket);
                    return;
                }
                ClientConnection clientConnection;
                if (!clientConnections.TryGetValue(e.Socket, out clientConnection))
                {
                    TraceLog.WriteError("接收到不在连接池中的socket数据，哪里有bug。");
                    listener.CloseSocket(e.Socket);
                    return;
                }

                if (clientConnection.GameId == 0) clientConnection.GameId = gameId;
                if (clientConnection.ServerId == 0) clientConnection.ServerId = serverId;

                requestParam["UserHostAddress"] = ip;
                requestParam["ssid"] = clientConnection.SSID.ToString("N");
                requestParam["http"] = "0";
                string paramStr = string.Format("{0}&UserHostAddress={1}&ssid={2}&http=0",
                    data,
                    ip,
                    requestParam["ssid"]);
                if (!string.IsNullOrEmpty(routeName))
                {
                    requestParam["route"] = routeName;
                    paramStr += "&route=" + routeName;
                }
                byte[] paramData = Encoding.ASCII.GetBytes(paramStr);

                try
                {
                    if (!gsConnectionManager.Send(gameId, serverId, paramData))
                    {
                        var responseData = RequestParse.CtorErrMsg(10000, RequestParse.ErrorMsgConnectFail, requestParam);
                        SendDataBack(clientConnection.SSID, responseData, 0, responseData.Length);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("无法连接游服error:{0}\r\nparam:{1}", ex, paramStr);
                    var responseData = RequestParse.CtorErrMsg(10000, RequestParse.ErrorMsgConnectFail, requestParam);
                    SendDataBack(clientConnection.SSID, responseData, 0, responseData.Length);
                }
            }
            catch (Exception err)
            {
                TraceLog.WriteError("DataReceived error:{0},param:{1}", err, data);
            }
        }

        public bool SendDataBack(Guid ssid, byte[] data, int offset, int count)
        {
            try
            {
                ClientConnection clientConnection;
                if (!clientConnections.TryGetValue(ssid, out clientConnection))
                {
                    return false;
                }
                ExSocket socket = clientConnection.Socket;
                listener.PostSend(socket, data, offset, count);
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SendDataBack error:{0}", ex);
                return false;
            }
        }
    }

    class ClientConnection
    {
        public int GameId { get; set; }
        public int ServerId { get; set; }
        public Guid SSID { get; set; }
        public ExSocket Socket { get; set; }
        public Timer Timer { get; set; }
    }
}
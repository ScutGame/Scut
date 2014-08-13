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
using System.IO;
using System.Net;
using System.Threading;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 游戏服Socket通讯宿主基类
    /// </summary>
    public abstract class GameSocketHost : GameBaseHost
    {
        private static int httpRequestTimeout = ConfigUtils.GetSetting("Game.Http.Timeout", "120000").ToInt();
        //private SmartThreadPool threadPool;
        private SocketListener socketLintener;
        private HttpListener httpListener;

        /// <summary>
        /// The enable http.
        /// </summary>
        protected bool EnableHttp;


        /// <summary>
        /// Action repeater
        /// </summary>
        public IActionDispatcher ActionDispatcher
        {
            get { return _setting == null ? null : _setting.ActionDispatcher; }
            set
            {
                if (_setting != null)
                {
                    _setting.ActionDispatcher = value;
                }
            }
        }

        private EnvironmentSetting _setting;
        /// <summary>
        /// 
        /// </summary>
        protected GameSocketHost()
        {
            _setting = GameEnvironment.Setting;
            int port = _setting != null ? _setting.GamePort : 0;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            int maxConnections = ConfigUtils.GetSetting("MaxConnections", 10000);
            int backlog = ConfigUtils.GetSetting("Backlog", 1000);
            int maxAcceptOps = ConfigUtils.GetSetting("MaxAcceptOps", 1000);
            int bufferSize = ConfigUtils.GetSetting("BufferSize", 8192);
            int expireInterval = ConfigUtils.GetSetting("ExpireInterval", 600) * 1000;
            int expireTime = ConfigUtils.GetSetting("ExpireTime", 3600) * 1000;

            //threadPool = new SmartThreadPool(180 * 1000, 100, 5);
            //threadPool.Start();

            var socketSettings = new SocketSettings(maxConnections, backlog, maxAcceptOps, bufferSize, localEndPoint, expireInterval, expireTime);
            socketLintener = new SocketListener(socketSettings);
            socketLintener.DataReceived += new ConnectionEventHandler(socketLintener_DataReceived);
            socketLintener.Connected += new ConnectionEventHandler(socketLintener_OnConnectCompleted);
            socketLintener.Disconnected += new ConnectionEventHandler(socketLintener_Disconnected);


            httpListener = new HttpListener();
            var httpHost = ConfigUtils.GetSetting("Game.Http.Host");
            var httpPort = ConfigUtils.GetSetting("Game.Http.Port", 80);
            var httpName = ConfigUtils.GetSetting("Game.Http.Name", "Service.aspx");

            if (!string.IsNullOrEmpty(httpHost))
            {
                EnableHttp = true;
                var hosts = httpHost.Split(',');
                foreach (var host in hosts)
                {
                    string address = host.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)
                                         ? host
                                         : "http://" + host;
                    httpListener.Prefixes.Add(string.Format("{0}:{1}/{2}/", address, httpPort, httpName));
                }
            }
        }

        private void socketLintener_OnConnectCompleted(object sender, ConnectionEventArgs e)
        {
            try
            {
                GameSession.CreateNew(e.Socket.HashCode, e.Socket, socketLintener.PostSend);
                OnConnectCompleted(sender, e);
            }
            catch (Exception err)
            {
                TraceLog.WriteError("ConnectCompleted error:{0}", err);
            }
        }

        private void socketLintener_Disconnected(object sender, ConnectionEventArgs e)
        {
            try
            {
                GameSession session = GameSession.Get(e.Socket.HashCode);
                if (session != null)
                {
                    OnDisconnected(session);
                    session.ProxySid = Guid.Empty;
                    session.Close();
                }
            }
            catch (Exception err)
            {
                TraceLog.WriteError("Disconnected error:{0}", err);
            }
        }

        private void socketLintener_DataReceived(object sender, ConnectionEventArgs e)
        {
            try
            {
                OnReceivedBefore(e);
                RequestPackage package;
                if (!ActionDispatcher.TryDecodePackage(e, out package))
                {
                    return;
                }
                var session = GetSession(e, package);
                if (CheckSpecialPackge(package, session))
                {
                    return;
                }
                package.Bind(session);
                ProcessPackage(package);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Received to Host:{0} error:{1}", e.Socket.RemoteEndPoint, ex);
            }
        }

        private bool CheckSpecialPackge(RequestPackage package, GameSession session)
        {
            //处理特殊包
            if (package.ActionId == ((int)ActionEnum.Interrupt))
            {
                //Proxy server notifly interrupt connect ops
                OnDisconnected(session);
                if (session != null && (session.ProxySid == Guid.Empty || GameSession.Count > 1))
                {
                    //保留代理服连接
                    session.Close();
                    session.ProxySid = Guid.Empty;
                }
                return true;
            }

            if (session != null && package.ActionId == ((int)ActionEnum.Heartbeat))
            {
                // 客户端tcp心跳包
                session.Refresh();
                OnHeartbeat(session);
                session.ExitSession();
                return true;
            }
            return false;
        }

        private GameSession GetSession(ConnectionEventArgs e, RequestPackage package)
        {
            //使用代理分发器时,每个ssid建立一个游服Serssion
            GameSession session;
            if (package.ProxySid != Guid.Empty)
            {
                session = GameSession.Get(package.ProxySid) ??
                          (package.IsProxyRequest
                              ? GameSession.Get(e.Socket.HashCode)
                              : GameSession.CreateNew(package.ProxySid, e.Socket, socketLintener.PostSend));
                if (session != null)
                {
                    session.ProxySid = package.ProxySid;
                }
            }
            else
            {
                session = GameSession.Get(package.SessionId) ?? GameSession.Get(e.Socket.HashCode);
            }
            if (session != null && !session.Connected)
            {
                GameSession.Recover(session, e.Socket.HashCode, e.Socket, socketLintener.PostSend);
            }
            return session;
        }

        /// <summary>
        /// Raises the received before event.
        /// </summary>
        /// <param name="e">E.</param>
        protected virtual void OnReceivedBefore(ConnectionEventArgs e)
        {
        }

        private void ProcessPackage(RequestPackage package)
        {
            if (package == null) return;

            var session = package.Session;
            try
            {
                byte[] data = new byte[0];
                if (!string.IsNullOrEmpty(package.RouteName))
                {
                    ActionGetter actionGetter = ActionDispatcher.GetActionGetter(package);
                    if (CheckRemote(package.RouteName, actionGetter))
                    {
                        MessageStructure response = new MessageStructure();
                        OnCallRemote(package.RouteName, actionGetter, response);
                        data = response.PopBuffer();
                    }
                }
                else
                {
                    SocketGameResponse response = new SocketGameResponse();
                    response.WriteErrorCallback += ActionDispatcher.ResponseError;
                    ActionGetter actionGetter = ActionDispatcher.GetActionGetter(package);
                    DoAction(actionGetter, response);
                    data = response.ReadByte();
                }
                try
                {
                    if (data.Length > 0)
                    {
                        session.SendAsync(data, 0, data.Length);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("PostSend异常{0}", ex);
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Task异常{0}", ex);
            }
            finally
            {
                session.ExitSession();
            }
        }

        /// <summary>
        /// 心跳包
        /// </summary>
        /// <param name="session"></param>
        protected virtual void OnHeartbeat(GameSession session)
        {
        }


        /// <summary>
        /// Raises the connect completed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected virtual void OnConnectCompleted(object sender, ConnectionEventArgs e)
        {

        }
        /// <summary>
        /// Raises the disconnected event.
        /// </summary>
        /// <param name="session">Session.</param>
        protected virtual void OnDisconnected(GameSession session)
        {

        }

        private void OnHttpRequest(IAsyncResult result)
        {
            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                HttpListenerContext context = listener.EndGetContext(result);
                listener.BeginGetContext(OnHttpRequest, listener);

                RequestPackage package;
                if (!ActionDispatcher.TryDecodePackage(context, out package))
                {
                    return;
                }

                GameSession session;
                if (package.ProxySid != Guid.Empty)
                {
                    session = GameSession.Get(package.ProxySid) ?? GameSession.CreateNew(package.ProxySid, context.Request);
                    session.ProxySid = package.ProxySid;
                }
                else
                {
                    session = GameSession.Get(package.SessionId) ?? GameSession.CreateNew(Guid.NewGuid(), context.Request);
                }
                package.Bind(session);

                ActionGetter httpGet = ActionDispatcher.GetActionGetter(package);
                if (package.IsUrlParam)
                {
                    httpGet["UserHostAddress"] = session.EndAddress;
                    httpGet["ssid"] = session.KeyCode.ToString("N");
                    httpGet["http"] = "1";
                }

                var httpresponse = new SocketGameResponse();
                httpresponse.WriteErrorCallback += new ScutActionDispatcher().ResponseError;

                var clientConnection = new HttpClientConnection
                {
                    Context = context,
                    Session = session,
                    ActionGetter = httpGet,
                    GameResponse = httpresponse
                };
                clientConnection.TimeoutTimer = new Timer(OnHttpRequestTimeout, clientConnection, httpRequestTimeout, Timeout.Infinite);
                byte[] respData = new byte[0];
                if (!string.IsNullOrEmpty(package.RouteName))
                {
                    if (CheckRemote(package.RouteName, httpGet))
                    {
                        MessageStructure response = new MessageStructure();
                        OnCallRemote(package.RouteName, httpGet, response);
                        respData = response.PopBuffer();
                    }
                }
                else
                {
                    DoAction(httpGet, httpresponse);
                    respData = httpresponse.ReadByte();
                }
                OnHttpResponse(clientConnection, respData, 0, respData.Length);

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnHttpRequest error:{0}", ex);
            }
        }

        private void OnHttpRequestTimeout(object state)
        {
            try
            {
                HttpClientConnection clientConnection = (HttpClientConnection)state;
                if (clientConnection == null) return;
                var actionGetter = clientConnection.ActionGetter;
                clientConnection.GameResponse.WriteError(actionGetter, Language.Instance.ErrorCode, "Request Timeout.");
                byte[] respData = clientConnection.GameResponse.ReadByte();
                OnHttpResponse(clientConnection, respData, 0, respData.Length);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnHttpRequestTimeout:{0}", ex);
            }
        }

        private void OnHttpResponse(HttpClientConnection connection, byte[] data, int offset, int count)
        {
            try
            {
                connection.TimeoutTimer.Dispose();
                HttpListenerResponse response = connection.Context.Response;
                response.ContentType = "application/octet-stream";
                if (data[offset] == 0x1f && data[offset + 1] == 0x8b && data[offset + 2] == 0x08 && data[offset + 3] == 0x00)
                {
                    response.AddHeader("Content-Encoding", "gzip");
                }
                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.ContentLength64 = count;
                Stream output = response.OutputStream;
                output.Write(data, offset, count);
                output.Close();
                connection.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start(string[] args)
        {
            socketLintener.StartListen();
            if (EnableHttp)
            {
                httpListener.Start();
                httpListener.BeginGetContext(OnHttpRequest, httpListener);
            }
            EntitySyncManger.SendHandle += (userId, data) =>
            {
                GameSession session = GameSession.Get(userId);
                if (session != null)
                {
                    return session.SendAsync(data, 0, data.Length);
                }
                return false;
            };
            OnStartAffer();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            if (EnableHttp)
            {
                httpListener.Stop();
            }
            socketLintener.Close();
            OnServiceStop();
            try
            {
                //threadPool.Dispose();
                EntitySyncManger.Dispose();
                //threadPool = null;
            }
            catch
            {
            }
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Raises the start affer event.
        /// </summary>
        protected abstract void OnStartAffer();
        /// <summary>
        /// Raises the service stop event.
        /// </summary>
        protected abstract void OnServiceStop();


        private class HttpClientConnection
        {
            public GameSession Session;
            public HttpListenerContext Context;
            public Timer TimeoutTimer;
            public ActionGetter ActionGetter;
            public SocketGameResponse GameResponse;
            public void Close()
            {
            }
        }
    }
}
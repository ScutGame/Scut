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
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Reflect;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Net;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets.Threading;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 游戏服Socket通讯宿主基类
    /// </summary>
    public abstract class GameSocketHost
    {
        private static int httpRequestTimeout = ConfigUtils.GetSetting("Game.Http.Timeout", "120000").ToInt();
        private ConcurrentQueue<RequestPackage> requestQueue = new ConcurrentQueue<RequestPackage>();
        private ConcurrentQueue<RequestPackage> lockedQueue = new ConcurrentQueue<RequestPackage>();

        private ManualResetEvent singal = new ManualResetEvent(false);
        private Thread queueProcessThread;
        private SmartThreadPool threadPool;
        private SocketListener socketLintener;
        private HttpListener httpListener;
        /// <summary>
        /// The enable http.
        /// </summary>
        protected bool EnableHttp;
        /// <summary>
        /// The receive number.
        /// </summary>
        internal protected int receiveNum;
        /// <summary>
        /// The error drop number.
        /// </summary>
        internal protected int errorDropNum;
        /// <summary>
        /// The timeout drop number.
        /// </summary>
        internal int timeoutDropNum = 0;
        /// <summary>
        /// The running number.
        /// </summary>
        internal protected int runningNum;
        private Timer _LockedQueueChecker;
        /// <summary>
        /// is running process queue.
        /// </summary>
        private int _runningQueue;

        /// <summary>
        /// Gets the blocking number.
        /// </summary>
        /// <value>The blocking number.</value>
        internal protected int blockingNum { get { return lockedQueue.Count; } }

        /// <summary>
        /// 
        /// </summary>
        protected GameSocketHost()
        {
            int port = GameEnvironment.Setting.GamePort;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            int maxConnections = ConfigUtils.GetSetting("MaxConnections", 10000);
            int backlog = ConfigUtils.GetSetting("Backlog", 1000);
            int maxAcceptOps = ConfigUtils.GetSetting("MaxAcceptOps", 1000);
            int bufferSize = ConfigUtils.GetSetting("BufferSize", 8192);
            int expireInterval = ConfigUtils.GetSetting("ExpireInterval", 600) * 1000;
            int expireTime = ConfigUtils.GetSetting("ExpireTime", 3600) * 1000;

            threadPool = new SmartThreadPool(180 * 1000, 100, 5);
            threadPool.Start();

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
            Interlocked.Exchange(ref _runningQueue, 1);
            queueProcessThread = new Thread(ProcessQueue);
            queueProcessThread.Start();
            _LockedQueueChecker = new Timer(LockedQueueChecker, null, 100, 100);
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
                Interlocked.Increment(ref receiveNum);
                OnReceivedBefore(e);
                //if (isInStopping) return;
                Dictionary<string, string> param = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                string paramStr = Encoding.ASCII.GetString(e.Data);
                int index = paramStr.IndexOf("?d=");
                string routeName = string.Empty;
                if (index != -1)
                {
                    if (paramStr.StartsWith("route:", StringComparison.CurrentCultureIgnoreCase))
                    {
                        routeName = paramStr.Substring(6, index - 6);
                    }
                    paramStr = paramStr.Substring(index, paramStr.Length - index);
                    paramStr = HttpUtility.ParseQueryString(paramStr)["d"];
                }

                var nvc = HttpUtility.ParseQueryString(paramStr);
                foreach (var key in nvc.AllKeys)
                {
                    param[key] = nvc[key];
                }
                if (param.ContainsKey("route"))
                {
                    routeName = param["route"];
                }
                //if (!param.ContainsKey("ssid")) { Interlocked.Increment(ref errorDropNum); return; }
                if (!param.ContainsKey("actionid")) { Interlocked.Increment(ref errorDropNum); return; }
                if (!param.ContainsKey("msgid")) { Interlocked.Increment(ref errorDropNum); return; }

                //sessionId of proxy server
                Guid proxySid;
                if (!param.ContainsKey("ssid") || !Guid.TryParse(param["ssid"], out proxySid))
                {
                    proxySid = Guid.Empty;
                }
                int actionid;
                if (!int.TryParse(param["actionid"], out actionid)) { Interlocked.Increment(ref errorDropNum); return; }
                int msgid;
                if (!int.TryParse(param["msgid"], out msgid)) { Interlocked.Increment(ref errorDropNum); return; }
                bool isproxy = param.ContainsKey("isproxy");//proxy server send
                string sessionId = param.ContainsKey("sid") ? param["sid"] : "";

                //使用代理分发器时,每个ssid建立一个游服Serssion
                GameSession session;
                if (proxySid != Guid.Empty)
                {
                    session = GameSession.Get(proxySid) ??
                               (isproxy
                                ? GameSession.Get(e.Socket.HashCode)
                                : GameSession.CreateNew(proxySid, e.Socket, socketLintener.PostSend));
                    if (session != null)
                    {
                        session.ProxySid = proxySid;
                    }
                }
                else
                {
                    session = GameSession.Get(e.Socket.HashCode);
                }
                if (session != null && !session.Connected)
                {
                    GameSession.Recover(session, e.Socket.HashCode, e.Socket, socketLintener.PostSend);
                }
                if (actionid == (int)ActionEnum.Interrupt)
                {
                    //Proxy server notifly interrupt connect ops
                    OnDisconnected(session);
                    if (session != null && (session.ProxySid == Guid.Empty || GameSession.Count > 1))
                    {
                        //保留代理服连接
                        session.Close();
                        session.ProxySid = Guid.Empty;
                    }
                    return;
                }


                requestQueue.Enqueue(new RequestPackage
                                         {
                                             Route = routeName,
                                             ActionId = actionid,
                                             Param = paramStr,
                                             Session = session,
                                             ReceiveTime = DateTime.Now
                                         });
                singal.Set();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Received to Host:{0} error:{1}", e.Socket.RemoteEndPoint, ex);
            }
        }

        /// <summary>
        /// Raises the received before event.
        /// </summary>
        /// <param name="e">E.</param>
        protected virtual void OnReceivedBefore(ConnectionEventArgs e)
        {
        }

        void ProcessQueue(object state)
        {
            try
            {
                while (_runningQueue == 1)
                {
                    singal.WaitOne();
                    while (_runningQueue == 1)
                    {
                        try
                        {
                            RequestPackage package;
                            if (requestQueue.TryDequeue(out package))
                            {
                                if (package.Session == null || !package.Session.EnterSession()) lockedQueue.Enqueue(package);
                                else
                                {
                                    Interlocked.Increment(ref runningNum);
                                    threadPool.QueueWorkItem(ProcessPackage, package);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            TraceLog.WriteError("ProcessQueue request error:{0}", ex);
                        }
                    }
                    //if (isInStopping) break;
                    singal.Reset();
                }
            }
            catch (Exception er)
            {
                TraceLog.WriteError("ProcessQueue error:{0}", er);
            }
        }

        void LockedQueueChecker(object state)
        {
            try
            {
                RequestPackage package;
                while (lockedQueue.TryDequeue(out package))
                {
                    if (MathUtils.Now.Subtract(package.ReceiveTime).TotalSeconds >= 30)
                    {
                        Interlocked.Increment(ref timeoutDropNum);
                        continue;
                    }
                    requestQueue.Enqueue(package);
                    singal.Set();
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("LockedQueue error:{0}", ex);
            }
        }

        private void ProcessPackage(object state)
        {
            var package = (RequestPackage)state;
            if (package == null) return;

            var param = package.Param;
            var session = package.Session;
            var actionid = package.ActionId;
            try
            {
                if (actionid == (int)ActionEnum.Heartbeat)
                {
                    // 客户端tcp心跳包
                    session.LastActivityTime = DateTime.Now;
                    OnHeartbeat(session);
                    session.ExitSession();
                    Interlocked.Decrement(ref runningNum);
                    return;
                }

                HttpGet httpGet = new HttpGet(param, session);
                byte[] data = new byte[0];
                if (!string.IsNullOrEmpty(package.Route))
                {
                    if (CheckRemote(package.Route, httpGet))
                    {
                        MessageStructure response = new MessageStructure();
                        OnCallRemote(package.Route, httpGet, response);
                        data = response.PopBuffer();
                    }
                }
                else
                {
                    SocketGameResponse response = new SocketGameResponse();
                    OnRequested(httpGet, response);
                    data = response.ReadByte();
                }
                try
                {
                    session.SendAsync(data, 0, data.Length);
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
                Interlocked.Decrement(ref runningNum);
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
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string sid = request.QueryString["sid"];
                GameSession session;
                if (string.IsNullOrEmpty(sid))
                {
                    session = GameSession.CreateNew(Guid.NewGuid(), request);
                }
                else
                {
                    session = GameSession.Get(sid) ?? GameSession.CreateNew(Guid.NewGuid(), request);
                }

                string data = "";
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    if (string.Compare(request.HttpMethod, "get", true) == 0)
                    {
                        data = request.RawUrl.Substring(8);
                        data = HttpUtility.UrlDecode(data);
                    }
                }
                else
                {
                    data = request.QueryString["d"];
                }

                if (string.IsNullOrEmpty(data))
                {
                    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        data = reader.ReadToEnd();
                        data = HttpUtility.ParseQueryString(data)["d"];
                    }
                }

                int statuscode = 0;
                Dictionary<string, string> param = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                if (data != null)
                {
                    var nvc = HttpUtility.ParseQueryString(data);
                    foreach (var key in nvc.AllKeys)
                    {
                        param[key] = nvc[key];
                    }
                }
                statuscode = CheckHttpParam(param);

                if (statuscode != (int)HttpStatusCode.OK)
                {
                    response.StatusCode = statuscode;
                    response.Close();
                    return;
                }

                var clientConnection = new HttpClientConnection { Context = context, Session = session, Param = param };
                clientConnection.TimeoutTimer = new Timer(OnHttpRequestTimeout, clientConnection, httpRequestTimeout, Timeout.Infinite);
                HttpGet httpGet = new HttpGet(data, session);
                httpGet["UserHostAddress"] = session.EndAddress;
                httpGet["ssid"] = session.KeyCode.ToString("N");
                httpGet["http"] = "1";
                SocketGameResponse httpresponse = new SocketGameResponse();
                OnRequested(httpGet, httpresponse);
                byte[] respData = httpresponse.ReadByte();
                OnHttpResponse(clientConnection, respData, 0, respData.Length);

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnHttpRequest error:{0}", ex);
            }
        }

        private int CheckHttpParam(Dictionary<string, string> param)
        {
            if (!param.ContainsKey("actionid"))
            {
                return (int)HttpStatusCode.BadRequest;
            }
            if (!param.ContainsKey("msgid"))
            {
                return (int)HttpStatusCode.BadRequest;
            }
            return (int)HttpStatusCode.OK;
        }

        private void OnHttpRequestTimeout(object state)
        {
            try
            {
                HttpClientConnection clientConnection = (HttpClientConnection)state;
                if (clientConnection == null) return;

                int actionId = 0;
                string action;
                if (clientConnection.Param.TryGetValue("ActionId", out action))
                {
                    actionId = action.ToInt();
                }

                var httpresponse = new SocketGameResponse();
                ActionFactory.RequestError(httpresponse, actionId, "Request Timeout.");
                byte[] respData = httpresponse.ReadByte();
                OnHttpResponse(clientConnection, respData, 0, respData.Length);
            }
            catch(Exception ex)
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
        public void Start(string[] args)
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
        public void Stop()
        {
            Interlocked.Exchange(ref _runningQueue, 0);
            if (EnableHttp)
            {
                httpListener.Stop();
            }
            socketLintener.Close();
            OnServiceStop();
            try
            {
                singal.Set();
                threadPool.Dispose();
                queueProcessThread.Abort();
                _LockedQueueChecker.Dispose();
                singal.Dispose();
                EntitySyncManger.Dispose();

                threadPool = null;
                queueProcessThread = null;
                _LockedQueueChecker = null;
                singal = null;
            }
            catch
            {
            }
            GC.SuppressFinalize(this);
        }

        private delegate void RemoteHandle(HttpGet httpGet, MessageHead head, MessageStructure writer);

        private void OnCallRemote(string route, HttpGet httpGet, MessageStructure response)
        {
            try
            {
                string[] mapList = route.Split('.');
                string funcName = "";
                string routeName = "";
                if (mapList.Length > 1)
                {
                    funcName = mapList[mapList.Length - 1];
                    routeName = string.Join("/", mapList, 0, mapList.Length - 1);
                }
                string routeFile = "";
                string typeName = string.Format("Game.Script.Remote.{0}", routeName);
                int actionId = httpGet.ActionId;
                MessageHead head = new MessageHead(actionId);
                if (!ScriptEngines.DisablePython)
                {
                    routeFile = string.Format("{0}/Remote/{1}.py", ScriptEngines.PythonDirName, routeName);
                    dynamic scope = ScriptEngines.Execute(routeFile, typeName);
                    if (scope != null)
                    {
                        var funcHandle = scope.GetVariable<RemoteHandle>(funcName);
                        if (funcHandle != null)
                        {
                            funcHandle(httpGet, head, response);
                            response.WriteBuffer(head);
                            return;
                        }
                    }
                }
                routeFile = string.Format("{0}/Remote/{1}.cs", ScriptEngines.CSharpDirName, routeName);
                var instance = (object)ScriptEngines.Execute(routeFile, typeName);
                if (instance != null)
                {
                    var result = ObjectAccessor.Create(instance, true)[funcName];
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("{0}", ex);
            }
        }

        /// <summary>
        /// Checks the remote.
        /// </summary>
        /// <returns><c>true</c>, if remote was checked, <c>false</c> otherwise.</returns>
        /// <param name="route">Route.</param>
        /// <param name="httpGet">Http get.</param>
        protected virtual bool CheckRemote(string route, HttpGet httpGet)
        {
            return true;
        }

        /// <summary>
        /// Raises the requested event.
        /// </summary>
        /// <param name="httpGet">Http get.</param>
        /// <param name="response">Response.</param>
        protected virtual void OnRequested(HttpGet httpGet, IGameResponse response)
        {
            if (GameEnvironment.IsRunning)
            {
                //todo trace
#if DEBUG 
                ActionFactory.RequestError(response, httpGet.ActionId, 0, "");
#else
                ActionFactory.Request(httpGet, response, GetUser);
#endif
            }
            else
            {
                ActionFactory.RequestError(response, httpGet.ActionId, Language.Instance.ServerMaintain);
            }
        }

        /// <summary>
        /// Get user object by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected abstract BaseUser GetUser(int userId);

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
            public Dictionary<string, string> Param;

            public void Close()
            {
            }
        }
    }
}
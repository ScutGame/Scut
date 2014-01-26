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
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Net;
using ZyGames.Framework.RPC.Sockets;
using NLog;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets.Threading;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Contract
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
    class RequestPackage
    {
        public string Route { get; set; }
        public int ActionId { get; set; }
        public string Param { get; set; }
        public GameSession Session { get; set; }
        public Guid SSID { get; set; }
        public DateTime ReceiveTime { get; set; }
    }

    /// <summary>
    /// 游戏服Socket通讯宿主基类
    /// </summary>
    public abstract class GameSocketHost
    {
        private static int httpRequestTimeout = ConfigUtils.GetSetting("Game.Http.Timeout", "120000").ToInt();
        private ConcurrentQueue<RequestPackage> requestQueue = new ConcurrentQueue<RequestPackage>();
        private ConcurrentQueue<RequestPackage> lockedQueue = new ConcurrentQueue<RequestPackage>();
        private ConcurrentDictionary<Guid, HttpClientConnection> httpRequestPool = new ConcurrentDictionary<Guid, HttpClientConnection>();
        internal ConcurrentDictionary<string, GameSession> globalSessions = new ConcurrentDictionary<string, GameSession>();
        internal ConcurrentDictionary<long, string> globalUid2SidMap = new ConcurrentDictionary<long, string>();

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
        /// Gets the blocking number.
        /// </summary>
        /// <value>The blocking number.</value>
        internal protected int blockingNum { get { return lockedQueue.Count; } }

        /// <summary>
        /// 
        /// </summary>
        protected GameSocketHost()
        {
            int port = ActionFactory.ActionConfig.Current.Port;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            int maxConnections = ConfigUtils.GetSetting("MaxConnections", 10000);
            int backlog = ConfigUtils.GetSetting("Backlog", 1000);
            int maxAcceptOps = ConfigUtils.GetSetting("MaxAcceptOps", 1000);
            int bufferSize = ConfigUtils.GetSetting("BufferSize", 8192);

            threadPool = new SmartThreadPool(180 * 1000, 100, 5);
            threadPool.Start();

            var socketSettings = new SocketSettings(maxConnections, backlog, maxAcceptOps, bufferSize, localEndPoint, 3600000, 3600000);
            socketLintener = new SocketListener(socketSettings);
            socketLintener.DataReceived += new ConnectionEventHandler(socketLintener_DataReceived);
            socketLintener.Connected += new ConnectionEventHandler(OnConnectCompleted);
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
                    httpListener.Prefixes.Add(string.Format("{0}:{1}/{2}/", host, httpPort, httpName));
                }
            }
            queueProcessThread = new Thread(ProcessQueue);
            queueProcessThread.Start();
            _LockedQueueChecker = new Timer(LockedQueueChecker, null, 100, 100);
        }

        void socketLintener_Disconnected(object sender, ConnectionEventArgs e)
        {
        }

        void socketLintener_DataReceived(object sender, ConnectionEventArgs e)
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

                Guid ssid;
                if (!param.ContainsKey("ssid") || !Guid.TryParse(param["ssid"], out ssid))
                {
                    ssid = Guid.Empty;
                }
                int actionid;
                if (!int.TryParse(param["actionid"], out actionid)) { Interlocked.Increment(ref errorDropNum); return; }
                int msgid;
                if (!int.TryParse(param["msgid"], out msgid)) { Interlocked.Increment(ref errorDropNum); return; }

                GameSession session = null;
                bool hasSession = false;
                if (param.ContainsKey("sid") && !string.IsNullOrEmpty(param["sid"]))
                {
                    if (globalSessions.TryGetValue(param["sid"], out session))
                    {
                        session.Channel = e.Socket;
                        hasSession = true;
                    }
                }
                if (actionid == (int)ActionEnum.Interrupt)
                {
                    // 客户端tcp连接断开
                    foreach (var s in globalSessions.Values)
                    {
                        if (s.SSId == ssid)
                        {
                            OnDisconnected(s);
                            s.SSId = Guid.Empty;
                            s.LastActivityTime = DateTime.Now;
                            break;
                        }
                    }
                    return;
                }

                if (!hasSession)
                {
                    session = new GameSession();
                    session.SessionId = GenerateSid();
                }

                session.SSId = ssid;
                session.Channel = e.Socket;

                requestQueue.Enqueue(new RequestPackage
                                         {
                                             Route = routeName,
                                             ActionId = actionid,
                                             Param = paramStr,
                                             Session = session,
                                             SSID = ssid,
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
            while (true)
            {
                singal.WaitOne();
                while (true)
                {
                    RequestPackage package;
                    if (requestQueue.TryDequeue(out package))
                    {
                        if (!package.Session.EnterSession()) lockedQueue.Enqueue(package);
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
                //if (isInStopping) break;
                singal.Reset();
            }
        }

        void LockedQueueChecker(object state)
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

        private void ProcessPackage(object state)
        {
            var package = (RequestPackage)state;
            var param = package.Param;
            var session = package.Session;
            var ssid = package.SSID;
            var actionid = package.ActionId;
            if (actionid == (int)ActionEnum.Heartbeat)
            {
                // 客户端tcp心跳包
                session.LastActivityTime = DateTime.Now;
                OnHeartbeat(session);
                session.ExitSession();
                Interlocked.Decrement(ref runningNum);
                return;
            }

            try
            {
                HttpGet httpGet = new HttpGet(param, session.SessionId, session.Channel.RemoteEndPoint.ToString(), LoginSuccessCallback, session);
                byte[] data = new byte[0];
                if (!string.IsNullOrEmpty(package.Route))
                {
                    if (CheckRemote(package.Route, httpGet))
                    {
                        MessageStructure response = new MessageStructure();
                        OnCallRemote(package.Route, httpGet, response);
                        data = response.ReadBuffer();
                    }
                }
                else
                {
                    SocketGameResponse response = new SocketGameResponse();
                    OnRequested(httpGet, response);
                    data = response.ReadByte();
                }
                ExSocket channel = session.Channel;
                byte[] buffer = CheckAdditionalHead(data, ssid);
                try
                {
                    socketLintener.PostSend(channel, buffer, 0, buffer.Length);
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
        /// 检查加头16位ssid
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ssid"></param>
        /// <returns></returns>
        private byte[] CheckAdditionalHead(byte[] data, Guid ssid)
        {
            if (ssid == Guid.Empty)
            {
                return data;
            }
            var buffer = new byte[data.Length + 16];
            Buffer.BlockCopy(ssid.ToByteArray(), 0, buffer, 0, 16);
            Buffer.BlockCopy(data, 0, buffer, 16, data.Length);
            return buffer;
        }

        private void LoginSuccessCallback(object obj, int userId)
        {
            var session = obj as GameSession;
            session.UserId = userId;
            string old_sid;
            if (globalUid2SidMap.TryGetValue(userId, out old_sid))
            {
                if (old_sid != session.SessionId)
                {
                    //旧的session被踢下线
                    GameSession old_session;
                    if (globalSessions.TryRemove(old_sid, out old_session))
                    {
                    }
                }
            }
            globalUid2SidMap[userId] = session.SessionId;
            globalSessions[session.SessionId] = session;
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

                var ssid = Guid.NewGuid();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

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

                var clientConnection = new HttpClientConnection { Context = context, SSID = ssid, Param = param };
                clientConnection.TimeoutTimer = new Timer(OnHttpRequestTimeout, clientConnection, httpRequestTimeout, Timeout.Infinite);
                httpRequestPool[ssid] = clientConnection;

                string hostAddress = request.RemoteEndPoint == null ? "" : request.RemoteEndPoint.Address.ToString();
                HttpGet httpGet = new HttpGet(data, GenerateSid(), hostAddress);
                httpGet["UserHostAddress"] = hostAddress;
                httpGet["ssid"] = ssid.ToString("N");
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
            catch
            {

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
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            socketLintener.StartListen();
            if (EnableHttp)
            {
                httpListener.Start();
                httpListener.BeginGetContext(OnHttpRequest, httpListener);
            }
            EntitySyncManger.SendHandle += SendAsync;
            OnStartAffer();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (EnableHttp)
            {
                httpListener.Stop();
            }
            OnServiceStop();
        }

        /// <summary>
        /// 将指定的Action结果广播给指定范围的玩家
        /// </summary>
        /// <typeparam name="T">BaseUser对象</typeparam>
        /// <param name="actionId">指定的Action</param>
        /// <param name="userList">指定范围的玩家</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="successHandle">成功回调</param>
        public void BroadcastAction<T>(int actionId, List<T> userList, Parameters parameters, Action<T> successHandle) where T : BaseUser
        {
            StringBuilder shareParam = new StringBuilder();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    shareParam.AppendFormat("&{0}={1}", parameter.Key, parameter.Value);
                }
            }
            HttpGet httpGet;
            byte[] sendData = ActionFactory.GetActionResponse(actionId, null, shareParam.ToString(), out httpGet);
            foreach (var user in userList)
            {
                if (user == default(T))
                {
                    continue;
                }
                try
                {
                    GameSession session;
                    if (globalSessions.TryGetValue(user.SocketSid, out session))
                    {
                        if (SendAsync(session, sendData))
                        {
                            if (successHandle != null) successHandle(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("BroadcastAction  action:{0} userId:{1} error:{2}", actionId, user.PersonalId, ex);
                }
            }
        }

        /// <summary>
        /// 给指定范围的每个玩家发送指定的Action结果
        /// </summary>
        /// <typeparam name="T">BaseUser对象</typeparam>
        /// <param name="userList">指定范围的玩家</param>
        /// <param name="actionId">指定的Action</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="successHandle">成功回调</param>
        public void SendAsyncAction<T>(List<T> userList, int actionId, Parameters parameters, Action<HttpGet> successHandle) where T : BaseUser
        {
            StringBuilder shareParam = new StringBuilder();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    shareParam.AppendFormat("&{0}={1}", parameter.Key, parameter.Value);
                }
            }
            foreach (var user in userList)
            {
                if (user == default(T))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(user.SocketSid))
                {
                    TraceLog.WriteError("SendToClient action:{0} userId:{1} ssid is empty.", actionId, user.PersonalId);
                    continue;
                }
                try
                {
                    GameSession session;
                    if (globalSessions.TryGetValue(user.SocketSid, out session))
                    {
                        HttpGet httpGet;
                        byte[] sendData = ActionFactory.GetActionResponse(actionId, user, shareParam.ToString(), out httpGet);

                        SendAsync(session, sendData);
                        if (httpGet != null)
                        {
                            successHandle(httpGet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("SendToClient action:{0} userId:{1} error:{2}", actionId, user.PersonalId, ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SendAsync(int userId, byte[] data)
        {
            string sessionId;
            if (globalUid2SidMap.TryGetValue(userId, out sessionId))
            {
                GameSession session;
                if (globalSessions.TryGetValue(sessionId, out session))
                {
                    return SendAsync(session, data);
                }
            }
            return false;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="data"></param>
        public bool SendAsync(string sessionId, byte[] data)
        {
            GameSession session;
            if (globalSessions.TryGetValue(sessionId, out session))
            {
                return SendAsync(session, data);
            }
            return false;
        }

        private bool SendAsync(GameSession session, byte[] data)
        {
            if (session.Connected)
            {
                data = CheckAdditionalHead(data, session.SSId);
                socketLintener.PostSend(session.Channel, data, 0, data.Length);
                return true;
            }
            return false;
        }

        private string GenerateSid()
        {
            return string.Format("s_{0}|{1}|{2}", Guid.NewGuid().ToString("N"), GameEnvironment.ProductCode, GameEnvironment.ProductServerId);
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
                string routeFile = string.Format("Remote/{0}.py", routeName);
                string typeName = string.Format("Game.Script.Remote.{0}", routeName);
                int actionId = httpGet.GetInt("actionId");
                MessageHead head = new MessageHead(actionId);
                dynamic scope = ScriptEngines.Execute(routeFile, typeName);
                if (scope != null)
                {
                    var funcHandle = scope.GetVariable<RemoteHandle>(funcName);
                    if (funcHandle != null)
                    {
                        funcHandle(httpGet, head, response);
                        response.WriteBuffer(head);
                    }
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
                ActionFactory.Request(httpGet, response, GetUser);
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
            public Guid SSID;
            public HttpListenerContext Context;
            public Timer TimeoutTimer;
            public Dictionary<string, string> Param;
        }
    }


}
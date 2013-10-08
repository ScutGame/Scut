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
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using NetLibrary.Threading;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Net;
using ZyGames.Framework.NetLibrary;
using NLog;

namespace ZyGames.Framework.Game.Contract
{

    class RequestPackage
    {
        public Dictionary<string, string> Param { get; set; }
        public GameSession Session { get; set; }
        public Guid SSID { get; set; }
        public DateTime ReceiveTime { get; set; }
    }

    public abstract class GameSocketHost
    {
        private static readonly Logger Logger = LogManager.GetLogger("GameSocketHost");
        ConcurrentQueue<RequestPackage> requestQueue = new ConcurrentQueue<RequestPackage>();
        ConcurrentQueue<RequestPackage> lockedQueue = new ConcurrentQueue<RequestPackage>();
        ManualResetEvent singal = new ManualResetEvent(false);
        private SocketListener socketLintener;
        Thread queueProcessThread;

        private ConcurrentDictionary<ExSocket, HashSet<Guid>> clientSocketMap = new ConcurrentDictionary<ExSocket, HashSet<Guid>>();
        internal ConcurrentDictionary<string, GameSession> globalSessions = new ConcurrentDictionary<string, GameSession>();
        internal ConcurrentDictionary<long, string> globalUid2SidMap = new ConcurrentDictionary<long, string>();
        private SmartThreadPool threadPool ;

        internal int receiveNum;
        internal int errorDropNum;
        internal int timeoutDropNum;
        internal int runningNum;
        internal int blockingNum { get { return lockedQueue.Count; } }

        void socketLintener_DataReceived(object sender, ConnectionEventArgs e)
        {
            Interlocked.Increment(ref receiveNum);
            //if (isInStopping) return;
            Dictionary<string, string> param = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            var nvc = HttpUtility.ParseQueryString(Encoding.ASCII.GetString(e.Data));
            foreach (var key in nvc.AllKeys)
            {
                param[key] = nvc[key];
            }

            if (!param.ContainsKey("ssid")) { Interlocked.Increment(ref errorDropNum); return; }
            if (!param.ContainsKey("actionid")) { Interlocked.Increment(ref errorDropNum); return; }
            if (!param.ContainsKey("msgid")) { Interlocked.Increment(ref errorDropNum); return; }

            Guid ssid;
            if (!Guid.TryParse(param["ssid"], out ssid)) { Interlocked.Increment(ref errorDropNum); return; }
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

            if (actionid == 2)
            {// 客户端tcp连接断开
                foreach (var s in globalSessions.Values)
                {
                    if (s.SSId == ssid)
                    {
                        s.SSId = Guid.Empty;
                        s.LastActivityTime = DateTime.Now;
                        break;
                    }
                }
                return;
            }

            if (!hasSession)
            {
                //if (actionid == 1000 || actionid == 1002)
                //{
                //    string userName91;
                //    if (!param.TryGetValue("UserName91", out userName91)) { Interlocked.Increment(ref errorDropNum); return; }
                //    do
                //    {
                //        if (tmpSessionDict.TryGetValue(userName91, out session)) { Interlocked.Increment(ref errorDropNum); return; }
                //        session = new GameSession();
                //        session.SessionId = GenerateSid();
                //        if (tmpSessionDict.TryAdd(userName91, session)) break;
                //    } while (true);
                //}
                //else
                //{
                session = new GameSession();
                session.SessionId = GenerateSid();
                //}
            }

            session.SSId = ssid;
            session.Channel = e.Socket;

            requestQueue.Enqueue(new RequestPackage { Param = param, Session = session, SSID = ssid, ReceiveTime = DateTime.Now });
            singal.Set();
        }

        private void ProcessPackage(object state)
        {
            var package = (RequestPackage)state;
            var param = package.Param;
            var session = package.Session;
            var ssid = package.SSID;
            var actionid = int.Parse(param["actionid"]);
            if (actionid == 1)
            {// 客户端tcp心跳包
                session.LastActivityTime = DateTime.Now;
                session.ExitSession();
                Interlocked.Decrement(ref runningNum);
                return;
            }

            try
            {
                bool needWrite;
                //var data = ActionFactory.GetActionResponse(param, session, out needWrite);

                SocketGameResponse response = new SocketGameResponse();
                HttpGet httpGet = new HttpGet(param, session.SessionId, session.Channel.RemoteEndPoint.ToString(), LoginSuccessCallback, session);
                OnRequested(httpGet, response);
                byte[] data = response.ReadByte();

                ExSocket channel = session.Channel;
                var buffer = new byte[data.Length + 16];
                Buffer.BlockCopy(ssid.ToByteArray(), 0, buffer, 0, 16);
                Buffer.BlockCopy(data, 0, buffer, 16, data.Length);
                try
                {
                    socketLintener.PostSend(channel, buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Logger.Error("PostSend异常", ex);
                }

            }
            catch (Exception ex)
            {
                Logger.Error("Task异常", ex);
            }
            finally
            {
                session.ExitSession();
                Interlocked.Decrement(ref runningNum);
            }
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

        void socketLintener_Disconnected(object sender, ConnectionEventArgs e)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="packetHead"></param>
        protected GameSocketHost()
        {
            var port = ConfigUtils.GetSetting("Game.Port", "9001").ToInt();
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            int maxConnections = ConfigUtils.GetSetting("MaxConnections", "1000").ToInt();
            int backlog = ConfigUtils.GetSetting("Backlog", "100").ToInt();
            int maxAcceptOps = ConfigUtils.GetSetting("MaxAcceptOps", "100").ToInt();
            int bufferSize = ConfigUtils.GetSetting("BufferSize", "8192").ToInt();

            threadPool = new SmartThreadPool(180 * 1000, 100, 5);
            threadPool.Start();

            var socketSettings = new SocketSettings(maxConnections, backlog, maxAcceptOps, bufferSize, localEndPoint, 3600000, 3600000);
            socketLintener = new SocketListener(socketSettings);
            socketLintener.DataReceived += new ConnectionEventHandler(socketLintener_DataReceived);
            socketLintener.Connected += new ConnectionEventHandler(OnConnectCompleted);
            socketLintener.Disconnected += new ConnectionEventHandler(socketLintener_Disconnected);

            queueProcessThread = new Thread(ProcessQueue);
            queueProcessThread.Start();

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

        protected abstract void OnConnectCompleted(object sender, ConnectionEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            socketLintener.StartListen();
            OnStartAffer();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            OnServiceStop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userList"></param>
        /// <param name="actionId"></param>
        /// <param name="parameters"></param>
        /// <param name="successHandle"></param>
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
                try
                {
                    if (user == default(T))
                    {
                        continue;
                    }
                    HttpGet httpGet;
                    byte[] sendData = ActionFactory.GetActionResponse(actionId, user, shareParam.ToString(), out httpGet);
                    SendAsync(user.SocketSid, sendData);
                    if (httpGet != null)
                    {
                        successHandle(httpGet);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("SendToClient error:{0}", ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="data"></param>
        public void SendAsync(string sessionId, byte[] data)
        {
            GameSession session;
            if (globalSessions.TryGetValue(sessionId, out session))
            {
                socketLintener.PostSend(session.Channel, data, 0, data.Length);
            }
        }

        private string GenerateSid()
        {
            return string.Format("s_{0}|{1}|{2}", Guid.NewGuid().ToString("N"), GameEnvironment.ProductCode, GameEnvironment.ProductServerId);
        }


        protected abstract void OnRequested(HttpGet httpGet, IGameResponse response);

        protected abstract void OnStartAffer();

        protected abstract void OnServiceStop();

    }


}
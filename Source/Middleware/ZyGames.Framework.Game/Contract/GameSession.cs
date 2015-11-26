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
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using ProtoBuf;
using System.Threading;
using System.Collections.Concurrent;
using ServiceStack.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Redis;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionPushEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public ExSocket Socket { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OpCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }
    }
    /// <summary>
    /// 用户会话
    /// </summary>
    [ProtoContract]
    public class GameSession
    {
        #region static member

        private static ConcurrentDictionary<Guid, GameSession> _globalSession;
        private static ConcurrentDictionary<int, Guid> _userHash;
        private static ConcurrentDictionary<string, Guid> _remoteHash;
        private static SyncTimer clearTime;
        private static string sessionRedisKey = "__GLOBAL_SESSIONS";

        static GameSession()
        {
            HeartbeatTimeout = 60;//60s
            RequestTimeout = 500;
            Timeout = 2 * 60 * 60;//2H
            clearTime = new SyncTimer(OnClearSession, 6000, 60000);
            clearTime.Start();
            _globalSession = new ConcurrentDictionary<Guid, GameSession>();
            _userHash = new ConcurrentDictionary<int, Guid>();
            _remoteHash = new ConcurrentDictionary<string, Guid>();
            LoadUnLineData();
        }

        private static void LoadUnLineData()
        {
            try
            {
                IDictionary<string, byte[]> pairs = null;
                RedisConnectionPool.ProcessReadOnly(client =>
                {
                    //client.Add(string.Format("{0}:{1}", sessionRedisKey, sessionId), new GameSession().User, new TimeSpan(Timeout * 1000));
                    var keys = client.SearchKeys(string.Format("{0}:*", sessionRedisKey));
                    if (keys == null || keys.Count == 0) return;
                    pairs = client.GetAll<byte[]>(keys);
                });
                pairs = pairs ?? new Dictionary<string, byte[]>();
                foreach (var pair in pairs)
                {
                    var arr = pair.Key.Split(new char[] { ':' }, 2);
                    if (arr.Length != 2) continue;
                    Guid sessionId;
                    if (Guid.TryParse(arr[1], out sessionId))
                    {
                        var user = Encoding.UTF8.GetString(pair.Value).ParseJson<SessionUser>();
                        if (user == null) continue;
                        var session = new GameSession(sessionId, null) { LastActivityTime = user.OnlineDate };
                        _globalSession[sessionId] = session;

                        int userId = user.GetUserId();
                        GameSession oldsession;
                        Guid sid;
                        if (_userHash.TryGetValue(userId, out sid) &&
                            sid != session.KeyCode &&
                            (oldsession = Get(sid)) != null)
                        {
                            //防止先后问题
                            if (session.IsReplaced) continue;

                            if (oldsession.IsReplaced || oldsession.LastActivityTime < session.LastActivityTime)
                            {
                                oldsession.UnBind();
                                Expire(oldsession, 0);
                                continue;
                            }
                        }
                        _userHash[userId] = session.KeyCode;
                        session.User = user;
                    }
                }
            }
            catch (Exception er)
            {
                TraceLog.WriteError("Load GameSession from redis faild,{0}", er);
            }
        }

        private static void OnChangedSave(GameSession session)
        {
            try
            {
                if (session == null || !session.IsAuthorized) return;
                Expire(session, Timeout);
            }
            catch (Exception er)
            {
                TraceLog.WriteError("Save session to redis faild,{0}", er);
            }
        }

        private static void Expire(GameSession session, int timeout)
        {
            if (session == null) return;
            var user = MathUtils.ToJson(session.User);
            string sid = session.KeyCode.ToString("N");
            RedisConnectionPool.SetExpire(string.Format("{0}:{1}", sessionRedisKey, sid), user, timeout);
        }
        private static void SaveTo(IList<GameSession> sessions)
        {
            try
            {
                if (sessions.Count == 0) return;

                var keys = new List<string>();
                var values = new List<string>();
                foreach (GameSession session in sessions)
                {
                    var user = MathUtils.ToJson(session.User);
                    keys.Add(string.Format("{0}:{1}", sessionRedisKey, session.KeyCode.ToString("N")));
                    values.Add(user);
                }
                RedisConnectionPool.SetExpire(keys.ToArray(), values.ToArray(), Timeout);
            }
            catch (Exception er)
            {
                TraceLog.WriteError("Save session to redis faild,{0}", er);
            }
        }

        /// <summary>
        /// Count
        /// </summary>
        public static int Count { get { return _globalSession.Count; } }

        /// <summary>
        /// Heartbeat timeout(sec), default 60s
        /// </summary>
        public static int HeartbeatTimeout { get; set; }

        /// <summary>
        /// session timeout(sec), default 2h
        /// </summary>
        public static int Timeout { get; set; }
        /// <summary>
        /// Request timeout(ms), default 1s
        /// </summary>
        public static int RequestTimeout { get; set; }


        private static string GenerateSid(Guid guid)
        {
            return string.Format("s_{0}|{1}|{2}", guid.ToString("N"), GameEnvironment.ProductCode, GameEnvironment.ProductServerId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public static void ClearSession(GameSession session)
        {
            if (session == null) return;
            session.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ClearSession(Predicate<GameSession> match)
        {
            foreach (var pair in _globalSession)
            {
                var session = pair.Value;
                if (session == null) continue;
                if (match(session))
                {
                    ClearSession(session);
                }
            }
        }

        private static void OnClearSession(object state)
        {
            try
            {
                var sessions = new List<GameSession>();
                foreach (var pair in _globalSession)
                {
                    var session = pair.Value;
                    if (session == null) continue;
                    if (session.LastActivityTime.AddMinutes(1) <= DateTime.Now && session.User != null)
                    {
                        //1min to change the session
                        sessions.Add(session);
                    }

                    if (session.CheckExpired())
                    {
                        TraceLog.ReleaseWriteDebug("User {0} sessionId{1} is expire {2}({3}sec)",
                            session.UserId,
                            session.SessionId,
                            session.LastActivityTime,
                            Timeout);
                        session.DoHeartbeatTimeout();
                        session.Reset();

                    }
                    else if (!session.IsHeartbeatTimeout &&
                        HeartbeatTimeout > 0 &&
                        session.LastActivityTime < MathUtils.Now.AddSeconds(-HeartbeatTimeout))
                    {
                        session.DoHeartbeatTimeout();
                    }
                }
                SaveTo(sessions);
            }
            catch (Exception er)
            {
                TraceLog.WriteError("ClearSession error:{0}", er);
            }
        }

        /// <summary>
        /// Add session to cache
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static GameSession CreateNew(Guid keyCode, HttpRequest request)
        {
            return OnCreate(keyCode, request);
        }
        /// <summary>
        /// Add session to cache
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="request"></param>
        public static GameSession CreateNew(Guid keyCode, HttpListenerRequest request)
        {
            return OnCreate(keyCode, request);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public static GameSession CreateNew(Guid keyCode)
        {
            return OnCreate(keyCode);
        }

        /// <summary>
        /// Add session to cache
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="socket"></param>
        /// <param name="appServer"></param>
        public static GameSession CreateNew(Guid keyCode, ExSocket socket, ISocket appServer)
        {
            return OnCreate(keyCode, socket, appServer);
        }

        private static GameSession OnCreate(Guid keyCode, params object[] args)
        {
            GameSession session;
            if (args.Length == 0)
            {
                session = new GameSession(keyCode, null);
            }
            else if (args.Length == 1)
            {
                session = new GameSession(keyCode, args[0]);
            }
            else if (args.Length == 2 && args[0] is ExSocket)
            {
                ExSocket socket = args[0] as ExSocket;
                var appServer = args[1] as ISocket;
                session = new GameSession(keyCode, socket, appServer);
            }
            else
            {
                throw new ArgumentOutOfRangeException("param is error");
            }
            _globalSession[keyCode] = session;
            return session;
        }

        /// <summary>
        /// Recover session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="newSessionKey"></param>
        /// <param name="socket"></param>
        /// <param name="appServer"></param>
        /// <returns></returns>
        public static void Recover(GameSession session, Guid newSessionKey, ExSocket socket, ISocket appServer)
        {
            var newSession = Get(newSessionKey);
            if (session != null &&
                newSession != null &&
                session != newSession)
            {

                try
                {
                    if (session._exSocket != null) session._exSocket.Close();
                }
                catch { }
                //modify socket's keycod not found reason
                if (socket != null && appServer != null)
                {
                    socket.Reset(session.KeyCode);
                    session._exSocket = socket;
                    session.AppServer = appServer;
                }
                GameSession temp;
                if (_globalSession.TryRemove(newSessionKey, out temp))
                {
                    OnChangedSave(session);
                }
            }
        }

        /// <summary>
        /// Get session by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static GameSession Get(int userId)
        {
            Guid val;
            return _userHash.TryGetValue(userId, out val) ? Get(val) : null;
        }

        internal static Guid GetUserBindSid(int userId)
        {
            Guid val;
            if (_userHash.TryGetValue(userId, out val))
            {
                return val;
            }
            return Guid.Empty;
        }
        /// <summary>
        /// Get session by sessionid.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static GameSession Get(string sessionId)
        {
            GameSession session = null;
            Guid hashCode;
            if (TryParseSessionId(sessionId, out hashCode))
            {
                session = Get(hashCode);
            }
            return session;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static bool TryParseSessionId(string sessionId, out Guid sid)
        {
            sid = Guid.Empty;
            string[] arr = (sessionId ?? "").Split('_', '|');
            if (arr.Length > 1)
            {
                Guid hashCode;
                if (Guid.TryParse(arr[1], out hashCode))
                {
                    sid = hashCode;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get session by ExSocket.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static GameSession Get(Guid key)
        {
            GameSession session;
            return _globalSession.TryGetValue(key, out session) ? session : null;
        }

        /// <summary>
        /// Get all session
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<GameSession> GetAll()
        {
            return _globalSession.Values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyId"></param>
        /// <returns></returns>
        public static GameSession GetRemote(string proxyId)
        {
            Guid val;
            return _remoteHash.TryGetValue(proxyId, out val) ? Get(val) : null;
        }
        /// <summary>
        /// Get remote all
        /// </summary>
        /// <returns></returns>
        public static List<GameSession> GetRemoteAll()
        {
            return _remoteHash.Select(pair => Get(pair.Value)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<GameSession> GetOnlineAll()
        {
            return GetOnlineAll(HeartbeatTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<GameSession> GetOnlineAll(int delayTime)
        {
            foreach (var pair in _globalSession)
            {
                var session = pair.Value;
                if (!session.IsRemote &&
                    (!session.IsSocket || session.Connected) &&
                    session.LastActivityTime > MathUtils.Now.AddSeconds(-delayTime))
                {
                    yield return session;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static GameSession GetSessionByCookie(HttpRequest request)
        {
            var cookie = request.Cookies.Get("sid");
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                return Get(cookie.Value);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static GameSession GetSessionByCookie(HttpListenerRequest request)
        {
            var cookie = request.Cookies["sid"];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                return Get(cookie.Value);
            }
            return null;
        }

        #endregion

        private string _remoteAddress;
        private int _isInSession;
        private ExSocket _exSocket;
        /// <summary>
        /// 
        /// </summary>
        public ISocket AppServer { get; private set; }

        /// <summary>
        /// Heartbeat Timeout event
        /// </summary>
        public event Action<GameSession> HeartbeatTimeoutHandle;

        private void DoHeartbeatTimeout()
        {
            try
            {
                IsHeartbeatTimeout = true;
                Action<GameSession> handler = HeartbeatTimeoutHandle;
                if (handler != null) handler(this);
            }
            catch (Exception)
            {
            }
        }

        private readonly LockCachePool _monitorLock;

        /// <summary>
        /// 获得锁
        /// </summary>
        public bool EnterLock(int actionId, object agr1, out object arg2, out long waitTimeOutNum)
        {
#if LOCK_TEST
            arg2 = null;
            return true;
#else
            return _monitorLock.TryEnter(actionId, RequestTimeout, agr1, out arg2, out waitTimeOutNum);
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionId"></param>
        public void ExitLock(int actionId)
        {
#if LOCK_TEST
#else
            _monitorLock.Exit(actionId);
#endif
        }

        /// <summary>
        /// init proto deserialize use
        /// </summary>
        private GameSession()
        {
            _monitorLock = new LockCachePool();
            Refresh();
        }

        private GameSession(Guid keyCode, object request)
            : this()
        {
            KeyCode = keyCode;
            SessionId = GenerateSid(KeyCode);
            InitHttp(request);
        }

        #region property

        /// <summary>
        /// Remote end address
        /// </summary>
        [JsonIgnore]
        public string RemoteAddress
        {
            get { return _remoteAddress; }
            internal set { _remoteAddress = value; }
        }
        /// <summary>
        /// Remote end address
        /// </summary>
        [JsonIgnore]
        [Obsolete]
        public string EndAddress
        {
            get { return _remoteAddress; }
        }

        /// <summary>
        /// 对象被踢出
        /// </summary>
        [JsonIgnore]
        public bool IsReplaced
        {
            get { return !string.IsNullOrEmpty(OldSessionId); }
        }
        /// <summary>
        /// 标记此对象被踢出
        /// </summary>
        [ProtoMember(15)]
        public string OldSessionId { get; set; }

        /// <summary>
        /// key code
        /// </summary>
        [ProtoMember(1)]
        public Guid KeyCode { get; private set; }

        /// <summary>
        /// SessionId
        /// </summary>
        [ProtoMember(2)]
        public string SessionId { get; private set; }

        /// <summary>
        /// login UserId
        /// </summary>
        [JsonIgnore]
        public int UserId { get { return User != null ? User.GetUserId() : 0; } }

        /// <summary>
        /// User
        /// </summary>
        [ProtoMember(3)]
        public IUser User { get; private set; }

        /// <summary>
        /// 远程代理客户端的会话ID
        /// </summary>
        [ProtoMember(4)]
        public Guid ProxySid { get; internal set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        [ProtoMember(5)]
        public DateTime LastActivityTime { get; private set; }

        /// <summary>
        /// 是否会话超时
        /// </summary>
        [JsonIgnore]
        public bool IsTimeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public bool IsHeartbeatTimeout { get; set; }

        private string _proxyId;

        /// <summary>
        /// 远程代理客户端的标识ID
        /// </summary>
        [ProtoMember(6)]
        public string ProxyId
        {
            get { return _proxyId; }
            set
            {
                _proxyId = value;
                if (!string.IsNullOrEmpty(_proxyId))
                {
                    _remoteHash[_proxyId] = KeyCode;
                }
            }
        }

        /// <summary>
        /// 是否标识关闭状态
        /// </summary>
        [JsonIgnore]
        public bool IsClosed
        {
            get { return _exSocket != null && _exSocket.IsClosed; }
        }

        /// <summary>
        /// 是否已连接
        /// </summary>
        [JsonIgnore]
        public bool Connected
        {
            get
            {
                try
                {
                    return _exSocket != null && _exSocket.Connected;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// is socket
        /// </summary>
        [JsonIgnore]
        public bool IsSocket
        {
            get { return _exSocket != null; }
        }

        /// <summary>
        /// is websocket
        /// </summary>
        [JsonIgnore]
        public bool IsWebSocket
        {
            get { return _exSocket != null && _exSocket.IsWebSocket; }
        }

        #endregion

        private GameSession(Guid sid, ExSocket exSocket, ISocket appServer)
            : this(sid, null)
        {
            InitSocket(exSocket, appServer);
        }

        internal void InitSocket(ExSocket exSocket, ISocket appServer)
        {
            _exSocket = exSocket;
            if (_exSocket != null) _remoteAddress = _exSocket.RemoteEndPoint.ToNotNullString();
            AppServer = appServer;
            if (User != null)
            {
                //update userid with sid.
                _userHash[UserId] = KeyCode;
            }
        }

        internal void InitHttp(object request)
        {
            if (User != null)
            {
                //update userid with sid.
                _userHash[UserId] = KeyCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            IsTimeout = false;
            IsHeartbeatTimeout = false;
            LastActivityTime = DateTime.Now;
            if (User != null)
            {
                User.RefleshOnlineDate();
            }
        }

        /// <summary>
        /// 设置过期
        /// </summary>
        public void SetExpired()
        {
            LastActivityTime = MathUtils.UnixEpochDateTime;
            if (User != null)
            {
                User.SetExpired(LastActivityTime);
            }
            Expire(this, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Bind(IUser user)
        {
            if (user == null) return;
            int userId = user.GetUserId();
            if (userId > 0)
            {
                //解除UserId与前一次的Session连接对象绑定
                Guid sid;
                if (_userHash.TryGetValue(userId, out sid) && sid != KeyCode)
                {
                    var session = Get(sid);
                    if (session != null)
                    {
                        //防止先后问题
                        if (session.IsReplaced || session.LastActivityTime < this.LastActivityTime)
                        {
                            session.ReplpaceSession();
                            OnChangedSave(session); //保留被踢的状态
                            session.UnBind();
                        }
                    }
                }
            }
            _userHash[userId] = KeyCode;
            User = user;
            OnChangedSave(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnBind()
        {
            User = null;
        }

        /// <summary>
        /// 设置此对象被踢出
        /// </summary>
        private void ReplpaceSession()
        {
            OldSessionId = SessionId;
            if (User != null)
            {
                User.IsReplaced = true;
            }
        }

        /// <summary>
        /// Is authorized.
        /// </summary>
        [JsonIgnore]
        public bool IsAuthorized
        {
            get { return User != null && User.GetUserId() > 0; }
        }

        /// <summary>
        /// Is proxy server session
        /// </summary>
        [JsonIgnore]
        public bool IsProxyServer
        {
            get { return ProxySid != Guid.Empty && UserId == 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public bool IsRemote
        {
            get { return !string.IsNullOrEmpty(ProxyId); }
        }
        /// <summary>
        /// Close
        /// </summary>
        public void Close()
        {
            GameSession session;
            if (_globalSession.TryGetValue(KeyCode, out session) && session._exSocket != null)
            {
                //设置Socket为Closed的状态, 并未将物理连接马上中断
                session._exSocket.IsClosed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true:is expired</returns>
        private bool CheckExpired()
        {
            return LastActivityTime < MathUtils.Now.AddSeconds(-Timeout);
        }

        private void Reset()
        {
            IsTimeout = true;
            if (_exSocket != null)
            {
                try
                {
                    //设置Socket为Closed的状态, 并未将物理连接马上中断
                    _exSocket.IsClosed = true;
                    _exSocket.Close();
                }
                catch { }
            }
            Guid code;
            if (_userHash.TryRemove(UserId, out code))
            {
                UnBind();
            }

            if (!string.IsNullOrEmpty(ProxyId)) _remoteHash.TryRemove(ProxyId, out code);
            GameSession session;
            if (_globalSession.TryRemove(KeyCode, out session))
            {
                session._monitorLock.Clear();
            }
        }

        /// <summary>
        /// Post send to client
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        private async System.Threading.Tasks.Task<bool> PostSend(sbyte opCode, byte[] data, int offset, int count, Action<SocketAsyncResult> callback)
        {
            if (!IsSocket)
            {
                TraceLog.WriteError("Session does not support the push message");
                return false;
            }
            if (data == null || data.Length == 0)
            {
                return false;
            }
            if (!IsRemote && ProxySid != Guid.Empty)
            {
                //return proxy data format.
                data = CheckAdditionalHead(data, ProxySid);
                count += 16;
            }

            await AppServer.PostSend(_exSocket, opCode, data, offset, count, callback);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<bool> SendAsync(byte[] data, int offset, int count)
        {
            return await SendAsync(OpCode.Binary, data, offset, count, result => { });
        }

        /// <summary>
        /// Send async, add 16 len head
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<bool> SendAsync(sbyte opCode, byte[] data, int offset, int count, Action<SocketAsyncResult> callback)
        {
            return await PostSend(opCode, data, 0, data.Length, callback);
        }
        /// <summary>
        /// 检查加头16位ssid
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ssid"></param>
        /// <returns></returns>
        private static byte[] CheckAdditionalHead(byte[] data, Guid ssid)
        {
            var buffer = new byte[data.Length + 16];
            Buffer.BlockCopy(ssid.ToByteArray(), 0, buffer, 0, 16);
            Buffer.BlockCopy(data, 0, buffer, 16, data.Length);
            return buffer;
        }

        internal bool EnterSession()
        {
            return Interlocked.CompareExchange(ref _isInSession, 1, 0) == 0;
        }

        internal void ExitSession()
        {
            Interlocked.Exchange(ref _isInSession, 0);
        }

    }
}
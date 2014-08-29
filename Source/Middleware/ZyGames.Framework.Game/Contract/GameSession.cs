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
using System.Web;
using Newtonsoft.Json;
using ProtoBuf;
using System.Threading;
using System.Collections.Concurrent;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Redis;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 用户会话
    /// </summary>
    [ProtoContract]
    public class GameSession
    {
        private static ConcurrentDictionary<Guid, GameSession> _globalSession;
        private static ConcurrentDictionary<int, Guid> _userHash;
        private static ConcurrentDictionary<string, Guid> _remoteHash;
        private static Timer clearTime;
        private static string sessionRedisKey = "__GLOBAL_SESSIONS";
        private static int _isChanged;

        static GameSession()
        {
            Timeout = 600;
            clearTime = new Timer(OnClearSession, null, new TimeSpan(0, 0, 60), new TimeSpan(0, 0, 10));
            _globalSession = new ConcurrentDictionary<Guid, GameSession>();
            _userHash = new ConcurrentDictionary<int, Guid>();
            _remoteHash = new ConcurrentDictionary<string, Guid>();
            //LoadUnLineData();//不能恢复user对象
        }

        private static void LoadUnLineData()
        {
            try
            {
                RedisConnectionPool.ProcessReadOnly(client =>
                {
                    byte[] data = client.Get(sessionRedisKey) ?? new byte[0];
                    if (data.Length == 0)
                    {
                        return;
                    }
                    var temp = ProtoBufUtils.Deserialize<ConcurrentDictionary<Guid, GameSession>>(data);
                    if (temp != null)
                    {
                        var paris = temp.Where(p =>
                        {
                            p.Value.UserId = 0;//reset userid
                            return !p.Value.CheckExpired();
                        }).ToArray();
                        _globalSession = new ConcurrentDictionary<Guid, GameSession>(paris);
                    }
                });
            }
            catch (Exception er)
            {
                TraceLog.WriteError("Load GameSession from redis faild,{0}", er);
            }
        }

        private static void SaveTo()
        {
            try
            {
                //不能恢复user对象 不需要存储
                //byte[] data = ProtoBufUtils.Serialize(_globalSession);
                //RedisConnectionPool.Process(client => client.Set(sessionRedisKey, data));
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
        /// session timeout(sec).
        /// </summary>
        public static int Timeout { get; set; }

        private static string GenerateSid(Guid guid)
        {
            return string.Format("s_{0}|{1}|{2}", guid.ToString("N"), GameEnvironment.ProductCode, GameEnvironment.ProductServerId);
        }

        private static void OnClearSession(object state)
        {
            try
            {
                foreach (var pair in _globalSession)
                {
                    var session = pair.Value;
                    if (session != null && session.CheckExpired())
                    {
                        session.Reset();
                        //todo session
                        TraceLog.ReleaseWriteDebug("User {0} sessionId{1} is expire {2}({3}sec)",
                            session.UserId,
                            session.SessionId,
                            session.LastActivityTime,
                            Timeout);

                    }
                }
                if (_isChanged == 1)
                {
                    SaveTo();
                    Interlocked.Exchange(ref _isChanged, 0);
                }
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
        /// Add session to cache
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="socket"></param>
        /// <param name="sendCallback"></param>
        public static GameSession CreateNew(Guid keyCode, ExSocket socket, Action<ExSocket, byte[], int, int> sendCallback)
        {
            return OnCreate(keyCode, socket, sendCallback);
        }

        private static GameSession OnCreate(Guid keyCode, params object[] args)
        {
            GameSession session;
            if (args.Length == 1)
            {
                session = new GameSession(keyCode, args[0]);
            }
            else if (args.Length == 2 && args[0] is ExSocket)
            {
                ExSocket socket = args[0] as ExSocket;
                var sendCallback = args[1] as Action<ExSocket, byte[], int, int>;
                session = new GameSession(keyCode, socket, sendCallback);
            }
            else
            {
                throw new ArgumentOutOfRangeException("param is error");
            }
            _globalSession[keyCode] = session;
            OnChangedSave();
            return session;
        }

        /// <summary>
        /// Recover session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="newSessionKey"></param>
        /// <param name="socket"></param>
        /// <param name="sendCallback"></param>
        /// <returns></returns>
        public static void Recover(GameSession session, Guid newSessionKey, ExSocket socket, Action<ExSocket, byte[], int, int> sendCallback)
        {
            var newSession = Get(newSessionKey);
            if (session != null &&
                newSession != null &&
                session != newSession)
            {
                session._exSocket = socket;
                session._sendCallback = sendCallback;
                GameSession temp;
                if (_globalSession.TryRemove(newSessionKey, out temp))
                {
                    OnChangedSave();
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
            string[] arr = (sessionId ?? "").Split('_', '|');
            if (arr.Length > 1)
            {
                Guid hashCode;
                if (Guid.TryParse(arr[1], out hashCode))
                {
                    session = Get(hashCode);
                }
            }
            return session;
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
        public static List<GameSession> GetAll()
        {
            return _globalSession.Values.ToList();
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

        private string _remoteAddress;
        private int _isInSession;
        private ExSocket _exSocket;
        private Action<ExSocket, byte[], int, int> _sendCallback;

        private readonly IMonitorStrategy _monitorLock;

        /// <summary>
        /// 获得锁策略
        /// </summary>
        [JsonIgnore]
        public IMonitorStrategy MonitorLock
        {
            get { return _monitorLock; }
        }


        /// <summary>
        /// init proto deserialize use
        /// </summary>
        private GameSession()
        {
            _monitorLock = new MonitorLockStrategy();
            Refresh();
        }

        private GameSession(Guid keyCode, object request)
            : this()
        {
            KeyCode = keyCode;
            SessionId = GenerateSid(KeyCode);
            if (request is HttpRequest)
            {
                HttpRequest req = ((HttpRequest)request);
                var cookie = req.Cookies.Get("sid");
                if (cookie == null)
                {
                    cookie = new HttpCookie("sid", SessionId);
                    cookie.Expires = DateTime.Now.AddMinutes(2);
                    req.Cookies.Add(cookie);
                }
                _remoteAddress = req.UserHostAddress;
            }
            else if (request is HttpListenerRequest)
            {
                HttpListenerRequest req = ((HttpListenerRequest)request);
                var endPoint = req.RemoteEndPoint;
                _remoteAddress = endPoint != null ? endPoint.Address.ToString() : "";
            }
        }

        private GameSession(Guid sid, ExSocket exSocket, Action<ExSocket, byte[], int, int> sendCallback)
            : this(sid, null)
        {
            InitSocket(exSocket, sendCallback);
        }

        private void InitSocket(ExSocket exSocket, Action<ExSocket, byte[], int, int> sendCallback)
        {
            _exSocket = exSocket;
            _remoteAddress = _exSocket.RemoteEndPoint.ToString();
            _sendCallback = sendCallback;
        }

        internal void Refresh()
        {
            LastActivityTime = DateTime.Now;
            if (User != null)
            {
                User.RefleshOnlineDate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Bind(IUser user)
        {
            int userId = user.GetUserId();
            if (userId > 0)
            {
                //解除UserId与前一次的Session连接对象绑定
                Guid sid;
                if (_userHash.TryGetValue(userId, out sid))
                {
                    var session = Get(sid);
                    if (session != null)
                    {
                        session.UnBind();
                    }
                }
            }
            _userHash[userId] = KeyCode;
            UserId = userId;
            User = user;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnBind()
        {
            User = null;
            UserId = 0;
            OldSessionId = SessionId;
        }

        /// <summary>
        /// Is authorized.
        /// </summary>
        [JsonIgnore]
        public bool IsAuthorized
        {
            get { return User != null && UserId > 0; }
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

        private bool CheckExpired()
        {
            return LastActivityTime < MathUtils.Now.AddSeconds(-Timeout);
        }

        private void Reset()
        {
            IsTimeout = true;
            GameSession session;
            _globalSession.TryRemove(KeyCode, out session);
            if (_exSocket != null)
            {
                //设置Socket为Closed的状态, 并未将物理连接马上中断
                _exSocket.IsClosed = true;
            }
            Guid code;
            if (_userHash.TryRemove(UserId, out code))
            {
                UnBind();
            }

            if (!string.IsNullOrEmpty(ProxyId)) _remoteHash.TryRemove(ProxyId, out code);
        }


        private static void OnChangedSave()
        {
            Interlocked.Exchange(ref _isChanged, 1);
        }

        /// <summary>
        /// Remote end address
        /// </summary>
        [JsonIgnore]
        public string EndAddress
        {
            get
            {
                return _remoteAddress;
            }
        }

        /// <summary>
        /// Old sessionid
        /// </summary>
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
        [ProtoMember(3)]
        public int UserId { get; private set; }

        /// <summary>
        /// User
        /// </summary>
        [JsonIgnore]
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
        public DateTime LastActivityTime { get; internal set; }

        /// <summary>
        /// 是否会话超时
        /// </summary>
        [JsonIgnore]
        public bool IsTimeout { get; set; }

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
        /// Post send to client
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        private void PostSend(byte[] data, int offset, int count)
        {
            if (!IsSocket)
            {
                throw new Exception("Session does not support the push message");
            }
            if (data == null || data.Length == 0)
            {
                return;
            }
            _sendCallback(_exSocket, data, offset, count);
        }

        /// <summary>
        /// Send async, add 16 len head
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool SendAsync(byte[] data, int offset, int count)
        {
            if (Connected)
            {
                if (!IsRemote)
                {
                    data = CheckAdditionalHead(data, ProxySid);
                }
                PostSend(data, 0, data.Length);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查加头16位ssid
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ssid"></param>
        /// <returns></returns>
        private static byte[] CheckAdditionalHead(byte[] data, Guid ssid)
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
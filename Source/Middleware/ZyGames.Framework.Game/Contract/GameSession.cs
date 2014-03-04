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
using ProtoBuf;
using System.Threading;
using System.Collections.Concurrent;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Runtime;
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
        private static Timer clearTime;

        static GameSession()
        {
            _globalSession = new ConcurrentDictionary<Guid, GameSession>();
            _userHash = new ConcurrentDictionary<int, Guid>();
            Timeout = 120;
        }

        /// <summary>
        /// session timeout(sec).
        /// </summary>
        public static int Timeout { get; set; }

        private static string GenerateSid(Guid guid)
        {
            return string.Format("s_{0}|{1}|{2}", guid.ToString("N"), GameEnvironment.ProductCode, GameEnvironment.ProductServerId);
        }

        /// <summary>
        /// start clear session
        /// </summary>
        public static void StartClear()
        {
            clearTime = new Timer(OnClearSession, null, new TimeSpan(0, 0, 60), new TimeSpan(0, 5, 0));
        }

        private static void OnClearSession(object state)
        {
            try
            {
                foreach (var pair in _globalSession)
                {
                    if (pair.Value.LastActivityTime < MathUtils.Now.AddSeconds(-Timeout))
                    {
                        pair.Value.Close();
                    }
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
            GameSession session = new GameSession(keyCode, request);
            _globalSession[keyCode] = session;
            return session;
        }
        /// <summary>
        /// Add session to cache
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="request"></param>
        public static GameSession CreateNew(Guid keyCode, HttpListenerRequest request)
        {
            GameSession session = new GameSession(keyCode, request);
            _globalSession[keyCode] = session;
            return session;
        }

        /// <summary>
        /// Add session to cache
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="socket"></param>
        /// <param name="sendCallback"></param>
        public static GameSession CreateNew(Guid keyCode, ExSocket socket, Action<ExSocket, byte[], int, int> sendCallback)
        {
            GameSession session = new GameSession(keyCode, socket, sendCallback);
            _globalSession[keyCode] = session;
            return session;
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

        /// <summary>
        /// Get session by sessionid.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static GameSession Get(string sessionId)
        {
            GameSession session = null;
            string[] arr = sessionId.Split('_', '|');
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

        private string _remoteAddress;
        private int _isInSession;
        private readonly Guid _keyCode;
        private readonly object _request;
        private ExSocket _exSocket;
        private readonly Action<ExSocket, byte[], int, int> _sendCallback;

        private GameSession(Guid keyCode, object request)
        {
            _keyCode = keyCode;
            SessionId = GenerateSid(_keyCode);
            LastActivityTime = DateTime.Now;
            _request = request;
            if (request is HttpRequest)
            {
                _remoteAddress = ((HttpRequest)request).UserHostAddress;
            }
            else if (request is HttpListenerRequest)
            {
                var endPoint = ((HttpListenerRequest)request).RemoteEndPoint;
                _remoteAddress = endPoint != null ? endPoint.Address.ToString() : "";
            }
        }

        private GameSession(Guid sid, ExSocket exSocket, Action<ExSocket, byte[], int, int> sendCallback)
            : this(sid, null)
        {
            _exSocket = exSocket;
            _sendCallback = sendCallback;
        }

        /// <summary>
        /// Sid
        /// </summary>
        public Guid KeyCode { get { return _keyCode; } }

        /// <summary>
        /// bind Identity userid
        /// </summary>
        public GameSession BindIdentity(int userId)
        {
            //如果是代理服,需要新分配session
            if (IsProxyServer)
            {
                var session = GameSession.CreateNew(Guid.NewGuid(), Socket, _sendCallback);
                session.ProxySid = ProxySid;
                session.UserId = userId;
                return session;
            }
            UserId = userId;
            return this;
        }

        /// <summary>
        /// Is proxy server session
        /// </summary>
        public bool IsProxyServer
        {
            get { return ProxySid != Guid.Empty && UserId == 0; }
        }

        /// <summary>
        /// Close
        /// </summary>
        public void Close()
        {
            GameSession temp;
            _globalSession.TryRemove(KeyCode, out temp);
            Guid code;
            _userHash.TryRemove(UserId, out code);
        }

        /// <summary>
        /// Extend Socket object
        /// </summary>
        private ExSocket Socket
        {
            get
            {
                return _exSocket;
            }
        }

        /// <summary>
        /// Remote end address
        /// </summary>
        public string EndAddress
        {
            get
            {
                return _remoteAddress;
            }
        }

        /// <summary>
        /// SessionId
        /// </summary>
        [ProtoMember(1)]
        public string SessionId { get; private set; }

        private int _userId;

        /// <summary>
        /// login UserId
        /// </summary>
        [ProtoMember(2)]
        public int UserId
        {
            get { return _userId; }
            private set
            {
                _userId = value;
                _userHash[UserId] = KeyCode;
            }
        }

        /// <summary>
        /// Gets or sets ssid identifier by the server proxy.
        /// </summary>
        [ProtoMember(3)]
        public Guid ProxySid { get; internal set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        [ProtoMember(4)]
        public DateTime LastActivityTime { get; internal set; }

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool Connected
        {
            get
            {
                return _exSocket != null ? _exSocket.WorkSocket.Connected : false;
            }
        }

        /// <summary>
        /// Post send to client
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void PostSend(byte[] data, int offset, int count)
        {
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
                data = CheckAdditionalHead(data, ProxySid);
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
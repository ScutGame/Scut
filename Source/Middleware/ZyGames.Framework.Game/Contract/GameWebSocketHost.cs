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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Config;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.RPC.Sockets.WebSocket;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GameWebSocketHost : GameBaseHost
    {
        /// <summary>
        /// Protocol Section
        /// </summary>
        public ProtocolSection GetSection()
        {
            return ConfigManager.Configger.GetFirstOrAddConfig<ProtocolSection>();
        }

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

        private SocketListener socketListener;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSecurity"></param>
        protected GameWebSocketHost(bool isSecurity = false)
            : this(new WebSocketRequestHandler(isSecurity))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestHandler"></param>
        protected GameWebSocketHost(WebSocketRequestHandler requestHandler)
        {
            _setting = GameEnvironment.Setting;
            int port = _setting != null ? _setting.GamePort : 0;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            var section = GetSection();
            int maxConnections = section.SocketMaxConnection;
            int backlog = section.SocketBacklog;
            int maxAcceptOps = section.SocketMaxAcceptOps;
            int bufferSize = section.SocketBufferSize;
            int expireInterval = section.SocketExpireInterval;
            int expireTime = section.SocketExpireTime;

            var socketSettings = new SocketSettings(maxConnections, backlog, maxAcceptOps, bufferSize, localEndPoint, expireInterval, expireTime);
            socketListener = new SocketListener(socketSettings, requestHandler);
            socketListener.DataReceived += new ConnectionEventHandler(OnDataReceived);
            socketListener.Connected += new ConnectionEventHandler(socketLintener_OnConnectCompleted);
            socketListener.Handshaked += new ConnectionEventHandler(OnHandshaked);
            socketListener.Disconnected += new ConnectionEventHandler(OnDisconnected);
            socketListener.OnPing += new ConnectionEventHandler(socketLintener_OnPing);
            socketListener.OnPong += new ConnectionEventHandler(socketLintener_OnPong);
            socketListener.OnClosedStatus += socketLintener_OnClosedStatus;
        }

        private void socketLintener_OnClosedStatus(ExSocket socket, int closeStatusCode)
        {
            try
            {
                OnClosedStatus(socket, closeStatusCode);
            }
            catch (Exception err)
            {
                TraceLog.WriteError("OnPong error:{0}", err);
            }
        }


        private void socketLintener_OnConnectCompleted(ISocket sender, ConnectionEventArgs e)
        {
            try
            {
                var session = GameSession.CreateNew(e.Socket.HashCode, e.Socket, socketListener);
                session.HeartbeatTimeoutHandle += OnHeartbeatTimeout;
                OnConnectCompleted(sender, e);
            }
            catch (Exception err)
            {
                TraceLog.WriteError("ConnectCompleted error:{0}", err);
            }
        }

        private void OnDataReceived(ISocket sender, ConnectionEventArgs e)
        {
            try
            {
                RequestPackage package;
                if (!ActionDispatcher.TryDecodePackage(e, out package))
                {
                    //check command
                    string command = e.Meaage.Message;
                    if ("ping".Equals(command, StringComparison.OrdinalIgnoreCase))
                    {
                        OnPing(sender, e);
                        return;
                    }
                    if ("pong".Equals(command, StringComparison.OrdinalIgnoreCase))
                    {
                        OnPong(sender, e);
                        return;
                    }
                    OnError(sender, e);
                    return;
                }
                var session = GetSession(e, package);
                if (CheckSpecialPackge(package, session))
                {
                    return;
                }
                package.Bind(session);
                ProcessPackage(package, session).Wait();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Received to Host:{0} error:{1}", e.Socket.RemoteEndPoint, ex);
            }
        }

        private void OnDisconnected(ISocket sender, ConnectionEventArgs e)
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

        private void socketLintener_OnPong(ISocket sender, ConnectionEventArgs e)
        {
            try
            {
                OnPong(sender, e);
            }
            catch (Exception err)
            {
                TraceLog.WriteError("OnPong error:{0}", err);
            }
        }

        private void socketLintener_OnPing(ISocket sender, ConnectionEventArgs e)
        {
            try
            {
                OnPing(sender, e);
            }
            catch (Exception err)
            {
                TraceLog.WriteError("OnPing error:{0}", err);
            }
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
                              : GameSession.CreateNew(package.ProxySid, e.Socket, socketListener));
                if (session != null)
                {
                    session.ProxySid = package.ProxySid;
                }
            }
            else
            {
                session = GameSession.Get(package.SessionId) ?? GameSession.Get(e.Socket.HashCode);
            }
            if (session == null)
            {
                session = GameSession.CreateNew(package.ProxySid, e.Socket, socketListener);
            }
            if ((!session.Connected || !Equals(session.RemoteAddress, e.Socket.RemoteEndPoint.ToString())))
            {
                GameSession.Recover(session, e.Socket.HashCode, e.Socket, socketListener);
            }
            session.InitSocket(e.Socket, socketListener);
            return session;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnHandshaked(ISocket sender, ConnectionEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnConnectCompleted(object sender, ConnectionEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPong(ISocket sender, ConnectionEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPing(ISocket sender, ConnectionEventArgs e)
        {
            sender.Pong(e.Socket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="closeStatusCode"></param>
        protected virtual void OnClosedStatus(ExSocket socket, int closeStatusCode)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnError(ISocket sender, ConnectionEventArgs e)
        {
            sender.CloseHandshake(e.Socket, "param error");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        protected virtual void OnSendCompleted(SocketAsyncResult result)
        {
        }

        /// <summary>
        /// Raises the service stop event.
        /// </summary>
        protected override void OnServiceStop()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start(string[] args)
        {
            socketListener.StartListen();
            TraceLog.WriteLine("{0} WebSocket service {1}:{2} is started.", DateTime.Now.ToString("HH:mm:ss"), _setting.GameIpAddress, _setting.GamePort);
            base.Start(args);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            socketListener.Dispose();
            OnServiceStop();
            try
            {
                EntitySyncManger.Dispose();
            }
            catch { }
        }

        private async System.Threading.Tasks.Task ProcessPackage(RequestPackage package, GameSession session)
        {
            if (package == null) return;

            try
            {
                ActionGetter actionGetter;
                byte[] data = new byte[0];
                if (!string.IsNullOrEmpty(package.RouteName))
                {
                    actionGetter = ActionDispatcher.GetActionGetter(package, session);
                    if (CheckRemote(package.RouteName, actionGetter))
                    {
                        MessageStructure response = new MessageStructure();
                        OnCallRemote(package.RouteName, actionGetter, response);
                        data = response.PopBuffer();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    SocketGameResponse response = new SocketGameResponse();
                    response.WriteErrorCallback += ActionDispatcher.ResponseError;
                    actionGetter = ActionDispatcher.GetActionGetter(package, session);
                    DoAction(actionGetter, response);
                    data = response.ReadByte();
                }
                try
                {
                    if (session != null && data.Length > 0)
                    {
                        await session.SendAsync(actionGetter.OpCode, data, 0, data.Length, OnSendCompleted);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("PostSend error:{0}", ex);
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Task error:{0}", ex);
            }
            finally
            {
                if (session != null) session.ExitSession();
            }
        }

    }
}

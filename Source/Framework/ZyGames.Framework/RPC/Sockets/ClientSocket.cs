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
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public enum ConnectState
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Success,
        /// <summary>
        /// 
        /// </summary>
        Closed,
        /// <summary>
        /// 
        /// </summary>
        Error
    }
    /// <summary>
    /// Socket event arguments.
    /// </summary>
    public class SocketEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public DataMeaage Source { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data
        {
            get { return Source.Data; }
            set
            {
                if (Source == null)
                {
                    Source = new DataMeaage() { Data = value };
                }
                else
                {
                    Source.Data = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ExSocket Socket { get; set; }

        /// <summary>
        /// The empty.
        /// </summary>
        public new static SocketEventArgs Empty = new SocketEventArgs();
    }
    /// <summary>
    /// Socket event handler.
    /// </summary>
    public delegate void SocketEventHandler(ClientSocket sender, SocketEventArgs e);

    /// <summary>
    /// Client socket.
    /// </summary>
    public class ClientSocket : ISocket, IDisposable
    {
        #region 事件

        /// <summary>
        /// 接收到数据事件
        /// </summary>
        public event SocketEventHandler DataReceived;
        private void OnDataReceived(SocketEventArgs e)
        {
            if (DataReceived != null)
            {
                DataReceived(this, e);
            }
        }


        /// <summary>
        /// 连接断开事件
        /// </summary>
        public event SocketEventHandler Disconnected;
        private void OnDisconnected(SocketEventArgs e)
        {
            if (Disconnected != null)
            {
                Disconnected(this, e);
            }
        }
        #endregion

        private Socket socketClient;
        private ClientSocketSettings clientSettings;
        private SocketAsyncEventArgs sendEventArg;
        private SocketAsyncEventArgs receiveEventArg;
        private AutoResetEvent receiveWaitEvent = new AutoResetEvent(false);
        /// <summary>
        /// 
        /// </summary>
        protected RequestHandler requestHandler;
        private int isInSending;

        /// <summary>
        /// 0: no connect, 1: connected, 2: closed
        /// </summary>
        protected ConnectState connectState;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="clientSettings">Client settings.</param>
        public ClientSocket(ClientSocketSettings clientSettings)
            : this(clientSettings, new RequestHandler(new MessageHandler()))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSettings"></param>
        /// <param name="requestHandler"></param>
        public ClientSocket(ClientSocketSettings clientSettings, RequestHandler requestHandler)
        {
            this.clientSettings = clientSettings;
            this.requestHandler = requestHandler;
            Restart();
        }

        /// <summary>
        /// 0: no connect, 1: connected, 2: closed
        /// </summary>
        public ConnectState ReadyState { get { return connectState; } }

        /// <summary>
        /// Whether to receive data sync
        /// </summary>
        public bool IsSyncReceived { get; set; }

        /// <summary>
        /// connected.
        /// </summary>
        public bool Connected { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object UserData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ClientSocketSettings Settings { get { return clientSettings; } }

        /// <summary>
        /// 
        /// </summary>
        public EndPoint LocalEndPoint
        {
            get { return socketClient.LocalEndPoint; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ExSocket Socket
        {
            get
            {
                var dataToken = sendEventArg.UserToken as DataToken;
                return dataToken != null ? dataToken.Socket : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Restart()
        {
            Connected = false;
            connectState = ConnectState.None;
            socketClient = new Socket(this.clientSettings.RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ConnectAsync()
        {
            Connected = false;
            connectState = ConnectState.None;
            var e = new SocketAsyncEventArgs();
            e.AcceptSocket = socketClient;
            e.Completed += OnConnectComplated;
            try
            {
                return socketClient.ConnectAsync(e);
            }
            catch (ObjectDisposedException)
            {
                Restart();
                return socketClient.ConnectAsync(e);
            }
            catch (InvalidOperationException)
            {
                Restart();
                return socketClient.ConnectAsync(e);
            }
        }

        private void OnConnectComplated(object sender, SocketAsyncEventArgs e)
        {
            ConnectComplated(e.AcceptSocket);
        }

        /// <summary>
        /// Connect this instance.
        /// </summary>
        public void Connect()
        {
            Connected = false;
            connectState = ConnectState.None;
            try
            {
                socketClient.Connect(clientSettings.RemoteEndPoint);
            }
            catch (ObjectDisposedException)
            {
                Restart();
                socketClient.Connect(clientSettings.RemoteEndPoint);
            }
            catch (InvalidOperationException)
            {
                Restart();
                socketClient.Connect(clientSettings.RemoteEndPoint);
            }
            ConnectComplated(socketClient);
        }

        private void ConnectComplated(Socket acceSocket)
        {
            Connected = true;
            Bind(acceSocket);
            if (!SendHandshake(sendEventArg))
            {
                return;
            }
            PostReceive(receiveEventArg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void DoOpened(SocketEventArgs e)
        {
            connectState = ConnectState.Success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void DoClosed(SocketEventArgs e)
        {
        }

        private void Bind(Socket acceptSocket)
        {
            var exSocket = new ExSocket(acceptSocket);
            exSocket.LastAccessTime = DateTime.Now;
            var buffer = new byte[this.clientSettings.BufferSize * 2];
            this.sendEventArg = new SocketAsyncEventArgs();
            this.sendEventArg.SetBuffer(buffer, 0, this.clientSettings.BufferSize);
            var sendDataToken = new DataToken() { Socket = exSocket };
            sendDataToken.bufferOffset = this.sendEventArg.Offset;
            this.sendEventArg.UserToken = sendDataToken;
            this.sendEventArg.AcceptSocket = acceptSocket;
            this.sendEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

            this.receiveEventArg = new SocketAsyncEventArgs();
            this.receiveEventArg.SetBuffer(buffer, this.clientSettings.BufferSize, this.clientSettings.BufferSize);
            var receiveDataToken = new DataToken { Socket = exSocket, SyncSegments = new Queue<ArraySegment<byte>>() };
            receiveDataToken.bufferOffset = this.receiveEventArg.Offset;
            this.receiveEventArg.UserToken = receiveDataToken;
            this.receiveEventArg.AcceptSocket = acceptSocket;
            this.receiveEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            if (IsSyncReceived)
            {
                DataReceived += OnReceived;
            }
            requestHandler.Bind(this);
        }

        private void OnReceived(ClientSocket sender, SocketEventArgs e)
        {
            var token = receiveEventArg.UserToken as DataToken;
            if (token != null)
            {
                token.SyncSegments.Enqueue(new ArraySegment<byte>(e.Data));
            }
            receiveWaitEvent.Set();
        }

        private bool SendHandshake(SocketAsyncEventArgs ioEventArgs)
        {
            return requestHandler.SendHandshake(ioEventArgs);
        }


        private void PostReceive(SocketAsyncEventArgs ioEventArgs)
        {
            bool willRaiseEvent = socketClient.ReceiveAsync(ioEventArgs);

            if (!willRaiseEvent)
            {
                ProcessReceive(ioEventArgs);
            }
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs ioEventArgs)
        {
            try
            {
                DataToken ioDataToken = (DataToken)ioEventArgs.UserToken;

                switch (ioEventArgs.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        ProcessReceive(ioEventArgs);
                        break;
                    case SocketAsyncOperation.Send:
                        ProcessSend(ioEventArgs);
                        break;

                    default:
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (ObjectDisposedException)
            {
                ReleaseIOEventArgs(ioEventArgs);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("IO_Completed of client error:{0}", ex);
            }
        }

        private void ReleaseIOEventArgs(SocketAsyncEventArgs ioEventArgs)
        {
            if (ioEventArgs == null) return;

            var dataToken = (DataToken)ioEventArgs.UserToken;
            if (dataToken != null)
            {
                dataToken.Reset(true);
                dataToken.Socket = null;
            }
            ioEventArgs.AcceptSocket = null;
        }

        private void ProcessReceive(SocketAsyncEventArgs ioEventArgs)
        {
            DataToken dataToken = (DataToken)ioEventArgs.UserToken;

            if (ioEventArgs.BytesTransferred == 0)
            {
                //对方主动关闭socket
                //if (logger.IsDebugEnabled) logger.Debug("对方关闭Socket");
                Closing(ioEventArgs, OpCode.Empty);
                return;
            }

            if (ioEventArgs.SocketError != SocketError.Success)
            {
                //Socket错误
                //if (logger.IsDebugEnabled) logger.Debug("Socket接收错误:{0}", ioEventArg.SocketError);
                Closing(ioEventArgs);
                return;
            }

            List<DataMeaage> messages;
            bool hasHandshaked;
            bool needPostAnother = requestHandler.TryReceiveMessage(ioEventArgs, out messages, out hasHandshaked);
            if (hasHandshaked)
            {
                DoOpened(new SocketEventArgs() { Socket = dataToken.Socket });
            }
            // 触发收到消息事件
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    try
                    {
                        if (message.OpCode == OpCode.Close)
                        {
                            var statusCode = requestHandler.MessageProcessor != null
                                      ? requestHandler.MessageProcessor.GetCloseStatus(message.Data)
                                      : OpCode.Empty;
                            if (statusCode != OpCode.Empty)
                            {
                                OnClosedStatus(statusCode);
                            }
                            Closing(ioEventArgs, OpCode.Empty);
                            needPostAnother = false;
                            break;
                        }
                        OnDataReceived(new SocketEventArgs { Source = message });
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("OnDataReceived of client error:{0}", ex);
                    }
                }
            }
            if (needPostAnother)
            {
                PostReceive(ioEventArgs);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        protected virtual void OnClosedStatus(int statusCode)
        {
        }

        private bool TrySetSendFlag()
        {
            return Interlocked.CompareExchange(ref isInSending, 1, 0) == 0;
        }
        private void ResetSendFlag()
        {
            Interlocked.Exchange(ref isInSending, 0);
        }

        private void TryDequeueAndPostSend(ExSocket socket, SocketAsyncEventArgs ioEventArgs)
        {
            SocketAsyncResult result;
            if (socket.TryDequeueOrReset(out result))
            {
                DataToken dataToken = (DataToken)ioEventArgs.UserToken;
                dataToken.Socket = socket;
                dataToken.AsyncResult = result;
                dataToken.byteArrayForMessage = result.Data;
                dataToken.messageLength = result.Data.Length;
                try
                {
                    PostSend(ioEventArgs);
                }
                catch (Exception ex)
                {
                    dataToken.ResultCallback(ResultCode.Error, ex);
                    ResetSendFlag();
                }
            }
            else
            {
                //ResetSendFlag();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        public bool Receive(int millisecondsTimeout = 0)
        {
            return Wait(millisecondsTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Wait(int millisecondsTimeout = 0)
        {
            bool result = true;
            if (millisecondsTimeout > 0)
            {
                result = receiveWaitEvent.WaitOne(millisecondsTimeout);
            }
            else
            {
                receiveWaitEvent.WaitOne();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool TryReceiveBytes(out byte[] data)
        {
            data = new byte[0];
            var token = receiveEventArg.UserToken as DataToken;
            if (token == null)
            {
                return false;
            }
            if (token.SyncSegments.Count > 0)
            {
                data = token.SyncSegments.Dequeue().Array;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public async Task PostSend(byte[] data, int offset, int count)
        {
            await PostSend(Socket, data, offset, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public async Task PostSend(sbyte opCode, byte[] data, int offset, int count)
        {
            await PostSend(Socket, opCode, data, offset, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override async Task PostSend(ExSocket socket, byte[] data, int offset, int count)
        {
            await PostSend(socket, OpCode.Binary, data, offset, count);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override async Task PostSend(ExSocket socket, sbyte opCode, byte[] data, int offset, int count)
        {
            await PostSend(socket, opCode, data, offset, count, result => { });
        }

        /// <summary>
        /// Posts the send.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="opCode"></param>
        /// <param name="data">Data.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        /// <param name="callback"></param>
        public override async Task PostSend(ExSocket socket, sbyte opCode, byte[] data, int offset, int count, Action<SocketAsyncResult> callback)
        {
            byte[] buffer = requestHandler.MessageProcessor.BuildMessagePack(socket, opCode, data, offset, count);
            await SendAsync(socket, buffer, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public override void Ping(ExSocket socket)
        {
            byte[] data = Encoding.UTF8.GetBytes("ping");
            PostSend(socket, OpCode.Ping, data, 0, data.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public override void Pong(ExSocket socket)
        {
            byte[] data = Encoding.UTF8.GetBytes("pong");
            PostSend(socket, OpCode.Pong, data, 0, data.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="reason"></param>
        public override void CloseHandshake(ExSocket socket, string reason)
        {
            Dispose(socket, OpCode.Close, reason);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="callback"></param>
        internal protected override async Task<bool> SendAsync(ExSocket socket, byte[] buffer, Action<SocketAsyncResult> callback)
        {
            //socket.Enqueue(buffer, callback);
            if (socket.DirectSendOrEnqueue(buffer, callback))
            {
                try
                {
                    TryDequeueAndPostSend(socket, sendEventArg);
                    return true;
                }
                catch (Exception ex)
                {
                    ResetSendFlag();
                    TraceLog.WriteError("SendAsync {0} error:{1}", socket.RemoteEndPoint, ex);
                }
            }
            return false;
        }

        private void PostSend(SocketAsyncEventArgs ioEventArgs)
        {
            DataToken dataToken = (DataToken)ioEventArgs.UserToken;

            if (dataToken.messageLength - dataToken.messageBytesDone <= this.clientSettings.BufferSize)
            {
                ioEventArgs.SetBuffer(dataToken.bufferOffset, dataToken.messageLength - dataToken.messageBytesDone);
                Buffer.BlockCopy(dataToken.byteArrayForMessage, dataToken.messageBytesDone, ioEventArgs.Buffer, dataToken.bufferOffset, dataToken.messageLength - dataToken.messageBytesDone);
            }
            else
            {
                this.sendEventArg.SetBuffer(dataToken.bufferOffset, this.clientSettings.BufferSize);
                Buffer.BlockCopy(dataToken.byteArrayForMessage, dataToken.messageBytesDone, ioEventArgs.Buffer, dataToken.bufferOffset, this.clientSettings.BufferSize);
            }

            bool willRaiseEvent = this.socketClient.SendAsync(this.sendEventArg);
            if (!willRaiseEvent)
            {
                ProcessSend(ioEventArgs);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs ioEventArgs)
        {
            DataToken dataToken = (DataToken)ioEventArgs.UserToken;

            if (ioEventArgs.SocketError == SocketError.Success)
            {
                dataToken.messageBytesDone += ioEventArgs.BytesTransferred;
                if (dataToken.messageBytesDone != dataToken.messageLength)
                {
                    PostSend(ioEventArgs);
                }
                else
                {
                    dataToken.ResultCallback(ResultCode.Success);
                    dataToken.Reset(true);
                    try
                    {
                        TryDequeueAndPostSend(dataToken.Socket, ioEventArgs);
                    }
                    catch
                    {
                        ResetSendFlag();
                        throw;
                    }
                }
            }
            else
            {
                dataToken.ResultCallback(ResultCode.Close);
                ResetSendFlag();
                Closing(ioEventArgs);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ioEventArgs"></param>
        /// <param name="opCode"></param>
        /// <param name="reason"></param>
        internal protected override void Closing(SocketAsyncEventArgs ioEventArgs, sbyte opCode = OpCode.Close, string reason = "")
        {
            var dataToken = (DataToken)ioEventArgs.UserToken;
            try
            {
                if (connectState != ConnectState.Closed && opCode != OpCode.Empty)
                {
                    CloseHandshake(dataToken.Socket, reason);
                }

                DoCloseState();
                Dispose(ioEventArgs, opCode, reason, dataToken);
            }
            catch { }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ioEventArgs"></param>
        /// <param name="opCode"></param>
        /// <param name="reason"></param>
        /// <param name="dataToken"></param>
        protected void Dispose(SocketAsyncEventArgs ioEventArgs, sbyte opCode, string reason, DataToken dataToken)
        {
            ReleaseIOEventArgs(ioEventArgs);
            Dispose(dataToken.Socket, opCode, reason);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exSocket"></param>
        /// <param name="opCode"></param>
        /// <param name="reason"></param>
        protected void Dispose(ExSocket exSocket, sbyte opCode, string reason)
        {
            try
            {
                var e = new SocketEventArgs()
                {
                    Socket = exSocket,
                    Source = new DataMeaage() { OpCode = opCode, Message = reason }
                };
                DoClosed(e);
                OnDisconnected(e);
                if (exSocket != null) exSocket.Close();
                sendEventArg.Dispose();
                receiveEventArg.Dispose();
                receiveWaitEvent.Dispose();
                socketClient.Dispose();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Dispose connect of client error:{0}", ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reason"></param>
        public void Close(string reason = "")
        {
            //After receiving data processing close
            DoCloseState();
            CloseHandshake(Socket, reason);
        }

        private void DoCloseState()
        {
            Connected = false;
            connectState = ConnectState.Closed;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
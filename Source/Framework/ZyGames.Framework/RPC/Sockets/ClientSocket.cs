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
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using NLog;

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
    public class ClientSocket : ISocket
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

        Logger logger = LogManager.GetLogger("ClientSocket");
        private Socket socketClient;
        private ClientSocketSettings clientSettings;
        private SocketAsyncEventArgs sendEventArg;
        private SocketAsyncEventArgs receiveEventArg;
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
            socketClient = new Socket(this.clientSettings.RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// 0: no connect, 1: connected, 2: closed
        /// </summary>
        public ConnectState ReadyState { get { return connectState; } }

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
        /// <returns></returns>
        public bool ConnectAsync()
        {
            Connected = false;
            connectState = ConnectState.None;
            var e = new SocketAsyncEventArgs();
            e.AcceptSocket = socketClient;
            e.Completed += OnConnectComplated;
            return socketClient.ConnectAsync(e);
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
            socketClient.Connect(this.clientSettings.RemoteEndPoint);
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
            var receiveDataToken = new DataToken { Socket = exSocket };
            receiveDataToken.bufferOffset = this.receiveEventArg.Offset;
            this.receiveEventArg.UserToken = receiveDataToken;
            this.receiveEventArg.AcceptSocket = acceptSocket;
            this.receiveEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            requestHandler.Bind(this);
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
                logger.Error("IO_Completed", ex);
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
            if (ioEventArgs.SocketError != SocketError.Success)
            {
                //Socket错误
                //if (logger.IsDebugEnabled) logger.Debug("Socket接收错误:{0}", ioEventArg.SocketError);
                Closing(ioEventArgs);
                return;
            }

            if (ioEventArgs.BytesTransferred == 0)
            {
                //对方主动关闭socket
                //if (logger.IsDebugEnabled) logger.Debug("对方关闭Socket");
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
                            Closing(ioEventArgs, statusCode);
                            needPostAnother = false;
                            break;
                        }
                        OnDataReceived(new SocketEventArgs { Source = message });
                    }
                    catch (Exception ex)
                    {
                        logger.Error("OnDataReceived", ex);
                    }
                }
            }
            if (needPostAnother)
            {
                PostReceive(ioEventArgs);
            }
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
            byte[] data;
            if (socket.TryDequeue(out data))
            {
                DataToken dataToken = (DataToken)ioEventArgs.UserToken;
                dataToken.Socket = socket;
                dataToken.byteArrayForMessage = data;
                dataToken.messageLength = data.Length;

                PostSend(ioEventArgs);
            }
            else
            {
                ResetSendFlag();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void PostSend(byte[] data, int offset, int count)
        {
            PostSend(Socket, data, offset, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void PostSend(sbyte opCode, byte[] data, int offset, int count)
        {
            PostSend(Socket, opCode, data, offset, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void PostSend(ExSocket socket, byte[] data, int offset, int count)
        {
            PostSend(socket, OpCode.Binary, data, offset, count);
        }

        /// <summary>
        /// Posts the send.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="opCode"></param>
        /// <param name="data">Data.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public override void PostSend(ExSocket socket, sbyte opCode, byte[] data, int offset, int count)
        {
            byte[] buffer = requestHandler.MessageProcessor.BuildMessagePack(socket, opCode, data, offset, count);
            SendAsync(socket, buffer);
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
        internal protected override bool SendAsync(ExSocket socket, byte[] buffer)
        {
            socket.Enqueue(buffer);
            if (TrySetSendFlag())
            {
                try
                {
                    TryDequeueAndPostSend(socket, sendEventArg);
                    return true;
                }
                catch
                {
                    ResetSendFlag();
                    throw;
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
                exSocket.Close();
            }
            catch (Exception ex)
            {
                logger.Error("OnDisconnected", ex);
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
    }
}
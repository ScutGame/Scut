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
using System.Threading;
using ZyGames.Framework.Common.Log;
using NLog;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Socket listener.
    /// </summary>
    public class SocketListener
    {
        #region 事件
        /// <summary>
        /// 连接事件
        /// </summary>
        public event ConnectionEventHandler Connected;
        private void OnConnected(ConnectionEventArgs e)
        {
            if (Connected != null)
            {
                Connected(this, e);
            }
        }
        /// <summary>
        /// 断开连接事件
        /// </summary>
        public event ConnectionEventHandler Disconnected;
        private void OnDisconnected(ConnectionEventArgs e)
        {
            if (Disconnected != null)
            {
                Disconnected(this, e);
            }
        }
        /// <summary>
        /// 接收到数据包事件
        /// </summary>
        public event ConnectionEventHandler DataReceived;
        private void OnDataReceived(ConnectionEventArgs e)
        {
            if (DataReceived != null)
            {
                DataReceived(this, e);
            }
        }
        #endregion

        Logger logger = LogManager.GetLogger("SocketListener");
        BufferManager bufferManager;
        Socket listenSocket;
        HashSet<ExSocket> clientSockets = new HashSet<ExSocket>();
        Semaphore maxConnectionsEnforcer;
        SocketSettings socketSettings;
        PrefixHandler prefixHandler;
        MessageHandler messageHandler;
        ThreadSafeStack<SocketAsyncEventArgs> acceptEventArgsPool;
        ThreadSafeStack<SocketAsyncEventArgs> ioEventArgsPool;
        Timer expireTimer;
        /// <summary>
        /// Gets the connections.
        /// </summary>
        /// <value>The connections.</value>
        public int Connections
        {
            get { return clientSockets.Count; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.RPC.Sockets.SocketListener"/> class.
        /// </summary>
        /// <param name="socketSettings">Socket settings.</param>
        public SocketListener(SocketSettings socketSettings)
        {
            this.socketSettings = socketSettings;
            this.prefixHandler = new PrefixHandler();
            this.messageHandler = new MessageHandler();

            this.bufferManager = new BufferManager(this.socketSettings.BufferSize * this.socketSettings.NumOfSaeaForRecSend, this.socketSettings.BufferSize);

            this.ioEventArgsPool = new ThreadSafeStack<SocketAsyncEventArgs>(socketSettings.NumOfSaeaForRecSend);
            this.acceptEventArgsPool = new ThreadSafeStack<SocketAsyncEventArgs>(socketSettings.MaxAcceptOps);
            this.maxConnectionsEnforcer = new Semaphore(this.socketSettings.MaxConnections, this.socketSettings.MaxConnections);
            Init();
            expireTimer = new Timer(CheckExpire, null, socketSettings.ExpireInterval, socketSettings.ExpireInterval);
        }

        private void CheckExpire(object state)
        {
            try
            {
                lock (clientSockets)
                {
                    var now = DateTime.Now;
                    foreach (var socket in clientSockets)
                    {
                        if (now.Subtract(socket.LastAccessTime).TotalMilliseconds > socketSettings.ExpireTime)
                        {
                            socket.WorkSocket.Close();
                        }
                    }
                    //logger.InfoFormat("客户端连接数：{0}", clientSockets.Count);
                }
            }
            catch (Exception er)
            {
                TraceLog.WriteError("Socket listenner CheckExpire:{0}", er);
            }
        }

        private void Init()
        {
            this.bufferManager.InitBuffer();

            for (int i = 0; i < this.socketSettings.MaxAcceptOps; i++)
            {
                this.acceptEventArgsPool.Push(CreateAcceptEventArgs());
            }

            SocketAsyncEventArgs ioEventArgs;
            for (int i = 0; i < this.socketSettings.NumOfSaeaForRecSend; i++)
            {
                ioEventArgs = new SocketAsyncEventArgs();
                this.bufferManager.SetBuffer(ioEventArgs);
                ioEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                DataToken dataToken = new DataToken();
                dataToken.bufferOffset = ioEventArgs.Offset;
                ioEventArgs.UserToken = dataToken;
                this.ioEventArgsPool.Push(ioEventArgs);
            }
        }

        private void AddClient(ExSocket socket)
        {
            lock (clientSockets)
            {
                clientSockets.Add(socket);
            }
        }

        private SocketAsyncEventArgs CreateAcceptEventArgs()
        {
            SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();
            acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Completed);
            return acceptEventArg;
        }
        /// <summary>
        /// Starts the listen.
        /// </summary>
        public void StartListen()
        {
            listenSocket = new Socket(this.socketSettings.LocalEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            listenSocket.Bind(this.socketSettings.LocalEndPoint);
            listenSocket.Listen(socketSettings.Backlog);
            PostAccept();
        }

        private void PostAccept()
        {
            SocketAsyncEventArgs acceptEventArgs;
            if (this.acceptEventArgsPool.Count > 1)
            {
                try
                {
                    acceptEventArgs = this.acceptEventArgsPool.Pop();
                }
                catch
                {
                    acceptEventArgs = CreateAcceptEventArgs();
                }
            }
            else
            {
                acceptEventArgs = CreateAcceptEventArgs();
            }

            this.maxConnectionsEnforcer.WaitOne();
            bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArgs);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArgs);
            }
        }

        private void Accept_Completed(object sender, SocketAsyncEventArgs acceptEventArgs)
        {
            try
            {
                ProcessAccept(acceptEventArgs);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("AcceptCompleted method error:{0}", ex));

                if (acceptEventArgs.AcceptSocket != null)
                {
                    acceptEventArgs.AcceptSocket.Close();
                    acceptEventArgs.AcceptSocket = null;
                }
                acceptEventArgsPool.Push(acceptEventArgs);
                maxConnectionsEnforcer.Release();
            }
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs ioEventArgs)
        {
            try
            {
                DataToken ioDataToken = (DataToken)ioEventArgs.UserToken;
                ioDataToken.Socket.LastAccessTime = DateTime.Now;
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

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            PostAccept();

            if (acceptEventArgs.SocketError != SocketError.Success)
            {
                HandleBadAccept(acceptEventArgs);
                return;
            }

            SocketAsyncEventArgs ioEventArgs = this.ioEventArgsPool.Pop();
            ioEventArgs.AcceptSocket = acceptEventArgs.AcceptSocket;
            acceptEventArgs.AcceptSocket = null;
            this.acceptEventArgsPool.Push(acceptEventArgs);
            var dataToken = (DataToken)ioEventArgs.UserToken;
            ioEventArgs.SetBuffer(dataToken.bufferOffset, socketSettings.BufferSize);
            var exSocket = new ExSocket(ioEventArgs.AcceptSocket);
            exSocket.LastAccessTime = DateTime.Now;
            dataToken.Socket = exSocket;

            AddClient(exSocket);

            try
            {
                OnConnected(new ConnectionEventArgs { Socket = exSocket });
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnConnected error:{0}", ex);
            }

            PostReceive(ioEventArgs);
        }

        private void ReleaseIOEventArgs(SocketAsyncEventArgs ioEventArgs)
        {
            var dataToken = (DataToken)ioEventArgs.UserToken;
            dataToken.Reset(true);
            dataToken.Socket = null;
            ioEventArgs.AcceptSocket = null;
            ioEventArgsPool.Push(ioEventArgs);
        }
        /// <summary>
        /// 投递接收数据请求
        /// </summary>
        /// <param name="ioEventArgs"></param>
        private void PostReceive(SocketAsyncEventArgs ioEventArgs)
        {
            bool willRaiseEvent = ioEventArgs.AcceptSocket.ReceiveAsync(ioEventArgs);

            if (!willRaiseEvent)
            {
                ProcessReceive(ioEventArgs);
            }
        }

        /// <summary>
        /// 处理数据接收回调
        /// </summary>
        /// <param name="ioEventArgs"></param>
        private void ProcessReceive(SocketAsyncEventArgs ioEventArgs)
        {
            DataToken dataToken = (DataToken)ioEventArgs.UserToken;
            if (ioEventArgs.SocketError != SocketError.Success)
            {
                //Socket错误
                //if (logger.IsDebugEnabled) logger.Debug("ProcessReceive:{0}", ioEventArgs.SocketError);
                HandleCloseSocket(ioEventArgs);
                return;
            }

            if (ioEventArgs.BytesTransferred == 0)
            {
                //对方主动关闭socket
                //if (logger.IsDebugEnabled) logger.Debug("对方关闭Socket");
                HandleCloseSocket(ioEventArgs);
                return;
            }

            var exSocket = dataToken.Socket;

            #region 数据解析
            List<byte[]> msgs = new List<byte[]>();
            int remainingBytesToProcess = ioEventArgs.BytesTransferred;
            bool needPostAnother = true;
            do
            {
                if (dataToken.prefixBytesDone < 4)
                {
                    remainingBytesToProcess = prefixHandler.HandlePrefix(ioEventArgs, dataToken, remainingBytesToProcess);
                    if (dataToken.prefixBytesDone == 4 && (dataToken.messageLength > 10 * 1024 * 1024 || dataToken.messageLength <= 0))
                    {
                        //消息头已接收完毕，并且接收到的消息长度大于10M，socket传输的数据已紊乱，关闭掉
                        logger.Warn("接收到的消息长度错误:{0}", dataToken.messageLength);
                        needPostAnother = false;
                        HandleCloseSocket(ioEventArgs);
                        break;
                    }
                    //if (logger.IsDebugEnabled) logger.Debug("处理消息头，消息长度[{0}]，剩余字节[{1}]", dataToken.messageLength, remainingBytesToProcess);
                    if (remainingBytesToProcess == 0) break;
                }

                remainingBytesToProcess = messageHandler.HandleMessage(ioEventArgs, dataToken, remainingBytesToProcess);

                if (dataToken.IsMessageReady)
                {
                    //if (logger.IsDebugEnabled) logger.Debug("完整封包 长度[{0}],总传输[{1}],剩余[{2}]", dataToken.messageLength, ioEventArgs.BytesTransferred, remainingBytesToProcess);
                    msgs.Add(dataToken.byteArrayForMessage);
                    if (remainingBytesToProcess != 0)
                    {
                        //if (logger.IsDebugEnabled) logger.Debug("重置缓冲区,buffskip指针[{0}]。", dataToken.bufferSkip);
                        dataToken.Reset(false);
                    }
                }
                else
                {
                    //if (logger.IsDebugEnabled) logger.Debug("不完整封包 长度[{0}],总传输[{1}],已接收[{2}]", dataToken.messageLength, ioEventArgs.BytesTransferred, dataToken.messageBytesDone);
                }
            } while (remainingBytesToProcess != 0);
            #endregion
            //modify reason:数据包接收事件触发乱序
            foreach (var m in msgs)
            {
                try
                {
                    OnDataReceived(new ConnectionEventArgs { Socket = exSocket, Data = m });
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("OnDataReceived error:{0}", ex);
                }
            }

            if (needPostAnother)
            {
                //处理下个请求包
                if (dataToken.prefixBytesDone == 4 && dataToken.IsMessageReady)
                {
                    dataToken.Reset(true);
                }
                dataToken.bufferSkip = 0;
                PostReceive(ioEventArgs);
            }

            
        }

        private void TryDequeueAndPostSend(ExSocket socket, SocketAsyncEventArgs ioEventArgs)
        {
            bool isOwner = ioEventArgs == null;
            byte[] data;
            if (socket.TryDequeue(out data))
            {
                if (ioEventArgs == null)
                {
                    ioEventArgs = ioEventArgsPool.Pop();
                    ioEventArgs.AcceptSocket = socket.WorkSocket;
                }
                DataToken dataToken = (DataToken)ioEventArgs.UserToken;
                dataToken.Socket = socket;
                dataToken.byteArrayForMessage = data;
                dataToken.messageLength = data.Length;
                try
                {
                    PostSend(ioEventArgs);
                }
                catch
                {
                    if (isOwner)
                        ReleaseIOEventArgs(ioEventArgs);
                    throw;
                }
            }
            else
            {
                ReleaseIOEventArgs(ioEventArgs);
                socket.ResetSendFlag();
            }
        }
        /// <summary>
        /// Posts the send.
        /// </summary>
        /// <param name="socket">Socket.</param>
        /// <param name="data">Data.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public void PostSend(ExSocket socket, byte[] data, int offset, int count)
        {
            byte[] buffer = new byte[count + 4];
            Buffer.BlockCopy(BitConverter.GetBytes(count), 0, buffer, 0, 4);
            Buffer.BlockCopy(data, offset, buffer, 4, count);
            socket.Enqueue(buffer);
            if (socket.TrySetSendFlag())
            {
                try
                {
                    TryDequeueAndPostSend(socket, null);
                }
                catch
                {
                    socket.ResetSendFlag();
                    throw;
                }
            }
        }

        private void PostSend(SocketAsyncEventArgs ioEventArgs)
        {
            DataToken dataToken = (DataToken)ioEventArgs.UserToken;
            if (dataToken.messageLength - dataToken.messageBytesDone <= this.socketSettings.BufferSize)
            {
                ioEventArgs.SetBuffer(dataToken.bufferOffset, dataToken.messageLength - dataToken.messageBytesDone);
                Buffer.BlockCopy(dataToken.byteArrayForMessage, dataToken.messageBytesDone, ioEventArgs.Buffer, dataToken.bufferOffset, dataToken.messageLength - dataToken.messageBytesDone);
            }
            else
            {
                ioEventArgs.SetBuffer(dataToken.bufferOffset, this.socketSettings.BufferSize);
                Buffer.BlockCopy(dataToken.byteArrayForMessage, dataToken.messageBytesDone, ioEventArgs.Buffer, dataToken.bufferOffset, this.socketSettings.BufferSize);
            }

            var willRaiseEvent = ioEventArgs.AcceptSocket.SendAsync(ioEventArgs);
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
                        dataToken.Socket.ResetSendFlag();
                        throw;
                    }
                }
            }
            else
            {
                dataToken.Socket.ResetSendFlag();
                HandleCloseSocket(ioEventArgs);
            }
        }

        private void HandleCloseSocket(SocketAsyncEventArgs ioEventArgs)
        {
            bool needClose;
            var dataToken = (DataToken)ioEventArgs.UserToken;
            lock (clientSockets)
            {
                needClose = clientSockets.Remove(dataToken.Socket);
            }

            if (needClose)
            {
                //logger.InfoFormat("Socket：{0}关闭，关闭原因：SocketError：{1}。", dataToken.Socket.RemoteEndPoint, ioEventArgs.SocketError);
                maxConnectionsEnforcer.Release();
                try
                {
                    OnDisconnected(new ConnectionEventArgs { Socket = dataToken.Socket });
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("OnDisconnected error:{0}", ex);
                }
                ResetSAEAObject(ioEventArgs);
            }
            ReleaseIOEventArgs(ioEventArgs);
        }

        private void HandleBadAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            ResetSAEAObject(acceptEventArgs);
            acceptEventArgsPool.Push(acceptEventArgs);
            maxConnectionsEnforcer.Release();
        }

        /// <summary>
        /// Closes the socket.
        /// </summary>
        /// <param name="socket">Socket.</param>
        public void CloseSocket(ExSocket socket)
        {
            try
            {
                socket.WorkSocket.Shutdown(SocketShutdown.Both);
            }
            catch { }
            socket.WorkSocket.Close();
        }

        /// <summary>
        /// Close this instance.
        /// </summary>
        public void Close()
        {
            listenSocket.Close();

            lock (clientSockets)
            {
                foreach (var socket in clientSockets)
                {
                    socket.WorkSocket.Shutdown(SocketShutdown.Both);
                }
            }

            while (clientSockets.Count != 0) Thread.Sleep(10);
            DisposeAllSaeaObjects();
        }

        private void DisposeAllSaeaObjects()
        {
            SocketAsyncEventArgs eventArgs;
            while (this.acceptEventArgsPool.Count > 0)
            {
                eventArgs = acceptEventArgsPool.Pop();
                ResetSAEAObject(eventArgs);
            }
            while (this.ioEventArgsPool.Count > 0)
            {
                eventArgs = ioEventArgsPool.Pop();
                ResetSAEAObject(eventArgs);
            }
        }

        private static void ResetSAEAObject(SocketAsyncEventArgs eventArgs)
        {
            try
            {
                eventArgs.AcceptSocket.Close();
            }
            catch (Exception)
            {
            }
            eventArgs.AcceptSocket = null;
        }
    }
}
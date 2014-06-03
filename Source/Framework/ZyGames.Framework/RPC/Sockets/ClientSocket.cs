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
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using NLog;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Socket event arguments.
    /// </summary>
    public class SocketEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data { get; set; }

        /// <summary>
        /// The empty.
        /// </summary>
        public new static SocketEventArgs Empty = new SocketEventArgs();
    }
    /// <summary>
    /// Socket event handler.
    /// </summary>
    public delegate void SocketEventHandler(object sender, SocketEventArgs e);

    /// <summary>
    /// Client socket.
    /// </summary>
    public class ClientSocket
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
        Socket socket;
        ClientSocketSettings clientSettings;
        PrefixHandler prefixHandler;
        MessageHandler messageHandler;
        SocketAsyncEventArgs sendEventArg;
        DataToken sendDataToken;
        SocketAsyncEventArgs receiveEventArg;
        DataToken receiveDataToken;
        ConcurrentQueue<byte[]> sendQueue = new ConcurrentQueue<byte[]>();
        int isInSending;
        bool connected;
        /// <summary>
        /// Gets a value indicating whether this <see cref="ZyGames.Framework.RPC.Sockets.ClientSocket"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected { get { return connected; } }
        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.RPC.Sockets.ClientSocket"/> class.
        /// </summary>
        /// <param name="clientSettings">Client settings.</param>
        public ClientSocket(ClientSocketSettings clientSettings)
        {
            this.clientSettings = clientSettings;
            this.prefixHandler = new PrefixHandler();
            this.messageHandler = new MessageHandler();
        }
        /// <summary>
        /// 
        /// </summary>
        public object UserData { get; set; }

        /// <summary>
        /// Connect this instance.
        /// </summary>
        public void Connect()
        {
            socket = new Socket(this.clientSettings.RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(this.clientSettings.RemoteEndPoint);
            connected = true;

            var buffer = new byte[this.clientSettings.BufferSize * 2];
            this.sendEventArg = new SocketAsyncEventArgs();
            this.sendEventArg.SetBuffer(buffer, 0, this.clientSettings.BufferSize);
            this.sendDataToken = new DataToken();
            this.sendDataToken.bufferOffset = this.sendEventArg.Offset;
            this.sendEventArg.UserToken = this.sendDataToken;
            this.sendEventArg.AcceptSocket = socket;
            this.sendEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

            this.receiveEventArg = new SocketAsyncEventArgs();
            this.receiveEventArg.SetBuffer(buffer, this.clientSettings.BufferSize, this.clientSettings.BufferSize);
            this.receiveDataToken = new DataToken();
            this.receiveDataToken.bufferOffset = this.receiveEventArg.Offset;
            this.receiveEventArg.UserToken = this.receiveDataToken;
            this.receiveEventArg.AcceptSocket = socket;
            this.receiveEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

            PostReceive();
        }

        private void PostReceive()
        {
            bool willRaiseEvent = socket.ReceiveAsync(receiveEventArg);

            if (!willRaiseEvent)
            {
                ProcessReceive();
            }
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                DataToken ioDataToken = (DataToken)e.UserToken;

                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        ProcessReceive();
                        break;
                    case SocketAsyncOperation.Send:
                        ProcessSend();
                        break;

                    default:
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                logger.Error("IO_Completed", ex);
            }
        }

        private void ProcessReceive()
        {
            var dataToken = this.receiveDataToken;
            var ioEventArg = this.receiveEventArg;
            if (ioEventArg.SocketError != SocketError.Success)
            {
                //Socket错误
                //if (logger.IsDebugEnabled) logger.Debug("Socket接收错误:{0}", ioEventArg.SocketError);
                HandleCloseSocket();
                return;
            }

            if (ioEventArg.BytesTransferred == 0)
            {
                //对方主动关闭socket
                //if (logger.IsDebugEnabled) logger.Debug("对方关闭Socket");
                HandleCloseSocket();
                return;
            }

            #region 数据解析
            List<byte[]> msgs = new List<byte[]>();
            int remainingBytesToProcess = ioEventArg.BytesTransferred;
            bool needPostAnother = true;
            do
            {
                if (dataToken.prefixBytesDone < 4)
                {
                    remainingBytesToProcess = prefixHandler.HandlePrefix(ioEventArg, dataToken, remainingBytesToProcess);
                    if (dataToken.prefixBytesDone == 4 && (dataToken.messageLength > 10 * 1024 * 1024 || dataToken.messageLength <= 0))
                    {
                        //消息头已接收完毕，并且接收到的消息长度大于10M，socket传输的数据已紊乱，关闭掉
                        logger.Warn("接收到的消息长度错误:{0}", dataToken.messageLength);
                        needPostAnother = false;
                        HandleCloseSocket();
                        break;
                    }
                    //if (logger.IsDebugEnabled) logger.Debug("处理消息头，消息长度[{0}]，剩余字节[{1}]", dataToken.messageLength, remainingBytesToProcess);
                    if (remainingBytesToProcess == 0) break;
                }

                remainingBytesToProcess = messageHandler.HandleMessage(ioEventArg, dataToken, remainingBytesToProcess);

                if (dataToken.IsMessageReady)
                {
                    //if (logger.IsDebugEnabled) logger.Debug("完整封包 长度[{0}],总传输[{1}],剩余[{2}]", dataToken.messageLength, ioEventArg.BytesTransferred, remainingBytesToProcess);
                    msgs.Add(dataToken.byteArrayForMessage);
                    if (remainingBytesToProcess != 0)
                    {
                        //if (logger.IsDebugEnabled) logger.Debug("重置缓冲区,buffskip指针[{0}]。", dataToken.bufferSkip);
                        dataToken.Reset(false);
                    }
                }
                else
                {
                    //if (logger.IsDebugEnabled) logger.Debug("不完整封包 长度[{0}],总传输[{1}],已接收[{2}]", dataToken.messageLength, ioEventArg.BytesTransferred, dataToken.messageBytesDone);
                }
            } while (remainingBytesToProcess != 0);
            #endregion

            //modify reason:数据包接收事件触发乱序
            // 触发收到消息事件
            foreach (var m in msgs)
            {
                try
                {
                    OnDataReceived(new SocketEventArgs { Data = m });
                }
                catch (Exception ex)
                {
                    logger.Error("OnDataReceived", ex);
                }
            }
            if (needPostAnother)
            {
                if (dataToken.prefixBytesDone == 4 && dataToken.IsMessageReady)
                {
                    dataToken.Reset(true);
                }
                dataToken.bufferSkip = 0;
                PostReceive();
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

        private void TryDequeueAndPostSend()
        {
            byte[] data;
            if (sendQueue.TryDequeue(out data))
            {
                DataToken dataToken = sendDataToken;
                dataToken.byteArrayForMessage = data;
                dataToken.messageLength = data.Length;
                PostSend();
            }
            else
            {
                ResetSendFlag();
            }
        }
        /// <summary>
        /// Posts the send.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public void PostSend(byte[] data, int offset, int count)
        {
            if (!connected) throw new ObjectDisposedException("socket");
            byte[] buffer = new byte[count + 4];
            Buffer.BlockCopy(BitConverter.GetBytes(count), 0, buffer, 0, 4);
            Buffer.BlockCopy(data, offset, buffer, 4, count);
            sendQueue.Enqueue(buffer);
            if (TrySetSendFlag())
            {
                try
                {
                    TryDequeueAndPostSend();
                }
                catch
                {
                    ResetSendFlag();
                    throw;
                }
            }
        }

        private void PostSend()
        {
            var dataToken = this.sendDataToken;
            var ioEventArg = this.sendEventArg;

            if (dataToken.messageLength - dataToken.messageBytesDone <= this.clientSettings.BufferSize)
            {
                ioEventArg.SetBuffer(dataToken.bufferOffset, dataToken.messageLength - dataToken.messageBytesDone);
                Buffer.BlockCopy(dataToken.byteArrayForMessage, dataToken.messageBytesDone, ioEventArg.Buffer, dataToken.bufferOffset, dataToken.messageLength - dataToken.messageBytesDone);
            }
            else
            {
                this.sendEventArg.SetBuffer(dataToken.bufferOffset, this.clientSettings.BufferSize);
                Buffer.BlockCopy(dataToken.byteArrayForMessage, dataToken.messageBytesDone, ioEventArg.Buffer, dataToken.bufferOffset, this.clientSettings.BufferSize);
            }

            bool willRaiseEvent = this.socket.SendAsync(this.sendEventArg);
            if (!willRaiseEvent)
            {
                ProcessSend();
            }
        }

        private void ProcessSend()
        {
            var dataToken = this.sendDataToken;
            var ioEventArg = this.sendEventArg;

            if (ioEventArg.SocketError == SocketError.Success)
            {
                dataToken.messageBytesDone += ioEventArg.BytesTransferred;
                if (dataToken.messageBytesDone != dataToken.messageLength)
                {
                    PostSend();
                }
                else
                {
                    dataToken.Reset(true);
                    try
                    {
                        TryDequeueAndPostSend();
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
                HandleCloseSocket();
            }
        }

        private object syncRoot = new object();

        private void HandleCloseSocket()
        {
            if (connected)
            {
                lock (syncRoot)
                {
                    if (connected)
                    {
                        connected = false;
                        try
                        {
                            socket.Shutdown(SocketShutdown.Both);
                            try
                            {
                                OnDisconnected(SocketEventArgs.Empty);
                            }
                            catch (Exception ex)
                            {
                                logger.Error("OnDisconnected", ex);
                            }
                        }
                        catch { }

                        socket.Close();
                    }
                }
            }
        }
        /// <summary>
        /// Close this instance.
        /// </summary>
        public void Close()
        {
            HandleCloseSocket();
        }
    }
}
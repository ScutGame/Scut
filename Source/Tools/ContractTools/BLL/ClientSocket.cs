using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;

namespace ZyGames.OA.BLL.Remote
{
    public class SocketEventArgs : EventArgs
    {
        public byte[] Data { get; set; }

        public new static SocketEventArgs Empty = new SocketEventArgs();
    }

    public delegate void SocketEventHandler(object sender, SocketEventArgs e);

    public class ClientSocketSettings
    {
        public ClientSocketSettings(int bufferSize, IPEndPoint remoteEndPoint)
        {
            this.BufferSize = bufferSize;
            this.RemoteEndPoint = remoteEndPoint;
        }

        public int BufferSize { get; private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }
    }

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
        Queue<byte[]> sendQueue = new Queue<byte[]>();
        int isInSending;
        bool connected;
        private AutoResetEvent[] _waitHandle;

        public bool Connected { get { return connected; } }

        public ClientSocket(ClientSocketSettings clientSettings)
        {
            _waitHandle = new[] { new AutoResetEvent(false) };
            this.clientSettings = clientSettings;
            this.prefixHandler = new PrefixHandler();
            this.messageHandler = new MessageHandler();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WaitAll(int msTimeout)
        {
            if (msTimeout == 0)
            {
                return WaitHandle.WaitAll(_waitHandle);
            }
            else
            {
                return WaitHandle.WaitAll(_waitHandle, msTimeout);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopWait()
        {
            _waitHandle[0].Set();
        }
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
                HandleCloseSocket();
                return;
            }

            if (ioEventArg.BytesTransferred == 0)
            {
                //对方主动关闭socket
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
                    if (remainingBytesToProcess == 0) break;
                }

                remainingBytesToProcess = messageHandler.HandleMessage(ioEventArg, dataToken, remainingBytesToProcess);

                if (dataToken.IsMessageReady)
                {
                    msgs.Add(dataToken.byteArrayForMessage);
                    if (remainingBytesToProcess != 0)
                    {
                        dataToken.Reset(false);
                    }
                }
                else
                {
                }
            } while (remainingBytesToProcess != 0);
            #endregion

            if (needPostAnother)
            {
                if (dataToken.prefixBytesDone == 4 && dataToken.IsMessageReady)
                    dataToken.Reset(true);
                dataToken.bufferSkip = 0;
                PostReceive();
            }
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
            byte[] data = sendQueue.Dequeue();
            if (data != null)
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

        public void Close()
        {
            HandleCloseSocket();
        }
    }
}

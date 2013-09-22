using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.RPC.Sockets
{
    internal class SocketAsyncEventArgsProxy
    {
        private readonly int _bufferSize;
        private PrefixHandler _prefixHandler;
        private MessageHandler _messageHandler;

        internal SocketAsyncEventArgsProxy(int bufferSize)
        {
            _bufferSize = bufferSize;
            _prefixHandler = new PrefixHandler();
            _messageHandler = new MessageHandler();
        }

        public int BufferSize
        {
            get { return _bufferSize; }
        }

        public SocketProcessEvent ReceiveCompleted;
        public Action<SocketAsyncEventArgs> SendCompleted;
        public Action<SocketAsyncEventArgs> ClosedHandle;

        public SocketAsyncEventArgs CreateNewSaea()
        {
            var saea = new SocketAsyncEventArgs();
            saea.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            return saea;
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                DataToken ioDataToken = (DataToken)e.UserToken;
                ioDataToken.Socket.LastAccessTime = DateTime.Now;

                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        ProcessReceive(e);
                        break;
                    case SocketAsyncOperation.Send:
                        ProcessSend(e);
                        break;
                    default:
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Socket Io error:{0}", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="data"></param>
        public void Send(SocketAsyncEventArgs e, byte[] data)
        {
            DataToken session = e.UserToken as DataToken;
            session.byteArrayForMessage = data;
            session.messageLength = data.Length;
            DoInternalSend(e);
        }

        private void DoInternalSend(SocketAsyncEventArgs e)
        {
            if (StartProcessSend(e))
            {
                return;
            }
            var spinWait = new SpinWait();
            while (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                spinWait.SpinOnce();

                if (StartProcessSend(e))
                {
                    return;
                }
            }
        }

        private bool StartProcessSend(SocketAsyncEventArgs e)
        {
            try
            {
                DataToken session = e.UserToken as DataToken;
                if (session.messageLength - session.messageBytesDone <= _bufferSize)
                {
                    e.SetBuffer(session.bufferOffset, session.messageLength - session.messageBytesDone);
                    Buffer.BlockCopy(session.byteArrayForMessage, session.messageBytesDone, e.Buffer, session.bufferOffset, session.messageLength - session.messageBytesDone);
                }
                else
                {
                    e.SetBuffer(session.bufferOffset, _bufferSize);
                    Buffer.BlockCopy(session.byteArrayForMessage, session.messageBytesDone, e.Buffer, session.bufferOffset, _bufferSize);
                }

                bool willRaiseEvent = true;
                willRaiseEvent = e.AcceptSocket.SendAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessSend(e);
                }
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("TryProcessSend:{0}", ex);
            }
            return false;
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            DataToken session = (DataToken)e.UserToken;
            if (e.SocketError == SocketError.Success)
            {
                session.messageBytesDone += e.BytesTransferred;

                if (session.messageBytesDone != session.messageLength)
                {
                    DoInternalSend(e);
                }
                else
                {
                    session.Reset(true);
                    // 触发数据发送成功事件
                    if (SendCompleted != null)
                    {
                        SendCompleted.BeginInvoke(e, null, null);
                    }
                }
            }
            else
            {
                session.Reset(true);
                ProcessSocketError(e);
            }
        }

        /// <summary>
        /// 接收处理
        /// </summary>
        /// <param name="e"></param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            DataToken session = e.UserToken as DataToken;
            if (e.BytesTransferred > 0)
            {
                if (e.SocketError == SocketError.Success)
                {
                    //是否有已接收但未处理的数据
                    int remainingBytesToProcess = e.BytesTransferred;
                    do
                    {
                        if (session.prefixBytesDone < 4)
                        {
                            remainingBytesToProcess = _prefixHandler.HandlePrefix(e, session, remainingBytesToProcess);
                            if (remainingBytesToProcess == 0)
                            {
                                session.bufferSkip = 0;
                                StartReceive(e);
                                return;
                            }
                        }

                        remainingBytesToProcess = _messageHandler.HandleMessage(e, session, remainingBytesToProcess);

                        if (session.IsMessageReady)
                        {
                            // 触发收到消息事件
                            if (ReceiveCompleted != null)
                            {
                                byte[] data = session.byteArrayForMessage;
                                ReceiveCompleted.BeginInvoke(new SocketProcessEventArgs() { Socket = session.Socket, Data = data }, null, null);
                            }
                            if (remainingBytesToProcess != 0)
                                session.Reset(false);
                        }
                    }
                    while (remainingBytesToProcess != 0);

                    if (session.prefixBytesDone == 4 && session.IsMessageReady)
                        session.Reset(true);
                    session.bufferSkip = 0;
                    StartReceive(e);
                }
                else
                {
                    session.Reset(true);
                    ProcessSocketError(e);
                }
            }
            else
            {
                session.Reset(true);
                CloseClientSocket(e);
            }
        }

        internal void StartReceive(SocketAsyncEventArgs receiveArgs)
        {
            DataToken session = receiveArgs.UserToken as DataToken;
            receiveArgs.SetBuffer(session.bufferOffset, BufferSize);

            if (!receiveArgs.AcceptSocket.ReceiveAsync(receiveArgs))
            {
                ProcessReceive(receiveArgs);
            }
        }

        private void ProcessSocketError(SocketAsyncEventArgs e)
        {
            CloseClientSocket(e);
        }

        internal void CloseClientSocket(SocketAsyncEventArgs e)
        {
            if (ClosedHandle != null)
            {
                ClosedHandle(e);
            }
        }

        internal void Disconnect(SocketAsyncEventArgs e)
        {
            ((DataToken)e.UserToken).Socket.Disconnect();
        }

        internal void CloseConnect(SocketAsyncEventArgs e)
        {
            ((DataToken)e.UserToken).Socket.Close();
        }
    }
}

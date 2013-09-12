using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Sockets
{
    internal class SocketAsyncEventArgsProxy
    {
        private readonly int _bufferSize;

        internal SocketAsyncEventArgsProxy(int bufferSize)
        {
            _bufferSize = bufferSize;
        }

        public int BufferSize
        {
            get { return _bufferSize; }
        }

        public Action<SocketSession, byte[]> ReceiveCompleted;
        public Action<SocketAsyncEventArgs> SendCompleted;
        public Action<SocketAsyncEventArgs> ClosedHandle;
        public Action<SocketAsyncEventArgs> SendingHandle;

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
        public void DoStartSend(SocketAsyncEventArgs e)
        {
            SocketSession session = e.UserToken as SocketSession;
            byte[] buffer = session.SendPacket.ReadBlockData(_bufferSize);
            if (buffer.Length == 0)
            {
                return;
            }
            DoInternalSend(e, new ArraySegment<byte>(buffer));
        }

        private void DoInternalSend(SocketAsyncEventArgs e, ArraySegment<byte> segment)
        {
            if (TryProcessSend(e, segment))
            {
                return;
            }
            var spinWait = new SpinWait();
            while (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                spinWait.SpinOnce();

                if (TryProcessSend(e, segment))
                {
                    return;
                }
            }
        }

        private bool TryProcessSend(SocketAsyncEventArgs e, ArraySegment<byte> segment)
        {
            if (e.SocketError != SocketError.Success)
            {
                return false;
            }
            SocketSession session = e.UserToken as SocketSession;
            e.SetBuffer(session.BufferOffset, segment.Count);
            Buffer.BlockCopy(segment.Array, 0, e.Buffer, session.BufferOffset, segment.Count);

            if (!e.AcceptSocket.SendAsync(e))
            {
                ProcessSend(e);
            }
            return true;
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0)
            {
                SocketSession session = e.UserToken as SocketSession;
                if (e.SocketError == SocketError.Success)
                {
                    //处理上一次传送字节长度
                    session.SendPacket.SetDoneByteCount(e.BytesTransferred);
                    if (SendingHandle!=null)
                    {
                        SendingHandle.BeginInvoke(e,null,null);
                    }
                    if (session.SendPacket.RemainingByteCount > 0)
                    {
                        DoStartSend(e);
                    }
                    else
                    {
                        OnSendCompleted(e);
                    }
                }
                else
                {
                    ProcessSocketError(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        /// <summary>
        /// 接收处理
        /// </summary>
        /// <param name="e"></param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0)
            {
                SocketSession session = e.UserToken as SocketSession;
                if (e.SocketError == SocketError.Success)
                {
                    //是否有已接收但未处理的数据
                    byte[] data = new byte[e.BytesTransferred];
                    Buffer.BlockCopy(e.Buffer, session.BufferOffset, data, 0, data.Length);
                    session.ReceivePacket.InsertByteArray(data);

                    //session.ReceiveBuffer(e);
                    if (session.ReceivePacket.HasCompleteBytes)
                    {
                        byte[] buffer;
                        bool hasNext = false;
                        do
                        {
                            hasNext = session.ReceivePacket.CheckCompletePacket(out buffer);
                            if (hasNext)
                            {
                                //处理接收数据
                                if (ReceiveCompleted != null)
                                {
                                    ReceiveCompleted.BeginInvoke(session, buffer, null, null);
                                }
                            }

                        } while (hasNext);

                    }
                    StartReceive(e);
                }
                else
                {
                    ProcessSocketError(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        internal void StartReceive(SocketAsyncEventArgs receiveArgs)
        {
            SocketSession session = receiveArgs.UserToken as SocketSession;
            receiveArgs.SetBuffer(session.BufferOffset, BufferSize);

            if (!receiveArgs.AcceptSocket.ReceiveAsync(receiveArgs))
            {
                ProcessReceive(receiveArgs);
            }
        }

        private void OnSendCompleted(SocketAsyncEventArgs e)
        {
            if (SendCompleted != null)
            {
                SendCompleted(e);
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
            try
            {
                e.AcceptSocket.Shutdown(SocketShutdown.Both);
                e.AcceptSocket.Disconnect(true);
            }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }
        }

        internal void CloseConnect(SocketAsyncEventArgs e)
        {
            try
            {
                e.AcceptSocket.Shutdown(SocketShutdown.Both);
                e.AcceptSocket.Close();
            }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Socket会话对象
    /// </summary>
    public sealed class SocketSession : IDisposable
    {
        private Stopwatch _watch = new Stopwatch();
        private BufferPacket _receivePacket;
        private BufferPacket _sendPacket;
        private IPEndPoint _remoteAdrress;
        private DateTime _preAccessTime;
        private SocketAsyncEventArgs _receiveSendEventArgs;
        private SocketAsyncEventArgs sendSEAVObject;
        internal readonly int bufferOffsetReceive;
        internal readonly int bufferOffsetSend;
        private readonly int _bufferSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveSendEventArgs">客户端连接Socket对象</param>
        /// <param name="revOffset">接收缓冲区的位置偏移量</param>
        /// <param name="sendOffset">发关缓冲区的位置偏移量</param>
        public SocketSession(SocketAsyncEventArgs receiveSendEventArgs, int revOffset, int sendOffset, int bufferSize)
        {
            _receivePacket = new BufferPacket();
            _sendPacket = new BufferPacket();
            IsDisposed = false;
            _preAccessTime = DateTime.Now;
            _receiveSendEventArgs = receiveSendEventArgs;
            bufferOffsetReceive = revOffset;
            bufferOffsetSend = sendOffset;
            _bufferSize = bufferSize;

            if (_receiveSendEventArgs.AcceptSocket != null)
            {
                _remoteAdrress = _receiveSendEventArgs.AcceptSocket.RemoteEndPoint as IPEndPoint;
                //_connection.ExclusiveAddressUse = true;
                //延迟关闭Socket
                //_connection.LingerState = new LingerOption(true, 10);
                _receiveSendEventArgs.AcceptSocket.NoDelay = true;
                //_connection.Ttl = 42;

            }
        }

        /// <summary>
        /// 开启观察运行时间
        /// </summary>
        public void StartWatchTime()
        {
            _watch.Start();
        }

        /// <summary>
        /// 停止观察运行时间
        /// </summary>
        public TimeSpan StopWatchTime()
        {
            _watch.Stop();
            return _watch.Elapsed;
        }

        internal void InitAsyncEventSend(BufferManager bufferManager)
        {
            sendSEAVObject = new SocketAsyncEventArgs();
            sendSEAVObject.AcceptSocket = _receiveSendEventArgs.AcceptSocket;
            sendSEAVObject.SetBuffer(bufferManager.BufferData, bufferOffsetSend, _bufferSize);
            sendSEAVObject.UserToken = this;
            sendSEAVObject.Completed += OnSendingCompleted;
        }

        private void OnSendingCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                switch (e.LastOperation)
                {
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

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0)
            {
                SocketSession session = e.UserToken as SocketSession;
                if (e.SocketError == SocketError.Success)
                {
                    //处理上一次传送字节长度
                    session.SendPacket.SetRemainingByteCount(e.BytesTransferred);
                    if (session.SendPacket.RemainingByteCount > 0)
                    {
                        DoStartSend(e);
                    }
                }
                else
                {
                    OnSendError(e);
                }
            }
            else
            {
                OnSendClose(e);
            }
        }

        private void OnSendClose(SocketAsyncEventArgs e)
        {
            //todo test
            Console.WriteLine("OnSendClose");
        }

        private void OnSendError(SocketAsyncEventArgs e)
        {
            //todo test
            Console.WriteLine("OnSendError");
        }

        internal void StartSend()
        {
            DoStartSend(sendSEAVObject);
        }

        private void DoStartSend(SocketAsyncEventArgs sendArgs)
        {
            SocketSession session = sendArgs.UserToken as SocketSession;
            byte[] buffer = session.SendPacket.ReadBlockData(_bufferSize);
            if (buffer.Length == 0)
            {
                return;
            }
            InternalSend(sendArgs, new ArraySegment<byte>(buffer));
        }


        private void InternalSend(SocketAsyncEventArgs e, ArraySegment<byte> segment)
        {
            if (InternalTrySend(e, segment))
                return;

            var spinWait = new SpinWait();

            while (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                spinWait.SpinOnce();

                if (InternalTrySend(e, segment))
                    return;
            }
        }

        private bool InternalTrySend(SocketAsyncEventArgs e, ArraySegment<byte> segment)
        {
            if (!TrySend(e, segment))
                return false;
            return true;
        }

        private bool TrySend(SocketAsyncEventArgs e, ArraySegment<byte> segment)
        {
            if (e.SocketError != SocketError.Success)
                return false;
            SocketSession session = e.UserToken as SocketSession;

#if DEBUG
            //todo test
            Console.WriteLine("准备传送{0}:{1}byte", session.RemoteAddress, segment.Count);
#endif
            e.SetBuffer(session.bufferOffsetSend, segment.Count);
            Buffer.BlockCopy(segment.Array, 0, e.Buffer, session.bufferOffsetSend, segment.Count);

            if (!e.AcceptSocket.SendAsync(e))
            {
                ProcessSend(e);
            }
            return true;
        }

        /// <summary>
        /// 连接关闭事件
        /// </summary>
        public event SocketClosingHandle OnClosing;

        internal BufferPacket ReceivePacket
        {
            get { return _receivePacket; }
        }
        internal BufferPacket SendPacket
        {
            get { return _sendPacket; }
        }

        /// <summary>
        /// 是否释放
        /// </summary>
        public bool IsDisposed
        {
            get;
            private set;
        }
        /// <summary>
        /// 用户自定数据
        /// </summary>
        public object UserData
        {
            get;
            set;
        }
        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime AccessTime
        {
            get
            {
                return _preAccessTime;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RemoteAddress
        {
            get { return _remoteAdrress == null ? "" : _remoteAdrress.ToString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get { return _remoteAdrress; }
        }

        public bool Connected
        {
            get { return sendSEAVObject.BytesTransferred > 0; }
        }

        /// <summary>
        /// 客户端连接Socket对象
        /// </summary>
        public Socket Connection
        {
            get
            {
                return _receiveSendEventArgs.AcceptSocket;
            }
        }

        /// <summary>
        /// 通知上层处理关闭事件
        /// </summary>
        public void OnNotifyClosed()
        {
            if (OnClosing != null)
            {
                OnClosing(RemoteAddress);
            }
        }

        /// <summary>
        /// 接收客户端上传数据报（可分段）
        /// </summary>
        /// <param name="receiveArgs"></param>
        /// <returns>分段读取是否结束,True则读取结束</returns>
        internal void ReceiveBuffer(SocketAsyncEventArgs receiveArgs)
        {
            SocketSession session = receiveArgs.UserToken as SocketSession;
            byte[] data = new byte[receiveArgs.BytesTransferred];
            Buffer.BlockCopy(receiveArgs.Buffer, session.bufferOffsetReceive, data, 0, data.Length);
            _receivePacket.InsertByteArray(data);
        }

        internal void PutInSendQueue(byte[] buffer)
        {
            _sendPacket.InsertByteArray(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisConnection()
        {
            try
            {
                _receiveSendEventArgs.AcceptSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
            }
            _receiveSendEventArgs.AcceptSocket.Close();
        }
        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            try
            {
                _receiveSendEventArgs = null;
                //sendSEAVObject = null;
                _receivePacket = null;
                _sendPacket = null;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SocketSession disposed error:{0}", ex);
            }
            finally
            {
                IsDisposed = true;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.RPC.Sockets
{
    internal delegate void ReceiveHandle(SocketSession session, byte[] buffer);

    /// <summary>
    /// 接收传送的异步操作池  
    /// </summary>
    public sealed class SocketAsyncPool : IDisposable
    {
        private Stack<SocketAsyncEventArgs> _pool;
        private ISocketReceiver _receiver;
        private readonly int _minCapacity;
        private readonly int _maxCapacity;
        private readonly int _bufferSize;
        private int _currentUseNum;
        private int _SAEAObjectCount;
        private BufferManager _bufferManager;
        private ReceiveHandle _receiveHandle;

        public SocketAsyncPool(int minCapacity, int maxCapacity, int bufferSize, ISocketReceiver receiver)
            : this(minCapacity, maxCapacity, bufferSize, 2, receiver)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minCapacity">初始容量</param>
        /// <param name="maxCapacity">最大容量</param>
        /// <param name="bufferSize"></param>
        /// <param name="opsToPreAlloc">读写数</param>
        /// <param name="receiver"></param>
        public SocketAsyncPool(int minCapacity, int maxCapacity, int bufferSize, int opsToPreAlloc, ISocketReceiver receiver)
        {
            _minCapacity = minCapacity;
            _bufferSize = bufferSize;
            _receiver = receiver;
            _maxCapacity = maxCapacity;
            _pool = new Stack<SocketAsyncEventArgs>(_maxCapacity);
            long numSize = maxCapacity * bufferSize * opsToPreAlloc;
            _bufferManager = new BufferManager(numSize, bufferSize * opsToPreAlloc);
            _bufferManager.Init();
            _receiveHandle += OnSyncReceive;
            _currentUseNum = 0;

            for (int i = 0; i < _minCapacity; i++)
            {
                SocketAsyncEventArgs e = CreateSAEAObject();
                InitPushIntoPool(e);
                Interlocked.Increment(ref _SAEAObjectCount);
            }
        }

        private void InitPushIntoPool(SocketAsyncEventArgs e)
        {
            lock (_pool)
            {
                _pool.Push(e);
            }
        }

        /// <summary>
        /// 缓存管理
        /// </summary>
        internal BufferManager Buffers
        {
            get { return _bufferManager; }
        }

        /// <summary>
        /// 缓冲大小
        /// </summary>
        public int BufferSize
        {
            get { return _bufferSize; }
        }
        /// <summary>
        /// Max容量
        /// </summary>
        public int MaxCapacity
        {
            get { return _maxCapacity; }
        }

        /// <summary>
        /// 当前容量
        /// </summary>
        public int Capacity
        {
            get { return _pool.Count; }
        }

        /// <summary>
        /// 使用的容量
        /// </summary>
        public int UseCapacity
        {
            get { return _currentUseNum; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SocketAsyncEventArgs CreateSAEAObject()
        {
            SocketAsyncEventArgs readWriteEventArg = new SocketAsyncEventArgs();
            readWriteEventArg.Completed += OnIoCompleted;
            readWriteEventArg.SetBuffer(new Byte[_bufferSize], 0, _bufferSize);
            return readWriteEventArg;
        }

        /// <summary>
        /// 取出SocketAsyncEventArgs对象
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Pop()
        {
            lock (_pool)
            {
                SocketAsyncEventArgs e = null;
                Interlocked.Increment(ref _currentUseNum);
                if (_pool.Count > 0)
                {
                    e = _pool.Pop();
                }
                else if (_maxCapacity == 0 || _SAEAObjectCount < _maxCapacity)
                {
                    e = CreateSAEAObject();
                    Interlocked.Increment(ref _SAEAObjectCount);
                }
                if (e != null)
                {
                    _bufferManager.SetBuffer(e);
                    return e;
                }
                return null;
            }
        }

        /// <summary>
        /// 释放SAEA对象，并放入池
        /// </summary>
        /// <param name="item"></param>
        public void ReleaseSAEAToPush(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            lock (_pool)
            {
                _bufferManager.FreeBuffer(item);
                Interlocked.Decrement(ref _currentUseNum);
                var session = (item.UserToken as SocketSession);
                string remoteAddress = session != null ? session.RemoteAddress : "0.0.0.0";
                if (session != null)
                {
                    session.DisConnection();
                }
                if (item.AcceptSocket != null && item.AcceptSocket.Connected)
                {
                    try
                    {
                        item.AcceptSocket.Shutdown(SocketShutdown.Both);
                        item.AcceptSocket.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
                item.AcceptSocket = null;
                item.UserToken = null;
                _pool.Push(item);
                TraceLog.ReleaseWrite("The {0} socket has be connected.", remoteAddress);
            }
        }

        private void OnIoCompleted(object sender, SocketAsyncEventArgs e)
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
        /// 发送处理
        /// </summary>
        /// <param name="sendArgs"></param>
        private void ProcessSend(SocketAsyncEventArgs sendArgs)
        {
            SocketSession session = sendArgs.UserToken as SocketSession;
            if (sendArgs.BytesTransferred > 0)
            {
                if (sendArgs.SocketError == SocketError.Success)
                {
                    //处理上一次传送字节长度
                    session.SendPacket.SetRemainingByteCount(sendArgs.BytesTransferred);
#if DEBUG
    //todo trace
                Console.WriteLine("{0}->传送字节长度:{1}/{2}", 
                    DateTime.Now.ToLongTimeString(), 
                    sendArgs.BytesTransferred, 
                    session.SendPacket.RemainingByteCount);
#endif
                    if (session.SendPacket.RemainingByteCount <= 0)
                    {
                        StartReceive(sendArgs);
                    }
                    else
                    {
                        StartSend(sendArgs);
                    }
                }
                else
                {
                    ProcessError(sendArgs);
                }
            }
            else
            {
                CloseClientSocket(sendArgs);
            }
        }

        private void StartSend(SocketAsyncEventArgs sendArgs)
        {
            SocketSession session = sendArgs.UserToken as SocketSession;
            byte[] buffer = session.SendPacket.ReadBlockData(BufferSize);
            if (buffer.Length == 0)
            {
                return;
            }
            sendArgs.SetBuffer(session.bufferOffsetSend, buffer.Length);
            Buffer.BlockCopy(buffer, 0, sendArgs.Buffer, session.bufferOffsetSend, buffer.Length);

            if (!sendArgs.AcceptSocket.SendAsync(sendArgs))
            {
                // Read the next block of data send from the client.
                ProcessSend(sendArgs);
            }
        }

        internal void StartNewSend(SocketAsyncEventArgs sendArgs)
        {
            //var sendSAEAObject = CreateSAEAObject();
            //sendSAEAObject.AcceptSocket = sendArgs.AcceptSocket;
            //sendSAEAObject.UserToken = session;

            //Buffer.BlockCopy(buffer, 0, sendSAEAObject.Buffer, 0, buffer.Length);

            //if (!sendSAEAObject.AcceptSocket.SendAsync(sendSAEAObject))
            //{
            //    ProcessSend(sendSAEAObject);
            //}
        }
        private void AsynCallBack(IAsyncResult result)
        {
            try
            {
                Socket socket = result.AsyncState as Socket;
                if (socket != null)
                {
                    SocketError errorCode;
                    int resultByte = socket.EndSend(result, out errorCode);
                    if (errorCode != SocketError.Success)
                    {
                        TraceLog.WriteError("Send to {0} end error:{1}", socket.RemoteEndPoint, errorCode);
                    }
                    else
                    {
                        string str = string.Format("{0}>>Send to {1} {2}/{3}byte end.",
                                                   DateTime.Now.ToLongTimeString(), socket.RemoteEndPoint, resultByte,
                                                   socket.SendBufferSize);
                        Console.WriteLine(str);
                        TraceLog.ReleaseWrite(str);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 接收处理
        /// </summary>
        /// <param name="receiveArgs"></param>
        private void ProcessReceive(SocketAsyncEventArgs receiveArgs)
        {
            if (receiveArgs.BytesTransferred > 0)
            {
                SocketSession session = receiveArgs.UserToken as SocketSession;
                if (receiveArgs.SocketError == SocketError.Success)
                {
                    //是否有已接收但未处理的数据
                    session.ReceiveBuffer(receiveArgs);
                    if (session.ReceivePacket.HasCompleteBytes)
                    {
                        byte[] buffer;
                        bool hasNext = false;
                        do
                        {
                            hasNext = session.ReceivePacket.TryGetData(out buffer);
                            if (hasNext)
                            {
                                //处理接收数据
                                //_receiveHandle(session, buffer);
                                _receiveHandle.BeginInvoke(session, buffer, ReceiveCallback, session);
                            }

                        } while (hasNext);

                    }
                    if (session.ReceivePacket.RemainingByteCount > 0)
                    {
                        //是否有未接收完的流
                        StartReceive(receiveArgs);
                        return;
                    }
                    if (session.SendPacket.RemainingByteCount > 0)
                    {
                        StartSend(receiveArgs);
                    }
                    else
                    {
                        StartReceive(receiveArgs);
                    }

                }
                else
                {
                    ProcessError(receiveArgs);
                }
            }
            else
            {
                CloseClientSocket(receiveArgs);
            }
        }

        internal void StartReceive(SocketAsyncEventArgs receiveArgs)
        {
            SocketSession session = receiveArgs.UserToken as SocketSession;
            receiveArgs.SetBuffer(session.bufferOffsetReceive, BufferSize);

            if (!receiveArgs.AcceptSocket.ReceiveAsync(receiveArgs))
            {
                //Console.WriteLine("next trans {0} byte", e.Buffer.Length);
                // Read the next block of data sent by client.
                ProcessReceive(receiveArgs);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var session = ar.AsyncState as SocketSession;
            try
            {
                if (session != null)
                {
                    session.StartSend();
                    var ts = session.StopWatchTime();
                    string param = "";
                    MessageHead head = session.UserData as MessageHead;
                    if (head != null)
                    {
                        param = string.Format("Action:{0},MsgId:{1},Error:{2}-{3}",
                            head.Action, head.MsgId, head.ErrorCode, head.ErrorInfo);
                    }
                    TraceLog.WriteComplement("{0}参数:{1}运行结果:{2}ms", session.RemoteAddress, param, ts.TotalMilliseconds);
                }
            }
            catch (Exception er)
            {
                TraceLog.WriteError("AsyncReceive error:{0}", er);
            }
            _receiveHandle.EndInvoke(ar);
        }

        //同步处理请求响应接收
        private void OnSyncReceive(SocketSession session, byte[] buffer)
        {
            try
            {
                if (_receiver != null)
                {
                    session.StartWatchTime();
                    session.PutInSendQueue(_receiver.Receive(session, buffer));
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnAsyncReceive:{0}", ex);
            }
        }

        private void ProcessError(SocketAsyncEventArgs e)
        {
            SocketSession session = e.UserToken as SocketSession;
            string host = "Unknown";
            if (session != null)
            {
                IPEndPoint localEp = session.RemoteEndPoint;
                host = string.Format("{0}:{1}", localEp.Address, localEp.Port);
            }
            else if (e.AcceptSocket != null)
            {
                host = e.AcceptSocket.RemoteEndPoint.ToString();
            }
            TraceLog.WriteError("SocketAsyncPool {0} op {1} error:{2}-{3}", host, e.LastOperation, (int)e.SocketError, e.SocketError);

            if (session != null)
            {
                session.OnNotifyClosed();
            }
            SocketSessionPool.Current.Remove(e);
            ReleaseSAEAToPush(e);
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            SocketSession session = e.UserToken as SocketSession;
            this.CloseClientSocket(session, e);
        }

        private void CloseClientSocket(SocketSession session, SocketAsyncEventArgs asyncEventArgs)
        {
            string remoteAddress = session == null ? "Unknown" : session.RemoteAddress;
            TraceLog.ReleaseWrite("Socket client {0} is closed.", remoteAddress);
            if (session != null)
            {
                session.OnNotifyClosed();
            }
            SocketSessionPool.Current.Remove(asyncEventArgs);
            ReleaseSAEAToPush(asyncEventArgs);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            DoDispose(true);
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing"></param>
        private void DoDispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管对象
                _pool = null;
                _receiver = null;
                GC.SuppressFinalize(this);
            }
            //清理非托管对象

        }
    }
}

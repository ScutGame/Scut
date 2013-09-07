using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Socket客户端连接
    /// <code>
    /// <![CDATA[
    /// ]]>
    /// </code>
    /// </summary>
    public sealed class SocketClient : IDisposable
    {
        private readonly string _host;
        private readonly int _prot;
        private Socket _socket;
        private BufferPacket _revPacket;
        /// <summary>
        /// Socket对象
        /// </summary>
        public Socket Socket { get { return _socket; } }

        /// <summary>
        /// 用户数据
        /// </summary>
        public object UserToken
        {
            get;
            set;
        }
        /// <summary>
        /// 数据包长度
        /// </summary>
        public int BuffLength { get; set; }

        /// <summary>
        /// 数据输入处理
        /// </summary>
        public Action<SocketClient, byte[]> ReceiveHandle { get; set; }

        /// <summary>
        /// 异常错误通常是用户断开处理
        /// </summary>
        public Action<SocketClient, string> ErrorHandle { get; set; }

        /// <summary>
        /// 有连接
        /// </summary>
        public Action<bool> ConnectedHandle { get; set; }

        private SocketError socketError;

        public SocketClient(string host, int prot, int bufferSize)
        {
            _host = host;
            _prot = prot;
            BuffLength = bufferSize;
            _revPacket = new BufferPacket(bufferSize);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }


        /// <summary>
        ///连接到目标主机
        /// </summary>
        /// <param name="host">IP</param>
        /// <param name="prot">端口</param>
        public bool Connect()
        {
            try
            {
                _socket.Connect(_host, _prot);
                if (_socket.Connected)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (SocketException)
            {
                return false;
            }
            catch
            {
                throw;
            }

        }


        /// <summary>
        ///连接到目标主机
        /// </summary>
        /// <param name="host">IP</param>
        /// <param name="prot">端口</param>
        public void BeginConnect(string host, int prot)
        {
            try
            {
                _socket.BeginConnect(host, prot, new AsyncCallback(ConnAsyncCallBack), _socket);

            }
            catch
            {
                throw;
            }

        }

        private void ConnAsyncCallBack(IAsyncResult result)
        {
            try
            {
                _socket.EndConnect(result);

                if (_socket.Connected)
                {
                    if (ConnectedHandle != null)
                        ConnectedHandle(true);
                }
                else
                    if (ConnectedHandle != null)
                        ConnectedHandle(false);
            }
            catch (Exception)
            {
                if (ConnectedHandle != null)
                    ConnectedHandle(false);
            }
        }


        /// <summary>
        /// 接收结果
        /// </summary>
        public void ReceiveResult()
        {
            DoReceive();
        }

        public void ReceiveAsyncResult()
        {
            BeginReceive();
        }

        private void DoReceive()
        {
            while (true)
            {
                byte[] buffer = new byte[BuffLength];
                int count = _socket.Receive(buffer, 0, buffer.Length, SocketFlags.None, out socketError);
                byte[] data = new byte[count];
                Array.Copy(buffer, 0, data, 0, data.Length);
                if (!HasWaitReceiveData(data))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 接收数据处理
        /// </summary>
        /// <param name="data"></param>
        private bool HasWaitReceiveData(byte[] data)
        {
            _revPacket.InsertByteArray(data);
            if (_revPacket.HasCompleteBytes)
            {
                byte[] buffer;
                bool hasNext = false;
                do
                {
                    hasNext = _revPacket.TryGetData(out buffer);
                    if (hasNext)
                    {
                        //处理接收数据
                        if (this.ReceiveHandle != null)
                        {
                            this.ReceiveHandle(this, buffer);
                        }
                    }

                } while (hasNext);
            }
            return _revPacket.RemainingByteCount > 0;
        }

        private void BeginReceive()
        {
            byte[] data = new byte[BuffLength];
            _socket.BeginReceive(data, 0, data.Length, SocketFlags.None, out socketError, args_Completed, data);
        }

        private void args_Completed(IAsyncResult reault)
        {
            int cout = 0;
            try
            {
                cout = _socket.EndReceive(reault);
            }
            catch (SocketException e)
            {
                socketError = e.SocketErrorCode;
            }
            catch
            {
                socketError = SocketError.HostDown;
            }

            if (socketError == SocketError.Success && cout > 0)
            {
                byte[] buffer = reault.AsyncState as byte[];
                byte[] data = new byte[cout];
                Array.Copy(buffer, 0, data, 0, data.Length);
                HasWaitReceiveData(data);
                BeginReceive();
            }
            else
            {
                _socket.Close();
                if (ErrorHandle != null)
                    ErrorHandle(this, "Closed");
            }

        }

        /// <summary>
        /// 异步发送数据包
        /// </summary>
        /// <param name="data"></param>
        public void SendAsync(byte[] data)
        {
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, AsynCallBack, _socket);

        }

        private void AsynCallBack(IAsyncResult result)
        {
            try
            {
                Socket sock = result.AsyncState as Socket;

                if (sock != null)
                {
                    sock.EndSend(result);
                }
            }
            catch
            {

            }
        }


        public void Close()
        {
            _socket.Close();
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
        void DoDispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管对象
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
            if (_socket != null)
            {
                try
                {
                    IDisposable disposable = _socket;
                    disposable.Dispose();
                    _socket = null;
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
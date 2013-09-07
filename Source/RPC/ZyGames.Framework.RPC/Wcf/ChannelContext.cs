using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// 通道上下文
    /// </summary>
    public class ChannelContext : IDisposable
    {
        private int _identityId;
        internal OperationContext Operation;

        internal ChannelContext()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string SessionId
        {
            get { return Operation != null ? Operation.SessionId : ""; }
        }

        /// <summary>
        /// 身份标识ID
        /// </summary>
        public int IdentityId
        {
            get { return _identityId; }
            set { _identityId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RemoteAddress
        {
            get
            {
                return Operation != null
                    ? IdentityId + ":" + Operation.SessionId
                    : IdentityId.ToString();
            }
        }

        /// <summary>
        /// 下发通知
        /// </summary>
        public IWcfCallback GetCallback()
        {
            return Operation.GetCallbackChannel<IWcfCallback>();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConnect
        {
            get
            {
                return Operation != null &&
                    Operation.Channel != null &&
                    Operation.Channel.State == CommunicationState.Opened;
            }
        }
        /// <summary>
        /// 当前通道
        /// </summary>
        public IContextChannel Channel
        {
            get { return Operation != null ? Operation.Channel : null; }
        }

        /// <summary>
        /// 是否关闭状态
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// 开始处理请求
        /// </summary>
        public event RequestHandle OnRequested;

        /// <summary>
        /// 远端调用
        /// </summary>
        public event CallRemoteHandle OnCallRemote;

        /// <summary>
        /// 关闭
        /// </summary>
        public event ClosingHandle OnClosed;

        /// <summary>
        /// 关闭
        /// </summary>
        public event ClosingHandle OnSocketClosed;

        /// <summary>
        /// 开始处理请求
        /// </summary>
        /// <param name="param"></param>
        /// <param name="remoteAddress"></param>
        public byte[] Request(string param, string remoteAddress)
        {
            IsClosed = false;
            return OnRequested(this, param, remoteAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="callback"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public IAsyncResult BeginRequest(string param, string remoteAddress, AsyncCallback callback, object asyncState)
        {
            IsClosed = false;
            return OnRequested.BeginInvoke(this, param, remoteAddress, callback, asyncState);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public byte[] EndRequest(IAsyncResult result)
        {
            return OnRequested.EndInvoke(result);
        }

        /// <summary>
        /// 提供程序内部之间通信
        /// </summary>
        /// <param name="route"></param>
        /// <param name="param"></param>
        /// <param name="remoteAddress"></param>
        /// <returns></returns>
        public byte[] CallRemote(string route, string param, string remoteAddress)
        {
            IsClosed = false;
            return OnCallRemote(this, route, param, remoteAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="route"></param>
        /// <param name="param"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="callback"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public IAsyncResult BeginCallRemote(string route, string param, string remoteAddress, AsyncCallback callback, object asyncState)
        {
            IsClosed = false;
            return OnCallRemote.BeginInvoke(this, route, param, remoteAddress, callback, asyncState);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public byte[] EndCallRemote(IAsyncResult result)
        {
            return OnCallRemote.EndInvoke(result);
        }

        /// <summary>
        /// Socket连接关闭
        /// </summary>
        public void SocketClose(string remoteAddress)
        {
            OnSocketClosed(this, remoteAddress);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            DoDispose(true);
        }

        protected virtual void DoDispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管对象
                _identityId = 0;
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
        }

        ~ChannelContext()
        {
            DoDispose(false);
        }

    }
}
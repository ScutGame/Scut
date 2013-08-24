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
        private readonly string _sessionId;
        private int _identityId;
        private readonly OperationContext _context;

        internal ChannelContext(string sessionId, int identityId, OperationContext context)
        {
            _sessionId = sessionId;
            _identityId = identityId;
            _context = context;
            RemoteAddress = _context.Channel != null ? _context.Channel.RemoteAddress.ToString() : "";
        }

        /// <summary>
        /// 
        /// </summary>
        public string SessionId
        {
            get { return _sessionId; }
        }

        /// <summary>
        /// 身份标识ID
        /// </summary>
        public int IdentityId
        {
            get { return _identityId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RemoteAddress { get; set; }

        /// <summary>
        /// 下发通知
        /// </summary>
        public IWcfCallback GetCallback()
        {
            return _context.GetCallbackChannel<IWcfCallback>();
        }

        /// <summary>
        /// 当前通道
        /// </summary>
        public IContextChannel Channel
        {
            get { return _context.Channel; }
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
            return OnRequested(this, param, remoteAddress);
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
            return OnCallRemote(this, route, param, remoteAddress);
        }

        /// <summary>
        /// Socket连接关闭
        /// </summary>
        public void SocketClose(string remoteAddress)
        {
            OnSocketClosed(this, remoteAddress);
        }

        /// <summary>
        /// Wcf连接关闭
        /// </summary>
        public void Close(string remoteAddress)
        {
            OnClosed(this, remoteAddress);
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
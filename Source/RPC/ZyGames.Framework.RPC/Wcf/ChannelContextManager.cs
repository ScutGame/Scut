using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Wcf
{
    public delegate byte[] RequestHandle(ChannelContext context, string param, string remoteAddress);

    public delegate byte[] CallRemoteHandle(ChannelContext context, string route, string param, string remoteAddress);

    public delegate void ClosingHandle(ChannelContext context, string remoteAddress);

    /// <summary>
    /// 服务连接池管理
    /// </summary>
    public class ChannelContextManager : IDisposable
    {
        private static ChannelContextManager _instance;

        /// <summary>
        /// 连接池
        /// </summary>
        public static ChannelContextManager Current
        {
            get
            {
                return _instance;
            }
        }

        static ChannelContextManager()
        {
            _instance = new ChannelContextManager();
        }

        private ChannelContext _context;

        private ChannelContextManager()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        internal void Init()
        {
            _context = new ChannelContext();
            if (OnRequested != null)
                _context.OnRequested += new RequestHandle(OnRequested);
            if (OnCallRemote != null)
                _context.OnCallRemote += new CallRemoteHandle(OnCallRemote);
            if (OnClosing != null)
                _context.OnClosed += new ClosingHandle(OnClosing);
            if (OnSocketClosed != null)
                _context.OnSocketClosed += new ClosingHandle(OnSocketClosed);
        }
        /// <summary>
        /// 开始处理请求
        /// </summary>
        public event RequestHandle OnRequested;

        /// <summary>
        /// 远端调用
        /// </summary>
        public event CallRemoteHandle OnCallRemote;


        /// <summary>
        /// 连接关闭
        /// </summary>
        public event ClosingHandle OnClosing;

        /// <summary>
        /// Socket连接关闭
        /// </summary>
        public event ClosingHandle OnSocketClosed;

        /// <summary>
        /// 获取当前ID
        /// </summary>
        public int IdentityId
        {
            get
            {
                return _context.IdentityId;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CurrentSessionId
        {
            get
            {
                return _context.SessionId;
            }
        }
        /// <summary>
        /// 当前通道
        /// </summary>
        public ChannelContext CurrentChannel
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// Bind到连接池
        /// </summary>
        public void Bind(int identityId = 0)
        {
            OperationContext opContext = OperationContext.Current;
            if (opContext != null)
            {
#if DEBUG
                    //todo test
                    Console.WriteLine("{0}>>bind Sid:{1}", DateTime.Now.ToLongTimeString(), opContext.SessionId);
#endif
                opContext.Channel.Closing += ChannelClosing;
                opContext.Channel.Faulted += ChannelClosing;

                if (_context.SessionId != opContext.SessionId)
                {
                    _context.Operation = opContext;
                }
                _context.IsClosed = false;
            }
        }

        /// <summary>
        /// 通知当前通道
        /// </summary>
        /// <param name="param"></param>
        /// <param name="buffer"></param>
        public bool Notify(string param, byte[] buffer)
        {
            return Notify(CurrentSessionId, param, buffer);
        }

        /// <summary>
        /// 获取当前上下文通道
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="param"></param>
        /// <param name="buffer"></param>
        public bool Notify(string sessionId, string param, byte[] buffer)
        {
            return NotifyChannelReceive(_context, param, buffer);
        }

        /// <summary>
        /// 通知所有
        /// </summary>
        /// <param name="param"></param>
        /// <param name="buffer"></param>
        [Obsolete("method is removed", true)]
        public void NotifyAll(string param, byte[] buffer)
        {
            //NotifyAll(x => true, param, buffer);
        }


        private bool NotifyChannelReceive(ChannelContext context, string param, byte[] buffer)
        {
            //并发 
            lock (_instance)
            {
                if (context != null && context.IsConnect)
                {
                    try
                    {
                        var callback = context.GetCallback();
                        if (callback == null)
                        {
                            return false;
                        }
                        callback.Receive(param, buffer);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        context.IsClosed = true;
                        TraceLog.WriteError("Notify:{0} {1}", param, ex);
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }

        /// <summary>
        /// 通道关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelClosing(object sender, EventArgs e)
        {
            try
            {
                IWcfCallback callback = sender as IWcfCallback;
                if (callback == null)
                {
                    return;
                }
                if (_context != null && _context.GetCallback() == callback && !_context.IsClosed)
                {
                    _context.IsClosed = true;
                    try
                    {
                        if (OnClosing != null)
                        {
                            //业务层通知
                            OnClosing(_context, _context.RemoteAddress);
                        }
                    }
                    catch (Exception er)
                    {
                        TraceLog.WriteError("Business notify closing error:{0}", er);
                    }
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("ChannelClosing:{0}", ex);
            }
        }

    }
}

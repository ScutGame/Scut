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
    public class ChannelContextManager
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

        private readonly ConcurrentDictionary<string, ChannelContext> _pools;
        private int _identityId;

        private ChannelContextManager()
        {
            _pools = new ConcurrentDictionary<string, ChannelContext>();
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
                return _identityId;
            }
        }

        private string _currentSessionId;
        public string CurrentSessionId
        {
            get
            {
                return _currentSessionId;
            }
        }
        /// <summary>
        /// 当前通道
        /// </summary>
        public ChannelContext CurrentChannel
        {
            get
            {
                return GetChannel(_currentSessionId);
            }
        }

        /// <summary>
        /// Bind到连接池
        /// </summary>
        public void Bind(int identityId = 0)
        {
            _identityId = identityId;
            lock (_instance)
            {
                OperationContext opContext = OperationContext.Current;
                if (opContext != null)
                {
                    _currentSessionId = opContext.SessionId;
                    if (!ContainsKey(_currentSessionId))
                    {
                        opContext.Channel.Closing += ChannelClosing;
                        opContext.Channel.Faulted += ChannelClosing;
                        var channelContext = new ChannelContext(_currentSessionId, identityId, opContext);
                        channelContext.OnRequested += new RequestHandle(OnRequested);
                        channelContext.OnCallRemote += new CallRemoteHandle(OnCallRemote);
                        channelContext.OnClosed += new ClosingHandle(OnClosing);
                        channelContext.OnSocketClosed += new ClosingHandle(OnSocketClosed);
                        PutIntoPool(_currentSessionId, channelContext);
                    }
                }
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
            var context = GetChannel(sessionId);
            return NotifyChannelReceive(context, param, buffer);
        }

        /// <summary>
        /// 通知所有
        /// </summary>
        /// <param name="param"></param>
        /// <param name="buffer"></param>
        public void NotifyAll(string param, byte[] buffer)
        {
            NotifyAll(x => true, param, buffer);
        }

        /// <summary>
        /// 以身份标识获取匹配的上下文通道
        /// </summary>
        /// <param name="match">匹配多个连接通道</param>
        /// <param name="param">发下参数</param>
        /// <param name="buffer"></param>
        public void NotifyAll(Predicate<string> match, string param, byte[] buffer)
        {
            var list = GetChannel(match);
            foreach (var context in list)
            {
                if (context != null)
                {
                    NotifyChannelReceive(context, param, buffer);
                }
            }
        }

        private bool NotifyChannelReceive(ChannelContext context, string param, byte[] buffer)
        {
            if (context != null)
            {
                var callback = context.GetCallback();
                if (callback == null)
                {
                    return false;
                }
                //并发 
                lock (_instance)
                {
                    string sessionId = context.SessionId;
                    try
                    {
                        callback.Receive(param, buffer);
                        if (context.IsClosed)
                        {
                            callback.Close();
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        context.IsClosed = true;
                        Remove(sessionId);
                        TraceLog.WriteError("Notify:{0} {1}", param, ex);
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="sessionId"></param>
        public bool Remove(string sessionId)
        {
            ChannelContext context;
            if (_pools.TryRemove(sessionId, out context))
            {
                context.Dispose();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            Foreach((key, channel) =>
            {
                if (channel != null)
                {
                    channel.Dispose();
                }
                return true;
            });
            _pools.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public void Foreach(Func<string, ChannelContext, bool> func)
        {
            var e = _pools.GetEnumerator();
            while (e.MoveNext())
            {
                if (!func(e.Current.Key, e.Current.Value))
                {
                    break;
                }
            }
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
                Foreach((sessionId, channel) =>
                {
                    if (channel != null && channel.GetCallback() == callback)
                    {
                        channel.IsClosed = true;
                        try
                        {
                            if (OnClosing != null)
                            {
                                //业务层通知
                                OnClosing(channel, channel.RemoteAddress);
                            }
                        }
                        catch (Exception er)
                        {
                            TraceLog.WriteError("Business notify closing error:{0}", er);
                        }
                        TraceLog.ReleaseWrite("WcfChannel:{0} is closed.", sessionId);
                        return false;
                    }
                    return true;
                });

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("ChannelClosing:{0}", ex);
            }
        }

        private bool ContainsKey(string sessionId)
        {
            return _pools.ContainsKey(sessionId);
        }

        private void PutIntoPool(string sessionId, ChannelContext context)
        {
            ChannelContext oldValue;
            if (_pools.TryGetValue(sessionId, out oldValue))
            {
                _pools.TryUpdate(sessionId, context, oldValue);
            }
            else
            {
                _pools.TryAdd(sessionId, context);
            }
        }


        /// <summary>
        /// 获取当前上下文通道
        /// </summary>
        /// <param name="sessionId"></param>
        /// <exception cref="NullReferenceException"></exception>
        private ChannelContext GetChannel(string sessionId)
        {
            ChannelContext context;
            if (_pools.TryGetValue(sessionId, out context))
            {
                return context;
            }
            return null;
        }

        /// <summary>
        /// 以身份标识获取匹配的上下文通道
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private List<ChannelContext> GetChannel(Predicate<string> match)
        {
            List<ChannelContext> list = new List<ChannelContext>();
            Foreach((sessionId, channel) =>
            {
                if (channel != null)
                {
                    if (match(sessionId))
                    {
                        list.Add(channel);
                    }
                }
                return true;
            });
            return list;
        }

    }
}

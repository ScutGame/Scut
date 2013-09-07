using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// 实例使用Single，共享一个
    /// 并发使用Mutiple, 支持多线程访问(一定要加锁)
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class WcfService : IWcfService
    {
        /// <summary>
        /// 
        /// </summary>
        public WcfService()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityId"></param>
        public void Bind(int identityId)
        {
            ChannelContextManager.Current.Bind(identityId);
        }

        public IAsyncResult BeginBind(int identityId, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndBind(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="remoteAddress">远程连接地址</param>
        /// <returns></returns>
        public byte[] Request(string param, string remoteAddress)
        {
            var channel = ChannelContextManager.Current.CurrentChannel;
            if (channel != null)
            {
                return channel.Request(param, remoteAddress);
            }
            return null;
        }

        public IAsyncResult BeginRequest(string param, string remoteAddress, AsyncCallback callback, object asyncState)
        {
            return ChannelContextManager.Current.CurrentChannel.BeginRequest(param, remoteAddress, callback, asyncState);
        }

        public byte[] EndRequest(IAsyncResult result)
        {
            return ChannelContextManager.Current.CurrentChannel.EndRequest(result);
        }

        /// <summary>
        /// 提供程序内部之间通信，参数不需要验证
        /// </summary>
        /// <param name="route"></param>
        /// <param name="param"></param>
        /// <param name="remoteAddress"></param>
        /// <returns></returns>
        public byte[] CallRemote(string route, string param, string remoteAddress)
        {
            var channel = ChannelContextManager.Current.CurrentChannel;
            if (channel != null)
            {
                return channel.CallRemote(route, param, remoteAddress);
            }
            return null;
        }

        public IAsyncResult BeginCallRemote(string route, string param, string remoteAddress, AsyncCallback callback, object asyncState)
        {
            return ChannelContextManager.Current.CurrentChannel.BeginCallRemote(route, param, remoteAddress, callback, asyncState);
        }

        public byte[] EndCallRemote(IAsyncResult result)
        {
            return ChannelContextManager.Current.CurrentChannel.EndCallRemote(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteAddress"></param>
        public void SocketClose(string remoteAddress)
        {
            var channel = ChannelContextManager.Current.CurrentChannel;
            if (channel != null)
            {
                channel.SocketClose(remoteAddress);
            }
        }
    }
}

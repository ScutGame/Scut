using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Wcf;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 请求处理宿主辅助类
    /// </summary>
    public abstract class GameHost
    {
        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            Type type = Type.GetType(ConfigUtils.GetSetting("Game.Host.TypeName"));
            if (type == null)
            {
                throw new Exception(string.Format("The config \"Game.Host.TypeName\" is empty."));
            }
            Start((GameHost)Activator.CreateInstance(type));
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="host"></param>
        public static void Start(GameHost host)
        {
            try
            {
                host.Bind();
                host.Listen();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TraceLog.WriteError("HostServer error:", ex);
            }
            finally
            {
                Console.WriteLine("Press any key to exit the listener!");
                Console.ReadKey();
                if (host != null)
                {
                    host.Stop();
                }
            }
        }

        protected WcfServiceProxy ServiceProxy;

        /// <summary>
        /// 
        /// </summary>
        protected GameHost()
        {
            ServiceProxy = new WcfServiceProxy();
            ServiceProxy.ClosedHandle += new EventHandler(OnServiceProxyClosed);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Bind()
        {
            string host = ActionFactory.ActionConfig.Current.IpAddress;
            int port = ActionFactory.ActionConfig.Current.Port;
            Bind(host, port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="connectTimeout"></param>
        /// <param name="inactivityTimeout"></param>
        public void Bind(string host, int port, int connectTimeout = 10, int inactivityTimeout = 10)
        {
            Bind(host, port, new TimeSpan(0, 0, connectTimeout), TimeSpan.MaxValue, new TimeSpan(0, 0, inactivityTimeout));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Bind(string host, int port, TimeSpan connectTimeout, TimeSpan receiveTimeout, TimeSpan inactivityTimeout)
        {
            ServiceProxy.Setting = new BindingBehaviorSetting(host, port);
            ServiceProxy.Setting.InactivityTimeout = inactivityTimeout;
            ServiceProxy.Setting.ConnectTimeout = connectTimeout;
            ServiceProxy.Setting.ReceiveTimeout = receiveTimeout;

            ChannelContextManager.Current.OnRequested += new RequestHandle(OnRequestComplated);
            ChannelContextManager.Current.OnCallRemote += new CallRemoteHandle(OnCallRemoteComplated);
            ChannelContextManager.Current.OnClosing += new ClosingHandle(OnClosed);
            ChannelContextManager.Current.OnSocketClosed += new ClosingHandle(OnSocketClosed);
            DoBindAfter();
        }

        protected virtual void DoBindAfter()
        {
            //在应用程序启动时运行的代码
            int cacheInterval = 600;
            GameEnvironment.Start(cacheInterval, () =>
            {
                return true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Listen()
        {
            DoListen();
            ServiceProxy.Listen();
            ListenAfter();
        }

        /// <summary>
        /// 监听启动之前处理
        /// </summary>
        protected virtual void DoListen()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ListenAfter()
        {
            Console.WriteLine("Game server {0} has been started, is listening ...", ServiceProxy.Setting.Url);
        }

        protected abstract void OnRequested(HttpGet httpGet, IGameResponse response);

        protected abstract void OnCallRemote(string route, HttpGet httpGet, MessageHead head, MessageStructure structure);

        protected abstract void OnClosed(ChannelContext context, string remoteaddress);

        protected abstract void OnSocketClosed(ChannelContext context, string remoteaddress);

        /// <summary>
        /// 服务停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected abstract void OnServiceStop(object sender, EventArgs eventArgs);

        /// <summary>
        /// 
        /// </summary>
        public virtual void Stop()
        {
            GameEnvironment.Stop();
            ServiceProxy.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="param"></param>
        /// <param name="remoteAddress"></param>
        /// <returns></returns>
        private byte[] OnRequestComplated(ChannelContext context, string param, string remoteAddress)
        {
            lock (context)
            {
                SocketGameResponse response = new SocketGameResponse();
                HttpGet httpGet = new HttpGet(param, remoteAddress);
                OnRequested(httpGet, response);
                return response.ReadByte();
            }
        }

        protected byte[] OnCallRemoteComplated(ChannelContext context, string route, string param, string remoteAddress)
        {
            lock (context)
            {
                HttpGet httpGet = new HttpGet(param, remoteAddress);
                MessageStructure structure = new MessageStructure();
                MessageHead head = new MessageHead();
                OnCallRemote(route, httpGet, head, structure);
                structure.WriteBuffer(head);
                return structure.ReadBuffer();
            }
        }

        private void OnServiceProxyClosed(object sender, EventArgs e)
        {
            try
            {
                OnServiceStop(sender, e);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnServiceProxyClosed:{0}", ex);
            }
        }

    }
}

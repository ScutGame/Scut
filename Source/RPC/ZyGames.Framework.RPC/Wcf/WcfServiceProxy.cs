using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;

namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// 处理服务请求代理操作
    /// </summary>
    public class WcfServiceProxy : IDisposable
    {
        private ServiceHost _serviceHost;
        private string _ipAddress;
        private int _port;
        private string _serviceUrl;


        public WcfServiceProxy()
        {
            _ipAddress = "";
            _serviceUrl = "";
        }
        /// <summary>
        /// 
        /// </summary>
        public string IPAddress
        {
            get { return _ipAddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Port
        {
            get { return _port; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceUrl
        {
            get { return _serviceUrl; }
        }

        public event EventHandler ClosedHandle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="connectTimeout"></param>
        /// <param name="inactivityTimeout"></param>
        /// <param name="connectionCount"></param>
        public void Listen(int port, int connectTimeout = 10, int inactivityTimeout = 30, int connectionCount = 100)
        {
            Listen(port, new TimeSpan(0, 0, connectTimeout), TimeSpan.MaxValue, new TimeSpan(0, 0, inactivityTimeout), connectionCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="connectTimeout">建立连接和传送数据的超时间</param>
        /// <param name="receiveTimeout">保持连接的情况下，空闲超时触发Closing事件</param>
        /// <param name="inactivityTimeout">连接断开的情况下，空闲超时触发Faulted事件,要小于receiveTimeout时间间隔</param>
        /// <param name="connectionCount">并发连接数</param>
        public void Listen(int port, TimeSpan connectTimeout, TimeSpan receiveTimeout, TimeSpan inactivityTimeout, int connectionCount = 100)
        {
            IPAddress[] addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
            string localIp = "127.0.0.1";
            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                    break;
                }
            }
            Listen(localIp, port, connectTimeout, receiveTimeout, inactivityTimeout, connectionCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="connectTimeout"></param>
        /// <param name="inactivityTimeout"></param>
        /// <param name="connectionCount"></param>
        public void Listen(string ip, int port, int connectTimeout = 10, int inactivityTimeout = 30, int connectionCount = 100)
        {
            Listen(ip, port, new TimeSpan(0, 0, connectTimeout), TimeSpan.MaxValue, new TimeSpan(0, 0, inactivityTimeout), connectionCount);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="connectTimeout">建立连接和传送数据的超时间</param>
        /// <param name="receiveTimeout">保持连接的情况下，空闲超时触发Closing事件</param>
        /// <param name="inactivityTimeout">连接断开的情况下，空闲超时触发Faulted事件,要小于receiveTimeout时间间隔</param>
        /// <param name="connectionCount">并发连接数</param>
        public void Listen(string ip, int port, TimeSpan connectTimeout, TimeSpan receiveTimeout, TimeSpan inactivityTimeout, int connectionCount)
        {
            _ipAddress = ip;
            _port = port;
            _serviceUrl = string.Format("net.tcp://{0}:{1}/WcfService", ip, port);
            Uri baseAddress = new Uri(_serviceUrl);
            _serviceHost = new ServiceHost(typeof(WcfService), baseAddress);
            //Set binding
            NetTcpBinding binding = new NetTcpBinding();
            binding.ReliableSession.Enabled = true;
            binding.ReliableSession.InactivityTimeout = inactivityTimeout;
            binding.Security.Mode = SecurityMode.None;
            binding.OpenTimeout = connectTimeout;
            binding.SendTimeout = connectTimeout;
            binding.OpenTimeout = connectTimeout;
            binding.CloseTimeout = connectTimeout;
            binding.ReceiveTimeout = receiveTimeout;
            //Add endpoints
            var contract = ContractDescription.GetContract(typeof(IWcfService));
            ServiceEndpoint endpoint = new ServiceEndpoint(contract, binding, new EndpointAddress(baseAddress));
            _serviceHost.Description.Endpoints.Add(endpoint);
            //Add Behaviors
            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            _serviceHost.Description.Behaviors.Add(behavior);
            var throttling = new ServiceThrottlingBehavior();
            throttling.MaxConcurrentCalls = connectionCount;
            throttling.MaxConcurrentInstances = connectionCount;
            throttling.MaxConcurrentSessions = connectionCount;
            _serviceHost.Description.Behaviors.Add(throttling);

            _serviceHost.Closing += OnServiceClosing;
            _serviceHost.Faulted += OnServiceClosing;
            _serviceHost.Open();
        }

        private void OnServiceClosing(object sender, EventArgs e)
        {
            try
            {
                if (ClosedHandle != null)
                {
                    ClosedHandle(sender, e);
                }
                ChannelContextManager.Current.Foreach((identity, channel) =>
                {
                    try
                    {
                        if (channel != null)
                        {
                            TraceLog.ReleaseWrite("WcfChannel:{0} callback is closed.", identity);
                            var callback = channel.GetCallback();
                            if (callback != null)
                            {
                                callback.Close();
                            }
                        }
                    }
                    catch
                    {
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnServiceClosing:{0}", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            DoDispose(true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void DoDispose(bool disposing)
        {
            if (disposing)
            {
                if (_serviceHost != null)
                {
                    _serviceHost.Close();
                    (_serviceHost as IDisposable).Dispose();
                }
                //清理托管对象
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
        }
        /// <summary>
        /// 
        /// </summary>
        ~WcfServiceProxy()
        {
            DoDispose(false);
        }
    }
}

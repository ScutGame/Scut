using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
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
        private BindingBehaviorSetting _setting;

        /// <summary>
        /// 
        /// </summary>
        public WcfServiceProxy()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public WcfServiceProxy(BindingBehaviorSetting setting)
        {
            _setting = setting;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceUrl
        {
            get { return _setting.Url; }
        }
        /// <summary>
        /// Wcf配置
        /// </summary>
        public BindingBehaviorSetting Setting
        {
            get { return _setting; }
            set { _setting = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ClosedHandle;

        /// <summary>
        /// 
        /// </summary>
        public void Listen()
        {
            if (_setting == null)
            {
                throw new InstanceNotFoundException("The setting is empty.");
            }
            ChannelContextManager.Current.Init();
            Uri baseAddress = new Uri(_setting.Url);
            _serviceHost = new ServiceHost(typeof(WcfService), baseAddress);
            //Set binding
            NetTcpBinding binding = new NetTcpBinding();
            binding.ReliableSession.Enabled = true;
            binding.ReliableSession.InactivityTimeout = _setting.InactivityTimeout;
            binding.Security.Mode = SecurityMode.None;
            binding.OpenTimeout = _setting.ConnectTimeout;
            binding.CloseTimeout = _setting.ConnectTimeout;
            binding.SendTimeout = _setting.SendTimeout;
            binding.ReceiveTimeout = _setting.ReceiveTimeout;
            //Add endpoints
            var contract = ContractDescription.GetContract(typeof(IWcfService));
            ServiceEndpoint endpoint = new ServiceEndpoint(contract, binding, new EndpointAddress(baseAddress));
            _serviceHost.Description.Endpoints.Add(endpoint);
            //Add Behaviors
            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            _serviceHost.Description.Behaviors.Add(behavior);
            var throttling = new ServiceThrottlingBehavior();
            throttling.MaxConcurrentCalls = _setting.MaxConcurrentCalls;
            throttling.MaxConcurrentInstances = _setting.MaxConcurrentInstances;
            throttling.MaxConcurrentSessions = _setting.MaxConcurrentSessions;
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
                TraceLog.ReleaseWrite("WcfChannel:{0} callback is closed.", ChannelContextManager.Current.IdentityId);
                var callback = ChannelContextManager.Current.CurrentChannel.GetCallback();
                if (callback != null)
                {
                    callback.Closing();
                }

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

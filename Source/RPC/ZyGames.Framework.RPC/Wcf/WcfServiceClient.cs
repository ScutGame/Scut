using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;

namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// 客户端调用服务操作
    /// </summary>
    public class WcfServiceClient : IDisposable
    {
        private const int MaxReceivedSize = 65535000;
        private string _ip;
        private int _port;
        private readonly int _identityId;
        private bool _connected;
        private IWcfService _serviceChannel;
        private DuplexChannelFactory<IWcfService> _channelFactory;
        private TimeSpan _connectTimeout;
        private bool _connectError;
        private EndpointAddress _address;
        private WcfCallback _callbackHandle;
        private Binding _binding;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityId">身份标识ID</param>
        public WcfServiceClient(int identityId)
        {
            _identityId = identityId;
        }

        /// <summary>
        /// 身份标识
        /// </summary>
        public int IdentityId
        {
            get { return _identityId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// 已连接
        /// </summary>
        public bool Connected
        {
            get { return _channelFactory.State == CommunicationState.Opened; }
        }
        /// <summary>
        /// 连接是否出错
        /// </summary>
        public bool ConnectError
        {
            get { return _connectError; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan ConnectTimeout
        {
            get
            {
                return _connectTimeout;
            }
        }

        private TimeSpan _inactivityTimeout;

        private TimeSpan _receiveTimeout;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="receivedCallback"></param>
        /// <param name="connectTimeout"></param>
        /// <param name="inactivityTimeout"></param>
        public void Bind(string ip, int port, Action<string, byte[]> receivedCallback, int connectTimeout = 10, int inactivityTimeout = 30)
        {
            TimeSpan receiveTimeout = TimeSpan.MaxValue;
            Bind(ip, port, receivedCallback, connectTimeout, receiveTimeout, inactivityTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="receivedCallback"></param>
        /// <param name="connectTimeout">建立连接和传送数据的超时间</param>
        /// <param name="receiveTimeout">保持连接的情况下，空闲超时触发Closing事件</param>
        /// <param name="inactivityTimeout">连接断开的情况下，空闲超时触发Faulted事件,要小于receiveTimeout时间间隔</param>
        public void Bind(string ip, int port, Action<string, byte[]> receivedCallback, int connectTimeout, TimeSpan receiveTimeout, int inactivityTimeout)
        {
            var setting = new BindingBehaviorSetting(ip, port);
            _connectTimeout = new TimeSpan(0, 0, connectTimeout);
            _receiveTimeout = receiveTimeout;
            _inactivityTimeout = new TimeSpan(0, 0, inactivityTimeout);
            setting.ConnectTimeout = _connectTimeout;
            setting.ReceiveTimeout = _receiveTimeout;
            setting.InactivityTimeout = _inactivityTimeout;
            Binding binding = CreateBinding(BindingType.NetTcpBinding, setting);
            Bind(ip, port, binding, receivedCallback);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Bind(string ip, int port, Binding binding, Action<string, byte[]> receivedCallback)
        {
            _ip = ip;
            _port = port;
            _binding = binding;
            Uri uri = new Uri(string.Format("net.tcp://{0}:{1}/WcfService", _ip, _port));
            _address = new EndpointAddress(uri);
            _callbackHandle = new WcfCallback();
            _callbackHandle.OnReceived += receivedCallback;
            _callbackHandle.OnClosed += OnWcfClose;
            ResetChannel();
        }

        private void ChannelClosing(object sender, EventArgs e)
        {
            _connected = false;
            _connectError = true;
            TraceLog.ReleaseWrite("The wcfchannel:{0} connection was closed!", _identityId);
        }

        private void ChannelFaulted(object sender, EventArgs e)
        {
            _connected = false;
            _connectError = true;
            TraceLog.ReleaseWrite("The wcfchannel:{0} connection was faulted!", _identityId);
        }

        private DuplexChannelFactory<IWcfService> BuildChannel()
        {
            var ic = new InstanceContext(_callbackHandle);
            var channelFactory = new DuplexChannelFactory<IWcfService>(ic, _binding, _address);
            channelFactory.Closing += ChannelClosing;
            channelFactory.Faulted += ChannelFaulted;
            return channelFactory;
        }

        /// <summary>
        /// 重置连接通道
        /// </summary>
        public void ResetChannel()
        {
            _connected = false;
            if (_channelFactory == null ||
                _channelFactory.State == CommunicationState.Faulted ||
                _channelFactory.State == CommunicationState.Closed)
            {
                _channelFactory = BuildChannel();
            }
            _serviceChannel = _channelFactory.CreateChannel();

        }

        /// <summary>
        /// 连接
        /// </summary>
        public bool Connect()
        {
            _serviceChannel.Bind(_identityId);
            _connected = true;
            _connectError = false;
            return _connected;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            _channelFactory.Abort();
            _channelFactory.Close();
            OnWcfClose(this, new EventArgs());
        }

        private void OnWcfClose(object sender, EventArgs e)
        {
            _connected = false;
            TraceLog.ReleaseWrite("The wcfchannel:{0} connection was closed!", _identityId);
        }

        /// <summary>
        /// 请求服务
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="remoteAddress">远程连接地址</param>
        /// <param name="buffer"></param>
        /// <remarks>在当前项目建立Remote目录,将处理Remote的Action类放在Remote目录下</remarks>
        /// <returns></returns>
        public bool TryRequest(string param, string remoteAddress, out byte[] buffer)
        {
            param = param ?? "";
            buffer = null;
            if (_serviceChannel != null)
            {
                buffer = _serviceChannel.Request(param, remoteAddress);
                return buffer != null;
            }
            return false;
        }

        /// <summary>
        /// 远端调用
        /// </summary>
        /// <param name="route"></param>
        /// <param name="param"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool TryCallRemote(string route, string param, string remoteAddress, out byte[] buffer)
        {
            param = param ?? "";
            buffer = null;
            if (_serviceChannel != null)
            {
                buffer = _serviceChannel.CallRemote(route, param, remoteAddress);
                return buffer != null;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingType"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public Binding CreateBinding(BindingType bindingType, BindingBehaviorSetting setting)
        {
            Binding bindinginstance = null;
            switch (bindingType)
            {
                case BindingType.BasicHttpBinding:
                    BasicHttpBinding basicHttp = new BasicHttpBinding();
                    basicHttp.MaxReceivedMessageSize = MaxReceivedSize;
                    bindinginstance = basicHttp;
                    break;
                case BindingType.NetNamedPipeBinding:
                    NetNamedPipeBinding wsPipe = new NetNamedPipeBinding();
                    wsPipe.MaxReceivedMessageSize = MaxReceivedSize;
                    bindinginstance = wsPipe;
                    break;
                case BindingType.NetPeerTcpBinding:
                    NetPeerTcpBinding wsPeerTcp = new NetPeerTcpBinding();
                    wsPeerTcp.MaxReceivedMessageSize = MaxReceivedSize;
                    bindinginstance = wsPeerTcp;
                    break;
                case BindingType.NetTcpBinding:
                    NetTcpBinding wsTcp = new NetTcpBinding();
                    wsTcp.MaxReceivedMessageSize = MaxReceivedSize;
                    wsTcp.ReliableSession.Enabled = true;
                    wsTcp.ReliableSession.InactivityTimeout = setting.InactivityTimeout;
                    wsTcp.Security.Mode = SecurityMode.None;
                    wsTcp.OpenTimeout = setting.ConnectTimeout;
                    wsTcp.CloseTimeout = setting.ConnectTimeout;
                    wsTcp.SendTimeout = setting.SendTimeout;
                    wsTcp.ReceiveTimeout = setting.ReceiveTimeout;
                    bindinginstance = wsTcp;
                    break;
                case BindingType.WsDualHttpBinding:
                    WSDualHttpBinding wsDual = new WSDualHttpBinding();
                    wsDual.MaxReceivedMessageSize = MaxReceivedSize;
                    bindinginstance = wsDual;
                    break;
                case BindingType.WsFederationHttpBinding:
                    WSFederationHttpBinding wsFederation = new WSFederationHttpBinding();
                    wsFederation.MaxReceivedMessageSize = MaxReceivedSize;
                    bindinginstance = wsFederation;
                    break;
                case BindingType.WsHttpBinding:
                    WSHttpBinding wsHttp = new WSHttpBinding(SecurityMode.None);
                    wsHttp.MaxReceivedMessageSize = MaxReceivedSize;
                    wsHttp.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                    wsHttp.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                    bindinginstance = wsHttp;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("bindingType");
            }
            return bindinginstance;
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
                //清理托管对象
                _serviceChannel = null;
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
            if (_channelFactory != null)
            {
                try
                {
                    IDisposable disposable = _channelFactory as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                    else
                    {
                        _channelFactory.Abort();
                        _channelFactory.Close();
                    }
                    _channelFactory = null;
                }
                catch (Exception)
                {
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        ~WcfServiceClient()
        {
            DoDispose(false);
        }

    }
}

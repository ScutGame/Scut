using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// Wcf配置
    /// </summary>
    public class BindingBehaviorSetting
    {
        private readonly string _host;
        private readonly int _port;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        public BindingBehaviorSetting(int port)
        {
            _host = Dns.GetHostName();
            _port = port;
            var ipAddress = Dns.GetHostAddresses(_host);
            var address = ipAddress.First(p => p.AddressFamily == AddressFamily.InterNetwork);
            if (address != null)
            {
                _host = address.ToString();
            }
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="port">端口</param>
        public BindingBehaviorSetting(string host, int port)
        {
            _host = host;
            _port = port;
            Init();
        }

        private void Init()
        {
            Url = string.Format("net.tcp://{0}:{1}/WcfService", _host, _port);
            InactivityTimeout = new TimeSpan(0, 0, 10);
            ConnectTimeout = new TimeSpan(0, 0, 5);
            SendTimeout = new TimeSpan(0, 0, 30);
            ReceiveTimeout = new TimeSpan(0, 30, 0);
            SetMaxConcurrent(10);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Host { get { return _host; } }
        /// <summary>
        /// 
        /// </summary>
        public int Port { get { return _port; } }

        /// <summary>
        /// 服务的URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 连接断开时，保持等待时间间隔，超时触发Faulted事件，小于receiveTimeout时间间隔(10s)
        /// </summary>
        public TimeSpan InactivityTimeout { get; set; }

        /// <summary>
        /// 创建与关闭连接失败或引发异常时等待时间(5s)
        /// </summary>
        public TimeSpan ConnectTimeout { get; set; }
        /// <summary>
        /// 在传输完成写入操作的间隔时间(30s)
        /// </summary>
        public TimeSpan SendTimeout { get; set; }
        /// <summary>
        /// 保持等待接收的空闲时间，用于设置长连接空闲时间,超时触发Closing事件(30m)
        /// </summary>
        public TimeSpan ReceiveTimeout { get; set; }
        /// <summary>
        /// 并发处理的消息数(10)
        /// </summary>
        public int MaxConcurrentCalls { get; set; }
        /// <summary>
        /// 并发执行的对象数(10)
        /// </summary>
        public int MaxConcurrentInstances { get; set; }
        /// <summary>
        /// 并发可接受的会话数(10)
        /// </summary>
        public int MaxConcurrentSessions { get; set; }

        /// <summary>
        /// 设置最大并发
        /// </summary>
        /// <param name="max"></param>
        public void SetMaxConcurrent(int max)
        {
            MaxConcurrentCalls = max;
            MaxConcurrentInstances = max;
            MaxConcurrentSessions = max;
        }
    }
}

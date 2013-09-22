using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketSettings
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IPEndPoint GetHostAddress(int port)
        {
            return GetHostAddress(Dns.GetHostName(), port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IPEndPoint GetHostAddress(string host, int port)
        {
            var ipAddressList = Dns.GetHostAddresses(host);
            var ipAddress = Array.Find(ipAddressList, m => m.AddressFamily == AddressFamily.InterNetwork);
            return new IPEndPoint(ipAddress, port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IPEndPoint GetHostV6Address(string host, int port)
        {
            var ipAddressList = Dns.GetHostAddresses(host);
            var ipAddress = Array.Find(ipAddressList, m => m.AddressFamily == AddressFamily.InterNetworkV6);
            return new IPEndPoint(ipAddress, port);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localEndPoint"></param>
        /// <param name="maxConnection"></param>
        /// <param name="bufferSize"></param>
        /// <param name="backlog"></param>
        public SocketSettings(IPEndPoint localEndPoint, int maxConnection, int bufferSize, int backlog)
        {
            LocalEndPoint = localEndPoint;
            MaxConnection = maxConnection;
            NumOfSaeaForRecSend = maxConnection * 2;
            BufferSize = bufferSize;
            Backlog = backlog;
            ContinuedTimeout = 300;
        }
        /// <summary>
        /// 
        /// </summary>
        public int NumOfSaeaForRecSend { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IPEndPoint LocalEndPoint { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int MaxConnection { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int BufferSize { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Backlog { get; private set; }

        /// <summary>
        /// 启用接收处理响应超时
        /// </summary>
        public bool EnableReceiveTimeout { get; set; }
        /// <summary>
        /// 接收处理响应超时（毫秒）
        /// </summary>
        public int ReceiveTimeout { get; set; }

        /// <summary>
        /// 持续连接超时时间（秒）
        /// </summary>
        public int ContinuedTimeout { get; set; }
    }
}

using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading;

namespace ZyGames.Framework.RPC.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpConnection
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid SSID;
        /// <summary>
        /// 
        /// </summary>
        public HttpListenerContext Context;
        /// <summary>
        /// 
        /// </summary>
        public Timer TimeoutTimer;
        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection Param;
    }
}
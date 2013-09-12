namespace ZyGames.Framework.RPC.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpSettings()
        {
            HostAddress = "http://127.0.0.1";
            Port = 80;
            RequestTimeout = 120 * 1000;
        }

        /// <summary>
        /// IP或域名
        /// </summary>
        public string HostAddress { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 游戏程序名
        /// </summary>
        public string GameAppName { get; set; }

        /// <summary>
        /// 请求超时（毫秒）
        /// </summary>
        public int RequestTimeout { get; set; }
    }
}
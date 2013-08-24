namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestSettings
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="serverId"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="paramString"></param>
        /// <param name="routeName"></param>
        public RequestSettings(int gameId, int serverId, string remoteAddress, string paramString = "", string routeName = "")
        {
            GameId = gameId;
            ServerId = serverId;
            RemoteAddress = remoteAddress;
            ParamString = paramString;
            RouteName = routeName;
        }

        /// <summary>
        /// 
        /// </summary>
        public int GameId;
        /// <summary>
        /// 
        /// </summary>
        public int ServerId;

        /// <summary>
        /// 
        /// </summary>
        public int ActionId;
        /// <summary>
        /// 路由名
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ParamString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RemoteAddress { get; set; }
    }
}
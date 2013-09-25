using System;
using System.Net;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class GameHostSetting
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="serverId"></param>
        /// <param name="remotePoint"></param>
        public GameHostSetting(int gameId, int serverId, IPEndPoint remotePoint)
        {
            GameId = gameId;
            ServerId = serverId;
            RemotePoint = remotePoint;
            ConnectTimeout = 3;
            ConnectNum = 10;
            IntervalTime = 10;
            EnableGzip = false;
            ClientNum = 10;
            BufferSize = 1024;
        }

        /// <summary>
        /// 客户端数
        /// </summary>
        public int ClientNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IPEndPoint RemotePoint { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int BufferSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GameId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int ServerId { get; private set; }
        /// <summary>
        /// 连接超时（秒）
        /// </summary>
        public int ConnectTimeout { get; set; }
        /// <summary>
        /// 心跳包间隔时间（秒）
        /// </summary>
        public int IntervalTime { get; set; }
        /// <summary>
        /// 是否开启Gzip压缩
        /// </summary>
        public bool EnableGzip { get; set; }

        /// <summary>
        /// 启用的连接数
        /// </summary>
        public int ConnectNum { get; set; }
    }
}
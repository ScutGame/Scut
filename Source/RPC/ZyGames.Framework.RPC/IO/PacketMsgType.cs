namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 
    /// </summary>
    public enum PacketMsgType
    {
        /// <summary>
        /// 心跳包
        /// </summary>
        None = 0,
        /// <summary>
        /// 注册
        /// </summary>
        Register = 1,
        /// <summary>
        /// 请求
        /// </summary>
        Request,
        /// <summary>
        /// 推送
        /// </summary>
        Push,
        /// <summary>
        /// 内部通道接入
        /// </summary>
        SendTo,
        /// <summary>
        /// 广播
        /// </summary>
        Broadcast,
        /// <summary>
        /// 关闭
        /// </summary>
        Closed,
        /// <summary>
        /// 未知
        /// </summary>
        Unknown
    }
}
using System;

namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// 请求错误
    /// </summary>
    public enum RequestError
    {
        /// <summary>
        /// 
        /// </summary>
        Success,
        /// <summary>
        /// 未能找到服务
        /// </summary>
        NotFindService = 100,
        /// <summary>
        /// 无法连接
        /// </summary>
        UnableConnect,
        /// <summary>
        /// 连接超时
        /// </summary>
        Timeout,
        /// <summary>
        /// 未知异常
        /// </summary>
        Unknown
    }
}

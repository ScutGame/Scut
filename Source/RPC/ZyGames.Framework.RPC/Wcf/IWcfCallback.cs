using System;
using System.ServiceModel;

namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// Wcf通讯回调接口
    /// </summary>
    public interface IWcfCallback
    {
        /// <summary>
        /// 客户端处理接收操作
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="buffer">下发字节流</param>
        [OperationContract(IsOneWay = true)]
        void Receive(string param, byte[] buffer);

        /// <summary>
        /// 通知关闭
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Close();
    }
}
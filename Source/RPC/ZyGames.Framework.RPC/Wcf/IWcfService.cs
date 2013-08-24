using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// Wcf通讯服务接口
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IWcfCallback))]
    internal interface IWcfService
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="identityId"></param>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        void Bind(int identityId);

        /// <summary>
        /// 服务端接收客户端上发的消息，参数需要验证
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="remoteAddress">远程连接地址</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        byte[] Request(string param, string remoteAddress);


        /// <summary>
        /// 提供程序内部之间通信，参数不需要验证
        /// </summary>
        /// <param name="route">路由</param>
        /// <param name="param">参数</param>
        /// <param name="remoteAddress">远程连接地址</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        byte[] CallRemote(string route, string param, string remoteAddress);

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="remoteAddress"></param>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        void SocketClose(string remoteAddress);
    }
}

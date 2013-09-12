using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Socket分发器接口
    /// </summary>
    public interface ISocketTransponder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteAddress"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        void Receive(string remoteAddress, byte[] buffer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteAddress"></param>
        /// <param name="buffer"></param>
        void Send(string remoteAddress, byte[] buffer);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.RPC.IO;

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
        /// <returns></returns>
        void Receive(SocketProcessEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="packet"></param>
        void Send(SocketObject socket, PacketData packet);
    }
}

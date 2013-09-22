using System;
using System.Collections.Concurrent;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    public delegate void SocketProcessEvent(SocketProcessEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class SocketProcessEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public SocketProcessEventArgs()
        {
            Data = new byte[0];
        }
        /// <summary>
        /// 
        /// </summary>
        public SocketObject Socket { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; set; }

    }
}
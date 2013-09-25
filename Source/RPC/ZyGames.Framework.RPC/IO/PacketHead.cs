using System;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PacketBaseHead 
    {
        internal int PacketLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToByte();
    }

    ///<summary>
    ///</summary>
    public class PacketHead : PacketBaseHead
    {
        public override byte[] ToByte()
        {
            return new byte[0];
        }
    }
}

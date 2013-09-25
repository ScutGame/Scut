using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class PacketData
    {
        private readonly PacketBaseHead _head;
        private readonly byte[] _data;

        /// <summary>
        /// 
        /// </summary>
        public PacketData(PacketBaseHead head, byte[] data)
        {
            _head = head;
            _data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ToByte()
        {
            byte[] headByte = _head.ToByte();
            _head.PacketLength = headByte.Length + _data.Length;
            byte[] body = new byte[_head.PacketLength + 4];
            int pos = 0;

            Buffer.BlockCopy(BitConverter.GetBytes(_head.PacketLength), 0, body, pos, 4);
            pos += 4;
            Buffer.BlockCopy(headByte, 0, body, pos, headByte.Length);
            pos += headByte.Length;
            Buffer.BlockCopy(_data, 0, body, pos, _data.Length);

            return body;
        }

    }
}

/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
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
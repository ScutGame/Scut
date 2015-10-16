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

namespace ZyGames.Framework.RPC.Sockets.WebSocket
{
    /// <summary>
    /// Message Head Frame
    /// </summary>
    public class MessageHeadFrame
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static MessageHeadFrame Parse(byte[] data, int offset = 0)
        {
            if (data.Length - offset > 1)
            {
                return new MessageHeadFrame(data, offset);
            }
            return null;
        }

        private byte[] _data;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        private MessageHeadFrame(byte[] data, int offset)
        {
            _data = new byte[2];
            Buffer.BlockCopy(data, offset, _data, 0, _data.Length);
        }

        /// <summary>
        /// RSV is 0
        /// </summary>
        public bool CheckRSV
        {
            get { return ((_data[0] & 0x70) == 0x00); }
        }

        /// <summary>
        /// Is Finsh
        /// </summary>
        public bool FIN
        {
            get { return ((_data[0] & 0x80) == 0x80); }
        }

        /// <summary>
        /// custom protocol
        /// </summary>
        public bool RSV1
        {
            get { return ((_data[0] & 0x40) == 0x40); }
        }

        /// <summary>
        /// custom protocol
        /// </summary>
        public bool RSV2
        {
            get { return ((_data[0] & 0x20) == 0x20); }
        }

        /// <summary>
        /// custom protocol
        /// </summary>
        public bool RSV3
        {
            get { return ((_data[0] & 0x10) == 0x10); }
        }

        /// <summary>
        /// op code:
        /// 0: continue
        /// 1: text message
        /// 2: bir message
        /// 3-7: no use
        /// 8: close connect
        /// 9: ping message
        /// A: pong message
        /// B-F: no use
        /// </summary>
        public sbyte OpCode
        {
            get { return (sbyte)(_data[0] & 0x0f); }
        }

        /// <summary>
        /// has mask
        /// </summary>
        public bool HasMask
        {
            get { return ((_data[1] & 0x80) == 0x80); }
        }

        /// <summary>
        /// message length
        /// </summary>
        public sbyte PayloadLenght
        {
            get { return (sbyte)(_data[1] & 0x7f); }
        }
    }
}

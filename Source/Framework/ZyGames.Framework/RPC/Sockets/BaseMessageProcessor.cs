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
using System.Collections.Generic;
using ZyGames.Framework.RPC.Sockets.WebSocket;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// stream reader
    /// </summary>
    public abstract class BaseMessageProcessor
    {
        /// <summary>
        /// Response is encode mask data
        /// </summary>
        public bool IsMask { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CloseStatusCode CloseStatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected BaseMessageProcessor()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataToken"></param>
        /// <param name="buffer"></param>
        /// <param name="messageList"></param>
        /// <returns></returns>
        public abstract bool TryReadMeaage(DataToken dataToken, byte[] buffer, out List<DataMeaage> messageList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exSocket"></param>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public abstract byte[] BuildMessagePack(ExSocket exSocket, sbyte opCode, byte[] data, int offset, int count);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="exSocket"></param>
        /// <param name="opCode"></param>
        /// <param name="reason"></param>
        public abstract byte[] CloseMessage(ExSocket exSocket, sbyte opCode, string reason);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int GetCloseStatus(byte[] data)
        {
            if (data == null || data.Length <= 1)
            {
                return OpCode.Empty;
            }
            var code = data[0] * 256 + data[1];

            if (!IsValidCloseCode(code))
            {
                return OpCode.Empty;
            }
            return code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected abstract bool IsValidCloseCode(int code);
    }
}

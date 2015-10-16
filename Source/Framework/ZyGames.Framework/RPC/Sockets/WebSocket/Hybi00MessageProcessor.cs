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
using ZyGames.Framework.Common;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.RPC.Sockets.WebSocket
{
    /// <summary>
    /// 
    /// </summary>
    public class Hybi00MessageProcessor : BaseMessageProcessor
    {
        private const int PreByteLength = 1;
        private const byte StartByte = 0x00;
        private const byte EndByte = 0xFF;
        private static byte[] ClosingBytes = new byte[] { EndByte, StartByte };

        /// <summary>
        /// 
        /// </summary>
        public Hybi00MessageProcessor()
        {
            CloseStatusCode = new Hybi10CloseStatusCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataToken"></param>
        /// <param name="buffer"></param>
        /// <param name="messageList"></param>
        /// <returns></returns>
        public override bool TryReadMeaage(DataToken dataToken, byte[] buffer, out List<DataMeaage> messageList)
        {
            messageList = new List<DataMeaage>();
            int offset = 0;
            do
            {
                //check close flag
                if (CheckCloseFlag(buffer, ref offset))
                {
                    messageList.Add(new DataMeaage() { Data = buffer, OpCode = OpCode.Close });
                    dataToken.Reset(true);
                    return true;
                }
                //receive buffer is complated
                if (!CheckPrefixHeadComplated(dataToken, buffer, ref offset) ||
                    !CheckDataComplated(dataToken, buffer, ref offset))
                {
                    return false;
                }
                byte[] data = dataToken.byteArrayForMessage;
                if (data != null)
                {
                    messageList.Add(new DataMeaage() { Data = data, OpCode = OpCode.Text });
                }
                dataToken.Reset(true);
            } while (offset < buffer.Length);
            return true;
        }

        private bool CheckCloseFlag(byte[] buffer, ref int offset)
        {
            if (buffer.Length - offset == ClosingBytes.Length &&
                MathUtils.IndexOf(buffer, offset, buffer.Length, ClosingBytes) != -1)
            {
                offset += ClosingBytes.Length;
                return true;
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="exSocket"></param>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override byte[] BuildMessagePack(ExSocket exSocket, sbyte opCode, byte[] data, int offset, int count)
        {
            if (opCode == OpCode.Close)
            {
                return ClosingBytes;
            }
            byte[] buffer = new byte[count + 2];
            buffer[0] = StartByte;
            Buffer.BlockCopy(data, offset, buffer, 1, count);
            buffer[count + 1] = EndByte;
            return buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exSocket"></param>
        /// <param name="opCode"></param>
        /// <param name="reason"></param>
        public override byte[] CloseMessage(ExSocket exSocket, sbyte opCode, string reason)
        {
            return ClosingBytes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected override bool IsValidCloseCode(int code)
        {
            return false;
        }


        private bool CheckPrefixHeadComplated(DataToken dataToken, byte[] buffer, ref int offset)
        {
            if (dataToken.byteArrayForPrefix == null || dataToken.byteArrayForPrefix.Length != PreByteLength)
            {
                dataToken.byteArrayForPrefix = new byte[PreByteLength];
            }
            if (PreByteLength - dataToken.prefixBytesDone > buffer.Length - offset)
            {
                Buffer.BlockCopy(buffer, offset, dataToken.byteArrayForPrefix, dataToken.prefixBytesDone, buffer.Length - offset);
                dataToken.prefixBytesDone += buffer.Length - offset;
                return false;
            }

            int count = dataToken.byteArrayForPrefix.Length - dataToken.prefixBytesDone;
            if (count > 0)
            {
                Buffer.BlockCopy(buffer, offset, dataToken.byteArrayForPrefix, dataToken.prefixBytesDone, count);
                dataToken.prefixBytesDone += count;
                offset += count;
            }
            return true;
        }

        private bool CheckDataComplated(DataToken dataToken, byte[] buffer, ref int offset)
        {
            byte[] data;
            int endMaskIndex = MathUtils.IndexOf(buffer, offset, buffer.Length - offset + 1, new[] { EndByte });
            if (endMaskIndex < 0)
            {
                data = new byte[buffer.Length - offset];
                Buffer.BlockCopy(buffer, offset, data, 0, data.Length);
                if (dataToken.byteArrayForMessage == null)
                {
                    dataToken.byteArrayForMessage = data;
                }
                else
                {
                    dataToken.byteArrayForMessage = BufferUtils.MergeBytes(dataToken.byteArrayForMessage, data);
                }
                offset += data.Length;
                return false;
            }
            //end mask not received
            if (endMaskIndex == 0)
            {
                offset += 1;
                return true;
            }
            data = new byte[endMaskIndex - offset];
            Buffer.BlockCopy(buffer, offset, data, 0, data.Length);
            dataToken.byteArrayForMessage = BufferUtils.MergeBytes(dataToken.byteArrayForMessage, data);
            offset += data.Length + 1;
            return true;
        }
    }
}

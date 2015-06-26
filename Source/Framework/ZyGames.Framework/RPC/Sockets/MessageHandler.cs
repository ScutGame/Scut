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
using System.Net.Sockets;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageHandler : BaseMessageProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageHandler()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        /// <param name="dataToken"></param>
        /// <param name="remainingBytesToProcess"></param>
        /// <returns></returns>
        public int HandlePrefix(SocketAsyncEventArgs saea, DataToken dataToken, int remainingBytesToProcess)
        {
            return HandlePrefix(saea.Buffer, dataToken.DataOffset, dataToken, remainingBytesToProcess);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        /// <param name="dataToken"></param>
        /// <param name="remainingBytesToProcess"></param>
        /// <returns></returns>
        public int HandleMessage(SocketAsyncEventArgs saea, DataToken dataToken, int remainingBytesToProcess)
        {
            return HandleMessage(saea.Buffer, dataToken.DataOffset, dataToken, remainingBytesToProcess);
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
            int remainingBytesToProcess = buffer.Length;
            bool needPostAnother = true;
            do
            {
                if (dataToken.prefixBytesDone < 4)
                {
                    remainingBytesToProcess = HandlePrefix(buffer, dataToken.bufferSkip, dataToken, remainingBytesToProcess);
                    if (dataToken.prefixBytesDone == 4 && (dataToken.messageLength > 10 * 1024 * 1024 || dataToken.messageLength <= 0))
                    {
                        byte[] bufferBytes = dataToken.byteArrayForMessage == null ?
                            BufferUtils.MergeBytes(dataToken.byteArrayForPrefix, BufferUtils.GetBytes(buffer, dataToken.bufferSkip, buffer.Length - dataToken.bufferSkip)) :
                            BufferUtils.MergeBytes(dataToken.byteArrayForPrefix, dataToken.byteArrayForMessage, buffer);
                        string data = Encoding.UTF8.GetString(bufferBytes);
                        //消息头已接收完毕，并且接收到的消息长度大于10M，socket传输的数据已紊乱，关闭掉
                        TraceLog.Write("The host[{0}] request parser head byte error, byte len={1}\r\ndata:{2}",
                            dataToken.Socket.RemoteEndPoint.ToNotNullString(),
                            dataToken.messageLength,
                            data);
                        needPostAnother = false;
                        break;
                    }
                    //if (logger.IsDebugEnabled) logger.Debug("处理消息头，消息长度[{0}]，剩余字节[{1}]", dataToken.messageLength, remainingBytesToProcess);
                    if (remainingBytesToProcess == 0) break;
                }

                remainingBytesToProcess = HandleMessage(buffer, dataToken.bufferSkip, dataToken, remainingBytesToProcess);

                if (dataToken.IsMessageReady)
                {
                    //if (logger.IsDebugEnabled) logger.Debug("完整封包 长度[{0}],总传输[{1}],剩余[{2}]", dataToken.messageLength, ioEventArgs.BytesTransferred, remainingBytesToProcess);
                    messageList.Add(new DataMeaage() { Data = dataToken.byteArrayForMessage, OpCode = OpCode.Binary });
                    if (remainingBytesToProcess != 0)
                    {
                        //if (logger.IsDebugEnabled) logger.Debug("重置缓冲区,buffskip指针[{0}]。", dataToken.bufferSkip);
                        dataToken.Reset(false);
                    }
                }
                else
                {
                    //if (logger.IsDebugEnabled) logger.Debug("不完整封包 长度[{0}],总传输[{1}],已接收[{2}]", dataToken.messageLength, ioEventArgs.BytesTransferred, dataToken.messageBytesDone);
                }
            } while (remainingBytesToProcess != 0);


            if (needPostAnother)
            {
                //继续处理下个请求包
                if (dataToken.prefixBytesDone == 4 && dataToken.IsMessageReady)
                {
                    dataToken.Reset(true);
                }
                dataToken.bufferSkip = 0;
            }
            return needPostAnother;
        }

        private int HandlePrefix(byte[] buffer, int offset, DataToken dataToken, int remainingBytesToProcess)
        {
            if (remainingBytesToProcess >= 4 - dataToken.prefixBytesDone)
            {
                for (int i = 0; i < 4 - dataToken.prefixBytesDone; i++)
                {
                    dataToken.byteArrayForPrefix[dataToken.prefixBytesDone + i] = buffer[offset + i];
                }
                remainingBytesToProcess = remainingBytesToProcess - 4 + dataToken.prefixBytesDone;
                dataToken.bufferSkip += 4 - dataToken.prefixBytesDone;
                dataToken.prefixBytesDone = 4;
                dataToken.messageLength = BitConverter.ToInt32(dataToken.byteArrayForPrefix, 0);
            }
            else
            {
                for (int i = 0; i < remainingBytesToProcess; i++)
                {
                    dataToken.byteArrayForPrefix[dataToken.prefixBytesDone + i] = buffer[offset + i];
                }
                dataToken.prefixBytesDone += remainingBytesToProcess;
                remainingBytesToProcess = 0;
            }

            return remainingBytesToProcess;
        }

        private int HandleMessage(byte[] buffer, int offset, DataToken dataToken, int remainingBytesToProcess)
        {
            if (dataToken.messageBytesDone == 0)
            {
                dataToken.byteArrayForMessage = new byte[dataToken.messageLength];
            }
            var nonCopiedBytes = 0;
            if (remainingBytesToProcess + dataToken.messageBytesDone >= dataToken.messageLength)
            {
                var copyedBytes = dataToken.RemainByte;
                nonCopiedBytes = remainingBytesToProcess - copyedBytes;
                Buffer.BlockCopy(buffer, offset, dataToken.byteArrayForMessage, dataToken.messageBytesDone, copyedBytes);
                dataToken.messageBytesDone = dataToken.messageLength;
                dataToken.bufferSkip += copyedBytes;
            }
            else
            {
                Buffer.BlockCopy(buffer, offset, dataToken.byteArrayForMessage, dataToken.messageBytesDone, remainingBytesToProcess);
                dataToken.messageBytesDone += remainingBytesToProcess;
            }

            return nonCopiedBytes;
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
            byte[] buffer = new byte[count + 4];
            Buffer.BlockCopy(BitConverter.GetBytes(count), 0, buffer, 0, 4);
            Buffer.BlockCopy(data, offset, buffer, 4, count);
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
            return null;
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
    }
}
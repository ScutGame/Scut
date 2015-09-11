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
using ZyGames.Framework.Common;

namespace ZyGames.Framework.RPC.Sockets.WebSocket
{
    /// <summary>
    /// 
    /// </summary>
    public class Hybi10MessageProcessor : Hybi00MessageProcessor
    {
        private const int Version = 8;
        /// <summary>
        /// length 2
        /// </summary>
        private const int PreByteLength = 2;
        private const int MaskLength = 4;

        /// <summary>
        /// 
        /// </summary>
        public Hybi10MessageProcessor()
        {
            CloseStatusCode = new Hybi10CloseStatusCode();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exSocket"></param>
        /// <returns></returns>
        protected bool CheckVersion(ExSocket exSocket)
        {
            return exSocket != null &&
                exSocket.Handshake != null &&
                exSocket.Handshake.WebSocketVersion < Version;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataToken"></param>
        /// <param name="buffer"></param>
        /// <param name="messageList"></param>
        /// <returns>true: message is complate</returns>
        public override bool TryReadMeaage(DataToken dataToken, byte[] buffer, out List<DataMeaage> messageList)
        {
            if (CheckVersion(dataToken.Socket))
            {
                return base.TryReadMeaage(dataToken, buffer, out messageList);
            }
            messageList = new List<DataMeaage>();
            int offset = 0;
            do
            {
                //receive buffer is complated
                if (!CheckPrefixHeadComplated(dataToken, buffer, ref offset) ||
                    !dataToken.HeadFrame.CheckRSV ||
                    !CheckPayloadHeadComplated(dataToken, buffer, ref offset) ||
                    !CheckPayloadDataComplated(dataToken, buffer, ref offset))
                {
                    return false;
                }
                byte[] data = dataToken.HeadFrame.HasMask
                        ? DecodeMask(dataToken.byteArrayForMessage, dataToken.byteArrayMask, 0, dataToken.messageLength)
                        : dataToken.byteArrayForMessage;

                if (!dataToken.HeadFrame.FIN)
                {
                    dataToken.DataFrames.Add(new DataSegmentFrame()
                    {
                        Head = dataToken.HeadFrame,
                        Data = new ArraySegment<byte>(data)
                    });
                }
                else
                {
                    //frame complated
                    sbyte opCode;
                    if (dataToken.DataFrames.Count > 0)
                    {
                        dataToken.DataFrames.Add(new DataSegmentFrame()
                        {
                            Head = dataToken.HeadFrame,
                            Data = new ArraySegment<byte>(data)
                        });
                        opCode = dataToken.DataFrames[0].Head.OpCode;
                        data = CombineDataFrames(dataToken.DataFrames);
                    }
                    else
                    {
                        opCode = dataToken.HeadFrame.OpCode;
                    }
                    messageList.Add(new DataMeaage() { Data = data, OpCode = opCode });
                    dataToken.DataFrames.Clear();
                }
                dataToken.Reset(true);
            } while (offset < buffer.Length);

            return true;
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
            if (CheckVersion(exSocket))
            {
                return base.BuildMessagePack(exSocket, opCode, data, offset, count);
            }
            bool isMask = IsMask;
            int maskNum = isMask ? MaskLength : 0;
            byte[] buffer;
            if (count < 126)
            {
                buffer = new byte[count + maskNum + 2];
                //buffer[0] = 0x81;
                buffer[1] = (byte)count;
                //Buffer.BlockCopy(data, offset, buffer, 2, count);
            }
            else if (count < 0xFFFF)
            {
                //uint16 bit
                buffer = new byte[count + maskNum + 4];
                //buffer[0] = 0x81;
                buffer[1] = 126;
                buffer[2] = (byte)(count / 256);
                buffer[3] = (byte)(count % 256);
                //Buffer.BlockCopy(data, offset, buffer, 4, count);
            }
            else
            {
                //uint64 bit
                buffer = new byte[count + maskNum + 10];
                //buffer[0] = 0x81;
                buffer[1] = 127;
                int num2 = count;
                int num3 = 256;
                for (int i = 9; i > 1; i--)
                {
                    buffer[i] = (byte)(num2 % num3);
                    num2 /= num3;
                    if (num2 == 0)
                    {
                        break;
                    }
                }
            }
            if (isMask)
            {
                //mask after of payloadLength 
                byte[] mask = GenerateMask();
                int maskPos = buffer.Length - maskNum - count;
                Buffer.BlockCopy(mask, 0, buffer, maskPos, mask.Length);
                EncodeMask(data, offset, count, mask, buffer, buffer.Length - count);
            }
            else
            {
                Buffer.BlockCopy(data, offset, buffer, buffer.Length - count, count);
            }
            buffer[0] = (byte)((byte)opCode | 0x80);
            if (isMask)
            {
                buffer[1] = (byte)(buffer[1] | 0x80);
            }
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
            byte[] data = Encoding.UTF8.GetBytes(reason);
            return BuildMessagePack(exSocket, opCode, data, 0, data.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected override bool IsValidCloseCode(int code)
        {
            var closeCode = CloseStatusCode;

            if (code >= 0 && code <= 999)
                return false;

            if (code >= 1000 && code <= 1999)
            {
                if (code == closeCode.NormalClosure
                    || code == closeCode.GoingAway
                    || code == closeCode.ProtocolError
                    || code == closeCode.UnexpectedCondition
                    //|| code == closeCode.Reserved
                    //|| code == closeCode.NoStatusRcvd
                    || code == closeCode.AbnormalClosure
                    || code == closeCode.InvalidUTF8
                    || code == closeCode.PolicyViolation
                    || code == closeCode.MessageTooBig
                    || code == closeCode.MandatoryExt)
                {
                    return true;
                }
                return false;
            }
            //2000-2999 use by extensions
            //3000-3999 libraries and frameworks
            //4000-4999 application code
            if (code >= 2000 && code <= 4999)
                return true;

            return false;
        }

        private byte[] CombineDataFrames(List<DataSegmentFrame> dataFrames)
        {
            int len = dataFrames.Sum(t => t.Head.PayloadLenght);
            byte[] buffer = new byte[len];
            int offset = 0;
            foreach (var frame in dataFrames)
            {
                Buffer.BlockCopy(frame.Data.Array, frame.Data.Offset, buffer, offset, frame.Head.PayloadLenght);
                offset += frame.Head.PayloadLenght;
            }
            return buffer;
        }

        internal byte[] GenerateMask()
        {
            var mask = new byte[MaskLength];
            for (int i = 0; i < MaskLength; i++)
            {
                mask[i] = (byte)RandomUtils.GetRandom(0, 255);
            }
            return mask;
        }

        internal void EncodeMask(byte[] rawData, int rowOffset, int rowCount, byte[] mask, byte[] buffer, int offset)
        {
            for (int i = 0; i < rowCount; i++)
            {
                int num = rowOffset + i;
                buffer[offset++] = (byte)(rawData[num] ^ mask[i % 4]);
            }
        }


        internal byte[] DecodeMask(byte[] buffer, byte[] maskKey, int offset, int count)
        {
            for (var i = offset; i < count; i++)
            {
                buffer[i] = (byte)(buffer[i] ^ maskKey[i % maskKey.Length]);
            }
            return buffer;
        }

        private bool CheckPayloadDataComplated(DataToken dataToken, byte[] buffer, ref int offset)
        {
            int copyByteCount = dataToken.RemainByte;
            if (buffer.Length - offset >= copyByteCount)
            {
                Buffer.BlockCopy(buffer, offset, dataToken.byteArrayForMessage, dataToken.messageBytesDone, copyByteCount);
                dataToken.messageBytesDone += copyByteCount;
                offset += copyByteCount;
            }
            else
            {
                Buffer.BlockCopy(buffer, offset, dataToken.byteArrayForMessage, dataToken.messageBytesDone, buffer.Length - offset);
                dataToken.messageBytesDone += buffer.Length - offset;
                offset += buffer.Length - offset;
            }
            return dataToken.IsMessageReady;
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
            if (dataToken.HeadFrame == null)
            {
                //build message head
                dataToken.HeadFrame = MessageHeadFrame.Parse(dataToken.byteArrayForPrefix);
            }
            return true;
        }

        private bool CheckPayloadHeadComplated(DataToken dataToken, byte[] buffer, ref int offset)
        {
            try
            {

                if (dataToken.byteArrayForMessage == null)
                {
                    int size = 0;
                    int payloadLenght = dataToken.HeadFrame.PayloadLenght;
                    switch (payloadLenght)
                    {
                        case 126:
                            size = 2; //uint16 2bit
                            if (!CheckPrefix2Complated(dataToken, buffer, ref offset, size)) return false;
                            UInt16 len = (UInt16)(dataToken.byteArrayForPrefix2[0] << 8 | dataToken.byteArrayForPrefix2[1]);
                            dataToken.byteArrayForMessage = new byte[len];
                            break;
                        case 127:
                            size = 8; //uint64 8bit
                            if (!CheckPrefix2Complated(dataToken, buffer, ref offset, size)) return false;
                            UInt64 len64 = BitConverter.ToUInt64(dataToken.byteArrayForPrefix2.Reverse().ToArray(), 0);
                            dataToken.byteArrayForMessage = new byte[len64];
                            break;
                        default:
                            dataToken.byteArrayForMessage = new byte[payloadLenght];
                            break;
                    }
                    dataToken.messageLength = dataToken.byteArrayForMessage.Length;
                }
                if (dataToken.HeadFrame.HasMask)
                {
                    if (dataToken.byteArrayMask == null || dataToken.byteArrayMask.Length != MaskLength)
                    {
                        dataToken.byteArrayMask = new byte[MaskLength];
                    }
                    if (MaskLength - dataToken.maskBytesDone > buffer.Length - offset)
                    {
                        Buffer.BlockCopy(buffer, offset, dataToken.byteArrayMask, dataToken.maskBytesDone, buffer.Length - offset);
                        dataToken.maskBytesDone += buffer.Length - offset;
                        return false;
                    }
                    int count = dataToken.byteArrayMask.Length - dataToken.maskBytesDone;
                    if (count > 0)
                    {
                        Buffer.BlockCopy(buffer, offset, dataToken.byteArrayMask, dataToken.maskBytesDone, count);
                        dataToken.maskBytesDone += count;
                        offset += count;
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckPrefix2Complated(DataToken dataToken, byte[] buffer, ref int offset, int size)
        {
            if (dataToken.byteArrayForPrefix2 == null || dataToken.byteArrayForPrefix2.Length != size)
            {
                dataToken.byteArrayForPrefix2 = new byte[size];
            }
            if (size - dataToken.prefixBytesDone2 > buffer.Length - offset)
            {
                Buffer.BlockCopy(buffer, offset, dataToken.byteArrayForPrefix2, dataToken.prefixBytesDone2, buffer.Length - offset);
                dataToken.prefixBytesDone2 += buffer.Length - offset;
                return false;
            }
            int count = dataToken.byteArrayForPrefix2.Length - dataToken.prefixBytesDone2;
            if (count > 0)
            {
                Buffer.BlockCopy(buffer, offset, dataToken.byteArrayForPrefix2, dataToken.prefixBytesDone2, count);
                dataToken.prefixBytesDone2 += count;
                offset += count;
            }
            return true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ZyGames.OA.BLL.Remote
{
    public class ExSocket
    {
        private Socket socket;
        private IPEndPoint remoteEndPoint;
        private Queue<byte[]> sendQueue;
        private int isInSending;
        internal DateTime LastAccessTime;

        public Socket WorkSocket { get { return socket; } }
        public EndPoint RemoteEndPoint { get { return remoteEndPoint; } }
        public int QueueLength { get { return sendQueue.Count; } }

        public ExSocket(Socket socket)
        {
            this.socket = socket;
            this.remoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            sendQueue = new Queue<byte[]>();
        }

        internal void Enqueue(byte[] data)
        {
            sendQueue.Enqueue(data);
        }
        internal bool TryDequeue(out byte[] result)
        {
            result = sendQueue.Dequeue();
            return true;
        }
        internal bool TrySetSendFlag()
        {
            return Interlocked.CompareExchange(ref isInSending, 1, 0) == 0;
        }
        internal void ResetSendFlag()
        {
            Interlocked.Exchange(ref isInSending, 0);
        }
    }
    class DataToken
    {
        internal byte[] byteArrayForMessage;
        internal byte[] byteArrayForPrefix;
        internal int messageBytesDone;
        internal int prefixBytesDone;
        internal int messageLength;
        internal int bufferOffset;
        internal int bufferSkip;
        internal ExSocket Socket;

        internal int DataOffset
        {
            get { return bufferOffset + bufferSkip; }
        }
        internal int RemainByte
        {
            get { return messageLength - messageBytesDone; }
        }
        internal bool IsMessageReady
        {
            get { return messageBytesDone == messageLength; }
        }
        internal DataToken()
        {
            byteArrayForPrefix = new byte[4];
        }

        internal void Reset(bool skip)
        {
            this.byteArrayForMessage = null;
            byteArrayForPrefix[0] = 0;
            byteArrayForPrefix[1] = 0;
            byteArrayForPrefix[2] = 0;
            byteArrayForPrefix[3] = 0;
            this.messageBytesDone = 0;
            this.prefixBytesDone = 0;
            this.messageLength = 0;
            if (skip)
                this.bufferSkip = 0;
        }
    }

    class PrefixHandler
    {
        public int HandlePrefix(SocketAsyncEventArgs saea, DataToken dataToken, int remainingBytesToProcess)
        {
            if (remainingBytesToProcess >= 4 - dataToken.prefixBytesDone)
            {
                for (int i = 0; i < 4 - dataToken.prefixBytesDone; i++)
                {
                    dataToken.byteArrayForPrefix[dataToken.prefixBytesDone + i] = saea.Buffer[dataToken.DataOffset + i];
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
                    dataToken.byteArrayForPrefix[dataToken.prefixBytesDone + i] = saea.Buffer[dataToken.DataOffset + i];
                }
                dataToken.prefixBytesDone += remainingBytesToProcess;
                remainingBytesToProcess = 0;
            }

            return remainingBytesToProcess;
        }
    }

    class MessageHandler
    {
        public int HandleMessage(SocketAsyncEventArgs saea, DataToken dataToken, int remainingBytesToProcess)
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
                Buffer.BlockCopy(saea.Buffer, dataToken.DataOffset, dataToken.byteArrayForMessage, dataToken.messageBytesDone, copyedBytes);
                dataToken.messageBytesDone = dataToken.messageLength;
                dataToken.bufferSkip += copyedBytes;
            }
            else
            {
                Buffer.BlockCopy(saea.Buffer, dataToken.DataOffset, dataToken.byteArrayForMessage, dataToken.messageBytesDone, remainingBytesToProcess);
                dataToken.messageBytesDone += remainingBytesToProcess;
            }

            return nonCopiedBytes;
        }
    }
}

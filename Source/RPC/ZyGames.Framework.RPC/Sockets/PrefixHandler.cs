using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ZyGames.Framework.RPC.Sockets
{
    class PrefixHandler
    {
        public int HandlePrefix(SocketAsyncEventArgs saea, DataToken dataToken, int remainingBytesToProcess)
        {
            if (remainingBytesToProcess >= 4 - dataToken.prefixBytesDone)
            {
                for (int i = 0; i < 4 - dataToken.prefixBytesDone; i++)
                {
                    dataToken.byteArrayForPrefix[dataToken.prefixBytesDone + i] = saea.Buffer[dataToken.dataOffset + i];
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
                    dataToken.byteArrayForPrefix[dataToken.prefixBytesDone + i] = saea.Buffer[dataToken.dataOffset + i];
                }
                dataToken.prefixBytesDone += remainingBytesToProcess;
                remainingBytesToProcess = 0;
            }

            return remainingBytesToProcess;
        }
    }
}

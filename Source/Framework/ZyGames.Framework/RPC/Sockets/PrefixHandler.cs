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
}
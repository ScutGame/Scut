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
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace ZyGames.Framework.RPC.Sockets
{
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
}
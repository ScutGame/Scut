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
using System.Net;
using System.Text;

namespace ZyGames.Framework.RPC.Sockets
{
    public class SocketSettings
    {
        public SocketSettings(int maxConnections, int backlog, int maxAcceptOps, int bufferSize, IPEndPoint localEndPoint, int expireInterval, int expireTime)
        {
            this.MaxConnections = maxConnections;
            this.NumOfSaeaForRecSend = 2 * maxConnections;
            this.Backlog = backlog;
            this.MaxAcceptOps = maxAcceptOps;
            this.BufferSize = bufferSize;
            this.LocalEndPoint = localEndPoint;
            this.ExpireInterval = expireInterval;
            this.ExpireTime = expireTime;
        }

        public int MaxConnections { get; private set; }
        public int NumOfSaeaForRecSend { get; private set; }
        public int Backlog { get; private set; }
        public int MaxAcceptOps { get; private set; }
        public int BufferSize { get; private set; }
        public IPEndPoint LocalEndPoint { get; private set; }
        public int ExpireInterval { get; private set; }
        public int ExpireTime { get; private set; }
    }
}
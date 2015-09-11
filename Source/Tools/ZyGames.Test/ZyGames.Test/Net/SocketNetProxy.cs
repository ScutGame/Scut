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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Test.Net
{
    internal class SocketNetProxy : NetProxy
    {
        private ClientSocket _client;
        private IPEndPoint _address;
        private Func<byte[], bool> _callback;
        private bool _isConnected;
        private ClientSocketSettings setting;

        public SocketNetProxy(string host, int port)
        {
            var ipAddress = Dns.GetHostAddresses(host).First();
            _address = new IPEndPoint(ipAddress, port);
            setting = new ClientSocketSettings(BufferSize, _address);

        }

        private void DoClosed(object sender, SocketEventArgs e)
        {
            _isConnected = false;
            StopWait();
        }

        private void DoReceived(object sender, SocketEventArgs e)
        {
            if (_callback != null)
            {
                if (_callback(e.Data))
                {
                    StopWait();
                }
            }
        }

        public override void CheckConnect()
        {
            if (!_isConnected)
            {
                _client = new ClientSocket(setting);
                _client.DataReceived += DoReceived;
                _client.Disconnected += DoClosed;
                _client.Connect();
                _isConnected = true;
            }
        }

        public override void SendAsync(byte[] data, Func<byte[], bool> callback)
        {
            _callback = callback;
            _client.PostSend(data, 0, data.Length);
            StartWait(RequestTimeout);
        }

    }
}
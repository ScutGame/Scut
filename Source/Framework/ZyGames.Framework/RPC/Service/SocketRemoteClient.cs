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
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.RPC.Service
{
    /// <summary>
    /// Remote client for socket
    /// </summary>
    public class SocketRemoteClient : RemoteClient
    {
        private const int BufferSize = 1024;
        private ClientSocket _client;
        private Encoding _encoding;
        private Timer _timer;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="heartInterval">ms</param>
        public SocketRemoteClient(string host, int port, int heartInterval)
        {
            IsSocket = true;
            var remoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
            var settings = new ClientSocketSettings(BufferSize, remoteEndPoint);
            _client = new ClientSocket(settings);
            _client.DataReceived += OnDataReceived;
            _client.Disconnected += OnDisconnected;
            _timer = new Timer(DoCheckHeartbeat, null, 1000, heartInterval);

        }

        /// <summary>
        /// 
        /// </summary>
        public Encoding Encoding
        {
            set { _encoding = value; }
        }

        /// <summary>
        /// Heartbeat packet data.
        /// </summary>
        public byte[] HeartPacket { get; set; }

        /// <summary>
        /// Connect
        /// </summary>
        public void Connect()
        {
            _client.Connect();
            LocalAddress = _client.LocalEndPoint.ToString();
            Connected = true;
        }
        /// <summary>
        /// Close
        /// </summary>
        public override void Close()
        {
            _client.Close();
            Connected = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public bool Wait(int millisecondsTimeout = 0)
        {
            return _client.Wait(millisecondsTimeout);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override async Task Send(string data)
        {
            byte[] buffer = (_encoding ?? Encoding.ASCII).GetBytes(data);
            await Send(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override async Task Send(byte[] data)
        {
            if (!Connected)
            {
                Connect();
            }
            await _client.PostSend(data, 0, data.Length);
        }


        private void OnDataReceived(object sender, SocketEventArgs e)
        {
            try
            {
                var remoteArgs = new RemoteEventArgs() { Data = e.Data };
                OnCallback(remoteArgs);

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Socket remote received error:{0}", ex);
            }
        }

        private void OnDisconnected(object sender, SocketEventArgs e)
        {
            try
            {
                Connected = false;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Socket remote disconnected error:{0}", ex);
            }
        }

        private void DoCheckHeartbeat(object state)
        {
            try
            {
                if (HeartPacket != null && HeartPacket.Length > 0)
                {
                    Send(HeartPacket);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Socket remote heartbeat error:{0}", ex);
            }
        }

    }
}

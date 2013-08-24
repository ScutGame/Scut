using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 客户端连接
    /// </summary>
    [Obsolete]
    class SocketConnection : IDisposable
    {
        private Socket _client;
        private int _bufferSize = 2048;
        private ConcurrentQueue<byte[]> _sendBuffer = new ConcurrentQueue<byte[]>();

        public SocketConnection()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectAsync(string host, int port)
        {
            IPAddress address = null;
            var iplist = Dns.GetHostAddresses(host);
            if (iplist.Length > 0)
            {
                address = iplist[0];
            }
            else
            {
                throw new Exception(string.Format("Not found \"{0}\" ip", host));
            }
            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
            connectArgs.RemoteEndPoint = new IPEndPoint(address, port);
            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ConnectCompleted);
            _client.ConnectAsync(connectArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SendAsync(byte[] data)
        {
            var sendArgs = new SocketAsyncEventArgs();
            sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SendCompleted);
            sendArgs.SetBuffer(data, 0, data.Length);
            //先调用异步接收，再调用异步发送。让你体验到异步明显非一般的感觉。
            _client.SendAsync(sendArgs);
        }

        private void DoRecieve(Socket client)
        {
            SocketAsyncEventArgs recieveArgs = new SocketAsyncEventArgs();
            recieveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(RecieveCompleted);
            byte[] buffer = new byte[_bufferSize];
            recieveArgs.SetBuffer(buffer, 0, buffer.Length);
            client.ReceiveAsync(recieveArgs);
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            Socket client = sender as Socket;
            if (client != null && e.SocketError == SocketError.Success && client.Connected)
            {
                DoRecieve(client);
            }
        }

        private void RecieveCompleted(object sender, SocketAsyncEventArgs e)
        {
            Socket sk = sender as Socket;
            byte[] data = e.Buffer;  //注意这里，如何取关联到套接字的发送接受的缓冲区中的值。
            Console.WriteLine("Message received: " + data.Length);
        }

        private void SendCompleted(object sender, SocketAsyncEventArgs e)
        {
            Socket sk = sender as Socket;
            if (e.SocketError == SocketError.Success)
            {
                Console.WriteLine("Send complete,{0}byte!", e.BytesTransferred);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
            _client = null;
            _sendBuffer = null;
            //清理托管对象
            GC.SuppressFinalize(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.Game.SocketServer.Net
{
    /// <summary>
    /// 分发传送器
    /// </summary>
    public abstract class SocketTransponder : ISocketTransponder
    {

        public Action<EndPoint> ConnectCompleted;
        public Action<string, byte[]> ReceiveCompleted;
        internal Action<string, byte[]> SendCompleted;
        public Action<object, EndPoint> SocketClosing;
        public event Func<string, bool> CheckConnectedHandle;

        public bool CheckConnected(string remoteAddress)
        {
            return CheckConnectedHandle(remoteAddress);
        }

        public abstract void Receive(string remoteAddress, byte[] data);

        public abstract void Send(string remoteAddress, byte[] data);

        public virtual void OnSending(string fromAddress,string remoteAddress, int bytesTransferred, int remainingByteCount)
        {
            Console.WriteLine("{0}>>{1}发送到{2}字节数:{3}byte,剩余:{4}",
                DateTime.Now.ToString("HH:mm:ss:ms"),
                fromAddress,
                remoteAddress,
                bytesTransferred,
                remainingByteCount);
        }

        public abstract void OnReceiveTimeout(string remoteAddress, byte[] data);
    }
}

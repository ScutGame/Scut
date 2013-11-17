using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace ZyGames.OA.WatchService.BLL.Tools
{
    public delegate void SocketCallback(int error, string errorInfo, byte[] buffer);

    public class GameSocketClient : IDisposable
    {
        private static AutoResetEvent autoConnectEvent = new AutoResetEvent(false);
        private StringBuilder sb = new StringBuilder();
        private int bufferSize = 1024;
        private Socket clientSock;
        private bool _connected;

        public event SocketCallback ReceiveHandle;

        public GameSocketClient()
        {
        }

        public void Connect(string host)
        {
            var list = host.Split(':');
            if (list.Length > 1)
            {
                Connect(list[0], int.Parse(list[1]));
            }
        }
        public void Connect(string ip, int port)
        {
            IPAddress destinationAddr = null;
            destinationAddr = IPAddress.Parse(ip);
            int destinationPort = port;
            clientSock = new Socket(destinationAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnect);
            socketEventArg.RemoteEndPoint = new IPEndPoint(destinationAddr, destinationPort);
            socketEventArg.UserToken = clientSock;
            clientSock.ConnectAsync(socketEventArg);

            autoConnectEvent.WaitOne();
        }

        public bool Connected
        {
            get
            {
                return _connected;
            }
        }
        /// <summary>
        /// 开始读取数据
        /// </summary>
        public void StartRead()
        {
            BeginReceive();
        }

        /// <summary>
        /// Disconnect from the host.
        /// </summary>
        public void Disconnect()
        {
            clientSock.Disconnect(false);
        }
        private void BeginReceive()
        {
            SocketError socketError;
            byte[] data = new byte[bufferSize];
            clientSock.BeginReceive(data, 0, data.Length, SocketFlags.None, out socketError, ReceiveCompleted, data);

            autoConnectEvent.WaitOne();
        }

        private void ReceiveCompleted(IAsyncResult reault)
        {
            SocketError socketError = 0;
            int cout = 0;
            try
            {
                cout = clientSock.EndReceive(reault);
            }
            catch (SocketException e)
            {
                socketError = e.SocketErrorCode;
            }
            catch
            {
                socketError = SocketError.HostDown;
            }

            if (socketError == SocketError.Success && cout > 0)
            {
                byte[] buffer = reault.AsyncState as byte[];
                byte[] data = new byte[cout];
                Array.Copy(buffer, 0, data, 0, data.Length);
                if (ReceiveHandle != null)
                {
                    int error;
                    string errorInfo;
                    ReadHead(data, out error, out errorInfo);
                    ReceiveHandle(error, errorInfo, data);
                    autoConnectEvent.Set();
                }
                //Console.WriteLine("Recv:{0}", Encoding.UTF8.GetString(data));

                //BeginReceive();
            }
            else
            {
                clientSock.Close();
                Console.WriteLine("与服务器连接断开");
            }

        }

        private void ReadHead(byte[] buffer, out int error, out string errorInfo)
        {
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader reader = new BinaryReader(ms, Encoding.UTF8);
            int TotalLength = reader.ReadInt32();
            error = reader.ReadInt32();
            int MsgId = reader.ReadInt32();
            errorInfo = "";
            Int32 length = reader.ReadInt32();
            if (length >= 0)
            {
                errorInfo = Encoding.UTF8.GetString(reader.ReadBytes(length));
            }
            int Action = reader.ReadInt32();
        }
        /// <summary>
        /// 发送到游戏服
        /// </summary>
        public void SendToServer(int gameId, int serverId, int actionId, string command)
        {
            string param = string.Format("Sid={0}&Uid={1}&ActionID={2}&GameType={3}&ServerID={4}&Cmd={5}",
                        DateTime.Now.ToString("yyMMddHHmmss"),
                        10000,
                        actionId,
                        gameId,
                        serverId,
                        command);
            param += string.Format("&sign={0}", GetSign(param));
            param = HttpUtility.UrlEncode(param);
            Byte[] data = Encoding.UTF8.GetBytes(param);
            data = MergeBytes(GetSocketBytes(data.Length), data);
            SendData(data);
            BeginReceive();
        }

        static private Byte[] MergeBytes(params Byte[][] args)
        {
            Int32 length = 0;
            foreach (byte[] tempbyte in args)
            {
                length += tempbyte.Length;  //计算数据包总长度
            }

            Byte[] bytes = new Byte[length]; //建立新的数据包

            Int32 tempLength = 0;

            foreach (byte[] tempByte in args)
            {
                tempByte.CopyTo(bytes, tempLength);
                tempLength += tempByte.Length;  //复制数据包到新数据包
            }

            return bytes;

        }

        /// <summary>
        /// 将一个32位整形转换成一个BYTE[]4字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static private Byte[] GetSocketBytes(Int32 data)
        {
            return BitConverter.GetBytes(data);
        }
        private static string GetSign(string param)
        {
            string attachParam = param + "44CAC8ED53714BF18D60C5C7B6296000";
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(attachParam, "MD5") ?? "";
            return sign.ToLower();
        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="data"></param>
        public void SendData(byte[] data)
        {
            clientSock.BeginSend(data, 0, data.Length, SocketFlags.None, AsynCallBack, clientSock);

        }

        private void AsynCallBack(IAsyncResult result)
        {
            try
            {
                Socket sock = result.AsyncState as Socket;

                if (sock != null)
                {
                    sock.EndSend(result);
                }
            }
            catch
            {

            }
        }

        private void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            // Signals the end of connection.
            // Set the flag for socket connected.
            _connected = (e.SocketError == SocketError.Success);
            autoConnectEvent.Set();
        }

        public void Dispose()
        {
            clientSock.Close();
        }
    }
}
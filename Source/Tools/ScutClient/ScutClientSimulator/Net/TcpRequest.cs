using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using ZyGames.Framework.RPC.Sockets;

namespace Scut.Client.Net
{
    public class TcpRequest
    {
        private static TcpRequest _instance = new TcpRequest();

        public static TcpRequest Instance()
        {
            return _instance;
        }

        private Dictionary<string, SocketClient> _clientDict;
        private int bufferSize = 4096;

        private TcpRequest()
        {
            _clientDict = new Dictionary<string, SocketClient>();
        }

        public bool EnableSigin { get; set; }

        public Action<SocketClient, byte[]> ReceiveCompleted;

        public string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        public string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        public void Send(string host, int port, string sendData, UserToken token)
        {
            sendData = string.Format("?d={0}", UrlEncode(sendData + "&sign=" + CreateSigin(sendData)));
            byte[] data = System.Text.Encoding.UTF8.GetBytes(sendData);
            byte[] head = BitConverter.GetBytes(data.Length);
            byte[] result = new byte[data.Length + head.Length];
            Buffer.BlockCopy(head, 0, result, 0, head.Length);
            Buffer.BlockCopy(data, 0, result, head.Length, data.Length);
            Send(host, port, result, token);
        }

        public void Send(string host, int port, byte[] sendData, UserToken token)
        {
            string endpoint = string.Format("{0}:{1}", host, port);
            SocketClient client = null;
            if (!_clientDict.ContainsKey(endpoint))
            {
                client = new SocketClient(host, port, bufferSize);
                client.ReceiveHandle += ReceiveCompleted;
                _clientDict.Add(endpoint, client);
            }
            else
            {
                client = _clientDict[endpoint];
            }
            if (client.Socket.Connected || client.Connect())
            {
                client.SendAsync(sendData);
                client.ReceiveAsyncResult();
            }
            else
            {
                throw new Exception("连接服务器失败");
            }
        }

        private string CreateSigin(string str)
        {
            string attachParam = str + "44CAC8ED53714BF18D60C5C7B6296000";
            string key = FormsAuthentication.HashPasswordForStoringInConfigFile(attachParam, "MD5");
            return key.ToLower();
        }
    }
}

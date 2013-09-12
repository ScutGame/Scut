using System;
using System.Net;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.SocketServer.Context;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Web;

namespace ZyGames.Framework.Game.SocketServer.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpTransponder : ClientTransponder
    {
        public event Action<HttpListenerResponse, byte[], int, int> ResponseCompleted;
        public Action<string, byte[]> GameReceiveCompleted;

        public void Request(HttpConnection connection, string clientAddress, byte[] data)
        {
            string paramString = string.Empty;
            try
            {
                paramString = Encoding.ASCII.GetString(data);
                PacketMessage packet = ParsePacketMessage(clientAddress, paramString, ConnectType.Http);
                packet.Head.SSID = connection.SSID.ToString("N");
                HttpConnectionManager.Push(connection);

                if (ReceiveCompleted != null)
                {
                    //分发送到游戏服
                    byte[] packData = packet.ToByte();
                    string successMsg = string.Format("{0}>>{1}接收到{2}字节！",
                        DateTime.Now.ToString("HH:mm:ss:ms"), clientAddress, data.Length);
                    ReceiveCompleted.BeginInvoke(clientAddress, packData, OnReceiveCompleted, successMsg);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Receive form http request:{0} error:{1}", paramString, ex);
            }
        }


        protected override void OnSendToClient(PacketMessage packet)
        {
            if (packet.Head.ConnectType == ConnectType.Http)
            {
                string ssid = packet.Head.SSID;
                var connection = HttpConnectionManager.Get(ssid);
                if (connection != null)
                {
                    connection.TimeoutTimer.Dispose();
                    OnResponseCompleted(connection, packet.Content);
                }
                else
                {
                    Console.WriteLine("Error of http connection is empty.");
                }
                return;
            }
            base.OnSendToClient(packet);
        }

        public void RequestTimeout(HttpConnection connection)
        {
            var param = connection.Param;
            int msgId = param.Get("MsgId").ToInt();
            int actionId = param.Get("ActionId").ToInt();
            int errorCode = LanguageHelper.GetLang().ErrorCode;
            string errorMsg = LanguageHelper.GetLang().RequestTimeout;
            var head = new MessageHead(msgId, actionId, "st", errorCode, errorMsg);
            head.HasGzip = true;
            var ms = new MessageStructure();
            ms.WriteBuffer(head);
            byte[] data = ms.ReadBuffer();

            string remoteAddress = connection.Context.Request.RemoteEndPoint.Address.ToString();
            string successMsg = string.Format("{0}>>发送超时到{1} {2}字节！",
                                              DateTime.Now.ToString("HH:mm:ss:ms"), remoteAddress, data.Length);
            Console.WriteLine(successMsg);
            OnResponseCompleted(connection, data);
        }

        private void OnResponseCompleted(HttpConnection connection, byte[] data)
        {
            HttpConnectionManager.Remove(connection);
            if (ResponseCompleted != null)
            {
                string address = connection.Context.Request.UserHostAddress;
                string successMsg = string.Format("{0}>>准备发送到{1}[Http]的字节数:{2}byte...",
                                                  DateTime.Now.ToString("HH:mm:ss:ms"), address, data.Length);
                Console.WriteLine(successMsg);
                ResponseCompleted(connection.Context.Response, data, 0, data.Length);
            }
        }
    }
}

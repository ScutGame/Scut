using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.SocketServer.Context;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.Game.SocketServer.Net
{
    /// <summary>
    /// 接收或传送给客户端操作
    /// </summary>
    public class ClientTransponder : SocketTransponder
    {
        public ClientTransponder()
        {
        }

        /// <summary>
        /// 从客户端接收数据
        /// </summary>
        /// <param name="clientAddress"></param>
        /// <param name="data"></param>
        public override void Receive(string clientAddress, byte[] data)
        {
            try
            {
                //解析头部信息,转换成统一的流格式：head[MsgType(1)+uid(4)+gameId(4)+serverId(4)+gzip(1)] + (len(4)+data)
                BufferReader reader = new BufferReader(data);
                string paramString = reader.ReadPacketString();
                paramString = HttpUtility.UrlDecode(paramString, Encoding.UTF8);
                int index = paramString.IndexOf("?d=");
                if (index != -1)
                {
                    index += 3;
                    paramString = paramString.Substring(index, paramString.Length - index);
                }
                PacketMessage packet = ParsePacketMessage(clientAddress, paramString, ConnectType.Tcp);
                var token = new UserToken();
                token.GameId = packet.Head.GameId;
                token.ServerId = packet.Head.ServerId;
                token.Uid = packet.Head.Uid;
                ClientConnectManager.Push(clientAddress, token);

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
                TraceLog.WriteError("Receive form client {0} error:{1}", clientAddress, ex);
            }
        }

        protected void OnReceiveCompleted(IAsyncResult ar)
        {
            string address = ar.AsyncState as string;
            Console.WriteLine(address);
        }

        protected PacketMessage ParsePacketMessage(string clientAddress, string paramString, ConnectType connectType)
        {
            ParamGeter paramGeter = new ParamGeter(paramString);
            PacketHead head = new PacketHead(connectType, PacketMsgType.Request);
            head.Address = clientAddress;
            head.MsgId = paramGeter.GetInt("msgid");
            head.Uid = paramGeter.GetInt("uid");
            head.ActionId = paramGeter.GetInt("actionId");
            string[] sidArray = paramGeter.GetString("sid").Split('|');
            if (sidArray.Length > 2)
            {
                head.GameId = sidArray[1].ToInt();
                head.ServerId = sidArray[2].ToInt();
            }
            else
            {
                head.GameId = paramGeter.GetInt("gametype");
                head.ServerId = paramGeter.GetInt("serverid");
            }
            head.EnableGzip = false;
            PacketMessage packet = new PacketMessage();
            packet.Head = head;
            packet.Content = BufferUtils.GetBytes(paramString);
            return packet;
        }


        /// <summary>
        /// 发送数据到客户端
        /// </summary>
        /// <param name="clientAddress"></param>
        /// <param name="data"></param>
        public override void Send(string clientAddress, byte[] data)
        {
            try
            {
                PacketMessage packet = PacketMessage.Parse(data);
                OnSendToClient(packet);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Send to client {0} error:{1}", clientAddress, ex);
            }
        }

        protected virtual void OnSendToClient(PacketMessage packet)
        {
            var clientAddress = packet.Head.Address;
            byte[] content = packet.Content;
            if (!packet.Head.EnableGzip)
            {
                //gzip压缩包
                MessageStructure ds = new MessageStructure();
                ds.WriteGzipBuffer(content);
                packet.Content = ds.ReadBuffer();
            }
            byte[] data = packet.Content;
            OnSendCompleted(clientAddress, data);
        }

        public override void OnReceiveTimeout(string clientAddress, byte[] receiveData)
        {
            try
            {
                BufferReader reader = new BufferReader(receiveData);
                string paramString = reader.ReadPacketString();
                paramString = HttpUtility.UrlDecode(paramString, Encoding.UTF8);
                int index = paramString.IndexOf("?d=");
                if (index != -1)
                {
                    index += 3;
                    paramString = paramString.Substring(index, paramString.Length - index);
                }
                PacketMessage receivePacket = ParsePacketMessage(clientAddress, paramString, ConnectType.Tcp);
                var recHead = receivePacket.Head;

                int errorCode = LanguageHelper.GetLang().ErrorCode;
                string errorMsg = LanguageHelper.GetLang().RequestTimeout;
                MessageHead head = new MessageHead(recHead.MsgId, recHead.ActionId, "st", errorCode, errorMsg);
                head.HasGzip = true;
                MessageStructure ds = new MessageStructure();
                ds.WriteBuffer(head);
                byte[] data = ds.ReadBuffer();
                OnSendCompleted(clientAddress, data);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Send to client {0} timeout error:{1}", clientAddress, ex);
            }
        }

        protected void OnSendCompleted(string clientAddress, byte[] data)
        {
            if (SendCompleted != null)
            {
                string successMsg = string.Format("{0}>>准备发送到{1}的字节数:{2}byte...",
                                                  DateTime.Now.ToString("HH:mm:ss:ms"), clientAddress, data.Length);
                SendCompleted.BeginInvoke(clientAddress, data,
                    ar =>
                    {
                        string address = ar.AsyncState as string;
                        Console.WriteLine(address);
                    }, successMsg);
            }
        }

    }
}

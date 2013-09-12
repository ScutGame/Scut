using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.SocketServer.Context;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.Game.SocketServer.Net
{
    /// <summary>
    /// 接收或传送给游戏服
    /// </summary>
    public class GameTransponder : SocketTransponder
    {
        public GameTransponder()
        {
            this.SocketClosing += OnSocketClosing;
        }

        /// <summary>
        /// 从游戏服接收数据
        /// </summary>
        /// <param name="gameAddress">游戏服的地址</param>
        /// <param name="data"></param>
        public override void Receive(string gameAddress, byte[] data)
        {
            try
            {
                PacketMessage packet = PacketMessage.Parse(data);
                var head = packet.Head;
                GameSession session = null;
                switch (head.MsgType)
                {
                    case PacketMsgType.None:
                        //心跳包
                        session = GameSessionManager.GetSession(head.GameId, head.ServerId);
                        if (session != null)
                        {
                            session.GameAddress = gameAddress;
                            session.AccessTime = DateTime.Now;
                            Console.WriteLine("{0}>>Connect to host of game server {1}-{2} from {3}", DateTime.Now.ToLongTimeString(), head.GameId, head.ServerId, gameAddress);
                        }
                        break;
                    case PacketMsgType.Register:
                        session = new GameSession(head.GameId, head.ServerId);
                        session.GameAddress = gameAddress;
                        session.AccessTime = DateTime.Now;
                        GameSessionManager.Register(session);
                        break;
                    case PacketMsgType.Push:
                        OnSendToClient(head.Address, data);
                        break;
                    case PacketMsgType.SendTo:
                    case PacketMsgType.Request:
                        //发送到其它通道
                        Send(head.Address, packet.ToByte());
                        break;
                    case PacketMsgType.Broadcast:
                        //广播到客户端
                        var clientPointList = ClientConnectManager.FindAll(head.GameId, head.ServerId);
                        foreach (var endPoint in clientPointList)
                        {
                            OnSendToClient(endPoint.ToString(), data);
                        }
                        break;
                    default:
                        throw new Exception(string.Format("The message type:{0} is not supported.", head.MsgType));
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Receive game {0} error:{1}", gameAddress, ex);
            }
        }

        /// <summary>
        /// 发送数据到游戏服
        /// </summary>
        /// <param name="clientAddress">客户端的地址</param>
        /// <param name="data"></param>
        public override void Send(string clientAddress, byte[] data)
        {
            PacketHead head = null;
            try
            {
                PacketMessage packet = PacketMessage.Parse(data);
                head = packet.Head;
                switch (head.MsgType)
                {
                    case PacketMsgType.Request:
                        var session = GameSessionManager.GetSession(head.GameId, head.ServerId);
                        if (session != null)
                        {
                            string gameAddress = session.GameAddress;
                            OnSendToGame(gameAddress, data);
                        }
                        else
                        {
                            var error = PacketMessage.CreateError(head, MessageError.NotFound, LanguageHelper.GetLang().ServerMaintain);
                            OnSendToClient(clientAddress, error.ToByte());
                            TraceLog.WriteError("Can not find the game:{0} server:{1}", head.GameId, head.ServerId);
                        }
                        break;
                    default:
                        throw new Exception(string.Format("The message type:{0} is not supported.", head.MsgType));

                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Send game to {0} error:{1}", clientAddress, ex);
                var error = PacketMessage.CreateError(head, MessageError.SystemError, LanguageHelper.GetLang().ServerBusy);
                OnSendToClient(clientAddress, error.ToByte());
            }
        }

        public override void OnReceiveTimeout(string remoteAddress, byte[] data)
        {
            
        }

        /// <summary>
        /// Tcp客户端连接断开，通知业务层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="clientPoint"></param>
        public void OnTcpClientClosed(object sender, EndPoint clientPoint)
        {
            var token = ClientConnectManager.GetToken(clientPoint.ToString());
            PacketHead head = new PacketHead(ConnectType.Tcp, PacketMsgType.Closed);
            head.GameId = token.GameId;
            head.ServerId = token.ServerId;
            head.Uid = token.Uid;
            var session = GameSessionManager.GetSession(head.GameId, head.ServerId);
            if (session == null)
            {
                return;
            }
            head.Address = session.GameAddress;
            head.EnableGzip = false;
            PacketMessage packet = new PacketMessage();
            packet.Head = head;
            packet.Content = new byte[0];
            if (CheckConnected(head.Address))
            {
                OnSendToGame(head.Address, packet.ToByte());
            }
        }

        private void OnSendToClient(string address, byte[] data)
        {
            if (ReceiveCompleted != null)
            {
                //推送到客户端
                ReceiveCompleted.BeginInvoke(address, data, null, null);
            }
        }

        private void OnSendToGame(string address, byte[] data)
        {
            if (SendCompleted != null)
            {
                //分发到游戏服
                SendCompleted.BeginInvoke(address, data, null, null);
            }
        }

        private void OnSocketClosing(object sender, EndPoint gamePoint)
        {
            if (sender is byte[])
            {
                PacketMessage packet = PacketMessage.Parse(sender as byte[]);
                PacketMessage error = PacketMessage.CreateError(packet.Head, MessageError.SystemError, LanguageHelper.GetLang().ServerBusy);
                OnSendToClient(packet.Head.Address, error.ToByte());
            }
        }

    }
}

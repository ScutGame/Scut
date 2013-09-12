using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Net;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// Socket宿主类
    /// </summary>
    public abstract class GameSocketHost
    {
        private SocketAsyncClient _socketClient;
        private readonly int _gameId;
        private readonly int _serverId;
        private readonly int _connectTimeout;
        private readonly int _intervalTime;
        private Thread _intervalThread;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="gameId"></param>
        /// <param name="serverId"></param>
        /// <param name="connectTimeout">连接超时（秒）</param>
        /// <param name="intervalTime">心跳包间隔时间（秒）</param>
        protected GameSocketHost(SocketAsyncClient socketClient, int gameId, int serverId, int connectTimeout, int intervalTime = 10)
        {
            _socketClient = socketClient;
            _gameId = gameId;
            _serverId = serverId;
            _connectTimeout = connectTimeout;
            _intervalTime = intervalTime;
            _socketClient.ReceiveCompleted += OnReceiveCompleted;
            _socketClient.SocketClosing += OnSocketClosing;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Bind()
        {
            if (!_socketClient.Connect(_connectTimeout * 1000))
            {
                throw new Exception(string.Format("Failed to connect to host {0}", _socketClient.RemoteAddress));
            }
            PacketMessage packet = new PacketMessage();
            packet.Head = new PacketHead(ConnectType.Tcp, PacketMsgType.Register);
            packet.Head.GameId = _gameId;
            packet.Head.ServerId = _serverId;
            packet.Head.EnableGzip = false;
            packet.Content = new byte[0];
            _socketClient.PushSend(packet.ToByte());
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (!_socketClient.Connected)
            {
                throw new Exception(string.Format("Failed to connect to host {0}", _socketClient.RemoteAddress));
            }
            _intervalThread = new Thread(new ParameterizedThreadStart(OnIntervalConnected));
            _intervalThread.Start();
            OnStartAffer();
        }

        private int _intervalErrorNum;

        private void OnIntervalConnected(object obj)
        {
            while (true)
            {
                //心跳包
                try
                {
                    Thread.Sleep(_intervalTime * 1000);
                    PacketMessage packet = new PacketMessage();
                    packet.Head = new PacketHead(ConnectType.Tcp, PacketMsgType.None);
                    packet.Head.GameId = _gameId;
                    packet.Head.ServerId = _serverId;
                    packet.Head.EnableGzip = false;
                    packet.Content = new byte[0];
                    if (!_socketClient.PushSend(packet.ToByte()))
                    {
                        _intervalErrorNum++;
                        if (_intervalErrorNum > 3)
                        {
                            _intervalErrorNum = 0;
                            TraceLog.WriteError("Failed of connect to host:{0}", _socketClient.RemoteAddress);
                            Bind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("OnIntervalConnected error:{0}", ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            try
            {
                _intervalThread.Abort();
            }
            catch { }
            OnServiceStop(this, new EventArgs());
            _socketClient.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userList"></param>
        /// <param name="actionId"></param>
        /// <param name="parameters"></param>
        /// <param name="successHandle"></param>
        public void SendAsyncAction<T>(List<T> userList, int actionId, Parameters parameters, Action<HttpGet> successHandle) where T : BaseUser
        {
            StringBuilder shareParam = new StringBuilder();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    shareParam.AppendFormat("&{0}={1}", parameter.Key, parameter.Value);
                }
            }
            foreach (var user in userList)
            {
                try
                {
                    if (user == default(T))
                    {
                        continue;
                    }
                    int userId = user.GetUserId();
                    HttpGet httpGet;
                    byte[] sendData = ActionFactory.GetActionResponse(userId, user, shareParam.ToString(), out httpGet);
                    SendAsync(userId, "", user.RemoteAddress, (ConnectType)user.ConnectType, sendData);
                    if (httpGet != null)
                    {
                        successHandle(httpGet);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("SendToClient error:{0}", ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="ssid"></param>
        /// <param name="toAddress"></param>
        /// <param name="connectType"></param>
        /// <param name="data"></param>
        /// <param name="enableGzip"></param>
        public void SendAsync(int uid, string ssid, string toAddress, ConnectType connectType, byte[] data, bool enableGzip = false)
        {
            PacketMessage respPacket = new PacketMessage();
            respPacket.Head = new PacketHead(connectType, PacketMsgType.Push);
            respPacket.Head.GameId = _gameId;
            respPacket.Head.ServerId = _serverId;
            respPacket.Head.SSID = ssid;
            respPacket.Head.Address = toAddress;
            respPacket.Head.Uid = uid;
            respPacket.Head.EnableGzip = enableGzip;
            respPacket.Content = data;

            if (respPacket.Head.EnableGzip)
            {
                //gzip压缩包
                MessageStructure ds = new MessageStructure();
                ds.WriteGzipBuffer(respPacket.Content);
                respPacket.Content = ds.ReadBuffer();
            }
            SendAsync(respPacket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SendAsync(PacketMessage message)
        {
            _socketClient.PushSend(message.ToByte());
        }

        protected abstract void OnRequested(HttpGet httpGet, IGameResponse response);

        protected abstract void OnStartAffer();

        protected abstract void OnServiceStop(object sender, EventArgs eventArgs);

        protected abstract void ProcessPacket(PacketMessage packet);

        protected virtual void OnSocketClosing(EndPoint remotePoint)
        {
        }

        private void OnReceiveCompleted(SocketAsyncClient client, string remoteAddress, byte[] data)
        {
            PacketHead head = null;
            try
            {
                PacketMessage reqPacket = PacketMessage.Parse(data);
                head = reqPacket.Head;
                if (head.MsgType == PacketMsgType.Request)
                {
                    try
                    {
                        string param = new BufferReader(reqPacket.Content).ReadPacketString();
                        SocketGameResponse response = new SocketGameResponse();
                        HttpGet httpGet = new HttpGet(param, head.Address);
                        OnRequested(httpGet, response);
                        byte[] sendData = response.ReadByte();
                        SendAsync(head.Uid, head.SSID, head.Address, head.ConnectType, sendData);
                    }
                    catch (Exception e)
                    {
                        var error = PacketMessage.CreateError(head, MessageError.SystemError, LanguageHelper.GetLang().ServerBusy);
                        try
                        {
                            SendAsync(head.Uid, head.SSID, head.Address, head.ConnectType, error.ToByte());
                        }
                        catch (Exception er)
                        {
                            TraceLog.WriteError("Receive error:{0}", e);
                        }
                    }
                }
                else
                {
                    ProcessPacket(reqPacket);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Receive error:{0}", ex);

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
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
        private readonly SocketSettings _setting;
        private readonly PacketBaseHead _packetHead;
        private SocketListener listener;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="packetHead"></param>
        protected GameSocketHost(SocketSettings setting, PacketBaseHead packetHead)
        {
            _setting = setting;
            _packetHead = packetHead;
            listener = new SocketListener(setting);
            listener.ConnectCompleted += OnConnectCompleted;
            listener.ReceiveCompleted += OnReceiveCompleted;
            listener.SocketClosing += OnSocketClosing;
        }

        protected abstract void OnConnectCompleted(SocketProcessEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            listener.Listen();
            OnStartAffer();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            listener.Stop();
            OnServiceStop();
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
                    HttpGet httpGet;
                    byte[] sendData = ActionFactory.GetActionResponse(actionId, user, shareParam.ToString(), out httpGet);
                    SendAsync(user.SocketSid, sendData);
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
        /// <param name="sessionId"></param>
        /// <param name="data"></param>
        public void SendAsync(Guid sessionId, byte[] data)
        {
            var socketObject = listener.GetSession(sessionId);
            listener.PushSend(socketObject, data);
        }

        protected abstract void OnRequested(HttpGet httpGet, IGameResponse response);

        protected abstract void OnStartAffer();

        protected abstract void OnServiceStop();

        protected virtual void OnSocketClosing(SocketProcessEventArgs e)
        {
        }

        private void OnReceiveCompleted(SocketProcessEventArgs e)
        {
            try
            {
                SocketGameResponse response = new SocketGameResponse();
                //string param = "Sid=&Uid=&ActionID=1004&MobileType=1&Pid=z17997&Pwd=%25A9%25F0%2506m%2508%25D9%25EB%2528O%25BDYR9%25AA%2583%25D0&DeviceID=&GameType=5&ScreenX=&ScreenY=&RetailID=0000&ServerID=1&RetailUser=&ClientAppVersion=&rl=1";
                string param = Encoding.ASCII.GetString(e.Data);
                int index = param.IndexOf("?d=");
                if (index != -1)
                {
                    param = param.Substring(index, param.Length - index);
                    param = HttpUtility.ParseQueryString(param)["d"];
                }
                HttpGet httpGet = new HttpGet(param, e.Socket.SessionId, e.Socket.RemoteEndPoint.ToString());
                OnRequested(httpGet, response);
                byte[] sendData = response.ReadByte();

                //sendData = BufferUtils.MergeBytes(
                //    e.Data,
                //    sendData);
                PacketData packetData = new PacketData(_packetHead, sendData);
                listener.PushSend(e.Socket, packetData);

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Receive error:{0}", ex);

            }
        }

    }
}

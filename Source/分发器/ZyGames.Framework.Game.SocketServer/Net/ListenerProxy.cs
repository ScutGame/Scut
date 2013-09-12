using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.SocketServer.Context;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.Game.SocketServer.Net
{
    public class ListenerProxy
    {
        private readonly SocketTransponder _transponder;
        private ConnectSetting _setting;
        private SocketListener _listener;

        public ListenerProxy(SocketTransponder transponder, ConnectSetting setting)
        {
            _transponder = transponder;
            _setting = setting;
            var localPoint = SocketSettings.GetHostAddress(_setting.Host, _setting.Port);
            var socketSetting = new SocketSettings(localPoint, setting.MaxPoolSize, setting.BufferSize, setting.Backlog);
            socketSetting.ContinuedTimeout = _setting.ConnectTimeout;
            socketSetting.EnableReceiveTimeout = _setting.EnableReceiveTimeout;
            socketSetting.ReceiveTimeout = _setting.ReceiveTimeout;

            _listener = new SocketListener(socketSetting);
            _listener.ConnectCompleted += OnConnectCompleted;
            _listener.ReceiveCompleted += _transponder.Receive;
            _listener.SocketSending += _transponder.OnSending;
            _listener.SocketClosing += OnSocketClosing;
            _listener.ReceiveTimeout += _transponder.OnReceiveTimeout;
            _transponder.SendCompleted += (address, data) =>
            {
                if (!_listener.PushSend(address, data))
                {
                    Console.WriteLine("发送到{0} {1}byte失败", address, data.Length);
                }
            };
            _transponder.CheckConnectedHandle += OnCheckConnected;
        }

        private void OnConnectCompleted(SocketAsyncEventArgs e)
        {
            if (_transponder.ConnectCompleted != null)
            {
                _transponder.ConnectCompleted(e.AcceptSocket.RemoteEndPoint);
            }
        }

        private bool OnCheckConnected(string remoteAddress)
        {
            return _listener.CheckConnected(remoteAddress);
        }

        private void OnSocketClosing(object sender, EndPoint remotePoint)
        {
            try
            {
                string remoteAddress = remotePoint.ToString();
                if (!string.IsNullOrEmpty(remoteAddress))
                {
                    if (_transponder.SocketClosing != null)
                    {
                        _transponder.SocketClosing.BeginInvoke(sender, remotePoint, OnCloseCompleted, remoteAddress);
                    }
                    else
                    {
                        ClientConnectManager.Remove(remoteAddress);
                    }
                }
                TraceLog.ReleaseWrite("The {0} socket connection is closed.", remoteAddress);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnSocketClosing error:{0}", ex);
            }
        }

        private void OnCloseCompleted(IAsyncResult ar)
        {
            string remoteAddress = ar.AsyncState as string;
            if (!string.IsNullOrEmpty(remoteAddress))
            {
                ClientConnectManager.Remove(remoteAddress);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Listen()
        {
            _listener.Listen();
        }

        public void Dispose()
        {
            _listener.Stop();
            _listener.Close();
            _listener.Dispose();
        }

    }
}

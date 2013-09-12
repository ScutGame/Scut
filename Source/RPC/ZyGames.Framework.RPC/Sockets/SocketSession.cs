using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Socket会话对象
    /// </summary>
    internal class SocketSession
    {
        private BufferPacket _receivePacket;
        private BufferPacket _sendPacket;
        private DateTime _preAccessTime;
        private int _bufferOffset;

        /// <summary>
        /// 
        /// </summary>
        public SocketSession(int offset)
        {
            _bufferOffset = offset;
            _receivePacket = new BufferPacket();
            _sendPacket = new BufferPacket();
        }

        /// <summary>
        /// 
        /// </summary>
        public EndPoint LocalEndPoint { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EndPoint RemoteEndPoint { get; set; }
        /// <summary>
        /// 用户自定数据
        /// </summary>
        public object UserData
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        private Timer _timeoutTimer;

        public bool EnableTimeoutState
        {
            get;
            private set;
        }
        /// <summary>
        /// 开启超时处理
        /// </summary>
        public void EnableTimeout(TimerCallback callback, object state, int timeout)
        {
            EnableTimeoutState = true;
            _timeoutTimer = new Timer(callback, state, timeout, Timeout.Infinite);
        }

        /// <summary>
        /// 关闭超时等待
        /// </summary>
        public void CloseTimeout()
        {
            EnableTimeoutState = false;
            _timeoutTimer.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _receivePacket.Init();
            _sendPacket.Init();
            RemoteEndPoint = null;
            UserData = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            _preAccessTime = DateTime.Now;
        }

        internal int BufferOffset
        {
            get { return _bufferOffset; }
        }

        internal BufferPacket ReceivePacket
        {
            get { return _receivePacket; }
        }

        internal BufferPacket SendPacket
        {
            get { return _sendPacket; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LocalAddress
        {
            get { return LocalEndPoint == null ? "" : LocalEndPoint.ToString(); }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RemoteAddress
        {
            get { return RemoteEndPoint == null ? "" : RemoteEndPoint.ToString(); }
        }

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime AccessTime
        {
            get
            {
                return _preAccessTime;
            }
        }


    }
}

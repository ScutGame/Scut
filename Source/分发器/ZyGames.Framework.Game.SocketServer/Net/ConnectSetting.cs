using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.SocketServer.Net
{
    public class ConnectSetting
    {
        public ConnectSetting()
        {
            
        }

        public string Host { get; set; }

        public int Port { get; set; }

        public int Backlog { get; set; }

        public int ConnectTimeout { get; set; }

        public int BufferSize { get; set; }

        public int MinPoolSize { get; set; }

        public int MaxPoolSize { get; set; }

        public bool EnableReceiveTimeout { get; set; }

        public int ReceiveTimeout { get; set; }
    }
}

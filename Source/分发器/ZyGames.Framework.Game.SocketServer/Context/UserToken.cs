using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.SocketServer.Context
{
    public class UserToken
    {
        public int Uid { get; set; }

        public int GameId { get; set; }

        public int ServerId { get; set; }
    }
}

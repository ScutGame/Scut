using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Contract;

namespace GameNotice
{
    class Program
    {
        static void Main(string[] args)
        {
            GameHost.Start(new GameHostApp());
        }
    }
}

using System;
using System.Collections.Generic;
using ZyGames.Doudizhu.Bll.Logic;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Script;
using ZyGames.Framework.Model;
using ZyGames.Framework.Plugin.PythonScript;

namespace ZyGames.Doudizhu.HostServer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            GameHost.Start();
        }
    }
}

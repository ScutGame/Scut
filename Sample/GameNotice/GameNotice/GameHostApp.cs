using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.NetLibrary;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Wcf;

namespace GameNotice
{
    class GameHostApp : GameSocketHost
    {
        private static GameHostApp instance;

        static GameHostApp()
        {
            instance = new GameHostApp();
        }

        private GameHostApp()
        {
        }

        public static GameHostApp Current
        {
            get { return instance; }
        }

        protected override void OnConnectCompleted(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine("Client:{0} connect to server.", e.Socket.RemoteEndPoint);
        }


        protected override void OnRequested(HttpGet httpGet, IGameResponse response)
        {
            try
            {
                ActionFactory.Request(httpGet, response, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
            }
        }

        protected override void OnStartAffer()
        {
            //时间间隔更新库
            int cacheInterval = 600;
            BaseLog log = null;
            try
            {
                log = new BaseLog();
                GameEnvironment.Start(cacheInterval, () => true);
                Console.WriteLine("The server is staring...");
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.SaveLog(ex);
                }
            }
        }

        protected override void OnServiceStop()
        {
        }

    }
}

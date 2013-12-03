using System;
using GameNotice.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Model;
using ZyGames.Framework.RPC.Sockets;

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
            try
            {
                //时间间隔更新库
                int cacheInterval = 600;
                GameEnvironment.Start(cacheInterval, () =>
                {
                    ActionFactory.SetActionIgnoreAuthorize(2001, 404);
                    InitNotice();
                    return true;
                });
                Console.WriteLine("The server is staring...");

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("The server start error:{0}", ex);
            }
        }

    private void InitNotice()
    {
        var cacheSet = new ShareCacheStruct<Notice>();
        for (int i = 0; i < 5; i++)
        {
            int id = (int)cacheSet.GetNextNo();
            Notice notice = new Notice(id);
            notice.Title = "tile" + id;
            notice.Content = "Content" + id;
            notice.CreateDate = DateTime.Now;
            cacheSet.Add(notice);
        }
    }

        protected override void OnServiceStop()
        {
            GameEnvironment.Stop();
        }

    }
}

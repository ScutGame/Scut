using System;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.Sockets;

namespace HelloWorld
{
    class Program : GameSocketHost
    {
        static void Main(string[] args)
        {
            new Program().Start();
        }
        
        protected override void OnConnectCompleted(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine("Client:{0} connect to server.", e.Socket.RemoteEndPoint);
        }

        protected override void OnRequested(HttpGet httpGet, IGameResponse response)
        {
            Console.WriteLine("Request data:{0}", httpGet.ParamString);
        }

        protected override void OnStartAffer()
        {
            try
            {
                //时间间隔更新库
                int cacheInterval = 600;
                GameEnvironment.Start(cacheInterval, () => true);
                Console.WriteLine("The server is staring...");
                Console.WriteLine("Helo world.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error:{0}", ex.Message);
            }
        }

        protected override void OnServiceStop()
        {
            GameEnvironment.Stop();
            Console.WriteLine("The server is stoped");
        }
    }
}

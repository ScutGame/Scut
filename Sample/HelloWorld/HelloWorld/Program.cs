using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Wcf;

namespace HelloWorld
{
    class Program : GameHost
    {
        static void Main(string[] args)
        {
            Start(new Program());
        }

        protected override void DoListen()
        {
            this.ServiceProxy.Listen(9000);
        }

        protected override void ListenAfter()
        {
            Console.WriteLine("Hello World");
        }

        protected override void OnRequested(HttpGet httpGet, IGameResponse response)
        {
        }

        protected override void OnCallRemote(string route, HttpGet httpGet, MessageHead head, MessageStructure structure)
        {
        }

        protected override void OnClosed(ChannelContext context, string remoteaddress)
        {
        }

        protected override void OnSocketClosed(ChannelContext context, string remoteaddress)
        {
        }

        protected override void OnServiceStop(object sender, EventArgs eventArgs)
        {
        }
    }
}

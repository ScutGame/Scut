using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Wcf;

namespace GameNotice
{
    class GameHostApp : GameHost
    {
        protected override void OnRequested(HttpGet httpGet, IGameResponse response)
        {
            try
            {
                ActionFactory.Request(httpGet, response, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}",ex.Message);
            }
        }

        //protected override void DoListen()
        //{
        //    this.ServiceProxy.Listen(9001);
        //}

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

/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
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
            Console.WriteLine("Request data ActionId:{0},ip:{1}", httpGet.ActionId, httpGet.RemoteAddress);
            ActionFactory.Request(httpGet, response, null);
        }

        protected override void OnStartAffer()
        {
            try
            {
                //时间间隔更新库
                int cacheInterval = 600;
                GameEnvironment.Start(cacheInterval, () => true);
                Console.WriteLine("The server is staring...");
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

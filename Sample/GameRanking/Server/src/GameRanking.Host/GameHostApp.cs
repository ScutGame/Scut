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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.Script;

namespace GameRanking.Host
{
    public class GameHostApp : GameSocketHost
    {

        protected override void OnConnectCompleted(object sender, ConnectionEventArgs e)
        {
        }

        protected override BaseUser GetUser(int userId)
        {
            return null;
        }

        protected override void OnRequested(HttpGet httpGet, IGameResponse response)
        {
            try
            {
                ActionFactory.Request(httpGet, response, GetUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OnRequested error:{0}", ex.Message);
            }
        }

        protected override void OnStartAffer()
        {
            try
            {
                var setting = new EnvironmentSetting();
                setting.EntityAssembly = Assembly.Load("GameRanking.Model");
                setting.ScriptStartBeforeHandle += () =>
                {
                    ScriptEngines.AddReferencedAssembly("GameRanking.Model.dll");
                    ActionFactory.SetActionIgnoreAuthorize(1000, 1001);
                };
                GameEnvironment.Start(setting);
                Console.WriteLine("The server is staring...");
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("App star error:{0}", ex);
            }
        }

        protected override void OnServiceStop()
        {
        }
    }
}
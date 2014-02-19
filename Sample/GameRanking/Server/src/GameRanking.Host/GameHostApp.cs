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
using System.Web;
using GameRanking.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Net;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.Script;

namespace GameRanking.Host
{
    public class GameHostApp : GameSocketHost
    {

        protected override void OnConnectCompleted(object sender, ConnectionEventArgs e)
        {
            var session = GameSession.Get(e.Socket.HashCode);
            if (session != null)
            {
                var user = new GuestUser();
                user.Init();
                var p = new PersonalCacheStruct<GuestUser>();
                p.Add(user);
                session.BindIdentity(user.GetUserId());
            }
        }


        protected override BaseUser GetUser(int userId)
        {
            return new PersonalCacheStruct<GuestUser>().FindKey(userId.ToString());
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
            var setting = new EnvironmentSetting();
            setting.EntityAssembly = Assembly.Load("GameRanking.Model");
            ScriptEngines.AddReferencedAssembly("GameRanking.Model.dll");
            ActionFactory.SetActionIgnoreAuthorize(1000, 1001);
            GameEnvironment.Start(setting);
            int pushInterval = ConfigUtils.GetSetting("Ranking.PushInterval", 60);
            TimeListener.Append(new PlanConfig(DoPushRanking, true, pushInterval, "GetRanking"));
            Console.WriteLine("The server is staring...");
        }

        private void DoPushRanking(PlanConfig planconfig)
        {
            try
            {

                Console.WriteLine("{0}>>The server push ranking", DateTime.Now.ToString("HH:mm:ss"));
                var sessionList = GameSession.GetAll();
                int actionId = 1001;
                StringBuilder shareParam = new StringBuilder();
                shareParam.AppendFormat("&{0}={1}", "PageIndex", "1");
                shareParam.AppendFormat("&{0}={1}", "PageSize", "50");
                HttpGet httpGet;
                byte[] sendData = ActionFactory.GetActionResponse(actionId, new GuestUser(), shareParam.ToString(), out httpGet);
                foreach (var session in sessionList)
                {
                    if (session.SendAsync(sendData, 0, sendData.Length))
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("PushRanking error:{0}", ex);
            }
        }

        protected override void OnServiceStop()
        {
        }
    }
}
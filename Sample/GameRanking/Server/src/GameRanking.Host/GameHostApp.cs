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
        private DictionaryExtend<string, GuestUser> _sessions = new DictionaryExtend<string, GuestUser>();

        protected override void OnConnectCompleted(object sender, ConnectionEventArgs e)
        {
            string address = e.Socket.RemoteEndPoint.ToString();
            if (!_sessions.ContainsKey(address))
            {
                var user = new GuestUser();
                user.Init();
                _sessions[address] = user;
            }
        }

        protected override void OnDisconnected(GameSession session)
        {
            UnLine((int)session.UserId);
        }

        private void UnLine(int userId)
        {
            var pair = _sessions.Where(p => p.Value.UserId == userId).FirstOrDefault();
            _sessions.Remove(pair);
        }

        protected override BaseUser GetUser(int userId)
        {
            return _sessions.Where(pair => pair.Value.UserId == userId).Select(s => s.Value).FirstOrDefault();
        }

        protected override void OnRequested(HttpGet httpGet, IGameResponse response)
        {
            try
            {
                GuestUser user = null;
                if (_sessions.ContainsKey(httpGet.RemoteAddress))
                {
                    user = _sessions[httpGet.RemoteAddress];
                }
                ActionFactory.Request(httpGet, response, GetUser);
                if (user != null)
                {
                    httpGet.LoginSuccessCallback(user.UserId);
                }
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
                int pushInterval = ConfigUtils.GetSetting("Ranking.PushInterval", 60);
                TimeListener.Append(new PlanConfig(DoPushRanking, true, pushInterval, "GetRanking"));
                Console.WriteLine("The server is staring...");
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("App star error:{0}", ex);
            }
        }

        private void DoPushRanking(PlanConfig planconfig)
        {
            try
            {

                Console.WriteLine("{0}>>The server push ranking", DateTime.Now.ToString("HH:mm:ss"));
                var sessionList = _sessions.Values.ToList();
                int actionId = 1001;
                StringBuilder shareParam = new StringBuilder();
                shareParam.AppendFormat("&{0}={1}", "PageIndex", "1");
                shareParam.AppendFormat("&{0}={1}", "PageSize", "50");
                HttpGet httpGet;
                byte[] sendData = ActionFactory.GetActionResponse(actionId, new GuestUser(), shareParam.ToString(), out httpGet);
                foreach (var user in sessionList)
                {
                    if (!SendAsync(user.UserId, sendData))
                    {
                        UnLine(user.UserId);
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
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using GameRanking.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Event;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Model;
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

                    var cache = new ShareCacheStruct<UserRanking>();
                    Stopwatch t = new Stopwatch();
                    t.Start();
                    var list = cache.FindAll(false);
                    t.Stop();
                    if(list.Count > 0)
                    {
                        
                    }
                };

                var cacheSetting = new CacheSetting();
                cacheSetting.ChangedHandle += OnChangedNotify;
                GameEnvironment.Start(setting, cacheSetting);
                Console.WriteLine("The server is staring...");
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("App star error:{0}", ex);
            }
        }

        private void OnChangedNotify(AbstractEntity sender, CacheItemEventArgs e)
        {
            if (sender != null)
            {
                //这里处理Model更新通知
                Console.WriteLine("update:{0},{1},{2}", sender.GetType().Name, e.ChangeType, e.PropertyName);
                EntitySyncManger.OnChange(sender, e);
            }
        }

        protected override void OnServiceStop()
        {
        }
    }
}
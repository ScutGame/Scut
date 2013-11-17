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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Script;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Plugin.PythonScript;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Tianjiexing.BLL
{
    public class AppGameHost : GameSocketHost
    {
        private static AppGameHost instance;

        static AppGameHost()
        {
            instance = new AppGameHost();
        }

        private AppGameHost()
        {
        }

        public static AppGameHost Current
        {
            get { return instance; }
        }


        protected override void OnStartAffer()
        {
            //时间间隔更新库
            int cacheInterval = 600;
            try
            {
                GameEnvironment.ClientDesDeKey = "j6=9=1ac";

                GameEnvironment.Start(cacheInterval, () =>
                {
                    SystemGlobal.Run();
                    PythonContext pythonContext;
                    PythonScriptManager.Current.TryLoadPython(@"Lib/action.py", out pythonContext);
                    RouteItem routeItem;
                    PythonScriptManager.Current.TryGetAction(1008, out routeItem);
                    return true;
                });

               
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnStartAffer error:{0}", ex);
            }
        }

        protected override void OnServiceStop()
        {
            SystemGlobal.Stop();
            GameEnvironment.Stop();
        }


        protected override void OnConnectCompleted(object sender, ConnectionEventArgs e)
        {
        }

        protected override void OnRequested(HttpGet httpGet, IGameResponse response)
        {
            try
            {
                //int actionId = 0;
                //if (httpGet.GetInt("actionId", ref actionId))
                //{
                //    Console.WriteLine("request actionId:{0}", actionId);

                //}
                ActionFactory.Request(httpGet, response, userId => new GameDataCacheSet<GameUser>().FindKey(userId.ToNotNullString()));
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("d={0},{1}", httpGet.ParamString, ex.ToString());
            }
        }

    }
}
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
using System.Threading.Tasks;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.Http;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GameHttpHost : GameBaseHost
    {

        private HttpAsyncHost httpListener;

        /// <summary>
        /// 
        /// </summary>
        protected GameHttpHost()
            : this(HttpAsyncHandler.Default)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected GameHttpHost(IHttpAsyncHandler handler)
        {
            listenUrls = new List<string>();
            httpListener = new HttpAsyncHost(handler, 16);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitLoad()
        {
            var httpHost = ConfigUtils.GetSetting("Game.Http.Host");
            var httpPort = ConfigUtils.GetSetting("Game.Http.Port", 80);
            var httpName = ConfigUtils.GetSetting("Game.Http.Name", "Service.aspx");
            if (!string.IsNullOrEmpty(httpHost))
            {
                var hosts = httpHost.Split(',');
                foreach (var host in hosts)
                {
                    string address = host.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)
                        ? host
                        : "http://" + host;
                    listenUrls.Add(string.Format("{0}:{1}/{2}/", address, httpPort, httpName));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected List<string> listenUrls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Start(string[] args)
        {
            InitLoad();
            httpListener.Run(listenUrls.ToArray());
            OnStartAffer();
            base.Start(args);
        }

        /// <summary>
        /// Raises the start affer event.
        /// </summary>
        protected abstract void OnStartAffer();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Request(System.Web.HttpContext context)
        {
            var actionDispatcher = GameEnvironment.Setting.ActionDispatcher;
            RequestPackage package;
            if (!actionDispatcher.TryDecodePackage(context, out package))
            {
                return;
            }
            var session = GetSession(context, package);
            package.Bind(session);
            ActionGetter actionGetter = actionDispatcher.GetActionGetter(package, session);
            BaseGameResponse response = new HttpGameResponse(context.Response);
            response.WriteErrorCallback += actionDispatcher.ResponseError;
            DoAction(actionGetter, response);
        }

        private static GameSession GetSession(System.Web.HttpContext context, RequestPackage package)
        {
            GameSession session;
            if (package.ProxySid != Guid.Empty)
            {
                session = GameSession.Get(package.ProxySid) ?? GameSession.CreateNew(package.ProxySid, context.Request);
                session.ProxySid = package.ProxySid;
            }
            else
            {
                session = GameSession.Get(package.SessionId) ?? GameSession.CreateNew(Guid.NewGuid(), context.Request);
            }
            return session;
        }
    }

    class HttpAsyncHandler : IHttpAsyncHandler
    {
        public static readonly IHttpAsyncHandler Default = new HttpAsyncHandler();

        public async Task<IHttpResponseAction> Execute(IHttpRequestContext context)
        {
            int statusCode;
            RequestPackage package;
            var actionDispatcher = GameEnvironment.Setting.ActionDispatcher;
            if (!actionDispatcher.TryDecodePackage(context.Request, out package, out statusCode))
            {
                return new ByteResponse(statusCode, "FAIL", new byte[0]);
            }

            GameSession session;
            if (package.ProxySid != Guid.Empty)
            {
                session = GameSession.Get(package.ProxySid) ?? GameSession.CreateNew(package.ProxySid, context.Request);
                session.ProxySid = package.ProxySid;
            }
            else
            {
                session = GameSession.Get(package.SessionId) ?? GameSession.CreateNew(Guid.NewGuid(), context.Request);
            }
            package.Bind(session);

            ActionGetter httpGet = actionDispatcher.GetActionGetter(package, session);
            if (package.IsUrlParam)
            {
                httpGet["UserHostAddress"] = session.RemoteAddress;
                httpGet["ssid"] = session.KeyCode.ToString("N");
                httpGet["http"] = "1";
            }

            var result = await System.Threading.Tasks.Task.Run<byte[]>(() =>
            {
                try
                {
                    return ScriptEngines.GetCurrentMainScript().ProcessRequest(package, httpGet);
                }
                catch (Exception)
                {
                    return new byte[0];
                }
            });

            return new ByteResponse(statusCode, "OK", result);
        }

    }
}

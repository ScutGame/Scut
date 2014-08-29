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
using System.Web;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GameHttpHost : GameBaseHost
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Request(HttpContext context)
        {
            var actionDispatcher = GameEnvironment.Setting.ActionDispatcher;
            RequestPackage package;
            if (!actionDispatcher.TryDecodePackage(context, out package))
            {
                return;
            }
            var session = GetSession(context, package);
            package.Bind(session);
            ActionGetter actionGetter = actionDispatcher.GetActionGetter(package);
            BaseGameResponse response = new HttpGameResponse(context.Response);
            response.WriteErrorCallback += actionDispatcher.ResponseError;
            DoAction(actionGetter, response);
        }

        private static GameSession GetSession(HttpContext context, RequestPackage package)
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
}

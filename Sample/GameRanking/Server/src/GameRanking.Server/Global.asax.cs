using System;
using System.Reflection;
using GameRanking.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Runtime;

namespace GameRanking.Server
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

            try
            {
                int cacheInterval = 600;
                Assembly entityAssembly = Assembly.Load("GameRanking.Model");
                GameEnvironment.Start(cacheInterval, () => { return true; }, 600, entityAssembly);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("App star error:{0}", ex);
            }

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            GameEnvironment.Stop();
        }
    }
}
using System;
using System.Reflection;
using GameRanking.Model;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Script;

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
                GameEnvironment.Start(cacheInterval, () => {
                    ScriptEngines.AddReferencedAssembly("GameRanking.Model.dll");
                    ActionFactory.SetActionIgnoreAuthorize(1000, 1001);
                    return true;
                
                }, 600, entityAssembly);
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
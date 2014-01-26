using System;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Script;

namespace GameWebServer
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                string str = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string mainClass = "MainClass";
                ScriptEngines.AddReferencedAssembly("ZyGames.Framework.Game.dll");
                ScriptEngines.RunMainClass(mainClass);
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

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.RPC.Wcf;

namespace ZyGames.Framework.Game.WebServer
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                WcfServiceClientManager.Current.InitConfig(path, OnSendTo);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("{0}", ex);
            }
        }

        private void OnSendTo(string param, byte[] buffer)
        {
            //主动下发消息
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
            TraceLog.WriteError("未处理异常");
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

    }
}
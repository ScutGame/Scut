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
using System.Reflection;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Net;
using ZyGames.Framework.Script;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.Service
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                var setting = new EnvironmentSetting();
                setting.ClientDesDeKey = "j6=9=1ac";
				setting.EntityAssembly = Assembly.Load("ZyGames.Tianjiexing.Model");
				ScriptEngines.AddReferencedAssembly(new string[] {
					"ZyGames.Tianjiexing.Lang.dll",
					"ZyGames.Tianjiexing.Model.dll",
					"ZyGames.Tianjiexing.Component.dll",
					"ZyGames.Tianjiexing.BLL.Combat.dll",
					"ZyGames.Tianjiexing.BLL.GM.dll",
					"ZyGames.Tianjiexing.BLL.dll"
				});
                GameEnvironment.Start(setting);
				
				SystemGlobal.Run();

#if(DEBUG)
                TraceLog.WriteError("系统正使用Debug版本");
#else
                TraceLog.ReleaseWrite("系统正使用Release版本");
#endif
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("global start error:{0}", ex);
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
            SystemGlobal.Stop();
            GameEnvironment.Stop();
        }
    }
}
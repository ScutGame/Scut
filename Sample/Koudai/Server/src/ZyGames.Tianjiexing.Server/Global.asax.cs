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
using ZyGames.Framework.Cache;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Script;
using ZyGames.Framework.Plugin.PythonScript;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.Service
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            BaseLog log = null;
            try
            {
                GameEnvironment.ClientDesDeKey = "j6=9=1ac";
                log = new BaseLog();
                int cacheInterval = 600;
                var assembly = Assembly.Load("ZyGames.Tianjiexing.Model");
                GameEnvironment.Start(cacheInterval, () =>
                {
                    SystemGlobal.Run();
                    log.SaveLog(PythonScriptManager.Current.PythonRootPath);
                    PythonContext pythonContext;
                    PythonScriptManager.Current.TryLoadPython(@"Lib/action.py", out pythonContext);
                    RouteItem routeItem;
                    PythonScriptManager.Current.TryGetAction(1008, out routeItem);
                    return true;
                }, 600, assembly);

                if (log != null)
                {
#if(DEBUG)
                    log.SaveLog(new Exception("系统正使用Debug版本"));
#else
                    log.SaveLog("系统正使用Release版本");
#endif
                }
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.SaveLog(ex);
                }
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
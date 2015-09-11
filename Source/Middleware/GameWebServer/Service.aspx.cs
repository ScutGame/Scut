using System;
using System.Web;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Script;

namespace GameWebServer
{
    public partial class Service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptEngines.RequestMainProgram(HttpContext.Current);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Service error:{0}", ex);
            }
        }

    }
}
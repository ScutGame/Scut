using System;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;

namespace GameWebServer
{
    public partial class Service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ActionFactory.RequestScript();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Service error:{0}", ex);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;

namespace ZyGames.Framework.Game.WebServer
{
    public partial class Service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Charset = "unicode";
                new HttpServiceRequest(HttpContext.Current).Request();
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }
    }
}
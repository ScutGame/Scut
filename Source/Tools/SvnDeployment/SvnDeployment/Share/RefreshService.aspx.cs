using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ZyGames.OA.BLL.Action;
using CitGame;
using ZyGames.OA.BLL.Common;

namespace ZyGames.OA.Share
{
    public partial class RefreshService : BasePage
    {
        public RefreshService()
            : base(false)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.ContentType = "application/Json";
                Response.Charset = "utf-8";

                string actionId = Convert.ToString(Request.Params["action"]);
                object[] args = new object[] { actionId, HttpContext.Current };
                BaseAction baseAction = BaseAction.CreateInstance(actionId, args);
                if (baseAction != null)
                {
                    baseAction.Proccess();
                }
            }
            catch (Exception ex)
            {
                Response.Write(new JsonObject().Add("state", false).Add("message", "出错:" + ex.Message).ToJson());
                BaseLog log = new BaseLog();
                log.SaveLog(ex);
            }
        }
    }
}

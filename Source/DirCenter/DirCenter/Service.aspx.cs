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
using System.Reflection;
using ZyGames.GameService.BaseService.LogService;
using ZyGames.DirCenter.Action;


namespace ZyGames.DirCenter
{
    public partial class Service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;//new HttpResponse(sw);
            response.Charset = "unicode";// "unicode";
            HttpGet httpGet = new HttpGet();
            String ActionID = string.Empty;
            if (httpGet.GetString("ActionID", ref ActionID))
            {
                try
                {
                    string actionName = string.Concat("Action", ActionID);
                    string sname = string.Concat("ZyGames.DirCenter.Action." + actionName);
                    object[] args = new object[1];
                    args[0] = response;
                   
                    BaseStruct obj = (BaseStruct) Activator.CreateInstance(Type.GetType(sname), new object[] { httpGet });
                    if (obj.ReadUrlElement() && obj.DoAction() && !obj.GetError())
                    {
                        obj.BuildPacket();
                        obj.WriteAction();
                    }
                    else
                    {
                        obj.WriteErrorAction();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    BaseLog oBaseLog = new BaseLog("DirCenterErrMain");
                    oBaseLog.SaveLog(ex);
                }
            }
        }
    }
}

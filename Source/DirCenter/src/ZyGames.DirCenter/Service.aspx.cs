using System;
using System.Web;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Service;


namespace ZyGames.DirCenter
{
    public partial class Service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Charset = "unicode";// "unicode";
            HttpGet httpGet = new HttpGet(HttpContext.Current.Request);
            IGameResponse gameResponse = new HttpGameResponse(response);
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
                    obj.DoInit();
                    if (obj.ReadUrlElement() && obj.DoAction() && !obj.GetError())
                    {
                        obj.BuildPacket();
                        obj.WriteAction(gameResponse);
                    }
                    else
                    {
                        obj.WriteErrorAction(gameResponse);
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

using System;
using System.Reflection;
using System.Web;
using ZyGames.OA.BLL.Action;
using ZyGames.OA.BLL.Common;
using CitGame;
using ZyGames.OA.BLL.Remote;

namespace ZyGames.OA.Share
{
    public partial class ActionService : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.ContentType = "application/Json";
                Response.Charset = "utf-8";

                if (Request["remote"] != null)
                {
                    string className = "";
                    string method = "";
                    string route = Request["remote"] ?? "";
                    int index = route.LastIndexOf(".");
                    if (index != -1)
                    {
                        className = route.Substring(0, index);
                        if (route.Length > index)
                        {
                            method = route.Substring(index + 1, route.Length - index - 1);
                        }
                    }
                    Type type = Type.GetType(string.Format("ZyGames.OA.BLL.Remote.{0}Remote,ZyGames.OA.BLL", className));
                    if (type != null)
                    {
                        object[] args = new object[] { HttpContext.Current };
                        var obj = (GameRemote)Activator.CreateInstance(type, args);
                        obj.Request(method);
                    }
                }
                else
                {
                    string actionId = Convert.ToString(Request.Params["action"]);
                    object[] args = new object[] { actionId, HttpContext.Current };
                    BaseAction baseAction = BaseAction.CreateInstance(actionId, args);
                    if (baseAction != null)
                    {
                        baseAction.Init();
                        baseAction.Proccess();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(new JsonObject().Add("state", false).Add("message", "出错:" + ex.Message + ex.StackTrace).ToJson());
                BaseLog log = new BaseLog();
                log.SaveLog(ex);
            }
        }
    }
}

using System;
using System.Web;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;

namespace GameWebServer
{
    public partial class Service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var actionDispatcher = GameEnvironment.Setting.ActionDispatcher;
                RequestPackage package;
                if (!actionDispatcher.TryDecodePackage(HttpContext.Current, out package))
                {
                    return;
                }
                ActionGetter actionGetter = actionDispatcher.GetActionGetter(package);
                BaseGameResponse response = new HttpGameResponse(HttpContext.Current.Response);
                ActionFactory.RequestScript(actionGetter, response, GetUser);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Service error:{0}", ex);
            }
        }

        private BaseUser GetUser(int userId)
        {
            return (BaseUser)CacheFactory.GetPersonalEntity("GameServer.Model.GameUser", userId.ToString(), userId);
        }
    }
}
using System;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;

namespace GameWebServer
{
    public partial class Service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ActionFactory.RequestScript(GetUser);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Service error:{0}", ex);
            }
        }

        private BaseUser GetUser(int userId)
        {
            return (BaseUser) CacheFactory.GetPersonalEntity("GameServer.Model.GameUser", userId.ToString(), userId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using ZyGames.DirCenter.CacheData;
using ZyGames.DirCenter.Model;
using ZyGames.GameService.BaseService.LogService;

namespace ZyGames.DirCenter
{
    /// <summary>
    /// 提供官网服务
    /// </summary>
    public partial class OfficialService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BaseLog log = new BaseLog("OfficialService");
            try
            {
                string result = "";
                int gameId;
                string gameParam = Request["gid"];
                if (!string.IsNullOrEmpty(gameParam) && int.TryParse(gameParam, out gameId))
                {
                    result = DoComboWrite(gameId);
                }
                else
                {
                    result = "FAILURE";
                    log.SaveLog("OfficialService param error.");
                }
                Response.Write(result);
            }
            catch (Exception ex)
            {
                Response.Write("FAILURE");
                log.SaveLog(ex);
            }
        }

        private string DoComboWrite(int gameId)
        {
            CacheServer cacheServer = new CacheServer();
            var serverList = cacheServer.GetOfficialServerList(gameId);
            List<ComboData> responseList = new List<ComboData>();
            int index = 0;
            foreach (var serverInfo in serverList)
            {
                ComboData item = new ComboData();
                item.id = serverInfo.ID;
                item.text = string.Format("{0}({1}服)", serverInfo.ServerName, serverInfo.ID);
                if (index == 0)
                {
                    item.selected = true;
                }
                responseList.Add(item);
                index++;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(responseList);
        }
    }
}

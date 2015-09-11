/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using ZyGames.DirCenter.CacheData;
using ZyGames.DirCenter.Model;
using ZyGames.Framework.Common.Log;

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
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
using System.ComponentModel;
using System.Web.Services;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.WebService;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Framework.Game.Model;

namespace ZyGames.Tianjiexing.Service.WebService
{
    /// <summary>
    /// OAService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://kd1.36you.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class OAService : System.Web.Services.WebService
    {

        [WebMethod]
        public GameNotice[] GetNotices(int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;
            try
            {
                List<GameNotice> list = new ShareCacheStruct<GameNotice>().FindAll();
                list.QuickSort((x, y) =>
                {
                    int result = y.IsTop.CompareTo(x.IsTop);
                    if (result == 0)
                    {
                        return y.CreateDate.CompareTo(x.CreateDate);
                    }
                    return result;
                });
                var tempList = list.GetPaging(pageIndex, pageSize, out recordCount);
                return tempList.ToArray();
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog("取公告出错:", ex);
                throw;
            }
        }

        [WebMethod]
        public void SendNotice(GameNotice notice, bool isRemove)
        {
            try
            {
                var cacheSet = new ShareCacheStruct<GameNotice>();
                GameNotice gameNotice = cacheSet.FindKey(notice.NoticeID);
                if (isRemove)
                {
                    if (gameNotice != null)
                    {
                        cacheSet.Delete(gameNotice);
                    }
                }
                else
                {
                    if (gameNotice == null)
                    {
                        gameNotice = new GameNotice();
                        gameNotice.NoticeID = Guid.NewGuid().ToString();
                        cacheSet.Add(gameNotice);
                        gameNotice = cacheSet.FindKey(gameNotice.NoticeID);
                    }
                    gameNotice.Title = notice.Title;
                    gameNotice.Content = notice.Content;
                    gameNotice.ExpiryDate = notice.ExpiryDate;
                    gameNotice.IsTop = notice.IsTop;
                    gameNotice.IsBroadcast = notice.IsBroadcast;
                    gameNotice.Creater = notice.Creater;
                    gameNotice.CreateDate = notice.CreateDate;
                    gameNotice.NoticeType = notice.NoticeType;
                    if (gameNotice.IsBroadcast)
                    {
                        var broadcastService = new TjxBroadcastService(null);
                        var msg = broadcastService.Create(NoticeType.System, gameNotice.Content);
                        broadcastService.Send(msg);
                    }

                }
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog("保存公告出错:", ex);
                throw;
            }
        }


        [WebMethod(Description = "获取数据接口")]
        public string GetData(string type, JsonParameter[] paramList)
        {
            try
            {
                string jsonString = string.Empty;

                object obj = new OATianjiexingService().GetData(type, new object[0], paramList);
                if (obj != null)
                {
                    jsonString = JsonUtils.Serialize(obj);
                }
                return jsonString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [WebMethod(Description = "获取处理接口")]
        public void DataProcess(string type, JsonParameter[] paramList)
        {
            try
            {
                new OATianjiexingService().Process(type, new object[0], paramList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
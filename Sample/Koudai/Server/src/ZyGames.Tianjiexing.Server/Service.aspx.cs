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
using System.Diagnostics;
using System.Web;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.Service
{
    public partial class Service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;//new HttpResponse(sw);
                response.Charset = "unicode";// "unicode";
                //Stopwatch a = new Stopwatch();
                //a.Start();
                HttpGet httpGet = new HttpGet(HttpContext.Current.Request);
                int ActionID = httpGet.ActionId;
                ActionFactory.Request("ZyGames.Tianjiexing.BLL.Action.Action{0},ZyGames.Tianjiexing.BLL", userId => new GameDataCacheSet<GameUser>().FindKey(userId.ToNotNullString()));

                //a.Stop();
                //TraceLog.WriteError("接口访问{0},{1}", ActionID, a.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Service error:{0}", ex);
            }
        }
    }
}
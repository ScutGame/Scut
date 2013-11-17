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
using System.Data;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Cache;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.WebService
{
    public class UserCacheProcesser : BaseDataProcesser
    {
        public override void Process(JsonParameter[] paramList)
        {
            JsonParameterList parameters = JsonParameter.Convert(paramList);
            if (parameters["op"].Length > 0)
            {
                string userID = parameters["UserId"];
                if (!string.IsNullOrEmpty(userID) && userID.ToUpper().StartsWith("Z"))
                {
                    string pid = userID;
                    List<GameUser> list = new List<GameUser>();
                    new GameDataCacheSet<GameUser>().Foreach((personalId, key, user) =>
                    {
                        if (user.Pid.ToUpper() == pid.ToUpper())
                        {
                            list.Add(user);
                        }
                        return true;
                    });
                    if (list.Count > 0)
                    {
                        userID = list[0].UserID;
                    }
                }
                if (!string.IsNullOrEmpty(userID) && parameters["op"].Equals("1"))
                {
                    UserCacheGlobal.ReLoad(userID);
                }
                else
                {
                    var cacheSet = new ConfigCacheSet<CareerInfo>();
                    if (parameters["op"].Equals("0"))
                    {
                        ConfigCacheGlobal.Load();
                        UserCacheGlobal.ReLoad(string.Empty);
                        SystemGlobal.LoadGlobalData();
                        GameConfigSet.Initialize();
                    }
                    else if (parameters["op"].Equals("2"))
                    {
                        //刷新data配置表
                        SystemGlobal.LoadGlobalData();
                        GameConfigSet.Initialize();
                    }
                    else if (parameters["op"].Equals("3"))
                    {
                        //刷新config配置表
                        ConfigCacheGlobal.Load();
                        GameConfigSet.Initialize();
                    }
                }
            }
        }
    }
}
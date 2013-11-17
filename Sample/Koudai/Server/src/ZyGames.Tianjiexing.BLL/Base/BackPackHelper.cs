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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class BackPackHelper
    {
        /// <summary>
        /// 初始化背包
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static void AddBack(GameUser user)
        {
            var cacheSet = new GameDataCacheSet<UserPack>();
            var cacheSetBackPack = new ConfigCacheSet<BackpackConfigInfo>();
            var userPack = cacheSet.FindKey(user.UserID);

            if (userPack == null)
            {
                userPack = new UserPack();

                userPack.UserID = user.UserID.ToInt();
                userPack.CreateDate = DateTime.Now;
                var backpackConfig = cacheSetBackPack.FindKey(BackpackType.ZhuangBei);
                if (backpackConfig != null)
                {
                    var packType = new PackType();
                    packType.BackpackType = BackpackType.ZhuangBei;
                    packType.OpenNum = 0;
                    packType.Position = backpackConfig.DefaultNum;
                }

                backpackConfig = cacheSetBackPack.FindKey(BackpackType.YongBing);
                if (backpackConfig != null)
                {
                    var packType = new PackType();
                    packType.BackpackType = BackpackType.YongBing;
                    packType.OpenNum = 0;
                    packType.Position = backpackConfig.DefaultNum;
                }

                backpackConfig = cacheSetBackPack.FindKey(BackpackType.HunJi);
                if (backpackConfig != null)
                {
                    var packType = new PackType();
                    packType.BackpackType = BackpackType.HunJi;
                    packType.OpenNum = 0;
                    packType.Position = backpackConfig.DefaultNum;
                }

                backpackConfig = cacheSetBackPack.FindKey(BackpackType.BeiBao);
                if (backpackConfig != null)
                {
                    var packType = new PackType();
                    packType.BackpackType = BackpackType.BeiBao;
                    packType.OpenNum = 0;
                    packType.Position = backpackConfig.DefaultNum;
                    user.GridNum = MathUtils.Addition(user.GridNum, backpackConfig.DefaultNum);
                }
            }

        }
    }
}
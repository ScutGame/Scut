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
using System.Linq;
using System.Web;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.Component
{
    public class UserAlbumHelper
    {
        public static GameDataCacheSet<UserAlbum> _cacheSetAlbum = new GameDataCacheSet<UserAlbum>();

        /// <summary>
        /// 添加玩家集邮卡牌
        /// </summary>
        /// <param name="userId">玩家 ID</param>
        /// <param name="albumType">1、佣兵 2、装备  3、魂技</param>
        /// <param name="albumID">对应的 ID</param>
        public static void AddUserAlbum(string userId, AlbumType albumType, int albumID)
        {
            var userAlbum = _cacheSetAlbum.FindKey(userId);
            if (userAlbum != null)
            {
                var album = userAlbum.AlbumList.Find(s => s.AlbumProperty == albumType && s.ID == albumID);
                if (album == null)
                {
                    album = new Album();
                    album.AlbumProperty = albumType;
                    album.ID = albumID;
                    userAlbum.AlbumList.Add(album);
                }
            }
        }
    }
}
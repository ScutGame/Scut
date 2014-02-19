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
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9102添加好友列表
    /// </summary>
    public class Action9102 : BaseAction
    {
        private int _pageIndex;
        private int _pageSize;
        private int _pageCount;
        private List<GameUser> _userFriendArray = new List<GameUser>();

        public Action9102(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9102, httpGet)
        {

        }
        public override void BuildPacket()
        {
            PushIntoStack(_pageCount);
            PushIntoStack(_userFriendArray.Count);
            foreach (GameUser friends in _userFriendArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(friends.UserID);
                dsItem.PushIntoStack(friends.NickName.ToNotNullString());
                dsItem.PushIntoStack(friends.UserLv);
                //获取该好友的关系
                UserFriends userInfo = new ShareCacheStruct<UserFriends>().FindKey(ContextUser.UserID, friends.UserID);
                dsItem.PushIntoStack(userInfo == null ? FriendType.Fans.ToShort() : userInfo.FriendType.ToShort());
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref _pageIndex)
               && httpGet.GetInt("PageSize", ref _pageSize))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            List<GameUser> friendArray = new List<GameUser>();
            new GameDataCacheSet<GameUser>().Foreach((personId, key, user) =>
            {
                //查找相同等级的 并且好友类型非好友和关注的类型 
                if (user.UserLv == ContextUser.UserLv && user.UserID != ContextUser.UserID)
                {
                    UserFriends userInfo = new ShareCacheStruct<UserFriends>().FindKey(ContextUser.UserID, user.UserID);
                    if (userInfo == null)
                    {
                        friendArray.Add(user);
                    }
                    else
                    {
                        if (userInfo.FriendType != FriendType.Friend && userInfo.FriendType != FriendType.Attention)
                        {
                            friendArray.Add(user);
                        }
                    }

                }
                return true;
            });

            _userFriendArray = friendArray.GetPaging(_pageIndex, _pageSize, out _pageCount);
            return true;
        }
    }
}
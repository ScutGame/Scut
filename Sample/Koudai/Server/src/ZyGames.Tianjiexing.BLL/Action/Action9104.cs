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
using System.Data;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9104_删除好友接口
    /// </summary>
    public class Action9104 : BaseAction
    {
        private string friendID = string.Empty;


        public Action9104(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9104, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("FriendID", ref friendID))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            var cacheSet = new ShareCacheStruct<UserFriends>();
            UserFriends userFriend = cacheSet.FindKey(ContextUser.UserID, friendID);
            UserFriends userFriend1 = cacheSet.FindKey(friendID, ContextUser.UserID);
            //如果原来是好友 要将对方的状态改为关注 其他的直接删除
            if (userFriend.FriendType == FriendType.Friend)
            {
                cacheSet.Delete(userFriend);
                userFriend1.FriendType = FriendType.Attention;
            }else
            {
                cacheSet.Delete(userFriend);
            }
           
            return true;
        }
    }
}
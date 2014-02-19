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
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9101_好友列表接口
    /// </summary>
    public class Action9101 : BaseAction
    {
        private FriendType _friendType;
        private int _pageIndex;
        private int _pageSize;
        private int _pageCount;

        private List<UserFriends> _userFriendList = new List<UserFriends>();

        public Action9101(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9101, httpGet)
        {
           
        }

        public override void BuildPacket()
        {
            PushIntoStack(_pageCount);
            PushIntoStack(_userFriendList.Count);
            foreach (UserFriends friends in _userFriendList)
            {
                GameUser gameUser = UserCacheGlobal.LoadOffline(friends.FriendID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(friends.FriendID.ToNotNullString());
                dsItem.PushIntoStack(gameUser == null ? string.Empty : gameUser.NickName.ToNotNullString());
                dsItem.PushIntoStack(gameUser == null ? LanguageManager.GetLang().shortInt : gameUser.UserLv);
                dsItem.PushIntoStack((short)friends.FriendType);
                dsItem.PushIntoStack(gameUser == null ? string.Empty : gameUser.UserID.ToNotNullString());
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("FriendType", ref _friendType)
                 && httpGet.GetInt("PageIndex", ref _pageIndex)
                 && httpGet.GetInt("PageSize", ref _pageSize))
            {

                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {

            List<UserFriends> userFriendsesArray = new ShareCacheStruct<UserFriends>().FindAll(m => m.UserID == ContextUser.UserID && m.FriendType == _friendType);
            userFriendsesArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                //普通的玩家根据等级来排序 如果是仇敌的话需要用最后战斗时间来排序
                int result = 0;
                if (_friendType != FriendType.ChouDi)
                {
                    GameUser userx = new GameDataCacheSet<GameUser>().FindKey(x.UserID);
                    GameUser usery = new GameDataCacheSet<GameUser>().FindKey(y.UserID);
                    int userLvx = userx == null ? 0 : userx.UserLv;
                    int userLvy = usery == null ? 0 : usery.UserLv;
                    int currExperiencex = (UserGeneral.GetMainGeneral(x.UserID) ?? new UserGeneral()).CurrExperience;
                    int currExperiencey = (UserGeneral.GetMainGeneral(y.UserID) ?? new UserGeneral()).CurrExperience;
                    result = userLvy.CompareTo(userLvx);
                    if (result == 0)
                    {
                        result = currExperiencey.CompareTo(currExperiencex);
                    }

                }
                else
                {
                    result = x.FightTime.CompareTo(y.FightTime);
                    if (result == 0)
                    {
                        result = x.UserID.CompareTo(y.UserID);
                    }
                }
                return result;
            });
            _userFriendList = userFriendsesArray.GetPaging(_pageIndex, _pageSize, out _pageCount);
            return true;
        }
    }
}
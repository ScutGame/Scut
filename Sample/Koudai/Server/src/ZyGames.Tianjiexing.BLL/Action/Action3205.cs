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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3205_拦截界面接口
    /// </summary>
    public class Action3205 : BaseAction
    {
        private int _petId;
        private int _pageIndex;
        private int _pageSize;
        private int _pageCount;
        private List<PetRunPool> _petRunPools = new List<PetRunPool>();


        public Action3205(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3205, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_pageCount);
            PushIntoStack(_petRunPools.Count);
            foreach (var item in _petRunPools)
            {
                var user = UserCacheGlobal.LoadOffline(item.UserID) ?? new GameUser();
                string guidName = string.Empty;
                if (!string.IsNullOrEmpty(user.MercenariesID))
                {
                    guidName = (new ShareCacheStruct<UserGuild>().FindKey(user.MercenariesID) ?? new UserGuild()).GuildName;
                }
                string friendName = string.Empty;
                if (!string.IsNullOrEmpty(item.FriendID))
                {
                    friendName = (UserCacheGlobal.LoadOffline(item.FriendID) ?? new GameUser()).NickName;
                }

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.UserID.ToNotNullString());
                dsItem.PushIntoStack(user.NickName.ToNotNullString());
                dsItem.PushIntoStack(user.UserLv);
                dsItem.PushIntoStack(guidName.ToNotNullString());
                dsItem.PushIntoStack(item.ColdTime);
                dsItem.PushIntoStack(item.PetID);
                dsItem.PushIntoStack(friendName.ToNotNullString());
                dsItem.PushIntoStack(item.InterceptGameCoin);
                dsItem.PushIntoStack(item.InterceptObtainNum);

                this.PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PetId", ref _petId)
                 && httpGet.GetInt("PageIndex", ref _pageIndex)
                 && httpGet.GetInt("PageSize", ref _pageSize))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var tempList = new ShareCacheStruct<PetRunPool>().FindAll(m => m.PetID == _petId && m.PetID > 0 && m.UserID != ContextUser.UserID);
            var petList = new List<PetRunPool>();
            foreach (var petRunPool in tempList)
            {
                if (petRunPool.PetID > 0 && petRunPool.ColdTime == 0)
                {
                    UserHelper.ProcessPetPrize(petRunPool);
                    continue;
                }
                petList.Add(petRunPool);
            }
            _petRunPools = petList.GetPaging(_pageIndex, _pageSize, out _pageCount);
            return true;
        }
    }
}
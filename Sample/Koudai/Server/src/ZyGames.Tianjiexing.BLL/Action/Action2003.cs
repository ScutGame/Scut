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
    /// 2003_城市玩家刷新接口
    /// </summary>
    public class Action2003 : BaseAction
    {
        private const int totalDaySeconds = 86400;
        private int cityID = 0;
        private int _pageSize;

        private List<GameUser> gameUserArray = new List<GameUser>();
        private CityInfo cityInfo = null;

        public Action2003(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action2003, httpGet)
        {

        }

        public override bool TakeAction()
        {
            cityInfo = new ConfigCacheSet<CityInfo>().FindKey(cityID);
            if (ContextUser.UserLocation == Location.Guid)
            {
                new GameDataCacheSet<GameUser>().Foreach((personalId, key, m) =>
                {
                    if (m.UserID != Uid && m.MercenariesID == ContextUser.MercenariesID && m.UserLocation == Location.Guid && m.DayTime < totalDaySeconds && m.CityID == cityID)
                    {
                        gameUserArray.Add(m);
                    }
                    return true;
                });
            }
            else if (ContextUser.UserLocation == Location.GuildExercise)
            {
                gameUserArray = GetGuildExercise();
            }
            else
            {
                new GameDataCacheSet<GameUser>().Foreach((personalId, key, m) =>
                {
                    if (m.UserID != Uid && m.DayTime < totalDaySeconds && m.CityID == cityID)
                    {
                        gameUserArray.Add(m);
                    }
                    return true;
                });
            }
            int recordCount;
            gameUserArray = gameUserArray.GetPaging(0, _pageSize, out recordCount);
            return true;
        }

        private List<GameUser> GetGuildExercise()
        {
            List<GameUser> userList = new List<GameUser>();
            if (!string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
                if (guild != null)
                {
                    if (guild.GuildExercise != null)
                    {
                        if (guild.GuildExercise.UserList != null)
                        {
                            foreach (var item in guild.GuildExercise.UserList)
                            {
                                if (item.UserID != Uid)
                                {
                                    GameUser user = new GameDataCacheSet<GameUser>().FindKey(item.UserID);
                                    if (user != null)
                                        userList.Add(user);
                                }
                            }
                        }
                    }
                }
            }
            return userList;

        }

        public override void BuildPacket()
        {
            PushIntoStack(cityInfo == null ? string.Empty : cityInfo.CityName.ToNotNullString());
            PushIntoStack(gameUserArray.Count);
            foreach (GameUser user in gameUserArray)
            {
                UserGeneral uGeneral = UserGeneral.GetMainGeneral(user.UserID);
                string HeadID = string.Empty;
                if (uGeneral != null)
                {
                    CareerInfo careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(uGeneral.CareerID);
                    HeadID = user.Sex ? careerInfo.HeadID2 : careerInfo.HeadID;
                }
                UserGuild userGuild = new ShareCacheStruct<UserGuild>().FindKey(user.MercenariesID);
                string pictureID = string.Empty;

                //原因：排除月饼和双倍材料卡  
                List<UserProps> propsArray = new GameDataCacheSet<UserProps>().FindAll(user.UserID, u => u.PropType == 3 && u.ItemID != 5200 && u.ItemID != 7003);
                if (propsArray.Count > 0 && propsArray[0].DoRefresh() > 0)
                {
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(propsArray[0].ItemID);
                    if (itemInfo != null)
                    {
                        pictureID = itemInfo.PictrueID;
                    }
                }
                else
                {
                    pictureID = string.Empty;
                }

                DataStruct ds = new DataStruct();
                ds.PushIntoStack(user.UserID.ToNotNullString());
                ds.PushIntoStack(user.NickName.ToNotNullString());
                ds.PushIntoStack(HeadID.ToNotNullString());
                ds.PushIntoStack(user.MercenariesID.ToNotNullString());
                ds.PushIntoStack(userGuild == null ? string.Empty : userGuild.GuildName.ToNotNullString());
                ds.PushIntoStack((short)user.UserStatus);
                ds.PushIntoStack(user.PointX);
                ds.PushIntoStack(user.PointY);
                ds.PushIntoStack(pictureID.ToNotNullString());
                ds.PushIntoStack(NoviceHelper.IsWingFestivalInfo(user.UserID) ? (short)1 : (short)0);
                ds.PushIntoStack(0);
                PushIntoStack(ds);
            }

        }

        public override bool GetUrlElement()
        {
            int maxSize = ConfigEnvSet.GetInt("City.ShowMaxNum");
            if (httpGet.GetInt("CityID", ref cityID)
                   && httpGet.GetInt("PageSize", ref _pageSize, 0, maxSize))
            {
                if (_pageSize == 0) _pageSize = maxSize;
                return true;
            }
            return false;
        }
    }
}
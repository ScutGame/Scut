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
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Common;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 2001_世界图片加载接口
    /// </summary>
    public class Action2001 : BaseAction
    {

        private GameUser _gameUser = null;
        private List<CityInfo> _cityInfoArray = new List<CityInfo>();
        public short CityStatus = 1;
        private FightCombat combat = new FightCombat();

        public Action2001(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action2001, httpGet)
        {

        }


        public override bool TakeAction()
        {
            _gameUser = new GameDataCacheSet<GameUser>().FindKey(ContextUser.UserID);
            _cityInfoArray = new ConfigCacheSet<CityInfo>().FindAll(c => c.CityType == 0 &&
                c.MinLv <= _gameUser.UserLv &&
                ((int)c.CountryID == 0 || c.CountryID == ContextUser.CountryID));
            return true;
        }

        public override void BuildPacket()
        {
            PushIntoStack(_gameUser.CityID);
            PushIntoStack(_cityInfoArray.Count);
            foreach (CityInfo info in _cityInfoArray)
            {
                string guildID = GuildFightCombat.FightChampion(info.CityID);
                string guildBanner = GetGuildBanner(guildID);
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(info.PointX);
                ds.PushIntoStack(info.PointY);
                ds.PushIntoStack(info.CityID);
                ds.PushIntoStack(info.CityName);
                ds.PushIntoStack(info.HeadID);
                ds.PushIntoStack(info.MinLv);
                ds.PushIntoStack(info.MaxLv);
                ds.PushIntoStack(CityStatus);
                ds.PushIntoStack(guildID.ToNotNullString());
                ds.PushIntoStack(guildBanner.ToNotNullString());
                PushIntoStack(ds);
            }
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public string GetGuildBanner(string guildID)
        {
            string guildBanner = string.Empty;
            UserGuild userGuild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (userGuild != null)
            {
                guildBanner = userGuild.GuildName.Substring(0, 1);
            }
            int fastID = MathUtils.Subtraction(combat.FastID, 1, 1);
            var fight = new ShareCacheStruct<ServerFight>().FindKey(fastID, guildID);
            if (fight != null && !string.IsNullOrEmpty(fight.GuildBanner) && !fight.IsRemove)
            {
                guildBanner = fight.GuildBanner;
            }
            return guildBanner;
        }
    }
}
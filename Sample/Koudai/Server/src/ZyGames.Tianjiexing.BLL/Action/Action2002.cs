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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 2002_城市地图加载接口
    /// </summary>
    public class Action2002 : BaseAction
    {
        private int cityID = 0;

        public Action2002(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action2002, httpGet)
        {

        }

        public override bool TakeAction()
        {
            CityInfo cityInfo = new ConfigCacheSet<CityInfo>().FindKey(cityID);
            if (cityInfo != null && cityInfo.CityType == 0)
            {
                ContextUser.UserLocation = Location.City;
                ContextUser.CityID = cityID;
            }
            else if (cityInfo != null && cityInfo.CityType == 1)
            {
                ContextUser.UserLocation = Location.Guid;
            }
            GuildFightCombat.SantoVisit(cityID, ContextUser);
            return true;
        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("CityID", ref cityID))
            {
                return true;
            }
            return false;
        }
    }
}
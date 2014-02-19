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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 2005_城市地图配置下发接口
    /// </summary>
    public class Action2005 : BaseAction
    {
        private int ClientVersion;
        private List<CityInfo> cityList = new List<CityInfo>();

        public Action2005(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action2005, httpGet)
        {
        }

        public override void BuildPacket()
        {
            this.PushIntoStack(cityList.Count);
            foreach (CityInfo cityInfo in cityList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(cityInfo.CityID);
                dsItem.PushIntoStack(cityInfo.CityName.ToNotNullString());
                dsItem.PushIntoStack(cityInfo.PointX);
                dsItem.PushIntoStack(cityInfo.PointY);
                dsItem.PushIntoStack(cityInfo.GoPointY1);
                dsItem.PushIntoStack(cityInfo.GoPointY2);
                dsItem.PushIntoStack(cityInfo.MinLv);
                dsItem.PushIntoStack(cityInfo.MaxLv);
                dsItem.PushIntoStack(cityInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(cityInfo.BgScence.ToNotNullString());
                dsItem.PushIntoStack(cityInfo.FgScence.ToNotNullString());
                dsItem.PushIntoStack(IsExistElite(cityInfo.CityID));

                var cityNpcList = new ConfigCacheSet<CityNpcInfo>().FindAll(m => m.CityID == cityInfo.CityID);
                dsItem.PushIntoStack(cityNpcList.Count);
                foreach (CityNpcInfo cityNpc in cityNpcList)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(cityNpc.NpcID);
                    dsItem1.PushIntoStack(cityNpc.NpcName.ToNotNullString());
                    dsItem1.PushIntoStack(cityNpc.HeadID.ToNotNullString());
                    dsItem1.PushIntoStack(cityNpc.HeadID2.ToNotNullString());
                    dsItem1.PushIntoStack(cityNpc.PointX);
                    dsItem1.PushIntoStack(cityNpc.PointY);
                    dsItem1.PushIntoStack(cityNpc.CommandID);
                    dsItem1.PushIntoStack(cityNpc.NpcDesc.ToNotNullString());

                    dsItem.PushIntoStack(dsItem1);
                }
                this.PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ClientVersion", ref ClientVersion))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            ConfigVersion version = new ConfigCacheSet<ConfigVersion>().FindKey(VersionType.City);
            if (version != null)
            {
                //int currVersion = new ConfigCacheSet<ConfigVersion>().FindKey(VersionType.City).CurVersion;
                int currVersion = version.CurVersion;
                cityList = new ConfigCacheSet<CityInfo>().FindAll(m => m.Version > ClientVersion && m.Version <= currVersion);
            }
            return true;
        }

        private short IsExistElite(int cityId)
        {
            short isPlotType = 0;
            List<PlotInfo> plotInfoList = new ConfigCacheSet<PlotInfo>().FindAll(m => m.CityID == cityId && m.PlotType == PlotType.HeroPlot);
            if (plotInfoList.Count > 0)
            {
                isPlotType = 2;
            }
            else
            {
                plotInfoList = new ConfigCacheSet<PlotInfo>().FindAll(m => m.CityID == cityId && m.PlotType == PlotType.Elite);
                if (plotInfoList.Count > 0)
                {
                    isPlotType = 1;
                }
            }
            return isPlotType;
        }
    }
}
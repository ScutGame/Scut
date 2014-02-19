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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 4001_副本列表接口
    /// </summary>
    public class Action4001 : BaseAction
    {
        private int cityID;
        private PlotType plotType;
        private int resetNum = 0;
        private List<CityInfo> _cityInfoList = new List<CityInfo>();
        private int _backpackType = 0;
        private ConfigCacheSet<PlotInfo> _cacheSetPlot = new ConfigCacheSet<PlotInfo>();
        public Action4001(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4001, httpGet)
        {

        }

        public override void BuildPacket()
        {
            var cacheSetItem = new ConfigCacheSet<ItemBaseInfo>();
            PushIntoStack(_cityInfoList.Count);
            foreach (var city in _cityInfoList)
            {

                List<PlotInfo> plotList = _cacheSetPlot.FindAll(s => s.CityID == city.CityID && s.PlotType == plotType);

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(plotList.Count);
                foreach (PlotInfo item in plotList)
                {
                    UserPlotInfo userPlot = UserPlotHelper.GetUserPlotInfo(ContextUser.UserID, item.PlotID);
                    int isKill = PlotHelper.IsKill(ContextUser.UserID, item.PlotID, cityID) ? 1 : 0;
                    DataStruct dsItemPlot = new DataStruct();
                    dsItemPlot.PushIntoStack(item.PlotID);
                    dsItemPlot.PushIntoStack(item.PlotSeqNo.ToShort());
                    dsItemPlot.PushIntoStack(item.PlotName.ToNotNullString());
                    dsItemPlot.PushIntoStack(item.BossHeadID.ToNotNullString());
                    dsItemPlot.PushIntoStack(GetPlotStatus(userPlot, plotType));
                    dsItemPlot.PushIntoStack(userPlot == null ? 1 : (int)userPlot.StarScore);
                    dsItemPlot.PushIntoStack(isKill);
                    dsItemPlot.PushIntoStack(item.HonourNum);
                    dsItemPlot.PushIntoStack(item.GameCoin);
                    dsItemPlot.PushIntoStack(item.PlotDesc);
                    dsItemPlot.PushIntoStack(item.PlotLv);
                    dsItemPlot.PushIntoStack(ConfigEnvSet.GetInt("Plot.BattleEnergyNum"));
                    dsItemPlot.PushIntoStack(PlotHelper.GetPlotChallengeNum(UserId.ToString(), item.PlotID));
                    dsItemPlot.PushIntoStack(item.ChallengeNum);
                    dsItemPlot.PushIntoStack(userPlot == null ? 0 : userPlot.PlotNum);
                    dsItem.PushIntoStack(dsItemPlot);
                    var itemArray = item.ItemRank.Split(',');
                    dsItemPlot.PushIntoStack(itemArray.Length);
                    foreach (var s in itemArray)
                    {
                        var sArray = s.Split('=');
                        var itemInfo = cacheSetItem.FindKey(sArray.Length > 0 ? sArray[0].ToInt() : 0);
                        DataStruct dsItemInfo = new DataStruct();
                        dsItemInfo.PushIntoStack(itemInfo != null ? itemInfo.ItemName : string.Empty);
                        dsItemInfo.PushIntoStack(sArray.Length > 1 ? sArray[1].ToInt() : 0);
                        dsItemPlot.PushIntoStack(dsItemInfo);
                    }
                }

                dsItem.PushIntoStack(city.CityID);
                dsItem.PushIntoStack(city.CityName.ToNotNullString());

                PushIntoStack(dsItem);
            }
            PushIntoStack(resetNum);
            PushIntoStack(_backpackType);
        }

        private short GetPlotStatus(UserPlotInfo userPlot, PlotType plotType)
        {
            if (userPlot != null)
            {
                if (plotType == PlotType.Normal)
                {
                    var plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(userPlot.PlotID);
                    if (plotInfo.PrePlotID > 0)
                    {
                        var preUserPlot = UserPlotHelper.GetUserPlotInfo(ContextUser.UserID, plotInfo.PrePlotID);
                        if (preUserPlot == null || preUserPlot.PlotStatus != PlotStatus.Completed)
                        {
                            return (short)PlotStatus.Locked;
                        }
                    }
                }
                else
                {
                    if (plotType == PlotType.Elite)
                    {
                        var plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(userPlot.PlotID);
                        if (plotInfo.PrePlotID > 0 && plotInfo.JYPrePlotID > 0)
                        {
                            var preUserPlot = UserPlotHelper.GetUserPlotInfo(ContextUser.UserID, plotInfo.JYPrePlotID);
                            if (preUserPlot == null || preUserPlot.PlotStatus != PlotStatus.Completed)
                            {
                                return (short)PlotStatus.Locked;
                            }
                        }
                    }
                }
                return (short)userPlot.PlotStatus;
            }
            return (short)PlotStatus.Locked;
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum<PlotType>("PlotType", ref plotType))
            {
                return true;
            }
            return false;

        }
        private void PlotLsit()
        {
            var cacheSet = new GameDataCacheSet<UserPlotPackage>();
            var userPlotPack = cacheSet.FindKey(ContextUser.UserID);
            var plotPackList = userPlotPack != null ? userPlotPack.PlotPackage : null;


            var cityInfoList = new ConfigCacheSet<CityInfo>().FindAll();
            foreach (var cityInfo in cityInfoList)
            {
                var plotInfoList = _cacheSetPlot.FindAll(s => s.CityID == cityInfo.CityID && s.PlotType == plotType);

                foreach (var plotInfo in plotInfoList)
                {
                    var plotPack = plotPackList != null ? plotPackList.Find(s => s.PlotID == plotInfo.PlotID) : null;
                    if (plotPack != null)
                    {
                        if (_cityInfoList.Find(s => s.CityID == cityInfo.CityID) == null)
                        {
                            _cityInfoList.Add(cityInfo);
                        }
                        if (plotPack.RefleshDate.Date != DateTime.Now.Date)
                        {
                            plotPack.UpdateNotify(obj =>
                            {
                                plotPack.RefleshDate = DateTime.Now;
                                plotPack.PlotNum = 0;
                                return true;
                            });
                        }
                        //break;
                    }
                }
            }
        }
        public override bool TakeAction()
        {
            if(PlotType.Elite == plotType)
            {
                PlotHelper.EnablePlot(Uid, ConfigEnvSet.GetInt("UserPlot.OpenLockJYPlotID"));
            }

            if (plotType == PlotType.Elite && ContextUser.UserLv >= ConfigEnvSet.GetInt("User.JYLv"))
            {
                PlotLsit();
            }
            else
            {
                if (plotType == PlotType.Normal)
                {
                    PlotLsit();
                }
            }
            //var cacheSetUserAbility = new GameDataCacheSet<UserAbility>();
            //var cacheSetItemPackage = new GameDataCacheSet<UserItemPackage>();
            //var cacheSetUserGeneral = new GameDataCacheSet<UserGeneral>();
            //var cacheSetUserPack = new GameDataCacheSet<UserPack>();
            //var userPack = cacheSetUserPack.FindKey(UserId.ToString());
            //if (userPack != null)
            //{
            //    var userItemPack = cacheSetItemPackage.FindKey(UserId.ToString());
            //    userPack.PackTypeList.Foreach(PackTyp =>
            //    {
            //        switch (PackTyp.BackpackType)
            //        {
            //            case BackpackType.BeiBao:

            //                if (userItemPack != null)
            //                {
            //                    int itemNum = userItemPack.ItemPackage.FindAll(s => s.ItemType == ItemType.ZhuangBei).Count;
            //                    _backpackType = itemNum >= PackTyp.Position ? BackpackType.BeiBao.ToInt() : 0;
            //                }
            //                break;
            //            case BackpackType.ZhuangBei:
            //                if (userItemPack != null)
            //                {
            //                    int packNum = userItemPack.ItemPackage.FindAll(s => s.ItemType != ItemType.ZhuangBei).Count;
            //                    _backpackType = packNum >= PackTyp.Position ? BackpackType.ZhuangBei.ToInt() : 0;
            //                }
            //                break;
            //            case BackpackType.HunJi:
            //                var userAbility = cacheSetUserAbility.FindKey(Uid);
            //                if (userAbility != null)
            //                {
            //                    int abilityNum = userAbility.AbilityList.Count;
            //                    _backpackType = abilityNum >= PackTyp.Position ? BackpackType.HunJi.ToInt() : 0;
            //                }
            //                break;
            //            case BackpackType.YongBing:
            //                int generalNum = cacheSetUserGeneral.FindAll(Uid).Count;


            //                _backpackType = generalNum >= PackTyp.Position ? BackpackType.YongBing.ToInt() : 0;

            //                break;
            //        }
            //        return true;
            //    });
            //}
            return true;
        }


    }
}
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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 4301_天地劫副本接口
    /// </summary>
    public class Action4301 : BaseAction
    {
        private string npcName;
        private List<PlotInfo> plotInfoArray = new List<PlotInfo>();
        private PlotInfo plotInfo = null;
        private List<PlotNPCInfo> plotNpcInfoArray = new List<PlotNPCInfo>();
        private const int itemNum = 1;
        private int hurdleNum = 0;
        private bool _isOverCombat;
        private string[] strEnchant = new string[0];
        private UniversalInfo[] universalArray = new UniversalInfo[0];

        public Action4301(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4301, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(plotInfoArray.Count);
            foreach (var item in plotInfoArray)
            {
                var userPlot = UserPlotHelper.GetUserPlotInfo(ContextUser.UserID, item.PlotID); //new GameDataCacheSet<UserPlot>().FindKey(ContextUser.UserID, item.PlotID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.PlotID);
                dsItem.PushIntoStack(item.PlotSeqNo);
                dsItem.PushIntoStack(item.PlotName.ToNotNullString());
                dsItem.PushIntoStack(item.BossHeadID.ToNotNullString());
                dsItem.PushIntoStack(UserPlotHelper.GetPlotStatus(ContextUser.UserID, userPlot));
                dsItem.PushIntoStack(userPlot == null ? 0 : (int)userPlot.StarScore);
                dsItem.PushIntoStack(UserHelper.IsKill(ContextUser, item.PlotID) ? 1 : 0);
                PushIntoStack(dsItem);
            }
            PushIntoStack(plotInfo == null ? string.Empty : plotInfo.PlotName.ToNotNullString());
            PushIntoStack(hurdleNum);
            PushIntoStack(npcName.ToNotNullString());
            PushIntoStack(RefreshKapla(ContextUser.UserID, 1));
            PushIntoStack(plotInfo == null ? 0 : plotInfo.Experience);
            PushIntoStack(plotNpcInfoArray.Count);
            foreach (PlotNPCInfo npcInfo in plotNpcInfoArray)
            {
                SparePartInfo partInfo = new ConfigCacheSet<SparePartInfo>().FindKey(npcInfo.SparePartID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(partInfo == null ? 0 : partInfo.Id);
                dsItem.PushIntoStack(partInfo == null ? string.Empty : partInfo.Name.ToNotNullString());
                dsItem.PushIntoStack(itemNum);
                PushIntoStack(dsItem);
            }
            PushIntoStack(_isOverCombat ? (short)1 : (short)0);
            PushIntoStack(RefreshKapla(ContextUser.UserID, 2));
            PushIntoStack(universalArray.Length);
            foreach (var item in universalArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.Name.ToNotNullString());
                dsItem.PushIntoStack(item.Num);
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (true)
            {
                return true;
            }
        }

        public override bool TakeAction()
        {
            List<UniversalInfo> universalList = new List<UniversalInfo>();
            int layerNum = CheckUserPlotLayerNum(ContextUser);
            hurdleNum = CheckUserPlotHurdleNum(ContextUser);
            plotInfoArray = new ConfigCacheSet<PlotInfo>().FindAll(m => m.LayerNum == layerNum && m.PlotType == PlotType.Kalpa);
            if (plotInfoArray.Count > 0)
            {
                plotInfo = new List<PlotInfo>(plotInfoArray).Find(u => u.PlotSeqNo == hurdleNum);
                if (plotInfo != null)
                {
                    plotNpcInfoArray = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == plotInfo.PlotID);
                    foreach (var info in plotNpcInfoArray)
                    {
                        SparePartInfo partInfo = new ConfigCacheSet<SparePartInfo>().FindKey(info.SparePartID);
                        if (partInfo != null)
                        {
                            universalList.Add(new UniversalInfo() { Name = partInfo.Name, HeadID = partInfo.HeadID, Num = 1 });
                        }
                    }
                    npcName = plotNpcInfoArray.Count == 0 ? string.Empty : plotNpcInfoArray[0].NpcName;
                    var userPlot = UserPlotHelper.GetUserPlotInfo(ContextUser.UserID, plotInfo.PlotID);
                    //todo new GameDataCacheSet<UserPlot>().FindKey(Uid, plotInfo.PlotID);
                    _isOverCombat = userPlot != null && userPlot.PlotStatus == PlotStatus.Completed;
                    strEnchant = plotInfo.EnchantID.Split(',');
                    if (strEnchant.Length > 0)
                    {
                        universalList.Add(new UniversalInfo() { Name = LanguageManager.GetLang().St4301_RandomEnchant, HeadID = string.Empty, Num = 1 });
                    }
                    universalArray = universalList.ToArray();
                }
            }

            return true;
        }

        /// <summary>
        /// 是否可刷新 1:刷新本层 2：刷新上一层
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="retype"></param>
        /// <returns></returns>
        private static short RefreshKapla(string userId, int retype)
        {
            short refreshNum = 0;
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userId);
            if (dailyRestrain != null && dailyRestrain.UserExtend != null && dailyRestrain.UserExtend.KalpaDate.Date == DateTime.Now.Date)
            {
                if (retype == 1)
                {
                    if (dailyRestrain.UserExtend.KalpaNum == 0) { refreshNum = 1; }
                    else if (dailyRestrain.UserExtend.KalpaNum > 0 && dailyRestrain.UserExtend.KalpaNum < 2) { refreshNum = 2; }
                    else { refreshNum = 0; }

                }
                else if (retype == 2)
                {
                    if (dailyRestrain.UserExtend.LastKalpaNum > 0) { refreshNum = 0; }
                    else { refreshNum = 1; }
                }
            }
            else
            {
                refreshNum = 1;
            }
            return refreshNum;
        }

        /// <summary>
        /// 当前玩家所在的层、关的副本
        /// </summary>
        /// <returns></returns>
        public static PlotInfo CheckUserPlotKalpa(GameUser user)
        {
            PlotInfo plotInfo = null;

            var userPlotArray = UserPlotHelper.UserPlotFindAll(user.UserID).FindAll(m => m.PlotType == PlotType.Kalpa);
            if (userPlotArray.Count == 0)
            {
                PlotHelper.EnablePlot(user.UserID, 8000);
                UserPlotHelper.UserPlotFindAll(user.UserID).FindAll(m => m.PlotType == PlotType.Kalpa);
            }
            if (userPlotArray.Count > 0)
            {
                userPlotArray.QuickSort((x, y) =>
                                                        {
                                                            int result = 0;
                                                            if (x == null && y == null) return 0;
                                                            if (x != null && y == null) return 1;
                                                            if (x == null) return -1;
                                                            result = new ConfigCacheSet<PlotInfo>().FindKey(y.PlotID).LayerNum.CompareTo(
                                                                new ConfigCacheSet<PlotInfo>().FindKey(x.PlotID).LayerNum);
                                                            if (result == 0)
                                                            {
                                                                result = new ConfigCacheSet<PlotInfo>().FindKey(y.PlotID).PlotSeqNo.CompareTo(
                                                                    new ConfigCacheSet<PlotInfo>().FindKey(x.PlotID).PlotSeqNo);
                                                            }
                                                            return result;
                                                        });
                plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(userPlotArray[0].PlotID);
            }
            return plotInfo;
        }

        /// <summary>
        /// 天地劫副本层数
        /// </summary>
        /// <param name="user"></param>
        public static int CheckUserPlotLayerNum(GameUser user)
        {
            int layerNum = 0;
            PlotInfo plotInfo = CheckUserPlotKalpa(user);
            if (user != null && plotInfo != null)
            {
                //最高层
                if (user.UserExtend != null)
                {
                    //修改：当前层大于最大层数时更新
                    if (user.UserExtend.MaxLayerNum < plotInfo.LayerNum)
                    {
                        user.UserExtend.UpdateNotify(obj =>
                        {
                            user.UserExtend.MaxLayerNum = plotInfo.LayerNum;
                            return true;
                        });
                        //user.Update();
                    }
                }
                //当前层
                if (user.UserExtend != null && user.UserExtend.LayerNum > 0)
                {
                    UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
                    if (dailyRestrain != null && dailyRestrain.UserExtend != null && dailyRestrain.UserExtend.LastKalpaNum > 0)
                    {
                        layerNum = user.UserExtend.LayerNum;
                    }
                    else
                    {
                        layerNum = plotInfo.LayerNum;
                        user.UserExtend.UpdateNotify(obj =>
                            {
                                user.UserExtend.LayerNum = layerNum;
                                return true;
                            });
                        //user.Update();
                    }
                }
                else
                {
                    layerNum = plotInfo.LayerNum;
                    if (user.UserExtend == null)
                    {
                        user.UserExtend = new GameUserExtend();
                    }
                    user.UserExtend.UpdateNotify(obj =>
                        {
                            user.UserExtend.LayerNum = layerNum;
                            return true;
                        });
                    //user.Update();
                }
            }
            else
            {
                layerNum = 1;
            }
            return layerNum;
        }

        /// <summary>
        /// 天地劫副本关数
        /// </summary>
        /// <param name="user"></param>
        public static int CheckUserPlotHurdleNum(GameUser user)
        {
            int hurdleNum = 0;
            PlotInfo plotInfo = CheckUserPlotKalpa(user);
            if (plotInfo != null)
            {
                UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
                if (dailyRestrain != null && dailyRestrain.UserExtend != null && dailyRestrain.UserExtend.KalpaDate.Date == DateTime.Now.Date)
                {
                    hurdleNum = user.UserExtend.HurdleNum;
                }
                else
                {
                    hurdleNum = plotInfo.PlotSeqNo;

                    if (user.UserExtend != null && hurdleNum > 0)
                    {
                        user.UserExtend.UpdateNotify(obj =>
                            {
                                user.UserExtend.HurdleNum = hurdleNum;
                                return true;
                            });
                        //user.Update();
                    }
                }
            }
            else
            {
                hurdleNum = 1;
            }
            return hurdleNum;
        }
    }
}
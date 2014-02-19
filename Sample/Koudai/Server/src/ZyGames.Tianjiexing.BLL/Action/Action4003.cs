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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 4003_副本通关后显示评价接口
    /// </summary>
    public class Action4003 : BaseAction
    {
        private int PlotID;
        private UserPlotInfo userPlot;
        private ItemBaseInfo itemInfo;
        private List<UserEmbattle> embattleList = new List<UserEmbattle>();
        private UniversalInfo[] universalArray = new UniversalInfo[0];
        //玩家通过一个城市的所有副本时，增加聊天频道系统提示
        private string plotName = string.Empty;
        private int maxHonourNum = 0;
        private int lastMaxHonourNum = 0;

        public Action4003(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4003, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(userPlot == null ? (short)0 : userPlot.AttackScore);
            PushIntoStack(userPlot == null ? (short)0 : userPlot.DefenseScore);
            PushIntoStack(userPlot == null ? (short)0 : userPlot.ScoreNum);
            PushIntoStack(userPlot == null ? (short)0 : userPlot.StarScore);
            PushIntoStack(userPlot == null ? 0 : userPlot.Experience);
            PushIntoStack(userPlot == null ? 0 : userPlot.ExpNum);
            PushIntoStack(userPlot == null ? 0 : userPlot.PennyNum);
            PushIntoStack(userPlot == null ? 0 : userPlot.GoldNum);
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID);
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName);
            PushIntoStack(itemInfo == null ? (short)0 : (short)itemInfo.QualityType);

            this.PushIntoStack(embattleList.Count);
            foreach (UserEmbattle item in embattleList)
            {
                UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(item.UserID, item.GeneralID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(general != null ? general.GeneralName.ToNotNullString() : string.Empty);

                PushIntoStack(dsItem);
            }
            PushIntoStack(userPlot == null ? 0 : userPlot.BlessPennyNum);
            this.PushIntoStack(universalArray.Length);
            foreach (var item in universalArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.Name.ToNotNullString());
                dsItem.PushIntoStack(item.HeadID.ToNotNullString());
                dsItem.PushIntoStack(item.Num);
                dsItem.PushIntoStack(item.ItemID);
                dsItem.PushIntoStack(item.MaxHeadID.ToNotNullString());
                dsItem.PushIntoStack(item.ItemDesc.ToNotNullString());
                this.PushIntoStack(dsItem);
            }
            PushIntoStack(userPlot == null ? 0 : userPlot.HonourNum);
            PushIntoStack(userPlot == null ? 0 : userPlot.PlotSuccessType.ToInt());
            PushIntoStack(userPlot == null ? 0 : userPlot.PlotFailureType.ToInt());
            PushIntoStack(plotName);
            PushIntoStack(maxHonourNum);
            PushIntoStack(ContextUser.HonourNum);
            PushIntoStack(lastMaxHonourNum);
            PushIntoStack(ContextUser.IsLv ? 1: 0);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotID", ref PlotID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(PlotID);
            if (plotInfo != null)
            {
                plotName = plotInfo.PlotName;
            }
            var cacheSetGeneralEscalate = new ConfigCacheSet<GeneralEscalateInfo>();
            GeneralEscalateHelper.AddUserLv(ContextUser, 0);
            int lv = ContextUser.UserLv;
            lv = lv < 0 ? 1 : lv + 1;
            var generalEscalate =
                cacheSetGeneralEscalate.Find(s => s.GeneralType == GeneralType.YongHu && s.GeneralLv == lv);
            if (generalEscalate != null)
            {
                maxHonourNum = generalEscalate.UpExperience;
            }
            var lastGeneralEscalate = cacheSetGeneralEscalate.Find(s => s.GeneralType == GeneralType.YongHu && s.GeneralLv == (lv-1));
            if (lastGeneralEscalate!=null)
            {
                lastMaxHonourNum = lastGeneralEscalate.UpExperience;
            }

            embattleList = new GameDataCacheSet<UserEmbattle>().FindAll(ContextUser.UserID, m => m.MagicID == ContextUser.UseMagicID);
            embattleList.QuickSort((x, y) => x.GeneralID.CompareTo(y.GeneralID));
            List<UniversalInfo> universalList = new List<UniversalInfo>();
            userPlot = UserPlotHelper.GetUserPlotInfo(ContextUser.UserID, PlotID);
            //var cacheSetPlotInfo = new ConfigCacheSet<PlotInfo>();
            //var cacheSetItem = new ConfigCacheSet<ItemBaseInfo>();
            //var plotInfo = cacheSetPlotInfo.FindKey(PlotID);

            if (userPlot != null && userPlot.ItemList.Count > 0)
            {
                universalList.AddRange(userPlot.ItemList);
                //if (plotInfo != null && userPlot.PlotStatus == PlotStatus.Completed && plotInfo.ItemRank != "" && plotInfo.ItemRank != null)
                //{
                //    var itemArray = plotInfo.ItemRank.Split(',');
                //    foreach (var s in itemArray)
                //    {
                //        int itemId = s.Split('=')[0].ToInt();
                //        var item = cacheSetItem.FindKey(itemId);
                //        if (item != null)
                //        {
                //            universalList.Add(new UniversalInfo() { Name = item.ItemName, HeadID = item.HeadID, Num = s.Split('=')[1].ToInt() });
                //        }
                //    }

                //}
                //if (userPlot.ItemID > 0)
                //{
                //    itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userPlot.ItemID);
                //    if (itemInfo != null)
                //    {
                //        universalList.Add(new UniversalInfo() { Name = itemInfo.ItemName, HeadID = itemInfo.HeadID, Num = 1 });
                //    }
                //}
                //if (userPlot.EnchantID > 0)
                //{
                //    enchant = new ConfigCacheSet<EnchantInfo>().FindKey(userPlot.EnchantID);

                //    if (enchant != null)
                //    {
                //        universalList.Add(new UniversalInfo() { Name = enchant.EnchantName, HeadID = enchant.HeadID, Num = 1 });
                //    }
                //}
                userPlot.ItemList.Clear();
            }
            universalArray = universalList.ToArray();

           
            return true;
        }
    }
}
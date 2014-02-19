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


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 副本配置信息下发接口
    /// </summary>
    public class Action4011 : BaseStruct
    {
        private int ClientVersion = 0;
        private List<PlotInfo> plotList = new List<PlotInfo>();

        public Action4011(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4011, httpGet)
        {
        }

        public override bool TakeAction()
        {
           // int currVersion = new ConfigCacheSet<ConfigVersion>().FindKey(VersionType.Plot).CurVersion;
            plotList = new ConfigCacheSet<PlotInfo>().FindAll();
            return true;
        }

        public override void BuildPacket()
        {
            this.PushIntoStack(plotList.Count);
            foreach (PlotInfo plotInfo in plotList)
            {
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(plotInfo.PlotID);
                ds.PushIntoStack(plotInfo.CityID);
                ds.PushIntoStack(plotInfo.PlotType.ToShort());
                ds.PushIntoStack(plotInfo.PlotSeqNo);
                ds.PushIntoStack(plotInfo.PlotName.ToNotNullString());
                ds.PushIntoStack(plotInfo.BossHeadID.ToNotNullString());
                ds.PushIntoStack(plotInfo.BgScene.ToNotNullString());
                ds.PushIntoStack(plotInfo.FgScene.ToNotNullString());
                ds.PushIntoStack(plotInfo.AftPlotID);
                ds.PushIntoStack(plotInfo.SceneY1);
                ds.PushIntoStack(plotInfo.SceneY2);

                var npcList = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == plotInfo.PlotID);
                ds.PushIntoStack(npcList.Count);
                foreach (PlotNPCInfo npcInfo in npcList)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(npcInfo.PlotNpcID);
                    dsItem.PushIntoStack(npcInfo.NpcName.ToNotNullString());
                    dsItem.PushIntoStack(npcInfo.HeadID.ToNotNullString());
                    dsItem.PushIntoStack(npcInfo.NpcSeqNo.ToInt());
                    dsItem.PushIntoStack(npcInfo.PointX);
                    dsItem.PushIntoStack(npcInfo.PointY);
                    dsItem.PushIntoStack(npcInfo.NpcTip.ToNotNullString());
                    dsItem.PushIntoStack(npcInfo.PreStoryCode.ToNotNullString());
                    dsItem.PushIntoStack(npcInfo.AftStoryCode.ToNotNullString());
                    dsItem.PushIntoStack(npcInfo.IsBoss.ToShort());

                    var plotEmbattleList = new ConfigCacheSet<PlotEmbattleInfo>().FindAll(m => m.PlotNpcID == npcInfo.PlotNpcID);
                    dsItem.PushIntoStack(plotEmbattleList.Count);
                    foreach (PlotEmbattleInfo embattleInfo in plotEmbattleList)
                    {
                        MonsterInfo monster = new ConfigCacheSet<MonsterInfo>().FindKey(embattleInfo.MonsterID);
                        if (monster == null)
                        {
                            SaveLog(new Exception(string.Format(LanguageManager.GetLang().St4011_NoMonster, plotInfo.PlotID, embattleInfo.MonsterID)));
                        }
                        DataStruct dsItem1 = new DataStruct();
                        dsItem1.PushIntoStack(embattleInfo.MonsterID);
                        dsItem1.PushIntoStack(embattleInfo.GridSeqNo.ToInt());
                        dsItem1.PushIntoStack(monster == null ? (short)0 : monster.MonsterType.ToShort());
                        dsItem1.PushIntoStack(monster == null ? string.Empty : monster.HeadID.ToNotNullString());
                        dsItem1.PushIntoStack(monster == null ? string.Empty : monster.GeneralName.ToNotNullString());
                        dsItem1.PushIntoStack(monster == null ? (short)0 : monster.GeneralLv.ToShort());
                        dsItem1.PushIntoStack(monster == null ? (int)0 : monster.LifeNum);
                        dsItem1.PushIntoStack(monster == null ? (int)0 : monster.MomentumNum);
                        dsItem1.PushIntoStack(monster == null ? (short)0 : monster.CareerID);
                        dsItem1.PushIntoStack(monster == null ? (int)0 : monster.ItemID);

                        dsItem.PushIntoStack(dsItem1);
                    }


                    ds.PushIntoStack(dsItem);
                }
                this.PushIntoStack(ds);
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
    }
}
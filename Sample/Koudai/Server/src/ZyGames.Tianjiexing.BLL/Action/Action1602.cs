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
    /// 1602_装备、丹药合成材料详情接口
    /// </summary>
    public class Action1602 : BaseAction
    {
        private int itemID = 0;
        private ItemBaseInfo itemInfo = null;
        private int plotID = 0; //未完成
        private string plotName = string.Empty;

        public Action1602(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1602, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
            PushIntoStack(plotID);
            PushIntoStack(plotName);
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemDesc.ToNotNullString());

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ItemID", ref itemID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            List<MonsterInfo> monsterArray = new ConfigCacheSet<MonsterInfo>().FindAll(m => m.ItemID == itemID);
            if (monsterArray.Count > 0)
            {
                PlotNPCInfo npcInfo = new ConfigCacheSet<PlotNPCInfo>().FindKey(monsterArray[0].MonsterID);
                if (npcInfo != null)
                {
                    PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
                    if (plotInfo != null)
                    {
                        plotID = plotInfo.PlotID;
                        plotName = plotInfo.PlotName;
                    }
                }
            }
            return true;
        }
    }
}
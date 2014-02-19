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
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1701_队列列表接口
    /// </summary>
    public class Action1701 : BaseAction
    {
        private int equCount = 0;
        private int currPlantNum;
        private int maxPlantNum;
        private int currExploreNum;
        private int maxExploreNum;
        private int equMaxNum = 0;
        private int _magicMaxNum = 0;
        private int coldTime = 0;
        private int equOpenCount = 0;

        private short typeID;
        private List<UserQueue> queueArray = new List<UserQueue>();

        public Action1701(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1701, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(equCount);
            PushIntoStack(currPlantNum);
            PushIntoStack(maxPlantNum);
            PushIntoStack(currExploreNum);
            PushIntoStack(maxExploreNum);
            PushIntoStack(queueArray.Count);
            foreach (UserQueue queue in queueArray)
            {
                int totalColdeTime = queue.DoRefresh();
                if (queue.QueueType == QueueType.EquipmentStrong)
                {
                    coldTime = queue.DoRefresh();
                    if (queue.StrengNum >= equMaxNum && totalColdeTime > 0)
                    {
                        typeID = 1;
                    }
                    else
                    {
                        typeID = 0;
                    }
                }


                if (queue.QueueType == QueueType.MagicStrong)
                {
                    coldTime = queue.DoRefresh();
                    if (queue.StrengNum >= _magicMaxNum && totalColdeTime > 0)
                    {
                        typeID = 1;
                    }
                    else
                    {
                        typeID = 0;
                    }
                }

                if (queue.QueueType == QueueType.TianXianStrong)
                {
                    if (totalColdeTime > 0)
                    {
                        coldTime = totalColdeTime;
                        typeID = 1;
                    }
                    else
                    {
                        coldTime = 0;
                        typeID = 0;
                    }
                }


                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(queue.QueueID);
                dsItem.PushIntoStack((short)queue.QueueType);
                dsItem.PushIntoStack(coldTime);
                dsItem.PushIntoStack(typeID);
                PushIntoStack(dsItem);
            }
            PushIntoStack(equOpenCount);
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
            equMaxNum = ConfigEnvSet.GetInt("UserQueue.EquStrengMaxNum");
            _magicMaxNum = ConfigEnvSet.GetInt("UserQueue.MagicStrengMaxNum");

            equCount = ContextUser.QueueNum;
            List<UserLand> landArray = new GameDataCacheSet<UserLand>().FindAll(ContextUser.UserID, u => u.IsGain == 1 || u.DoRefresh() > 0);
            currPlantNum = landArray.Count;
            UserPlant plant = new GameDataCacheSet<UserPlant>().FindKey(ContextUser.UserID);
            if (plant != null)
            {
                maxPlantNum = plant.LandNum;
            }
            else
            {
                maxPlantNum = 0;
            }

            UserExpedition expedition = new GameDataCacheSet<UserExpedition>().FindKey(ContextUser.UserID);
            if (expedition != null && DateTime.Now.Date == expedition.InsertDate.Date)
            {
                currExploreNum = expedition.ExpeditionNum;
            }
            else
            {
                currExploreNum = 0;
            }

            UserFunction function = new GameDataCacheSet<UserFunction>().FindKey(ContextUser.UserID, FunctionEnum.Meiritanxian);
            if (function != null)
            {
                maxExploreNum = 10;
            }
            else
            {
                maxExploreNum = 0;
            }
            var cacheSet = new GameDataCacheSet<UserQueue>();
            List<UserQueue> uQueueArray = cacheSet.FindAll(ContextUser.UserID, m => m.QueueType == QueueType.EquipmentStrong);
            if (uQueueArray.Count > 3)
            {
                for (int i = 0; i < uQueueArray.Count - 3; i++)
                {
                    cacheSet.Delete(uQueueArray[i]);
                }
            }
            queueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, u => u.QueueType == QueueType.EquipmentStrong || u.QueueType == QueueType.MagicStrong || u.QueueType == QueueType.TianXianStrong);
            queueArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return y.Timing.CompareTo(x.Timing);
            });
            equOpenCount = EquStrongQueue(ContextUser.UserID);
            return true;
        }

        public static int EquStrongQueue(string userID)
        {
            int equNum = 0;
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            UserFunction userFunction = new GameDataCacheSet<UserFunction>().FindKey(userID, FunctionEnum.Qianghuaqueue);
            if (user != null && userFunction != null && user.QueueNum <= 1)
            {
                equNum = MathUtils.Addition(equNum, 1, int.MaxValue);
            }

            if (user != null)
            {
                if (VipHelper.GetVipOpenFun(user.VipLv, ExpandType.KaiQiQiangHuaDuiLie) && user.QueueNum <= 2)
                {
                    equNum = MathUtils.Addition(equNum, 1, int.MaxValue);
                }
            }
            return equNum;
        }
    }
}
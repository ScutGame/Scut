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
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 10001_庄园土地列表接口
    /// </summary>
    public class Action10001 : BaseAction
    {
        private short isShow = 0;
        private UserPlant userPlant = null;
        private List<UserLand> userLandArray = new List<UserLand>();

        public Action10001(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action10001, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(userPlant == null ? 0 : userPlant.DewNum);
            this.PushIntoStack(isShow);
            this.PushIntoStack(userPlant == null ? 0 : userPlant.LandNum);
            this.PushIntoStack(userLandArray.Count);
            foreach (UserLand land in userLandArray)
            {
                short isOpen = 1;
                int codetime = land.DoRefresh();
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(land.LandPositon);
                dsItem.PushIntoStack(isOpen);
                dsItem.PushIntoStack(land.IsGain);
                dsItem.PushIntoStack(land.IsRedLand);
                dsItem.PushIntoStack(land.IsBlackLand);
                dsItem.PushIntoStack(codetime);
                dsItem.PushIntoStack(land.GeneralID);
                dsItem.PushIntoStack((short)land.PlantType);

                this.PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            var cacheSet = new GameDataCacheSet<UserPlant>();
            UserPlant plant = cacheSet.FindKey(ContextUser.UserID);
            var landCacheSet = new GameDataCacheSet<UserLand>();
            List<UserLand> landArray = landCacheSet.FindAll(ContextUser.UserID);
            if (plant == null)
            {
                UserPlant uPlant = new UserPlant()
                {
                    UserID = ContextUser.UserID,
                    LandNum = 1,
                    DewNum = ConfigEnvSet.GetInt("UserQueue.ShengShuiMaxNum"),
                    PayDewTime = 0,
                };
                cacheSet.Add(uPlant);
            }
            if (landArray.Count == 0)
            {
                UserLand userLand = new UserLand()
                {
                    UserID = ContextUser.UserID,
                    PlantType = PlantType.Experience,
                    LandPositon = 1,
                    IsRedLand = 2,
                    IsBlackLand = 2,
                    IsGain = 2
                };
                landCacheSet.Add(userLand);
            }

            if (new GameDataCacheSet<UserFunction>().FindKey(ContextUser.UserID, FunctionEnum.Zhongzhijingqianshu) == null)
            {
                isShow = 2;
            }
            else
            {
                isShow = 1;
            }
            userPlant = new GameDataCacheSet<UserPlant>().FindKey(ContextUser.UserID);
            userLandArray = new GameDataCacheSet<UserLand>().FindAll(ContextUser.UserID);

            //圣水恢复
            var queueCacheSet = new GameDataCacheSet<UserQueue>();
            List<UserQueue> ShengShuiQueueArray = queueCacheSet.FindAll(ContextUser.UserID, m => m.QueueType == QueueType.ShengShuiHuiFu);

            if (ShengShuiQueueArray.Count > 0)
            {
                UserQueue shengShuiQueue = ShengShuiQueueArray[0];
                if (userPlant != null)
                {
                    int shengshuiMaxNum = ConfigEnvSet.GetInt("UserQueue.ShengShuiMaxNum");
                    int restorationDate = ConfigEnvSet.GetInt("UserQueue.ShengShuiRestorationDate"); //三小时
                    int restorationNum = ConfigEnvSet.GetInt("UserQueue.ShengShuiRestorationNum"); //恢复1点
                    int timeCount = (int)(DateTime.Now - shengShuiQueue.Timing).TotalSeconds / restorationDate;

                    if (userPlant.DewNum < shengshuiMaxNum && timeCount > 0)
                    {
                        userPlant.DewNum = MathUtils.Addition(userPlant.DewNum, (timeCount * restorationNum), shengshuiMaxNum);
                        //userPlant.Update();

                        shengShuiQueue.Timing = DateTime.Now;
                        //shengShuiQueue.Update();
                    }
                }
            }
            else
            {
                UserQueue queue = new UserQueue()
                {
                    QueueID = Guid.NewGuid().ToString(),
                    UserID = ContextUser.UserID,
                    QueueType = QueueType.ShengShuiHuiFu,
                    QueueName = QueueType.ShengShuiHuiFu.ToString(),
                    Timing = DateTime.Now,
                    ColdTime = 0,
                    TotalColdTime = 0,
                    IsSuspend = false,
                    StrengNum = 0
                };
                queueCacheSet.Add(queue);
            }

            return true;
        }
    }
}
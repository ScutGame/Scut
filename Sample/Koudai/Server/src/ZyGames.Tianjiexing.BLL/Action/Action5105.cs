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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 5105_消除竞技冷却时间接口
    /// </summary>
    public class Action5105 : BaseAction
    {
        private int ops = 0;


        public Action5105(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5105, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var cacheSet = new GameDataCacheSet<UserQueue>();
            List<UserQueue> userQueueArray = cacheSet.FindAll(ContextUser.UserID, m => m.QueueType == QueueType.JingJiTiaoZhan);
            if (userQueueArray.Count > 0)
            {
                UserQueue userQueue = userQueueArray[0];
                int queueColdTime = userQueue.DoRefresh();
                queueColdTime = queueColdTime < 0 ? 0 : queueColdTime;
                if (ops == 1)
                {
                    this.ErrorCode = ops;
                    this.ErrorInfo = GetPrice().ToString();
                    return false;
                }
                else if (ops == 2)
                {
                    if (ContextUser.GoldNum >= GetPrice())
                    {
                        this.ErrorCode = ops;
                        ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, GetPrice(), int.MaxValue);
                        //ContextUser.Update();
                        cacheSet.Delete(userQueue);
                    }
                    else
                    {

                        this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                }
                else
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
            }
            return true;
        }

        private int GetPrice()
        {
            int currGoldNum = 0;
            List<UserQueue> userQueueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.JingJiTiaoZhan);
            if (userQueueArray.Count > 0)
            {
                UserQueue queue = userQueueArray[0];
                if (queue != null)
                {
                    int queueColdTime = (queue.DoRefresh() / 60);
                    if (queueColdTime != 0)
                    {
                        currGoldNum = MathUtils.Addition(queueColdTime, 1, 10);
                    }
                }
            }
            return currGoldNum;
        }
    }
}
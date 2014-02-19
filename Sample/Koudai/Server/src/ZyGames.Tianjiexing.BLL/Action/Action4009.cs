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
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 4009_扫荡详情接口
    /// </summary>
    public class Action4009 : BaseAction
    {
        private int plotID;
        private List<UserSweepPool> userSweepPoolList = new List<UserSweepPool>();
        private List<UserEmbattle> userEmbattleList = new List<UserEmbattle>();


        public Action4009(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4009, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(userSweepPoolList.Count);
            foreach (UserSweepPool sweepPool in userSweepPoolList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(sweepPool.TurnsNum);
                dsItem.PushIntoStack(sweepPool.BattleNum);
                dsItem.PushIntoStack(sweepPool.GameCoin);
                dsItem.PushIntoStack(sweepPool.Gold);
                dsItem.PushIntoStack(sweepPool.ExpNum);
                dsItem.PushIntoStack(userEmbattleList.Count);
                foreach (UserEmbattle userEmbattle in userEmbattleList)
                {
                    UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, userEmbattle.GeneralID);
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(userGeneral.GeneralName.ToNotNullString());
                    dsItem1.PushIntoStack(sweepPool.Experience);
                    dsItem1.PushIntoStack(sweepPool.BlessExperience);

                    dsItem.PushIntoStack(dsItem1);
                }

                dsItem.PushIntoStack(sweepPool.PrizeItems.Count);
                foreach (PrizeItemInfo prizeItemInfo in sweepPool.PrizeItems)
                {
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prizeItemInfo.ItemID);
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(itemInfo != null ? itemInfo.ItemName.ToNotNullString() : string.Empty);
                    dsItem1.PushIntoStack(prizeItemInfo.Num);
                    dsItem1.PushIntoStack(itemInfo != null ? (short)itemInfo.QualityType : (short)0);

                    dsItem.PushIntoStack(dsItem1);
                }

                dsItem.PushIntoStack(sweepPool.BlessPennyNum);
                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotID", ref plotID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            //刷新副本奖励
            if (!PlotHelper.RefleshPrize(ContextUser.UserID, plotID))
            {
                //this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                //this.ErrorInfo = LanguageManager.GetLang().St4007_SaodangOver;
            }
            var sweepCacheSet = new GameDataCacheSet<UserSweepPool>();
            var tempSweepPoolList = sweepCacheSet.FindAll(ContextUser.UserID, m => m.IsSend == false);
            tempSweepPoolList.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                int result = 0;
                x.UserID = x.UserID == null ? "" : x.UserID;
                y.UserID = y.UserID == null ? "" : y.UserID;

                result = x.UserID.CompareTo(y.UserID);
                if (result == 0)
                {
                    if (x.TurnsNum < y.TurnsNum)
                    {
                        return -1;
                    }
                    else if (x.TurnsNum > y.TurnsNum)
                    {
                        return 1;
                    }
                    else
                    {
                        if (x.BattleNum < y.BattleNum)
                        {
                            return -1;
                        }
                        else if (x.BattleNum > y.BattleNum)
                        {
                            return 1;
                        }
                    }
                }
                return result;
            });


            userEmbattleList = new GameDataCacheSet<UserEmbattle>().FindAll(ContextUser.UserID, m => m.GeneralID > 0 && m.MagicID == ContextUser.UseMagicID);

            foreach (UserSweepPool userSweepPool in tempSweepPoolList)
            {
                if (userSweepPool != null)
                {
                    sweepCacheSet.Delete(userSweepPool);
                }
            }

            var tempList = new List<UserSweepPool>(tempSweepPoolList);
            int pageSize = 12;
            int index = 0;
            if (tempList.Count > pageSize)
            {
                index = tempList.Count - pageSize;
            }
            else
            {
                pageSize = tempList.Count;
            }
            if (tempList.Count > 0)
            {
                userSweepPoolList = tempList.GetRange(index, pageSize);
            }

            return true;
        }
    }
}
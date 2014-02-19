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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using System;

namespace ZyGames.Tianjiexing.BLL.Action
{

    ///<summary>
    ///9003_充值礼包列表接口
    ///</summary>
    public class Action9003 : BaseAction
    {
        private List<RechargePacks> _chargePacksArray = new List<RechargePacks>();
        private static int festivalID = 1012;

        public Action9003(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9003, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_chargePacksArray.Count);
            foreach (RechargePacks packs in _chargePacksArray)
            {
                short _isRevice = 0;
                short _isShow = 0;
                List<PackageReceive> receivesArray = new GameDataCacheSet<PackageReceive>().FindAll(ContextUser.UserID, u => u.IsReceive == false && u.PacksID == packs.PacksID);
                if (receivesArray.Count == 0)
                {
                    _isRevice = 2;
                }
                else
                {
                    _isRevice = 1;
                }
                SaveLog(receivesArray.Count + ContextUser.UserID + packs.PacksID);

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(packs.PacksID);
                dsItem.PushIntoStack(packs.PacksName);
                dsItem.PushIntoStack(_isRevice);
                dsItem.PushIntoStack(_isShow);
                dsItem.PushIntoStack(receivesArray.Count);
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
            //判断充值礼包是否开启
            FestivalInfo festivalInfo = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festivalInfo != null && !festivalInfo.IsStop) return false;

            RemovePack(ContextUser.UserID);

            List<RechargePacks> packsesList = new ConfigCacheSet<RechargePacks>().FindAll();
            foreach (RechargePacks packse in packsesList)
            {
                if (packse.PacksType == 1 || packse.PacksType == 2)
                {
                    List<PackageReceive> receivesArray = new GameDataCacheSet<PackageReceive>().FindAll(ContextUser.UserID, m => m.PacksID == packse.PacksID);
                    if (receivesArray.Count == 0 ||
                        (receivesArray.Count > 0 && !receivesArray[0].IsReceive))
                    {
                        _chargePacksArray.Add(packse);
                    }
                }
                else
                {
                    _chargePacksArray.Add(packse);
                }
            }
            return true;
        }

        /// <summary>
        ///  //删除不是本周，本月已领取的数据
        /// </summary>
        /// <param name="userID"></param>
        public static void RemovePack(string userID)
        {
            var cacheSet = new GameDataCacheSet<PackageReceive>();
            List<PackageReceive> packageReceiveArray = cacheSet.FindAll(userID, u => u.PacksType == 3 || u.PacksType == 4);
            foreach (PackageReceive packageReceive in packageReceiveArray)
            {
                if (!PaymentService.IsCurrentWeek(packageReceive.ReceiveDate) && packageReceive.PacksType == 3)
                {
                    cacheSet.Delete(packageReceive);
                }
                if (packageReceive.ReceiveDate.Month != DateTime.Now.Month && packageReceive.PacksType == 4)
                {
                    cacheSet.Delete(packageReceive);
                }
            }
        }
    }
}
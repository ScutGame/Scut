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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9005_充值礼包领取接口
    /// </summary>
    public class Action9005 : BaseAction
    {
        private int _packsID;
        private static int festivalID = 1012;


        public Action9005(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9005, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PacksID", ref _packsID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            //判断充值礼包是否开启
            FestivalInfo festivalInfo = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festivalInfo != null && !festivalInfo.IsStop) return false;

            UserRecharge recharges = new GameDataCacheSet<UserRecharge>().FindKey(ContextUser.UserID);
            RechargePacks rePacksInfo = new ConfigCacheSet<RechargePacks>().FindKey(_packsID);
            List<PackageReward> rewardArray = new List<PackageReward>();
            if (rePacksInfo != null)
            {
                rewardArray = rePacksInfo.Reward.ToList();
                var userItemArray = UserItemHelper.GetItems(Uid).FindAll(m => m.ItemStatus == ItemStatus.BeiBao);
                int subPackNum = MathUtils.Subtraction(ContextUser.GridNum, userItemArray.Count, 0);
                if (rewardArray.Count > subPackNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1606_GridNumNotEnough;
                    return false;
                }
            }
            else
            {
                return false;
            }
            if (!isRecevies(ContextUser.UserID, _packsID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St9003_AlreadyReceived;
                return false;
            }
            List<PackageReceive> receiveArray = new GameDataCacheSet<PackageReceive>().FindAll(ContextUser.UserID, m => m.PacksID == _packsID);

            bool isRece = false;
            PackageReceive pReceive = null;
            foreach (PackageReceive receive in receiveArray)
            {
                if (!receive.IsReceive)
                {
                    isRece = true;
                    receive.IsReceive = true;
                    pReceive = receive;
                    break;
                }
            }
            if (pReceive == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St9003_AlreadyReceived;
                return false;
            }


            pReceive.IsReceive = true;
            pReceive.ReceiveDate = DateTime.Now;
            //pReceive.Update();


            if (pReceive.PacksType == 1 && recharges != null && isRece)
            {
                int totalPay = (int)(recharges.FirstNum * rePacksInfo.ProportionNum);
                if (totalPay >= 1)
                {
                    ContextUser.ItemGold = MathUtils.Addition(ContextUser.ItemGold, totalPay, int.MaxValue);
                    //ContextUser.Update();
                }
                //ContextUser.PayGold = ContextUser.PayGold.Addition(recharges.FirstNum, int.MaxValue);

                foreach (PackageReward reward in rewardArray)
                {
                    UserItemHelper.AddUserItem(ContextUser.UserID, reward.Num, 1);
                }
            }
            else if (recharges != null && isRece)
            {
                foreach (PackageReward reward in rewardArray)
                {

                    if (reward.Type == 2)
                    {
                        ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, reward.Num, int.MaxValue);
                    }
                    else if (reward.Type == 3)
                    {
                        ContextUser.ExpNum = MathUtils.Addition(ContextUser.ExpNum, reward.Num, int.MaxValue);
                    }
                    else if (reward.Type == 4)
                    {
                        ContextUser.ObtainNum = MathUtils.Addition(ContextUser.ObtainNum, reward.Num, int.MaxValue);
                    }
                    else if (reward.Type == 5)
                    {
                        ContextUser.EnergyNum = MathUtils.Addition(ContextUser.EnergyNum, reward.Num.ToShort(), short.MaxValue);
                    }
                    else if (reward.Type == 6)
                    {
                        GameUserExtend extend = ContextUser.UserExtend;
                        extend.GainBlessing = MathUtils.Addition(extend.GainBlessing, reward.Num, int.MaxValue);

                        //List<GuildMember> memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.UserID ==  ContextUser.UserID);
                        //if (memberArray.Length > 0)
                        //{
                        //    memberArray[0].GainBlessing = memberArray[0].GainBlessing.Addition(reward.Num, int.MaxValue);
                        //    memberArray[0].Update();
                        //}
                    }
                    //ContextUser.Update();

                }
            }

            PackageReceiveLog receiveLog = new PackageReceiveLog()
              {
                  LogID = Guid.NewGuid().ToString(),
                  ReceiveID = receiveArray[0].ReceiveID,
                  PacksID = _packsID,
                  UserID = ContextUser.UserID,
                  ReceiveDate = DateTime.Now,
              };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(receiveLog);
            return true;
        }

        public static bool isRecevies(string userID, int packID)
        {
            bool result = false;
            List<PackageReceive> receivesArray = new GameDataCacheSet<PackageReceive>().FindAll(userID, m => m.PacksID == packID);
            foreach (PackageReceive package in receivesArray)
            {
                if (!package.IsReceive)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
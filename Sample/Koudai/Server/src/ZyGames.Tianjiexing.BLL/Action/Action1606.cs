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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Cache.Generic;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1606_新手礼包使用接口
    /// </summary>
    public class Action1606 : BaseAction
    {
        private string userItemID = string.Empty;
        private int hasNextGift = 0;
        private string content = "";

        public Action1606(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1606, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(hasNextGift);
            PushIntoStack(content);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserItemID", ref userItemID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            ItemUseHelper itemuse = new ItemUseHelper();
            itemuse.UseItem(userItemID, Uid);
            if (itemuse.result && itemuse.content != string.Empty)
            {
                content = itemuse.content.Trim(',');
            }
            if (!itemuse.result)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
            }

            // 使用还可以继续使用该礼包
            int itemid = UserItemHelper.GetUserItemInfoID(ContextUser.UserID, userItemID);
            bool hasGift = UserItemHelper.IsEnoughBeiBaoItem(ContextUser.UserID, itemid, 1);
            if (hasGift)
            {
                hasNextGift = 1;
            }
            return true;
        }

        /// <summary>
        /// 领取神秘礼包
        /// </summary>
        /// <returns></returns>
        public static void MysteriousSpree(UserItemInfo item, GameUser user)
        {
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
            if (itemInfo != null)
            {
                var prizeInfosArray = itemInfo.ItemPack;
                int randomNum = RandomUtils.GetRandom(0, prizeInfosArray.Count);
                PrizeInfo prizeInfo = prizeInfosArray[randomNum];
                ActivitiesAward.GameUserReward(user.UserID, prizeInfo.Type, prizeInfo.ItemID, prizeInfo.Num);
            }
        }


        /// <summary>
        /// 节日活动奖励
        /// </summary>
        /// <param name="user"></param>
        /// <param name="festivalID"></param>
        /// <param name="itemID"></param>
        public static void UserFestivalReward(GameUser user, int festivalID, int itemID)
        {
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                var rewardsArray = festival.Reward;
                foreach (PrizeInfo reward in rewardsArray)
                {
                    ActivitiesAward.GameUserRewardNocite(user.UserID, reward.Type, reward.ItemID, reward.Num, reward.CrystalType);
                }
            }
        }

        /// <summary>
        /// 新手礼包
        /// </summary>
        /// <param name="user"></param>
        /// <param name="item"></param>
        /// <param name="noviceActivities"></param>
        /// <returns></returns>
        public static bool GetNoviceActivities(GameUser user, UserItemInfo item, NoviceActivities noviceActivities)
        {
            List<NoviceReward> noviceArray = noviceActivities.Reward.ToList();
            var package = UserItemPackage.Get(user.UserID);
            var userItemArray = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus == ItemStatus.BeiBao);
            int subPackNum = MathUtils.Subtraction(user.GridNum, MathUtils.Subtraction(userItemArray.Count, 5, 0), 0);
            if (noviceArray.Count > subPackNum)
            {
                return false;
            }

            bool isupdate = false;

            foreach (NoviceReward reward in noviceArray)
            {
                if (reward.Type == 1)
                {
                    isupdate = true;
                    user.EnergyNum = MathUtils.Addition(user.EnergyNum, (short)reward.Num, short.MaxValue);
                }
                if (reward.Type == 2)
                {
                    isupdate = true;
                    user.GameCoin = MathUtils.Addition(user.GameCoin, reward.Num, int.MaxValue);
                }
                else if (reward.Type == 3)
                {
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(reward.Reward);
                    UserGeneral userGeneral = UserGeneral.GetMainGeneral(user.UserID);
                    if (itemInfo.ItemType == ItemType.ZhuangBei && !itemInfo.CheckCareer(userGeneral.CareerID))
                    {
                        continue;
                    }
                    UserItemHelper.AddUserItem(user.UserID, itemInfo.ItemID, reward.Num);
                }
                else if (reward.Type == 4)
                {
                    //晶石
                    isupdate = true;
                    user.GiftGold = MathUtils.Addition(user.GiftGold, reward.Num, int.MaxValue);
                }
            }
            UserItemHelper.UseUserItem(user.UserID, item.ItemID, 1);

            if (isupdate)
            {
                //user.Update();
            }

            if (item.ItemID == 5000 || item.ItemID == 5001 ||
                item.ItemID == 5002 || item.ItemID == 5003)
            {
                int itemID = MathUtils.Addition(item.ItemID, 1, int.MaxValue);
                UserItemHelper.AddUserItem(user.UserID, itemID, 1);
            }
            return true;
        }

        /// <summary>
        /// 随机获得金币、精力奖励
        /// </summary>
        /// <param name="item"></param>
        /// <param name="user"></param>
        public static void GetProbabilityReward(GameUser user, UserItemInfo item)
        {
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
            if (itemInfo != null)
            {
                var prizeInfosArray = itemInfo.ItemPack;
                double[] probability = new double[prizeInfosArray.Count];
                for (int i = 0; i < prizeInfosArray.Count; i++)
                {
                    probability[i] = (double)prizeInfosArray[i].Probability;
                }
                int index2 = RandomUtils.GetHitIndex(probability);
                PrizeInfo prizeInfo = prizeInfosArray[index2];
                ActivitiesAward.GameUserReward(user.UserID, prizeInfo.Type, prizeInfo.ItemID, prizeInfo.Num);
                UserItemHelper.UseUserItem(user.UserID, item.ItemID, 1);
            }
        }
    }
}
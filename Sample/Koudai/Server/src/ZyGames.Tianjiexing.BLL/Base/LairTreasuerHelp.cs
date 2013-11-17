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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{

    public partial class LairTreasuerHelp
    {
        public static int ChestLairTreasuerPosition(LairTreasureType lairTreasureType)       //��Ѩȡ����õ�λ��
        {
            int postion = 0;
            var lairTreasuer = new ConfigCacheSet<LairTreasureInfo>().FindKey(MathUtils.ToInt(lairTreasureType));
            double[] LairDoubleList = new double[lairTreasuer.LairTreasureList.Count];
            for (int i = 0; i < lairTreasuer.LairTreasureList.Count; i++)
            {
                LairDoubleList[i] = (double)lairTreasuer.LairTreasureList[i].Probability;
            }
            postion = (int)RandomUtils.GetHitIndex(LairDoubleList);
            return postion;
        }
        public static ItemBaseInfo ShowLairReward(LairTreasure lairTreasure, GameUser gameUser, LairTreasureType lairTreasureType)    //��ʾ��õ���Ʒ
        {
            var cacheSetItem = new ConfigCacheSet<ItemBaseInfo>();
            ItemBaseInfo itemBaseInfo = null;
            var lairRewardList = new ConfigCacheSet<LairRewardInfo>().FindAll(s => s.LairPosition == lairTreasure.LairPosition && s.LairTreasureType == lairTreasureType.ToInt());
            if (lairRewardList.Count >0)
            {
                var lairRewardInfo = lairRewardList[RandomUtils.GetRandom(0, lairRewardList.Count)];
                itemBaseInfo = cacheSetItem.FindKey(lairRewardInfo.ItemID);
                if (itemBaseInfo != null)
                {
                    UserItemHelper.AddUserItem(gameUser.UserID, lairRewardInfo.ItemID, lairTreasure.Num);
                }
            }

            return itemBaseInfo;
        }

        public static void GetLaiReward(LairTreasureType lairTreasureType, GameUser gameUser, int id, int postion)       //��ȡ��Ʒ
        {
            var lairTreasuerList = new ConfigCacheSet<LairTreasureInfo>().FindKey(MathUtils.ToInt(lairTreasureType));
            LairTreasure lairTreasure = lairTreasuerList.LairTreasureList[postion];
            var lairRewardInfo = new ConfigCacheSet<LairRewardInfo>().FindKey(id);
            var rewardTye = lairRewardInfo.LairRewardType.ToEnum<LairRewardType>();
            switch (rewardTye)
            {
                case LairRewardType.GameGoin:
                    gameUser.GameCoin = MathUtils.Addition(gameUser.GameCoin, lairTreasure.Num);
                    break;
                case LairRewardType.Gold:
                    gameUser.PayGold = MathUtils.Addition(gameUser.PayGold, lairTreasure.Num);
                    break;
                case LairRewardType.FunJi:
                case LairRewardType.WuPing:
                    UserItemHelper.AddUserItem(gameUser.UserID, lairRewardInfo.ItemID, 1);
                    break;

            }
        }

        public static int GetItemID(LairRewardType lairRewardType)         //��ȡ�������ж��ֽ����������ȡһ��
        {
            var rewardTypeAll = new ConfigCacheSet<LairRewardInfo>().FindAll(u => u.LairRewardType == lairRewardType.ToInt());
            var i = RandomUtils.GetRandom(0, rewardTypeAll.Count);
            var itemID = rewardTypeAll[i].ItemID;
            return itemID;
        }

        public static int GetLaiRewardIndex(List<LairTreasure> lairTreasureList )
        {
            int count = lairTreasureList.Count;
            int[] precent = new int[count];
            int i = 0;
            foreach (var lair in lairTreasureList)
            {
                precent[i] = (lair.Probability*1000).ToInt();
                i++;
            }
            return RandomUtils.GetHitIndexByTH(precent);
        }
    }

}
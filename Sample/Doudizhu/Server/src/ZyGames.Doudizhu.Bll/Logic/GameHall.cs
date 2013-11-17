using System;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;

namespace ZyGames.Doudizhu.Bll.Logic
{
    /// <summary>
    /// 游戏大厅
    /// </summary>
    public class GameHall
    {
        private GameUser _user;

        /// <summary>
        /// 游戏大厅
        /// </summary>
        /// <param name="user"></param>
        public GameHall(GameUser user)
        {
            _user = user;
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="headIcon"></param>
        public bool ChangeUserHead(string headIcon)
        {
            if (_user == null) return false;
            _user.HeadIcon = headIcon;
            var cacheSet = new GameDataCacheSet<GameUser>();
            cacheSet.Update();
            return true;
        }

        /// <summary>
        /// 玩家当前元宝
        /// </summary>
        public int UserGold
        {
            get
            {
                if (_user == null) return 0;
                int gold = _user.PayGold + _user.GiftGold + _user.ExtGold - _user.UseGold;
                return gold.IsValid() ? gold : 0;
            }
        }
        /// <summary>
        /// 获取称号
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        {
            if (_user == null) return "";
            var title = new ConfigCacheSet<TitleInfo>().FindKey(_user.TitleId);
            return title != null ? title.Name : "";
        }

        /// <summary>
        /// 胜率
        /// </summary>
        /// <returns></returns>
        public int GetWinRate()
        {
            if (_user == null) return 0;
            int totalNum = _user.WinNum + _user.FailNum;
            if (totalNum == 0) return 0;
            decimal val = MathUtils.RoundCustom((decimal)_user.WinNum / totalNum, 2);
            return (int)(val * 100);
        }

        /// <summary>
        /// 背包
        /// </summary>
        /// <returns></returns>
        public UserItemPackage Get()
        {
            var cacheSet = new GameDataCacheSet<UserItemPackage>();
            var data = cacheSet.FindKey(_user.UserId.ToString());
            if (data != null)
            {
                return data;
            }
            TraceLog.WriteError(string.Format("User:{0} item package is null.", _user.UserId));
            return null;
        }

        /// <summary>
        /// 检查是否有当前物品
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public bool CheckUserItem(int itemID)
        {
            var package = Get();
            if (package != null)
            {
                var useritem = package.ItemPackage.FindAll(s => s.ItemID == itemID);
                if (useritem.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 增加物品
        /// </summary>
        /// <param name="itemID"></param>
        public void AddUserItem(int itemID, ShopType shopType)
        {
            AddUserItem(itemID, 1, shopType);
        }

        public void PutPackage(int itemID, int num = 1)
        {
            AddUserItem(itemID, num, ShopType.HeadID);
        }
        /// <summary>
        /// 增加物品
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="num"></param>
        public void AddUserItem(int itemID, int num, ShopType shopType)
        {
            UserItem item = new UserItem();
            item.UserItemID = Guid.NewGuid().ToString();
            item.ItemID = itemID;
            item.Num = num;
            item.ShopType = shopType;

            var package = Get();
            if (package != null)
            {
                package.ItemPackage.Add(item);
            }
        }

        /// <summary>
        /// 转盘抽奖获得物品与位置
        /// </summary>
        /// <returns></returns>
        public short DialPrizePostion()
        {
            int postion = 0;
            var dialList = new ConfigCacheSet<DialInfo>().FindAll();
            double[] dialDoubleList = new double[dialList.Count];
            for (int i = 0; i < dialList.Count; i++)
            {
                dialDoubleList[i] = (double)dialList[i].Probability;
            }
            postion = RandomUtils.GetHitIndex(dialDoubleList);
            return postion.ToShort();
        }
    }
}

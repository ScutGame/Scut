using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Doudizhu.Bll.Logic;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Generic;
using ZyGames.Framework.Game.Pay;

namespace ZyGames.Doudizhu.Bll.Com.Share
{
    class DdzPaymentNotify : PaymentNotify
    {
        protected override bool DoNotify(int userId, OrderInfo orderInfo)
        {
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userId.ToString());
            if (userInfo == null) return false;
            userInfo.PayGold = MathUtils.Addition(userInfo.PayGold, orderInfo.GameCoins);
            AchieveTask.SaveUserTask(userId.ToString(), TaskClass.ChongZhi, orderInfo.GameCoins);
            return true;
        }
    }
}

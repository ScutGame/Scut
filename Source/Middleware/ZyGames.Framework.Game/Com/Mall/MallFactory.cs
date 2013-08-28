using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Caching;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Mall
{
    /// <summary>
    /// 商城工厂
    /// </summary>
    public static class MallFactory
    {
        private static Dictionary<int, Merchant> _merchantList = new Dictionary<int, Merchant>();

        public static void RegisterMerchant(int merchantId, MallController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }
            Merchant merchant = new Merchant(merchantId, controller);
            merchant.InitializeGoods();
            if (!_merchantList.ContainsKey(merchantId))
            {
                _merchantList.Add(merchantId, merchant);
            }
        }

        public static Merchant GetMerchant(int merchantId)
        {
            return _merchantList.ContainsKey(merchantId) ? _merchantList[merchantId] : null;
        }

        public static void Dispose()
        {
            _merchantList.Clear();
            _merchantList = null;
        }
    }
}

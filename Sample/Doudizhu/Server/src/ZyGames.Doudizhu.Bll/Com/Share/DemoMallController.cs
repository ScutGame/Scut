using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Com.Mall;
using ZyGames.Framework.Game.Com.Model;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Doudizhu.Bll.Com.Share
{
    internal class MyClass
    {
        private const int SystemMerId = 1000;
        static void InitMall()
        {
            MallFactory.RegisterMerchant(SystemMerId, new DemoMallController());

            Merchant merchant = MallFactory.GetMerchant(SystemMerId);
            int pageIndex = 1;
            int pageSize = 10;
            int pageCount;
            var goodList = merchant.ShowGoods(m => true, pageIndex, pageSize, out pageCount);

            merchant.BuyGoods(user, goodsId, goodsNum,)
        }
    }
    class DemoMallController : MallController
    {
        public DemoMallController()
            : base(new SingleTrade())
        {
        }

        public override IList<GoodsData> LoadGoodsData(int merchantId)
        {
            //加载商品数据
        }

        protected override object ProduceGoods(GoodsData goods, int goodsNum)
        {
            //创建物品对象
        }

        protected override bool HasTraded(BaseUser user, GoodsData goods, int goodsNum, out double amount, out TradeErrorCode errorCode)
        {
            //检查是否可交易
        }

        protected override bool PayAmount(BaseUser user, int tradeType, double payAmount)
        {
            //扣除玩家代币
        }

        protected override bool SendGoods(BaseUser user, object items)
        {
            //交易成功，向玩家背包放入物品
        }
    }
}

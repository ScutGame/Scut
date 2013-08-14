using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Pay.Section;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract.Action
{
    public class AppStoreDetailAction : BaseStruct
    {
        private List<AppStorePayElement> appList = new List<AppStorePayElement>();
        private MobileType MobileType = 0;

        public AppStoreDetailAction(short aActionId, HttpGet httpGet)
            : base(aActionId, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(appList.Count);
            foreach (var item in appList)
            {
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(item.Dollar.ToString());
                ds.PushIntoStack(item.ProductId);
                ds.PushIntoStack(item.SilverPiece);
                PushIntoStack(ds);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("MobileType", ref MobileType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var paySection = AppStoreFactory.GetPaySection();
            var rates = paySection.Rates;

            foreach (AppStorePayElement rate in rates)
            {
                if (rate.MobileType == MobileType.Normal || rate.MobileType == MobileType)
                {
                    appList.Add(rate);
                }
            }
            return true;
        }
    }
}

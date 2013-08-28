using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Pay.Section
{
    public class AppStorePayElement : ConfigurationElement
    {
        [ConfigurationProperty("productId", IsRequired = true, IsKey = true)]
        public string ProductId
        {
            get { return this["productId"] as string; }
            set { this["productId"] = value; }
        }

        [ConfigurationProperty("mobileType")]
        public MobileType MobileType
        {
            get { return this["mobileType"].ToEnum<MobileType>(); }
            set { this["mobileType"] = value; }
        }

        [ConfigurationProperty("dollar", IsRequired = true)]
        public decimal Dollar
        {
            get { return this["dollar"].ToDecimal(); }
            set { this["dollar"] = value; }
        }

        [ConfigurationProperty("currency", IsRequired = true)]
        public int Currency
        {
            get { return this["currency"].ToInt(); }
            set { this["currency"] = value; }
        }

        [ConfigurationProperty("silverPiece", IsRequired = true)]
        public int SilverPiece
        {
            get { return this["silverPiece"].ToInt(); }
            set { this["silverPiece"] = value; }
        }
    }
}

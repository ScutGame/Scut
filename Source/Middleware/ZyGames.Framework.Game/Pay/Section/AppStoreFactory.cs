using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Pay.Section
{
    public static class AppStoreFactory
    {
        public static AppStorePaySection GetPaySection()
        {
            return GetPaySection("appStorePay");
        }

        private static AppStorePaySection GetPaySection(string name)
        {
            return ConfigurationManager.GetSection(name) as AppStorePaySection;
        }
    }
}

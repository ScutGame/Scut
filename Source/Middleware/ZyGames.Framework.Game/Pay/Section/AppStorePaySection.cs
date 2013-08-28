using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Pay.Section
{
    /// <summary>
    /// APP STORE 充值配置
    /// </summary>
    public class AppStorePaySection : ConfigurationSection
    {

        //[ConfigurationProperty("", IsDefaultCollection = true)]
        //public ChannelUCCollection Channels
        //{
        //    get { return (ChannelUCCollection)this[""]; }
        //}

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public AppStorePayCollection Rates
        {
            get { return this[""] as AppStorePayCollection; }
        }
    }
}

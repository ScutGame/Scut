using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Game.Sns.Section
{
    public class Section10086Element : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return this["url"] as string; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("version", IsRequired = true)]
        public string Version
        {
            get { return this["version"] as string; }
            set { this["version"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public int OrderType
        {
            get { return this["type"].ToInt(); }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public Channel10086Collection Channels
        {
            get { return (Channel10086Collection)this[""]; }
        }

    }
}

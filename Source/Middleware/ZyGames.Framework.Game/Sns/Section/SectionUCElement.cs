using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    public class SectionUCElement : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return this["url"] as string; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("service", IsRequired = true)]
        public string Service
        {
            get { return this["service"] as string; }
            set { this["service"] = value; }
        }

        [ConfigurationProperty("channelId", IsRequired = false)]
        public string ChannelId
        {
            get { return this["channelId"] as string; }
            set { this["channelId"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ChannelUCCollection Channels
        {
            get { return (ChannelUCCollection)this[""]; }
        }

    }
}

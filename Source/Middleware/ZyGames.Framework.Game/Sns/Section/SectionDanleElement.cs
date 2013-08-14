using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    public class SectionDanleElement : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = false)]
        public string Url
        {
            get { return this["url"] as string; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("version", IsRequired = false)]
        public string Version
        {
            get { return this["version"] as string; }
            set { this["version"] = value; }
        }

        public bool IsOldVersion
        {
            get { return Equals("0.1", Version); }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ChannelDanleCollection Channels
        {
            get { return (ChannelDanleCollection)this[""]; }
        }

    }
}

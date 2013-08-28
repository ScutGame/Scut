using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    public class Section91Element : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = false)]
        public string Url
        {
            get { return this["url"] as string; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public Channel91Collection Channels
        {
            get { return (Channel91Collection)this[""]; }
        }

    }
}

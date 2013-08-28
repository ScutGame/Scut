using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Game.Sns.Section
{
    public class Section360Element : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return this["url"] as string; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("aceess_token_url", IsRequired = true)]
        public string AceessTokenUrl
        {
            get { return this["aceess_token_url"] as string; }
            set { this["aceess_token_url"] = value; }
        }

        [ConfigurationProperty("get_aceess_token_url", IsRequired = true)]
        public string GetAceessTokenUrl
        {
            get { return this["get_aceess_token_url"] as string; }
            set { this["get_aceess_token_url"] = value; }
        }
        //[ConfigurationProperty("version", IsRequired = true)]
        //public string Version
        //{
        //    get { return this["version"] as string; }
        //    set { this["version"] = value; }
        //}

        //[ConfigurationProperty("type", IsRequired = true)]
        //public int OrderType
        //{
        //    get { return this["type"].ToInt(); }
        //    set { this["type"] = value; }
        //}

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public Channel360Collection Channels
        {
            get { return (Channel360Collection)this[""]; }
        }

    }
}

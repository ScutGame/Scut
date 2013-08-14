using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    public class Channel360Element : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("appId", IsRequired = false)]
        public string AppId
        {
            get { return this["appId"] as string; }
            set { this["appId"] = value; }
        }

        [ConfigurationProperty("appKey", IsRequired = false)]
        public string AppKey
        {
            get { return this["appKey"] as string; }
            set { this["appKey"] = value; }
        }

        [ConfigurationProperty("appSecret", IsRequired = false)]
        public string AppSecret
        {
            get { return this["appSecret"] as string; }
            set { this["appSecret"] = value; }
        }

    }
}

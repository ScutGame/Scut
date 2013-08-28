using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    public class ChannelUCElement : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("cpId", IsRequired = false)]
        public string CpId
        {
            get { return this["cpId"] as string; }
            set { this["cpId"] = value; }
        }

        [ConfigurationProperty("apiKey", IsRequired = false)]
        public string ApiKey
        {
            get { return this["apiKey"] as string; }
            set { this["apiKey"] = value; }
        }

        [ConfigurationProperty("gameId", IsRequired = false)]
        public string GameId
        {
            get { return this["gameId"] as string; }
            set { this["gameId"] = value; }
        }

        [ConfigurationProperty("serverId", IsRequired = false)]
        public string ServerId
        {
            get { return this["serverId"] as string; }
            set { this["serverId"] = value; }
        }

    }
}

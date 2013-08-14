using System.Configuration;

namespace ZyGames.Framework.Game.Command
{
    public class CommandElement : ConfigurationElement
    {
        [ConfigurationProperty("cmd", IsRequired = true)]
        public string Cmd
        {
            get { return this["cmd"] as string; }
            set { this["cmd"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeName
        {
            get { return this["type"] as string; }
            set { this["type"] = value; }
        }

    }
}
